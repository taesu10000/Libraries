using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using System.Data.Entity.Migrations;
using DominoDatabase.DSM;
using System.Text;
using System.Data.SqlClient;
using System.Collections;

namespace DominoDatabase.Controls
{
    public class ReadBarcodeController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static long TotalPageCount { get; set; }
        public static long TotalCount { get; set; }
        public static List<ReadBarcode> SelectLocal(string standardCode = "", string orderNo = "", string seqNo = "", string jobDetailType = "",
            string serialNumber = "", string parentStdCode = "", string parentSerialNo = "", string status = "", int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            List<ReadBarcode> retList = new List<ReadBarcode>();
            try
            {
                var readBarcodeProps = typeof(ReadBarcode).GetProperties();
                var entityProps = typeof(Local.Dmn_ReadBarcode).GetProperties();
                var commonPropertyNames = readBarcodeProps.Select(q => q.Name).Intersect(entityProps.Select(q => q.Name)).ToList();
                var sqlProps =  string.Join(", ", commonPropertyNames);
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    StringBuilder sb = new StringBuilder();
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    List<string> conditions = new List<string>();
                    string parStdCode = "@stdCode";
                    string parSerial = "@serial";
                    string parOrderNo = "@orderNo";
                    string parSeqNo = "@seqNo";
                    string parJobDetailType = "@jobDetailType";
                    string parParentStdCode = "@parentStdCode";
                    string parParentSerialNo = "@parentSerialNo";
                    string parStatus = "@status";


                    if (!string.IsNullOrEmpty(standardCode))
                    { 
                        parameters.Add(new SqlParameter(parStdCode, standardCode));
                        conditions.Add($"ProdStdCode = {parStdCode}");
                    }
                    if (!string.IsNullOrEmpty(serialNumber))
                    { 
                        parameters.Add(new SqlParameter(parSerial, $"%{serialNumber}%"));
                        conditions.Add($" SerialNum LIKE {parSerial}");
                    }
                    if (!string.IsNullOrEmpty(orderNo))
                    { 
                        parameters.Add(new SqlParameter(parOrderNo, orderNo));
                        conditions.Add($"OrderNo = {parOrderNo}");
                    }
                    if (!string.IsNullOrEmpty(seqNo))
                    { 
                        parameters.Add(new SqlParameter(parSeqNo, seqNo));
                        conditions.Add($"SeqNo = {parSeqNo}");
                    }
                    if (!string.IsNullOrEmpty(jobDetailType))
                    { 
                        parameters.Add(new SqlParameter(parJobDetailType, jobDetailType));
                        conditions.Add($"JobDetailType = {parJobDetailType}");
                    }
                    if (!string.IsNullOrEmpty(parentStdCode))
                    { 
                        parameters.Add(new SqlParameter(parParentStdCode, $"%{parentStdCode}%"));
                        conditions.Add($"ParentProdStdCode LIKE {parParentStdCode}");
                    }
                    if (!string.IsNullOrEmpty(parentSerialNo))
                    { 
                        parameters.Add(new SqlParameter(parParentSerialNo, $"%{parentSerialNo}%"));
                        conditions.Add($"ParentSerialNum LIKE {parParentSerialNo}");
                    }
                    if (!string.IsNullOrEmpty(status))
                    { 
                        parameters.Add(new SqlParameter(parStatus, $"%{status}%"));
                        conditions.Add($"Status LIKE {parStatus}");
                    }

                    StringBuilder query = new StringBuilder($"SELECT {sqlProps} FROM Dmn_ReadBarcode ");
                    if (conditions.Count > 0)
                    {
                        query.Append(" WHERE ");
                        query.Append("(");
                        query.Append(string.Join(" AND ", conditions));
                        query.Append(")");
                    }

                    var sql = query.ToString();
                    retList = db.Database.SqlQuery<ReadBarcode>(sql.ToString(), parameters.ToArray()).ToList();
                }

                if (pageIndex >= 0)
                {
                    TotalCount = retList.Count();
                    TotalPageCount = Convert.ToInt64(Math.Ceiling(Convert.ToDouble(TotalCount) / Convert.ToDouble(pageSize)));
                    retList = retList.OrderBy(x => x.InsertDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }

                return retList;
            }
            catch (Exception ex)
            {
                log.InfoFormat("ReadBarcodeController SelectLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ReadBarcodeController SelectLocal Exception", ex);
                return new List<ReadBarcode>();
            }
        }
        public static List<ReadBarcode> SelectLocal(List<SGTIN> serials, bool RaiseException = false)
        {
            List<ReadBarcode> retList = new List<ReadBarcode>();
            try
            {
                var readBarcodeProps = typeof(ReadBarcode).GetProperties();
                var entityProps = typeof(Local.Dmn_ReadBarcode).GetProperties();
                var commonPropertyNames = readBarcodeProps.Select(q => q.Name).Intersect(entityProps.Select(q => q.Name)).ToList();
                var sqlProps = string.Join(", ", commonPropertyNames);
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    DateTime now = DateTime.Now;
					StringBuilder sql = new StringBuilder($"SELECT {sqlProps} FROM Dmn_ReadBarcode");
                    List<string> conditions = new List<string>();
					List<SqlParameter> parameters = new List<SqlParameter>();

                    for (int i = 0; i < serials.Count; i++)
                    {
						string stdCodeParam = "@stdCode" + i;
						string serialNumParam = "@serialNum" + i;

                        List<string> subConditions = new List<string>();
                        StringBuilder subConditionsSb = new StringBuilder();
                        if (!string.IsNullOrEmpty(serials[i].StdCode))
                        {
                            subConditions.Add($"ProdStdCode = {stdCodeParam}");
                            parameters.Add(new SqlParameter(stdCodeParam, serials[i].StdCode));
                        }
                        if (!string.IsNullOrEmpty(serials[i].Serial))
                        {
                            subConditions.Add($"SerialNum = {serialNumParam}");
                            parameters.Add(new SqlParameter(serialNumParam, serials[i].Serial));
                        }
                        if (subConditions.Count > 0)
                        {
                            subConditionsSb.Append("(");
                            subConditionsSb.Append(string.Join(" AND ", subConditions));
                            subConditionsSb.Append(")");
                        }

                        conditions.Add(subConditionsSb.ToString());
					}

                    if (conditions.Count > 0)
                    {
                        sql.Append(" WHERE ");
                        sql.Append(string.Join(" OR ", conditions));
                    }
                    else
                        return new List<ReadBarcode>();

                    retList = db.Database.SqlQuery<ReadBarcode>(sql.ToString(), parameters.ToArray()).ToList();
                    log.InfoFormat("SelectLocal Elapsed {0}", (DateTime.Now - now).TotalMilliseconds);
                    Console.WriteLine("SelectLocal Elapsed {0}", (DateTime.Now - now).TotalMilliseconds);

                    return retList;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ReadBarcodeController SelectLocalQuery Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ReadBarcodeController SelectLocalQuery Exception", ex);
                throw;
            }
        }
		public static ReadBarcode SelectLocalSingle(string stdCode, string serial, bool RaiseException = false)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
					StringBuilder sql = new StringBuilder($"SELECT * FROM Dmn_ReadBarcode");

