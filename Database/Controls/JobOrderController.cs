using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using DominoDatabase.DSM;
using DominoFunctions.Enums;
using DominoFunctions.ExtensionMethod;
using log4net;

namespace DominoDatabase.Controls
{
    public class JobOrderController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static long TotalPageCount { get; set; }
        public static long TotalCount { get; set; }
        public static List<JobOrder> SelectLocalPM (string jobdetailtype, string orderNo, string seqNo, string lot, string productCode, string standardCode, string productName, string status, string orderType,
            DateTime? orderdateStart, DateTime? orderdateEnd, DateTime? completeStart, DateTime? completeEnd, bool? use , int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            List<JobOrder> retList = new List<JobOrder>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_JobOrder_M
                               join b in db.Dmn_JobOrder_PM on new { a.OrderNo, a.SeqNo } equals new { b.OrderNo, b.SeqNo } into bb
                               from b in bb.DefaultIfEmpty()
                               join c in db.Dmn_Product_M on a.ProdCode equals c.ProdCode into cc
                               from c in cc.DefaultIfEmpty()
                               join d in db.Dmn_Product_PM on new { c.ProdCode, b.JobDetailType } equals new { d.ProdCode, d.JobDetailType } into dd
                               from d in dd.DefaultIfEmpty()
                               join e in db.Dmn_Serial_Expression on d.SnExpressionID equals e.SnExpressionID into ee
                               from e in ee.DefaultIfEmpty()
                               join h in db.Dmn_CustomBarcodeFormat on d.BarcodeDataFormat equals h.CustomBarcodeFormatID into hh
                               from h in hh.DefaultIfEmpty()
                               select new
                               {
                                   a,
                                   b,
                                   c,
                                   d,
                                   e,
                                   h,
                                   cnt_total = (from f in db.Dmn_SerialPool where f.OrderNo.Equals(a.OrderNo) && f.SeqNo.Equals(a.SeqNo) && f.JobDetailType.Equals(b.JobDetailType) && f.SerialType.Equals(d.SnType) select f).Count(),
                                   cnt_notused = (from f in db.Dmn_SerialPool where f.OrderNo.Equals(a.OrderNo) && f.SeqNo.Equals(a.SeqNo) && f.JobDetailType.Equals(b.JobDetailType) && f.UseYN != "Y" && f.SerialType.Equals(d.SnType) select f).Count()
                               };
                    if (!string.IsNullOrEmpty(orderNo))
                        list = list.Where(q => q.a.OrderNo.Equals(orderNo));
                    if (!string.IsNullOrEmpty(seqNo))
                        list = list.Where(q => q.a.SeqNo.Equals(seqNo));
                    if (!string.IsNullOrEmpty(jobdetailtype))
                        list = list.Where(q => q.b.JobDetailType.Equals(jobdetailtype));
                    if (!string.IsNullOrEmpty(lot))
                        list = list.Where(q => q.a.LotNo.Contains(lot));
                    if (!string.IsNullOrEmpty(productCode))
                        list = list.Where(q => q.a.ProdCode.Contains(productCode));
                    if (!string.IsNullOrEmpty(standardCode))
                        list = list.Where(q => q.c.ProdStdCode.Contains(standardCode));
                    if (!string.IsNullOrEmpty(productName))
                        list = list.Where(q => q.c.ProdName.Contains(productName));
                    if (!string.IsNullOrEmpty(status))
                        list = list.Where(q => q.b.JobStatus.Equals(status));
                    if (!string.IsNullOrEmpty(orderType))
                        list = list.Where(q => q.a.OrderType.Equals(orderType));
                    if (orderdateStart != null)
                        list = list.Where(q => q.a.InsertDate >= orderdateStart.Value);
                    if (orderdateEnd != null)
                        list = list.Where(q => q.a.InsertDate <= orderdateEnd.Value);
                    if (completeStart != null)
                        list = list.Where(q => q.b.CompleteDate >= completeStart.Value);
                    if (completeEnd != null)
                        list = list.Where(q => q.b.CompleteDate <= completeEnd.Value);
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Y") : q.a.UseYN.Equals("N"));
                    foreach (var item in list)
                    {
                        if (!retList.Any(x => x.OrderNo.Equals(item.a.OrderNo) && x.SeqNo.Equals(item.a.SeqNo)))
                        {
                            retList.Add(new JobOrder(item.a)
                            {
                                ProductStdCode = item.c == null ? null : item.c.ProdStdCode,
                                ProductName = item.c == null ? null : string.Format("{0} {1}", item.c.ProdName, item.c.ProdName2),
                                ProductName1 = item.c == null ? null : item.c.ProdName,
                                ProductName2 = item.c == null ? null : item.c.ProdName2,
                                AGLevel = item.c == null ? null : item.c.AGLevel,
                                Delay_NG = item.c == null || item.c.Delay_NG == null ? 0 : (int)item.c.Delay_NG,
                                Delay_Print = item.c == null || item.c.Delay_Print == null ? 0 : (int)item.c.Delay_Print,
								Delay_Print2 = item.c == null || item.c.Delay_Print2 == null ? 0 : (int)item.c.Delay_Print2,
								Delay_Shot1 = item.c == null || item.c.Delay_Shot1 == null ? 0 : (int)item.c.Delay_Shot1,
                                Delay_Shot2 = item.c == null || item.c.Delay_Shot2 == null ? 0 : (int)item.c.Delay_Shot2,
                                ProdName = item.c == null ? null : item.c.ProdName,
                                ProdName2 = item.c == null ? null : item.c.ProdName2,
                                MedicineType = item.c?.MedicineType,
                                ProductReserved1 = item.c == null ? null : item.c.Reserved1,
                                ProductReserved2 = item.c == null ? null : item.c.Reserved2,
                                ProductReserved3 = item.c == null ? null : item.c.Reserved3,
                                ProductReserved4 = item.c == null ? null : item.c.Reserved4,
                                ProductReserved5 = item.c == null ? null : item.c.Reserved5,
								BarcodeDataFormat = item.d == null ? null : item.d.BarcodeDataFormat,
                            });
                        }
                        retList.Single(x => x.OrderNo.Equals(item.a.OrderNo) && x.SeqNo.Equals(item.a.SeqNo)).JobOrderDetail.Add(new JobOrderDetail(item.b)
                        {
                            SnExpressionID = item.e == null ? null : item.e.SnExpressionID,
                            SnExpressionStr = item.e == null ? null : item.e.SnExpressionStr,
                            SnType = item.d == null ? null : item.d.SnType,
                            BarcodeDataFormat = item.d == null ? null : item.d.BarcodeDataFormat,
                            BarcodeType = item.d == null ? null : item.d.BarcodeType,
                            Cnt_SerialTotal = item.cnt_total,
                            Cnt_SerialNotUsed = item.cnt_notused,
                            DesignID = item.d == null ? null : item.d.DesignID,
                            DesignID2 = item.d == null ? null : item.d.DesignID2,
                            Capacity = item.d == null ? null : item.d.Capacity,
                            ResourceType = item.d == null ? null : item.d.ResourceType,
                            PharmaCode = item.d == null ? null : item.d.PharmaCode,
                            Price = item.d == null ? null : item.d.Price,
                            ExtractSerialStart = item.d == null ? null : item.d.ExtractSerialStart,
                            ExtractSerialEnd = item.d == null ? null : item.d.ExtractSerialEnd,
                            MinimumWeight = item.d == null ? null : item.d.MinimumWeight,
                            MaximumWeight = item.d == null ? null : item.d.MaximumWeight,
                            CustomBarcodeFormat = item.h == null ? null : item.h.CustomBarcodeFormatStr,
                            ProdStdCode = item.d == null ? null : item.d.ProdStdCode,
                            ContentCount = item.d == null ? null : item.d.ContentCount
                        });
                    }
                    if(pageIndex >= 0)
                    {
                        TotalCount = retList.Count();
                        TotalPageCount = Convert.ToInt64(Math.Ceiling(Convert.ToDouble(TotalCount) / Convert.ToDouble(pageSize)));
                        retList = retList.OrderBy(x => x.OrderNo).ThenBy(x => x.SeqNo).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    }
                }
                return retList;
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController SelectLocalPM Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController SelectLocalPM Exception", ex);
            }
            return retList;
        }
        public static JobOrder SelectLocalSinglePM(string orderNo, string seqNo, string lot, string productCode, string productName, string status, string orderType, DateTime? orderdateStart,
             DateTime? orderdateEnd, DateTime? completeStart, DateTime? completeEnd, bool? use, bool RaiseException = false)
        {
            List<JobOrder> retList = new List<JobOrder>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_JobOrder_M
                               join b in db.Dmn_JobOrder_PM on new { a.OrderNo, a.SeqNo } equals new { b.OrderNo, b.SeqNo } into bb
                               from b in bb.DefaultIfEmpty()
                               join c in db.Dmn_Product_M on a.ProdCode equals c.ProdCode into cc
                               from c in cc.DefaultIfEmpty()
                               join d in db.Dmn_Product_PM on new { c.ProdCode, b.JobDetailType } equals new { d.ProdCode, d.JobDetailType } into dd
                               from d in dd.DefaultIfEmpty()
                               join e in db.Dmn_Serial_Expression on d.SnExpressionID equals e.SnExpressionID into ee
                               from e in ee.DefaultIfEmpty()
                               join h in db.Dmn_CustomBarcodeFormat on d.BarcodeDataFormat equals h.CustomBarcodeFormatID into hh
                               from h in hh.DefaultIfEmpty()
                               select new
                               {
                                   a,
                                   b,
                                   c,
                                   d,
                                   e,
                                   h,
                                   cnt_total = (from f in db.Dmn_SerialPool where f.OrderNo.Equals(a.OrderNo) && f.SeqNo.Equals(a.SeqNo) && f.JobDetailType.Equals(b.JobDetailType) select f).Count(),
                                   cnt_notused = (from f in db.Dmn_SerialPool where f.OrderNo.Equals(a.OrderNo) && f.SeqNo.Equals(a.SeqNo) && f.JobDetailType.Equals(b.JobDetailType) && f.UseYN != "Y" select f).Count()
                               };
                    if (!string.IsNullOrEmpty(orderNo))
                        list = list.Where(q => q.a.OrderNo.Equals(orderNo));
                    if (!string.IsNullOrEmpty(seqNo))
                        list = list.Where(q => q.a.SeqNo.Equals(seqNo));
                    if (!string.IsNullOrEmpty(lot))
                        list = list.Where(q => q.a.LotNo.Contains(lot));
                    if (!string.IsNullOrEmpty(productCode))
                        list = list.Where(q => q.a.ProdCode.Contains(productCode));
                    if(!string.IsNullOrEmpty(productName))
                        list = list.Where(q => q.c.ProdName.Contains(productName));
                    if (!string.IsNullOrEmpty(status))
                        list = list.Where(q => q.b.JobStatus.Equals(status));
                    if (!string.IsNullOrEmpty(orderType))
                        list = list.Where(q => q.a.OrderType.Equals(orderType));
                    if (orderdateStart != null)
                        list = list.Where(q => q.a.InsertDate >= orderdateStart.Value);
                    if (orderdateEnd != null)
                        list = list.Where(q => q.a.InsertDate <= orderdateEnd.Value);
                    if (completeStart != null)
                        list = list.Where(q => q.b.CompleteDate >= completeStart.Value);
                    if (completeEnd != null)
                        list = list.Where(q => q.b.CompleteDate <= completeEnd.Value);
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Y") : q.a.UseYN.Equals("N"));
                    foreach (var item in list)
                    {
                        if (!retList.Any(x => x.OrderNo.Equals(item.a.OrderNo) && x.SeqNo.Equals(item.a.SeqNo)))
                        {
                            retList.Add(new JobOrder(item.a)
                            {
                                ProductStdCode = item.c == null ? null : item.c.ProdStdCode,
                                ProductName = item.c == null ? null : string.Format("{0} {1}", item.c.ProdName, item.c.ProdName2),
                                ProductName1 = item.c == null ? null : item.c.ProdName,
                                ProductName2 = item.c == null ? null : item.c.ProdName2,
                                AGLevel = item.c == null ? null : item.c.AGLevel,
                                Delay_NG = item.c == null || item.c.Delay_NG == null ? 0 : (int)item.c.Delay_NG,
                                Delay_Print = item.c == null || item.c.Delay_Print == null ? 0 : (int)item.c.Delay_Print,
								Delay_Print2 = item.c == null || item.c.Delay_Print2 == null ? 0 : (int)item.c.Delay_Print2,
								Delay_Shot1 = item.c == null || item.c.Delay_Shot1 == null ? 0 : (int)item.c.Delay_Shot1,
                                Delay_Shot2 = item.c == null || item.c.Delay_Shot2 == null ? 0 : (int)item.c.Delay_Shot2,
                                ProdName = item.c == null ? null : item.c.ProdName,
                                ProdName2 = item.c == null ? null : item.c.ProdName2,
								MedicineType = item.c?.MedicineType,
								ProductReserved1 = item.c == null ? null : item.c.Reserved1,
                                ProductReserved2 = item.c == null ? null : item.c.Reserved2,
                                ProductReserved3 = item.c == null ? null : item.c.Reserved3,
                                ProductReserved4 = item.c == null ? null : item.c.Reserved4,
                                ProductReserved5 = item.c == null ? null : item.c.Reserved5,
								BarcodeDataFormat = item.d == null ? null : item.d.BarcodeDataFormat,
                            });
                        }
                        retList.Single(x => x.OrderNo.Equals(item.a.OrderNo) && x.SeqNo.Equals(item.a.SeqNo)).JobOrderDetail.Add(new JobOrderDetail(item.b)
                        {
                            SnExpressionID = item.e == null ? null : item.e.SnExpressionID,
                            SnExpressionStr = item.e == null ? null : item.e.SnExpressionStr,
                            SnType = item.d == null ? null : item.d.SnType,
                            BarcodeDataFormat = item.d == null ? null : item.d.BarcodeDataFormat,
                            BarcodeType = item.d == null ? null : item.d.BarcodeType,
                            Cnt_SerialTotal = item.cnt_total,
                            Cnt_SerialNotUsed = item.cnt_notused,
                            DesignID = item.d == null ? null : item.d.DesignID,
                            DesignID2 = item.d == null ? null : item.d.DesignID2,
                            UsePrinterGroup1 = item.d == null ? false : item.d.UsePrinterGroup1,
							UsePrinterGroup2 = item.d == null ? false : item.d.UsePrinterGroup2,
							Capacity = item.d == null ? null : item.d.Capacity,
                            ResourceType = item.d == null ? null : item.d.ResourceType,
                            PharmaCode = item.d == null ? null : item.d.PharmaCode,
                            Price = item.d == null ? null : item.d.Price,
                            ExtractSerialStart = item.d == null ? null : item.d.ExtractSerialStart,
                            ExtractSerialEnd = item.d == null ? null : item.d.ExtractSerialEnd,
                            MinimumWeight = item.d == null ? null : item.d.MinimumWeight,
                            MaximumWeight = item.d == null ? null : item.d.MaximumWeight,
                            CustomBarcodeFormat = item.h == null ? null : item.h.CustomBarcodeFormatStr,
                            ProdStdCode = item.d == null ? null : item.d.ProdStdCode,
                            ContentCount = item.d == null ? null : item.d.ContentCount
                        });
                    }
                    return retList.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController SelectLocalSinglePM Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController SelectLocalSinglePM Exception", ex);
            }
            return retList.FirstOrDefault();
        }
        public static JobOrder SelectLocalSinglePMERP(string erp, DateTime? orderdateStart, DateTime? orderdateEnd, DateTime? completeStart, DateTime? completeEnd, bool? use, bool RaiseException = false)
        {
            List<JobOrder> retList = new List<JobOrder>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_JobOrder_M
                               join b in db.Dmn_JobOrder_PM on new { a.OrderNo, a.SeqNo } equals new { b.OrderNo, b.SeqNo } into bb
                               from b in bb.DefaultIfEmpty()
                               join c in db.Dmn_Product_M on a.ProdCode equals c.ProdCode into cc
                               from c in cc.DefaultIfEmpty()
                               join d in db.Dmn_Product_PM on new { c.ProdCode, b.JobDetailType } equals new { d.ProdCode, d.JobDetailType } into dd
                               from d in dd.DefaultIfEmpty()
                               join e in db.Dmn_Serial_Expression on d.SnExpressionID equals e.SnExpressionID into ee
                               from e in ee.DefaultIfEmpty()
                               join h in db.Dmn_CustomBarcodeFormat on d.BarcodeDataFormat equals h.CustomBarcodeFormatID into hh
                               from h in hh.DefaultIfEmpty()
                               select new
                               {
                                   a,
                                   b,
                                   c,
                                   d,
                                   e,
                                   h,
                                   cnt_total = (from f in db.Dmn_SerialPool where f.OrderNo.Equals(a.OrderNo) && f.SeqNo.Equals(a.SeqNo) && f.JobDetailType.Equals(b.JobDetailType) select f).Count(),
                                   cnt_notused = (from f in db.Dmn_SerialPool where f.OrderNo.Equals(a.OrderNo) && f.SeqNo.Equals(a.SeqNo) && f.JobDetailType.Equals(b.JobDetailType) && f.UseYN != "Y" select f).Count()
                               };
                    if (!string.IsNullOrEmpty(erp))
                        list = list.Where(q => q.a.ErpOrderNo.Equals(erp));
                    if (orderdateStart != null)
                        list = list.Where(q => q.a.InsertDate >= orderdateStart.Value);
                    if (orderdateEnd != null)
                        list = list.Where(q => q.a.InsertDate <= orderdateEnd.Value);
                    if (completeStart != null)
                        list = list.Where(q => q.b.CompleteDate >= completeStart.Value);
                    if (completeEnd != null)
                        list = list.Where(q => q.b.CompleteDate <= completeEnd.Value);
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Y") : q.a.UseYN.Equals("N"));
                    foreach (var item in list)
                    {
                        if (!retList.Any(x => x.OrderNo.Equals(item.a.OrderNo) && x.SeqNo.Equals(item.a.SeqNo)))
                        {
                            retList.Add(new JobOrder(item.a)
                            {
                                ProductStdCode = item.c == null ? null : item.c.ProdStdCode,
                                ProductName = item.c == null ? null : string.Format("{0} {1}", item.c.ProdName, item.c.ProdName2),
                                ProductName1 = item.c == null ? null : item.c.ProdName,
                                ProductName2 = item.c == null ? null : item.c.ProdName2,
                                AGLevel = item.c == null ? null : item.c.AGLevel,
                                Delay_NG = item.c == null || item.c.Delay_NG == null ? 0 : (int)item.c.Delay_NG,
                                Delay_Print = item.c == null || item.c.Delay_Print == null ? 0 : (int)item.c.Delay_Print,
                                Delay_Shot1 = item.c == null || item.c.Delay_Shot1 == null ? 0 : (int)item.c.Delay_Shot1,
                                Delay_Shot2 = item.c == null || item.c.Delay_Shot2 == null ? 0 : (int)item.c.Delay_Shot2,
                                ProdName = item.c == null ? null : item.c.ProdName,
                                ProdName2 = item.c == null ? null : item.c.ProdName2,
								MedicineType = item.c?.MedicineType,
								ProductReserved1 = item.c == null ? null : item.c.Reserved1,
                                ProductReserved2 = item.c == null ? null : item.c.Reserved2,
                                ProductReserved3 = item.c == null ? null : item.c.Reserved3,
                                ProductReserved4 = item.c == null ? null : item.c.Reserved4,
                                ProductReserved5 = item.c == null ? null : item.c.Reserved5,
							});
                        }
                        retList.Single(x => x.OrderNo.Equals(item.a.OrderNo) && x.SeqNo.Equals(item.a.SeqNo)).JobOrderDetail.Add(new JobOrderDetail(item.b)
                        {
                            SnExpressionID = item.e == null ? null : item.e.SnExpressionID,
                            SnExpressionStr = item.e == null ? null : item.e.SnExpressionStr,
                            SnType = item.d == null ? null : item.d.SnType,
                            BarcodeDataFormat = item.d == null ? null : item.d.BarcodeDataFormat,
                            BarcodeType = item.d == null ? null : item.d.BarcodeType,
                            Cnt_SerialTotal = item.cnt_total,
                            Cnt_SerialNotUsed = item.cnt_notused,
                            DesignID = item.d == null ? null : item.d.DesignID,
							DesignID2 = item.d == null ? null : item.d.DesignID2,
							Capacity = item.d == null ? null : item.d.Capacity,
                            PharmaCode = item.d == null ? null : item.d.PharmaCode,
                            Price = item.d == null ? null : item.d.Price,
                            MinimumWeight = item.d == null ? null : item.d.MinimumWeight,
                            MaximumWeight = item.d == null ? null : item.d.MaximumWeight,
                            CustomBarcodeFormat = item.h == null ? null : item.h.CustomBarcodeFormatStr,
                            ProdStdCode = item.d == null ? null : item.d.ProdStdCode,
                            ContentCount = item.d == null ? null : item.d.ContentCount
                        });
                    }
                    return retList.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController SelectLocalSinglePM Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController SelectLocalSinglePM Exception", ex);
            }
            return retList.FirstOrDefault();
        }

        public static List<JobOrder> SelectLocalAG(string orderNo, string seqNo, string lot, string productCode, string standardCode, string productName, string status, string orderType,
            DateTime? orderdateStart, DateTime? orderdateEnd, DateTime? completeStart, DateTime? completeEnd, bool? use, int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            List<JobOrder> retList = new List<JobOrder>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_JobOrder_M
                               join b in db.Dmn_JobOrder_AG on new { a.OrderNo, a.SeqNo } equals new { b.OrderNo, b.SeqNo } into bb
                               from b in bb.DefaultIfEmpty()
                               join c in db.Dmn_Product_M on a.ProdCode equals c.ProdCode into cc
                               from c in cc.DefaultIfEmpty()
                               join d in db.Dmn_Product_AG on new { c.ProdCode, b.JobDetailType } equals new { d.ProdCode, d.JobDetailType } into dd
                               from d in dd.DefaultIfEmpty()
                               join e in db.Dmn_Serial_Expression on d.SnExpressionID equals e.SnExpressionID into ee
                               from e in ee.DefaultIfEmpty()
                               orderby d.JobDetailType
                               select new
                               {
                                   a,
                                   b,
                                   c,
                                   d,
                                   e,
                                   cnt_total = (from f in db.Dmn_SerialPool where f.OrderNo.Equals(a.OrderNo) && f.SeqNo.Equals(a.SeqNo) && f.JobDetailType.Equals(b.JobDetailType) select f).Count(),
                                   cnt_notused = (from f in db.Dmn_SerialPool where f.OrderNo.Equals(a.OrderNo) && f.SeqNo.Equals(a.SeqNo) && f.JobDetailType.Equals(b.JobDetailType) && f.UseYN != "Y" select f).Count(),
                                   g = (from g in db.Dmn_Product_AG where g.ProdCode.Equals(a.ProdCode)
                                        join j in db.Dmn_CustomBarcodeFormat on g.BarcodeDataFormat equals j.CustomBarcodeFormatID into jj
                                        from j in jj.DefaultIfEmpty()
                                        select new { g, j }),
                               };
                    if (!string.IsNullOrEmpty(orderNo))
                        list = list.Where(q => q.a.OrderNo.Equals(orderNo));
                    if (!string.IsNullOrEmpty(seqNo))
                        list = list.Where(q => q.a.SeqNo.Equals(seqNo));
                    if (!string.IsNullOrEmpty(lot))
                        list = list.Where(q => q.a.LotNo.Contains(lot));
                    if (!string.IsNullOrEmpty(productCode))
                        list = list.Where(q => q.a.ProdCode.Contains(productCode));
                    if (!string.IsNullOrEmpty(standardCode))
                        list = list.Where(q => q.c.ProdStdCode.Contains(standardCode));
                    if (!string.IsNullOrEmpty(productName))
                        list = list.Where(q => q.c.ProdName.Contains(productName));
                    if (!string.IsNullOrEmpty(status))
                        list = list.Where(q => q.b.JobStatus.Equals(status));
                    if (!string.IsNullOrEmpty(orderType))
                        list = list.Where(q => q.a.OrderType.Equals(orderType));
                    if (orderdateStart != null)
                        list = list.Where(q => q.a.InsertDate >= orderdateStart.Value);
                    if (orderdateEnd != null)
                        list = list.Where(q => q.a.InsertDate <= orderdateEnd.Value);
                    if (completeStart != null)
                        list = list.Where(q => q.b.CompleteDate >= completeStart.Value);
                    if (completeEnd != null)
                        list = list.Where(q => q.b.CompleteDate <= completeEnd.Value);
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Y") : q.a.UseYN.Equals("N"));
                    foreach (var item in list)
                    {
                        if (!retList.Any(x => x.OrderNo.Equals(item.a.OrderNo) && x.SeqNo.Equals(item.a.SeqNo)))
                        {
                            retList.Add(new JobOrder(item.a)
                            {
                                ProductStdCode = item.c == null ? null : item.c.ProdStdCode,
                                ProductName = item.c == null ? null : string.Format("{0} {1}", item.c.ProdName, item.c.ProdName2),
                                ProductName1 = item.c == null ? null : item.c.ProdName,
                                ProductName2 = item.c == null ? null : item.c.ProdName2,
                                AGLevel = item.c == null ? null : item.c.AGLevel,
                                Delay_NG = item.c == null || item.c.Delay_NG == null ? 0 : (int)item.c.Delay_NG,
                                Delay_Print = item.c == null || item.c.Delay_Print == null ? 0 : (int)item.c.Delay_Print,
                                Delay_Print2 = item.c == null || item.c.Delay_Print2 == null ? 0 : (int)item.c.Delay_Print2,
                                Delay_Shot1 = item.c == null || item.c.Delay_Shot1 == null ? 0 : (int)item.c.Delay_Shot1,
                                Delay_Shot2 = item.c == null || item.c.Delay_Shot2 == null ? 0 : (int)item.c.Delay_Shot2,
                                Delay_Shot3 = item.c == null || item.c.Delay_Shot3 == null ? 0 : (int)item.c.Delay_Shot3,
                                Delay_Shot4 = item.c == null || item.c.Delay_Shot4 == null ? 0 : (int)item.c.Delay_Shot4,
                                ProdName = item.c == null ? null : item.c.ProdName,
                                ProdName2 = item.c == null ? null : item.c.ProdName2,
								MedicineType = item.c?.MedicineType,
								ProductReserved1 = item.c == null ? null : item.c.Reserved1,
                                ProductReserved2 = item.c == null ? null : item.c.Reserved2,
                                ProductReserved3 = item.c == null ? null : item.c.Reserved3,
                                ProductReserved4 = item.c == null ? null : item.c.Reserved4,
                                ProductReserved5 = item.c == null ? null : item.c.Reserved5,
							});
                        }
                        retList.Single(x => x.OrderNo.Equals(item.a.OrderNo) && x.SeqNo.Equals(item.a.SeqNo)).JobOrderDetail.Add(new JobOrderDetail(item.b)
                        {
                            SnExpressionID = item.e == null ? null : item.e.SnExpressionID,
                            SnExpressionStr = item.e == null ? null : item.e.SnExpressionStr,
                            SnType = item.d == null ? null : item.d.SnType,
                            Cnt_SerialTotal = Convert.ToInt32(item.cnt_total),
                            Cnt_SerialNotUsed = Convert.ToInt32(item.cnt_notused),
                            BarcodeDataFormat = item.d == null ? null : item.d.BarcodeDataFormat,
                            BarcodeType = item.d == null ? null : item.d.BarcodeType,
                            DesignID = item.d == null ? null : item.d.DesignID,
                            Prefix_SSCC = item.d == null ? null : item.d.Prefix_SSCC,
                            Capacity = item.d == null ? null : item.d.Capacity,
                            ProdStdCodeChild = item.d == null ? null : item.d.ProdStdCodeChild,
                            GS1ExtensionCode = item.d == null ? null : item.d.GS1ExtensionCode,
                            PackingCount = item.d == null ? 1 : Convert.ToInt32(item.d.PackingCount),
                            ContentCount = item.d == null ? 1 : Convert.ToInt32(item.d.ContentCount),
                            PrinterName = item.d == null ? null : item.d.PrinterName,
                            ResourceType = item.d == null ? null : item.d.ResourceType,
                            Price = item.d == null ? null : item.d.Price,
                            ProductReserved1 = item.d == null ? null : item.d.Reserved1,
                            ProductReserved2 = item.d == null ? null : item.d.Reserved2,
                            ProductReserved3 = item.d == null ? null : item.d.Reserved3,
                            DesignID2 = item.d == null ? null : item.d.DesignID2,
                            PrinterName2 = item.d == null ? null : item.d.PrinterName2,
                            MinimumWeight = item.d == null ? null : item.d.MinimumWeight,
                            MaximumWeight = item.d == null ? null : item.d.MaximumWeight,
                            ProdStdCode = item.d == null ? null : item.d.ProdStdCode,
                        });
                        foreach (var pd in item.g)
                        {
                            var product = retList.Single(x => x.OrderNo.Equals(item.a.OrderNo) && x.SeqNo.Equals(item.a.SeqNo));
                            if (!product.ProductDetail.ContainsJobDetail(pd.g.JobDetailType))
                            {
                                product.ProductDetail.Add(new ProductDetail(pd.g)
                                {
                                    CustomBarcodeFormat = pd.j?.CustomBarcodeFormatStr ?? null
                                });
                            }
                        }
                    }
                    if (pageIndex >= 0)
                    {
                        TotalCount = retList.Count();
                        TotalPageCount = Convert.ToInt64(Math.Ceiling(Convert.ToDouble(TotalCount) / Convert.ToDouble(pageSize)));
                        retList = retList.OrderBy(x => x.OrderNo).ThenBy(x => x.SeqNo).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    }
                }
                return retList;
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController SelectLocalAG Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController SelectLocalAG Exception", ex);
            }
            return retList;
        }
        public static JobOrder SelectLocalSingleAG(string orderNo = "", string seqNo = "", string lot = "", string productCode = "", string productName = "", string status = "", string orderType = "", DateTime? orderdateStart = null,
             DateTime? orderdateEnd = null, DateTime? completeStart = null, DateTime? completeEnd = null, bool? use = null, bool RaiseException = false)
        {
            List<JobOrder> retList = new List<JobOrder>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_JobOrder_M
                               join b in db.Dmn_JobOrder_AG on new { a.OrderNo, a.SeqNo } equals new { b.OrderNo, b.SeqNo } into bb
                               from b in bb.DefaultIfEmpty()
                               join c in db.Dmn_Product_M on a.ProdCode equals c.ProdCode into cc
                               from c in cc.DefaultIfEmpty()
                               join d in db.Dmn_Product_AG on new { c.ProdCode, b.JobDetailType } equals new { d.ProdCode, d.JobDetailType } into dd
                               from d in dd.DefaultIfEmpty()
                               join e in db.Dmn_Serial_Expression on d.SnExpressionID equals e.SnExpressionID into ee
                               from e in ee.DefaultIfEmpty()
                               join f in db.Dmn_CustomBarcodeFormat on d.BarcodeDataFormat equals f.CustomBarcodeFormatID into ff
                               from f in ff.DefaultIfEmpty()
                               orderby d.JobDetailType
                               select new
                               {
                                   a,
                                   b,
                                   c,
                                   d,
                                   e,
                                   f,
                                   cnt_total = (from f in db.Dmn_SerialPool where f.OrderNo.Equals(a.OrderNo) && f.SeqNo.Equals(a.SeqNo) && f.JobDetailType.Equals(b.JobDetailType) select f).Count(),
                                   cnt_notused = (from f in db.Dmn_SerialPool where f.OrderNo.Equals(a.OrderNo) && f.SeqNo.Equals(a.SeqNo) && f.JobDetailType.Equals(b.JobDetailType) && f.UseYN != "Y" select f).Count(),
                                   g = (from g in db.Dmn_Product_AG where g.ProdCode.Equals(a.ProdCode)
                                        join j in db.Dmn_CustomBarcodeFormat on g.BarcodeDataFormat equals j.CustomBarcodeFormatID into jj
                                        from j in jj.DefaultIfEmpty()
                                        select new { g, j }),
                               };
                    if (!string.IsNullOrEmpty(orderNo))
                        list = list.Where(q => q.a.OrderNo.Equals(orderNo));
                    if (!string.IsNullOrEmpty(seqNo))
                        list = list.Where(q => q.a.SeqNo.Equals(seqNo));
                    if (!string.IsNullOrEmpty(lot))
                        list = list.Where(q => q.a.LotNo.Contains(lot));
                    if (!string.IsNullOrEmpty(productCode))
                        list = list.Where(q => q.a.ProdCode.Contains(productCode));
                    if (!string.IsNullOrEmpty(productName))
                        list = list.Where(q => q.c.ProdName.Contains(productName));
                    if (!string.IsNullOrEmpty(status))
                        list = list.Where(q => q.b.JobStatus.Equals(status));
                    if (!string.IsNullOrEmpty(orderType))
                        list = list.Where(q => q.a.OrderType.Equals(orderType));
                    if (orderdateStart != null)
                        list = list.Where(q => q.a.InsertDate >= orderdateStart.Value);
                    if (orderdateEnd != null)
                        list = list.Where(q => q.a.InsertDate <= orderdateEnd.Value);
                    if (completeStart != null)
                        list = list.Where(q => q.b.CompleteDate >= completeStart.Value);
                    if (completeEnd != null)
                        list = list.Where(q => q.b.CompleteDate <= completeEnd.Value);
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Y") : q.a.UseYN.Equals("N"));
                    foreach (var item in list)
                    {
                        if (!retList.Any(x => x.OrderNo.Equals(item.a.OrderNo) && x.SeqNo.Equals(item.a.SeqNo)))
                        {
                            retList.Add(new JobOrder(item.a)
                            {
                                ProductStdCode = item.c == null ? null : item.c.ProdStdCode,
                                ProductName = item.c == null ? null : string.Format("{0} {1}", item.c.ProdName, item.c.ProdName2),
                                ProductName1 = item.c == null ? null : item.c.ProdName,
                                ProductName2 = item.c == null ? null : item.c.ProdName2,
                                AGLevel = item.c == null ? null : item.c.AGLevel,
                                Delay_NG = item.c == null || item.c.Delay_NG == null ? 0 : (int)item.c.Delay_NG,
                                Delay_Print = item.c == null || item.c.Delay_Print == null ? 0 : (int)item.c.Delay_Print,
                                Delay_Shot1 = item.c == null || item.c.Delay_Shot1 == null ? 0 : (int)item.c.Delay_Shot1,
                                Delay_Shot2 = item.c == null || item.c.Delay_Shot2 == null ? 0 : (int)item.c.Delay_Shot2,
                                Delay_Print2 = item.c == null || item.c.Delay_Print2 == null ? 0 : (int)item.c.Delay_Print2,
                                Delay_Shot3 = item.c == null || item.c.Delay_Shot3 == null ? 0 : (int)item.c.Delay_Shot3,
                                Delay_Shot4 = item.c == null || item.c.Delay_Shot4 == null ? 0 : (int)item.c.Delay_Shot4,
                                ProdName = item.c == null ? null : item.c.ProdName,
                                ProdName2 = item.c == null ? null : item.c.ProdName2,
								MedicineType = item.c?.MedicineType,
								ProductReserved1 = item.c == null ? null : item.c.Reserved1,
                                ProductReserved2 = item.c == null ? null : item.c.Reserved2,
                                ProductReserved3 = item.c == null ? null : item.c.Reserved3,
                                ProductReserved4 = item.c == null ? null : item.c.Reserved4,
                                ProductReserved5 = item.c == null ? null : item.c.Reserved5,
							});
                        }
                        retList.Single(x => x.OrderNo.Equals(item.a.OrderNo) && x.SeqNo.Equals(item.a.SeqNo)).JobOrderDetail.Add(new JobOrderDetail(item.b)
                        {
                            SnExpressionID = item.e == null ? null : item.e.SnExpressionID,
                            SnExpressionStr = item.e == null ? null : item.e.SnExpressionStr,
                            SnType = item.d == null ? null : item.d.SnType,
                            Cnt_SerialTotal = item.cnt_total,
                            Cnt_SerialNotUsed = item.cnt_notused,
                            BarcodeDataFormat = item.d == null ? null : item.d.BarcodeDataFormat,
                            BarcodeType = item.d == null ? null : item.d.BarcodeType,
                            DesignID = item.d == null ? null : item.d.DesignID,
                            Prefix_SSCC = item.d == null ? null : item.d.Prefix_SSCC,
                            Capacity = item.d == null ? null : item.d.Capacity,
                            ProdStdCodeChild = item.d == null ? null : item.d.ProdStdCodeChild,
                            GS1ExtensionCode = item.d == null ? null : item.d.GS1ExtensionCode,
                            ContentCount = item.d == null ? 1 : Convert.ToInt32(item.d.ContentCount),
                            PackingCount = item.d == null ? 1 : Convert.ToInt32(item.d.PackingCount),
                            PrinterName = item.d == null ? null : item.d.PrinterName,
                            ResourceType = item.d == null ? null : item.d.ResourceType,
                            Price = item.d == null ? null : item.d.Price,
                            ProductReserved1 = item.d == null ? null : item.d.Reserved1,
                            ProductReserved2 = item.d == null ? null : item.d.Reserved2,
                            ProductReserved3 = item.d == null ? null : item.d.Reserved3,
                            DesignID2 = item.d == null ? null : item.d.DesignID2,
                            PrinterName2 = item.d == null ? null : item.d.PrinterName2,
                            ProdStdCode = item.d == null ? null : item.d.ProdStdCode,
                            MinimumWeight = item.d == null ? null : item.d.MinimumWeight,
                            MaximumWeight = item.d == null ? null : item.d.MaximumWeight,
                        });
                        foreach (var pd in item.g)
                        {
                            var product = retList.Single(x => x.OrderNo.Equals(item.a.OrderNo) && x.SeqNo.Equals(item.a.SeqNo));
                            if (!product.ProductDetail.ContainsJobDetail(pd.g.JobDetailType))
                            {
                                product.ProductDetail.Add(new ProductDetail(pd.g)
                                {
                                    CustomBarcodeFormat = pd.j?.CustomBarcodeFormatStr ?? null
                                });
                            }
                        }
                    }
                }
                return retList.FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController SelectLocalSingleAG Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController SelectLocalSingleAG Exception", ex);
            }
            return retList.FirstOrDefault();
        }
        public static JobOrder SelectLocalSingleAGERP(string erp, DateTime? orderdateStart,
     DateTime? orderdateEnd, DateTime? completeStart, DateTime? completeEnd, bool? use, bool RaiseException = false)
        {
            List<JobOrder> retList = new List<JobOrder>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_JobOrder_M
                               join b in db.Dmn_JobOrder_AG on new { a.OrderNo, a.SeqNo } equals new { b.OrderNo, b.SeqNo } into bb
                               from b in bb.DefaultIfEmpty()
                               join c in db.Dmn_Product_M on a.ProdCode equals c.ProdCode into cc
                               from c in cc.DefaultIfEmpty()
                               join d in db.Dmn_Product_AG on new { c.ProdCode, b.JobDetailType } equals new { d.ProdCode, d.JobDetailType } into dd
                               from d in dd.DefaultIfEmpty()
                               join e in db.Dmn_Serial_Expression on d.SnExpressionID equals e.SnExpressionID into ee
                               from e in ee.DefaultIfEmpty()
                               orderby d.JobDetailType
                               select new
                               {
                                   a,
                                   b,
                                   c,
                                   d,
                                   e,
                                   cnt_total = (from f in db.Dmn_SerialPool where f.OrderNo.Equals(a.OrderNo) && f.SeqNo.Equals(a.SeqNo) && f.JobDetailType.Equals(b.JobDetailType) select f).Count(),
                                   cnt_notused = (from f in db.Dmn_SerialPool where f.OrderNo.Equals(a.OrderNo) && f.SeqNo.Equals(a.SeqNo) && f.JobDetailType.Equals(b.JobDetailType) && f.UseYN != "Y" select f).Count(),
                                   g = (from g in db.Dmn_Product_AG
                                        where g.ProdCode.Equals(a.ProdCode)
                                        join j in db.Dmn_CustomBarcodeFormat on g.BarcodeDataFormat equals j.CustomBarcodeFormatID into jj
                                        from j in jj.DefaultIfEmpty()
                                        select new { g, j }),
                               };
                    if (!string.IsNullOrEmpty(erp))
                        list = list.Where(q => q.a.ErpOrderNo.Equals(erp));
                    if (orderdateStart != null)
                        list = list.Where(q => q.a.InsertDate >= orderdateStart.Value);
                    if (orderdateEnd != null)
                        list = list.Where(q => q.a.InsertDate <= orderdateEnd.Value);
                    if (completeStart != null)
                        list = list.Where(q => q.b.CompleteDate >= completeStart.Value);
                    if (completeEnd != null)
                        list = list.Where(q => q.b.CompleteDate <= completeEnd.Value);
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Y") : q.a.UseYN.Equals("N"));
                    foreach (var item in list)
                    {
                        if (!retList.Any(x => x.OrderNo.Equals(item.a.OrderNo) && x.SeqNo.Equals(item.a.SeqNo)))
                        {
                            retList.Add(new JobOrder(item.a)
                            {
                                ProductStdCode = item.c == null ? null : item.c.ProdStdCode,
                                ProductName = item.c == null ? null : string.Format("{0} {1}", item.c.ProdName, item.c.ProdName2),
                                ProductName1 = item.c == null ? null : item.c.ProdName,
                                ProductName2 = item.c == null ? null : item.c.ProdName2,
                                AGLevel = item.c == null ? null : item.c.AGLevel,
                                Delay_NG = item.c == null || item.c.Delay_NG == null ? 0 : (int)item.c.Delay_NG,
                                Delay_Print = item.c == null || item.c.Delay_Print == null ? 0 : (int)item.c.Delay_Print,
                                Delay_Shot1 = item.c == null || item.c.Delay_Shot1 == null ? 0 : (int)item.c.Delay_Shot1,
                                Delay_Shot2 = item.c == null || item.c.Delay_Shot2 == null ? 0 : (int)item.c.Delay_Shot2,
                                Delay_Print2 = item.c == null || item.c.Delay_Print2 == null ? 0 : (int)item.c.Delay_Print2,
                                Delay_Shot3 = item.c == null || item.c.Delay_Shot3 == null ? 0 : (int)item.c.Delay_Shot3,
                                Delay_Shot4 = item.c == null || item.c.Delay_Shot4 == null ? 0 : (int)item.c.Delay_Shot4,
                                ProdName = item.c == null ? null : item.c.ProdName,
                                ProdName2 = item.c == null ? null : item.c.ProdName2,
								MedicineType = item.c?.MedicineType,
								ProductReserved1 = item.c == null ? null : item.c.Reserved1,
                                ProductReserved2 = item.c == null ? null : item.c.Reserved2,
                                ProductReserved3 = item.c == null ? null : item.c.Reserved3,
                                ProductReserved4 = item.c == null ? null : item.c.Reserved4,
                                ProductReserved5 = item.c == null ? null : item.c.Reserved5,
							});
                        }
                        retList.Single(x => x.OrderNo.Equals(item.a.OrderNo) && x.SeqNo.Equals(item.a.SeqNo)).JobOrderDetail.Add(new JobOrderDetail(item.b)
                        {
                            SnExpressionID = item.e == null ? null : item.e.SnExpressionID,
                            SnExpressionStr = item.e == null ? null : item.e.SnExpressionStr,
                            SnType = item.d == null ? null : item.d.SnType,
                            Cnt_SerialTotal = item.cnt_total,
                            Cnt_SerialNotUsed = item.cnt_notused,
                            BarcodeDataFormat = item.d == null ? null : item.d.BarcodeDataFormat,
                            BarcodeType = item.d == null ? null : item.d.BarcodeType,
                            DesignID = item.d == null ? null : item.d.DesignID,
                            Prefix_SSCC = item.d == null ? null : item.d.Prefix_SSCC,
                            Capacity = item.d == null ? null : item.d.Capacity,
                            ProdStdCodeChild = item.d == null ? null : item.d.ProdStdCodeChild,
                            GS1ExtensionCode = item.d == null ? null : item.d.GS1ExtensionCode,
                            ContentCount = item.d == null ? 1 : Convert.ToInt32(item.d.ContentCount),
                            PackingCount = item.d == null ? 1 : Convert.ToInt32(item.d.PackingCount),
                            PrinterName = item.d == null ? null : item.d.PrinterName,
                            ResourceType = item.d == null ? null : item.d.ResourceType,
                            Price = item.d == null ? null : item.d.Price,
                            ProductReserved1 = item.d == null ? null : item.d.Reserved1,
                            ProductReserved2 = item.d == null ? null : item.d.Reserved2,
                            ProductReserved3 = item.d == null ? null : item.d.Reserved3,
                            DesignID2 = item.d == null ? null : item.d.DesignID2,
                            PrinterName2 = item.d == null ? null : item.d.PrinterName2,
                            ProdStdCode = item.d == null ? null : item.d.ProdStdCode,
                            MinimumWeight = item.d == null ? null : item.d.MinimumWeight,
                            MaximumWeight = item.d == null ? null : item.d.MaximumWeight,
                        });
                        foreach (var pd in item.g)
                        {
                            var product = retList.Single(x => x.OrderNo.Equals(item.a.OrderNo) && x.SeqNo.Equals(item.a.SeqNo));
                            if (!product.ProductDetail.ContainsJobDetail(pd.g.JobDetailType))
                            {
                                product.ProductDetail.Add(new ProductDetail(pd.g)
                                {
                                    CustomBarcodeFormat = pd.j?.CustomBarcodeFormatStr ?? null
                                });
                            }
                        }
                    }
                }
                return retList.FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController SelectLocalSingleAG Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController SelectLocalSingleAG Exception", ex);
            }
            return retList.FirstOrDefault();
        }

        public static int GetLastSerialPM(string standardCode, string lot = "", DateTime? mfdDateStart = null, DateTime? mfdDateEnd = null, DateTime? expDateStart = null, DateTime? expDateEnd = null, bool RaiseException = false)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_JobOrder_M
                               join b in db.Dmn_JobOrder_PM on new { a.OrderNo, a.SeqNo } equals new { b.OrderNo, b.SeqNo } into bb
                               from b in bb.DefaultIfEmpty()
                               join c in db.Dmn_Product_M on a.ProdCode equals c.ProdCode into cc
                               from c in cc.DefaultIfEmpty()
                               select new { a, b, c };
                    if (!string.IsNullOrEmpty(standardCode))
                        list = list.Where(q => q.c.ProdStdCode.Equals(standardCode));
                    if (!string.IsNullOrEmpty(lot))
                        list = list.Where(q => q.a.LotNo.Equals(lot));
                    if (mfdDateStart != null)
                        list = list.Where(q => q.a.MfdDate >= mfdDateStart.Value);
                    if(mfdDateEnd != null)
                        list = list.Where(q => q.a.MfdDate <= mfdDateEnd.Value);
                    if (expDateStart != null)
                        list = list.Where(q => q.a.ExpDate >= expDateStart.Value);
                    if(expDateEnd != null)
                        list = list.Where(q => q.a.ExpDate <= expDateEnd.Value);
                    var dd = list.ToList();
                    return list.Max(x => x.b.Cnt_SNLast == null ? 0 : x.b.Cnt_SNLast).Value;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController GetLastSerialPM Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController GetLastSerialPM Exception", ex);
                return 0;
            }
        }
        public static int GetLastPrintedSerialPM(string standardCode, string lot = "", DateTime? mfdDateStart = null, DateTime? mfdDateEnd = null, DateTime? expDateStart = null, DateTime? expDateEnd = null, bool RaiseException = false)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_JobOrder_M
                               join b in db.Dmn_JobOrder_PM on new { a.OrderNo, a.SeqNo } equals new { b.OrderNo, b.SeqNo }
                               join c in db.Dmn_Product_M on a.ProdCode equals c.ProdCode
                               select new { a, b, c };
                    if (!string.IsNullOrEmpty(standardCode))
                        list = list.Where(q => q.c.ProdStdCode.Equals(standardCode));
                    if (!string.IsNullOrEmpty(lot))
                        list = list.Where(q => q.a.LotNo.Equals(lot));
                    if (mfdDateStart != null)
                        list = list.Where(q => q.a.MfdDate >= mfdDateStart.Value);
                    if (mfdDateEnd != null)
                        list = list.Where(q => q.a.MfdDate <= mfdDateEnd.Value);
                    if (expDateStart != null)
                        list = list.Where(q => q.a.ExpDate >= expDateStart.Value);
                    if (expDateEnd != null)
                        list = list.Where(q => q.a.ExpDate <= expDateEnd.Value);
                    var dd = list.ToList();
                    return list.Max(x => x.b.Cnt_SNPrintLast == null ? 0 : x.b.Cnt_SNPrintLast).Value;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController GetLastPrintedSerialPM Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController GetLastPrintedSerialPM Exception", ex);
                return 0;
            }
        }
        public static int GetLastSerialAG(string standardCode, string jobDetailType, string lot = "", bool RaiseException = false)
        {
            List<JobOrder> retList = new List<JobOrder>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_JobOrder_M
                               join b in db.Dmn_JobOrder_AG on new { a.OrderNo, a.SeqNo } equals new { b.OrderNo, b.SeqNo }
                               join c in db.Dmn_Product_M on a.ProdCode equals c.ProdCode
                               orderby b.JobDetailType
                               select new { a, b, c };
                    if (!string.IsNullOrEmpty(standardCode))
                        list = list.Where(q => q.c.ProdStdCode.Equals(standardCode));
                    if (!string.IsNullOrEmpty(lot))
                        list = list.Where(q => q.a.LotNo.Equals(lot));
                    if (!string.IsNullOrEmpty(jobDetailType))
                        list = list.Where(q => q.b.JobDetailType.Equals(jobDetailType));
                    return Convert.ToInt32(list.Max(x => x.b.Cnt_SNLast));
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController GetLastSerialAG Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController GetLastSerialAG Exception", ex);
                return 0;
            }
        }
        public static int GetLastSSCCAG(string prefix, bool RaiseException = false)
        {
            List<JobOrder> retList = new List<JobOrder>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_JobOrder_M
                               join b in db.Dmn_JobOrder_AG on new { a.OrderNo, a.SeqNo } equals new { b.OrderNo, b.SeqNo }
                               join c in db.Dmn_Product_AG on new { a.ProdCode, b.JobDetailType } equals new { c.ProdCode, c.JobDetailType }
                               orderby c.JobDetailType
                               select new { a, b, c };
                    if (!string.IsNullOrEmpty(prefix))
                        list = list.Where(q => q.c.Prefix_SSCC.Equals(prefix));
                    return Convert.ToInt32(list.Max(x => x.b.Cnt_SNLast));
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController GetLastSerialAG Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController GetLastSerialAG Exception", ex);
                return 0;
            }
        }
        public static int GetLastPrintedSerialAG(string standardCode, string jobDetailType, string seqNo, string lot = "", bool RaiseException = false)
        {
            List<JobOrder> retList = new List<JobOrder>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_JobOrder_M
                               join b in db.Dmn_JobOrder_AG on new { a.OrderNo, a.SeqNo } equals new { b.OrderNo, b.SeqNo }
                               join c in db.Dmn_Product_M on a.ProdCode equals c.ProdCode
                               orderby b.JobDetailType
                               select new { a, b, c };

                    if (!string.IsNullOrEmpty(standardCode))
                        list = list.Where(q => q.c.ProdStdCode.Equals(standardCode));
                    if (!string.IsNullOrEmpty(lot))
                        list = list.Where(q => q.a.LotNo.Equals(lot));
                    if (!string.IsNullOrEmpty(seqNo))
                        list = list.Where(q => q.a.SeqNo.Equals(seqNo));
                    if (!string.IsNullOrEmpty(jobDetailType))
                        list = list.Where(q => q.b.JobDetailType.Equals(jobDetailType));
                    return Convert.ToInt32(list.Max(x => x.b.Cnt_SNPrintLast));
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController GetLastPrintedSerialAG Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController GetLastPrintedSerialAG Exception", ex);
                return 0;
            }
        }
        public static bool UpdateCountLocalPM(string orderNo, string seqNo, int? good, int? error, bool RaiseException = false)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var item = db.Dmn_JobOrder_PM.FirstOrDefault(q => q.OrderNo.Equals(orderNo) && q.SeqNo.Equals(seqNo));
                    if (good != null)
                        item.Cnt_Good = good;
                    if (error != null)
                        item.Cnt_Error = error;
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController UpdateCountLocalPM Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController UpdateCountLocalPM Exception", ex);
                return false;
            }
        }
        public static bool UpdateCountLocalQueryPM(string orderNo, string seqNo, int? good, int? error, bool RaiseException = false)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    string query;
                    if (good == null)
                    {
                        query = string.Format(@"UPDATE [DOMINO_DB].[dbo].[Dmn_JobOrder_PM]
                                                         Cnt_Error = '{0}'
                                                     Where OrderNo = '{1}' AND SeqNo = '{2}'",
                                 error, orderNo, seqNo);
                    }
                    else if (error == null)
                    {
                        query = string.Format(@"UPDATE [DOMINO_DB].[dbo].[Dmn_JobOrder_PM]
                                                     SET Cnt_Good = '{0}'
                                                     Where OrderNo = '{1}' AND SeqNo = '{2}'",
                                                         good, orderNo, seqNo);
                    }
                    else
                    {
                        query = string.Format(@"UPDATE [DOMINO_DB].[dbo].[Dmn_JobOrder_PM]
                                                     SET Cnt_Good = '{0}',
                                                         Cnt_Error = '{1}'
                                                     Where OrderNo = '{2}' AND SeqNo = '{3}'",
                                                         good, error, orderNo, seqNo);
                    }
                    db.Database.ExecuteSqlCommand(query);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController UpdateCountLocalPM Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController UpdateCountLocalPM Exception", ex);
                return false;
            }
        }
        public static bool UpdateCountLocalAG(string orderNo, string seqNo, string jobDetailType, int? good, int? error, bool RaiseException = false)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var item = db.Dmn_JobOrder_AG.FirstOrDefault(q => q.OrderNo.Equals(orderNo) && q.SeqNo.Equals(seqNo) && q.JobDetailType.Equals(jobDetailType));
                    if (good != null)
                        item.Cnt_Good = good;
                    if (error != null)
                        item.Cnt_Error = error;
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController UpdateCountLocalAG Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController UpdateCountLocalAG Exception", ex);
                return false;
            }
        }
        public static bool UpdateSerialIndexPM(string orderNo, string seqNo, int snLast, int snPrintedLast, bool RaiseException = false)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var item = db.Dmn_JobOrder_PM.FirstOrDefault(q => q.OrderNo.Equals(orderNo) && q.SeqNo.Equals(seqNo));
                    item.Cnt_SNLast = snLast;
                    item.Cnt_SNPrintLast = snPrintedLast;
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController UpdateSerialIndexPM Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController UpdateSerialIndexPM Exception", ex);
                return false;
            }
        }
        public static bool UpdateSerialIndexQueryPM(string orderNo, string seqNo, int snLast, int snPrintedLast, bool RaiseException = false)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    string query = string.Format(@"UPDATE [DOMINO_DB].[dbo].[Dmn_JobOrder_PM]
                                                     SET Cnt_SNLast = '{0}',
                                                         Cnt_SNPrintLast = '{1}'
                                                     Where OrderNo = '{2}' AND SeqNo = '{3}'",
                                                         snLast, snPrintedLast, orderNo, seqNo);
                    db.Database.ExecuteSqlCommand(query);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController UpdateSerialIndexQueryPM Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController UpdateSerialIndexQueryPM Exception", ex);
                return false;
            }
        }
        public static bool UpdateSerialIndexAG(string orderNo, string seqNo, string jobDetailType, int snLast, int snPrintedLast, bool RaiseException = false)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var item = db.Dmn_JobOrder_AG.FirstOrDefault(q => q.OrderNo.Equals(orderNo) && q.SeqNo.Equals(seqNo) && q.JobDetailType.Equals(jobDetailType));
                    item.Cnt_SNLast = snLast;
                    item.Cnt_SNPrintLast = snPrintedLast;
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController UpdateSerialIndexAG Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController UpdateSerialIndexAG Exception", ex);
                return false;
            }
        }

        public static bool InsertLocal(Local.Dmn_JobOrder_M item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("JobOrderController InsertLocalMaster by {0}, {1}", item.InsertUser, item.ToString());
                    db.Dmn_JobOrder_M.Add(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController InsertLocalMaster Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController InsertLocalMaster Exception", ex);
                return false;
            }
        }
        public static bool InsertLocal(Local.Dmn_JobOrder_PM item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("JobOrderController InsertLocalPM by {0}, {1}", item.UpdateUser ?? item.InsertUser, item.ToString());
                    db.Dmn_JobOrder_PM.Add(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController InsertLocalPM Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController InsertLocalPM Exception", ex);
                return false;
            }
        }
        public static bool InsertLocal(Local.Dmn_JobOrder_AG item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("JobOrderController InsertLocalAG by {0} , {1}", item.UpdateUser ?? item.InsertUser, item.ToString());
                    db.Dmn_JobOrder_AG.Add(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController InsertLocalAG Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController InsertLocalAG Exception", ex);
                return false;
            }
        }
        public static bool UpdateLocal(Local.Dmn_JobOrder_M item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("JobOrderController UpdateLocalMaster by {0}, {1}", item.UpdateUser, item.ToString());
                    var tmp = db.Dmn_JobOrder_M.First(q => q.OrderNo.Equals(item.OrderNo) && q.SeqNo.Equals(item.SeqNo));
                    tmp.WriteExistings(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController UpdateLocalMaster Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController UpdateLocalMaster Exception", ex);
                return false;
            }
        }
        public static bool UpdateLocal(Local.Dmn_JobOrder_PM item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("JobOrderController UpdateLocalPM by {0}, {1}", item.UpdateUser, item.ToString());
                    var tmp = db.Dmn_JobOrder_PM.First(q => q.OrderNo.Equals(item.OrderNo) && q.SeqNo.Equals(item.SeqNo));
                    tmp.WriteExistings(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController UpdateLocalPM Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController UpdateLocalPM Exception", ex);
                return false;
            }
        }
        public static bool UpdateLocal(Local.Dmn_JobOrder_AG item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("JobOrderController UpdateLocalAG by {0}, {1}", item.UpdateUser, item.ToString());
                    var tmp = db.Dmn_JobOrder_AG.First(q => q.OrderNo.Equals(item.OrderNo) && q.SeqNo.Equals(item.SeqNo));
                    tmp.WriteExistings(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController UpdateLocalAG Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController UpdateLocalAG Exception", ex);
                return false;
            }
        }
        public static bool DeleteLocalAll(string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("JobOrderController DeleteLocalAll by {0}", userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_JobOrder_AG");
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_JobOrder_M");
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_JobOrder_PM");
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController DeleteLocalAll Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController DeleteLocalAll Exception", ex);
                return false;
            }

        }
        public static bool DeleteLocalSingle(string orderNo, string seqNo, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("JobOrderController DeleteLocalSingle {0} by {1}", orderNo + seqNo, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_JobOrder_AG where OrderNo = {0} AND SeqNo = {1}", orderNo, seqNo);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_JobOrder_M where OrderNo = {0} AND SeqNo = {1}", orderNo, seqNo);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_JobOrder_PM where OrderNo = {0} AND SeqNo = {1}", orderNo, seqNo);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController DeleteLocalSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController DeleteLocalSingle Exception", ex);
                return false;
            }
        }
        public static bool DeleteLocalDetail(string orderNo, string seqNo, string jobDetail, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("JobOrderController DeleteLocalDetail by {0}", userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_JobOrder_PM where OrderNo = {0} AND SeqNo = {1} AND JobDetailType = {2}", orderNo, seqNo, jobDetail);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_JobOrder_AG where OrderNo = {0} AND SeqNo = {1} AND JobDetailType = {2}", orderNo, seqNo, jobDetail);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController DeleteLocalDetail Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController DeleteLocalDetail Exception", ex);
                return false;
            }
        }
        public static DateTime? GetLastInsertDateLocal(bool RaiseException = false)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    return db.Dmn_JobOrder_M.Select(q => q.InsertDate).Max();
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController GetLastInsertDateLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController GetLastInsertDateLocal Exception", ex);
            }
            return null;
        }
        public static DateTime? GetLastUpdateDateLocal(bool RaiseException = false)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    return db.Dmn_JobOrder_M.Select(q => q.UpdateDate).Max();
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController GetLastUpdateDateLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController GetLastUpdateDateLocal Exception", ex);
            }
            return null;
        }
        public static int? GetSequenceNumberLocal(DateTime date, bool RaiseException = false)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    string tmp = string.Format("{0}", date.ToString("yyyyMMdd"));
                    return Convert.ToInt32(db.Dmn_JobOrder_M.Where(q => q.OrderNo.Contains(tmp)).Select(q => q.SeqNo).Max());
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController GetSequenceNumberLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController GetSequenceNumberLocal Exception", ex);
            }
            return null;
        }
        public static bool IsErpNoExist(string erpNo, out string orderNo, out string seqNo, bool RaiseException = false)
        {
            orderNo = string.Empty;
            seqNo = string.Empty;
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    Local.Dmn_JobOrder_M job = db.Dmn_JobOrder_M.Where(q => q.ErpOrderNo.Equals(erpNo))?.FirstOrDefault();
                    if (job != null)
                    {
                        orderNo = job.OrderNo;
                        seqNo = job.SeqNo;
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController IsErpNoExist Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController IsErpNoExist Exception", ex);
            }
            return false;
        }

        public static List<JobOrder> SelectServer(string plantCode, string lineID, string orderNo, string seqNo, string lot, string productCode, string jobDetailType, string productName, string status, string orderType, string machineID,
            DateTime? orderdateStart, DateTime? orderdateEnd, DateTime? insertDateStart, DateTime? insertDateEnd, DateTime? updateDateStart, DateTime? updateDateEnd, DateTime? assigneDateStart, DateTime? assignDateEnd, bool? use, string standardCode, int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            List<JobOrder> retList = new List<JobOrder>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_JobOrder_M
                               join b in db.Dmn_JobOrder_D on new { a.PlantCode, a.OrderNo, a.SeqNo } equals new { b.PlantCode, b.OrderNo, b.SeqNo }
                               join c in db.Dmn_Product_M on new { a.PlantCode, a.ProdCode } equals new { c.PlantCode, c.ProdCode } into cc
                               from c in cc.DefaultIfEmpty()
                               join d in db.Dmn_Product_D on new { a.PlantCode, a.ProdCode, b.JobDetailType } equals new { d.PlantCode, d.ProdCode, d.JobDetailType } into dd
                               from d in dd.DefaultIfEmpty()
                               join e in db.Dmn_Line_M on new { a.PlantCode, a.LineID } equals new { e.PlantCode, e.LineID } into ee
                               from e in ee.DefaultIfEmpty()
                               join f in db.Dmn_Line_D on new { a.PlantCode, a.LineID, b.JobDetailType } equals new { f.PlantCode, f.LineID, f.JobDetailType } into ff
                               from f in ff.DefaultIfEmpty()
                               join g in db.Dmn_Serial_Expression on new { a.PlantCode, d.SnExpressionID } equals new { g.PlantCode, g.SnExpressionID } into gg
                               from g in gg.DefaultIfEmpty()
                               join j in db.Dmn_CustomBarcodeFormat on new { a.PlantCode, d.BarcodeDataFormat } equals new { j.PlantCode, BarcodeDataFormat = j.CustomBarcodeFormatID } into jj
                               from j in jj.DefaultIfEmpty()
                               orderby d.JobDetailType
                               select new
                               {
                                   a,
                                   b,
                                   c,
                                   d,
                                   e,
                                   f,
                                   g,
                                   j,
                                   cnt_total = (from h in db.Dmn_SerialPool where h.OrderNo.Equals(a.OrderNo) && h.SeqNo.Equals(a.SeqNo) && h.JobDetailType.Equals(b.JobDetailType) select h).Count(),
                                   cnt_notused = (from h in db.Dmn_SerialPool where h.OrderNo.Equals(a.OrderNo) && h.SeqNo.Equals(a.SeqNo) && h.JobDetailType.Equals(b.JobDetailType) && h.UseYN != "Y" select h).Count()
                               };
                    if (!string.IsNullOrEmpty(plantCode))
                        list = list.Where(q => q.a.PlantCode.Equals(plantCode));
                    if (!string.IsNullOrEmpty(lineID))
                        list = list.Where(q => q.a.LineID.Equals(lineID));
                    if (!string.IsNullOrEmpty(orderNo))
                        list = list.Where(q => q.a.OrderNo.Equals(orderNo));
                    if (!string.IsNullOrEmpty(seqNo))
                        list = list.Where(q => q.a.SeqNo.Equals(seqNo));
                    if (!string.IsNullOrEmpty(lot))
                        list = list.Where(q => q.a.LotNo.Contains(lot));
                    if (!string.IsNullOrEmpty(productCode))
                        list = list.Where(q => q.a.ProdCode.Contains(productCode));
                    if (!string.IsNullOrEmpty(jobDetailType))
                        list = list.Where(q => q.b.JobDetailType.Contains(jobDetailType));
                    if (!string.IsNullOrEmpty(productName))
                        list = list.Where(q => q.c.ProdName.Contains(productName));
                    if (!string.IsNullOrEmpty(status))
                        list = list.Where(q => q.b.JobStatus.Equals(status));
                    if (!string.IsNullOrEmpty(orderType))
                        list = list.Where(q => q.a.OrderType.Equals(orderType));
                    if (!string.IsNullOrEmpty(machineID))
                        list = list.Where(q => q.f.MachineID.Equals(machineID));
                    if (orderdateStart != null)
                        list = list.Where(q => q.a.InsertDate >= orderdateStart.Value);
                    if (orderdateEnd != null)
                        list = list.Where(q => q.a.InsertDate <= orderdateEnd.Value);
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Y") : q.a.UseYN.Equals("N"));
                    if (insertDateStart != null)
                        list = list.Where(q => q.a.InsertDate >= insertDateStart.Value);
                    if (insertDateEnd != null)
                        list = list.Where(q => q.a.InsertDate <= insertDateEnd.Value);
                    if (updateDateStart != null)
                        list = list.Where(q => q.a.UpdateDate != null && q.a.UpdateDate >= updateDateStart.Value);
                    if (updateDateEnd != null)
                        list = list.Where(q => q.a.UpdateDate != null && q.a.UpdateDate <= updateDateEnd.Value);
					if (assigneDateStart != null)
						list = list.Where(q => q.a.AssignDate != null && q.a.AssignDate >= assigneDateStart.Value);
					if (assignDateEnd != null)
						list = list.Where(q => q.a.AssignDate != null && q.a.AssignDate <= assignDateEnd.Value);
					if (!string.IsNullOrEmpty(standardCode))
                        list = list.Where(q => q.c.ProdStdCode != null && q.c.ProdStdCode.Equals(standardCode));
                    foreach (var item in list)
                    {
                        if (!retList.Any(x => x.OrderNo.Equals(item.a.OrderNo) && x.SeqNo.Equals(item.a.SeqNo)))
                        {
                            retList.Add(new JobOrder(item.a)
                            {
                                ProductStdCode = item.c == null ? null : item.c.ProdStdCode,
                                ProductName = item.c == null ? null : item.c.ProdName,
                                AGLevel = item.c == null ? null : item.c.AGLevel,
                                ProdName2 = item.c == null ? null : item.c.ProdName2,
                                ProductReserved1 = item.c == null ? null : item.c.Reserved1,
                                ProductReserved2 = item.c == null ? null : item.c.Reserved2,
                                ProductReserved3 = item.c == null ? null : item.c.Reserved3,
                                ProductReserved4 = item.c == null ? null : item.c.Reserved4,
                                ProductReserved5 = item.c == null ? null : item.c.Reserved5,
								ProductInterfaceDetail = item.c is null ? $"{EnInterfaceDetail.None}" : item.c.InterfaceDetail,
								ProductRemark = item.c == null ? null : item.c.Remark,
                            });
                        }
                        retList.Single(x => x.OrderNo.Equals(item.a.OrderNo) && x.SeqNo.Equals(item.a.SeqNo)).JobOrderDetail.Add(new JobOrderDetail(item.b)
                        {
                            SnExpressionID = item.g == null ? null : item.g.SnExpressionID,
                            SnExpressionStr = item.g == null ? null : item.g.SnExpressionStr,
                            SnType = item.d == null ? null : item.d.SnType,
                            BarcodeDataFormat = item.d == null ? null : item.d.BarcodeDataFormat,
                            BarcodeType = item.d == null ? null : item.d.BarcodeType,
                            GS1ExtensionCode = item.d == null ? null : item.d.GS1ExtensionCode,
                            MachineID = item.f == null ? null : item.f.MachineID,
                            Prefix_SSCC = item.d == null ? null : item.d.Prefix_SSCC,
                            PackingCount = item.d == null ? null : item.d.PackingCount,
                            ContentCount = item.d == null ? 1 : Convert.ToInt32(item.d.ContentCount),
                            Cnt_SerialTotal = item.cnt_total,
                            Cnt_SerialNotUsed = item.cnt_notused,
                            ResourceType = item.g == null ? null : item.d.ResourceType,
                            MinimumWeight = item.d == null ? null : item.d.MinimumWeight,
                            MaximumWeight = item.d == null ? null : item.d.MaximumWeight,
                            CustomBarcodeFormat = item.j == null ? null : item.j.CustomBarcodeFormatStr,

                        });
                    }
                    foreach(var item in retList)
                    {
                        item.JobOrderDetail = new JobOrderDetailCollection(item.JobOrderDetail.OrderByDescending(q => q.JobDetailType.Equals("LBL")).ThenByDescending(q => q.JobDetailType.Equals("EA")).ThenBy(q => q.JobDetailType).ToList());
                    }
                    if (pageIndex >= 0)
                    {
                        TotalCount = retList.Count();
                        TotalPageCount = Convert.ToInt64(Math.Ceiling(Convert.ToDouble(TotalCount) / Convert.ToDouble(pageSize)));
                        retList = retList.OrderBy(x => x.OrderNo).ThenBy(x => x.SeqNo).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    }
                }
                return retList;
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController SelectServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController SelectServer Exception", ex);
            }
            return retList;
        }
        public static JobOrder SelectServerSingle(string plantCode, string lineID, string orderNo, string seqNo, string lot, string productCode, string jobDetailType, string status, string orderType, string machineID,
            DateTime? orderdateStart, DateTime? orderdateEnd, DateTime? insertDateStart, DateTime? insertDateEnd, DateTime? updateDateStart, DateTime? updateDateEnd, DateTime? assigneDateStart, DateTime? assignDateEnd, bool? use, string standardCode, bool RaiseException = false)
        {
            List<JobOrder> retList = new List<JobOrder>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_JobOrder_M
                               join b in db.Dmn_JobOrder_D on new { a.PlantCode, a.OrderNo, a.SeqNo } equals new { b.PlantCode, b.OrderNo, b.SeqNo }
                               join c in db.Dmn_Product_M on new { a.PlantCode, a.ProdCode } equals new { c.PlantCode, c.ProdCode } into cc
                               from c in cc.DefaultIfEmpty()
                               join d in db.Dmn_Product_D on new { a.PlantCode, a.ProdCode, b.JobDetailType } equals new { d.PlantCode, d.ProdCode, d.JobDetailType } into dd
                               from d in dd.DefaultIfEmpty()
                               join e in db.Dmn_Line_M on new { a.PlantCode, a.LineID } equals new { e.PlantCode, e.LineID } into ee
                               from e in ee.DefaultIfEmpty()
                               join f in db.Dmn_Line_D on new { a.PlantCode, a.LineID, b.JobDetailType } equals new { f.PlantCode, f.LineID, f.JobDetailType } into ff
                               from f in ff.DefaultIfEmpty()
                               join g in db.Dmn_Serial_Expression on new { a.PlantCode, d.SnExpressionID } equals new { g.PlantCode, g.SnExpressionID } into gg
                               from g in gg.DefaultIfEmpty()
                               join j in db.Dmn_CustomBarcodeFormat on new { a.PlantCode, d.BarcodeDataFormat } equals new { j.PlantCode, BarcodeDataFormat = j.CustomBarcodeFormatID } into jj
                               from j in jj.DefaultIfEmpty()
                               orderby d.JobDetailType
                               select new
                               {
                                   a,
                                   b,
                                   c,
                                   d,
                                   e,
                                   f,
                                   g,
                                   j,
                                   cnt_total = (from h in db.Dmn_SerialPool where h.OrderNo.Equals(a.OrderNo) && h.SeqNo.Equals(a.SeqNo) && h.JobDetailType.Equals(b.JobDetailType) select h).Count(),
                                   cnt_notused = (from h in db.Dmn_SerialPool where h.OrderNo.Equals(a.OrderNo) && h.SeqNo.Equals(a.SeqNo) && h.JobDetailType.Equals(b.JobDetailType) && h.UseYN != "Y" select h).Count()
                               };
                    if (!string.IsNullOrEmpty(plantCode))
                        list = list.Where(q => q.a.PlantCode.Equals(plantCode));
                    if (!string.IsNullOrEmpty(lineID))
                        list = list.Where(q => q.a.LineID.Equals(lineID));
                    if (!string.IsNullOrEmpty(orderNo))
                        list = list.Where(q => q.a.OrderNo.Equals(orderNo));
                    if (!string.IsNullOrEmpty(seqNo))
                        list = list.Where(q => q.a.SeqNo.Equals(seqNo));
                    if (!string.IsNullOrEmpty(lot))
                        list = list.Where(q => q.a.LotNo.Contains(lot));
                    if (!string.IsNullOrEmpty(jobDetailType))
                        list = list.Where(q => q.b.JobDetailType.Equals(jobDetailType));
                    if (!string.IsNullOrEmpty(productCode))
                        list = list.Where(q => q.a.ProdCode.Equals(productCode));
                    if (!string.IsNullOrEmpty(status))
                        list = list.Where(q => q.b.JobStatus.Equals(status));
                    if (!string.IsNullOrEmpty(orderType))
                        list = list.Where(q => q.a.OrderType.Equals(orderType));
                    if (!string.IsNullOrEmpty(machineID))
                        list = list.Where(q => q.f.MachineID.Equals(machineID));
                    if (orderdateStart != null)
                        list = list.Where(q => q.a.InsertDate >= orderdateStart.Value);
                    if (orderdateEnd != null)
                        list = list.Where(q => q.a.InsertDate <= orderdateEnd.Value);
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Y") : q.a.UseYN.Equals("N"));
					if (insertDateStart != null)
						list = list.Where(q => q.a.InsertDate >= insertDateStart.Value);
					if (insertDateEnd != null)
						list = list.Where(q => q.a.InsertDate <= insertDateEnd.Value);
					if (updateDateStart != null)
						list = list.Where(q => q.a.UpdateDate != null && q.a.UpdateDate >= updateDateStart.Value);
					if (updateDateEnd != null)
						list = list.Where(q => q.a.UpdateDate != null && q.a.UpdateDate <= updateDateEnd.Value);
					if (assigneDateStart != null)
						list = list.Where(q => q.a.AssignDate != null && q.a.AssignDate >= assigneDateStart.Value);
					if (assignDateEnd != null)
						list = list.Where(q => q.a.AssignDate != null && q.a.AssignDate <= assignDateEnd.Value);
					if (!string.IsNullOrEmpty(standardCode))
						list = list.Where(q => q.c.ProdStdCode != null && q.c.ProdStdCode.Equals(standardCode));
					foreach (var item in list)
                    {
                        if (!retList.Any(x => x.OrderNo.Equals(item.a.OrderNo) && x.SeqNo.Equals(item.a.SeqNo)))
                        {
                            retList.Add(new JobOrder(item.a)
                            {
                                ProductStdCode = item.c == null ? null : item.c.ProdStdCode,
                                ProductName = item.c == null ? null : item.c.ProdName,
                                ProdName2 = item.c == null ? null : item.c.ProdName2,
                                AGLevel = item.c == null ? null : item.c.AGLevel,
                                ProductReserved1 = item.c == null ? null : item.c.Reserved1,
                                ProductReserved2 = item.c == null ? null : item.c.Reserved2,
                                ProductReserved3 = item.c == null ? null : item.c.Reserved3,
                                ProductReserved4 = item.c == null ? null : item.c.Reserved4,
                                ProductReserved5 = item.c == null ? null : item.c.Reserved5,
								ProductInterfaceDetail = item.c == null ? $"{EnInterfaceDetail.None}" : item.c.InterfaceDetail,
							});
                        }
                        retList.Single(x => x.OrderNo.Equals(item.a.OrderNo) && x.SeqNo.Equals(item.a.SeqNo)).JobOrderDetail.Add(new JobOrderDetail(item.b)
                        {
                            SnExpressionID = item.g == null ? null : item.g.SnExpressionID,
                            SnExpressionStr = item.g == null ? null : item.g.SnExpressionStr,
                            SnType = item.d == null ? null : item.d.SnType,
                            BarcodeDataFormat = item.d == null ? null : item.d.BarcodeDataFormat,
                            BarcodeType = item.d == null ? null : item.d.BarcodeType,
                            GS1ExtensionCode = item.d == null ? null : item.d.GS1ExtensionCode,
                            MachineID = item.f == null ? null : item.f.MachineID,
                            Prefix_SSCC = item.d == null ? null : item.d.Prefix_SSCC,
                            ContentCount = item.d == null ? 1 : Convert.ToInt32(item.d.ContentCount),
                            Cnt_SerialTotal = item.cnt_total,
                            Cnt_SerialNotUsed = item.cnt_notused,
                            ProdStdCode = item.d == null ? null : item.d.ProdStdCode,
                            MinimumWeight = item.d == null ? null : item.d.MinimumWeight,
                            MaximumWeight = item.d == null ? null : item.d.MaximumWeight,
                            CustomBarcodeFormat = item.j == null ? null : item.j.CustomBarcodeFormatStr,
                        });
                    }
                    foreach (var item in retList)
                    {
                        item.JobOrderDetail = new JobOrderDetailCollection(item.JobOrderDetail.OrderByDescending(q => q.JobDetailType.Equals("LBL")).ThenByDescending(q => q.JobDetailType.Equals("EA")).ThenBy(q => q.JobDetailType).ToList());
                    }
                    if (list.Count() == 0)
                    {
                        log.InfoFormat("JobOrderController : list.Count() == 0");
                    }
                    return retList.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController SelectServerSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController SelectServerSingle Exception", ex);
            }
            return retList.FirstOrDefault();
        }
        public static List<JobOrder> SelectServerPM(string plantCode, string lineID, string orderNo, string seqNo, string lot, string productCode, string status, string orderType, DateTime? orderdateStart,
              DateTime? orderdateEnd, bool? use, int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            List<JobOrder> retList = new List<JobOrder>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_JobOrder_M
                               join b in db.Dmn_JobOrder_D
                               on new { a.OrderNo, a.SeqNo } equals new { b.OrderNo, b.SeqNo }
                               where b.JobDetailType == "EA" || b.JobDetailType == "LBL"
                               join c in db.Dmn_Product_M on a.ProdCode equals c.ProdCode
                               select new { a, b, c };
                    if (!string.IsNullOrEmpty(plantCode))
                        list = list.Where(q => q.a.PlantCode.Equals(plantCode) && q.b.PlantCode.Equals(plantCode));
                    if (!string.IsNullOrEmpty(lineID))
                        list = list.Where(q => q.a.LineID.Equals(lineID));
                    if (!string.IsNullOrEmpty(orderNo))
                        list = list.Where(q => q.a.OrderNo.Equals(orderNo));
                    if (!string.IsNullOrEmpty(seqNo))
                        list = list.Where(q => q.a.SeqNo.Equals(seqNo));
                    if (!string.IsNullOrEmpty(lot))
                        list = list.Where(q => q.a.LotNo.Contains(lot));
                    if (!string.IsNullOrEmpty(productCode))
                        list = list.Where(q => q.a.ProdCode.Contains(productCode));
                    if (!string.IsNullOrEmpty(status))
                        list = list.Where(q => q.b.JobStatus.Equals(status));
                    if (!string.IsNullOrEmpty(orderType))
                        list = list.Where(q => q.a.OrderType.Equals(orderType));
                    if (orderdateStart != null)
                        list = list.Where(q => q.a.InsertDate >= orderdateStart.Value);
                    if (orderdateEnd != null)
                        list = list.Where(q => q.a.InsertDate <= orderdateEnd.Value);
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Y") : q.a.UseYN.Equals("N"));
                    foreach (var item in list)
                    {
                        if (!retList.Any(x => x.OrderNo.Equals(item.a.OrderNo) && x.SeqNo.Equals(item.a.SeqNo)))
                            retList.Add(new JobOrder(item.a)
                            {
                                ProductStdCode = item.c.ProdStdCode,
                                ProductName = item.c.ProdName,
                                AGLevel = item.c.AGLevel,
                                ProductReserved1 = item.c == null ? null : item.c.Reserved1,
                                ProductReserved2 = item.c == null ? null : item.c.Reserved2,
                                ProductReserved3 = item.c == null ? null : item.c.Reserved3,
                                ProductReserved4 = item.c == null ? null : item.c.Reserved4,
                                ProductReserved5 = item.c == null ? null : item.c.Reserved5,
								ProductInterfaceDetail = item.c is null ? $"{EnInterfaceDetail.None}" : item.c.InterfaceDetail,
							});
                        retList.Single(x => x.OrderNo.Equals(item.a.OrderNo) && x.SeqNo.Equals(item.a.SeqNo)).JobOrderDetail.Add(new JobOrderDetail(item.b));
                    }
                    if (pageIndex >= 0)
                    {
                        TotalCount = retList.Count();
                        TotalPageCount = Convert.ToInt64(Math.Ceiling(Convert.ToDouble(TotalCount) / Convert.ToDouble(pageSize)));
                        retList = retList.OrderBy(x => x.OrderNo).ThenBy(x => x.SeqNo).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    }
                }
                foreach (var item in retList)
                {
                    item.JobOrderDetail = new JobOrderDetailCollection(item.JobOrderDetail.OrderByDescending(q => q.JobDetailType.Equals("LBL")).ThenByDescending(q => q.JobDetailType.Equals("EA")).ThenBy(q => q.JobDetailType).ToList());
                }
                return retList;
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController SelectServerPM Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController SelectServerPM Exception", ex);
            }
            return retList;
        }
        public static JobOrder SelectServerSinglePM(string plantCode, string lineID, string orderNo, string seqNo, string lot, string productCode, string status, string orderType, DateTime? orderdateStart,
             DateTime? orderdateEnd, bool? use, bool RaiseException = false)
        {
            List<JobOrder> retList = new List<JobOrder>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_JobOrder_M
                               join b in db.Dmn_JobOrder_D
                               on new { a.OrderNo, a.SeqNo } equals new { b.OrderNo, b.SeqNo }
                               where b.JobDetailType == "EA" || b.JobDetailType == "LBL"
                               join c in db.Dmn_Product_M on a.ProdCode equals c.ProdCode
                               select new { a, b, c };
                    if (!string.IsNullOrEmpty(plantCode))
                        list = list.Where(q => q.a.PlantCode.Equals(plantCode) && q.b.PlantCode.Equals(plantCode));
                    if (!string.IsNullOrEmpty(lineID))
                        list = list.Where(q => q.a.LineID.Equals(lineID));
                    if (!string.IsNullOrEmpty(orderNo))
                        list = list.Where(q => q.a.OrderNo.Equals(orderNo));
                    if (!string.IsNullOrEmpty(seqNo))
                        list = list.Where(q => q.a.SeqNo.Equals(seqNo));
                    if (!string.IsNullOrEmpty(lot))
                        list = list.Where(q => q.a.LotNo.Contains(lot));
                    if (!string.IsNullOrEmpty(productCode))
                        list = list.Where(q => q.a.ProdCode.Contains(productCode));
                    if (!string.IsNullOrEmpty(status))
                        list = list.Where(q => q.b.JobStatus.Equals(status));
                    if (!string.IsNullOrEmpty(orderType))
                        list = list.Where(q => q.a.OrderType.Equals(orderType));
                    if (orderdateStart != null)
                        list = list.Where(q => q.a.InsertDate >= orderdateStart.Value);
                    if (orderdateEnd != null)
                        list = list.Where(q => q.a.InsertDate <= orderdateEnd.Value);
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Y") : q.a.UseYN.Equals("N"));
                    foreach (var item in list)
                    {
                        if (!retList.Any(x => x.OrderNo.Equals(item.a.OrderNo) && x.SeqNo.Equals(item.a.SeqNo)))
                            retList.Add(new JobOrder(item.a)
                            {
                                ProductStdCode = item.c.ProdStdCode,
                                ProductName = item.c.ProdName,
                                AGLevel = item.c.AGLevel,
                                ProductReserved1 = item.c == null ? null : item.c.Reserved1,
                                ProductReserved2 = item.c == null ? null : item.c.Reserved2,
                                ProductReserved3 = item.c == null ? null : item.c.Reserved3,
                                ProductReserved4 = item.c == null ? null : item.c.Reserved4,
                                ProductReserved5 = item.c == null ? null : item.c.Reserved5,
								ProductInterfaceDetail = item.c is null ? $"{EnInterfaceDetail.None}" : item.c.InterfaceDetail,
							});
                        retList.Single(x => x.OrderNo.Equals(item.a.OrderNo) && x.SeqNo.Equals(item.a.SeqNo)).JobOrderDetail.Add(new JobOrderDetail(item.b));
                    }
                    foreach (var item in retList)
                    {
                        item.JobOrderDetail = new JobOrderDetailCollection(item.JobOrderDetail.OrderByDescending(q => q.JobDetailType.Equals("LBL")).ThenByDescending(q => q.JobDetailType.Equals("EA")).ThenBy(q => q.JobDetailType).ToList());
                    }
                    return retList.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController SelectServerSinglePM Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController SelectServerSinglePM Exception", ex);
            }
            return retList.FirstOrDefault();
        }
        public static List<JobOrder> SelectServerAG(string plantCode, string lineID, string orderNo, string seqNo, string lot, string productCode, string status, string orderType, DateTime? orderdateStart,
              DateTime? orderdateEnd, bool? use, int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            List<JobOrder> retList = new List<JobOrder>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_JobOrder_M
                               join b in db.Dmn_JobOrder_D
                               on new { a.OrderNo, a.SeqNo } equals new { b.OrderNo, b.SeqNo }
                               where b.JobDetailType.Contains("BX")
                               join c in db.Dmn_Product_M on a.ProdCode equals c.ProdCode
                               orderby b.JobDetailType
                               select new { a, b, c };
                    if (!string.IsNullOrEmpty(plantCode))
                        list = list.Where(q => q.a.PlantCode.Equals(plantCode) && q.b.PlantCode.Equals(plantCode));
                    if (!string.IsNullOrEmpty(lineID))
                        list = list.Where(q => q.a.LineID.Equals(lineID));
                    if (!string.IsNullOrEmpty(orderNo))
                        list = list.Where(q => q.a.OrderNo.Equals(orderNo));
                    if (!string.IsNullOrEmpty(seqNo))
                        list = list.Where(q => q.a.SeqNo.Equals(seqNo));
                    if (!string.IsNullOrEmpty(lot))
                        list = list.Where(q => q.a.LotNo.Contains(lot));
                    if (!string.IsNullOrEmpty(productCode))
                        list = list.Where(q => q.a.ProdCode.Contains(productCode));
                    if (!string.IsNullOrEmpty(status))
                        list = list.Where(q => q.b.JobStatus.Equals(status));
                    if (!string.IsNullOrEmpty(orderType))
                        list = list.Where(q => q.a.OrderType.Equals(orderType));
                    if (orderdateStart != null)
                        list = list.Where(q => q.a.InsertDate >= orderdateStart.Value);
                    if (orderdateEnd != null)
                        list = list.Where(q => q.a.InsertDate <= orderdateEnd.Value);
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Y") : q.a.UseYN.Equals("N"));
                    foreach (var item in list)
                    {
                        if (!retList.Any(x => x.OrderNo.Equals(item.a.OrderNo) && x.SeqNo.Equals(item.a.SeqNo)))
                            retList.Add(new JobOrder(item.a)
                            {
                                ProductStdCode = item.c.ProdStdCode,
                                ProductName = item.c.ProdName,
                                AGLevel = item.c.AGLevel,
                                ProductReserved1 = item.c == null ? null : item.c.Reserved1,
                                ProductReserved2 = item.c == null ? null : item.c.Reserved2,
                                ProductReserved3 = item.c == null ? null : item.c.Reserved3,
                                ProductReserved4 = item.c == null ? null : item.c.Reserved4,
                                ProductReserved5 = item.c == null ? null : item.c.Reserved5,
								ProductInterfaceDetail = item.c is null ? $"{EnInterfaceDetail.None}" : item.c.InterfaceDetail,
							});
                        retList.Single(x => x.OrderNo.Equals(item.a.OrderNo) && x.SeqNo.Equals(item.a.SeqNo)).JobOrderDetail.Add(new JobOrderDetail(item.b));
                    }
                    if (pageIndex >= 0)
                    {
                        TotalCount = retList.Count();
                        TotalPageCount = Convert.ToInt64(Math.Ceiling(Convert.ToDouble(TotalCount) / Convert.ToDouble(pageSize)));
                        retList = retList.OrderBy(x => x.OrderNo).ThenBy(x => x.SeqNo).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    }
                }
                foreach (var item in retList)
                {
                    item.JobOrderDetail = new JobOrderDetailCollection(item.JobOrderDetail.OrderByDescending(q => q.JobDetailType.Equals("LBL")).ThenByDescending(q => q.JobDetailType.Equals("EA")).ThenBy(q => q.JobDetailType).ToList());
                }
                return retList;
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController SelectServerAG Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController SelectServerAG Exception", ex);
            }
            return retList;
        }
        public static JobOrder SelectServerSingleAG(string plantCode, string lineID, string orderNo, string seqNo, string lot, string productCode, string status, string orderType, DateTime? orderdateStart,
             DateTime? orderdateEnd, bool? use, bool RaiseException = false)
        {
            List<JobOrder> retList = new List<JobOrder>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_JobOrder_M
                               join b in db.Dmn_JobOrder_D
                               on new { a.OrderNo, a.SeqNo } equals new { b.OrderNo, b.SeqNo }
                               where b.JobDetailType.Contains("BX")
                               join c in db.Dmn_Product_M on a.ProdCode equals c.ProdCode
                               orderby b.JobDetailType
                               select new { a, b, c };
                    if (!string.IsNullOrEmpty(plantCode))
                        list = list.Where(q => q.a.PlantCode.Equals(plantCode) && q.b.PlantCode.Equals(plantCode));
                    if (!string.IsNullOrEmpty(lineID))
                        list = list.Where(q => q.a.LineID.Equals(lineID));
                    if (!string.IsNullOrEmpty(orderNo))
                        list = list.Where(q => q.a.OrderNo.Equals(orderNo));
                    if (!string.IsNullOrEmpty(seqNo))
                        list = list.Where(q => q.a.SeqNo.Equals(seqNo));
                    if (!string.IsNullOrEmpty(lot))
                        list = list.Where(q => q.a.LotNo.Contains(lot));
                    if (!string.IsNullOrEmpty(productCode))
                        list = list.Where(q => q.a.ProdCode.Contains(productCode));
                    if (!string.IsNullOrEmpty(status))
                        list = list.Where(q => q.b.JobStatus.Equals(status));
                    if (!string.IsNullOrEmpty(orderType))
                        list = list.Where(q => q.a.OrderType.Equals(orderType));
                    if (orderdateStart != null)
                        list = list.Where(q => q.a.InsertDate >= orderdateStart.Value);
                    if (orderdateEnd != null)
                        list = list.Where(q => q.a.InsertDate <= orderdateEnd.Value);
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Y") : q.a.UseYN.Equals("N"));
                    foreach (var item in list)
                    {
                        if (!retList.Any(x => x.OrderNo.Equals(item.a.OrderNo) && x.SeqNo.Equals(item.a.SeqNo)))
                            retList.Add(new JobOrder(item.a)
                            {
                                ProductStdCode = item.c.ProdStdCode,
                                ProductName = item.c.ProdName,
                                AGLevel = item.c.AGLevel,
                                ProductReserved1 = item.c == null ? null : item.c.Reserved1,
                                ProductReserved2 = item.c == null ? null : item.c.Reserved2,
                                ProductReserved3 = item.c == null ? null : item.c.Reserved3,
                                ProductReserved4 = item.c == null ? null : item.c.Reserved4,
                                ProductReserved5 = item.c == null ? null : item.c.Reserved5,
								ProductInterfaceDetail = item.c is null ? $"{EnInterfaceDetail.None}" : item.c.InterfaceDetail,
							});
                        retList.Single(x => x.OrderNo.Equals(item.a.OrderNo) && x.SeqNo.Equals(item.a.SeqNo)).JobOrderDetail.Add(new JobOrderDetail(item.b));
                    }
                }
                foreach (var item in retList)
                {
                    item.JobOrderDetail = new JobOrderDetailCollection(item.JobOrderDetail.OrderByDescending(q => q.JobDetailType.Equals("LBL")).ThenByDescending(q => q.JobDetailType.Equals("EA")).ThenBy(q => q.JobDetailType).ToList());
                }
                return retList.FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController SelectServerSingleAG Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController SelectServerSingleAG Exception", ex);
            }
            return retList.FirstOrDefault();
        }
        public static bool InsertServer(DSM.Dmn_JobOrder_M item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("JobOrderController InsertServerMaster by {0} , {1}", item.InsertUser, item.ToString());
                    db.Dmn_JobOrder_M.AddOrUpdate(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController InsertServerMaster Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController InsertServerMaster Exception", ex);
                return false;
            }
        }
        public static bool InsertServer(DSM.Dmn_JobOrder_D item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("JobOrderController InsertServerDetail by {0}, {1}", item.UpdateUser ?? item.InsertUser, item.ToString());
                    db.Dmn_JobOrder_D.AddOrUpdate(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController InsertServerPM Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController InsertServerPM Exception", ex);
                return false;
            }
        }
        public static bool UpdateServer(DSM.Dmn_JobOrder_M item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("JobOrderController UpdateServerMaster by {0}, {1}", item.UpdateUser, item.ToString());
                    var tmp = db.Dmn_JobOrder_M.First(q => q.PlantCode.Equals(item.PlantCode) && q.OrderNo.Equals(item.OrderNo) && q.SeqNo.Equals(item.SeqNo));
                    tmp.WriteExistings(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController UpdateServerMaster Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController UpdateServerMaster Exception", ex);
                return false;
            }
        }
        public static bool UpdateServer(DSM.Dmn_JobOrder_D item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("JobOrderController UpdateServerDetail by {0}, {1}", item.UpdateUser, item.ToString());
                    var tmp = db.Dmn_JobOrder_D.First(q => q.PlantCode.Equals(item.PlantCode) && q.OrderNo.Equals(item.OrderNo) && q.SeqNo.Equals(item.SeqNo));
                    tmp.WriteExistings(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController UpdateServerDetail Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController UpdateServerDetail Exception", ex);
                return false;
            }
        }
        public static bool UpdateCountServerQuery(JobOrder item, string machineid, bool RaiseException = false)
        {
            try
            {
                Dmn_JobOrder_D svr = null;
                var isChanged = false;
                using (var ctx = new DominoDBServer())
                {
                    var lineid = ctx.Dmn_JobOrder_M.FirstOrDefault(x => x.PlantCode == item.PlantCode && x.OrderNo == item.OrderNo && x.SeqNo == item.SeqNo)?.LineID ?? throw new InvalidOperationException();
                    var jobdetailtype = ctx.Dmn_Line_D.FirstOrDefault(x => x.LineID == lineid && x.MachineID == machineid)?.JobDetailType ?? throw new InvalidOperationException();

                    svr = ctx.Dmn_JobOrder_D.FirstOrDefault(x => x.PlantCode == item.PlantCode && x.OrderNo == item.OrderNo && x.SeqNo == item.SeqNo && x.JobDetailType == jobdetailtype);
                    JobOrderDetail reported = item.JobOrderDetail.FirstOrDefault(i => i.JobDetailType == jobdetailtype);
                    isChanged = IsCountDifferent(svr, reported);

                    if (isChanged)
                    {
                        var cntDesc = $" Cnt_Good = {reported.Cnt_Good}, "
                                      + $"Cnt_Error = {reported.Cnt_Error}, "
                                      + $"Cnt_Sample = {reported.Cnt_Sample}, "
                                      + $"Cnt_Destroy = {reported.Cnt_Destroy}, "
                                      + $"Cnt_Child = {reported.Cnt_Child}, "
                                      + $"Cnt_Work = {reported.Cnt_Work}, "
                                      + $"Cnt_Parent = {reported.Cnt_Parent}, "
                                      + $"Cnt_SNLast = {reported.Cnt_SNLast}, "
                                      + $"Cnt_SNPrintLast = {reported.Cnt_SNPrintLast}, "
                                      + $"Cnt_SN_Movil = {reported.Cnt_SN_Movil}, "
                                      + $"Cnt_SN_DSM = {reported.Cnt_SN_DSM}, "
                                      + $"Cnt_SN_Lot_O = {reported.Cnt_SN_Lot_O}, "
                                      + $"Cnt_SN_Lot_X = {reported.Cnt_SN_Lot_X}, "
                                      + $"Cnt_Status1 = {reported.Cnt_Status1}, "
                                      + $"Cnt_Status2 = {reported.Cnt_Status2}, "
                                      + $"Cnt_Status3 = {reported.Cnt_Status3}, "
                                      + $"Cnt_Status4 = {reported.Cnt_Status4}, "
                                      + $"Cnt_Status5 = {reported.Cnt_Status5} ";

                        var sql = $@"UPDATE {nameof(Dmn_JobOrder_D)}
                                     SET UPDATEUSER = '{item.UpdateUser}',
                                         UPDATEDATE = '{item.UpdateDate?.ToString("yyyy-MM-dd HH:mm:ss.fff")}', 
                                     {cntDesc}
                                     WHERE PLANTCODE = '{item.PlantCode}' AND OrderNo = '{item.OrderNo}' AND SeqNo = '{item.SeqNo}' AND JobDetailType = '{reported.JobDetailType}'";

                        
                        log.Info($"JobOrderController UpdateCountServer by {item.UpdateUser} [{machineid}], {item.PlantCode} {item.OrderNo}{item.SeqNo} {reported.JobDetailType} : \n{{{cntDesc}}}");

                        ctx.Database.ExecuteSqlCommand(sql);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController UpdateCountServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController UpdateCountServer Exception", ex);
                return false;
            }
        }

        private static bool IsCountDifferent(Dmn_JobOrder_D s, JobOrderDetail i)
        {
            return !(s.Cnt_Good == (i?.Cnt_Good ?? -1)
                   && s.Cnt_Error == (i?.Cnt_Error ?? -1)
                   && s.Cnt_Sample == (i?.Cnt_Sample ?? -1)
                   && s.Cnt_Destroy == (i?.Cnt_Destroy ?? -1)
                   && s.Cnt_Child == (i?.Cnt_Child ?? -1)
                   && s.Cnt_Work == (i?.Cnt_Work ?? -1)
                   && s.Cnt_Parent == (i?.Cnt_Parent ?? -1)
                   && s.Cnt_SNLast == (i?.Cnt_SNLast ?? -1)
                   && s.Cnt_SNPrintLast == (i?.Cnt_SNPrintLast ?? -1)
                   && s.Cnt_SN_Movil == (i?.Cnt_SN_Movil ?? -1)
                   && s.Cnt_SN_DSM == (i?.Cnt_SN_DSM ?? -1)
                   && s.Cnt_SN_Lot_O == (i?.Cnt_SN_Lot_O ?? -1)
                   && s.Cnt_SN_Lot_X == (i?.Cnt_SN_Lot_X ?? -1)
                   && s.Cnt_Status1 == (i?.Cnt_Status1 ?? -1)
                   && s.Cnt_Status2 == (i?.Cnt_Status2 ?? -1)
                   && s.Cnt_Status3 == (i?.Cnt_Status3 ?? -1)
                   && s.Cnt_Status4 == (i?.Cnt_Status4 ?? -1)
                   && s.Cnt_Status5 == (i?.Cnt_Status5 ?? -1));
        }

        public static bool DeleteServerDetail(string plantCode, string orderNo, string seqNo, string jobDetail, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("JobOrderController DeleteServerDetail by {0}", userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_JobOrder_D where PlantCode = {0} AND OrderNo = {1} AND SeqNo = {2} AND JobDetailType = {3}", plantCode, orderNo, seqNo, jobDetail);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController DeleteServerDetail Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController DeleteServerDetail Exception", ex);
                return false;
            }
        }
        public static bool DeleteServerDetailAll(string plantCode, string orderNo, string seqNo, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("JobOrderController DeleteServerDetailAll {0} by {1}", plantCode, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_JobOrder_D where PlantCode = {0} AND OrderNo = {1} AND SeqNo = {2}", plantCode, orderNo, seqNo);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController DeleteServerDetailAll Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController DeleteServerDetailAll Exception", ex);
                return false;
            }
        }
        public static int? GetSequenceNumber(string plantCode, DateTime date, bool RaiseException = false)
        {
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    string tmp = string.Format("{0}", date.ToString("yyyyMMdd"));
                    return Convert.ToInt32(db.Dmn_JobOrder_M.Where(q => q.PlantCode.Equals(plantCode) && q.OrderNo.Contains(tmp)).Select(q => q.SeqNo).Max());
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrderController SelectServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("JobOrderController SelectServer Exception", ex);
            }
            return null;
        }
    }
}
