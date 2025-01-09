using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using DominoFunctions;

namespace DominoDatabase.Controls
{
    public class ViewTableController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static long TotalPageCount { get; set; }
        public static long TotalCount { get; set; }
        public static List<DSM.Dmn_View_DSMData> SelectDSM(string plantCode, string serialNumber, string status, string orderNo, string seqNo, string prodCode, string prodName, DateTime? insertDateStart,
            DateTime? insertDateEnd, DateTime? inspectedDateStart, DateTime? inspectedDateEnd, DateTime? orderDateStart, DateTime? orderDateEnd, DateTime? completeDateStart, DateTime? completeDateEnd,
            bool? used, bool? inspected, int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            List < DSM.Dmn_View_DSMData> retList = new List<DSM.Dmn_View_DSMData>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_View_DSMDatas
                               select a;
                    if (!string.IsNullOrEmpty(plantCode))
                        list = list.Where(q => q.PlantCode.Equals(plantCode));
                    if (!string.IsNullOrEmpty(serialNumber))
                        list = list.Where(q => q.SerialNum.Contains(serialNumber));
                    if (!string.IsNullOrEmpty(status))
                        list = list.Where(q => q.Status.Equals(status));
                    if (!string.IsNullOrEmpty(orderNo))
                        list = list.Where(q => q.OrderNo.Equals(orderNo));
                    if (!string.IsNullOrEmpty(seqNo))
                        list = list.Where(q => q.SeqNo.Equals(seqNo));
                    if (!string.IsNullOrEmpty(prodCode))
                        list = list.Where(q => q.ProdCode.Equals(prodCode));
                    if (!string.IsNullOrEmpty(prodName))
                        list = list.Where(q => q.ProdName.ToLower().Contains(prodName.ToLower()));
                    if (insertDateStart != null)
                        list = list.Where(q => q.SP_InsertDate >= insertDateStart.Value);
                    if (insertDateEnd != null)
                        list = list.Where(q => q.SP_InsertDate >= insertDateEnd.Value);
                    if (inspectedDateStart != null)
                        list = list.Where(q => q.InspectedDate >= inspectedDateStart.Value);
                    if (inspectedDateEnd != null)
                        list = list.Where(q => q.InspectedDate >= inspectedDateEnd.Value);
                    if (orderDateStart != null)
                        list = list.Where(q => q.StartDate >= orderDateStart.Value);
                    if (orderDateEnd != null)
                        list = list.Where(q => q.StartDate >= orderDateEnd.Value);
                    if (completeDateStart != null)
                        list = list.Where(q => q.CompleteDate >= completeDateStart.Value);
                    if (completeDateEnd != null)
                        list = list.Where(q => q.CompleteDate >= completeDateEnd.Value);
                    if(used != null)
                        list = list.Where(q => (bool)used ? q.UseYN.Equals("Yes") : q.UseYN.Equals("No"));
                    if (inspected != null)
                        list = list.Where(q => (bool)inspected ? q.InspectedDate != null : q.InspectedDate == null);
                    foreach (var item in list)
                    {
                        retList.Add(new DSM.Dmn_View_DSMData(item));
                    }
                    if (pageIndex >= 0)
                    {
                        TotalCount = retList.Count();
                        TotalPageCount = Convert.ToInt64(Math.Ceiling(Convert.ToDouble(TotalCount) / Convert.ToDouble(pageSize)));
                        retList = retList.OrderBy(x => x.SP_InsertDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    }
                }
                retList = retList.OrderByDescending(q => q.JobDetailType.Equals("LBL")).ThenByDescending(q => q.JobDetailType.Equals("EA")).ThenBy(q => q.JobDetailType).ToList();
                return retList;
            }
            catch (Exception ex)
            {
                log.InfoFormat("ViewTableController SelectDSM Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ViewTableController SelectDSM Exception", ex);
            }
            return retList;
        }
        public static List<View_DSMData> SelectDSMData(string plantCode, string orderNo, string seqNo, string jobdetailType, string serialNum, string status, 
            int pageIndex, int pageSize, out int? count, bool RaiseException = false )
        {
            count = 0;
            List<View_DSMData> retList = new List<View_DSMData>();

            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    string sqlCount = string.Format(@"SELECT COUNT(1) FROM Dmn_View_DSMData A
                                                       WHERE A.PlantCode = '{0}'  
	                                                     AND A.OrderNo = '{1}' 
	                                                     AND A.SeqNo = '{2}' 
                                                         AND ('{3}' = '' OR A.JobDetailType = '{3}') 
                                                         AND ('{4}' = '' OR A.SerialNum = '{4}')
                                                         AND A.Status IN ({5}) ",
                                                         plantCode, orderNo, seqNo, jobdetailType, serialNum, status);
                    string sql = string.Format(@"SELECT * FROM Dmn_View_DSMData A
                                                  WHERE A.PlantCode = '{0}'  
	                                                AND A.OrderNo = '{1}' 
	                                                AND A.SeqNo = '{2}' 
                                                    AND ('{3}' = '' OR A.JobDetailType = '{3}') 
                                                    AND ('{4}' = '' OR A.SerialNum = '{4}')
                                                    AND A.Status IN ({5})
                                               ORDER BY A.SerialNum OFFSET {6} ROW 
			                                 FETCH NEXT {7} ROW ONLY ",
                                                        plantCode, orderNo, seqNo, jobdetailType, serialNum, status, (pageIndex - 1) * pageSize, pageSize);
                    try
                    {
                        count = db.Database.SqlQuery<int>(sqlCount).FirstOrDefault();
                    }
                    catch { }
                    retList = db.Select<View_DSMData>(sql);
                }
                return retList;
            }
            catch (Exception ex)
            {
                log.InfoFormat("ViewTableController SelectDSMData Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ViewTableController SelectDSMData Exception", ex);
            }
            return retList;
        }
        public static List<View_DSMData> SelectDefaultReport(string plantCode, string orderNo, string seqNo, bool RaiseException = false)
        {
            List<View_DSMData> retList = new List<View_DSMData>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    string sql = string.Format(@"SELECT *
                                                   FROM Dmn_View_DSMData A
                                                  WHERE A.PlantCode = '{0}'  
	                                                AND A.OrderNo = '{1}' 
	                                                AND A.SeqNo = '{2}'
                                                    AND A.JobDetailType = 'EA'
                                                    AND A.Status = 'Pass'
                                               ORDER BY A.SerialNum ",
                                                        plantCode, orderNo, seqNo);
                    retList = db.Select<View_DSMData>(sql);
                }
                return retList;
            }
            catch (Exception ex)
            {
                log.InfoFormat("ViewTableController SelectDSMDataReport Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ViewTableController SelectDSMDataReport Exception", ex);
            }
            return retList;
        }
        public static List<View_DSMData> SelectReportList(string plantCode, string orderNo, string seqNo, string status, bool RaiseException = false)
        {
            List<View_DSMData> retList = new List<View_DSMData>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    string sql = string.Format(@" EXEC SP_DSM_DATA '{0}', '{1}', '{2}', '{3}' ", plantCode, orderNo, seqNo, status);
                    retList = db.Select<View_DSMData>(sql);
                }
                return retList;
            }
            catch (Exception ex)
            {
                log.InfoFormat("ViewTableController SelectDSMDataReport Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ViewTableController SelectDSMDataReport Exception", ex);
            }
            return retList;
        }
        public static DSM.Dmn_View_DSMData SelectDSMSingle(string plantCode, string serialNumber, string status, string orderNo, string seqNo, string prodCode, string prodName, DateTime? insertDateStart,
            DateTime? insertDateEnd, DateTime? inspectedDateStart, DateTime? inspectedDateEnd, DateTime? orderDateStart, DateTime? orderDateEnd, DateTime? completeDateStart, DateTime? completeDateEnd,
            bool? used, bool? inspected, bool RaiseException = false)
        {
            List<DSM.Dmn_View_DSMData> retList = new List<DSM.Dmn_View_DSMData>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_View_DSMDatas
                               select a;
                    if (!string.IsNullOrEmpty(plantCode))
                        list = list.Where(q => q.PlantCode.Equals(plantCode));
                    if (!string.IsNullOrEmpty(serialNumber))
                        list = list.Where(q => q.PlantCode.Contains(serialNumber));
                    if (!string.IsNullOrEmpty(status))
                        list = list.Where(q => q.Status.Equals(status));
                    if (!string.IsNullOrEmpty(orderNo))
                        list = list.Where(q => q.OrderNo.Equals(orderNo));
                    if (!string.IsNullOrEmpty(seqNo))
                        list = list.Where(q => q.SeqNo.Equals(seqNo));
                    if (!string.IsNullOrEmpty(prodCode))
                        list = list.Where(q => q.ProdCode.Equals(prodCode));
                    if (!string.IsNullOrEmpty(prodName))
                        list = list.Where(q => q.ProdName.ToLower().Contains(prodName.ToLower()));
                    if (insertDateStart != null)
                        list = list.Where(q => q.SP_InsertDate >= insertDateStart.Value);
                    if (insertDateEnd != null)
                        list = list.Where(q => q.SP_InsertDate >= insertDateEnd.Value);
                    if (inspectedDateStart != null)
                        list = list.Where(q => q.InspectedDate >= inspectedDateStart.Value);
                    if (inspectedDateEnd != null)
                        list = list.Where(q => q.InspectedDate >= inspectedDateEnd.Value);
                    if (orderDateStart != null)
                        list = list.Where(q => q.StartDate >= orderDateStart.Value);
                    if (orderDateEnd != null)
                        list = list.Where(q => q.StartDate >= orderDateEnd.Value);
                    if (completeDateStart != null)
                        list = list.Where(q => q.CompleteDate >= completeDateStart.Value);
                    if (completeDateEnd != null)
                        list = list.Where(q => q.CompleteDate >= completeDateEnd.Value);
                    if (used != null)
                        list = list.Where(q => (bool)used ? q.UseYN.Equals("Yes") : q.UseYN.Equals("No"));
                    if (inspected != null)
                        list = list.Where(q => (bool)inspected ? q.InspectedDate != null : q.InspectedDate == null);
                    foreach (var item in list)
                    {
                        retList.Add(new DSM.Dmn_View_DSMData(item));
                    }
                }
                return retList.FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.InfoFormat("ViewTableController SelectDSMSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ViewTableController SelectDSMSingle Exception", ex);
            }
            return retList.FirstOrDefault();
        }
        public static List<Local.Dmn_View_PMData> SelectLocalPM(string standardCode, string orderNo, string seqNo, DateTime? insertDateStart, DateTime? insertDateEnd, DateTime? InspectedDateStart,
            DateTime? InspectedDateEnd, DateTime? OrderDateStart, DateTime? OrderDateEnd, bool? used, bool? inspected, string status, string serialNumber, int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            List<Local.Dmn_View_PMData> retList = new List<Local.Dmn_View_PMData>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_View_PMData
                               select a;
                    if (!string.IsNullOrEmpty(standardCode))
                        list = list.Where(q => q.ProdStdCode.Contains(standardCode));
                    if (!string.IsNullOrEmpty(orderNo))
                        list = list.Where(q => q.OrderNo.Contains(orderNo));
                    if (!string.IsNullOrEmpty(seqNo))
                        list = list.Where(q => q.SeqNo.Contains(seqNo));
                    if (insertDateStart != null)
                        list = list.Where(q => q.UseDate >= insertDateStart.Value);
                    if (insertDateEnd != null)
                        list = list.Where(q => q.UseDate <= insertDateEnd.Value);
                    if (InspectedDateStart != null)
                        list = list.Where(q => q.InspectedDate >= InspectedDateStart.Value);
                    if (InspectedDateEnd != null)
                        list = list.Where(q => q.InspectedDate >= InspectedDateEnd.Value);
                    if (OrderDateStart != null)
                        list = list.Where(q => q.StartDate >= OrderDateStart.Value);
                    if (OrderDateEnd != null)
                        list = list.Where(q => q.StartDate <= OrderDateEnd.Value);
                    if (used != null)
                        list = list.Where(q => (bool)used ? q.UseYN.Equals("Yes") : q.UseYN.Equals("No"));
                    if (inspected != null)
                        list = list.Where(q => (bool)inspected ? q.InspectedDate != null : q.InspectedDate == null);
                    if (!string.IsNullOrEmpty(status))
                        list = list.Where(q => q.Status.Equals(status));
                    if (!string.IsNullOrEmpty(serialNumber))
                        list = list.Where(q => q.SerialNum.Contains(serialNumber));
                    foreach (var item in list)
                    {
                        retList.Add(new Local.Dmn_View_PMData(item));
                    }
                    if (pageIndex >= 0)
                    {
                        TotalCount = retList.Count();
                        TotalPageCount = Convert.ToInt64(Math.Ceiling(Convert.ToDouble(TotalCount) / Convert.ToDouble(pageSize)));
                        retList = retList.OrderBy(x => x.SP_InsertDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    }
                }
                return retList;
            }
            catch (Exception ex)
            {
                log.InfoFormat("ViewTableController SelectLocalPM Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ViewTableController SelectLocalPM Exception", ex);
            }
            return retList;
        }
        public static Local.Dmn_View_PMData SelectLocalPMSingle(string standardCode, string productCode, string productName, string serialNum, string jobDetailType, string orderNo, string seqNo, string lot,
            string resourceType, string status, DateTime? useDateStart, DateTime? useDateEnd, bool? use, int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            List<Local.Dmn_View_PMData> retList = new List<Local.Dmn_View_PMData>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_SerialPool
                               join b in db.Dmn_Product_M on a.ProdStdCode equals b.ProdStdCode
                               join c in db.Dmn_VisionResult on a.InspectedDate equals c.InsertDate
                               join d in db.Dmn_JobOrder_M on new { a.OrderNo, a.SeqNo } equals new { d.OrderNo, d.SeqNo }
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
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Yes") : q.a.UseYN.Equals("No"));
                    foreach (var item in list)
                    {
                        retList.Add(new Local.Dmn_View_PMData(item));
                    }
                }
                return retList.FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.InfoFormat("ViewTableController SelectLocalPMSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ViewTableController SelectLocalPMSingle Exception", ex);
            }
            return retList.FirstOrDefault();
        }
        public static Local.Dmn_View_AGData SelectLocalAGSingle(string standardCode, string productCode, string productName, string serialNum, string jobDetailType, string orderNo, string seqNo, string lot,
            string resourceType, string status, DateTime? useDateStart, DateTime? useDateEnd, bool? use, int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            List<Local.Dmn_View_AGData> retList = new List<Local.Dmn_View_AGData>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_SerialPool
                               join b in db.Dmn_Product_M on a.ProdStdCode equals b.ProdStdCode
                               join c in db.Dmn_VisionResult on a.InspectedDate equals c.InsertDate
                               join d in db.Dmn_JobOrder_M on new { a.OrderNo, a.SeqNo } equals new { d.OrderNo, d.SeqNo }
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
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Yes") : q.a.UseYN.Equals("No"));
                    foreach (var item in list)
                    {
                        retList.Add(new Local.Dmn_View_AGData(item));
                    }
                }
                return retList.FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.InfoFormat("ViewTableController SelectLocalPMSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ViewTableController SelectLocalPMSingle Exception", ex);
            }
            return retList.FirstOrDefault();
        }
        public static List<Local.Dmn_View_AGData> SelectAGDataProcedure(string orderNo, string seqNo, string stdCode = "", string serial = "", string jobDetailType = "",
            DateTime? assignDateStart = null, DateTime? assignDateEnd = null, DateTime? insertDateStart = null, DateTime? insertDateEnd = null, bool? use = null, int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            List<Local.Dmn_View_AGData> retList = new List<Local.Dmn_View_AGData>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    string sql = string.Format(@"exec [dbo].[AGTable] '{0}', '{1}'", orderNo, seqNo);
                    var tmp = db.Database.SqlQuery<Local.Dmn_View_AGData>(sql).ToList();
                    if (!string.IsNullOrEmpty(stdCode))
                    {
                        var tmpStdCode = stdCode.Substring(1, stdCode.Length - 2);
                        tmp = tmp.Where(q => q.StandardCode.Contains(tmpStdCode)).ToList();
                    }
                    if (!string.IsNullOrEmpty(serial))
                        tmp = tmp.Where(q => q.SerialNum.Contains(serial)).ToList();
                    if (!string.IsNullOrEmpty(jobDetailType))
                        tmp = tmp.Where(q => q.JobDetailType == jobDetailType).ToList();
                    if (assignDateStart != null)
                        tmp = tmp.Where(q => q.JM_AssignDate >= assignDateStart).ToList();
                    if (assignDateEnd != null)
                        tmp = tmp.Where(q => q.JM_AssignDate >= assignDateEnd).ToList();
                    if (insertDateStart != null)
                        tmp = tmp.Where(q => q.RB_InsertDate >= insertDateStart).ToList();
                    if (insertDateEnd != null)
                        tmp = tmp.Where(q => q.RB_InsertDate >= insertDateEnd).ToList();
                    if (use != null)
                        tmp = tmp.Where(q => q.fSP_UseYN == use).ToList();

                    retList = tmp;
                }
                return retList;
            }
            catch (Exception ex)
            {
                log.InfoFormat("ViewTableController SelectAGDataProcedure Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ViewTableController SelectAGDataProcedure Exception", ex);
            }
            return retList;
        }
        public static List<DSM.Dmn_View_DSMData> SelectDSMDataProcedure(string plantCode, string orderNo, string seqNo, string status, bool RaiseException = false)
        {
            List<DSM.Dmn_View_DSMData> retList = new List<DSM.Dmn_View_DSMData>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    string sql = string.Format(@"exec [dbo].[SP_DSM_DATA] '{0}', '{1}', '{2}', '{3}'",
                                                        plantCode, orderNo, seqNo, status);
                    retList = db.Database.SqlQuery<DSM.Dmn_View_DSMData>(sql).ToList();
                }
                return retList;
            }
            catch (Exception ex)
            {
                log.InfoFormat("ViewTableController SelectDSMDataProcedure Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ViewTableController SelectDSMDataProcedure Exception", ex);
            }
            return retList;
        }

    }
}
