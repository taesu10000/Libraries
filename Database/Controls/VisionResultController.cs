using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;

namespace DominoDatabase.Controls
{
    public class VisionResultController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static long TotalPageCount { get; set; }
        public static long TotalCount { get; set; }
        public static List<VisionResult> SelectLocal(string standardCode, string productName, string orderNo, string seqNo, 
            string jobDetailType, string status, DateTime? startDate, DateTime? endDate ,int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            List<VisionResult> retList = new List<VisionResult>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_VisionResult
                               join b in db.Dmn_JobOrder_M on new { a.OrderNo, a.SeqNo } equals new { b.OrderNo, b.SeqNo } into bb
                               from b in bb.DefaultIfEmpty()
                               join c in db.Dmn_Product_M on b.ProdCode equals c.ProdCode into cc
                               from c in cc.DefaultIfEmpty()
                               join d in db.Dmn_SerialPool on new { a.OrderNo, a.SeqNo, a.InsertDate } equals new { d.OrderNo, d.SeqNo, InsertDate = d.InspectedDate  } into dd
                               from d in dd.DefaultIfEmpty()
                               select new { a, b, c, d };
                    if (!string.IsNullOrEmpty(standardCode))
                        list = list.Where(q => q.c.ProdStdCode.Equals(standardCode));
                    if (!string.IsNullOrEmpty(productName))
                        list = list.Where(q => q.c.ProdName.Contains(productName));
                    if (!string.IsNullOrEmpty(orderNo))
                        list = list.Where(q => q.a.OrderNo.Equals(orderNo));
                    if (!string.IsNullOrEmpty(seqNo))
                        list = list.Where(q => q.a.SeqNo.Equals(seqNo));
                    if (!string.IsNullOrEmpty(jobDetailType))
                        list = list.Where(q => q.a.JobDetailType.Equals(jobDetailType));
                    if (!string.IsNullOrEmpty(status))
                        list = list.Where(q => q.a.Status.Equals(status));
                    if (startDate != null)
                        list = list.Where(q => q.a.InsertDate >= startDate.Value);
                    if (endDate != null)
                        list = list.Where(q => q.a.InsertDate <= endDate.Value);
                    //list = list.Where(q => q.d != null && q.d.InspectedDate != null);
                    foreach (var item in list)
                    {
                        retList.Add(new VisionResult(item.a)
                        {
                            ProdStdCode = item.c == null ? null : item.c.ProdStdCode,

                            ProductName = item.c == null ? null : item.c.ProdName,
                            SerialNumber = item.d == null ? null : item.d.SerialNum
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
                log.InfoFormat("VisionResultController SelectLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("VisionResultController SelectLocal Exception", ex);
            }
            return retList;
        }
        public static VisionResult SelectLocalSingle(string standardCode, string productName, string orderNo, string seqNo,
            string jobDetailType, string status, string serialNo, DateTime? insertDate, bool RaiseException = false)
        {
            List<VisionResult> retList = new List<VisionResult>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_VisionResult
                               join b in db.Dmn_JobOrder_M on new { a.OrderNo, a.SeqNo } equals new { b.OrderNo, b.SeqNo } into bb
                               from b in bb.DefaultIfEmpty()
                               join c in db.Dmn_Product_M on b.ProdCode equals c.ProdCode into cc
                               from c in cc.DefaultIfEmpty()
                               join d in db.Dmn_SerialPool on new { a.OrderNo, a.SeqNo, a.InsertDate } equals new { d.OrderNo, d.SeqNo, InsertDate = d.InspectedDate  } into dd
                               from d in dd.DefaultIfEmpty()
                               select new { a, b, c, d };
                    if (!string.IsNullOrEmpty(standardCode))
                        list = list.Where(q => q.c.ProdStdCode.Equals(standardCode));
                    if (!string.IsNullOrEmpty(productName))
                        list = list.Where(q => q.c.ProdName.Contains(productName));
                    if (!string.IsNullOrEmpty(orderNo))
                        list = list.Where(q => q.a.OrderNo.Equals(orderNo));
                    if (!string.IsNullOrEmpty(seqNo))
                        list = list.Where(q => q.a.SeqNo.Equals(seqNo));
                    if (!string.IsNullOrEmpty(jobDetailType))
                        list = list.Where(q => q.a.JobDetailType.Equals(jobDetailType));
                    if (!string.IsNullOrEmpty(status))
                        list = list.Where(q => q.a.Status.Equals(status));
                    if (!string.IsNullOrEmpty(serialNo))
                        list = list.Where(q => q.d.SerialNum.Contains(serialNo));
                    if (insertDate != null)
                        list = list.Where(q => q.a.InsertDate == insertDate.Value);
                    foreach (var item in list)
                    {
                        retList.Add(new VisionResult(item.a)
                        {
                            ProdStdCode = item.c == null ? null : item.c.ProdStdCode,
                            ProductName = item.c == null ? null : item.c.ProdName,
                            SerialNumber = item.d == null ? null : item.d.SerialNum
                        });
                    }
                }
                return retList.FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.InfoFormat("VisionResultController SelectLocalSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("VisionResultController SelectLocalSingle Exception", ex);
            }
            return retList.FirstOrDefault();
        }
        public static bool InsertLocal(Local.Dmn_VisionResult item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    //log.InfoFormat("VisionResultController InsertLocal by {0}, Barcode : {1}, Decoded String : {2}", item.InsertUser, item.DecodedBarcode , item.Read_OCR);
                    //string InsQry = string.Format("INSERT INTO Dmn_VisionResult ([OrderNo], [SeqNo], [JobDetailType], [DecodedBarcode], [Read_OCR], [Grade_Barcode], [FilePath], [CameraIndex]" +
                    //                    ", [Status], [Reserved1], [Reserved2], [Reserved3], [Reserved4], [Reserved5], [InsertUser], [InsertDate], [UpdateUser], [UpdateDate]) " +
                    //                    "VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}'," +
                    //                    " '{17}')", item.OrderNo, item.SeqNo, item.JobDetailType, item.DecodedBarcode, item.Read_OCR, item.Grade_Barcode, item.FilePath, item.CameraIndex,
                    //                    item.Status, item.Reserved1, item.Reserved2, item.Reserved3, item.Reserved4, item.Reserved5, item.InsertUser, item.InsertDate?.ToString("yyyy-MM-dd HH:mm:ss.fff")
                    //                   , item.UpdateUser, item.UpdateDate?.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    //db.ExecuteQuery(InsQry);

                    string InsQry = "INSERT INTO Dmn_VisionResult ([OrderNo], [SeqNo], [JobDetailType], [DecodedBarcode], [Read_OCR], [Grade_Barcode], [FilePath], [CameraIndex], [Status], [Reserved1], [Reserved2], [Reserved3], [Reserved4], [Reserved5], [InsertUser], [InsertDate], [UpdateUser], [UpdateDate]) " +
                                    "VALUES (@OrderNo, @SeqNo, @JobDetailType, @DecodedBarcode, @ReadOCR, @BarcodeGrade, @FilePath, @CameraIndex, @Status, @Reserved1, @Reserved2, @Reserved3, @Reserved4, @Reserved5, @InsertUser, @InsertDate, @UpdateUser, @UpdateDate)";


                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@OrderNo", item.OrderNo),
                        new SqlParameter("@SeqNo", item.SeqNo),
                        new SqlParameter("@JobDetailType", item.JobDetailType),
                        new SqlParameter("@DecodedBarcode", item.DecodedBarcode),
                        new SqlParameter("@ReadOCR", item.Read_OCR),
                        new SqlParameter("@BarcodeGrade", item.Grade_Barcode),
                        new SqlParameter("@FilePath", item.FilePath),
                        new SqlParameter("@CameraIndex", item.CameraIndex),
                        new SqlParameter("@Status", item.Status),
                        new SqlParameter("@Reserved1", item.Reserved1 ?? (object)DBNull.Value),
                        new SqlParameter("@Reserved2", item.Reserved2 ?? (object)DBNull.Value),
                        new SqlParameter("@Reserved3", item.Reserved3 ?? (object)DBNull.Value),
                        new SqlParameter("@Reserved4", item.Reserved4 ?? (object)DBNull.Value),
                        new SqlParameter("@Reserved5", item.Reserved5 ?? (object)DBNull.Value),
                        new SqlParameter("@InsertUser", item.InsertUser),
                        new SqlParameter("@InsertDate", item.InsertDate != null ? item.InsertDate?.ToString("yyyy-MM-dd HH:mm:ss.fff") : null),
                        new SqlParameter("@UpdateUser", item.UpdateUser ?? (object)DBNull.Value),
                        new SqlParameter("@UpdateDate", item.UpdateDate != null ? item.UpdateDate?.ToString("yyyy-MM-dd HH:mm:ss.fff") : (object)DBNull.Value)
                    };

                    db.Database.ExecuteSqlCommand(InsQry, parameters);


                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("VisionResultController InsertLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("VisionResultController InsertLocal Exception", ex);
                return false;
            }
        }
        public static bool InsertLocalQuery(string orderNo, string seqNo, string readOCR, string barcodeGrade, string filePath, string cameraIndex,
            string status, string jobDetailType, string decodedBarcode, string insertdate, string insertUser, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("VisionResultController InsertLocal by {0}, Barcode : {1}, Decoded String : {2}", insertUser, decodedBarcode, readOCR);
                    string InsQry = "INSERT INTO Dmn_VisionResult ([OrderNo], [SeqNo], [JobDetailType], [DecodedBarcode], [Read_OCR], [Grade_Barcode], [FilePath], [CameraIndex], [Status], [InsertUser], [InsertDate]) " +
                                    "VALUES (@OrderNo, @SeqNo, @JobDetailType, @DecodedBarcode, @ReadOCR, @BarcodeGrade, @FilePath, @CameraIndex, @Status, @InsertUser, @InsertDate)";

                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@OrderNo", orderNo),
                        new SqlParameter("@SeqNo", seqNo),
                        new SqlParameter("@JobDetailType", jobDetailType),
                        new SqlParameter("@DecodedBarcode", decodedBarcode),
                        new SqlParameter("@ReadOCR", readOCR),
                        new SqlParameter("@BarcodeGrade", barcodeGrade),
                        new SqlParameter("@FilePath", filePath),
                        new SqlParameter("@CameraIndex", cameraIndex),
                        new SqlParameter("@Status", status),
                        new SqlParameter("@InsertUser", insertUser),
                        new SqlParameter("@InsertDate", insertdate)
                    };

                    db.Database.ExecuteSqlCommand(InsQry, parameters);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("VisionResultController InsertLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("VisionResultController InsertLocal Exception", ex);
                return false;
            }
        }
        public static bool InsertLocalQuery(string orderNo, string seqNo, string readOCR, string barcodeGrade, string filePath, string cameraIndex,
            string status, string jobDetailType, string decodedBarcode, string insertdate, string insertUser, string pharmaCode, string weight, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("VisionResultController InsertLocal by {0}, Barcode : {1}, Decoded String : {2}", insertUser, decodedBarcode, readOCR);
                    string InsQry = "INSERT INTO Dmn_VisionResult ([OrderNo], [SeqNo], [JobDetailType], [DecodedBarcode], [Read_OCR], [Grade_Barcode], [FilePath], [CameraIndex], [Status], [InsertUser], [InsertDate], [PharmaCode], [Weight]) " +
                                    "VALUES (@OrderNo, @SeqNo, @JobDetailType, @DecodedBarcode, @ReadOCR, @BarcodeGrade, @FilePath, @CameraIndex, @Status, @InsertUser, @InsertDate, @PharmaCode ,@Weight)";

                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@OrderNo", orderNo),
                        new SqlParameter("@SeqNo", seqNo),
                        new SqlParameter("@JobDetailType", jobDetailType),
                        new SqlParameter("@DecodedBarcode", decodedBarcode),
                        new SqlParameter("@ReadOCR", readOCR),
                        new SqlParameter("@BarcodeGrade", barcodeGrade),
                        new SqlParameter("@FilePath", filePath),
                        new SqlParameter("@CameraIndex", cameraIndex),
                        new SqlParameter("@Status", status),
                        new SqlParameter("@InsertUser", insertUser),
                        new SqlParameter("@InsertDate", insertdate),
                        new SqlParameter("@PharmaCode", pharmaCode),
                        new SqlParameter("@Weight", weight),
                    };

