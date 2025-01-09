using log4net;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.Entity;
using System.Collections.Generic;

namespace DominoDatabase
{
    public static class DBExtension
    {
        #region 전역변수 설정
        /// <summary>
        /// Log4Net 생성
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        public static bool ExecuteQuery(this DbContext dbContext, string _qryString)
        {
            try
            {
                dbContext.Database.ExecuteSqlCommand(_qryString);
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("Exception : {0}", ex.InnerException ?? ex);

                return false;
            }
        }

        public static bool ExecuteQuery(this DbContext dbContext, List<string> _qryStrings)
        {
            try
            {
                foreach (string szQry in _qryStrings)
                {
                    dbContext.ExecuteQuery(szQry);
                }
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("Exception : {0}", ex.InnerException ?? ex);
                return false;
            }
        }

        public static List<T> Select<T>(this DbContext dbContext, string _szQry) where T : class
        {
            //DLog.Write("DB", _szQry);
            return dbContext.Database.SqlQuery<T>(_szQry).ToList();
        }

        public static List<T> Select<T>(this DbContext dbContext, string EntityName, string szWhere) where T : class
        {
            string szQry = string.Format("SELECT * FROM {0} WHERE {1}", EntityName, szWhere);
            //DLog.Write("DB", szQry);
            return dbContext.Select<T>(szQry);
        }

        public static bool Insert<T>(this DbContext dbContext, string EntityName, T val) where T : class
        {
            string InsQry = string.Empty;
            try
            {
                StringBuilder sbProp = new StringBuilder();
                StringBuilder sbPropValue = new StringBuilder();
                PropertyInfo[] Props = val.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);
                foreach (PropertyInfo prop in Props)
                {
                    if (prop.GetValue(val, null) == null)
                        continue;

                    if (prop.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute), false).FirstOrDefault() != null)
                        continue;

                    sbProp.AppendFormat(",{0}", prop.Name);

                    if (prop.PropertyType == typeof(string))
                        sbPropValue.AppendFormat(",'{0}'", prop.GetValue(val, null));
                    else if (prop.PropertyType == typeof(DateTime?) || prop.PropertyType == typeof(DateTime))
                        sbPropValue.AppendFormat(",'{0}'", Convert.ToDateTime(prop.GetValue(val, null)).ToString("yyyy-MM-dd HH:mm:ss"));
                    else
                        sbPropValue.AppendFormat(",{0}", prop.GetValue(val, null));
                }
                InsQry = string.Format("INSERT INTO {0}({1}) VALUES({2})", EntityName, sbProp.ToString().Substring(1), sbPropValue.ToString().Substring(1));
                //DLog.Write("DB", InsQry);
                return dbContext.ExecuteQuery(InsQry);
            }
            catch (Exception ex)
            {
                log.InfoFormat("Exception Query {0} : {1}", InsQry, ex.InnerException ?? ex);
                return false;
            }
        }

        public static bool Delete(this DbContext dbContext, string szEntityName, string szWhere)
        {
            string szQry = string.Format("DELETE FROM {0} WHERE {1}", szEntityName, szWhere);
            //DLog.Write("DB", szQry);
            return dbContext.ExecuteQuery(szQry);
        }

        public static bool Update(this DbContext dbContext, string szEntityName, string szSet, string szWhere)
        {
            string szQry = string.Format("UPDATE {0} SET {1} WHERE {2}", szEntityName, szSet, szWhere);
            //DLog.Write("DB", szQry);
            return dbContext.ExecuteQuery(szQry);
        }

        public static bool HasDBTable(this DbContext dbContext, string _EntityName)
        {
            try
            {
                return dbContext.Database.SqlQuery<string>(string.Format("select name from sysobjects where id=object_ID('{0}')", _EntityName)).ToList().Count > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool BackupDatabase(this DbContext dbContext, string bakFileName)
        {
            try
            {
                string DbName = dbContext.Database.Connection.Database;
                dbContext.Database.CommandTimeout = 600;
                //string bakFileName = string.Format("{0}PM_{1}.bak", BakDBPath, DateTime.Now.ToString("yyyyMMddHHmmss"));
                string sqlCommand = @"BACKUP DATABASE [{0}] TO  DISK = N'{1}' WITH NOFORMAT, NOINIT,  NAME = N'{0}-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";
                string _qryString = string.Format(sqlCommand, DbName, bakFileName);
                dbContext.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, _qryString);

                log.DebugFormat("DataBase Backup Execute Query : {0}", _qryString);
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("Exception : {0}", ex.InnerException ?? ex);
                return false;
            }
        }

        public static bool BackupLocalDB(string path)
        {
            return BackupDatabase(new DominoDBLocal(), path);
        }

        public static bool BackupServerDB(string path)
        {
            return BackupDatabase(new DominoDBServer(), path);
        }
        public static void AddOrUpdate(this List<ReadBarcode> list, ReadBarcode target)
        {
            var exist = list.FirstOrDefault(q => q.ProdStdCode.Equals(target.ProdStdCode) && q.SerialNum.Equals(target.SerialNum));
            if (exist == null)
                list.Add(target);
            else
                exist = target;
        }
        public static void AddOrUpdate(this List<SerialPool> list, SerialPool target)
        {
            var exist = list.FirstOrDefault(q => q.ProdStdCode.Equals(target.ProdStdCode) && q.SerialNum.Equals(target.SerialNum));
            if (exist == null)
                list.Add(target);
            else
                exist = target;
        }

        public static bool HasDBObject(this DbContext dbContext, string _EntityName)
        {
            try
            {
                return dbContext.Database.SqlQuery<string>(string.Format("select name from sysobjects where id=object_ID('{0}')", _EntityName)).ToList().Count > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static List<string> GetQryArray(string fileName)
        {
            try
            {
                List<string> lsQry = new List<string>();

                string[] szQrys = ReadAllLines(fileName);
                StringBuilder szEachQry = new StringBuilder();
                foreach (string szQry in szQrys)
                {
                    if (string.Compare(szQry, "GO") == 0)
                    {
                        if (string.IsNullOrEmpty(szEachQry.ToString().Trim()))
                            continue;
                        lsQry.Add(szEachQry.ToString());
                        szEachQry.Clear();
                    }
                    else
                    {
                        szEachQry.AppendLine(szQry);
                    }
                }
                return lsQry;
            }
            catch (Exception ex)
            {
                log.InfoFormat("Exception : {0}", ex);
            }
            return null;
        }
        public static string[] ReadAllLines(string fileName)
        {
            try
            {
                return File.ReadAllLines(fileName);
            }
            catch (Exception)
            {
                return null;
            }
        }

        //public static List<string> GetQryArray(string fileName)
        //{
        //    try
        //    {
        //        List<string> lsQry = new List<string>();

        //        string[] szQrys = IOExtension.ReadAllLines(fileName);
        //        StringBuilder szEachQry = new StringBuilder();
        //        foreach(string szQry in szQrys)
        //        {
        //            if(string.Compare(szQry, "GO") == 0)
        //            {
        //                if(string.IsNullOrEmpty(szEachQry.ToString().Trim()))
        //                    continue;
        //                lsQry.Add(szEachQry.ToString());
        //                szEachQry.Clear();
        //            }
        //            else
        //            {
        //                szEachQry.AppendLine(szQry);
        //            }
        //        }
        //        return lsQry;
        //    }
        //    catch(Exception ex)
        //    {
        //        log.InfoFormat("Exception : {0}", ex);
        //    }
        //    return null;
        //}
    }
}
