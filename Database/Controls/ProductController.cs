using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using System.Data.Entity.Migrations;
using DominoDatabase.DSM;
using System.Text;
using System.Data.SqlClient;
using DominoFunctions.Enums;

namespace DominoDatabase.Controls
{
    public class ProductController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static long TotalPageCount { get; set; }
        public static long TotalCount { get; set; }
        public static List<Product> SelectLocalPM(string productCode, string standardCode, string productName, string productName2, bool? use, bool? machineUseYN, int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            List<Product> retList = new List<Product>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_Product_M
                               join b in db.Dmn_Product_PM on a.ProdCode equals b.ProdCode into bb
                               from b in bb.DefaultIfEmpty()
                               join c in db.Dmn_Serial_Expression on b.SnExpressionID equals c.SnExpressionID into cc
                               from c in cc.DefaultIfEmpty()
                               join d in db.Dmn_CustomBarcodeFormat on b.BarcodeDataFormat equals d.CustomBarcodeFormatID into dd
                               from d in dd.DefaultIfEmpty()
                               select new { a, b, c, d };
                    if (!string.IsNullOrEmpty(productCode))
                        list = list.Where(q => q.a.ProdCode.Contains(productCode));
                    if (!string.IsNullOrEmpty(standardCode))
                        list = list.Where(q => q.a.ProdStdCode.Contains(standardCode));
                    if (!string.IsNullOrEmpty(productName))
                        list = list.Where(q => q.a.ProdName.Contains(productName));
                    if (!string.IsNullOrEmpty(productName2))
                        list = list.Where(q => q.a.ProdName2.Contains(productName2));
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Y") : q.a.UseYN.Equals("N"));
                    if (machineUseYN != null)
                        list = list.Where(q => (bool)machineUseYN ? q.b.MachineUseYN == "Y" : q.b.MachineUseYN != "Y");
                    foreach (var item in list)
                    {
                        if (!retList.Any(x => x.ProdCode.Equals(item.a.ProdCode)))
                            retList.Add(new Product(item.a));
                        retList.Single(x => x.ProdCode.Equals(item.a.ProdCode)).ProductDetail.Add(new ProductDetail(item.b)
                        {
                            SnExpressionStr = item.c == null ? null : item.c.SnExpressionStr,
                            CustomBarcodeFormat = item.d == null ? null : item.d.CustomBarcodeFormatStr
                        });
                    }
                    if (pageIndex >= 0)
                    {
                        TotalCount = retList.Count();
                        TotalPageCount = Convert.ToInt64(Math.Ceiling(Convert.ToDouble(TotalCount) / Convert.ToDouble(pageSize)));
                        retList = retList.OrderBy(x => x.ProdCode.Length).ThenBy(x => x.ProdCode).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    }
                }
                return retList;
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController SelectLocalPM Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController SelectLocalPM Exception", ex);
            }
            return retList;
        }
        public static Product SelectLocalPMSingle(string productCode, string standardCode, string productName, bool? use, bool? machineUseYN, bool RaiseException = false)
        {
            List<Product> retList = new List<Product>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_Product_M
                               join b in db.Dmn_Product_PM on a.ProdCode equals b.ProdCode into bb
                               from b in bb.DefaultIfEmpty()
                               join c in db.Dmn_Serial_Expression on b.SnExpressionID equals c.SnExpressionID into cc
                               from c in cc.DefaultIfEmpty()
                               join d in db.Dmn_CustomBarcodeFormat on b.BarcodeDataFormat equals d.CustomBarcodeFormatID into dd
                               from d in dd.DefaultIfEmpty()
                               select new { a, b, c, d };
                    if (!string.IsNullOrEmpty(productCode))
                        list = list.Where(q => q.a.ProdCode.Equals(productCode));
                    if (!string.IsNullOrEmpty(standardCode))
                        list = list.Where(q => q.a.ProdStdCode.Contains(standardCode));
                    if (!string.IsNullOrEmpty(productName))
                        list = list.Where(q => q.a.ProdName.Contains(productName));
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Y") : q.a.UseYN.Equals("N"));
                    if (machineUseYN != null)
                        list = list.Where(q => (bool)machineUseYN ? q.b.MachineUseYN == "Y" : q.b.MachineUseYN != "Y");
                    foreach (var item in list)
                    {
                        if (!retList.Any(x => x.ProdCode.Equals(item.a.ProdCode)))
                            retList.Add(new Product(item.a));
                        retList.Single(x => x.ProdCode.Equals(item.a.ProdCode)).ProductDetail.Add(new ProductDetail(item.b)
                        {
                            SnExpressionStr = item.c == null ? null : item.c.SnExpressionStr,
                            CustomBarcodeFormat = item.d == null ? null : item.d.CustomBarcodeFormatStr
                        });
                    }
                }
                return retList.FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController SelectLocalSinglePM Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController SelectLocalSinglePM Exception", ex);
            }
            return retList.FirstOrDefault();
        }
        public static List<Product> SelectLocalAG(string productCode = "", string standardCode = "", string productName = "", bool? use = null, bool? machineUseYN = true, int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            List<Product> retList = new List<Product>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_Product_M
                               join b in db.Dmn_Product_AG on a.ProdCode equals b.ProdCode into bb
                               from b in bb.DefaultIfEmpty()
                               join c in db.Dmn_Serial_Expression on b.SnExpressionID equals c.SnExpressionID into cc
                               from c in cc.DefaultIfEmpty()
                               join d in db.Dmn_CustomBarcodeFormat on b.BarcodeDataFormat equals d.CustomBarcodeFormatID into dd
                               from d in dd.DefaultIfEmpty()
                               orderby b.JobDetailType
                               select new { a, b, c, d };
                    if (!string.IsNullOrEmpty(productCode))
                        list = list.Where(q => q.a.ProdCode.Contains(productCode));
                    if (!string.IsNullOrEmpty(standardCode))
                        list = list.Where(q => q.a.ProdStdCode.Contains(standardCode));
                    if (!string.IsNullOrEmpty(productName))
                        list = list.Where(q => q.a.ProdName.Contains(productName));
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Y") : q.a.UseYN.Equals("N"));
                    if (machineUseYN != null)
                        list = list.Where(q => (bool)machineUseYN ? q.b.MachineUseYN == "Y" : q.b.MachineUseYN != "Y");
                    foreach (var item in list)
                    {
                        if (!retList.Any(x => x.ProdCode.Equals(item.a.ProdCode)))
                            retList.Add(new Product(item.a));
                        retList.Single(x => x.ProdCode.Equals(item.a.ProdCode)).ProductDetail.Add(new ProductDetail(item.b)
                        {
                            SnExpressionStr = item.c == null ? null : item.c.SnExpressionStr,
                            CustomBarcodeFormat = item.d == null ? null : item.d.CustomBarcodeFormatStr
                        });
                    }
                    if (pageIndex >= 0)
                    {
                        TotalCount = retList.Count();
                        TotalPageCount = Convert.ToInt64(Math.Ceiling(Convert.ToDouble(TotalCount) / Convert.ToDouble(pageSize)));
                        retList = retList.OrderBy(x => x.ProdCode.Length).ThenBy(x => x.ProdCode).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    }
                }
                return retList;
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController SelectLocalAG Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController SelectLocalAG Exception", ex);
            }
            return retList;
        }
        public static Product SelectLocalAGSingle(string productCode = "", string standardCode = "", string prodName1 = "", bool? use = null, bool? machineUseYN = true, bool RaiseException = false)
        {
            List<Product> retList = new List<Product>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_Product_M
                               join b in db.Dmn_Product_AG on a.ProdCode equals b.ProdCode into bb
                               from b in bb.DefaultIfEmpty()
                               join c in db.Dmn_Serial_Expression on b.SnExpressionID equals c.SnExpressionID into cc
                               from c in cc.DefaultIfEmpty()
                               join d in db.Dmn_CustomBarcodeFormat on b.BarcodeDataFormat equals d.CustomBarcodeFormatID into dd
                               from d in dd.DefaultIfEmpty()
                               orderby b.JobDetailType
                               select new { a, b, c, d };
                    if (!string.IsNullOrEmpty(productCode))
                        list = list.Where(q => q.a.ProdCode.Equals(productCode));
                    if (!string.IsNullOrEmpty(standardCode))
                        list = list.Where(q => q.a.ProdStdCode.Contains(standardCode));
                    if (!string.IsNullOrEmpty(prodName1))
                        list = list.Where(q => q.a.ProdName.Contains(prodName1));
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Y") : q.a.UseYN.Equals("N"));
                    if (machineUseYN != null)
                        list = list.Where(q => (bool)machineUseYN ? q.b.MachineUseYN == "Y" : q.b.MachineUseYN != "Y");
                    foreach (var item in list)
                    {
                        if (!retList.Any(x => x.ProdCode.Equals(item.a.ProdCode)))
                            retList.Add(new Product(item.a));
                        retList.Single(x => x.ProdCode.Equals(item.a.ProdCode)).ProductDetail.Add(new ProductDetail(item.b)
                        {
                            SnExpressionStr = item.c == null ? null : item.c.SnExpressionStr,
                            CustomBarcodeFormat = item.d == null ? null : item.d.CustomBarcodeFormatStr
                        });
                    }
                }
                return retList.FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController SelectLocalSingleAG Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController SelectLocalSingleAG Exception", ex);
            }
            return retList.FirstOrDefault();
        }
        public static List<Local.Dmn_Product_M> SelectLocalMasterAll(bool RaiseException = false)
        {
            List<Local.Dmn_Product_M> retList = new List<Local.Dmn_Product_M>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_Product_M
                               select a;
                    retList = list.ToList();
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController SelectLocalMasterAll Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController SelectLocalMasterAll Exception", ex);
            }
            return retList;
        }
        public static List<Local.Dmn_Product_PM> SelectLocalPMAll(bool RaiseException = false)
        {
            List<Local.Dmn_Product_PM> retList = new List<Local.Dmn_Product_PM>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_Product_PM
                               select a;
                    retList = list.ToList();
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController SelectLocalPMAll Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController SelectLocalPMAll Exception", ex);
            }
            return retList;
        }
        public static List<Local.Dmn_Product_AG> SelectLocalAGAll(bool RaiseException = false)
        {
            List<Local.Dmn_Product_AG> retList = new List<Local.Dmn_Product_AG>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_Product_AG
                               select a;
                    retList = list.ToList();
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController SelectLocalAGAll Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController SelectLocalAGAll Exception", ex);
            }
            return retList;
        }
        public static bool InsertLocal(Local.Dmn_Product_M item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("ProductController InsertLocalMaster by {0}, {1}", item.InsertUser, item.ToString());
                    db.Dmn_Product_M.Add(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController InsertLocalMaster Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController InsertLocalMaster Exception", ex);
                return false;
            }
        }
        public static bool InsertLocal(Local.Dmn_Product_PM item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("ProductController InsertLocalPM by {0}, {1}", item.UpdateUser ?? item.InsertUser, item.ToString());
                    db.Dmn_Product_PM.Add(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController InsertLocalPM Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController InsertLocalPM Exception", ex);
                return false;
            }
        }
        public static bool AddOrUpdateLocal(Local.Dmn_Product_M item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("ProductController AddOrUpdateLocalMaster by {0}, {1}", item.InsertUser, item.ToString());
                    db.Dmn_Product_M.AddOrUpdate(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController AddOrUpdateLocalMaster Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController AddOrUpdateLocalMaster Exception", ex);
                return false;
            }
        }
        public static DateTime? GetUpdateDate(string prodCode)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    string sql = string.Format(@"SELECT UpdateDate
                                   FROM[DOMINO_DB].[dbo].[Dmn_Product_M]
                                   where ProdCode = '{0}'", prodCode);
                    return db.Database.SqlQuery<DateTime?>(sql).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController AddOrUpdateLocalMaster Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return null;
            }
        }
        public static bool AddOrUpdateLocal(Local.Dmn_Product_PM item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("ProductController AddOrUpdateLocalPM by {0}, {1}", item.UpdateUser ?? item.InsertUser, item.ToString());
                    db.Dmn_Product_PM.AddOrUpdate(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController AddOrUpdateLocalPM Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController AddOrUpdateLocalPM Exception", ex);
                return false;
            }
        }
        public static bool AddOrUpdateLocal(Local.Dmn_Product_AG item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("ProductController AddOrUpdateLocalAG by {0}, {1}", item.UpdateUser ?? item.InsertUser, item.ToString());
                    db.Dmn_Product_AG.AddOrUpdate(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController AddOrUpdateLocalAG Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController AddOrUpdateLocalAG Exception", ex);
                return false;
            }
        }
        public static bool Exist(string prodCode)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    var list = db.Select<Local.Dmn_SerialPool>(string.Format("SELECT * FROM Dmn_Product_M where ProdCode = '{0}'", prodCode));
                    return list.Count() != 0;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController InsertLocalAG Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public static bool InsertLocal(Local.Dmn_Product_AG item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("ProductController InsertLocalAG by {0}, {1}", item.UpdateUser ?? item.InsertUser, item.ToString());
                    db.Dmn_Product_AG.Add(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController InsertLocalAG Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController InsertLocalAG Exception", ex);
                return false;
            }
        }
        public static bool UpdateLocal(Local.Dmn_Product_M item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("ProductController UpdateLocalMaster by {0}. {1}", item.UpdateUser, item.ToString());
                    var tmp = db.Dmn_Product_M.First(q => q.ProdCode.Equals(item.ProdCode));
                    tmp.WriteExistings(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController UpdateLocalMaster Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController UpdateLocalMaster Exception", ex);
                return false;
            }
        }
        public static bool UpdateLocal(Local.Dmn_Product_PM item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("ProductController UpdateLocalPM by {0}, {1}", item.UpdateUser, item.ToString());
                    var tmp = db.Dmn_Product_PM.First(q => q.ProdCode.Equals(item.ProdCode));
                    tmp.WriteExistings(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController UpdateLocalPM Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController UpdateLocalPM Exception", ex);
                return false;
            }
        }
        public static string GetProductNameByCode(string code)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    string sql = string.Format(@"SELECT ProdName FROM [DOMINO_DB].[dbo].[Dmn_Product_M] where ProdCode = '{0}'", code);
                    return db.Database.SqlQuery<string>(sql).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController GetProductNameByCode Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return null;
            }
        }
        public static bool UpdateLocal(Local.Dmn_Product_AG item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("ProductController UpdateLocalAG by {0}, {1}", item.UpdateUser, item.ToString());
                    var tmp = db.Dmn_Product_AG.First(q => q.ProdCode.Equals(item.ProdCode));
                    tmp.WriteExistings(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController UpdateLocalAG Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController UpdateLocalAG Exception", ex);
                return false;
            }
        }
        public static bool DeleteLocalAll(string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("ProductController DeleteLocalAll by {0}", userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Product_PM");
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Product_M");
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Product_AG");
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController DeleteLocalAll Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController DeleteLocalAll Exception", ex);
                return false;
            }

        }
        public static bool DeleteLocalSingle(string prouductCode, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("ProductController DeleteLocalSingle {0} by {1}", prouductCode, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Product_PM where ProdCode = {0}", prouductCode);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Product_M where ProdCode = {0}", prouductCode);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Product_AG where ProdCode = {0}", prouductCode);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController DeleteLocalSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController DeleteLocalSingle Exception", ex);
                return false;
            }
        }
        public static bool DeleteLocalDetail(string detail, string jobDetailType, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("ProductController DeleteLocalDetail by {0}", userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Product_AG where ProdCode = {0} AND JobDetailType = {1}", detail, jobDetailType);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Product_PM where ProdCode = {0} AND JobDetailType = {1}", detail, jobDetailType);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController DeleteLocalDetail Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController DeleteLocalDetail Exception", ex);
                return false;
            }
        }
        public static DateTime? GetLastInsertDateLocal(bool RaiseException = false)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    return db.Dmn_Product_M.Select(q => q.InsertDate).Max();
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController GetLastInsertDateLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController GetLastInsertDateLocal Exception", ex);
            }
            return null;
        }
        public static DateTime? GetLastUpdateDateLocal(bool RaiseException = false)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    return db.Dmn_Product_M.Select(q => q.UpdateDate).Max();
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController GetLastUpdateDateLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController GetLastUpdateDateLocal Exception", ex);
            }
            return null;
        }
        public static List<Product> SelectServer(string plantCode, string lineID, string productCode, string standardCode, string productName, string jobDetailType, DateTime? insertDateStart, DateTime? insertDateEnd,
            DateTime? updateDateStart, DateTime? updateDateEnd, bool? use, string machineID, int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            List<Product> retList = new List<Product>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
				{
                    List<string> lineIDs = (from l in db.Dmn_Line_D where l.MachineID.Equals(machineID) select l.LineID).Distinct().ToList();

                    var list = from a in db.Dmn_Product_M
                               join b in db.Dmn_Product_D on new { a.PlantCode, a.ProdCode } equals new { b.PlantCode, b.ProdCode } into bb
                               from b in bb.DefaultIfEmpty()
                               join c in db.Dmn_Line_M on new { a.PlantCode, a.LineID } equals new { c.PlantCode, c.LineID } into cc
                               from c in cc.DefaultIfEmpty()
                               join d in db.Dmn_Line_D on new { b.PlantCode, b.JobDetailType, c.LineID } equals new { d.PlantCode, d.JobDetailType, d.LineID } into dd
                               from d in dd.DefaultIfEmpty()
                               join e in db.Dmn_Serial_Expression on new { a.PlantCode, b.SnExpressionID } equals new { e.PlantCode, e.SnExpressionID } into ee
                               from e in ee.DefaultIfEmpty()
                               orderby b.JobDetailType
                               select new { a, b, c, d, e };
                    if (!string.IsNullOrEmpty(plantCode))
                        list = list.Where(q => q.a.PlantCode.Equals(plantCode));
                    if (!string.IsNullOrEmpty(lineID))
                        list = list.Where(q => q.a.LineID.Equals(lineID));
					if (lineIDs.Count() > 0)
						list = list.Where(q => lineIDs.Contains(q.a.LineID));
					if (!string.IsNullOrEmpty(productCode))
                        list = list.Where(q => q.a.ProdCode.Contains(productCode));
                    if (!string.IsNullOrEmpty(standardCode))
                        list = list.Where(q => q.a.ProdStdCode.Contains(standardCode));
                    if (!string.IsNullOrEmpty(productName))
                        list = list.Where(q => q.a.ProdName.Contains(productName));
                    if (!string.IsNullOrEmpty(jobDetailType))
                        list = list.Where(q => q.b.JobDetailType.Contains(jobDetailType));
                    if (insertDateStart != null)
                        list = list.Where(q => q.a.InsertDate >= insertDateStart.Value);
                    if (insertDateEnd != null)
                        list = list.Where(q => q.a.InsertDate <= insertDateEnd.Value);
                    if (updateDateStart != null)
                        list = list.Where(q => q.a.UpdateDate != null && q.a.UpdateDate >= updateDateStart.Value);
                    if (updateDateEnd != null)
                        list = list.Where(q => q.a.UpdateDate != null && q.a.UpdateDate <= updateDateEnd.Value);
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Y") : q.a.UseYN.Equals("N"));
                    foreach (var item in list)
                    {
                        if (!retList.Any(x => x.ProdCode.Equals(item.a.ProdCode)))
                            retList.Add(new Product(item.a));
                        retList.Single(x => x.ProdCode.Equals(item.a.ProdCode)).ProductDetail.Add(new ProductDetail(item.b)
                        {
                            MachineUseYN = string.IsNullOrEmpty(machineID) || item.d == null || item.d.MachineID == null ? null : (item.d.MachineID.Equals(machineID) ? "Y" : "N"),
                            SnExpressionStr = item.e == null ? null : item.e.SnExpressionStr,
                        });
                    }

                    if (!string.IsNullOrWhiteSpace(machineID))
                    {
                        var LineChangedOrders =
                        (
                            from orderm in db.Dmn_JobOrder_M
                            join orderd in db.Dmn_JobOrder_D on new { orderm.PlantCode, orderm.OrderNo, orderm.SeqNo } equals new { orderd.PlantCode, orderd.OrderNo, orderd.SeqNo } into dmn_order_d
                            from orderd in dmn_order_d.DefaultIfEmpty()
                            join linem in db.Dmn_Line_M on new { orderm.PlantCode, orderm.LineID } equals new { linem.PlantCode, linem.LineID } into dmn_line_m
                            from linem in dmn_line_m.DefaultIfEmpty()
                            join lined in db.Dmn_Line_D on new { linem.PlantCode, orderd.JobDetailType, linem.LineID } equals new { lined.PlantCode, lined.JobDetailType, lined.LineID } into dmn_line_d
                            from lined in dmn_line_d.DefaultIfEmpty()
                            join prodm in db.Dmn_Product_M on new { orderm.PlantCode, orderm.ProdCode } equals new { prodm.PlantCode, prodm.ProdCode } into dmn_product_m
                            from prodm in dmn_product_m.DefaultIfEmpty()
                            join prodd in db.Dmn_Product_D on new { prodm.PlantCode, prodm.ProdCode, lined.JobDetailType } equals new { prodd.PlantCode, prodd.ProdCode, prodd.JobDetailType } into dmn_product_d
                            from prodd in dmn_product_d.DefaultIfEmpty()
                            join serexp in db.Dmn_Serial_Expression on new { prodd.PlantCode, prodd.SnExpressionID } equals new { serexp.PlantCode, serexp.SnExpressionID } into dmn_serial_expression
                            from serexp in dmn_serial_expression.DefaultIfEmpty()
                            orderby prodd.JobDetailType
                            where
                                (from b in db.Dmn_Line_D where b.MachineID == machineID select b.LineID).Contains(orderm.LineID)
                                && !(from c in db.Dmn_Product_M where c.ProdCode == orderm.ProdCode select c.LineID).Contains(orderm.LineID)
                            select new { a = prodm, b = prodd, c = linem, d = lined, e = serexp, f = orderm }
                        );

                        if (!string.IsNullOrEmpty(plantCode))
                            LineChangedOrders = LineChangedOrders.Where(q => q.a.PlantCode.Equals(plantCode));
                        if (!string.IsNullOrEmpty(lineID))
                            LineChangedOrders = LineChangedOrders.Where(q => q.a.LineID.Equals(lineID));
                        if (!string.IsNullOrEmpty(productCode))
                            LineChangedOrders = LineChangedOrders.Where(q => q.a.ProdCode.Contains(productCode));
                        if (!string.IsNullOrEmpty(standardCode))
                            LineChangedOrders = LineChangedOrders.Where(q => q.a.ProdStdCode.Contains(standardCode));
                        if (!string.IsNullOrEmpty(productName))
                            LineChangedOrders = LineChangedOrders.Where(q => q.a.ProdName.Contains(productName));
                        if (!string.IsNullOrEmpty(jobDetailType))
                            LineChangedOrders = LineChangedOrders.Where(q => q.b.JobDetailType.Contains(jobDetailType));
                        if (insertDateStart != null)
                            LineChangedOrders = LineChangedOrders.Where(q => q.f.InsertDate >= insertDateStart.Value);
                        if (insertDateEnd != null)
                            LineChangedOrders = LineChangedOrders.Where(q => q.f.InsertDate <= insertDateEnd.Value);
                        if (updateDateStart != null)
                            LineChangedOrders = LineChangedOrders.Where(q => q.f.UpdateDate != null && q.f.UpdateDate >= updateDateStart.Value);
                        if (updateDateEnd != null)
                            LineChangedOrders = LineChangedOrders.Where(q => q.f.UpdateDate != null && q.f.UpdateDate <= updateDateEnd.Value);
                        if (use != null)
                            LineChangedOrders = LineChangedOrders.Where(q => (bool)use ? q.f.UseYN.Equals("Y") : q.f.UseYN.Equals("N"));

                        foreach (var item in LineChangedOrders)
                        {
                            if (!retList.Any(x => x.ProdCode.Equals(item.a.ProdCode)))
                                retList.Add(new Product(item.a));
                            retList.Single(x => x.ProdCode.Equals(item.a.ProdCode)).ProductDetail.Add(new ProductDetail(item.b)
                            {
                                MachineUseYN = string.IsNullOrEmpty(machineID) || item.d == null || item.d.MachineID == null ? null : (item.d.MachineID.Equals(machineID) ? "Y" : "N"),
                                SnExpressionStr = item.e?.SnExpressionStr,
                            });
                        }
                    }

                    if (pageIndex >= 0)
                    {
                        TotalCount = retList.Count();
                        TotalPageCount = Convert.ToInt64(Math.Ceiling(Convert.ToDouble(TotalCount) / Convert.ToDouble(pageSize)));
                        retList = retList.OrderBy(x => x.ProdCode.Length).ThenBy(x => x.ProdCode).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    }
                }
                return retList;
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController SelectSever Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController SelectSever Exception", ex);
            }
            return retList;
        }
        public static Product SelectServerSingle(string plantCode, string lineID, string productCode, string standardCode, string productName, bool? use, string machineID, bool RaiseException = false)
        {
            List<Product> retList = new List<Product>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_Product_M
                               join b in db.Dmn_Product_D on new { a.PlantCode, a.ProdCode } equals new { b.PlantCode, b.ProdCode }
                               join c in db.Dmn_Line_M on new { a.PlantCode, a.LineID } equals new { c.PlantCode, c.LineID } into cc
                               from c in cc.DefaultIfEmpty()
                               join d in db.Dmn_Line_D on new { b.PlantCode, b.JobDetailType, c.LineID } equals new { d.PlantCode, d.JobDetailType, d.LineID } into dd
                               from d in dd.DefaultIfEmpty()
                               join e in db.Dmn_Serial_Expression on new { a.PlantCode, b.SnExpressionID } equals new { e.PlantCode, e.SnExpressionID } into ee
                               from e in ee.DefaultIfEmpty()
                               orderby b.JobDetailType
                               select new { a, b, c, d, e };

                    var builder = new StringBuilder();
                    builder.AppendLine($@"  SELECT 1 FROM Dmn_JobOrder_M JM
                                            LEFT JOIN Dmn_JobOrder_D JD ON JM.PlantCode = JD.PlantCode AND JM.OrderNo = JD.OrderNo AND JM.SeqNo = JD.SeqNo
                                            LEFT JOIN Dmn_Product_M	 PM	ON JM.PlantCode = PM.PlantCode AND JM.ProdCode = PM.ProdCode
                                            LEFT JOIN Dmn_Line_D	 LD	ON JM.PlantCode = LD.PlantCode AND JM.LineID = LD.LineID
                                            WHERE JM.PlantCode = '{plantCode}' AND JD.JobStatus NOT IN ('CC', 'IS') ");

                    if (!string.IsNullOrEmpty(plantCode))
                        list = list.Where(q => q.a.PlantCode.Equals(plantCode) && q.b.PlantCode.Equals(plantCode));
                    if (!string.IsNullOrEmpty(lineID))
                    {
                        list = list.Where(q => q.a.LineID.Equals(lineID));
                        builder.AppendLine($" AND JM.LineID = '{lineID}' ");
                    }
                    if (!string.IsNullOrEmpty(productCode))
                    {
                        list = list.Where(q => q.a.ProdCode.Contains(productCode));
                        builder.AppendLine($" AND JM.ProdCode = '{productCode}' ");
                    }
                    if (!string.IsNullOrEmpty(standardCode))
                    {
                        list = list.Where(q => q.a.ProdStdCode.Contains(standardCode));
                        builder.AppendLine($" AND PM.ProdStdCode = '{standardCode}' ");
                    }
                    if (!string.IsNullOrEmpty(productName))
                    {
                        list = list.Where(q => q.a.ProdName.Contains(productName));
                        builder.AppendLine($" AND PM.ProdName LIKE '%{productName}%' ");
                    }
                    if (use != null)
                    {
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Y") : q.a.UseYN.Equals("N"));
                        builder.AppendLine($" AND PM.UseYN = '{(use ?? false ? "Y" : "N")}' ");
                    }

                    var sql = $"{builder}";
                    var prodHasLineChangedOrder = db.Database.ExecuteSqlCommand(sql) > 0;

                    foreach (var item in list)
                    {
                        if (!retList.Any(x => x.ProdCode.Equals(item.a.ProdCode)))
                            retList.Add(new Product(item.a));
                        retList.Single(x => x.ProdCode.Equals(item.a.ProdCode)).ProductDetail.Add(new ProductDetail(item.b)
                        {
                            MachineUseYN = string.IsNullOrEmpty(machineID) || item.d == null || item.d.MachineID == null ? null : (item.d.MachineID.Equals(machineID) || prodHasLineChangedOrder ? "Y" : "N"),
                            SnExpressionStr = item.e == null ? null : item.e.SnExpressionStr,
                        });
                    }
                }
                return retList.FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController SelectServerSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController SelectServerSingle Exception", ex);
            }
            return retList.FirstOrDefault();
        }
        public static List<DSM.Dmn_Product_M> SelectServerMasterAll(bool RaiseException = false)
        {
            List<DSM.Dmn_Product_M> retList = new List<DSM.Dmn_Product_M>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_Product_M
                               select a;
                    retList = list.ToList();
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController SelectServerMasterAll Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController SelectServerMasterAll Exception", ex);
            }
            return retList;
        }
        public static List<DSM.Dmn_Product_D> SelectServerDetailAll(bool RaiseException = false)
        {
            List<DSM.Dmn_Product_D> retList = new List<DSM.Dmn_Product_D>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_Product_D
                               select a;
                    retList = list.ToList();
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController SelectServerDetailAll Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController SelectServerDetailAll Exception", ex);
            }
            return retList;
        }
        public static List<Product> SelectServerInterface(string plantCode)
        {
            List<Product> retList = new List<Product>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_Product_M
                               join b in db.Dmn_Product_D on new { a.PlantCode, a.ProdCode } equals new { b.PlantCode, b.ProdCode } into bb
                               from b in bb.DefaultIfEmpty()
                               orderby b.JobDetailType
                               select new { a, b };
                    if (!string.IsNullOrEmpty(plantCode))
                        list = list.Where(q => q.a.PlantCode.Equals(plantCode));
                    list = list.Where(q => q.b.SnType == "I");
                    foreach (var item in list)
                    {
                        if (!retList.Any(x => x.ProdCode.Equals(item.a.ProdCode)))
                            retList.Add(new Product(item.a));
                        retList.Single(x => x.ProdCode.Equals(item.a.ProdCode)).ProductDetail.Add(new ProductDetail(item.b)
                        {
                        });
                    }
                }
                return retList;
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController SelectSever Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                throw new Exception("ProductController SelectSever Exception", ex);
            }
        }
        public static bool InsertServer(DSM.Dmn_Product_M item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("ProductController InsertServerMaster by {0}, {1}", item.InsertUser, item.ToString());
                    db.Dmn_Product_M.AddOrUpdate(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController InsertServerMaster Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController InsertServerMaster Exception", ex);
                return false;
            }
        }
        public static bool InsertServer(DSM.Dmn_Product_D item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("ProductController InsertServerDetail by {0}, {1}", item.InsertUser, item.ToString());
                    db.Dmn_Product_D.AddOrUpdate(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController InsertServerDetail Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController InsertServerDetail Exception", ex);
                return false;
            }
        }
        public static bool UpdateServer(DSM.Dmn_Product_M item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("ProductController UpdateServerMaster by {0}, {1}", item.UpdateUser, item.ToString());
                    var tmp = db.Dmn_Product_M.First(q => q.PlantCode.Equals(item.PlantCode) && q.ProdCode.Equals(item.ProdCode));
                    tmp.WriteExistings(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController UpdateServerMaster Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController UpdateServerMaster Exception", ex);
                return false;
            }
        }
        public static bool UpdateServer(DSM.Dmn_Product_D item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("ProductController UpdateServerDetail by {0}, {1}", item.UpdateUser, item.ToString());
                    var tmp = db.Dmn_Product_D.First(q => q.PlantCode.Equals(item.PlantCode) && q.ProdCode.Equals(item.ProdCode) && q.JobDetailType.Equals(item.JobDetailType));
                    tmp.WriteExistings(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController UpdateServerDetail Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController UpdateServerDetail Exception", ex);
                return false;
            }
        }
        public static bool MergeIntoServer(DSM.Dmn_Product_M item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("ProductController MergeIntoServer Master by {0}, {1}", item.UpdateUser, item.ToString());
                    string sql = @"MERGE INTO Dmn_Product_M AS Target
                                   USING (SELECT @PlantCode, @ProdCode, @ProdName, @CompanyPrefix, @ProdStdCode, @Remark, @AGLevel, @UseYN) AS Source
                                   (PlantCode, ProdCode, ProdName, CompanyPrefix, ProdStdCode, Remark, AGLevel, UseYN)
                                   ON (Target.PlantCode = Source.PlantCode And Target.ProdCode = Source.ProdCode)
                                   WHEN MATCHED THEN
                                       UPDATE SET Target.ProdName = Source.ProdName, Target.CompanyPrefix = Source.CompanyPrefix, Target.ProdStdCode = Source.ProdStdCode, Target.Remark = Source.Remark, Target.AGLevel = Source.AGLevel, Target.UseYN = Source.UseYN
                                   WHEN NOT MATCHED THEN
                                       INSERT (PlantCode, ProdCode, ProdName, ProdStdCode, CompanyPrefix, Remark, AGLevel, Exp_Day, UseYN, InsertDate, InsertUser, UpdateDate, UpdateUser)
                                       VALUES (@PlantCode, @ProdCode, @ProdName, @ProdStdCode, @CompanyPrefix, @Remark, @AGLevel, @Exp_Day, @UseYN, @InsertDate, @InsertUser, @UpdateDate, @UpdateUser);";

                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(new SqlParameter("@PlantCode", item.PlantCode));
                    parameters.Add(new SqlParameter("@ProdCode", item.ProdCode));
                    parameters.Add(new SqlParameter("@ProdName", item.ProdName));
                    parameters.Add(new SqlParameter("@ProdStdCode", item.ProdStdCode));
                    parameters.Add(new SqlParameter("@CompanyPrefix", item.CompanyPrefix));
                    parameters.Add(new SqlParameter("@Remark", item.Remark));
                    parameters.Add(new SqlParameter("@AGLevel", item.AGLevel));
                    parameters.Add(new SqlParameter("@Exp_Day", item.Exp_Day));
                    parameters.Add(new SqlParameter("@UseYN", item.UseYN));
                    parameters.Add(new SqlParameter("@InsertDate", item.InsertDate == null || item.InsertDate == new DateTime() ? DateTime.Now : item.InsertDate));
                    parameters.Add(new SqlParameter("@InsertUser", item.InsertUser));
                    parameters.Add(new SqlParameter("@UpdateDate", item.InsertDate == null || item.InsertDate == new DateTime() ? (object)DBNull.Value : item.InsertDate));
                    parameters.Add(new SqlParameter("@UpdateUser", item.UpdateUser));
                    db.Database.ExecuteSqlCommand(sql, parameters.ToArray());

                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController UpdateServerDetail Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController UpdateServerDetail Exception", ex);
                return false;
            }
        }
        public static bool MergeIntoServer(DSM.Dmn_Product_D item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("ProductController MergeIntoServer Detail by {0}, {1}", item.UpdateUser, item.ToString());
                    string sql = @"MERGE INTO Dmn_Product_D AS Target 
                                   USING (SELECT @PlantCode, @ProdCode, @JobDetailType, @GS1ExtensionCode, @ProdStdCode, @Prefix_SSCC, @PackingCount) AS Source 
                                   (PlantCode, ProdCode, JobDetailType, GS1ExtensionCode, ProdStdCode, Prefix_SSCC, PackingCount) 
                                   ON (Target.PlantCode = Source.PlantCode And Target.ProdCode = Source.ProdCode And Target.JobDetailType = Source.JobDetailType) 
                                   WHEN MATCHED THEN 
                                       UPDATE SET Target.GS1ExtensionCode = Source.GS1ExtensionCode, Target.ProdStdCode = Source.ProdStdCode, Target.Prefix_SSCC = Source.Prefix_SSCC, Target.PackingCount = Source.PackingCount 
                                   WHEN NOT MATCHED THEN 
                                        INSERT (PlantCode, ProdCode, JobDetailType, ResourceType, GS1ExtensionCode, BarcodeType, BarcodeDataFormat, SnType, SnExpressionID, ProdStdCode, Prefix_SSCC, PackingCount, InsertDate, InsertUser, UpdateDate, UpdateUser) 
                                        VALUES (@PlantCode, @ProdCode, @JobDetailType, @ResourceType, @GS1ExtensionCode, @BarcodeType, @BarcodeDataFormat, @SnType, @SnExpressionID, @ProdStdCode, @Prefix_SSCC, @PackingCount, @InsertDate, @InsertUser, @UpdateDate, @UpdateUser);";

                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(new SqlParameter("@PlantCode", item.PlantCode));
                    parameters.Add(new SqlParameter("@ProdCode", item.ProdCode));
                    parameters.Add(new SqlParameter("@JobDetailType", item.JobDetailType));
                    parameters.Add(new SqlParameter("@ProdStdCode", item.ProdStdCode));
                    parameters.Add(new SqlParameter("@ResourceType", item.ResourceType));
                    parameters.Add(new SqlParameter("@GS1ExtensionCode", item.GS1ExtensionCode));
                    parameters.Add(new SqlParameter("@BarcodeType", item.BarcodeType));
                    parameters.Add(new SqlParameter("@BarcodeDataFormat", item.BarcodeDataFormat));
                    parameters.Add(new SqlParameter("@SnType", item.SnType));
                    parameters.Add(new SqlParameter("@SnExpressionID", item.SnExpressionID));
                    parameters.Add(new SqlParameter("@Prefix_SSCC", item.Prefix_SSCC));
                    parameters.Add(new SqlParameter("@PackingCount", item.PackingCount));
                    parameters.Add(new SqlParameter("@InsertDate", item.InsertDate == null || item.InsertDate == new DateTime() ? DateTime.Now : item.InsertDate));
                    parameters.Add(new SqlParameter("@InsertUser", item.InsertUser));
                    parameters.Add(new SqlParameter("@UpdateDate", item.InsertDate == null || item.InsertDate == new DateTime() ? (object)DBNull.Value : item.InsertDate));
                    parameters.Add(new SqlParameter("@UpdateUser", item.UpdateUser));
                    db.Database.ExecuteSqlCommand(sql, parameters.ToArray());
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController UpdateServerDetail Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController UpdateServerDetail Exception", ex);
                return false;
            }
        }
        public static bool DeleteDetailNonExistingJobDetail(string plant, string prodCode, List<string> jobDetail, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    string dtl = string.Join(", ", jobDetail.Select(q => $"'{q}'"));
                    log.InfoFormat("ProductController DeleteDetailNonExistingJobDetail {0}, {1}, {2}", plant, prodCode, dtl);
                    string sql = $@"DELETE Dmn_Product_D where PlantCode = @PlantCode And ProdCode = @ProdCode And JobDetailType Not IN ({dtl})";

                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(new SqlParameter("@PlantCode", plant));
                    parameters.Add(new SqlParameter("@ProdCode", prodCode));
                    db.Database.ExecuteSqlCommand(sql.ToString(), parameters.ToArray());
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController DeleteDetailNonExistingJobDetail Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController DeleteDetailNonExistingJobDetail Exception", ex);
                return false;
            }
        }
        public static bool DeleteServerAll(string plantCode, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("ProductController DeleteServerAll by {0}", userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Product_D where PlantCode = {0}", plantCode);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Product_M where PlantCode = {0}", plantCode);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController DeleteServerAll Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController DeleteServerAll Exception", ex);
                return false;
            }

        }
        public static bool DeleteServerSingle(string plantCode, string prouductCode, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("ProductController DeleteServerSingle {0} {1} by {1}", plantCode, prouductCode, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Product_D where PlantCode = {0} AND ProdCode = {1}", plantCode, prouductCode);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Product_M where PlantCode = {0} AND ProdCode = {1}", plantCode, prouductCode);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController DeleteServerSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController DeleteServerSingle Exception", ex);
                return false;
            }
        }
        public static bool DeleteServerDetail(string plantCode, string prodCode, string jobDetail, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("ProductController DeleteServerDetail {0} by {1}", plantCode, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Product_D where PlantCode = {0} AND ProdCode = {1} AND JobDetailType = {2}", plantCode, prodCode, jobDetail);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController DeleteServerDetail Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController DeleteServerDetail Exception", ex);
                return false;
            }
        }
        public static bool DeleteServerDetailAll(string plantCode, string prodCode, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("ProductController DeleteServerDetailAll {0} by {1}", plantCode, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Product_D where PlantCode = {0} AND ProdCode = {1}", plantCode, prodCode);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController DeleteServerDetailAll Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ProductController DeleteServerDetailAll Exception", ex);
                return false;
            }
        }

        /// <summary>
        /// Updates all Dmn_Product_D.ProdStdCode if it's null
        /// </summary>
        /// <returns>Row count of updated rows.</returns>
        /// <exception cref="Exception"></exception>
        public static List<ProdStdCodeUpdate> UpdateNullProdStdCode()
        {
            List<ProdStdCodeUpdate> ret = null;

			try
            {
                log.InfoFormat("ProductController UpdateNullProdStdCode start");

                var resName = "ufn_AddCheckDigit";
                if (!DominoDBServer.HasObject(resName, "FN"))
                    DominoDBServer.CreateDbObjectDSMFromResource(resName, false, "FN");

                using (var db = new DominoDBServer())
                {

                    var r = db.Database.ExecuteSqlCommand("select * from Dmn_Product_D where ProdStdCode is null");
                    log.InfoFormat("ProductController UpdateNullProdStdCode: found {0} rows of Product_D does not have ProdStdCode Value.", r);

                    if (r != 0)
                    {
                        ret = db.Database.SqlQuery<ProdStdCodeUpdate>(@"
                            if (select object_id('tempdb..#PD')) is not null drop table #PD

                            select      PlantCode, ProdCode, JobDetailType, ProdStdCode into #PD from Dmn_Product_D where ProdStdCode is null

                            Declare     @DT datetime = Getdate()

                            set nocount on;
                            update		M
                            set			UpdateDate = GETDATE()
			                            ,UpdateUser = 'System'
                            from		Dmn_Product_M M
                            where		ProdCode in (select ProdCode from #PD)

                            update		D
                            set			ProdStdCode = dbo.ufn_AddCheckDigit(GS1ExtensionCode, M.ProdStdCode)
			                            ,UpdateDate = GETDATE()
			                            ,UpdateUser = 'System'
                            from		Dmn_Product_D D
                            LEFT JOIN	Dmn_Product_M M
                            ON			M.PlantCode = D.PlantCode AND M.ProdCode = D.ProdCode
                            where		D.ProdStdCode is null
                            set nocount off;

                            select      P.PlantCode, P.ProdCode, P.JobDetailType, P.ProdStdCode [Before], D.ProdStdCode [After] 
                            from        #PD P
                            left join   Dmn_Product_D D
                            on          P.PlantCode = D.PlantCode
                                        and p.ProdCode = d.ProdCode 
                                        and p.JobDetailType = d.JobDetailType

                            if (select object_id('tempdb..#PD')) is not null drop table #PD").ToList();
                        log.InfoFormat("ProductController UpdateNullProdStdCode {0} rows has been updated", ret.Count());
					}
                }
                return ret;
            }
            catch (Exception ex)
            {
                log.InfoFormat("ProductController UpdateNullProdStdCode Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return null;
            }
        }

        public class ProdStdCodeUpdate
        {
            public string PlantCode { get; set; }
            public string Prodcode { get; set; }
            public string JobDetailType { get; set; }
            public string Before { get; set; }
            public string After { get; set; }

            public ProdStdCodeUpdate() : base() { }
        }
    }
}