                    db.Database.ExecuteSqlCommand(InsQry, parameters);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("VisionResultController InsertLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("VisionResultController InsertLocal Exception", ex);
                return false;
            }
        }
        public static bool UpdateLocal(Local.Dmn_VisionResult item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("VisionResultController UpdateLocal by {0}, {1}", item.UpdateUser, item.ToString());
                    var tmp = db.Dmn_VisionResult.First(q => q.InsertDate == item.InsertDate);
                    tmp.WriteExistings(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("VisionResultController UpdateLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("VisionResultController UpdateLocal Exception", ex);
                return false;
            }
        }
        public static bool UpdateStatusLocal(string status, DateTime inspectTime, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    var tmp = db.Dmn_VisionResult.FirstOrDefault(q => q.InsertDate == inspectTime);
                    tmp.Status = status;
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("VisionResultController UpdateStatusLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("VisionResultController UpdateStatusLocal Exception", ex);
                return false;
            }
        }
        public static bool UpdateStatusLocalQuery(string status, DateTime inspectTime, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    string sql = @"UPDATE Dmn_VisionResult SET Status = @Status Where InsertDate = @InsertDate";
                    SqlParameter[] param = new SqlParameter[]
                    {
                        new SqlParameter("@Status", status),
                        new SqlParameter("@InsertDate", inspectTime.ToString("yyyyMMdd HH:mm:ss.fff"))
                    };
                    db.Database.ExecuteSqlCommand(sql, param);
                    log.InfoFormat("VisionResultController UpdateStatusLocalQuery : {0}, {1}, {2}", sql, status, inspectTime);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("VisionResultController UpdateStatusLocalQuery Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("VisionResultController UpdateStatusLocalQuery Exception", ex);
                return false;
            }
        }
        public static bool UpdateWeightLocalQuery(DateTime inspectTime, string weight,bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    string sql = @"UPDATE Dmn_VisionResult SET Weight = @Weight Where InsertDate = @InsertDate";
                    SqlParameter[] param = new SqlParameter[]
                    {
                        new SqlParameter("@Weight", weight),
                        new SqlParameter("@InsertDate", inspectTime.ToString("yyyyMMdd HH:mm:ss.fff"))
                    };
                    db.Database.ExecuteSqlCommand(sql, param);
                    log.InfoFormat("VisionResultController UpdateWeightLocalQuery : {0}, {1}, {2}", sql, weight, inspectTime);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("VisionResultController UpdateWeightLocalQuery Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("VisionResultController UpdateWeightLocalQuery Exception", ex);
                return false;
            }
        }
        public static bool DeleteLocalAll(string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("VisionResultController DeleteLocalAll by {0}", userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_VisionResult");
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("VisionResultController DeleteLocalAll Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("VisionResultController DeleteLocalAll Exception", ex);
                return false;
            }

        }
        public static bool DeleteLocalSingle(string standardCode, string serialnumber, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("VisionResultController DeleteLocalSingle {0} {1} by {2}", standardCode, serialnumber, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_VisionResult where ProdStdCode = {0} AND SerialNum = {1}", standardCode, serialnumber);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("VisionResultController DeleteLocalSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("VisionResultController DeleteLocalSingle Exception", ex);
                return false;
            }
        }
        public static bool DeleteLocalByOrder(string orderNo, string seqNo, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("VisionResultController DeleteLocalByOrder {0} {1} by {2}", orderNo, seqNo, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_VisionResult where OrderNo = {0} AND SeqNo = {1}", orderNo, seqNo);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("VisionResultController DeleteLocalByOrder Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("VisionResultController DeleteLocalByOrder Exception", ex);
                return false;
            }
        }
        public static bool DeleteLocalByCompleteDate(DateTime dt, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    db.Database.CommandTimeout = 300;
                    log.InfoFormat("VisionResultController DeleteLocalByCompleteDate {0} by {1}", dt, userID);
                    string sql = string.Format(@"
                                                DELETE FROM Dmn_VisionResult
                                                WHERE OrderNo IN (
                                                    SELECT TOP 100000 A.OrderNo
                                                    FROM Dmn_VisionResult AS A
                                                    JOIN Dmn_JobOrder_PM AS B 
                                                    ON A.OrderNo = B.OrderNo AND A.SeqNo = B.SeqNo
                                                    WHERE B.CompleteDate < '{0}'
                                                )
                                            ", dt.ToString("yyyy-MM-dd"));
                    db.Database.ExecuteSqlCommand(sql);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("VisionResultController DeleteLocalByCompleteDate Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("VisionResultController DeleteLocalByCompleteDate Exception", ex);
                return false;
            }
        }
        public static bool DeleteLocalCancelByInsertDate(DateTime dt, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    db.Database.CommandTimeout = 300;
                    log.InfoFormat("VisionResultController DeleteLocalCancelByInsertDate {0} by {1}", dt, userID);
                    string sql = string.Format(@"
                                                DELETE FROM Dmn_VisionResult
                                                WHERE OrderNo IN (
                                                    SELECT TOP 100000 A.OrderNo
                                                    FROM Dmn_VisionResult AS A
                                                    JOIN Dmn_JobOrder_PM AS B 
                                                    ON A.OrderNo = B.OrderNo AND A.SeqNo = B.SeqNo
                                                    WHERE B.InsertDate < '{0}' AND B.JobStatus = 'CC'
                                                )
                                            ", dt.ToString("yyyy-MM-dd"));
                    db.Database.ExecuteSqlCommand(sql);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("VisionResultController DeleteLocalCancelByInsertDate Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("VisionResultController DeleteLocalCancelByInsertDate Exception", ex);
                return false;
            }
        }
        public static List<VisionResult> SelectServer(string plantCode, string machineID, string standardCode, string productName, string orderNo, string seqNo,
            string jobDetailType, string status, string serialNo, int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            List<VisionResult> retList = new List<VisionResult>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_VisionResult
                               join b in db.Dmn_JobOrder_M on new { a.OrderNo, a.SeqNo } equals new { b.OrderNo, b.SeqNo } into bb
                               from b in bb.DefaultIfEmpty()
                               join c in db.Dmn_Product_M on b.ProdCode equals c.ProdCode into cc
                               from c in cc.DefaultIfEmpty()
                               join d in db.Dmn_SerialPool on a.InsertDate equals d.InsertDate into dd
                               from d in dd.DefaultIfEmpty()
                               select new { a, b, c, d };
                    if (!string.IsNullOrEmpty(plantCode))
                        list = list.Where(q => q.a.PlantCode.Equals(plantCode));
                    if (!string.IsNullOrEmpty(machineID))
                        list = list.Where(q => q.a.MachineID.Equals(machineID));
                    if (!string.IsNullOrEmpty(standardCode))
                        list = list.Where(q => q.c.ProdStdCode.Equals(standardCode));
                    if (!string.IsNullOrEmpty(productName))
                        list = list.Where(q => q.c.ProdName.Contains(productName));
                    if (!string.IsNullOrEmpty(orderNo))
                        list = list.Where(q => q.a.OrderNo.Equals(orderNo));
                    if (!string.IsNullOrEmpty(seqNo))
                        list = list.Where(q => q.a.SeqNo.Equals(seqNo));
                    if (!string.IsNullOrEmpty(jobDetailType))
                        list = list.Where(q => q.a.JobDetailType.Equals(jobDetailType));
                    if (!string.IsNullOrEmpty(status))
                        list = list.Where(q => q.a.Status.Contains(status));
                    if (!string.IsNullOrEmpty(serialNo))
                        list = list.Where(q => q.d.SerialNum.Contains(serialNo));
                    foreach (var item in list)
                    {
                        retList.Add(new VisionResult(item.a));
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
                log.InfoFormat("VisionResultController SelectServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("VisionResultController SelectServer Exception", ex);
            }
            return retList;
        }
        public static VisionResult SelectServerSingle(string plantCode, string machineID, string standardCode, string productName, string orderNo, string seqNo,
            string jobDetailType, string status, string serialNo, bool RaiseException = false)
        {
            List<VisionResult> retList = new List<VisionResult>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_VisionResult
                               join b in db.Dmn_JobOrder_M on new { a.OrderNo, a.SeqNo } equals new { b.OrderNo, b.SeqNo } into bb
                               from b in bb.DefaultIfEmpty()
                               join c in db.Dmn_Product_M on b.ProdCode equals c.ProdCode into cc
                               from c in cc.DefaultIfEmpty()
                               join d in db.Dmn_SerialPool on a.InsertDate equals d.InsertDate into dd
                               from d in dd.DefaultIfEmpty()
                               select new { a, b, c, d };
                    if (!string.IsNullOrEmpty(plantCode))
                        list = list.Where(q => q.a.PlantCode.Equals(plantCode));
                    if (!string.IsNullOrEmpty(machineID))
                        list = list.Where(q => q.a.MachineID.Equals(machineID));
                    if (!string.IsNullOrEmpty(standardCode))
                        list = list.Where(q => q.c.ProdStdCode.Contains(standardCode));
                    if (!string.IsNullOrEmpty(productName))
                        list = list.Where(q => q.c.ProdName.Contains(productName));
                    if (!string.IsNullOrEmpty(orderNo))
                        list = list.Where(q => q.a.OrderNo.Contains(orderNo));
                    if (!string.IsNullOrEmpty(seqNo))
                        list = list.Where(q => q.a.SeqNo.Contains(seqNo));
                    if (!string.IsNullOrEmpty(jobDetailType))
                        list = list.Where(q => q.a.JobDetailType.Equals(jobDetailType));
                    if (!string.IsNullOrEmpty(status))
                        list = list.Where(q => q.a.Status.Contains(status));
                    if (!string.IsNullOrEmpty(serialNo))
                        list = list.Where(q => q.d.SerialNum.Contains(serialNo));
                    foreach (var item in list)
                    {
                        retList.Add(new VisionResult(item.a));
                    }
                }
                return retList.FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.InfoFormat("VisionResultController SelectServerSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("VisionResultController SelectServerSingle Exception", ex);
            }
            return retList.FirstOrDefault();
        }
        public static bool InsertServer(DSM.Dmn_VisionResult item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("VisionResultController InsertServer by {0}, {1}", item.InsertUser, item.ToString());
                    db.Dmn_VisionResult.AddOrUpdate(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("VisionResultController InsertServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("VisionResultController InsertServer Exception", ex);
                return false;
            }
        }
        public static bool InsertServer(List<DSM.Dmn_VisionResult> item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("VisionResultController AddOrUpdate List Start");
                    for (int i = 0; i < item.Count; i++)
                    {
                        db.Dmn_VisionResult.AddOrUpdate(item[i]);
                    }
                    db.SaveChanges();
                    log.InfoFormat("VisionResultController AddOrUpdate List End");
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("VisionResultController InsertServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("VisionResultController InsertServer Exception", ex);
                return false;
            }
        }
        public static bool ReportServer(List<DSM.Dmn_VisionResult> item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("VisionResultController Report Start");
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_VisionResult where PlantCode = {0} and MachineID = {1} and OrderNo = {2} and SeqNo = {3}", item[0].PlantCode,
                        item[0].MachineID, item[0].OrderNo, item[0].SeqNo);
                    db.Dmn_VisionResult.AddRange(item);
                    db.SaveChanges();
                    log.InfoFormat("VisionResultController Report End");
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("VisionResultController Report Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("VisionResultController Report Exception", ex);
                return false;
            }
        }
        public static bool UpdateServer(DSM.Dmn_VisionResult item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("VisionResultController UpdateServer by {0}, {1}", item.UpdateUser, item.ToString());
                    var tmp = db.Dmn_VisionResult.First(q => q.PlantCode.Equals(item.PlantCode) && q.InsertDate.Equals(item.InsertDate));
                    tmp.WriteExistings(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("VisionResultController UpdateServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("VisionResultController UpdateServer Exception", ex);
                return false;
            }
        }
        public static bool DeleteServerAll(string plantCode, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("VisionResultController DeleteServerAll {0} by {1}", plantCode, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_VisionResult where PlantCode = {0}", plantCode);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("VisionResultController DeleteServerAll Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("VisionResultController DeleteServerAll Exception", ex);
                return false;
            }

        }
        public static bool DeleteServerSingle(string plantCode, string standardCode, string serialnumber, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("VisionResultController DeleteServerSingle {0} {1} {2} by {3}", plantCode, standardCode, serialnumber, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_VisionResult where PlantCode = {0} AND ProdStdCode = {1} AND SerialNum = {2}", plantCode, standardCode, serialnumber);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("VisionResultController DeleteServerSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("VisionResultController DeleteServerSingle Exception", ex);
                return false;
            }
        }
        public static bool DeleteServerByOrder(string plantCode, string standardCode, string orderNo, string seqNo, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("VisionResultController DeleteServerByOrder {0} {1} {2} {3} by {4}", plantCode, standardCode, orderNo, seqNo, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_VisionResult where PlantCode = {0} ProdStdCode = {1} AND OrderNo = {2} AND SeqNo = {3}", plantCode, standardCode, orderNo, seqNo);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("VisionResultController DeleteServerByOrder Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("VisionResultController DeleteServerByOrder Exception", ex);
                return false;
            }
        }
        public static bool UpdateStatus(string plantCode, string orderNo, string seqNo, string serialNum, string status, string jobDetailType, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("VisionResultController UpdateStatus {0} {1} {2} {3} by {4}", plantCode, orderNo + seqNo, serialNum, status, userID);
                    db.Database.ExecuteSqlCommand("Update Dmn_VisionResult SET Status ={0} where PlantCode ={1} AND OrderNo ={2} AND SeqNo ={3} AND JobDetailType ={4} AND SerialNum ={5}", status, plantCode, orderNo, seqNo, jobDetailType, serialNum);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("VisionResultController UpdateStatus Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("VisionResultController UpdateStatus Exception", ex);
                return false;
            }
        }
    }
}
