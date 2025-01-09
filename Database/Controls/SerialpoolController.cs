using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using System.Data.Entity.Migrations;
using DominoFunctions.ExtensionMethod;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Threading.Tasks;
using System.Globalization;

namespace DominoDatabase.Controls
{
    public class SerialpoolController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static long TotalPageCount { get; set; }
        public static long TotalCount { get; set; }

        public static readonly DateTime SqlMinValue = new DateTime(1990, 1, 1, 0, 0, 1, 000);
        public static readonly DateTime SqlMaxValue = new DateTime(2199, 12, 31, 23, 59, 59, 999);

		public static List<SerialPool> SelectLocal(string standardCode,string productCode , string productName, string serialNum, string jobDetailType, string orderNo, string seqNo, string lot,
            string resourceType, string status, DateTime? useDateStart, DateTime? useDateEnd, bool? use, int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            List<SerialPool> retList = new List<SerialPool>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_SerialPool
                               join b in db.Dmn_Product_M on a.ProdStdCode equals b.ProdStdCode into bb
                               from b in bb.DefaultIfEmpty()
                               join c in db.Dmn_VisionResult on a.InspectedDate equals c.InsertDate into cc
                               from c in cc.DefaultIfEmpty()
                               join d in db.Dmn_JobOrder_M on new { a.OrderNo, a.SeqNo } equals new { d.OrderNo, d.SeqNo } into dd
                               from d in dd.DefaultIfEmpty()
                               select new { a, b, c, d };
                    if (!string.IsNullOrEmpty(standardCode))
                        list = list.Where(q => q.a.ProdStdCode.Contains(standardCode));
                    if (!string.IsNullOrEmpty(productCode))
                        list = list.Where(q => q.b.ProdCode.Contains(productCode));
                    if (!string.IsNullOrEmpty(productName))
                        list = list.Where(q => q.b.ProdName.Contains(productName));
                    if (!string.IsNullOrEmpty(serialNum))
                        list = list.Where(q => q.a.SerialNum.Contains(serialNum));
                    if (!string.IsNullOrEmpty(jobDetailType))
                        list = list.Where(q => q.a.JobDetailType.Contains(jobDetailType));
                    if (!string.IsNullOrEmpty(orderNo))
                        list = list.Where(q => q.a.OrderNo.Contains(orderNo));
                    if (!string.IsNullOrEmpty(seqNo))
                        list = list.Where(q => q.a.SeqNo.Contains(seqNo));
                    if (!string.IsNullOrEmpty(lot))
                        list = list.Where(q => q.d.LotNo.Contains(lot));
                    if (!string.IsNullOrEmpty(resourceType))
                        list = list.Where(q => q.a.ResourceType.Equals(resourceType));
                    if (!string.IsNullOrEmpty(status))
                        list = list.Where(q => q.a.Status.Contains(status));
                    if (useDateStart != null)
                        list = list.Where(q => q.a.UseDate >= useDateStart.Value);
                    if (useDateEnd != null)
                        list = list.Where(q => q.a.UseDate >= useDateEnd.Value);
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Y") : q.a.UseYN.Equals("N"));
                    foreach (var item in list)
                    {
                        retList.Add(new SerialPool(item.a));
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
                log.InfoFormat("SerialpoolController SelectLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController SelectLocal Exception", ex);
            }
            return retList;
        }
        public static SerialPool SelectLocalSingle(string standardCode, string productCode, string productName, string serialNum, string jobDetailType, string orderNo, string seqNo, string lot,
            string resourceType, string status, DateTime? useDateStart, DateTime? useDateEnd, bool? use, int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            List<SerialPool> retList = new List<SerialPool>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_SerialPool
                               join b in db.Dmn_Product_M on a.ProdStdCode equals b.ProdStdCode into bb
                               from b in bb.DefaultIfEmpty()
                               join c in db.Dmn_VisionResult on a.InspectedDate equals c.InsertDate into cc
                               from c in cc.DefaultIfEmpty()
                               join d in db.Dmn_JobOrder_M on new { a.OrderNo, a.SeqNo } equals new { d.OrderNo, d.SeqNo } into dd
                               from d in dd.DefaultIfEmpty()
                               select new { a, b, c, d };
                    if (!string.IsNullOrEmpty(standardCode))
                        list = list.Where(q => q.a.ProdStdCode.Equals(standardCode));
                    if (!string.IsNullOrEmpty(productCode))
                        list = list.Where(q => q.b.ProdCode.Contains(productCode));
                    if (!string.IsNullOrEmpty(productName))
                        list = list.Where(q => q.b.ProdName.Contains(productName));
                    if (!string.IsNullOrEmpty(serialNum))
                        list = list.Where(q => q.a.SerialNum.Equals(serialNum));
                    if (!string.IsNullOrEmpty(jobDetailType))
                        list = list.Where(q => q.a.JobDetailType.Contains(jobDetailType));
                    if (!string.IsNullOrEmpty(orderNo))
                        list = list.Where(q => q.a.OrderNo.Contains(orderNo));
                    if (!string.IsNullOrEmpty(seqNo))
                        list = list.Where(q => q.a.SeqNo.Contains(seqNo));
                    if (!string.IsNullOrEmpty(lot))
                        list = list.Where(q => q.d.LotNo.Contains(lot));
                    if (!string.IsNullOrEmpty(resourceType))
                        list = list.Where(q => q.a.ResourceType.Equals(resourceType));
                    if (!string.IsNullOrEmpty(status))
                        list = list.Where(q => q.a.Status.Contains(status));
                    if (useDateStart != null)
                        list = list.Where(q => q.a.UseDate >= useDateStart.Value);
                    if (useDateEnd != null)
                        list = list.Where(q => q.a.UseDate >= useDateEnd.Value);
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Y") : q.a.UseYN.Equals("N"));
                    foreach (var item in list)
                    {
                        retList.Add(new SerialPool(item.a));
                    }
                }
                return retList.FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController SelectLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController SelectLocal Exception", ex);
            }
            return retList.FirstOrDefault();
        }
        public static SerialPool SelectLocalSingleByQuery(string standardCode, string serialNumber, string orderNo, string seqNo, string jobDetailType, string status, bool RaiseException = false)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    string sql = "SELECT TOP 1 * FROM Dmn_SerialPool";
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    List<string> conditions = new List<string>();
                    if (!string.IsNullOrEmpty(standardCode))
                    {
                        conditions.Add("ProdStdCode = @StandardCode");
                        parameters.Add(new SqlParameter("@StandardCode", standardCode));
                    }
                    if (!string.IsNullOrEmpty(serialNumber))
                    {
                        conditions.Add("SerialNum = @SerialNum");
                        parameters.Add(new SqlParameter("@SerialNum", serialNumber));
                    }
                    if (!string.IsNullOrEmpty(orderNo))
                    {
                        conditions.Add("OrderNo = @OrderNo");
                        parameters.Add(new SqlParameter("@OrderNo", orderNo));
                    }
                    if (!string.IsNullOrEmpty(seqNo))
                    {
                        conditions.Add("SeqNo = @SeqNo");
                        parameters.Add(new SqlParameter("@SeqNo", seqNo));
                    }
                    if (!string.IsNullOrEmpty(jobDetailType))
                    {
                        conditions.Add("JobDetailType = @JobDetailType");
                        parameters.Add(new SqlParameter("@JobDetailType", jobDetailType));
                    }
                    if (!string.IsNullOrEmpty(status))
                    {
                        conditions.Add("Status = @Status");
                        parameters.Add(new SqlParameter("@Status", status));
                    }

                    if (conditions.Count > 0)
                    {
                        sql += " WHERE ";
                        sql += $" {string.Join(" AND ", conditions)}";
                    }
                    else 
                        return null;
                    var list = db.Database.SqlQuery<Local.Dmn_SerialPool>(sql, parameters.ToArray()).ToList();

                    if (list.Count > 0)
                        return new SerialPool(list.First());
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialPoolController SelectLocalSingleByQuery Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialPoolController SelectLocalSingleByQuery Exception", ex);
                return null;
            }
        }
        public static List<SerialPool> SelectLocalByQuery(string standardCode, string orderNo, string seqNo, string jobDetailType, string status, bool RaiseException = false)
        {
            List<SerialPool> retList = new List<SerialPool>();
            try
            {
                var serialPoolProps = typeof(SerialPool).GetProperties();
                var entityProps = typeof(Local.Dmn_SerialPool).GetProperties();
                var commonPropertyNames = serialPoolProps.Select(q => q.Name).Intersect(entityProps.Select(q => q.Name)).ToList();
                var sqlProps = string.Join(", ", commonPropertyNames);
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    string sql = "SELECT * FROM Dmn_SerialPool";
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    List<string> conditions = new List<string>();
                    if (!string.IsNullOrEmpty(standardCode))
                    {
                        conditions.Add($"ProdStdCode = @StandardCode");
                        parameters.Add(new SqlParameter("@StandardCode", standardCode));
                    }
                    if (!string.IsNullOrEmpty(orderNo))
                    {
                        conditions.Add($"OrderNo = @OrderNo");
                        parameters.Add(new SqlParameter("@OrderNo", orderNo));
                    }
                    if (!string.IsNullOrEmpty(seqNo))
                    {
                        conditions.Add($"SeqNo = @SeqNo");
                        parameters.Add(new SqlParameter("@SeqNo", seqNo));
                    }
                    if (!string.IsNullOrEmpty(jobDetailType))
                    {
                        conditions.Add($"JobDetailType = @JobDetailType");
                        parameters.Add(new SqlParameter("@JobDetailType", jobDetailType));
                    }
                    if (!string.IsNullOrEmpty(status))
                    {
                        conditions.Add($"Status = @Status");
                        parameters.Add(new SqlParameter("@Status", status));
                    }

                    if (conditions.Count > 0)
                    {
                        sql += " WHERE ";
                        sql += "(";
                        sql += string.Join(" AND ", conditions);
                        sql += ")";
                    }

                    retList = db.Database.SqlQuery<SerialPool>(sql.ToString(), parameters.ToArray()).ToList();
                }
                return retList;
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialPoolController SelectLocalByQuery Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialPoolController SelectLocalByQuery Exception", ex);
                return new List<SerialPool>();
            }
        }
        public static List<SerialPool> SelectLocalNoParent(string orderNo, string seqNo, string detailType, bool RaiseException = false)
        {
            List<SerialPool> retList = new List<SerialPool>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from sp in db.Dmn_SerialPool
                               join children in db.Dmn_ReadBarcode on new { stdCode = sp.ProdStdCode, serial = sp.SerialNum } equals new { stdCode = children.ParentProdStdCode, serial = children.ParentSerialNum } into bb
                               from children in bb.DefaultIfEmpty()
                               join parent in db.Dmn_ReadBarcode on new { stdCode = children.ParentProdStdCode, serial = children.ParentSerialNum } equals new { stdCode = parent.ProdStdCode, serial = parent.SerialNum } into cc
                               from parent in cc.DefaultIfEmpty()
                               where sp.OrderNo.Equals(orderNo) && sp.SeqNo.Equals(seqNo) && sp.JobDetailType.Equals(detailType) && parent == null
                               && sp.UseYN.Equals("Y") && (sp.Status == "PA" || sp.Status.StartsWith("Sample"))
                               select new { sp, children, parent };
                    
                    var spGroup = list.GroupBy(q => q.sp.SerialNum);
                    foreach (var item in spGroup)
                    {
                        var noParent = list.FirstOrDefault(q => q.sp.SerialNum.Equals(item.Key));
                        if (noParent.sp != null)
                        {
                            retList.Add(new SerialPool(noParent.sp));
                        }
                    }

                }
                return retList;
            }
            catch (Exception ex)
            {
                log.InfoFormat("ReadBarcodeController SelectLocalNoParent Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ReadBarcodeController SelectLocalNoParent Exception", ex);
            }
            return retList;
        }
        public static bool CheckExist(string standardCode, string serial, bool RaiseException = true)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    string sql = "SELECT TOP 1 * FROM Dmn_SerialPool";
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    List<string> conditions = new List<string>();

                    if (!string.IsNullOrEmpty(standardCode))
                    {
                        conditions.Add($"ProdStdCode = @ProdStdCode");
                        parameters.Add(new SqlParameter("@ProdStdCode", standardCode));
                    }
                    conditions.Add($"SerialNum = @SerialNum");
                    parameters.Add(new SqlParameter("@SerialNum", serial));

                    if (conditions.Count > 0)
                    {
                        sql += " WHERE ";
                        sql += "(";
                        sql += string.Join(" AND ", conditions);
                        sql += ")";
                    }

                    var retList = db.Database.SqlQuery<SerialPool>(sql.ToString(), parameters.ToArray()).ToList();
                    return retList.Count > 0;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController CheckExist Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController CheckExist Exception", ex);
                return false;
            }
        }
        public static bool CheckExistQuery(string standardCode, string serial, bool RaiseException = true)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = db.Select<Local.Dmn_SerialPool>(string.Format("SELECT * FROM Dmn_SerialPool where ProdStdCode = '{0}' AND SerialNum = '{1}'", standardCode, serial));
                    log.InfoFormat("CheckExistQuery {0}", list.Count() > 0);
                    return list.Count() > 0;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController CheckExistQuery Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController CheckExistQuery Exception", ex);
                return false;
            }
        }
        public static bool GetLastNumberByFixedText(string standardCode, string fixedText, bool RaiseException = true)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = db.Select<Local.Dmn_SerialPool>(string.Format("SELECT TOP 1 * FROM Dmn_SerialPool where ProdStdCode = '{0}' AND SerialNum LIKE '{1}%'", standardCode, fixedText));
                    log.InfoFormat("GetLastNumberByFixedText {0}", list.Count() > 0);
                    return list.Count() > 0;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController GetLastNumberByFixedText Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController GetLastNumberByFixedText Exception", ex);
                return false;
            }
        }
        public static bool CheckExistServer(string standardCode, string serial, bool RaiseException = true)
        {
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_SerialPool
                               where  a.ProdStdCode.Equals(standardCode) && a.SerialNum.Equals(serial)
                               select a;
                    return list.Count() != 0;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController CheckExist Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController CheckExist Exception", ex);
                return false;
            }
        }
        public static int GetCountLocal(string standardCode, string productCode, string jobDetailType, string orderNo, string seqNo,
            string lot, string resourceType, string status, bool? use, bool RaiseException = false)
        {
            List<SerialPool> retList = new List<SerialPool>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_SerialPool
                               join b in db.Dmn_Product_M on a.ProdStdCode equals b.ProdStdCode into bb
                               from b in bb.DefaultIfEmpty()
                               join c in db.Dmn_JobOrder_M on new { a.OrderNo, a.SeqNo } equals new { c.OrderNo, c.SeqNo } into cc
                               from c in cc.DefaultIfEmpty()
                               select new { a, b, c };
                    if (!string.IsNullOrEmpty(standardCode))
                        list = list.Where(q => q.a.ProdStdCode.Contains(standardCode));
                    if (!string.IsNullOrEmpty(productCode))
                        list = list.Where(q => q.b.ProdCode.Contains(productCode));
                    if (!string.IsNullOrEmpty(jobDetailType))
                        list = list.Where(q => q.a.JobDetailType.Contains(jobDetailType));
                    if (!string.IsNullOrEmpty(orderNo))
                        list = list.Where(q => q.a.OrderNo.Contains(orderNo));
                    if (!string.IsNullOrEmpty(seqNo))
                        list = list.Where(q => q.a.SeqNo.Contains(seqNo));
                    if (!string.IsNullOrEmpty(lot))
                        list = list.Where(q => q.c.LotNo.Contains(lot));
                    if (!string.IsNullOrEmpty(resourceType))
                        list = list.Where(q => q.a.ResourceType.Equals(resourceType));
                    if (!string.IsNullOrEmpty(status))
                        list = list.Where(q => q.a.Status.Contains(status));
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Y") : q.a.UseYN.Equals("N"));
                    foreach (var item in list)
                    {
                        retList.Add(new SerialPool(item.a));
                    }
                }
                return retList.Count();
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController GetCountLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController GetCountLocal Exception", ex);
            }
            return 0;
        }
        public static int GetCountLocalAG(string standardCode, string productCode, string jobDetailType, string orderNo, string seqNo,
            string lot, string resourceType, string barcodeFormat, string barcodeType, string status, bool? use, bool RaiseException = false)
        {
            List<SerialPool> retList = new List<SerialPool>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    string sql = "SELECT * FROM Dmn_SerialPool sp ";
                    sql += "LEFT JOIN Dmn_JobOrder_M jm ";
                    sql += "ON sp.OrderNo = jm.OrderNo AND sp.SeqNo = jm.SeqNo ";
                    sql += "LEFT JOIN Dmn_Product_M pm ";
                    sql += "ON jm.ProdCode = pm.ProdCode ";

                    List<SqlParameter> parameters = new List<SqlParameter>();
                    List<string> conditions = new List<string>();

                    if (!string.IsNullOrEmpty(standardCode))
                    {
                        conditions.Add($"sp.ProdStdCode = @ProdStdCode");
                        parameters.Add(new SqlParameter("@ProdStdCode", standardCode));
                    }
                    if (!string.IsNullOrEmpty(productCode))
                    {
                        conditions.Add($"pm.ProdCode = @ProdCode");
                        parameters.Add(new SqlParameter("@ProdCode", productCode));
                    }
                    if (!string.IsNullOrEmpty(jobDetailType))
                    {
                        conditions.Add($"sp.JobDetailType = @JobDetailType");
                        parameters.Add(new SqlParameter("@JobDetailType", jobDetailType));
                    }
                    if (!string.IsNullOrEmpty(orderNo))
                    {
                        conditions.Add($"jm.OrderNo = @OrderNo");
                        parameters.Add(new SqlParameter("@OrderNo", orderNo));
                    }
                    if (!string.IsNullOrEmpty(seqNo))
                    {
                        conditions.Add($"jm.SeqNo = @SeqNo");
                        parameters.Add(new SqlParameter("@SeqNo", seqNo));
                    }
                    if (!string.IsNullOrEmpty(lot))
                    {
                        conditions.Add($"jm.LotNo = @LotNo");
                        parameters.Add(new SqlParameter("@LotNo", lot));
                    }
                    if (!string.IsNullOrEmpty(resourceType))
                    {
                        conditions.Add($"sp.ResourceType = @ResourceType");
                        parameters.Add(new SqlParameter("@ResourceType", resourceType));
                    }
                    if (!string.IsNullOrEmpty(barcodeFormat))
                    {
                        conditions.Add($"sp.BarcodeDataFormat = @BarcodeDataFormat");
                        parameters.Add(new SqlParameter("@BarcodeDataFormat", barcodeFormat));
                    }
                    if (!string.IsNullOrEmpty(barcodeType))
                    {
                        conditions.Add($"sp.BarcodeType = @BarcodeType");
                        parameters.Add(new SqlParameter("@BarcodeType", barcodeType));
                    }
                    if (!string.IsNullOrEmpty(status))
                    {
                        conditions.Add($"sp.Status = @Status");
                        parameters.Add(new SqlParameter("@Status", status));
                    }
                    if (use != null)
                    {
                        conditions.Add($"UseYN = @UseYN");
                        parameters.Add(new SqlParameter("@UseYN", use.Value ? "Y" : "N"));
                    }

                    if (conditions.Count > 0)
                    {
                        sql += " WHERE ";
                        sql += "(";
                        sql += string.Join(" AND ", conditions);
                        sql += ")";
                    }

                    var list = db.Database.SqlQuery<Local.Dmn_SerialPool>(sql, parameters.ToArray()).ToList();
                    foreach (var item in list)
                    {
                        retList.Add(new SerialPool(item));
                    }
                }
                return retList.Count();
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController GetCountLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController GetCountLocal Exception", ex);
            }
            return 0;
        }
        public static int GetMaxGroupLocal(string standardCode)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_SerialPool
                               where a.ProdStdCode.Equals(standardCode)
                               select a;
                    return list.Count() < 1 ? 0 : list.Max(q => (int)q.idx_Group);
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController GetSerialCount Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return 0;
            }
        }
        public static int GetMaxIndexLocal(string standardCode)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_SerialPool
                               where a.ProdStdCode.Equals(standardCode)
                               select a;
                    return list.Max(q => (int)q.idx_Insert);
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController GetSerialCount Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return 0;
            }
        }
        public static void GetSerialIndex(string standardCode, string lotNo, out int groupIndex, out int index)
        {
            groupIndex = 0;
            index = 0;
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_SerialPool
                               join b in db.Dmn_JobOrder_M on new { a.OrderNo, a.SeqNo } equals new { b.OrderNo, b.SeqNo } into bb
                               from b in bb.DefaultIfEmpty()
                               select new { a, b };
                    if (!string.IsNullOrEmpty(standardCode))
                        list = list.Where(q => q.a.ProdStdCode.Equals(standardCode));
                    if (!string.IsNullOrEmpty(lotNo))
                        list = list.Where(q => q.b.LotNo.Equals(lotNo));
                    groupIndex = list.Count() < 1 || list.Select(q => q.a.idx_Group) == null ? 0 : (int)list.Select(q => q.a.idx_Group).Max();
                    index = list.Count() < 1 || list.Select(q => q.a.idx_Insert) == null ? 0 : (int)list.Select(q => q.a.idx_Insert).Max();
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController GetSerialIndex Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
            }
        }
        public static void GetActiveSerialIndex(string standardCode, string lotNo, out int groupIndex, out int index)
        {
            groupIndex = 0;
            index = 0;
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_SerialPool
                               join b in db.Dmn_JobOrder_M on new { a.OrderNo, a.SeqNo } equals new { b.OrderNo, b.SeqNo } into bb
                               from b in bb.DefaultIfEmpty()
                               join c in db.Dmn_JobOrder_PM on new {a.OrderNo, a.SeqNo } equals new { c.OrderNo, c.SeqNo } into cc
                               from c in cc.DefaultIfEmpty()
                               where c.JobStatus == "AP" || c.JobStatus == "PS" || c.JobStatus == "RU"
                               select new { a, b, c };
                    if (!string.IsNullOrEmpty(standardCode))
                        list = list.Where(q => q.a.ProdStdCode.Equals(standardCode));
                    if (!string.IsNullOrEmpty(lotNo))
                        list = list.Where(q => q.b.LotNo.Equals(lotNo));
                    groupIndex = list.Count() < 1 || list.Select(q => q.a.idx_Group) == null ? 0 : (int)list.Select(q => q.a.idx_Group).Max();
                    index = list.Count() < 1 || list.Select(q => q.a.idx_Insert) == null ? 0 : (int)list.Select(q => q.a.idx_Insert).Max();
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController GetSerialIndex Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
            }
        }
        public static void GetSSCCSerialIndex(string prefix, out int groupIndex, out int index)
        {
            if (string.IsNullOrEmpty(prefix))
                throw new ArgumentException("prefix is empty.");

            groupIndex = 0;
            index = 0;
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_SerialPool
                               where (a.SerialNum.StartsWith(prefix))
                               select new { a };

                    groupIndex = list.Select(q => q.a.idx_Group) == null ? 0 : (int)list.Select(q => q.a.idx_Group).Max();
                    index = list.Select(q => q.a.idx_Insert) == null ? 0 : (int)list.Select(q => q.a.idx_Insert).Max();
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController GetSerialIndex Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
            }
        }
        public static void GetSerialIndexServer(string standardCode, string lotNo, out long groupIndex, out long index)
        {
            groupIndex = 0;
            index = 0;
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_SerialPool
                               join b in db.Dmn_JobOrder_M on new { a.OrderNo, a.SeqNo } equals new { b.OrderNo, b.SeqNo } into bb
                               from b in bb.DefaultIfEmpty()
                               where a.SerialType != "R"
                               select new { a, b };
                    if (!string.IsNullOrEmpty(standardCode))
                        list = list.Where(q => q.a.ProdStdCode.Equals(standardCode));
                    if (!string.IsNullOrEmpty(lotNo))
                        list = list.Where(q => q.b.LotNo.Equals(lotNo));
                    groupIndex = list.Count() < 1 || list.Select(q => q.a.idx_Group) == null ? 0 : (long)list.Select(q => q.a.idx_Group).Max();
                    index = list.Count() < 1 || list.Select(q => q.a.idx_Insert) == null ? 0 : (long)list.Select(q => q.a.idx_Insert).Max();
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController GetSerialIndex Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
            }
        }
		public static void GetSerialIndexServer(string standardCode, DateTime startDateTime, DateTime endDatetime, out long groupIndex, out long index)
		{
			groupIndex = 0;
			index = 0;
			try
			{
				using (DominoDBServer db = new DominoDBServer())
				{
					var list = from a in db.Dmn_SerialPool
							   join b in db.Dmn_JobOrder_M on new { a.OrderNo, a.SeqNo } equals new { b.OrderNo, b.SeqNo } into bb
							   from b in bb.DefaultIfEmpty()
							   where a.SerialType != "R"
							   select new { a, b };
					if (!string.IsNullOrEmpty(standardCode))
						list = list.Where(q => q.a.ProdStdCode.Equals(standardCode));
                    list = list.Where(q => q.a.InsertDate >= startDateTime && q.a.InsertDate <= endDatetime);
					groupIndex = list.Count() < 1 || list.Select(q => q.a.idx_Group) == null ? 0 : (long)list.Select(q => q.a.idx_Group).Max();
					index = list.Count() < 1 || list.Select(q => q.a.idx_Insert) == null ? 0 : (long)list.Select(q => q.a.idx_Insert).Max();
				}
			}
			catch (Exception ex)
			{
				log.InfoFormat("SerialpoolController GetSerialIndex Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
			}
		}
        public static void GetSerialIndexServerMfd(string standardCode, DateTime startDateTime, DateTime endDatetime, out long groupIndex, out long index)
        {
            groupIndex = 0;
            index = 0;
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_SerialPool
                               join b in db.Dmn_JobOrder_M on new { a.OrderNo, a.SeqNo } equals new { b.OrderNo, b.SeqNo } into bb
                               from b in bb.DefaultIfEmpty()
                               where a.SerialType != "R"
                               select new { a, b };
                    if (!string.IsNullOrEmpty(standardCode))
                        list = list.Where(q => q.a.ProdStdCode.Equals(standardCode));
                    list = list.Where(q => q.b.MfdDate.Value >= startDateTime && q.b.MfdDate <= endDatetime);
                    groupIndex = list.Count() < 1 || list.Select(q => q.a.idx_Group) == null ? 0 : (long)list.Select(q => q.a.idx_Group).Max();
                    index = list.Count() < 1 || list.Select(q => q.a.idx_Insert) == null ? 0 : (long)list.Select(q => q.a.idx_Insert).Max();
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController GetSerialIndex Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
            }
        }
        public static void GetSerialIndexServerExp(string standardCode, DateTime startDateTime, DateTime endDatetime, out long groupIndex, out long index)
        {
            groupIndex = 0;
            index = 0;
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_SerialPool
                               join b in db.Dmn_JobOrder_M on new { a.OrderNo, a.SeqNo } equals new { b.OrderNo, b.SeqNo } into bb
                               from b in bb.DefaultIfEmpty()
                               where a.SerialType != "R"
                               select new { a, b };
                    if (!string.IsNullOrEmpty(standardCode))
                        list = list.Where(q => q.a.ProdStdCode.Equals(standardCode));
                    list = list.Where(q => q.b.ExpDate.Value >= startDateTime && q.b.ExpDate <= endDatetime);
                    groupIndex = list.Count() < 1 || list.Select(q => q.a.idx_Group) == null ? 0 : (long)list.Select(q => q.a.idx_Group).Max();
                    index = list.Count() < 1 || list.Select(q => q.a.idx_Insert) == null ? 0 : (long)list.Select(q => q.a.idx_Insert).Max();
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController GetSerialIndex Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
            }
        }

        public static void GetSerialIndexServer(string standardCode, out long groupIndex, out long index)
		{
			groupIndex = 0;
			index = 0;
			try
			{
				using (DominoDBServer db = new DominoDBServer())
				{
					var list = from a in db.Dmn_SerialPool
							   join b in db.Dmn_JobOrder_M on new { a.OrderNo, a.SeqNo } equals new { b.OrderNo, b.SeqNo } into bb
							   from b in bb.DefaultIfEmpty()
							   where a.SerialType != "R"
							   select new { a, b };
					if (!string.IsNullOrEmpty(standardCode))
						list = list.Where(q => q.a.ProdStdCode.Equals(standardCode));
					groupIndex = list.Count() < 1 || list.Select(q => q.a.idx_Group) == null ? 0 : (long)list.Select(q => q.a.idx_Group).Max();
					index = list.Count() < 1 || list.Select(q => q.a.idx_Insert) == null ? 0 : (long)list.Select(q => q.a.idx_Insert).Max();
				}
			}
			catch (Exception ex)
			{
				log.InfoFormat("SerialpoolController GetSerialIndex Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
			}
		}
		public static bool ExtractSerial(string orderNo, string seqNo, string jobDetailType, string barcodeDataFormat, string user, out string serial)
        {
            try
            {
                var result = ExtractSerial(orderNo, seqNo, jobDetailType, barcodeDataFormat, user, out SerialPool serialPool);
                serial = serialPool?.SerialNum;
                return result;
            }
            catch(Exception ex)
            {
                log.InfoFormat("SerialpoolController ExtractSerial Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                serial = string.Empty;
                return false;
            }
        }
        public static bool ExtractSerial(string orderNo, string seqNo, string jobDetailType, string barcodeDataFormat, string user, out SerialPool serial)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    StringBuilder sql = new StringBuilder($"SELECT TOP 1 * FROM Dmn_SerialPool WHERE ");

                    List<SqlParameter> parameters = new List<SqlParameter>();
                    var subConditions = new List<string>();

                    if (!string.IsNullOrEmpty(orderNo))
                    {
                        parameters.Add(new SqlParameter("@orderNo", orderNo ?? ""));
                        subConditions.Add($"OrderNo = @orderNo");
                    }
                    if (!string.IsNullOrEmpty(seqNo))
                    {
                        parameters.Add(new SqlParameter("@seqNo", seqNo ?? ""));
                        subConditions.Add($"SeqNo = @seqNo");
                    }
                    if (!string.IsNullOrEmpty(jobDetailType))
                    {
                        parameters.Add(new SqlParameter("@jobDetailType", jobDetailType ?? ""));
                        subConditions.Add($"JobDetailType = @jobDetailType");
                    }
                    if (!string.IsNullOrEmpty(barcodeDataFormat))
                    {
                        subConditions.Add($"BarcodeDataFormat = @barcodeDataFormat");
                        parameters.Add(new SqlParameter("@barcodeDataFormat", barcodeDataFormat ?? ""));
                    }
                    subConditions.Add($"UseYN <> 'Y' ");
                    sql.Append(string.Join(" AND ", subConditions));
                    sql.Append("ORDER BY Idx_Insert");
                    Local.Dmn_SerialPool tmp = db.Database.SqlQuery<Local.Dmn_SerialPool>(sql.ToString(), parameters.ToArray()).FirstOrDefault();
                    
                    if (tmp != null)
                    {
                        tmp.Status = "RE";
                        tmp.UseYN = "Y";
                        tmp.UpdateUser = user;
						tmp.UpdateDate = DateTime.Now;
						string updateSQL = @"UPDATE [DOMINO_DB].[dbo].[Dmn_SerialPool] SET
                                           UpdateUser = @user,
                                           UpdateDate = @useDate,
                                           UseDate = @useDate,
                                           Status = @status,
                                           UseYN = @useYN
                                           WHERE ProdStdCode = @prodStdCode AND SerialNum = @serialNum AND JobDetailType = @jobDetailType";

                        SqlParameter[] updateParameters = new SqlParameter[]
                        {
                            new SqlParameter("@status", tmp.Status),
                            new SqlParameter("@useYN", tmp.UseYN),
                            new SqlParameter("@user", tmp.UpdateUser),
                            new SqlParameter("@useDate", tmp.UpdateDate),
                            new SqlParameter("@prodStdCode", tmp.ProdStdCode),
                            new SqlParameter("@serialNum", tmp.SerialNum),
                            new SqlParameter("@jobDetailType", tmp.JobDetailType)
                        };
                        db.Database.ExecuteSqlCommand(updateSQL, updateParameters);
                        log.DebugFormat("ExtractSerialPool Extracted : {0}", tmp.SerialNum);
                        serial = new SerialPool(tmp);
                        return true;
                    }
                    log.DebugFormat("Serial Extract fail : {0}", tmp?.SerialNum);
                    throw new Exception("Serial Extraction failed.");
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController ExtractSerialPool Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                serial = null;
                return false;
            }
        }
        public static bool ExtractSerialQuery(string orderNo, string seqNo, string jobDetailType, string user, out string serial)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    serial = string.Empty;
                    string sql = $"SELECT TOP 1 * FROM [DOMINO_DB].[dbo].[Dmn_SerialPool] " +
                        $"where OrderNo = '{orderNo}' AND SeqNo = '{seqNo}' AND JobDetailType = '{jobDetailType}' AND UseYN <> 'Y' order by Idx_Insert";
                    DominoDatabase.Local.Dmn_SerialPool tmp = db.Select<DominoDatabase.Local.Dmn_SerialPool>(sql).FirstOrDefault();
                    if (tmp != null)
                    {
                        string useDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        string updateSQL = @"UPDATE [DOMINO_DB].[dbo].[Dmn_SerialPool] SET
                                           UpdateUser = @user,
                                           UpdateDate = @useDate,
                                           UseDate = @useDate,
                                           Status = 'RE',
                                           UseYN = 'Y'
                                           WHERE ProdStdCode = @prodStdCode AND SerialNum = @serialNum AND JobDetailType = @jobDetailType";

                        SqlParameter[] parameters = new SqlParameter[]
                        {
                            new SqlParameter("@user", user),
                            new SqlParameter("@useDate", useDate),
                            new SqlParameter("@prodStdCode", tmp.ProdStdCode),
                            new SqlParameter("@serialNum", tmp.SerialNum),
                            new SqlParameter("@jobDetailType", tmp.JobDetailType)
                        };


                        db.Database.ExecuteSqlCommand(updateSQL, parameters);
                        serial = tmp.SerialNum;
                        log.DebugFormat("Serial Extracted : {0}", tmp.SerialNum);
                        return true;
                    }
                    else
                    {
                        log.DebugFormat("Serial Extract fail : {0}", tmp.SerialNum);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController ExtractSerial Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                serial = string.Empty;
                return false;
            }
        }
       

        public static Local.Dmn_SerialPool ExtractSerialQueryforReserved(string orderNo, string seqNo, string jobDetailType, string user)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {

                    string sql = $"SELECT TOP 1 * FROM [DOMINO_DB].[dbo].[Dmn_SerialPool] " +
                        $"where OrderNo = '{orderNo}' AND SeqNo = '{seqNo}' AND JobDetailType = '{jobDetailType}' AND UseYN <> 'Y' order by Idx_Insert";
                    DominoDatabase.Local.Dmn_SerialPool tmp = db.Select<DominoDatabase.Local.Dmn_SerialPool>(sql).FirstOrDefault();
                    if (tmp != null)
                    {
                        string useDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        //db.Database.ExecuteSqlCommand("UPDATE[DOMINO_DB].[dbo].[Dmn_SerialPool] SET " +
                        //                              "UpdateUser = {0}," +
                        //                              "UpdateDate = {1}," +
                        //                              "UseDate = {1}," +
                        //                              "Status = 'RE'," +
                        //                              "UseYN = 'Y'" +
                        //                              "WHERE ProdStdCode = {2} AND SerialNum = {3} AND JobDetailType = {4}", user, useDate, tmp.ProdStdCode, tmp.SerialNum, tmp.JobDetailType);


                        string updateSQL = @"UPDATE [DOMINO_DB].[dbo].[Dmn_SerialPool] SET
                                             UpdateUser = @user,
                                             UpdateDate = @useDate,
                                             UseDate = @useDate,
                                             Status = 'RE',
                                             UseYN = 'Y'
                                             WHERE ProdStdCode = @prodStdCode AND SerialNum = @serialNum AND JobDetailType = @jobDetailType";

                        SqlParameter[] parameters = new SqlParameter[]
                         {
                            new SqlParameter("@user", user),
                            new SqlParameter("@useDate", useDate),
                            new SqlParameter("@prodStdCode", tmp.ProdStdCode),
                            new SqlParameter("@serialNum", tmp.SerialNum),
                            new SqlParameter("@jobDetailType", tmp.JobDetailType)
                        };


                        db.Database.ExecuteSqlCommand(updateSQL, parameters);
                        log.DebugFormat("Serial Extracted : {0}, Reserved1:{1}, Reserved2: {2}, Reserved3: {3}", tmp.SerialNum, tmp.Reserved1, tmp.Reserved2, tmp.Reserved3);
                        return tmp;
                    }
                    else
                    {
                        log.DebugFormat("Serial Extract fail : Serial:{0}, Reserved1:{1}, Reserved2:{2}, Reserved3:{3}", tmp.SerialNum, tmp.Reserved1, tmp.Reserved2, tmp.Reserved3);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController ExtractSerial Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return null;
            }

        }

        public static bool ExtractCurrentSerial(string orderNo, string seqNo, string jobDetailType, int index, string user, string barcodeDataFormat, out string serial)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_SerialPool
                               where a.OrderNo.Equals(orderNo) && a.SeqNo.Equals(seqNo) && a.JobDetailType.Equals(jobDetailType) && a.idx_Insert >= index -1 && a.UseYN == "Y" && a.BarcodeDataFormat.Equals(barcodeDataFormat)
                               select a;
                    var tmp = list.OrderByDescending(q => q.idx_Insert).ToList().FirstOrDefault();
                    serial = tmp.SerialNum;
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController ExtractSerial Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                serial = string.Empty;
                return false;
            }
        }
        public static bool InsertLocal(List<SerialPool> items, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("SerialpoolController InsertLocal");
                    var list = new List<Local.Dmn_SerialPool>();
                    foreach (var item in items)
                    {
                        list.Add(new Local.Dmn_SerialPool(item));
                        log.InfoFormat("SerialpoolController Inserted {0}", item.SerialNum);
                    }
                    db.Dmn_SerialPool.AddRange(list);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController InsertLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController InsertLocal Exception", ex);
                return false;
            }
        }
        public static bool InsertLocal(Local.Dmn_SerialPool item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("SerialpoolController Inserted {0}", item.SerialNum);
                    db.Dmn_SerialPool.Add(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController InsertLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController InsertLocal Exception", ex);
                return false;
            }
        }
        public static bool AddOrUpdate(Local.Dmn_SerialPool item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("SerialpoolController AddedOrUpdated {0}", item.SerialNum);
                    db.Dmn_SerialPool.AddOrUpdate(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController AddOrUpdate Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController AddOrUpdate Exception", ex);
                return false;
            }
        }
        public static bool UpdateLocal(Local.Dmn_SerialPool item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("SerialpoolController Updated {0}", item.SerialNum);
                    var tmp = db.Dmn_SerialPool.First(q => q.ProdStdCode.Equals(item.ProdStdCode) && q.SerialNum.Equals(item.SerialNum));
                    tmp.WriteExistings(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController UpdateLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController UpdateLocal Exception", ex);
                return false;
            }
        }
        
        public static bool UpdateLocalQuery(string prodStdCode, string serialNumber, string jobDetailType, string status, string useYN, string updateUser, DateTime? inspectedDate, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.Info($"SerialpoolController UpdateLocalQuery by {updateUser}, serial : {serialNumber}");
                    string query = "";
                    if (inspectedDate == null)
                    {
                        query = string.Format($@"UPDATE [DOMINO_DB].[dbo].[Dmn_SerialPool]
                                                  SET Status = '{status}',
                                                      UseYN = '{useYN}',
	                                                  UpdateUser = '{updateUser}'
                                                  where ProdStdCode = '{prodStdCode}' AND SerialNum = '{serialNumber}' AND JobDetailType = '{jobDetailType}'");
                    }
                    else
                    {
                        query = string.Format($@"UPDATE [DOMINO_DB].[dbo].[Dmn_SerialPool]
                                                  SET Status = '{status}',
                                                      UseYN = '{useYN}',
	                                                  InspectedDate = '{inspectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff")}',
	                                                  UpdateUser = '{updateUser}'
                                                  where ProdStdCode = '{prodStdCode}' AND SerialNum = '{serialNumber}' AND JobDetailType = '{jobDetailType}'");
                    }
                    db.Database.ExecuteSqlCommand(query);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController UpdateLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController UpdateLocal Exception", ex);
                return false;
            }
        }
        public static bool UpdateLocalQuery2(string prodStdCode, string serial, string status, string inspectedDate, string updateUser, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {


                    log.InfoFormat("SerialpoolController UpdateLocalQuery by {0}, serial : {1}", updateUser, serial);
                    string query = @"UPDATE [DOMINO_DB].[dbo].[Dmn_SerialPool]
                                    SET Status = @status,
                                        InspectedDate = @inspectedDate,
                                        UpdateDate = @inspectedDate,
                                        UpdateUser = @updateUser
                                        WHERE ProdStdCode = @prodStdCode AND SerialNum = @serialNum";

                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@status", status),
                        new SqlParameter("@inspectedDate", inspectedDate),
                        new SqlParameter("@updateUser", updateUser),
                        new SqlParameter("@prodStdCode", prodStdCode),
                        new SqlParameter("@serialNum", serial)
                    };
                    db.Database.ExecuteSqlCommand(query, parameters);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController UpdateLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController UpdateLocal Exception", ex);
                return false;
            }
        }
        public static bool DeleteLocalAll(string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("SerialpoolController DeleteLocalAll by {0}", userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_SerialPool");
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController DeleteLocalAll Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController DeleteLocalAll Exception", ex);
                return false;
            }

        }
        public static bool DeleteLocalSingle(string standardCode, string serialnumber, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("SerialpoolController DeleteLocalSingle {0} {1} by {2}", standardCode, serialnumber, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_SerialPool where ProdStdCode = {0} AND SerialNum = {1}", standardCode, serialnumber);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController DeleteLocalSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController DeleteLocalSingle Exception", ex);
                return false;
            }
        }
        public static bool DeleteLocalByOrder(string orderNo, string seqNo, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("SerialpoolController DeleteLocalByOrder {0} {1} by {2}", orderNo, seqNo, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_SerialPool where OrderNo = {0} AND SeqNo = {1}", orderNo, seqNo);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController DeleteLocalByOrder Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController DeleteLocalByOrder Exception", ex);
                return false;
            }
        }
        public static bool DeleteLocalByOrder(string orderNo, string seqNo, string jobDetailType, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("SerialpoolController DeleteLocalByOrder {0} {1} {2} by {3}", orderNo, seqNo, jobDetailType, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_SerialPool where OrderNo = {0} AND SeqNo = {1} AND JobDetailType = {2}", orderNo, seqNo, jobDetailType);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController DeleteLocalByOrder Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController DeleteLocalByOrder Exception", ex);
                return false;
            }
        }
        public static bool DeleteLocalByCompleteDatePM(DateTime dt, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    db.Database.CommandTimeout = 300;

                    log.InfoFormat("SerialpoolController DeleteLocalByCompleteDatePM {0} by {1}", dt, userID);
                    string sql = string.Format(@"
                                                DELETE FROM Dmn_Serialpool
                                                WHERE OrderNo IN (
                                                    SELECT TOP 100000 A.OrderNo
                                                    FROM Dmn_Serialpool AS A
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
                log.InfoFormat("SerialpoolController DeleteLocalByCompleteDatePM Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController DeleteLocalByCompleteDatePM Exception", ex);
                return false;
            }
        }
        public static bool DeleteLocalCancelByInsertDatePM(DateTime dt, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    string sql = string.Format(@"
                                                DELETE FROM Dmn_Serialpool
                                                WHERE OrderNo IN (
                                                    SELECT TOP 100000 A.OrderNo
                                                    FROM Dmn_Serialpool AS A
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
                log.InfoFormat("SerialpoolController DeleteLocalCancelByInsertDatePM Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController DeleteLocalCancelByInsertDatePM Exception", ex);
                return false;
            }
        }
        public static bool DeleteLocalByCompleteDateAG(DateTime dt, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("SerialpoolController DeleteLocalByCompleteDateAG {0} by {1}", dt, userID);
                    string sql = string.Format(@"DELETE A
                                   FROM Dmn_Serialpool AS A
                                   JOIN Dmn_JobOrder_AG AS B ON A.OrderNo = B.OrderNo AND A.SeqNo = B.SeqNo
                                   Where B.CompleteDate < '{0}' AND A.BarcodeDataFormat <> 'SSCC'", dt.ToString("yyyy-MM-dd"));
                    db.Database.ExecuteSqlCommand(sql);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController DeleteLocalByCompleteDateAG Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController DeleteLocalByCompleteDateAG Exception", ex);
                return false;
            }
        }
        public static bool DeleteLocalByCompleteDateAGSSCC(DateTime dt, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("SerialpoolController DeleteLocalByCompleteDateAG {0} by {1}", dt, userID);
                    string sql = string.Format(@"DELETE A
                                   FROM Dmn_Serialpool AS A
                                   JOIN Dmn_JobOrder_AG AS B ON A.OrderNo = B.OrderNo AND A.SeqNo = B.SeqNo
                                   Where B.CompleteDate < '{0}' AND A.BarcodeDataFormat = 'SSCC'", dt.ToString("yyyy-MM-dd"));
                    db.Database.ExecuteSqlCommand(sql);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController DeleteLocalByCompleteDateAG Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController DeleteLocalByCompleteDateAG Exception", ex);
                return false;
            }
        }
        public static bool DeleteLocalCancelByInsertDateAG(DateTime dt, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("SerialpoolController DeleteLocalCancelByInsertDateAG {0} by {1}", dt, userID);
                    string sql = string.Format(@"DELETE A
                                   FROM Dmn_Serialpool AS A
                                   JOIN Dmn_JobOrder_AG AS B ON A.OrderNo = B.OrderNo AND A.SeqNo = B.SeqNo
                                   Where B.InsertDate < '{0}' AND JobStatus = 'CC'", dt.ToString("yyyy-MM-dd"));
                    db.Database.ExecuteSqlCommand(sql);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController DeleteLocalCancelByInsertDateAG Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController DeleteLocalCancelByInsertDateAG Exception", ex);
                return false;
            }
        }
        public static bool DeleteLocalByOrderExceptSSCC(string orderNo, string seqNo, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("SerialpoolController DeleteLocalByOrderExceptSSCC {0} {1} by {2}", orderNo, seqNo, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_SerialPool where OrderNo = {0} AND SeqNo = {1} AND BarcodeDataFormat != 'SSCC'", orderNo, seqNo);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController DeleteLocalByOrderExceptSSCC Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController DeleteLocalByOrderExceptSSCC Exception", ex);
                return false;
            }
        }
        public static bool DeleteLocalByOrderOnlySSCC(string orderNo, string seqNo, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("SerialpoolController DeleteLocalByOrderOnlySSCC {0} {1} by {2}", orderNo, seqNo, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_SerialPool where OrderNo = {0} AND SeqNo = {1} AND BarcodeDataFormat = 'SSCC'", orderNo, seqNo);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController DeleteLocalByOrderOnlySSCC Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController DeleteLocalByOrderOnlySSCC Exception", ex);
                return false;
            }
        }
        public static bool ResetLocalSingle(string standardCode, string serialnumber, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("SerialpoolController ResetLocalSingle {0} {1} by {2}", standardCode, serialnumber, userID);
                    db.Database.ExecuteSqlCommand("Update Dmn_SerialPool  Set UseDate = NULL, InspectedDate = NULL, UseYN = 'N', Status = 'NU', FileName = NULL, UpdateDate = {0}, UpdateUser = {1} where ProdStdCode = {2} AND SerialNum = {3}", DateTime.Now, userID, standardCode, serialnumber);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController ResetLocalSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController ResetLocalSingle Exception", ex);
                return false;
            }
        }
        public static bool ResetLocalAllByJobOrder(string orderNo, string SeqNo, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("SerialpoolController ResetLocalAllByJobOrder {0} {1} by {2}", orderNo, SeqNo, userID);
                    db.Database.ExecuteSqlCommand("Update Dmn_SerialPool  Set UseDate = NULL, InspectedDate = NULL, UseYN = 'N', Status = 'NU', FileName = NULL, UpdateDate = {0}, UpdateUser = {1} where OrderNo = {2} AND SeqNo = {3}", DateTime.Now, userID, orderNo, SeqNo);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController ResetLocalAllByJobOrder Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController ResetLocalAllByJobOrder Exception", ex);
                return false;
            }
        }
		public static bool ResetLocalByJobOrder(string stdCode, string serial, string userID, bool RaiseException = false)
		{
			try
			{
				using (var db = new DominoDBLocal())
				{
					log.InfoFormat("SerialpoolController ResetLocalAllByJobOrder {0} {1} by {2}", stdCode, serial, userID);
                    StringBuilder sb = new StringBuilder();
                    sb.Append("UPDATE Dmn_SerialPool ");
                    sb.Append("SET UseDate = NULL, InspectedDate = NULL, UseYN = 'Y', Status = 'NU', FileName = NULL, ConfirmedYN = NULL, UpdateDate = {0}, UpdateUser = {1} ");
                    sb.Append("WHERE ProdStdCode = {2} AND SerialNum = {3}");
                    db.Database.ExecuteSqlCommand(sb.ToString(), DateTime.Now, userID, stdCode, serial);
					return true;
				}
			}
			catch (Exception ex)
			{
				log.InfoFormat("SerialpoolController ResetLocalAllByJobOrder Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
				if (RaiseException)
					throw new Exception("SerialpoolController ResetLocalAllByJobOrder Exception", ex);
				return false;
			}
		}
		public static bool ResetLocalAllByJobOrder(string orderNo, string SeqNo, string jobDetailType, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("SerialpoolController ResetLocalAllByJobOrder {0} {1} {2}by {3}", orderNo, SeqNo, jobDetailType, userID);
                    db.Database.ExecuteSqlCommand("Update Dmn_SerialPool  Set UseDate = NULL, InspectedDate = NULL, UseYN = 'N', Status = 'NU', FileName = NULL, ConfirmedYN = NULL, UpdateDate = {0}, UpdateUser = {1} where OrderNo = {2} AND SeqNo = {3} AND JobDetailType = {4}", DateTime.Now, userID, orderNo, SeqNo, jobDetailType);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController ResetLocalAllByJobOrder Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController ResetLocalAllByJobOrder Exception", ex);
                return false;
            }
        }
        public static DateTime? GetLastInsertDateLocal(bool RaiseException = false)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    return db.Dmn_SerialPool.Where(q => q.SerialType != "C" && q.UseYN != "Y").Select(q => q.InsertDate).Max();
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController GetLastInsertDateLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController GetLastInsertDateLocal Exception", ex);
            }
            return null;
        }
        public static DateTime? GetLastUpdateDateLocal(bool RaiseException = false)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    return db.Dmn_SerialPool.Where(q => q.SerialType != "C" && q.UseYN != "Y").Select(q => q.UpdateDate).Max();
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController GetLastUpdateDateLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController GetLastUpdateDateLocal Exception", ex);
            }
            return null;
        }
        public static DateTime? GetLastAssignDateLocal(bool RaiseException = false)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    string sql = $"SELECT MAX(AssignDate) FROM Dmn_SerialPool Where SerialType <> 'C'";
                    return db.Database.SqlQuery<DateTime?>(sql).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController GetLastAssignDateLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController GetLastAssignDateLocal Exception", ex);
            }
            return null;
        }
        public static List<SerialPool> SelectServer(string plantCode, string machineId, string standardCode, string productCode, string productName, string serialNum, string jobDetailType, string orderNo, string seqNo, string lot,
         string resourceType, string status, string barcodeType, string barcodeDataFormat, string prefix, DateTime? useDateStart, DateTime? useDateEnd, DateTime? assignDate, bool? assigned ,bool? use, int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            List<SerialPool> retList = new List<SerialPool>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_SerialPool
                               join b in db.Dmn_JobOrder_M on new { a.PlantCode, a.OrderNo, a.SeqNo } equals new { b.PlantCode, b.OrderNo, b.SeqNo } into bb
                               from b in bb.DefaultIfEmpty()
                               join c in db.Dmn_JobOrder_D on new { a.PlantCode, a.OrderNo, a.SeqNo, a.JobDetailType } equals new { c.PlantCode, c.OrderNo, c.SeqNo, c.JobDetailType } into cc
                               from c in cc.DefaultIfEmpty()
                               join d in db.Dmn_Product_M on new { a.PlantCode, b.ProdCode } equals new { d.PlantCode, d.ProdCode} into dd
                               from d in dd.DefaultIfEmpty()
                               join e in db.Dmn_Product_D on new { a.PlantCode, b.ProdCode, c.JobDetailType } equals new { e.PlantCode, e.ProdCode, e.JobDetailType } into ee
                               from e in ee.DefaultIfEmpty()
                               select new { a, b, c, d, e };
                    if (!string.IsNullOrEmpty(plantCode))
                        list = list.Where(q => q.a.PlantCode.Equals(plantCode));
                    if (!string.IsNullOrEmpty(machineId))
                        list = list.Where(q => q.a.MachineID.Equals(machineId));
                    if (!string.IsNullOrEmpty(standardCode))
                        list = list.Where(q => q.a.ProdStdCode.Contains(standardCode));
                    if (!string.IsNullOrEmpty(productCode))
                        list = list.Where(q => q.b.ProdCode.Contains(productCode));
                    if (!string.IsNullOrEmpty(productName))
                        list = list.Where(q => q.d.ProdName.Contains(productName));
                    if (!string.IsNullOrEmpty(serialNum))
                        list = list.Where(q => q.a.SerialNum.Contains(serialNum));
                    if (!string.IsNullOrEmpty(jobDetailType))
                        list = list.Where(q => q.a.JobDetailType.Contains(jobDetailType));
                    if (!string.IsNullOrEmpty(orderNo))
                        list = list.Where(q => q.a.OrderNo.Contains(orderNo));
                    if (!string.IsNullOrEmpty(seqNo))
                        list = list.Where(q => q.a.SeqNo.Contains(seqNo));
                    if (!string.IsNullOrEmpty(lot))
                        list = list.Where(q => q.b.LotNo.Contains(lot));
                    if (!string.IsNullOrEmpty(resourceType))
                        list = list.Where(q => q.a.ResourceType.Equals(resourceType));
                    if (!string.IsNullOrEmpty(barcodeType))
                        list = list.Where(q => q.a.BarcodeType.Equals(barcodeType));
                    if (!string.IsNullOrEmpty(barcodeDataFormat))
                        list = list.Where(q => q.a.BarcodeDataFormat.Equals(barcodeDataFormat));
                    if (!string.IsNullOrEmpty(status))
                        list = list.Where(q => q.a.Status.Contains(status));
                    if (useDateStart != null)
                        list = list.Where(q => q.a.UseDate >= useDateStart.Value);
                    if (useDateEnd != null)
                        list = list.Where(q => q.a.UseDate <= useDateEnd.Value);
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Y") : q.a.UseYN.Equals("N"));
                    if (assigned != null)
                        list = list.Where(q => q.c != null && q.c.JobStatus != "IS" && q.c.JobStatus != "CC");
                    if (assignDate != null)
                        list = list.Where(q => q.a.AssignDate > assignDate.Value);
                    if (prefix != null)
                        list = list.Where(q => q.e.Prefix_SSCC.Equals(prefix));
                    foreach (var item in list)
                    {
                        retList.Add(new SerialPool(item.a));
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
                log.InfoFormat("SerialpoolController SelectServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController SelectServer Exception", ex);
            }
            return retList;
        }

        public static List<SerialPool> SelectServerQuery(string plantCode, string machineId, string standardCode, string prodCode, string prodName, string serialNum, string jobDetailType, string orderNo, string seqNo, string lotNo,
         string resourceType, string status, string barcodeType, string barcodeDataFormat, string prefix, DateTime? useDateStart, DateTime? useDateEnd, DateTime? assignDate, bool? assigned, bool? use, int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            string sql = string.Empty;
            try
            {
                string dateFormat = "yyyy-MM-dd tt hh:mm:ss.fff";
                string convertFormat = "yyyy-MM-dd HH:mm:ss.fff";

                string strUseDateStart = null;
                string strUseDateEnd = null;
                string strAssignDate = null;

                if (useDateStart.HasValue)
                {
                    if (useDateStart < SqlMinValue)
                    {
                        useDateStart = SqlMinValue;
                    }

                    strUseDateStart = DateTime.ParseExact(useDateStart.Value.ToString(dateFormat), dateFormat, null, DateTimeStyles.AssumeLocal).ToString(convertFormat);
                }

                if (useDateEnd.HasValue)
                {
                    if (useDateEnd > SqlMaxValue)
                    {
                        useDateEnd = SqlMaxValue;
                    }

                    strUseDateEnd = DateTime.ParseExact(useDateEnd.Value.ToString(dateFormat), dateFormat, null, DateTimeStyles.AssumeLocal).ToString(convertFormat);
                }

                if (assignDate.HasValue)
                { 
                    if (assignDate < SqlMinValue)
                    { 
                        assignDate = SqlMinValue;
                    }
                
                    strAssignDate = DateTime.ParseExact(assignDate.Value.ToString(dateFormat), dateFormat, null, DateTimeStyles.AssumeLocal).ToString(convertFormat);
                }


                List<SerialPool> retList = new List<SerialPool>();
                using (DominoDBServer db = new DominoDBServer())
                {
                    sql = @"SELECT A.*
                                         ,C.ProdName
                                   FROM Dmn_SerialPool A
                                   INNER JOIN Dmn_JobOrder_M B 
                                   ON A.PlantCode = B.PlantCode AND A.OrderNo = B.OrderNo AND A.SeqNo = B.SeqNo
                                   INNER JOIN Dmn_Product_M C 
                                   ON B.PlantCode = C.PlantCode AND B.ProdCode = C.ProdCode
                                   INNER JOIN Dmn_JobOrder_D D
                                   ON A.PlantCode = D.PlantCode AND A.OrderNo = D.OrderNo AND A.SeqNo = D.SeqNo AND A.JobDetailType = D.JobDetailType
                                   INNER JOIN Dmn_Product_D E
                                   ON A.PlantCode = E.PlantCode AND C.ProdCode = E.ProdCode AND A.JobDetailType = E.JobDetailType
                                   WHERE 1=1 ";
                    if (!string.IsNullOrEmpty(plantCode))
                        sql += $" AND A.PlantCode = '{plantCode}'";
                    if (!string.IsNullOrEmpty(machineId))
                        sql += $" AND A.MachineId = '{machineId}'";
                    if (!string.IsNullOrEmpty(standardCode))
                        sql += $" AND A.ProdStdCode = '{standardCode}'";
                    if (!string.IsNullOrEmpty(prodCode))
                        sql += $" AND C.ProdCode = '{prodCode}'";
                    if (!string.IsNullOrEmpty(prodName))
                        sql += $" AND C.ProdName LIKE '%{prodName}%'";
                    if (!string.IsNullOrEmpty(serialNum))
                        sql += $" AND A.SerialNum = '{serialNum}'";
                    if (!string.IsNullOrEmpty(orderNo))
                        sql += $" AND A.OrderNo = '{orderNo}'";
                    if (!string.IsNullOrEmpty(seqNo))
                        sql += $" AND A.SeqNo = '{seqNo}'";
                    if (!string.IsNullOrEmpty(lotNo))
                        sql += $" AND B.LotNo = '{lotNo}'";
                    if (!string.IsNullOrEmpty(jobDetailType))
                        sql += $" AND A.JobDetailType = '{jobDetailType}'";
                    if (!string.IsNullOrEmpty(status))
                        sql += $" AND A.Status = '{status}'";
                    if (!string.IsNullOrEmpty(resourceType))
                        sql += $" AND A.ResourceType = '{resourceType}'";
                    if (!string.IsNullOrEmpty(barcodeType))
                        sql += $" AND A.BarcodeType = '{barcodeType}'";
                    if (!string.IsNullOrEmpty(barcodeDataFormat))
                        sql += $" AND A.BarcodeDataFormat = '{barcodeDataFormat}'";
                    if (!string.IsNullOrEmpty(prefix))
                        sql += $" AND E.Prefix_SSCC = '{prefix}' ";
                    if (assigned != null)
                        sql += $" AND D.JobStatus NOT IN ('IS','CC')";
                    if (assignDate != null)
                        sql += $" AND A.AssignDate > '{strAssignDate}'";
                    if (useDateStart != null)
                        sql += $" AND A.UseDate >= '{strUseDateStart}' ";
                    if (useDateEnd != null)
                        sql += $" AND A.UseDate <= '{strUseDateEnd}' "; 
                    if (use != null)
                    {
                        string useYn = (bool)use ? "Y" : "N";
                        sql += $" AND A.UseYN = '{useYn}'";
                    }

                    retList = db.Select<SerialPool>(sql);
                    if (pageIndex >= 0)
                    {
                        TotalCount = retList.Count();
                        TotalPageCount = Convert.ToInt64(Math.Ceiling(Convert.ToDouble(TotalCount) / Convert.ToDouble(pageSize)));
                        retList = retList.OrderBy(x => x.InsertDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    }
                    return retList;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialPoolController SelectServerQuery Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
				log.InfoFormat("SQL Query > \n{0}", sql);
				if (RaiseException)
                    throw new Exception("SerialPoolController SelectServerQuery Exception", ex);
            }
            return null;
        }

        public static List<DSM.Dmn_SerialPool> SelectServerRaw(string plantCode, string machineId, string standardCode, string productCode, string productName, string serialNum, string jobDetailType, string orderNo, string seqNo, string lot,
         string resourceType, string status, string barcodeType, string barcodeDataFormat, string prefix, DateTime? useDateStart, DateTime? useDateEnd, DateTime? assignDate, bool? assigned, bool? use, int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            List<DSM.Dmn_SerialPool> retList = new List<DSM.Dmn_SerialPool>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_SerialPool
                               join b in db.Dmn_JobOrder_M on new { a.PlantCode, a.OrderNo, a.SeqNo } equals new { b.PlantCode, b.OrderNo, b.SeqNo } into bb
                               from b in bb.DefaultIfEmpty()
                               join c in db.Dmn_JobOrder_D on new { a.PlantCode, a.OrderNo, a.SeqNo, a.JobDetailType } equals new { c.PlantCode, c.OrderNo, c.SeqNo, c.JobDetailType } into cc
                               from c in cc.DefaultIfEmpty()
                               join d in db.Dmn_Product_M on new { a.PlantCode, b.ProdCode } equals new { d.PlantCode, d.ProdCode } into dd
                               from d in dd.DefaultIfEmpty()
                               join e in db.Dmn_Product_D on new { a.PlantCode, b.ProdCode, c.JobDetailType } equals new { e.PlantCode, e.ProdCode, e.JobDetailType } into ee
                               from e in ee.DefaultIfEmpty()
                               join f in db.Dmn_VisionResult on new { a.PlantCode, a.InspectedDate } equals new { f.PlantCode, InspectedDate = f.InsertDate } into ff
                               from f in cc.DefaultIfEmpty()
                               select new { a, b, c, d, e, f };
                    if (!string.IsNullOrEmpty(plantCode))
                        list = list.Where(q => q.a.PlantCode.Equals(plantCode));
                    if (!string.IsNullOrEmpty(machineId))
                        list = list.Where(q => q.a.MachineID.Equals(machineId));
                    if (!string.IsNullOrEmpty(standardCode))
                        list = list.Where(q => q.a.ProdStdCode.Contains(standardCode));
                    if (!string.IsNullOrEmpty(productCode))
                        list = list.Where(q => q.b.ProdCode.Contains(productCode));
                    if (!string.IsNullOrEmpty(productName))
                        list = list.Where(q => q.d.ProdName.Contains(productName));
                    if (!string.IsNullOrEmpty(serialNum))
                        list = list.Where(q => q.a.SerialNum.Contains(serialNum));
                    if (!string.IsNullOrEmpty(jobDetailType))
                        list = list.Where(q => q.a.JobDetailType.Contains(jobDetailType));
                    if (!string.IsNullOrEmpty(orderNo))
                        list = list.Where(q => q.a.OrderNo.Contains(orderNo));
                    if (!string.IsNullOrEmpty(seqNo))
                        list = list.Where(q => q.a.SeqNo.Contains(seqNo));
                    if (!string.IsNullOrEmpty(lot))
                        list = list.Where(q => q.b.LotNo.Contains(lot));
                    if (!string.IsNullOrEmpty(resourceType))
                        list = list.Where(q => q.a.ResourceType.Equals(resourceType));
                    if (!string.IsNullOrEmpty(barcodeType))
                        list = list.Where(q => q.a.BarcodeType.Equals(barcodeType));
                    if (!string.IsNullOrEmpty(barcodeDataFormat))
                        list = list.Where(q => q.a.BarcodeDataFormat.Equals(barcodeDataFormat));
                    if (!string.IsNullOrEmpty(status))
                        list = list.Where(q => q.a.Status.Contains(status));
                    if (useDateStart != null)
                        list = list.Where(q => q.a.UseDate >= useDateStart.Value);
                    if (useDateEnd != null)
                        list = list.Where(q => q.a.UseDate >= useDateEnd.Value);
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Y") : q.a.UseYN.Equals("N"));
                    if (assigned != null)
                        list = list.Where(q => q.f != null && q.f.JobStatus != "IS" && q.f.JobStatus != "CC");
                    if (assignDate != null)
                        list = list.Where(q => q.a.AssignDate > assignDate.Value);
                    if (prefix != null)
                        list = list.Where(q => q.e.Prefix_SSCC.Equals(prefix));
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
                log.InfoFormat("SerialpoolController SelectServerRaw Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController SelectServerRaw Exception", ex);
            }
            return retList;
        }
        public static List<DSM.Dmn_SerialPool> SelectServerQuery(string plantCode, string orderNo, string seqNo, bool RaiseException = false)
        {
            List<DSM.Dmn_SerialPool> retList = new List<DSM.Dmn_SerialPool>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    retList = db.Dmn_SerialPool
                                .Where(a => a.PlantCode == plantCode && a.OrderNo == orderNo && a.SeqNo == seqNo)
                                .ToList();
                }
                return retList;
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController SelectServerQuery Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController SelectServerQuery Exception", ex);
            }
            return retList;
        }

        public static SerialPool SelectServerSingle(string plantCode, string machineId, string standardCode, string productCode, string productName, string serialNum, string jobDetailType, string orderNo, string seqNo, string lot,
         string resourceType, string status, string barcodeType, string barcodeDataFormat, bool? use, int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            List<SerialPool> retList = new List<SerialPool>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_SerialPool
                               join b in db.Dmn_Product_M on new { a.PlantCode, a.ProdStdCode } equals new { b.PlantCode, b.ProdStdCode } into bb
                               from b in bb.DefaultIfEmpty()
                               join c in db.Dmn_VisionResult on new { a.PlantCode, a.InspectedDate } equals new { c.PlantCode, InspectedDate = c.InsertDate } into cc
                               from c in cc.DefaultIfEmpty()
                               join d in db.Dmn_JobOrder_M on new { a.PlantCode, a.OrderNo, a.SeqNo } equals new { d.PlantCode, d.OrderNo, d.SeqNo } into dd
                               from d in dd.DefaultIfEmpty()
                               select new { a, b, c, d };
                    if (!string.IsNullOrEmpty(plantCode))
                        list = list.Where(q => q.a.PlantCode.Equals(plantCode));
                    if (!string.IsNullOrEmpty(machineId))
                        list = list.Where(q => q.a.MachineID.Equals(machineId));
                    if (!string.IsNullOrEmpty(standardCode))
                        list = list.Where(q => q.a.ProdStdCode.Contains(standardCode));
                    if (!string.IsNullOrEmpty(productCode))
                        list = list.Where(q => q.b.ProdCode.Contains(productCode));
                    if (!string.IsNullOrEmpty(productName))
                        list = list.Where(q => q.b.ProdName.Contains(productName));
                    if (!string.IsNullOrEmpty(serialNum))
                        list = list.Where(q => q.a.SerialNum.Contains(serialNum));
                    if (!string.IsNullOrEmpty(jobDetailType))
                        list = list.Where(q => q.a.JobDetailType.Contains(jobDetailType));
                    if (!string.IsNullOrEmpty(orderNo))
                        list = list.Where(q => q.a.OrderNo.Contains(orderNo));
                    if (!string.IsNullOrEmpty(seqNo))
                        list = list.Where(q => q.a.SeqNo.Contains(seqNo));
                    if (!string.IsNullOrEmpty(lot))
                        list = list.Where(q => q.d.LotNo.Contains(lot));
                    if (!string.IsNullOrEmpty(resourceType))
                        list = list.Where(q => q.a.ResourceType.Equals(resourceType));
                    if (!string.IsNullOrEmpty(barcodeType))
                        list = list.Where(q => q.a.BarcodeType.Equals(barcodeType));
                    if (!string.IsNullOrEmpty(barcodeDataFormat))
                        list = list.Where(q => q.a.BarcodeDataFormat.Equals(barcodeDataFormat));
                    if (!string.IsNullOrEmpty(status))
                        list = list.Where(q => q.a.Status.Contains(status));
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Y") : q.a.UseYN.Equals("N"));
                    foreach (var item in list)
                    {
                        retList.Add(new SerialPool(item.a));
                    }
                }
                return retList.FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController SelectServerSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController SelectServerSingle Exception", ex);
            }
            return retList.FirstOrDefault();
        }
        public static int GetOrder(SerialPool serialPool)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_SerialPool
                               where a.OrderNo == serialPool.OrderNo && a.SeqNo == serialPool.SeqNo && a.JobDetailType == serialPool.JobDetailType && a.idx_Insert <= serialPool.idx_Insert
                               orderby a.SerialNum
                               select a;

                    return list?.Count() ?? 1;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController GetOrder Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return 0;
            }
        }
        public static List<SerialPool> SelectSSCC(string prefix)
        {
            List<SerialPool> retList = new List<SerialPool>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_SerialPool
                               where a.BarcodeDataFormat.Equals("SSCC") && a.SerialNum.StartsWith(prefix)
                               select a;
                    foreach (var item in list)
                    {
                        retList.Add(new SerialPool(item));
                    }
                }
                return retList.ToList();
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController SelectSSCC Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return null;
            }
        }
        public static int GetSerialCount(string standardCode, string Serial)
        {
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_SerialPool
                               where a.ProdStdCode.Equals(standardCode) && a.SerialNum.Equals(Serial)
                               select a;
                    return list.Count();
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController GetSerialCount Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return 0;
            }
        }

        public static int GetSerialDuplicatedCount(string standardCode, string Serial)
        {
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_SerialPool
                               where a.ProdStdCode.Equals(standardCode) && a.SerialNum.Equals(Serial)
                               select a;
                    return list.Max(q => q.Duplicate_Cnt);
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController GetSerialCount Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return 0;
            }
        }
        public static int GetMaxGroup(string standardCode)
        {
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_SerialPool
                               where a.ProdStdCode.Equals(standardCode)
                               select a;
                    return list.Max(q => (int)q.idx_Group);
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController GetSerialCount Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return 0;
            }
        }
        public static int GetMaxIndex(string standardCode)
        {
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_SerialPool
                               where a.ProdStdCode.Equals(standardCode)
                               select a;
                    return list.Any() ? list.Max(q => (int)q.idx_Insert) : 0;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController GetSerialCount Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return 0;
            }
        }
        public static int GetMaxGroup(string plantCode, string orderNo, string seqNo, string jobDetailType)
        {
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_SerialPool
                               where a.PlantCode == plantCode && a.OrderNo == orderNo && a.SeqNo == seqNo && a.JobDetailType == jobDetailType
                               select a;
                    return list.Any() ? list.Max(q => (int)q.idx_Group) : 0;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController GetSerialCount Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return 0;
            }
        }
        public static int GetMaxIndex(string plantCode, string orderNo, string seqNo, string jobDetailType)
        {
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_SerialPool
                               where a.PlantCode == plantCode && a.OrderNo == orderNo && a.SeqNo == seqNo && a.JobDetailType == jobDetailType
                               select a;
                    return list.Any() ? list.Max(q => (int)q.idx_Insert) : 0;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController GetSerialCount Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return 0;
            }
        }
        public static bool InsertServer(DSM.Dmn_SerialPool item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("SerialpoolController InsertServer by {0}, {1}", item.InsertUser, item.ToString());
                    db.Dmn_SerialPool.Add(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var e in ex.EntityValidationErrors)
                {
                    log.Error(e.SerializeObjectToJson());
                    foreach (var v in e.ValidationErrors)
                    {
                        log.Error(v.SerializeObjectToJson());
                    }
                }
                if (RaiseException)
                    throw new Exception("SerialpoolController InsertServer Exception", ex);
                return false;
            }
        }
        public static bool InsertServer(List<DSM.Dmn_SerialPool> item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("SerialpoolController AddOrUpdate List Start");
                    db.Dmn_SerialPool.AddRange(item);
                    db.SaveChanges();
                    log.InfoFormat("SerialpoolController AddOrUpdate List End");
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController InsertServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController InsertServer Exception", ex);
                return false;
            }
        }
        public static bool BulkInsertServer(List<DSM.Dmn_SerialPool> items, bool RaiseException = false)
        {
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DSM"].ToString()))
            {
                try
                {
                    con.Open();
                    using (SqlTransaction sqlTran = con.BeginTransaction())
                    {
                        try
                        {
                            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con, SqlBulkCopyOptions.Default, sqlTran))
                            {
                                bulkCopy.DestinationTableName = "Dmn_SerialPool";

                                bulkCopy.ColumnMappings.Add("PlantCode", "PlantCode");                              
                                bulkCopy.ColumnMappings.Add("ProdStdCode", "ProdStdCode");
                                bulkCopy.ColumnMappings.Add("SerialNum", "SerialNum");
                                bulkCopy.ColumnMappings.Add("JobDetailType", "JobDetailType");
                                bulkCopy.ColumnMappings.Add("Duplicate_Cnt", "Duplicate_Cnt");
                                bulkCopy.ColumnMappings.Add("MachineID", "MachineID");
                                bulkCopy.ColumnMappings.Add("OrderNo", "OrderNo");
                                bulkCopy.ColumnMappings.Add("SeqNo", "SeqNo");
                                bulkCopy.ColumnMappings.Add("ResourceType", "ResourceType");
                                bulkCopy.ColumnMappings.Add("BarcodeDataFormat", "BarcodeDataFormat");
                                bulkCopy.ColumnMappings.Add("BarcodeType", "BarcodeType");
                                bulkCopy.ColumnMappings.Add("SerialType", "SerialType");
                                bulkCopy.ColumnMappings.Add("idx_Group", "idx_Group");
                                bulkCopy.ColumnMappings.Add("idx_Insert", "idx_Insert");
                                bulkCopy.ColumnMappings.Add("UseDate", "UseDate");
                                bulkCopy.ColumnMappings.Add("InspectedDate", "InspectedDate");
                                bulkCopy.ColumnMappings.Add("UseYN", "UseYN");
                                bulkCopy.ColumnMappings.Add("Status", "Status");
                                bulkCopy.ColumnMappings.Add("FileName", "FileName");
                                bulkCopy.ColumnMappings.Add("Reserved1", "Reserved1");
                                bulkCopy.ColumnMappings.Add("Reserved2", "Reserved2");
                                bulkCopy.ColumnMappings.Add("Reserved3", "Reserved3");
                                bulkCopy.ColumnMappings.Add("Reserved4", "Reserved4");
                                bulkCopy.ColumnMappings.Add("Reserved5", "Reserved5");
                                bulkCopy.ColumnMappings.Add("InsertUser", "InsertUser");
                                bulkCopy.ColumnMappings.Add("InsertDate", "InsertDate");
                                bulkCopy.ColumnMappings.Add("UpdateUser", "UpdateUser");
                                bulkCopy.ColumnMappings.Add("UpdateDate", "UpdateDate");
                                bulkCopy.ColumnMappings.Add("AssignDate", "AssignDate");

                                DataTable dt = new DataTable();
                                dt.Columns.Add("PlantCode", typeof(string));
                                dt.Columns.Add("ProdStdCode", typeof(string));
                                dt.Columns.Add("SerialNum", typeof(string));
                                dt.Columns.Add("JobDetailType", typeof(string));
                                dt.Columns.Add("Duplicate_Cnt", typeof(int));
                                dt.Columns.Add("MachineID", typeof(string));
                                dt.Columns.Add("OrderNo", typeof(string));
                                dt.Columns.Add("SeqNo", typeof(string));
                                dt.Columns.Add("ResourceType", typeof(string));
                                dt.Columns.Add("BarcodeDataFormat", typeof(string));
                                dt.Columns.Add("BarcodeType", typeof(string));
                                dt.Columns.Add("SerialType", typeof(string));
                                dt.Columns.Add("idx_Group", typeof(long));
                                dt.Columns.Add("idx_Insert", typeof(long));
                                dt.Columns.Add("UseDate", typeof(DateTime));
                                dt.Columns.Add("InspectedDate", typeof(DateTime));
                                dt.Columns.Add("UseYN", typeof(string));
                                dt.Columns.Add("Status", typeof(string));
                                dt.Columns.Add("FileName", typeof(string));
                                dt.Columns.Add("Reserved1", typeof(string));
                                dt.Columns.Add("Reserved2", typeof(string));
                                dt.Columns.Add("Reserved3", typeof(string));
                                dt.Columns.Add("Reserved4", typeof(string));
                                dt.Columns.Add("Reserved5", typeof(string));
                                dt.Columns.Add("InsertUser", typeof(string));
                                dt.Columns.Add("InsertDate", typeof(DateTime));
                                dt.Columns.Add("UpdateUser", typeof(string));
                                dt.Columns.Add("UpdateDate", typeof(DateTime));
                                dt.Columns.Add("AssignDate", typeof(DateTime));

                                foreach (var item in items)
                                {
                                    dt.Rows.Add(
                                        item.PlantCode,
                                        item.ProdStdCode,
                                        item.SerialNum,
                                        item.JobDetailType,
                                        item.Duplicate_Cnt,
                                        string.IsNullOrEmpty(item.MachineID) ? (object)DBNull.Value : item.MachineID,
                                        string.IsNullOrEmpty(item.OrderNo) ? (object)DBNull.Value : item.OrderNo,
                                        string.IsNullOrEmpty(item.SeqNo) ? (object)DBNull.Value : item.SeqNo,
                                        string.IsNullOrEmpty(item.ResourceType) ? "I" : item.ResourceType,
                                        string.IsNullOrEmpty(item.BarcodeDataFormat) ? (object)DBNull.Value : item.BarcodeDataFormat,
                                        string.IsNullOrEmpty(item.BarcodeType) ? (object)DBNull.Value : item.BarcodeType,
                                        string.IsNullOrEmpty(item.SerialType) ? (object)DBNull.Value : item.SerialType,
                                        item.idx_Group == null ? (object)DBNull.Value : item.idx_Group, 
                                        item.idx_Insert == null ? (object)DBNull.Value : item.idx_Insert, 
                                        item.UseDate == null ? (object)DBNull.Value : item.UseDate,
                                        item.InspectedDate == null ? (object)DBNull.Value : item.InspectedDate,
                                        string.IsNullOrEmpty(item.UseYN) ? (object)DBNull.Value : item.UseYN,
                                        string.IsNullOrEmpty(item.Status) ? (object)DBNull.Value : item.Status,
                                        string.IsNullOrEmpty(item.FileName) ? (object)DBNull.Value : item.FileName,
                                        string.IsNullOrEmpty(item.Reserved1) ? (object)DBNull.Value : item.Reserved1,
                                        string.IsNullOrEmpty(item.Reserved2) ? (object)DBNull.Value : item.Reserved2,
                                        string.IsNullOrEmpty(item.Reserved3) ? (object)DBNull.Value : item.Reserved3,
                                        string.IsNullOrEmpty(item.Reserved4) ? (object)DBNull.Value : item.Reserved4,
                                        string.IsNullOrEmpty(item.Reserved5) ? (object)DBNull.Value : item.Reserved5,
                                        item.InsertUser,
                                        item.InsertDate,
                                        string.IsNullOrEmpty(item.UpdateUser) ? (object)DBNull.Value : item.UpdateUser,
                                        item.UpdateDate == null ? (object)DBNull.Value : item.UpdateDate, 
                                        item.AssignDate == null ? (object)DBNull.Value : item.AssignDate);
                                }
                                System.Threading.Tasks.Task.Run(new Action(() => log.InfoFormat("SerialpoolController BulkInsertServer : {0}", items.Count)));
                                bulkCopy.WriteToServer(dt);
                                sqlTran.Commit();
                                return true;
                            }
                        }
                        catch(Exception ex)
                        {
                            sqlTran.Rollback();
                            log.InfoFormat("SerialpoolController BulkInsertServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                            if (RaiseException)
                                throw new Exception("SerialpoolController BulkInsertServer Exception", ex);
                            return false;
                        }
                    }
                }
                catch(Exception ex)
                {
                    log.InfoFormat("SerialpoolController BulkInsertServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                    if (RaiseException)
                        throw new Exception("SerialpoolController BulkInsertServer Exception", ex);
                    return false;
                }
            }
        }
        public static bool BulkInsertLocal(List<Local.Dmn_SerialPool> items, bool RaiseException = false)
        {
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Local"].ToString()))
            {
                try
                {
                    con.Open();
                    using (SqlTransaction sqlTran = con.BeginTransaction())
                    {
                        try
                        {
                            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con, SqlBulkCopyOptions.Default, sqlTran))
                            {
                                bulkCopy.DestinationTableName = "Dmn_SerialPool";

                                bulkCopy.ColumnMappings.Add("ProdStdCode", "ProdStdCode");
                                bulkCopy.ColumnMappings.Add("SerialNum", "SerialNum");
                                bulkCopy.ColumnMappings.Add("JobDetailType", "JobDetailType");
                                bulkCopy.ColumnMappings.Add("OrderNo", "OrderNo");
                                bulkCopy.ColumnMappings.Add("SeqNo", "SeqNo");
                                bulkCopy.ColumnMappings.Add("ResourceType", "ResourceType");
                                bulkCopy.ColumnMappings.Add("BarcodeDataFormat", "BarcodeDataFormat");
                                bulkCopy.ColumnMappings.Add("BarcodeType", "BarcodeType");
                                bulkCopy.ColumnMappings.Add("SerialType", "SerialType");
                                bulkCopy.ColumnMappings.Add("idx_Group", "idx_Group");
                                bulkCopy.ColumnMappings.Add("idx_Insert", "idx_Insert");
                                bulkCopy.ColumnMappings.Add("UseDate", "UseDate");
                                bulkCopy.ColumnMappings.Add("InspectedDate", "InspectedDate");
                                bulkCopy.ColumnMappings.Add("UseYN", "UseYN");
                                bulkCopy.ColumnMappings.Add("Status", "Status");
                                bulkCopy.ColumnMappings.Add("FileName", "FileName");
                                bulkCopy.ColumnMappings.Add("Reserved1", "Reserved1");
                                bulkCopy.ColumnMappings.Add("Reserved2", "Reserved2");
                                bulkCopy.ColumnMappings.Add("Reserved3", "Reserved3");
                                bulkCopy.ColumnMappings.Add("Reserved4", "Reserved4");
                                bulkCopy.ColumnMappings.Add("Reserved5", "Reserved5");
                                bulkCopy.ColumnMappings.Add("InsertUser", "InsertUser");
                                bulkCopy.ColumnMappings.Add("InsertDate", "InsertDate");
                                bulkCopy.ColumnMappings.Add("UpdateUser", "UpdateUser");
                                bulkCopy.ColumnMappings.Add("UpdateDate", "UpdateDate");
                                bulkCopy.ColumnMappings.Add("AssignDate", "AssignDate");

                                DataTable dt = new DataTable();
                                dt.Columns.Add("ProdStdCode", typeof(string));
                                dt.Columns.Add("SerialNum", typeof(string));
                                dt.Columns.Add("JobDetailType", typeof(string));
                                dt.Columns.Add("OrderNo", typeof(string));
                                dt.Columns.Add("SeqNo", typeof(string));
                                dt.Columns.Add("ResourceType", typeof(string));
                                dt.Columns.Add("BarcodeDataFormat", typeof(string));
                                dt.Columns.Add("BarcodeType", typeof(string));
                                dt.Columns.Add("SerialType", typeof(string));
                                dt.Columns.Add("idx_Group", typeof(long));
                                dt.Columns.Add("idx_Insert", typeof(long));
                                dt.Columns.Add("UseDate", typeof(DateTime));
                                dt.Columns.Add("InspectedDate", typeof(DateTime));
                                dt.Columns.Add("UseYN", typeof(string));
                                dt.Columns.Add("Status", typeof(string));
                                dt.Columns.Add("FileName", typeof(string));
                                dt.Columns.Add("Reserved1", typeof(string));
                                dt.Columns.Add("Reserved2", typeof(string));
                                dt.Columns.Add("Reserved3", typeof(string));
                                dt.Columns.Add("Reserved4", typeof(string));
                                dt.Columns.Add("Reserved5", typeof(string));
                                dt.Columns.Add("InsertUser", typeof(string));
                                dt.Columns.Add("InsertDate", typeof(DateTime));
                                dt.Columns.Add("UpdateUser", typeof(string));
                                dt.Columns.Add("UpdateDate", typeof(DateTime));
                                dt.Columns.Add("AssignDate", typeof(DateTime));

                                foreach (var item in items)
                                {
                                    dt.Rows.Add(
                                        item.ProdStdCode,
                                        item.SerialNum,
                                        item.JobDetailType,
                                        string.IsNullOrEmpty(item.OrderNo) ? (object)DBNull.Value : item.OrderNo,
                                        string.IsNullOrEmpty(item.SeqNo) ? (object)DBNull.Value : item.SeqNo,
                                        string.IsNullOrEmpty(item.ResourceType) ? "D" : item.ResourceType,
                                        string.IsNullOrEmpty(item.BarcodeDataFormat) ? (object)DBNull.Value : item.BarcodeDataFormat,
                                        string.IsNullOrEmpty(item.BarcodeType) ? (object)DBNull.Value : item.BarcodeType,
                                        string.IsNullOrEmpty(item.SerialType) ? (object)DBNull.Value : item.SerialType,
                                        item.idx_Group,
                                        item.idx_Insert,
                                        item.UseDate == null ? (object)DBNull.Value : item.UseDate,
                                        item.InspectedDate == null ? (object)DBNull.Value : item.InspectedDate,
                                        string.IsNullOrEmpty(item.UseYN) ? (object)DBNull.Value : item.UseYN,
                                        string.IsNullOrEmpty(item.Status) ? (object)DBNull.Value : item.Status,
                                        string.IsNullOrEmpty(item.FileName) ? (object)DBNull.Value : item.FileName,
                                        string.IsNullOrEmpty(item.Reserved1) ? (object)DBNull.Value : item.Reserved1,
                                        string.IsNullOrEmpty(item.Reserved2) ? (object)DBNull.Value : item.Reserved2,
                                        string.IsNullOrEmpty(item.Reserved3) ? (object)DBNull.Value : item.Reserved3,
                                        string.IsNullOrEmpty(item.Reserved4) ? (object)DBNull.Value : item.Reserved4,
                                        string.IsNullOrEmpty(item.Reserved5) ? (object)DBNull.Value : item.Reserved5,
                                        item.InsertUser,
                                        item.InsertDate,
                                        string.IsNullOrEmpty(item.UpdateUser) ? (object)DBNull.Value : item.UpdateUser,
                                        item.UpdateDate == null ? (object)DBNull.Value : item.UpdateDate,
                                        item.AssignDate == null ? (object)DBNull.Value : item.AssignDate);
                                }
                                System.Threading.Tasks.Task.Run(new Action(() => log.InfoFormat("SerialpoolController BulkInsertLocal : {0}", items.Count)));
                                bulkCopy.WriteToServer(dt);
                                sqlTran.Commit();
                                return true;
                            }
                        }
                        catch (Exception ex)
                        {
                            sqlTran.Rollback();
                            log.InfoFormat("SerialpoolController BulkInsertLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                            if (RaiseException)
                                throw new Exception("SerialpoolController BulkInsertLocal Exception", ex);
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.InfoFormat("SerialpoolController BulkInsertServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                    if (RaiseException)
                        throw new Exception("SerialpoolController BulkInsertServer Exception", ex);
                    return false;
                }
            }
        }
		public static bool BulkInsertLocal(List<SerialPool> items, bool RaiseException = false)
		{
			using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Local"].ToString()))
			{
				try
				{
					con.Open();
					using (SqlTransaction sqlTran = con.BeginTransaction())
					{
						try
						{
							using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con, SqlBulkCopyOptions.Default, sqlTran))
							{
								bulkCopy.DestinationTableName = "Dmn_SerialPool";

								bulkCopy.ColumnMappings.Add("ProdStdCode", "ProdStdCode");
								bulkCopy.ColumnMappings.Add("SerialNum", "SerialNum");
								bulkCopy.ColumnMappings.Add("JobDetailType", "JobDetailType");
								bulkCopy.ColumnMappings.Add("OrderNo", "OrderNo");
								bulkCopy.ColumnMappings.Add("SeqNo", "SeqNo");
								bulkCopy.ColumnMappings.Add("ResourceType", "ResourceType");
								bulkCopy.ColumnMappings.Add("BarcodeDataFormat", "BarcodeDataFormat");
								bulkCopy.ColumnMappings.Add("BarcodeType", "BarcodeType");
								bulkCopy.ColumnMappings.Add("SerialType", "SerialType");
								bulkCopy.ColumnMappings.Add("idx_Group", "idx_Group");
								bulkCopy.ColumnMappings.Add("idx_Insert", "idx_Insert");
								bulkCopy.ColumnMappings.Add("UseDate", "UseDate");
								bulkCopy.ColumnMappings.Add("InspectedDate", "InspectedDate");
								bulkCopy.ColumnMappings.Add("UseYN", "UseYN");
								bulkCopy.ColumnMappings.Add("Status", "Status");
								bulkCopy.ColumnMappings.Add("FileName", "FileName");
								bulkCopy.ColumnMappings.Add("Reserved1", "Reserved1");
								bulkCopy.ColumnMappings.Add("Reserved2", "Reserved2");
								bulkCopy.ColumnMappings.Add("Reserved3", "Reserved3");
								bulkCopy.ColumnMappings.Add("Reserved4", "Reserved4");
								bulkCopy.ColumnMappings.Add("Reserved5", "Reserved5");
								bulkCopy.ColumnMappings.Add("InsertUser", "InsertUser");
								bulkCopy.ColumnMappings.Add("InsertDate", "InsertDate");
								bulkCopy.ColumnMappings.Add("UpdateUser", "UpdateUser");
								bulkCopy.ColumnMappings.Add("UpdateDate", "UpdateDate");
								bulkCopy.ColumnMappings.Add("AssignDate", "AssignDate");

								DataTable dt = new DataTable();
								dt.Columns.Add("ProdStdCode", typeof(string));
								dt.Columns.Add("SerialNum", typeof(string));
								dt.Columns.Add("JobDetailType", typeof(string));
								dt.Columns.Add("OrderNo", typeof(string));
								dt.Columns.Add("SeqNo", typeof(string));
								dt.Columns.Add("ResourceType", typeof(string));
								dt.Columns.Add("BarcodeDataFormat", typeof(string));
								dt.Columns.Add("BarcodeType", typeof(string));
								dt.Columns.Add("SerialType", typeof(string));
								dt.Columns.Add("idx_Group", typeof(long));
								dt.Columns.Add("idx_Insert", typeof(long));
								dt.Columns.Add("UseDate", typeof(DateTime));
								dt.Columns.Add("InspectedDate", typeof(DateTime));
								dt.Columns.Add("UseYN", typeof(string));
								dt.Columns.Add("Status", typeof(string));
								dt.Columns.Add("FileName", typeof(string));
								dt.Columns.Add("Reserved1", typeof(string));
								dt.Columns.Add("Reserved2", typeof(string));
								dt.Columns.Add("Reserved3", typeof(string));
								dt.Columns.Add("Reserved4", typeof(string));
								dt.Columns.Add("Reserved5", typeof(string));
								dt.Columns.Add("InsertUser", typeof(string));
								dt.Columns.Add("InsertDate", typeof(DateTime));
								dt.Columns.Add("UpdateUser", typeof(string));
								dt.Columns.Add("UpdateDate", typeof(DateTime));
								dt.Columns.Add("AssignDate", typeof(DateTime));

								foreach (var item in items)
								{
									dt.Rows.Add(
										item.ProdStdCode,
										item.SerialNum,
										item.JobDetailType,
										string.IsNullOrEmpty(item.OrderNo) ? (object)DBNull.Value : item.OrderNo,
										string.IsNullOrEmpty(item.SeqNo) ? (object)DBNull.Value : item.SeqNo,
										string.IsNullOrEmpty(item.ResourceType) ? "D" : item.ResourceType,
										string.IsNullOrEmpty(item.BarcodeDataFormat) ? (object)DBNull.Value : item.BarcodeDataFormat,
										string.IsNullOrEmpty(item.BarcodeType) ? (object)DBNull.Value : item.BarcodeType,
										string.IsNullOrEmpty(item.SerialType) ? (object)DBNull.Value : item.SerialType,
										item.idx_Group,
										item.idx_Insert,
										item.UseDate == null ? (object)DBNull.Value : item.UseDate,
										item.InspectedDate == null ? (object)DBNull.Value : item.InspectedDate,
										string.IsNullOrEmpty(item.UseYN) ? (object)DBNull.Value : item.UseYN,
										string.IsNullOrEmpty(item.Status) ? (object)DBNull.Value : item.Status,
										string.IsNullOrEmpty(item.FileName) ? (object)DBNull.Value : item.FileName,
										string.IsNullOrEmpty(item.Reserved1) ? (object)DBNull.Value : item.Reserved1,
										string.IsNullOrEmpty(item.Reserved2) ? (object)DBNull.Value : item.Reserved2,
										string.IsNullOrEmpty(item.Reserved3) ? (object)DBNull.Value : item.Reserved3,
										string.IsNullOrEmpty(item.Reserved4) ? (object)DBNull.Value : item.Reserved4,
										string.IsNullOrEmpty(item.Reserved5) ? (object)DBNull.Value : item.Reserved5,
										item.InsertUser,
										item.InsertDate,
										string.IsNullOrEmpty(item.UpdateUser) ? (object)DBNull.Value : item.UpdateUser,
										item.UpdateDate == null ? (object)DBNull.Value : item.UpdateDate,
										item.AssignDate == null ? (object)DBNull.Value : item.AssignDate);
								}
								System.Threading.Tasks.Task.Run(new Action(() => log.InfoFormat("SerialpoolController BulkInsertLocal : {0}", items.Count)));
								bulkCopy.WriteToServer(dt);
								sqlTran.Commit();
								return true;
							}
						}
						catch (Exception ex)
						{
							sqlTran.Rollback();
							log.InfoFormat("SerialpoolController BulkInsertLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
							if (RaiseException)
								throw new Exception("SerialpoolController BulkInsertLocal Exception", ex);
							return false;
						}
					}
				}
				catch (Exception ex)
				{
					log.InfoFormat("SerialpoolController BulkInsertServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
					if (RaiseException)
						throw new Exception("SerialpoolController BulkInsertServer Exception", ex);
					return false;
				}
			}
		}

		public static bool ReportServer(List<DSM.Dmn_SerialPool> item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("SerialpoolController Report Start");
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_SerialPool where PlantCode = {0} and MachineID = {1} and OrderNo = {2} and SeqNo = {3}", item[0].PlantCode,
                        item[0].MachineID, item[0].OrderNo, item[0].SeqNo);
                    db.Dmn_SerialPool.AddRange(item);
                    db.SaveChanges();
                    log.InfoFormat("SerialpoolController Report End");
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController Report Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController Report Exception", ex);
                return false;
            }
        }

        public static bool UpdateServer(DSM.Dmn_SerialPool item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("SerialpoolController UpdateServer by {0}, {1}", item.UpdateUser, item.ToString());
                    var tmp = db.Dmn_SerialPool.First(q => q.PlantCode.Equals(item.PlantCode) && q.ProdStdCode.Equals(item.ProdStdCode) && q.SerialNum.Equals(item.SerialNum) && q.JobDetailType.Equals(item.JobDetailType) && q.Duplicate_Cnt.Equals(item.Duplicate_Cnt));
                    tmp.WriteExistings(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController UpdateServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController UpdateServer Exception", ex);
                return false;
            }
        }
        public static bool DeleteServerAll(string plantCode, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("SerialpoolController DeleteServerAll {0} by {1}", plantCode, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_SerialPool where PlantCode = {0}", plantCode);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController DeleteServerAll Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController DeleteServerAll Exception", ex);
                return false;
            }

        }
        public static bool DeleteServerSingle(string plantCode, string standardCode, string serialnumber, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("SerialpoolController DeleteServerSingle {0} {1} {2} by {3}", plantCode, standardCode, serialnumber, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_SerialPool where PlantCode = {0} AND ProdStdCode = {1} AND SerialNum = {2}", plantCode, standardCode, serialnumber);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController DeleteServerSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController DeleteServerSingle Exception", ex);
                return false;
            }
        }
        public static bool DeleteServerByOrder(string plantCode, string standardCode, string orderNo, string seqNo, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("SerialpoolController DeleteServerByOrder {0} {1} {2} {3} by {4}", plantCode, standardCode, orderNo, seqNo, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_SerialPool where PlantCode ={0} AND ProdStdCode = {1} AND OrderNo = {2} AND SeqNo = {3}", plantCode, standardCode, orderNo, seqNo);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController DeleteServerByOrder Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController DeleteServerByOrder Exception", ex);
                return false;
            }
        }
        public static bool UpdateStatus(string plantCode, string orderNo, string seqNo, string jobDetailType, string serialNum, string status, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("SerialpoolController UpdateStatus {0} {1} {2} {3} {4} by {5}", plantCode, orderNo + seqNo, jobDetailType, serialNum , status , userID);
                    db.Database.ExecuteSqlCommand("Update Dmn_SerialPool SET Status ={0} where PlantCode ={1} AND OrderNo ={2} AND SeqNo = {3} AND JobDetailType ={4} AND SerialNum ={5}", status ,plantCode, orderNo, seqNo, jobDetailType, serialNum);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController UpdateStatus Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController UpdateStatus Exception", ex);
                return false;
            }
        }
        public static bool AssignSerialNumber(string plantCode, string orderNo, string seqNo, string jobDetailType, DateTime assignDate, string machineID, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("SerialpoolController AssignSerialNumber {0} {1} by {2}", plantCode, orderNo + seqNo, userID);
                    db.Database.ExecuteSqlCommand($"UPDATE [DSM].[dbo].[Dmn_SerialPool]" +
                                                  $"SET AssignDate = '{assignDate.ToString("yyyy-MM-dd HH:mm:ss.fff")}', UpdateDate = '{assignDate.ToString("yyyy-MM-dd HH:mm:ss.fff")}', UpdateUser = '{userID}', MachineID = '{machineID}'" +
                                                  $"where PlantCode = '{plantCode}' and  OrderNo = '{orderNo}' and SeqNo = '{seqNo}' and JobDetailType = '{jobDetailType}'");
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController AssignSerialNumber Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController AssignSerialNumber Exception", ex);
                return false;
            }
        }
        public static bool AssignSerialNumber(string plantCode, string orderNo, string seqNo, string jobDetailType, string barcodeFormat, long startIdx, long endIdx, string machineID, DateTime assignDate,  string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("SerialpoolController AssignSerialNumber {0} {1} {2} {3} {4} by {5}", plantCode, orderNo + seqNo, jobDetailType, startIdx, endIdx, userID);
                    if (startIdx == endIdx)
                    {
                        db.Database.ExecuteSqlCommand("Update Dmn_SerialPool SET MachineID ={0}, AssignDate ={1}, UpdateDate ={1}, UpdateUser ={2} where PlantCode ={3} AND OrderNo ={4} AND SeqNo = {5} AND JobDetailType ={6} AND BarcodeDataFormat ={7} AND Idx_Insert ={8}",
                        machineID, assignDate, userID, plantCode, orderNo, seqNo, jobDetailType, barcodeFormat, startIdx);
                    }
                    else
                    {
                        db.Database.ExecuteSqlCommand("Update Dmn_SerialPool SET MachineID ={0}, AssignDate ={1}, UpdateDate ={1}, UpdateUser ={2} where PlantCode ={3} AND OrderNo ={4} AND SeqNo = {5} AND JobDetailType ={6} AND BarcodeDataFormat ={7} AND Idx_Insert >={8} AND Idx_Insert <={9}",
                            machineID, assignDate, userID, plantCode, orderNo, seqNo, jobDetailType, barcodeFormat, startIdx, endIdx);
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController AssignSerialNumber Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController AssignSerialNumber Exception", ex);
                return false;
            }
        }
        public static List<SerialPool> SelectLocalExceptSSCC(string standardCode, string serialNum, string jobDetailType, string orderNo, string seqNo,
            string resourceType, string status, DateTime? useDateStart, DateTime? useDateEnd, bool? use, int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            List<SerialPool> retList = new List<SerialPool>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_SerialPool
                               where a.BarcodeDataFormat != "SSCC"
                               select new { a };
                    if (!string.IsNullOrEmpty(standardCode))
                        list = list.Where(q => q.a.ProdStdCode.Contains(standardCode));
                    if (!string.IsNullOrEmpty(serialNum))
                        list = list.Where(q => q.a.SerialNum.Contains(serialNum));
                    if (!string.IsNullOrEmpty(jobDetailType))
                        list = list.Where(q => q.a.JobDetailType.Contains(jobDetailType));
                    if (!string.IsNullOrEmpty(orderNo))
                        list = list.Where(q => q.a.OrderNo.Contains(orderNo));
                    if (!string.IsNullOrEmpty(seqNo))
                        list = list.Where(q => q.a.SeqNo.Contains(seqNo));
                    if (!string.IsNullOrEmpty(resourceType))
                        list = list.Where(q => q.a.ResourceType.Equals(resourceType));
                    if (!string.IsNullOrEmpty(status))
                        list = list.Where(q => q.a.Status.Contains(status));
                    if (useDateStart != null)
                        list = list.Where(q => q.a.UseDate >= useDateStart.Value);
                    if (useDateEnd != null)
                        list = list.Where(q => q.a.UseDate >= useDateEnd.Value);
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Y") : q.a.UseYN.Equals("N"));
                    foreach (var item in list)
                    {
                        retList.Add(new SerialPool(item.a));
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
                log.InfoFormat("SerialpoolController SelectLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController SelectLocal Exception", ex);
            }
            return retList;
        }

        public static List<SerialPool> SelectLocalOnlySSCC(string standardCode, string prefix, string serialNum, string jobDetailType, string orderNo, string seqNo,
            string resourceType, string status, DateTime? useDateStart, DateTime? useDateEnd, bool? use, int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            List<SerialPool> retList = new List<SerialPool>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_SerialPool
                               where a.BarcodeDataFormat.Equals("SSCC")
                               select new { a };
                    if (!string.IsNullOrEmpty(standardCode))
                        list = list.Where(q => q.a.ProdStdCode.Contains(standardCode));
                    if (!string.IsNullOrEmpty(serialNum))
                        list = list.Where(q => q.a.SerialNum.Contains(serialNum));
                    if (!string.IsNullOrEmpty(jobDetailType))
                        list = list.Where(q => q.a.JobDetailType.Contains(jobDetailType));
                    if (!string.IsNullOrEmpty(orderNo))
                        list = list.Where(q => q.a.OrderNo.Contains(orderNo));
                    if (!string.IsNullOrEmpty(seqNo))
                        list = list.Where(q => q.a.SeqNo.Contains(seqNo));
                    if (!string.IsNullOrEmpty(resourceType))
                        list = list.Where(q => q.a.ResourceType.Equals(resourceType));
                    if (!string.IsNullOrEmpty(status))
                        list = list.Where(q => q.a.Status.Contains(status));
                    if (useDateStart != null)
                        list = list.Where(q => q.a.UseDate >= useDateStart.Value);
                    if (useDateEnd != null)
                        list = list.Where(q => q.a.UseDate >= useDateEnd.Value);
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Y") : q.a.UseYN.Equals("N"));
                    foreach (var item in list)
                    {
                        retList.Add(new SerialPool(item.a));
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
                log.InfoFormat("SerialpoolController SelectLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController SelectLocal Exception", ex);
            }
            return retList;
        }
        public static bool OtherEquipmentConnectionTest(string ip, bool RaiseException = false)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal(ip))
                {
                    string sql = "SELECT TOP 1 * FROM Dmn_SerialPool";
                    var list = db.Select<Local.Dmn_SerialPool>(sql);
                    if (list.Count > 0)
                        return true;

                    return db.Database.Exists();
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController SelectPreEquipmentSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController SelectPreEquipmentSingle Exception", ex);
            }
            return false;
        }
        public static async Task<SerialPool> SelectOtherEquipmentSingle(string ip, SGTIN serial, string status, bool RaiseException = false)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal(ip))
                {
					StringBuilder sql = new StringBuilder($"SELECT * FROM Dmn_SerialPool WHERE ");

					List<SqlParameter> parameters = new List<SqlParameter>();
					string stdCodeName = "@stdCode";
					string serialName = "@serial";
					string statusName = "@status";

					parameters.Add(new SqlParameter(stdCodeName, serial.StdCode ?? ""));
					parameters.Add(new SqlParameter(serialName, serial.Serial ?? ""));
					parameters.Add(new SqlParameter(statusName, status ?? ""));
					sql.Append("(");
					sql.Append($"SerialNum = {serialName} AND Status = {statusName}");
                    if (!string.IsNullOrEmpty(serial.StdCode))
					    sql.Append($" AND ProdStdCode = {stdCodeName}");
					sql.Append(")");

					var list = await db.Database.SqlQuery<Local.Dmn_SerialPool>(sql.ToString(), parameters.ToArray()).ToListAsync();
					var item = list.FirstOrDefault();
					if (item != null)
						return new SerialPool(item);
				}
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController SelectPreEquipmentSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController SelectPreEquipmentSingle Exception", ex);
            }
            return null;
        }
		public static async Task<List<SerialPool>> SelectOtherEquipment(string ip, List<SGTIN> serials, string status, bool RaiseException = false)
        {
            List<SerialPool> retList = new List<SerialPool>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal(ip))
                {
					DateTime now = DateTime.Now;
					StringBuilder sql = new StringBuilder("SELECT * FROM Dmn_SerialPool WHERE ");
					List<string> conditions = new List<string>();
					List<SqlParameter> parameters = new List<SqlParameter>();

					for (int i = 0; i < serials.Count; i++)
					{
						StringBuilder subConditions = new StringBuilder();
						string stdCodeParam = "@stdCode" + i;
						string serialNumParam = "@serialNum" + i;
						string statusName = "@status" + i;

						subConditions.Append("(");
						subConditions.Append($"SerialNum = {serialNumParam} AND Status = {statusName}");
						if (!string.IsNullOrEmpty(serials[i].StdCode))
							subConditions.Append($" AND ProdStdCode = {stdCodeParam}");
						subConditions.Append(")");
                        conditions.Add(subConditions.ToString());

						parameters.Add(new SqlParameter(stdCodeParam, serials[i].StdCode ?? ""));
						parameters.Add(new SqlParameter(serialNumParam, serials[i].Serial ?? ""));
						parameters.Add(new SqlParameter(statusName, status ?? ""));
					}

					sql.Append(string.Join(" OR ", conditions));
					var list = await db.Database.SqlQuery<Local.Dmn_SerialPool>(sql.ToString(), parameters.ToArray()).ToListAsync();
					log.InfoFormat("SelectLocal Elapsed {0}", (DateTime.Now - now).TotalMilliseconds);
					Console.WriteLine("SelectLocal Elapsed {0}", (DateTime.Now - now).TotalMilliseconds);

					foreach (var item in list)
						retList.Add(new SerialPool(item));
				}
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController SelectPreEquipmentSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController SelectPreEquipmentSingle Exception", ex);
                //throw ex;
            }
            return retList;
        }
        public static void RevertLastSerial(string orderNo, string seqNo, bool RaiseException = false)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    string sql = @";WITH CTE AS (
                                    SELECT TOP 1 ProdStdCode, SerialNum
                                    FROM [DOMINO_DB].[dbo].[Dmn_SerialPool]
                                    WHERE OrderNo = @OrderNo AND SeqNo = @SeqNo
                                    ORDER BY UseDate DESC
                                    )
                                    UPDATE sp
                                    SET sp.UseYN = 'N',
                                        sp.UseDate = NULL
                                    FROM [DOMINO_DB].[dbo].[Dmn_SerialPool] AS sp
                                    INNER JOIN CTE ON sp.ProdStdCode = CTE.ProdStdCode AND sp.SerialNum = CTE.SerialNum";
                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@OrderNo", orderNo),
                        new SqlParameter("@SeqNo", seqNo)
                    };
                    log.InfoFormat("SerialpoolController RevertLastSerial");
                    db.Database.ExecuteSqlCommand(sql, parameters);
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController SelectPreEquipmentSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController SelectPreEquipmentSingle Exception", ex);
            }
        }

        public static bool UpdateStatusQuery(string plantCode, string prodStdCode, string serialNum, string status, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat(@"SerialpoolController UpdateStatusQuery {0} {1} {2} {3} by {4}", plantCode, prodStdCode, serialNum, status, userID);

                    string qry = $@";WITH CTE as 
                                    (
                                        SELECT *
	                                    FROM Dmn_SerialPool A
	                                    WHERE A.PlantCode = '{plantCode}' AND A.ProdStdCode = '{prodStdCode}' AND A.SerialNum = '{serialNum}'
                                        UNION ALL
                                        SELECT A.*
	                                    FROM Dmn_SerialPool A 
	                                    INNER JOIN Dmn_ReadBarcode B 
	                                    ON A.PlantCode = B.PlantCode AND A.ProdStdCode = B.ProdStdCode AND A.SerialNum = B.SerialNum
	                                    INNER JOIN CTE C
                                        ON B.ParentProdStdCode = C.ProdStdCode AND B.ParentSerialNum = C.SerialNum
                                    )
                                    SELECT * INTO #TMP FROM CTE

                                    UPDATE A SET 
                                         A.Status = '{status}'
                                        ,A.UpdateUser = '{userID}'
                                        ,A.UpdateDate = GETDATE()
                                    FROM Dmn_SerialPool A 
                                    INNER JOIN #TMP B 
                                    ON A.PlantCode = B.PlantCode AND A.ProdStdCode = B.ProdStdCode AND A.SerialNum = B.SerialNum

                                    UPDATE A SET 
                                         A.Status = '{status}'
                                        ,A.UpdateUser = '{userID}'
                                        ,A.UpdateDate = GETDATE()
                                    FROM Dmn_ReadBarcode A 
                                    INNER JOIN #TMP B 
                                    ON A.PlantCode = B.PlantCode AND A.ProdStdCode = B.ProdStdCode AND A.SerialNum = B.SerialNum

                                    UPDATE A SET 
                                         A.Status = '{status}'
                                        ,A.UpdateUser = '{userID}'
                                        ,A.UpdateDate = GETDATE()
                                    FROM Dmn_VisionResult A 
                                    INNER JOIN #TMP B 
                                    ON A.PlantCode = B.PlantCode AND A.OrderNo = B.OrderNo AND A.SeqNo = B.SeqNo AND A.InsertDate = B.InspectedDate ";

                    db.Database.ExecuteSqlCommand(qry);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialpoolController UpdateStatusQuery Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialpoolController UpdateStatusQuery Exception", ex);
                return false;
            }
        }
    }
}