					List<SqlParameter> parameters = new List<SqlParameter>();
                    string stdCodeName = "@stdCode";
                    string serialName = "@serial";

                    List<string> subConditions = new List<string>();
                    StringBuilder subConditionsSb = new StringBuilder();
                    if (!string.IsNullOrEmpty(stdCode))
                    {
                        subConditions.Add($"ProdStdCode = {stdCodeName}");
                        parameters.Add(new SqlParameter(stdCodeName, stdCode));
                    }
                    if (!string.IsNullOrEmpty(serial))
                    {
                        subConditions.Add($"SerialNum = {serialName}");
                        parameters.Add(new SqlParameter(serialName, serial));
                    }
                    if (subConditions.Count > 0)
                    {
                        subConditionsSb.Append("(");
                        subConditionsSb.Append(string.Join(" AND ", subConditions));
                        subConditionsSb.Append(")");

                        sql.Append(" WHERE ");
                        sql.Append(string.Join(" AND ", subConditionsSb));
                    }

                    var list = db.Database.SqlQuery<Local.Dmn_ReadBarcode>(sql.ToString(), parameters.ToArray()).ToList();
					var item = list.FirstOrDefault();
                    if (item != null)
                        return new ReadBarcode(item);
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ReadBarcodeController SelectLocalQuery Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ReadBarcodeController SelectLocalQuery Exception", ex);
                throw ex;
            }
        }
        public static List<ReadBarcode> SelectLocalFromHelpCode(HelpCodePool helpCodes = null, bool RaiseException = false)
        {
            List<ReadBarcode> retList = new List<ReadBarcode>();
            if (helpCodes == null)
                return retList;
            try
            {
                List<string> serialList = helpCodes.HelpCodePoolDetail.Select(q => q.ChildProdStdCode + q.ChildSerialNum).ToList();
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_ReadBarcode
                               where serialList.Contains(a.ProdStdCode + a.SerialNum)
                               select a;

                    foreach (var rb in list)
                        retList.Add(new ReadBarcode(rb));
                }
                return retList;
            }
            catch (Exception ex)
            {
                log.InfoFormat("ReadBarcodeController SelectLocalFromHelpCode Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ReadBarcodeController SelectLocalFromHelpCode Exception", ex);
                return new List<ReadBarcode>();
            }
        }
        public static bool InsertLocal(Local.Dmn_ReadBarcode item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("ReadBarcodeController InsertLocal {0} by {1}", item.SerialNum, item.InsertUser);
                    db.Dmn_ReadBarcode.Add(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ReadBarcodeController InsertLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ReadBarcodeController InsertLocal Exception", ex);
                return false;
            }
        }
        public static bool InsertLocal(List<ReadBarcode> items, string userID, bool RaiseException = false)
        {
            try
            {
                List<Local.Dmn_ReadBarcode> list = new List<Local.Dmn_ReadBarcode>();
                string log = string.Format("ReadBarcodeController InsertLocal by {0}", userID);
                foreach (var item in items)
                { 
                    list.Add(new Local.Dmn_ReadBarcode(item));
                    log += string.Format("[StandardCode : {0}] [Serial : {1}]", item.ProdStdCode, item.SerialNum);
                }

                using (var db = new DominoDBLocal())
                {
                    db.Dmn_ReadBarcode.AddRange(list);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ReadBarcodeController InsertLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ReadBarcodeController InsertLocal Exception", ex);
                return false;
            }
        }
        public static bool UpdateLocal(Local.Dmn_ReadBarcode item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("ReadBarcodeController UpdateLocal {0} by {1}", item.SerialNum, item.UpdateUser);
                    var tmp = db.Dmn_ReadBarcode.First(q => q.ProdStdCode.Equals(item.ProdStdCode) && q.SerialNum.Equals(item.SerialNum));
                    tmp.WriteExistings(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ReadBarcodeController UpdateLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ReadBarcodeController UpdateLocal Exception", ex);
                return false;
            }
        }
        public static bool UpdateLocal(List<ReadBarcode> readBarcodes, bool RaiseException = false)
        {
            try
            {
                List<Local.Dmn_ReadBarcode> list = new List<Local.Dmn_ReadBarcode>();
                string log = "ReadBarcodeController UpdateLocal";
                foreach (var item in readBarcodes)
                {
                    list.Add(new Local.Dmn_ReadBarcode(item));
                    log += string.Format("\r\n[StandardCode : {0}] [Serial : {1}]", item.ProdStdCode, item.SerialNum);
                }

                using (var db = new DominoDBLocal())
                {
                    foreach (var item in list)
                    {
                        db.Dmn_ReadBarcode.Attach(item);
                        db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                        db.Dmn_ReadBarcode.WriteExistings(item);
                    }
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ReadBarcodeController UpdateLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ReadBarcodeController UpdateLocal Exception", ex);
                return false;
            }
        }
        public static bool DeleteLocalAll(string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("ReadBarcodeController DeleteLocalAll by {0}", userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_ReadBarcode");
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ReadBarcodeController DeleteLocalAll Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ReadBarcodeController DeleteLocalAll Exception", ex);
                return false;
            }

        }
        public static bool DeleteLocalSingle(string standardCode, string serialnumber, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("ReadBarcodeController DeleteLocalSingle {0} {1} by {2}" ,standardCode, serialnumber, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_ReadBarcode where ProdStdCode = {0} AND SerialNum = {1} ", standardCode, serialnumber);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ReadBarcodeController DeleteLocalSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ReadBarcodeController DeleteLocalSingle Exception", ex);
                return false;
            }
        }
        public static bool DeleteLocal(List<ReadBarcode> readBarcodes, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    StringBuilder sql = new StringBuilder("SELECT * FROM Dmn_ReadBarcode WHERE ");
                    List<string> conditions = new List<string>();
                    foreach (var readBarcode in readBarcodes)
                    {
                        conditions.Add($"(ProdStdCode = '{readBarcode.ProdStdCode}' AND SerialNum = '{readBarcode.SerialNum}')");
                    }
                    sql.Append(string.Join(" OR ", conditions));

                    var list = db.Select<Local.Dmn_ReadBarcode>(sql.ToString());
                    db.Dmn_ReadBarcode.RemoveRange(list);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ReadBarcodeController DeleteLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ReadBarcodeController DeleteLocal Exception", ex);
                return false;
            }
        }
        public static bool DeleteLocalSingleParent(string parentStandardCode, string parentSerialnumber, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("ReadBarcodeController DeleteLocalSingleParent {0} {1} by {2}", parentStandardCode, parentSerialnumber, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_ReadBarcode where ParentProdStdCode = {0} AND ParentSerialNum = {1}", parentStandardCode, parentSerialnumber);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ReadBarcodeController DeleteLocalSingleParent Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ReadBarcodeController DeleteLocalSingleParent Exception", ex);
                return false;
            }
        }
        public static bool DeleteLocalByOrder(string orderNo, string seqNo, string detailType, string userID, bool RaiseException = false)
        {
            try
            {
                if (string.IsNullOrEmpty(orderNo) || string.IsNullOrEmpty(seqNo))
                    return false;

                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("ReadBarcodeController DeleteLocalByOrder {0}{1} {2} by {3}", orderNo, seqNo, detailType, userID);
                    string sqlCmd = string.Format("DELETE FROM Dmn_ReadBarcode WHERE OrderNo = \'{0}\' AND SeqNo = \'{1}\'", orderNo, seqNo);
                    if (string.IsNullOrEmpty(detailType) == false)
                        sqlCmd += string.Format(" AND JobDetailType = \'{0}\'", detailType);

                    db.Database.ExecuteSqlCommand(sqlCmd);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ReadBarcodeController DeleteLocalByOrder Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ReadBarcodeController DeleteLocalByOrder Exception", ex);
                return false;
            }
        }
        public static List<ReadBarcode> SelectServer(string plantCode, string machinID, string standardCode, string productName, string orderNo, string seqNo, string jobDetailType,
           string serialNumber, string parentSerialNumber, string status, int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            List<ReadBarcode> retList = new List<ReadBarcode>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_ReadBarcode
                               join b in db.Dmn_JobOrder_M on new { a.PlantCode, a.OrderNo, a.SeqNo } equals new { b.PlantCode, b.OrderNo, b.SeqNo } into bb
                               from b in bb.DefaultIfEmpty()
                               join c in db.Dmn_Product_M on new { b.PlantCode, b.ProdCode } equals new { c.PlantCode, c.ProdCode } into cc
                               from c in cc.DefaultIfEmpty()
                               join d in db.Dmn_SerialPool on new { a.PlantCode, a.ProdStdCode, a.SerialNum } equals new { d.PlantCode, d.ProdStdCode, d.SerialNum } into dd
                               from d in dd.DefaultIfEmpty()
                               select new { a, b, c, d };
                    if (!string.IsNullOrEmpty(plantCode))
                        list = list.Where(q => q.a.PlantCode.Equals(plantCode) && q.b.PlantCode.Equals(plantCode));
                    if (!string.IsNullOrEmpty(machinID))
                        list = list.Where(q => q.a.MachineID.Equals(machinID));
                    if (!string.IsNullOrEmpty(standardCode))
                        list = list.Where(q => q.a.ProdStdCode.Equals(standardCode));
                    if (!string.IsNullOrEmpty(productName))
                        list = list.Where(q => q.c.ProdName.Contains(productName));
                    if (!string.IsNullOrEmpty(orderNo))
                        list = list.Where(q => q.a.OrderNo.Equals(orderNo));
                    if (!string.IsNullOrEmpty(seqNo))
                        list = list.Where(q => q.a.SeqNo.Equals(seqNo));
                    if (!string.IsNullOrEmpty(jobDetailType))
                        list = list.Where(q => q.a.JobDetailType.Equals(jobDetailType));
                    if (!string.IsNullOrEmpty(serialNumber))
                        list = list.Where(q => q.a.SerialNum.Contains(serialNumber));
                    if (!string.IsNullOrEmpty(parentSerialNumber))
                        list = list.Where(q => q.a.ParentSerialNum.Contains(parentSerialNumber));
                    if (!string.IsNullOrEmpty(status))
                        list = list.Where(q => q.a.Status.Equals(status));
                    foreach (var item in list)
                    {
                        retList.Add(new ReadBarcode(item.a)
                        {
                            BarcodeType = item.d == null ? null : item.d.BarcodeType,
                            BarcodeDataFormat = item.d == null ? null : item.d.BarcodeDataFormat,
                        });
                    }
                    if (pageIndex >= 0)
                    {
                        TotalCount = retList.Count();
                        TotalPageCount = Convert.ToInt64(Math.Ceiling(Convert.ToDouble(TotalCount) / Convert.ToDouble(pageSize)));
                        retList = retList.OrderBy(x => x.InsertDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    }
                }
                return retList;
            }
            catch (Exception ex)
            {
                log.InfoFormat("ReadBarcodeController SelectServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ReadBarcodeController SelectServer Exception", ex);
            }
            return retList;
        }
        public static List<Dmn_ReadBarcode> SelectServerQuery(string plantCode, string orderNo, string seqNo, bool RaiseException = false)
        {
            List<Dmn_ReadBarcode> retList = new List<Dmn_ReadBarcode>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    retList = db.Dmn_ReadBarcode
                                .Where(a => a.PlantCode == plantCode && a.OrderNo == orderNo && a.SeqNo == seqNo)
                                .ToList();
                }
                return retList;
            }
            catch (Exception ex)
            {
                log.InfoFormat("ReadBarcodeController SelectServerQuery Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ReadBarcodeController SelectServerQuery Exception", ex);
            }
            return retList;
        }
        public static ReadBarcode SelectServerSingle(string plantCode, string machinID, string standardCode, string productName, string orderNo, string seqNo, string jobDetailType,
            string serialNumber, string parentSerialNumber, bool RaiseException = false)
        {
            List<ReadBarcode> retList = new List<ReadBarcode>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_ReadBarcode
                               join b in db.Dmn_Product_M
                               on a.ProdStdCode equals b.ProdStdCode
                               select new { a, b };
                    if (!string.IsNullOrEmpty(plantCode))
                        list = list.Where(q => q.a.PlantCode.Equals(plantCode) && q.b.PlantCode.Equals(plantCode));
                    if (!string.IsNullOrEmpty(machinID))
                        list = list.Where(q => q.a.MachineID.Equals(machinID));
                    if (!string.IsNullOrEmpty(standardCode))
                        list = list.Where(q => q.a.ProdStdCode.Equals(standardCode));
                    if (!string.IsNullOrEmpty(productName))
                        list = list.Where(q => q.b.ProdName.Equals(productName));
                    if (!string.IsNullOrEmpty(orderNo))
                        list = list.Where(q => q.a.OrderNo.Equals(orderNo));
                    if (!string.IsNullOrEmpty(seqNo))
                        list = list.Where(q => q.a.SeqNo.Equals(seqNo));
                    if (!string.IsNullOrEmpty(jobDetailType))
                        list = list.Where(q => q.a.JobDetailType.Equals(jobDetailType));
                    if (!string.IsNullOrEmpty(serialNumber))
                        list = list.Where(q => q.a.SerialNum.Contains(serialNumber));
                    if (!string.IsNullOrEmpty(parentSerialNumber))
                        list = list.Where(q => q.a.ParentSerialNum.Contains(parentSerialNumber));
                    foreach (var item in list)
                    {
                        retList.Add(new ReadBarcode(item.a));
                    }
                }
                return retList.FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.InfoFormat("ReadBarcodeController SelectServerSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ReadBarcodeController SelectServerSingle Exception", ex);
            }
            return retList.FirstOrDefault();
        }

        [Obsolete("", true)]
		public static List<DSM.Dmn_ReadBarcode> SelectExtendedServerQuery(string plantCode, string orderNo, string seqNo, bool RaiseException = false)
		{
			var retList = new List<DSM.Dmn_ReadBarcode>();
			try
			{
				using (DominoDBServer db = new DominoDBServer())
				{
                    retList = db.Database.SqlQuery<DSM.Dmn_ReadBarcode>($" EXEC usp_GetExtendedReadBarcode '{plantCode}', '{orderNo}', '{seqNo}' --; ").ToList();
				}
				return retList;
			}
			catch (Exception ex)
			{
				log.InfoFormat("ReadBarcodeController SelectServerQuery Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
				if (RaiseException)
					throw new Exception("ReadBarcodeController SelectServerQuery Exception", ex);
			}
			return retList;
		}
		public static bool InsertServer(DSM.Dmn_ReadBarcode item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("ReadBarcodeController InsertServer by {0}, {1}", item.InsertUser, item.ToString());
                    db.Dmn_ReadBarcode.AddOrUpdate(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ReadBarcodeController InsertServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ReadBarcodeController InsertServer Exception", ex);
                return false;
            }
        }
        public static bool InsertServer(List<DSM.Dmn_ReadBarcode> item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("ReadBarcodeController AddOrUpdate List Start");
                    for (int i = 0; i < item.Count; i++)
                    {
                        db.Dmn_ReadBarcode.AddOrUpdate(item[i]);
                    }
                    db.SaveChanges();
                    log.InfoFormat("ReadBarcodeController AddOrUpdate List End");
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ReadBarcodeController InsertServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ReadBarcodeController InsertServer Exception", ex);
                return false;
            }
        }
        public static bool ReportServer(List<DSM.Dmn_ReadBarcode> item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("ReadBarcodeController Report Start");
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_ReadBarcode where PlantCode = {0} and MachineID = {1} and OrderNo = {2} and SeqNo = {3}", item[0].PlantCode,
                        item[0].MachineID, item[0].OrderNo, item[0].SeqNo);
                    db.Dmn_ReadBarcode.AddRange(item);
                    db.SaveChanges();
                    log.InfoFormat("ReadBarcodeController Report End");
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ReadBarcodeController Report Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ReadBarcodeController Report Exception", ex);
                return false;
            }
        }
        public static bool UpdateServer(DSM.Dmn_ReadBarcode item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("ReadBarcodeController UpdateServer by {0}", item.UpdateUser);
                    var tmp = db.Dmn_ReadBarcode.First(q => q.ProdStdCode.Equals(item.ProdStdCode) && q.SerialNum.Equals(item.SerialNum) && q.JobDetailType.Equals(item.JobDetailType));
                    tmp.WriteExistings(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ReadBarcodeController UpdateServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ReadBarcodeController UpdateServer Exception", ex);
                return false;
            }
        }
        public static bool DeleteServerAll(string plnatCode, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("ReadBarcodeController DeleteLocalAll {0} by {1}", plnatCode, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_ReadBarcode");
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ReadBarcodeController DeleteLocalAll Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ReadBarcodeController DeleteLocalAll Exception", ex);
                return false;
            }

        }
        public static bool DeleteServerSingle(string plnatCode, string standardCode, string serialnumber, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("ReadBarcodeController DeleteLocalSingle {0} {1} {2} by {3}", plnatCode, standardCode, serialnumber, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_ReadBarcode where PlantCode = {0} AND ProdStdCode = {1} AND SerialNum = {2}", plnatCode, standardCode, serialnumber);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ReadBarcodeController DeleteLocalSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ReadBarcodeController DeleteLocalSingle Exception", ex);
                return false;
            }
        }
        public static bool DeleteServerByOrder(string plnatCode, string standardCode, string orderNo, string seqNo, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("ReadBarcodeController DeleteServerByOrder {0} {1} {2} by {3}", standardCode, orderNo, seqNo, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_ReadBarcode where PlantCode = {0} AND ProdStdCode = {1} AND OrderNo = {2} AND SeqNo = {3}", plnatCode, standardCode, orderNo, seqNo);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ReadBarcodeController DeleteServerByOrder Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ReadBarcodeController DeleteServerByOrder Exception", ex);
                return false;
            }
        }
        public static bool DeleteLocalByCompleteDate(DateTime dt, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("ReadBarcodeController DeleteLocalByCompleteDate {0} by {1}", dt, userID);
                    string sql = string.Format(@"DELETE A
                                   FROM Dmn_ReadBarcode AS A
                                   JOIN Dmn_JobOrder_AG AS B ON A.OrderNo = B.OrderNo AND A.SeqNo = B.SeqNo
                                   Where B.CompleteDate < '{0}'", dt.ToString("yyyy-MM-dd"));
                    db.Database.ExecuteSqlCommand(sql);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ReadBarcodeController DeleteLocalByCompleteDate Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ReadBarcodeController DeleteLocalByCompleteDate Exception", ex);
                return false;
            }
        }
        public static bool DeleteLocalCancelByInsertDate(DateTime dt, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("ReadBarcodeController DeleteLocalCancelByInsertDate {0} by {1}", dt, userID);
                    string sql = string.Format(@"DELETE A
                                   FROM Dmn_ReadBarcode AS A
                                   JOIN Dmn_JobOrder_AG AS B ON A.OrderNo = B.OrderNo AND A.SeqNo = B.SeqNo
                                   Where B.InsertDate < '{0}' AND JobStatus = 'CC'", dt.ToString("yyyy-MM-dd"));
                    db.Database.ExecuteSqlCommand(sql);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ReadBarcodeController DeleteLocalCancelByInsertDate Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ReadBarcodeController DeleteLocalCancelByInsertDate Exception", ex);
                return false;
            }
        }
        public static bool UpdateStatus(string plantCode, string orderNo, string seqNo,  string jobDetailType, string serialNum, string status, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("ReadBarcodeController UpdateStatus {0} {1} {2} {3} {4} by {5}", plantCode, orderNo + seqNo, jobDetailType, serialNum, status, userID);
                    db.Database.ExecuteSqlCommand("Update Dmn_ReadBarcode SET Status ={0} where PlantCode ={1} AND OrderNo ={2} AND SeqNo ={3} AND JobDetailType ={4} AND SerialNum ={5}", status, plantCode, orderNo, seqNo, jobDetailType, serialNum);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ReadBarcodeController UpdateStatus Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ReadBarcodeController UpdateStatus Exception", ex);
                return false;
            }
        }

        public static bool UpdateAGStatus(string productcode , string orderNo, string seqNo, string jobDetailType, string serialNum, string status, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("ReadBarcodeController UpdateStatus {0} {1} {2} {3} {4} by {5}", productcode, orderNo + seqNo, jobDetailType, serialNum, status, userID);
                    db.Database.ExecuteSqlCommand("Update Dmn_ReadBarcode SET Status ={0} where ProdStdCode ={1} AND OrderNo ={2} AND SeqNo ={3} AND JobDetailType ={4} AND SerialNum ={5}", status, productcode, orderNo, seqNo, jobDetailType, serialNum);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ReadBarcodeController UpdateStatus Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ReadBarcodeController UpdateStatus Exception", ex);
                return false;
            }
        }

    }
}
