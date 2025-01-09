using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using DominoFunctions.ExtensionMethod;
using log4net;
using Newtonsoft.Json;
using DominoFunctions.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace DominoDatabase
{
    [Serializable]
    public class JobOrder
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string PlantCode { get; set; }
        public string OrderNo { get; set; }
        public string SeqNo { get; set; }
        public string LineID { get; set; }
        public string CorCode { get; set; }
        public string ErpOrderNo { get; set; }
        public string OrderType { get; set; }
        public DateTime? MfdDate { get; set; }
        public DateTime? ExpDate { get; set; }
        public string ProdCode { get; set; }
        public string LotNo { get; set; }
        public string LotNo_Sub { get; set; }
        public string UseYN { get; set; }
        public int Cnt_JobPlan { get; set; }
        public string Reserved1 { get; set; }
        public string Reserved2 { get; set; }
        public string Reserved3 { get; set; }
        public string Reserved4 { get; set; }
        public string Reserved5 { get; set; }
        public string AssignUser { get; set; }
        public DateTime? AssignDate { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DateOfTest { get; set; }
        public string DSMReportUser { get; set; }
        public DateTime? DSMReportDate { get; set; }
        public string ProductStdCode { get; set; }
        public string ProductName { get; set; }
        public string ProductName1 { get; set; }
        public string ProductName2 { get; set; }
        [JsonIgnore]
        public int ManualBufferIndex { get; set; }
        public int? AGLevel { get; set; }
        public int Delay_Print { get; set; }
        public int Delay_Print2 { get; set; }
        public int Delay_Shot1 { get; set; }
        public int Delay_Shot2 { get; set; }
        public int Delay_Shot3 { get; set; }
        public int Delay_Shot4 { get; set; }
        public int Delay_NG { get; set; }
        public string BarcodeDataFormat { get; set; }
        public string ProdName { get; set; }
        public string ProdName2 { get; set; }
		public int? MedicineType { get; set; }
		public string ProductReserved1 { get; set; }
        public string ProductReserved2 { get; set; }
        public string ProductReserved3 { get; set; }
        public string ProductReserved4 { get; set; }
        public string ProductReserved5 { get; set; }
        public string ProductRemark { get; set; }
        public string ProductInterfaceDetail { get; set; }

        [JsonIgnore]
        public string JobNumber
        {
            get { return string.Format("{0}{1}", OrderNo, SeqNo); }
        }
        [JsonIgnore]
        public EnJobOrderType EnOrderType
        {
            get
            {
                switch(OrderType)
                {
                    case "NM":
                        return EnJobOrderType.Normal;
                    case "TS":
                        return EnJobOrderType.Test;
                    case "MA":
                        return EnJobOrderType.Manual;
                    case "TP":
                        return EnJobOrderType.TestPrint;
                    case "MP":
                        return EnJobOrderType.ManualPrint;
                    case "RE":
                        return EnJobOrderType.Repack;
                    default:
                        throw new NotImplementedException();
                }
            }
            set
            {
                switch(value)
                {
                    case EnJobOrderType.Normal:
                        OrderType = "NM";
                        break;
                    case EnJobOrderType.Test:
                        OrderType = "TS";
                        break;
                    case EnJobOrderType.Manual:
                        OrderType = "MA";
                        break;
                    case EnJobOrderType.TestPrint:
                        OrderType = "TP";
                        break;
                    case EnJobOrderType.ManualPrint:
                        OrderType = "MP";
                        break;
                    case EnJobOrderType.Repack:
                        OrderType = "RE";
                        break;
                    case EnJobOrderType.Change:
                        OrderType = "CH";
                        break;
                }
            }
        }
        

        public static EnJobOrderType OrderTypeConverter(string value)
        {
            switch (value)
            {
                case "NM":
                    return EnJobOrderType.Normal;
                case "TS":
                    return EnJobOrderType.Test;
                case "MA":
                    return EnJobOrderType.Manual;
                case "TP":
                    return EnJobOrderType.TestPrint;
                case "MP":
                    return EnJobOrderType.ManualPrint;
                case "RE":
                    return EnJobOrderType.Repack;
                case "CH":
                    return EnJobOrderType.Change;
                default:
                    throw new NotImplementedException();
            }
        }
        public static string OrderTypeConverter(EnJobOrderType value)
        {
            switch (value)
            {
                case EnJobOrderType.Normal:
                    return "NM";
                case EnJobOrderType.Test:
                    return "TS";
                case EnJobOrderType.Manual:
                    return "MA";
                case EnJobOrderType.TestPrint:
                    return "TP";
                case EnJobOrderType.ManualPrint:
                    return "MP";
                case EnJobOrderType.Repack:
                    return "RE";
                case EnJobOrderType.Change:
                    return "CH";
                default:
                    throw new NotImplementedException();
            }
        }

        public static EnJobStatus JobStatusConverter(string status)
        {
            switch (status)
            {
                case "IS":
                    return EnJobStatus.InitialState;
                case "AS":
                    return EnJobStatus.Assign;
                case "AP":
                    return EnJobStatus.Applied;
                case "PS":
                    return EnJobStatus.Pause;
                case "RU":
                    return EnJobStatus.Run;
                case "PC":
                    return EnJobStatus.ProductionComplete;
                case "RP":
                    return EnJobStatus.ReportComplete;
                case "AC":
                    return EnJobStatus.AssembleComplete;
                case "DC":
                    return EnJobStatus.DSM_ReportComplete;
                case "CC":
                    return EnJobStatus.Cancel;
                default:
                    throw new NotImplementedException();
            }
        }
        public static string JobStatusConverter(EnJobStatus status)
        {
            switch (status)
            {
                case EnJobStatus.InitialState:
                    return "IS";
                case EnJobStatus.Assign:
                    return "AS";
                case EnJobStatus.Applied:
                    return "AP";
                case EnJobStatus.Pause:
                    return "PS";
                case EnJobStatus.Run:
                    return "RU";
                case EnJobStatus.ProductionComplete:
                    return "PC";
                case EnJobStatus.ReportComplete:
                    return "RP";
                case EnJobStatus.AssembleComplete:
                    return "AC";
                case EnJobStatus.DSM_ReportComplete:
                    return "DC";
                case EnJobStatus.Cancel:
                    return "CC";
                default:
                    throw new NotImplementedException();
            }
        }
        [JsonIgnore]
		public EnInterfaceDetail EnInterfaceDetail
		{
			get
			{
				if (Enum.TryParse(ProductInterfaceDetail, out EnInterfaceDetail ifd))
                {
                    return ifd;
                }
                else
                {
                    return EnInterfaceDetail.None;
                }
			}
			set
			{
                ProductInterfaceDetail = $"{value}";
			}
		}
		[JsonIgnore]
        public ProductDetailCollection ProductDetail { get; set; }
        public JobOrderDetailCollection JobOrderDetail { get; set; }
        [JsonIgnore]
        public string FirstDetailStandardCode
        {
            get
            {
                if (JobOrderDetail?.Count > 0 && !string.IsNullOrEmpty(JobOrderDetail[0].ProdStdCode))
                {
                    return JobOrderDetail[0].ProdStdCode;
                }
                return ProductStdCode;
            }
        }
        [JsonIgnore]
        public int ContentCount
        {
            get
            {
                if (JobOrderDetail?.Count > 0 && !(JobOrderDetail[0].ContentCount is null))
                    return (int)JobOrderDetail[0].ContentCount;
                else if (ProductDetail?.Count > 0 && !(ProductDetail[0].ContentCount is null))
                    return (int)ProductDetail[0].ContentCount;
                else
                    return 1;
            }
        }

        public JobOrder()
        {
            JobOrderDetail = new JobOrderDetailCollection();
            ProductDetail = new ProductDetailCollection();
        }
        public JobOrder(object list)
            : this()
        {
            this.UnionClass(list);
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Plant, PlantCode);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.OrderNo, OrderNo);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.SeqNo, SeqNo);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.LineID, LineID);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.CorCode, CorCode);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.ErpOrderNumber, ErpOrderNo);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.JobOrderType, OrderType);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.MfdDate, MfdDate);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.ExpDate, ExpDate);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.ProdCode, ProdCode);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.ProdName, ProdName);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.LotNo, LotNo);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.SubLot, LotNo_Sub);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved1, Reserved1);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved2, Reserved2);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved3, Reserved3);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved4, Reserved4);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved5, Reserved5);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.InsertUser, InsertUser);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.InsertDate, InsertDate);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.UpdateUser, UpdateUser);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.UpdateDate, UpdateDate);
            foreach (JobOrderDetail dt in this.JobOrderDetail)
            {
                sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Detail, dt.ToString());
            }
            return sb.ToString();
        }
        public string ToSplitString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if(!(PlantCode is null))
                sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.Plant, PlantCode);
            if (!(OrderNo is null))
                sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.OrderNo, OrderNo);
            if (!(SeqNo is null))
                sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.SeqNo, SeqNo);
            if (!(LineID is null))
                sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.LineID, LineID);
            if (!(CorCode is null))
                sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.CorCode, CorCode);
            if (!(ErpOrderNo is null))
                sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.ErpOrderNumber, ErpOrderNo);
            if (!(OrderType is null))
                sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.JobOrderType, OrderType);
            if (!(MfdDate is null))
                sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.MfdDate, ((DateTime)MfdDate).ToString("yyyyMMdd"));
            if (!(ExpDate is null))
                sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.ExpDate, ((DateTime)ExpDate).ToString("yyyyMMdd"));
            if (!(DateOfTest is null))
                sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.DateOfTest, ((DateTime)DateOfTest).ToString("yyyyMMdd"));
            if (!(this.FirstDetailStandardCode is null))
                sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.ProdStdCode, this.FirstDetailStandardCode);
            if (!(ProdCode is null))
                sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.ProdCode, ProdCode);
            if (!(ProdName is null))
                sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.ProdName, ProdName);
            if (!(LotNo is null))
                sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.LotNo, LotNo);
            if (!(LotNo_Sub is null))
                sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.SubLot, LotNo_Sub);
            if (!(Cnt_JobPlan.ToString() is null))
                sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.Cnt_JobPlan, Cnt_JobPlan);
            if (!(Reserved1 is null))
                sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.Reserved1, Reserved1);
            if (!(Reserved2 is null))
                sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.Reserved2, Reserved2);
            if (!(Reserved3 is null))
                sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.Reserved3, Reserved3);
            if (!(Reserved4 is null))
                sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.Reserved4, Reserved4);
            if (!(Reserved5 is null))
                sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.Reserved5, Reserved5);
            if (!(InsertUser is null))
                sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.InsertUser, InsertUser);
            if (!(InsertDate is null))
                sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.InsertDate, InsertDate);
            if (!(UpdateUser is null))
                sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.UpdateUser, UpdateUser);
            if (!(UpdateDate is null))
                sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.UpdateDate, UpdateDate);
            foreach (JobOrderDetail dt in this.JobOrderDetail)
            {
                if (!(dt is null))
                    sb.AppendFormat("[{0}:\r\n{1}]", LanguagePack.UserInterfaces.Detail, dt.ToSplitString());
            }
            return sb.ToString();
        }
        public bool InsertServer()
        {
            try
            {
                Controls.JobOrderController.InsertServer(new DSM.Dmn_JobOrder_M(this), true);
                foreach (JobOrderDetail dtl in this.JobOrderDetail)
                {
                    Controls.JobOrderController.InsertServer(new DSM.Dmn_JobOrder_D(dtl) { PlantCode = this.PlantCode, OrderNo = this.OrderNo, SeqNo = this.SeqNo , InsertUser = this.InsertUser, InsertDate = this.InsertDate });
                }
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrder InsertServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool UpdateServer()
        {
            try
            {
                Controls.JobOrderController.UpdateServer(new DSM.Dmn_JobOrder_M(this), true);
                Controls.JobOrderController.DeleteServerDetailAll(this.PlantCode, this.OrderNo, this.SeqNo, this.UpdateUser);
                foreach (JobOrderDetail dtl in this.JobOrderDetail)
                {
                    //Controls.JobOrderController.DeleteServerDetail(this.PlantCode, this.OrderNo, this.SeqNo, dtl.JobDetailType, this.UpdateUser);
                    Controls.JobOrderController.InsertServer(new DSM.Dmn_JobOrder_D(dtl) { PlantCode = this.PlantCode, OrderNo = this.OrderNo, SeqNo = this.SeqNo, InsertUser = this.InsertUser, InsertDate = this.InsertDate, UpdateDate = this.UpdateDate, UpdateUser = this.UpdateUser });
                }
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrder UpdateServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool UpdateDetailServer()
        {
            try
            {
                Controls.JobOrderController.UpdateServer(new DSM.Dmn_JobOrder_M(this), true);
                foreach (JobOrderDetail dtl in this.JobOrderDetail)
                {
                    Controls.JobOrderController.DeleteServerDetail(this.PlantCode, this.OrderNo, this.SeqNo, dtl.JobDetailType, this.UpdateUser);
                    Controls.JobOrderController.InsertServer(new DSM.Dmn_JobOrder_D(dtl) { PlantCode = this.PlantCode, OrderNo = this.OrderNo, SeqNo = this.SeqNo, InsertUser = this.InsertUser, InsertDate = this.InsertDate, UpdateDate = this.UpdateDate, UpdateUser = this.UpdateUser });
                }
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrder UpdateServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool UpdateDetailWithOutDateServer()
        {
            try
            {
                Controls.JobOrderController.UpdateServer(new DSM.Dmn_JobOrder_M(this), true);
                foreach (JobOrderDetail dtl in this.JobOrderDetail)
                {
                    Controls.JobOrderController.DeleteServerDetail(this.PlantCode, this.OrderNo, this.SeqNo, dtl.JobDetailType, this.UpdateUser);
                    Controls.JobOrderController.InsertServer(new DSM.Dmn_JobOrder_D(dtl) { PlantCode = this.PlantCode, OrderNo = this.OrderNo, SeqNo = this.SeqNo, InsertUser = this.InsertUser, InsertDate = this.InsertDate });
                }
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrder UpdateServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool UpdateCount()
        {
            try
            {
                Controls.JobOrderController.UpdateServer(new DSM.Dmn_JobOrder_M(this), true);
                foreach (JobOrderDetail dtl in this.JobOrderDetail)
                {
                    Controls.JobOrderController.DeleteServerDetail(this.PlantCode, this.OrderNo, this.SeqNo, dtl.JobDetailType, this.UpdateUser);
                    Controls.JobOrderController.InsertServer(new DSM.Dmn_JobOrder_D(dtl) { PlantCode = this.PlantCode, OrderNo = this.OrderNo, SeqNo = this.SeqNo, InsertUser = this.InsertUser, InsertDate = this.InsertDate });
                }
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrder UpdateServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool InsertLocalPM()
        {
            try
            {
                Controls.JobOrderController.InsertLocal(new Local.Dmn_JobOrder_M(this), true);
                foreach (JobOrderDetail dtl in this.JobOrderDetail)
                {
                    Controls.JobOrderController.InsertLocal(new Local.Dmn_JobOrder_PM(dtl) { OrderNo = this.OrderNo, SeqNo = this.SeqNo, InsertUser = this.InsertUser, InsertDate = this.InsertDate });
                }
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrder InsertLocalPM Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool InsertLocalAG()
        {
            try
            {
                Controls.JobOrderController.InsertLocal(new Local.Dmn_JobOrder_M(this), true);
                foreach (JobOrderDetail dtl in this.JobOrderDetail)
                {
                    Controls.JobOrderController.InsertLocal(new Local.Dmn_JobOrder_AG(dtl) { OrderNo = this.OrderNo, SeqNo = this.SeqNo, InsertUser = this.InsertUser, InsertDate = this.InsertDate });
                }
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrder InsertLocalAG Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool UpdateLocalPM()
        {
            try
            {
                Controls.JobOrderController.UpdateLocal(new Local.Dmn_JobOrder_M(this), true);
                foreach (JobOrderDetail dtl in this.JobOrderDetail)
                {
                    Controls.JobOrderController.DeleteLocalDetail(this.OrderNo, this.SeqNo, dtl.JobDetailType, this.UpdateUser);
                    Controls.JobOrderController.InsertLocal(new Local.Dmn_JobOrder_PM(dtl) { OrderNo = this.OrderNo, SeqNo = this.SeqNo, InsertDate = this.InsertDate, InsertUser = this.InsertUser,  UpdateDate = this.UpdateDate, UpdateUser = this.UpdateUser });
                }
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrder UpdateLocalPM Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool UpdateLocalAG()
        {
            try
            {
                Controls.JobOrderController.UpdateLocal(new Local.Dmn_JobOrder_M(this), true);
                foreach (JobOrderDetail dtl in this.JobOrderDetail)
                {
                    Controls.JobOrderController.DeleteLocalDetail(this.OrderNo, this.SeqNo, dtl.JobDetailType, this.UpdateUser);
                    Controls.JobOrderController.InsertLocal(new Local.Dmn_JobOrder_AG(dtl) { OrderNo = this.OrderNo, SeqNo = this.SeqNo, InsertDate = this.InsertDate, InsertUser = this.InsertUser, UpdateDate = this.UpdateDate, UpdateUser = this.UpdateUser });
                }
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("JobOrder UpdateLocalAG Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public JobOrder Clone()
        {
            var jobOrder =new JobOrder(this);
            for (int i = 0; i < this.JobOrderDetail.Count; i++)
                jobOrder.JobOrderDetail.Add(new JobOrderDetail(this.JobOrderDetail[i]));
            for (int i = 0; i < this.ProductDetail.Count; i++)
                jobOrder.ProductDetail.Add(new ProductDetail(this.ProductDetail[i]));
            return jobOrder;
        }
        public static JobOrder CreateTestJobPM(string prodCode, bool multiVision = false, bool usePrinterGroup1 = false, bool usePrinterGroup2 = false)
        {
            JobOrder tmp = new JobOrder();
            Product product = DominoDatabase.Controls.ProductController.SelectLocalPMSingle(prodCode, null, null, null, true);
            tmp.OrderNo = string.Format("TS{0}", DateTime.Today.ToString("yyyyMMdd"));
            tmp.SeqNo = "0000";
            tmp.PlantCode = "PlantCode";
            tmp.CorCode = "CorCode";
            tmp.ErpOrderNo = "ErpOrderNumber";
            tmp.InsertDate = DateTime.Now;
            tmp.AssignDate = DateTime.Now;
            tmp.MfdDate = DateTime.Now;
            tmp.ExpDate = DateTime.Now.AddYears(3);
            tmp.DateOfTest = DateTime.Now;
            tmp.ProdCode = prodCode;
            tmp.LotNo = "DOMINO";
            tmp.LotNo_Sub = "DOMINO";
            tmp.UseYN = "Y";
            tmp.Cnt_JobPlan = 100000;
            tmp.ProductName = string.Format("{0} {1}", product.ProdName, product.ProdName2);
            tmp.ProductStdCode = "08899123456789";
            tmp.Delay_NG = product.Delay_NG;
            tmp.Delay_Print = product.Delay_Print;
            if(multiVision)
				tmp.Delay_Print2 = product.Delay_Print2;
            tmp.Delay_Shot1 = product.Delay_Shot1;
            tmp.Delay_Shot2 = product.Delay_Shot2;
            if(multiVision)
            {
				tmp.JobOrderDetail.Add(new DominoDatabase.JobOrderDetail()
				{
					JobDetailType = product.ProductDetail.FirstOrDefault() == null ? "EA" : product.ProductDetail.FirstOrDefault().JobDetailType,
					JobStatus = "PS",
					Cnt_Good = 0,
					Cnt_Error = 0,
					Cnt_Parent = 0,
					Cnt_SerialNotUsed = 0,
					Cnt_Child = 0,
					Cnt_Sample = 0,
					Cnt_Destroy = 0,
					UserDefineData1 = "UserDefine1",
					UserDefineData2 = "UserDefine2",
					Reserved1 = "Reserved1",
					Reserved2 = "Reserved2",
					Reserved3 = "Reserved3",
					Reserved4 = "Reserved4",
					Reserved5 = "Reserved5",
					DesignID = product != null ? product.ProductDetail.FirstOrDefault().DesignID : null,
					DesignID2 = product != null? product.ProductDetail.FirstOrDefault().DesignID2 : null,
                    UsePrinterGroup1 = usePrinterGroup1,
                    UsePrinterGroup2 = usePrinterGroup2
				});
			}
            else
            {
				tmp.JobOrderDetail.Add(new DominoDatabase.JobOrderDetail()
				{
                    JobDetailType = product.ProductDetail.FirstOrDefault() == null ? "EA" : product.ProductDetail.FirstOrDefault().JobDetailType,
                    JobStatus = "PS",
					Cnt_Good = 0,
					Cnt_Error = 0,
					Cnt_Parent = 0,
					Cnt_SerialNotUsed = 0,
					Cnt_Child = 0,
					Cnt_Sample = 0,
					Cnt_Destroy = 0,
					UserDefineData1 = "UserDefine1",
					UserDefineData2 = "UserDefine2",
					Reserved1 = "Reserved1",
					Reserved2 = "Reserved2",
					Reserved3 = "Reserved3",
					Reserved4 = "Reserved4",
					Reserved5 = "Reserved5",
                    DesignID = product != null? product.ProductDetail.FirstOrDefault().DesignID : null,
                });
			}
            tmp.Reserved1 = "Reserved1";
            tmp.Reserved2 = "Reserved2";
            tmp.Reserved3 = "Reserved3";
            tmp.Reserved4 = "Reserved4";
            tmp.Reserved5 = "Reserved5";
            tmp.EnOrderType = EnJobOrderType.TestPrint;
            return tmp;
        }
        public static JobOrder CreateTestJobAG(string prodCode, string stdCode)
        {
            JobOrder tmp = new JobOrder();
            Product product = DominoDatabase.Controls.ProductController.SelectLocalAGSingle(prodCode, stdCode, null, null, true);
            tmp.OrderNo = string.Format("TS{0}", DateTime.Today.ToString("yyyyMMdd"));
            tmp.SeqNo = "0000";
            tmp.PlantCode = "PlantCode";
            tmp.CorCode = "CorCode";
            tmp.ErpOrderNo = "ErpOrderNumber";
            tmp.InsertDate = DateTime.Now;
            tmp.AssignDate = DateTime.Now;
            tmp.MfdDate = DateTime.Now;
            tmp.ExpDate = DateTime.Now.AddYears(3);
            tmp.DateOfTest = DateTime.Now;
            tmp.ProdCode = prodCode;
            tmp.LotNo = "DOMINO";
            tmp.LotNo_Sub = "DOMINO";
            tmp.UseYN = "Y";
            tmp.Cnt_JobPlan = 100000;
            tmp.ProductName = string.Format("{0} {1}", product.ProdName, product.ProdName2);
            tmp.ProductStdCode = product.ProdStdCode;
            for (int i = 0; i < product.ProductDetail.Count; i++)
            {
                string jobDetailType = product.ProductDetail[i].JobDetailType;
                tmp.JobOrderDetail.Add(new DominoDatabase.JobOrderDetail()
                {
                    JobDetailType = jobDetailType,
                    JobStatus = "PS",
                    Cnt_Good = 0,
                    Cnt_Error = 0,
                    Cnt_Parent = 0,
                    Cnt_SerialNotUsed = 0,
                    Cnt_Child = 0,
                    Cnt_Sample = 0,
                    Cnt_Destroy = 0,
                    UserDefineData1 = "UserDefine1",
                    UserDefineData2 = "UserDefine2",
                    Reserved1 = "Reserved1",
                    Reserved2 = "Reserved2",
                    Reserved3 = "Reserved3",
                    Reserved4 = "Reserved4",
                    Reserved5 = "Reserved5",
                    DesignID = product != null && string.IsNullOrEmpty(jobDetailType) ? null : product.ProductDetail[i].DesignID,
                    EnBarcodeFormatType = product.ProductDetail[i].EnBarcodeFormatType,
                    EnBarcodeType = product.ProductDetail[i].EnBarcodeType,
                    PrinterName = product.ProductDetail[i].PrinterName

                });
            }
            tmp.Reserved1 = "Reserved1";
            tmp.Reserved2 = "Reserved2";
            tmp.Reserved3 = "Reserved3";
            tmp.Reserved4 = "Reserved4";
            tmp.Reserved5 = "Reserved5";
            tmp.EnOrderType = EnJobOrderType.TestPrint;
            return tmp;
        }

        public static JobOrder CreateManualJobPM(string prodCode, bool multiVision = false, bool usePrinterGroup1 = false, bool usePrinterGroup2 = false)
        {
            JobOrder tmp = new JobOrder();
            Product product = DominoDatabase.Controls.ProductController.SelectLocalPMSingle(prodCode, null, null, null, true);
            tmp.SeqNo = "0000";
            tmp.InsertDate = DateTime.Now;
            tmp.ProdCode = prodCode;
            tmp.UseYN = "Y";
            tmp.EnOrderType = EnJobOrderType.ManualPrint;
            tmp.ManualBufferIndex = 0;
            tmp.ProductStdCode = product.FirstDetailStandardCode;
            tmp.ProductName = string.Format("{0} {1}", product.ProdName, product.ProdName2);
            tmp.Delay_NG = product.Delay_NG;
            tmp.Delay_Print = product.Delay_Print;
            if(multiVision)
				tmp.Delay_Print2 = product.Delay_Print2;
			tmp.Delay_Shot1 = product.Delay_Shot1;
            tmp.Delay_Shot2 = product.Delay_Shot2;
            if(multiVision)
            {
				tmp.JobOrderDetail.Add(new DominoDatabase.JobOrderDetail()
				{
                    JobDetailType = product.ProductDetail.FirstOrDefault() == null ? "EA" : product.ProductDetail.FirstOrDefault().JobDetailType,
                    JobStatus = "PS",
					Cnt_Good = 0,
					Cnt_Error = 0,
					Cnt_Parent = 0,
					Cnt_SerialNotUsed = 0,
					Cnt_Child = 0,
					Cnt_Sample = 0,
					Cnt_Destroy = 0,
                    DesignID = product != null ? product.ProductDetail.FirstOrDefault().DesignID : null,
                    DesignID2 = product != null ? product.ProductDetail.FirstOrDefault().DesignID2 : null,
                    UsePrinterGroup1 = usePrinterGroup1,
                    UsePrinterGroup2 = usePrinterGroup2
				});
			}
            else
            {
				tmp.JobOrderDetail.Add(new DominoDatabase.JobOrderDetail()
				{
                    JobDetailType = product.ProductDetail.FirstOrDefault() == null ? "EA" : product.ProductDetail.FirstOrDefault().JobDetailType,
                    JobStatus = "PS",
					Cnt_Good = 0,
					Cnt_Error = 0,
					Cnt_Parent = 0,
					Cnt_SerialNotUsed = 0,
					Cnt_Child = 0,
					Cnt_Sample = 0,
					Cnt_Destroy = 0,
                    DesignID = product != null ? product.ProductDetail.FirstOrDefault().DesignID : null,
                });
			}
            return tmp;
        }
        public static JobOrder CreateManualJobAG(string prodCode, string stdCode)
        {
            JobOrder tmp = new JobOrder();
            Product product = DominoDatabase.Controls.ProductController.SelectLocalAGSingle(prodCode, stdCode, null, null, true);
            tmp.SeqNo = "0000";
            tmp.InsertDate = DateTime.Now;
            tmp.ProdCode = prodCode;
            tmp.UseYN = "Y";
            tmp.EnOrderType = EnJobOrderType.ManualPrint;
            tmp.ManualBufferIndex = 0;
            tmp.ProductStdCode = product.ProdStdCode;
            tmp.ProductName = string.Format("{0} {1}", product.ProdName, product.ProdName2);
            for (int i = 0; i < product.ProductDetail.Count; i++)
            {
                tmp.JobOrderDetail.Add(new JobOrderDetail()
                {
                    JobDetailType = product.ProductDetail[i].JobDetailType,
                    JobStatus = "PS",
                    Cnt_Good = 0,
                    Cnt_Error = 0,
                    Cnt_Parent = 0,
                    Cnt_SerialNotUsed = 0,
                    Cnt_Child = 0,
                    Cnt_Sample = 0,
                    Cnt_Destroy = 0,
                    DesignID = product != null && product.ProductDetail[i] != null ? product.ProductDetail[i].DesignID : null,
                    PrinterName = product.ProductDetail[i].PrinterName,
                    EnBarcodeFormatType = product.ProductDetail[i].EnBarcodeFormatType,
                    EnBarcodeType = product.ProductDetail[i].EnBarcodeType,
                });
            }
            return tmp;
        }
        public static JobOrder CreateManualJobAG(JobOrder jobOrder)
        {
            JobOrder tmp = jobOrder.Clone();
            tmp.EnOrderType = EnJobOrderType.ManualPrint;
            return tmp;
        }
        public string GetPrintValue(EnVarKey key, string jobDetail)
        {
            switch (key)
            {
                case EnVarKey.StandardCode:
                case EnVarKey.oStandardCode:
                case EnVarKey.SSCC:
                case EnVarKey.oSSCC:
                    return this.FirstDetailStandardCode;
                case EnVarKey.AiSSCC:
                case EnVarKey.AiStandardCode:
                    return string.Format("({0}){1}", DominoGSLib.GS1.AI_GTIN, this.FirstDetailStandardCode);
                case EnVarKey.LotNumber:
                case EnVarKey.oLotNumber:
                    return LotNo;
                case EnVarKey.AiLotNumber:
                    return string.Format("({0}){1}", DominoGSLib.GS1.AI_LotNo, LotNo);
                case EnVarKey.Capacity:
                    return this.JobOrderDetail[jobDetail].Capacity;
                case EnVarKey.ProductName1:
                    return ProdName;
                case EnVarKey.ProductName2:
                    return ProdName2;
                case EnVarKey.UserDefine1:
                    return JobOrderDetail[jobDetail].UserDefineData1;
                case EnVarKey.UserDefine2:
                    return JobOrderDetail[jobDetail].UserDefineData2;
                case EnVarKey.SubLot:
                    return LotNo_Sub;
                case EnVarKey.SerialNumber:
                case EnVarKey.oSerialNumber:
                    return "0123456789";
                case EnVarKey.AiSerialNumber:
                    return string.Format("({0}){1}", DominoGSLib.GS1.AI_SerialNumber, "0123456789");
                case EnVarKey.Extension:
                    return this.JobOrderDetail[jobDetail].GS1ExtensionCode;
                case EnVarKey.oSerialNumber1:
                case EnVarKey.oSerialNumber2:
                    return "000000000000000000";
                case EnVarKey.CompanyInternalInfo1:
                case EnVarKey.CompanyInternalInfo2:
                case EnVarKey.CompanyInternalInfo3:
                    return "0000";
                case EnVarKey.SalesPrice:
                    return JobOrderDetail[jobDetail].Price;
                case EnVarKey.Reserved1:
                    return "Reserved1";
                case EnVarKey.oCounter:
                    return "1";
                case EnVarKey.oNumber:
                    return "0001";
                case EnVarKey.Weight:
                    return "100.0 g";
                default:
                    throw new NotImplementedException();
            }
        }
        public string GetPrintValue(string key, ref int VariableIndex)
        {
            if (!string.IsNullOrEmpty(JobOrderDetail[0].PrinterVariable1) && VariableIndex == 0)
            {
                VariableIndex++;
                return JobOrderDetail[0].PrinterVariable1;
            }
            if (!string.IsNullOrEmpty(JobOrderDetail[0].PrinterVariable2) && VariableIndex == 1)
            {
                VariableIndex++;
                return JobOrderDetail[0].PrinterVariable2;
            }
            if (!string.IsNullOrEmpty(JobOrderDetail[0].PrinterVariable3) && VariableIndex == 2)
            {
                VariableIndex++;
                return JobOrderDetail[0].PrinterVariable3;
            }
            if (!string.IsNullOrEmpty(JobOrderDetail[0].PrinterVariable4) && VariableIndex == 3)
            {
                VariableIndex++;
                return JobOrderDetail[0].PrinterVariable4;
            }
            if (!string.IsNullOrEmpty(JobOrderDetail[0].PrinterVariable5) && VariableIndex == 4)
            {
                VariableIndex++;
                return JobOrderDetail[0].PrinterVariable5;
            }
            return string.Empty;
        }
        public string GetReserved(int VariableIndex)
        {
            switch(VariableIndex)
            {
                case 0:
                    return JobOrderDetail[0].PrinterVariable1;
                case 1:
                    return JobOrderDetail[0].PrinterVariable2;
                case 2:
                    return JobOrderDetail[0].PrinterVariable3;
                case 3:
                    return JobOrderDetail[0].PrinterVariable4;
                case 4:
                    return JobOrderDetail[0].PrinterVariable5;
                default:
                    return string.Empty;
            }
        }

        public void UpdateSerialIndex()
        {
            foreach (JobOrderDetail dtl in this.JobOrderDetail)
            {
                if (string.IsNullOrEmpty(dtl.SnExpressionStr))
                    continue;
                DominoFunctions.SerialGenerator express = new DominoFunctions.SerialGenerator(dtl.SnExpressionID, dtl.SnExpressionStr);
                if (dtl.JobDetailType == "LBL" || dtl.JobDetailType == "EA")
                {
                    if (express.FixedTexts.Any(q => q.Format == EnSerialNumberExpressionFixedFormat.LotNumber))
                    {
                        dtl.Cnt_SNLast = Controls.JobOrderController.GetLastSerialPM(this.FirstDetailStandardCode, LotNo);
                        dtl.Cnt_SNPrintLast = Controls.JobOrderController.GetLastPrintedSerialPM(this.FirstDetailStandardCode, LotNo);
                    }
                    else if (express.FixedTexts.Any(q => q.Format == EnSerialNumberExpressionFixedFormat.ManufactureDateYY || q.Format == EnSerialNumberExpressionFixedFormat.ManufactureDateYYYY ||
                        q.Format == EnSerialNumberExpressionFixedFormat.ManufactureDateMMM || q.Format == EnSerialNumberExpressionFixedFormat.ManufactureDateMM || q.Format == EnSerialNumberExpressionFixedFormat.ManufactureDateDD))
                    {
                        DateTime startDateTime = new DateTime(1900, 1, 1, 0, 0, 0);
                        DateTime endDatetime = new DateTime(2999, 12, 31, 23, 59, 59, 999);
                        if (express.FixedTexts.Any(q => q.Format == EnSerialNumberExpressionFixedFormat.ManufactureDateYY || q.Format == EnSerialNumberExpressionFixedFormat.ManufactureDateYYYY))
                        {
                            startDateTime = new DateTime(MfdDate.Value.Year, startDateTime.Month, startDateTime.Day, 0, 0, 0);
                            endDatetime = new DateTime(MfdDate.Value.Year, endDatetime.Month, endDatetime.Day, 23, 59, 59, 999);
                        }
                        if(express.FixedTexts.Any(q => q.Format == EnSerialNumberExpressionFixedFormat.ManufactureDateMMM || q.Format == EnSerialNumberExpressionFixedFormat.ManufactureDateMM))
                        {
                            int endDay = DateTime.DaysInMonth(endDatetime.Year, MfdDate.Value.Month);
                            startDateTime = new DateTime(startDateTime.Year, MfdDate.Value.Month, startDateTime.Day, 0, 0, 0);
                            endDatetime = new DateTime(endDatetime.Year, MfdDate.Value.Month, endDay, 23, 59, 59, 999);
                        }
                        if (express.FixedTexts.Any(q => q.Format == EnSerialNumberExpressionFixedFormat.ManufactureDateDD))
                        {
                            startDateTime = new DateTime(startDateTime.Year, startDateTime.Month, MfdDate.Value.Day, 0, 0, 0);
                            endDatetime = new DateTime(endDatetime.Year, endDatetime.Month, MfdDate.Value.Day, 23, 59, 59, 999);
                        }
                        dtl.Cnt_SNLast = Controls.JobOrderController.GetLastSerialPM(this.FirstDetailStandardCode, null, startDateTime, endDatetime);
                        dtl.Cnt_SNPrintLast = Controls.JobOrderController.GetLastPrintedSerialPM(this.FirstDetailStandardCode, null, startDateTime, endDatetime);
                    }
                    else if(express.FixedTexts.Any(q => q.Format == EnSerialNumberExpressionFixedFormat.ExpiryDateYY || q.Format == EnSerialNumberExpressionFixedFormat.ExpiryDateYYYY ||
                        q.Format == EnSerialNumberExpressionFixedFormat.ExpiryDateMMM || q.Format == EnSerialNumberExpressionFixedFormat.ExpiryDateMM || q.Format == EnSerialNumberExpressionFixedFormat.ExpiryDateDD))
                    {
                        DateTime startDateTime = new DateTime(1900, 1, 1, 0, 0, 0);
                        DateTime endDatetime = new DateTime(2999, 12, 31, 23, 59, 59, 999);
                        if (express.FixedTexts.Any(q => q.Format == EnSerialNumberExpressionFixedFormat.ExpiryDateYY || q.Format == EnSerialNumberExpressionFixedFormat.ExpiryDateYYYY))
                        {
                            startDateTime = new DateTime(ExpDate.Value.Year, startDateTime.Month, startDateTime.Day, 0, 0, 0);
                            endDatetime = new DateTime(ExpDate.Value.Year, endDatetime.Month, endDatetime.Day, 23, 59, 59, 999);
                        }
                        if (express.FixedTexts.Any(q => q.Format == EnSerialNumberExpressionFixedFormat.ExpiryDateMMM || q.Format == EnSerialNumberExpressionFixedFormat.ExpiryDateMM))
                        {
                            int endDay = DateTime.DaysInMonth(ExpDate.Value.Year, ExpDate.Value.Month);
                            startDateTime = new DateTime(startDateTime.Year, ExpDate.Value.Month, startDateTime.Day, 0, 0, 0);
                            endDatetime = new DateTime(endDatetime.Year, ExpDate.Value.Month, endDay, 23, 59, 59, 999);
                        }
                        if (express.FixedTexts.Any(q => q.Format == EnSerialNumberExpressionFixedFormat.ExpiryDateDD))
                        {
                            startDateTime = new DateTime(startDateTime.Year, startDateTime.Month, ExpDate.Value.Day, 0, 0, 0);
                            endDatetime = new DateTime(endDatetime.Year, endDatetime.Month, ExpDate.Value.Day, 23, 59, 59, 999);
                        }
                        dtl.Cnt_SNLast = Controls.JobOrderController.GetLastSerialPM(this.FirstDetailStandardCode, null, null, null, startDateTime, endDatetime);
                        dtl.Cnt_SNPrintLast = Controls.JobOrderController.GetLastPrintedSerialPM(this.FirstDetailStandardCode, null, null, null, startDateTime, endDatetime);
                    }
                    else
                    {
                        dtl.Cnt_SNLast = Controls.JobOrderController.GetLastSerialPM(this.FirstDetailStandardCode);
                        dtl.Cnt_SNPrintLast = Controls.JobOrderController.GetLastPrintedSerialPM(this.FirstDetailStandardCode);
                    }
                }
                else
                {
                    switch (dtl.EnBarcodeFormatType)
                    {
                        case EnBarcodeFormatType.GS1:
                        case EnBarcodeFormatType.GS1andSSCC:
                            {
                                dtl.Cnt_SNPrintLast = dtl.Cnt_SNLast = DominoDatabase.Controls.JobOrderController.GetLastSerialAG(this.ProductStdCode, dtl.JobDetailType, LotNo);
                            }
                            break;
                        case EnBarcodeFormatType.SSCC:
                            {
                                dtl.Cnt_SNPrintLast = dtl.Cnt_SNLast = DominoDatabase.Controls.JobOrderController.GetLastSSCCAG(dtl.Prefix_SSCC);
                            }
                            break;
                    }
                }
            }
        }
        public void GetProductName()
        {
            this.ProductName = Controls.ProductController.GetProductNameByCode(this.ProdCode);
        }
    }
    [Serializable]
    public class JobOrderDetailCollection : ICollection<JobOrderDetail>
    {
        public JobOrderDetailCollection(List<JobOrderDetail> other)
        {
            _item = new List<JobOrderDetail>(other);
        }
        public JobOrderDetailCollection()
        {
            _item = new List<JobOrderDetail>();
        }
        public JobOrderDetail this[int index]
        {
            get
            {
                return _item[index];
            }
            set
            {
                _item[index] = value;
            }
        }
        public JobOrderDetail this[string jobDetail]
        {
            get
            {
                return _item.Where(x => x.JobDetailType == jobDetail).FirstOrDefault();
            }
            set
            {
                var tmp = _item.Where(x => x.JobDetailType == jobDetail).FirstOrDefault();
                tmp = value;
            }
        }
        public JobOrderDetail this[PackingType jobDetail]
        {
            get
            {
                return _item.Where(x => x.JobDetailType == jobDetail.ToString()).FirstOrDefault();
            }
            set
            {
                var tmp = _item.Where(x => x.JobDetailType == jobDetail.ToString()).FirstOrDefault();
                tmp = value;
            }
        }
        public EnJobStatus GetStatus()
        {
            try
            {
                if((EnJobStatus)_item.Max(q => (int)q.EnStatus) > EnJobStatus.ProductionComplete)
                    return (EnJobStatus)_item.Min(q => (int)q.EnStatus);
                return (EnJobStatus)_item.Max(q => (int)q.EnStatus);
            }
            catch
            {
                return EnJobStatus.InitialState;
            }
        }
        public EnJobStatus GetStatus(string jobDetailType)
        {
            try
            {
                return _item.Where(x => x.JobDetailType == jobDetailType).FirstOrDefault().EnStatus;
            }
            catch
            {
                return EnJobStatus.InitialState;
            }
        }
        public bool SetStatus(EnJobStatus status)
        {
            try
            {
                for(int i = 0; i < _item.Count; i++)
                {
                    if(_item[i].EnStatus < status)
                        _item[i].EnStatus = status;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool SetStatus(string jobDetailType, EnJobStatus status)
        {
            try
            {
                _item.Where(x => x.JobDetailType == jobDetailType).FirstOrDefault().EnStatus = status;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void Add(JobOrderDetail detail)
        {
            _item.Add(detail);
        }
        public void AddRange(IEnumerable<JobOrderDetail> list)
        {
            _item.AddRange(list);
        }
        public void RemoveAt(int index)
        {
            _item.RemoveAt(index);
        }
        public bool Remove(JobOrderDetail detail)
        {
            return _item.Remove(detail);
        }
        public void RemoveRange(int index, int count)
        {
            _item.RemoveRange(index, count);
        }

        protected readonly List<JobOrderDetail> _item;
        public IEnumerator<JobOrderDetail> GetEnumerator()
        {
            return _item.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        public void Clear()
        {
            _item.Clear();
        }
        public bool Contains(JobOrderDetail detail)
        {
            return _item.Contains(detail);
        }
        public void CopyTo(JobOrderDetail[] array, int arrayIndex)
        {
            _item.CopyTo(array, arrayIndex);
        }
        public int Count
        {
            get
            {
                return _item.Count;
            }
        }
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }
        public List<PackingType> DetailTypeList
        {
            get
            {
                List<PackingType> list = new List<PackingType>();
                foreach (var detail in _item)
                {
                    list.Add((PackingType)Enum.Parse(typeof(PackingType), detail.JobDetailType));
                }
                return list;
            }
        }
        public bool ContainsJobDetail(string jobDetail)
        {
            return _item.Any(q => q.JobDetailType == jobDetail);
        }
        public bool ContainsJobDetail(PackingType packingType)
        {
            return _item.Any(q => q.JobDetailType == packingType.ToString());
        }
        public int IndexOf(string packingType)
        {
            return _item.FindIndex(q => q.JobDetailType == packingType);
        }
        public int IndexOfStdCode(string stdCode)
        {
            return _item.FindIndex(q => q.StandardCode == stdCode);
        }
        public JobOrderDetail GetDetailByStdCode(string stdCode)
        {
            return _item.FirstOrDefault(q => q.StandardCode == stdCode);
        }
        public JobOrderDetailCollection Clone()
        {
            JobOrderDetailCollection retVal = new JobOrderDetailCollection();
            foreach(JobOrderDetail dt in this)
            {
                retVal.Add(dt.Clone());
            }
            return retVal;
        }
    }
    [Serializable]
    public class JobOrderDetail
    {
        public string JobDetailType { get; set; }
        public string JobStatus { get; set; }
        public int Cnt_Good { get; set; }
        public int Cnt_Error { get; set; }
        public int Cnt_Sample { get; set; }
        public int Cnt_Destroy { get; set; }
        public int Cnt_Child { get; set; }
        public int Cnt_Work { get; set; }
        public int Cnt_Parent { get; set; }
        public int Cnt_HelpCodeLast { get; set; }
        public int Cnt_SNLast { get; set; }
        public int Cnt_SNPrintLast { get; set; }
        public int Cnt_SN_Movil { get; set; }
        public int Cnt_SN_DSM { get; set; }
        public int Cnt_SN_Lot_O { get; set; }
        public int Cnt_SN_Lot_X { get; set; }
        public int Cnt_Status1 { get; set; }
        public int Cnt_Status2 { get; set; }
        public int Cnt_Status3 { get; set; }
        public int Cnt_Status4 { get; set; }
        public int Cnt_Status5 { get; set; }
        public string UserDefineData1 { get; set; }
        public string UserDefineData2 { get; set; }
        public string PrinterVariable1 { get; set; }
        public string PrinterVariable2 { get; set; }
        public string PrinterVariable3 { get; set; }
        public string PrinterVariable4 { get; set; }
        public string PrinterVariable5 { get; set; }
        public string Reserved1 { get; set; }
        public string Reserved2 { get; set; }
        public string Reserved3 { get; set; }
        public string Reserved4 { get; set; }
        public string Reserved5 { get; set; }
        public DateTime? StartDate { get; set; }
        public string StartUser { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string CompleteUser { get; set; }
        public string SnExpressionID { get; set; }
        public string SnExpressionStr { get; set; }
        public int Cnt_SerialTotal { get; set; }
        public int Cnt_SerialNotUsed { get; set; }
        public string SnType { get; set; }
        public string DesignID { get; set; }
        public string DesignID2 { get; set; }
        public bool? UsePrinterGroup1 { get; set; }
		public bool? UsePrinterGroup2 { get; set; }
		public string BarcodeDataFormat { get; set; }
        public string BarcodeType { get; set; }
        public string MachineID { get; set; }
        public string Prefix_SSCC { get; set; }
        public string Capacity { get; set; }
        public string ProdStdCodeChild { get; set; }
        public string GS1ExtensionCode { get; set; }
        public int? ContentCount{ get; set; }
        public int? LabelPrintCount { get; set; }
        public int? PackingCount { get; set; }
        public string PrinterName { get; set; }
        public string PrinterName2 { get; set; }
        public string ResourceType { get; set; }
        public string Price { get; set; }
        public string ProductReserved1 { get; set; }
        public string ProductReserved2 { get; set; }
        public string ProductReserved3 { get; set; }
        public string PharmaCode { get; set; }
        public string ProdStdCode { get; set; }
        public int? ExtractSerialStart { get; set; }
        public int? ExtractSerialEnd { get; set; }
        public decimal? MinimumWeight { get; set; }
        public decimal? MaximumWeight { get; set; }
        public string CustomBarcodeFormat { get; set; }
        [JsonIgnore]
        public EnJobStatus EnStatus
        {
            get
            {
                switch (JobStatus)
                {
                    case "IS":
                        return EnJobStatus.InitialState;
                    case "AS":
                        return EnJobStatus.Assign;
                    case "AP":
                        return EnJobStatus.Applied;
                    case "PS":
                        return EnJobStatus.Pause;
                    case "RU":
                        return EnJobStatus.Run;
                    case "PC":
                        return EnJobStatus.ProductionComplete;
                    case "RP":
                        return EnJobStatus.ReportComplete;
                    case "AC":
                        return EnJobStatus.AssembleComplete;
                    case "DC":
                        return EnJobStatus.DSM_ReportComplete;
                    case "CC":
                        return EnJobStatus.Cancel;
                    default:
                        throw new NotImplementedException();
                }
            }
            set
            {
                switch (value)
                {
                    case EnJobStatus.InitialState:
                        JobStatus = "IS";
                        break;
                    case EnJobStatus.Assign:
                        JobStatus = "AS";
                        break;
                    case EnJobStatus.Applied:
                        JobStatus = "AP";
                        break;
                    case EnJobStatus.Pause:
                        JobStatus = "PS";
                        break;
                    case EnJobStatus.Run:
                        JobStatus = "RU";
                        break;
                    case EnJobStatus.ProductionComplete:
                        JobStatus = "PC";
                        break;
                    case EnJobStatus.ReportComplete:
                        JobStatus = "RP";
                        break;
                    case EnJobStatus.AssembleComplete:
                        JobStatus = "AC";
                        break;
                    case EnJobStatus.DSM_ReportComplete:
                        JobStatus = "DC";
                        break;
                    case EnJobStatus.Cancel:
                        JobStatus = "CC";
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }
        [JsonIgnore]
        public int Cnt_SerialUsed
        {
            get { return Cnt_SerialTotal - Cnt_SerialNotUsed; }
        }
        [JsonIgnore]
        public int TotalCount
        {
            get
            {
                return Cnt_Good + Cnt_Error + Cnt_Destroy + Cnt_Sample;
            }
        }
        [JsonIgnore]
        public EnSerialType EnSerialType
        {
            get
            {
                if (string.IsNullOrEmpty(SnType))
                    return EnSerialType.None;
                switch (SnType)
                {
                    case "N":
                        return EnSerialType.None;
                    case "C":
                        return EnSerialType.CreateSerialNumber;
                    case "R":
                        return EnSerialType.ReceivedSerialNumber;
                    //case "S":
                    //    return EnSerialType.ScanSerialNumber;
                    case "D":
                        return EnSerialType.DSM;
                    case "M":
                        return EnSerialType.Movilitas;
                    case "I":
                        return EnSerialType.Interface;
                    case "H":
                        return EnSerialType.HelpCode;
                    //case "L":
                    //    return EnSerialType.RecievedSerialNumber_Lot_X;
                }
                throw new NotImplementedException();
            }
            set
            {
                switch (value)
                {
                    case EnSerialType.None:
                        SnType = "N";
                        break;
                    case EnSerialType.CreateSerialNumber:
                        SnType = "C";
                        break;
                    case EnSerialType.ReceivedSerialNumber:
                        SnType = "R";
                        break;
                    //case EnSerialType.RecievedSerialNumber_Lot_X:
                    //    SnType = "L";
                    //    break;
                    //case EnSerialType.ScanSerialNumber:
                    //    SnType = "S";
                    //    break;
                    case EnSerialType.DSM:
                        SnType = "D";
                        break;
                    case EnSerialType.Movilitas:
                        SnType = "M";
                        break;
                    case EnSerialType.Interface:
                        SnType = "I";
                        break;
                    case EnSerialType.HelpCode:
                        SnType = "H";
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }
        [JsonIgnore]
        public EnBarcodeFormatType EnBarcodeFormatType
        {
            get
            {
                if (Enum.TryParse(BarcodeDataFormat, out EnBarcodeFormatType tmp))
                    return tmp;
                else
                    return EnBarcodeFormatType.CustomBarcode;
            }
            set
            {
                BarcodeDataFormat = value.ToString();
            }
        }
        [JsonIgnore]
        public EnBarcodeType EnBarcodeType
        {
            get
            {
                return (EnBarcodeType)Enum.Parse(typeof(EnBarcodeType), BarcodeType);
            }
            set
            {
                BarcodeType = value.ToString();
            }
        }
        [JsonIgnore]
        // 시리얼 획득방식
        public EnResourceType EnResourceType
        {
            get
            {
                switch (ResourceType)
                {
                    case "L":
                        return EnResourceType.Local;
                    case "D":
                        return EnResourceType.DSM;
                    case "E":
                        return EnResourceType.ERP;
                    case "F":
                        return EnResourceType.File;
                    case "M":
                        return EnResourceType.Movilita;
                    case "K":
                        return EnResourceType.KEIDAS;
                    case "I":
                        return EnResourceType.Interface;
                    default:
                        return EnResourceType.Local;
                }
            }
            set
            {
                switch (value)
                {
                    case EnResourceType.Local:
                        ResourceType = "L";
                        break;
                    case EnResourceType.DSM:
                        ResourceType = "D";
                        break;
                    case EnResourceType.ERP:
                        ResourceType = "E";
                        break;
                    case EnResourceType.File:
                        ResourceType = "F";
                        break;
                    case EnResourceType.Movilita:
                        ResourceType = "M";
                        break;
                    case EnResourceType.KEIDAS:
                        ResourceType = "K";
                        break;
                    case EnResourceType.Interface:
                        ResourceType = "I";
                        break;
                    default:
                        ResourceType = "L";
                        break;
                }
            }
        }

        public int ReservedPrinterVariableCount
        {
            get
            {
                return Convert.ToInt32(!string.IsNullOrEmpty(PrinterVariable1)) + Convert.ToInt32(!string.IsNullOrEmpty(PrinterVariable2)) +
                     Convert.ToInt32(!string.IsNullOrEmpty(PrinterVariable3)) + Convert.ToInt32(!string.IsNullOrEmpty(PrinterVariable4)) +
                     Convert.ToInt32(!string.IsNullOrEmpty(PrinterVariable5));
            }
        }
        [JsonIgnore]
        public string StandardCode
        {
            get
            {
                if (!string.IsNullOrEmpty(ProdStdCode))
                    return ProdStdCode;
                else if (string.IsNullOrEmpty(ProdStdCodeChild))
                    return string.Empty;
                else
                {
                    string prefix = GS1ExtensionCode + ProdStdCodeChild.Substring(1, ProdStdCodeChild.Length - 2);
                    int WorkCheckDigit = DominoGSLib.GS1.GetCheckDigit(prefix);
                    return prefix + WorkCheckDigit.ToString();
                }
            }
        }
        [JsonIgnore]
        public PackingType PackingType
        {
            get { return (PackingType)Enum.Parse(typeof(PackingType), JobDetailType); }
        }
        public JobOrderDetail Clone()
        {
            return (JobOrderDetail)this.MemberwiseClone();
        }
        public JobOrderDetail()
        {
        }
        public JobOrderDetail(object list)
        {
            this.UnionClass(list);
        }
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.JobDetailType, JobDetailType);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Status, JobStatus);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.PassCount, Cnt_Good);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.FailCount, Cnt_Error);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.SampleCount, Cnt_Sample);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.DestroyCount, Cnt_Destroy);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.WorkCount, Cnt_Work);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.PrinterVariable1, PrinterVariable1);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.PrinterVariable2, PrinterVariable2);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.PrinterVariable3, PrinterVariable3);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.PrinterVariable4, PrinterVariable4);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.PrinterVariable5, PrinterVariable5);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.UserDefine1, UserDefineData1);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.UserDefine2, UserDefineData2);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.StartDate, StartDate);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.StartUser, StartUser);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.CompleteDate, CompleteDate);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.CompleteUser, CompleteUser);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved1, Reserved1);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved2, Reserved2);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved3, Reserved3);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved4, Reserved4);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved5, Reserved5);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.ProdStdCode, ProdStdCode);
            return sb.ToString();
        }
        public string ToSplitString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.DesignID, DesignID);
            sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.JobDetailType, JobDetailType);
            sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.Status, LanguagePack.LanguagePack.LanguagePackFromUserInterface(EnStatus.ToString()));
            sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.PassCount, Cnt_Good);
            sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.FailCount, Cnt_Error);
            sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.WorkCount, TotalCount);
            sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.UserDefine1, UserDefineData1);
            sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.UserDefine2, UserDefineData2);
            sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.Cnt_SNLast, Cnt_SNLast);
            sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.Cnt_SNPrintLast, Cnt_SNPrintLast);
            sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.StartDate, StartDate);
            sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.StartUser, StartUser);
            sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.CompleteDate, CompleteDate);
            sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.CompleteUser, CompleteUser);
            return sb.ToString();
        }
    }
}
