using System;
using DominoFunctions.ExtensionMethod;
using log4net;
using DominoFunctions.Enums;
using System.Linq;
using Newtonsoft.Json;


namespace DominoDatabase
{
    [Serializable]
    public class SerialPool
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string PlantCode { get; set; }
        public string ProdStdCode { get; set; }
        public string SerialNum { get; set; }
        public string JobDetailType { get; set; }
        public int Duplicate_Cnt { get; set; }
        public string MachineID { get; set; }
        public string OrderNo { get; set; }
        public string SeqNo { get; set; }
        public string ResourceType { get; set; }
        public string BarcodeDataFormat { get; set; }
        public string BarcodeType { get; set; }
        public string SerialType { get; set; }
        public long idx_Group { get; set; }
        public long idx_Insert { get; set; }
        public DateTime? UseDate { get; set; }
        public DateTime? InspectedDate { get; set; }
        public string UseYN { get; set; }
        public string Status { get; set; }
        public string FileName { get; set; }
        public string Weight { get; set; }
        [JsonIgnore]
        public string ConfirmedYN { get; set; }
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
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? AssignDate { get; set; }
        [JsonIgnore]
        public EnProductStatus EnStatus
        {
            get
            {
                switch (Status)
                {
                    case "PA":
                        return EnProductStatus.Pass;
                    case "RE":
                        return EnProductStatus.Reject;
                    case "NU":
                        return EnProductStatus.Notused;
                    case "OW":
                        return EnProductStatus.OverWeight;
                    case "UW":
                        return EnProductStatus.UnderWeight;
                    case "SN":
                        return EnProductStatus.Sample_Normal;
                    case "ST":
                        return EnProductStatus.Sample_Test;
                    case "SS":
                        return EnProductStatus.Sample_Storage;
                    case "SC":
                        return EnProductStatus.Sample_QC;
                    case "SA":
                        return EnProductStatus.Sample_QA;
                    case "NF":
                        return EnProductStatus.NotForSale;
                    case "DE":
                        return EnProductStatus.Destroy;
                    case "CC":
                        return EnProductStatus.Cancel;
                    default:
                        throw new NotImplementedException();
                }
            }
            set
            {
                switch (value)
                {
                    case EnProductStatus.Pass:
                        Status = "PA";
                        break;
                    case EnProductStatus.Reject:
                        Status = "RE";
                        break;
                    case EnProductStatus.Notused:
                        Status = "NU";
                        break;
                    case EnProductStatus.OverWeight:
                        Status = "OW";
                        break;
                    case EnProductStatus.UnderWeight:
                        Status = "UW";
                        break;
                    case EnProductStatus.Sample_Normal:
                        Status = "SN";
                        break;
                    case EnProductStatus.Sample_Test:
                        Status = "ST";
                        break;
                    case EnProductStatus.Sample_Storage:
                        Status = "SS";
                        break;
                    case EnProductStatus.Sample_QC:
                        Status = "SC";
                        break;
                    case EnProductStatus.Sample_QA:
                        Status = "SA";
                        break;
                    case EnProductStatus.NotForSale:
                        Status = "NF";
                        break;
                    case EnProductStatus.Destroy:
                        Status = "DE";
                        break;
                    case EnProductStatus.Cancel:
                        Status = "CC";
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
                return (EnBarcodeFormatType)Enum.Parse(typeof(EnBarcodeFormatType), BarcodeDataFormat);
            }
            set
            {
                BarcodeDataFormat = value.ToString();
            }
        }
        public SerialPool()
        {

        }
        public SerialPool(object list)
            :this()
        {
            this.UnionClass(list);
        }
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.ProdStdCode, ProdStdCode);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.SerialNum, SerialNum);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.JobDetailType, JobDetailType);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.OrderNo, OrderNo);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.SeqNo, SeqNo);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.ResourceType, ResourceType);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.BarcodeDataFormat, BarcodeDataFormat);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.BarcodeType, BarcodeType);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.SerialType, SerialType);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Idx_Group, idx_Group);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Idx_Insert, idx_Insert);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.UseDate, UseDate);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.InspectedDate, InspectedDate);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Status, Status);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.FileName, FileName);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Weight, Weight);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved1, Reserved1);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved2, Reserved2);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved3, Reserved3);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved4, Reserved4);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved5, Reserved5);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.InsertUser, InsertUser);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.InsertDate, InsertDate);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.UpdateUser, UpdateUser);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.UpdateDate, UpdateDate);
            return sb.ToString();
        }
        public bool InsertServer()
        {
            try
            {
                Controls.SerialpoolController.InsertServer(new DSM.Dmn_SerialPool(this), true);
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("Serialpool InsertServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool UpdateServer()
        {
            try
            {
                Controls.SerialpoolController.UpdateServer(new DSM.Dmn_SerialPool(this), true);
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("Serialpool UpdateServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool InsertLocal()
        {
            try
            {
                Controls.SerialpoolController.InsertLocal(new Local.Dmn_SerialPool(this), true);
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("Serialpool InsertLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool IsExist()
        {
            try
            {
                Local.Dmn_SerialPool serialPool = new Local.Dmn_SerialPool(this);
                return Controls.SerialpoolController.CheckExistQuery(serialPool.ProdStdCode,serialPool.SerialNum, true);
            }
            catch (Exception ex)
            {
                log.InfoFormat("Serialpool InsertLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool UpdateLocal()
        {
            try
            {
                Controls.SerialpoolController.UpdateLocal(new Local.Dmn_SerialPool(this), true);
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("Serialpool UpdateLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool DeleteLocal(string userID)
        {
            try
            {
                Controls.SerialpoolController.DeleteLocalSingle(ProdStdCode, SerialNum, userID, true);
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("Serialpool UpdateLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }

        public static string ProductStatusTostring(EnProductStatus status)
        {
            switch (status)
            {
                case EnProductStatus.Pass:
                    return "PA";
                case EnProductStatus.Reject:
                    return "RE";
                case EnProductStatus.Notused:
                    return "NU";
                case EnProductStatus.OverWeight:
                    return "OW";
                case EnProductStatus.UnderWeight:
                    return "UW";
                case EnProductStatus.Sample_Normal:
                    return "SN";
                case EnProductStatus.Sample_Test:
                    return "ST";
                case EnProductStatus.Sample_Storage:
                    return "SS";
                case EnProductStatus.Sample_QC:
                    return "SC";
                case EnProductStatus.Sample_QA:
                    return "SA";
                case EnProductStatus.NotForSale:
                    return "NF";
                case EnProductStatus.Destroy:
                    return "DE";
                case EnProductStatus.Cancel:
                    return "CC";
                default:
                    throw new NotImplementedException();
            }
        }
        public static EnProductStatus ProductStatusFromstring(string status)
        {
            switch (status)
            {
                case "PA":
                    return EnProductStatus.Pass;
                case "RE":
                    return EnProductStatus.Reject;
                case "NU":
                    return EnProductStatus.Notused;
                case "OW":
                    return EnProductStatus.OverWeight;
                case "UW":
                    return EnProductStatus.UnderWeight;
                case "SN":
                    return EnProductStatus.Sample_Normal;
                case "ST":
                    return EnProductStatus.Sample_Test;
                case "SS":
                    return EnProductStatus.Sample_Storage;
                case "SC":
                    return EnProductStatus.Sample_QC;
                case "SA":
                    return EnProductStatus.Sample_QA;
                case "NF":
                    return EnProductStatus.NotForSale;
                case "DE":
                    return EnProductStatus.Destroy;
                case "CC":
                    return EnProductStatus.Cancel;
                default:
                    throw new NotImplementedException();
            }
        }
        public static EnResourceType ResourceTypeFromstring(string type)
        {
            switch (type)
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
                    throw new NotImplementedException();
            }
        }
        public static string ResourceTypeTostring(EnResourceType type)
        {
            switch (type)
            {
                case EnResourceType.Local:
                    return "L";
                case EnResourceType.DSM:
                    return "D";
                case EnResourceType.ERP:
                    return "E";
                case EnResourceType.File:
                    return "F";
                case EnResourceType.Movilita:
                    return "M";
                case EnResourceType.KEIDAS:
                    return "K";
                case EnResourceType.Interface:
                    return "I";
                default:
                    throw new NotImplementedException();
            }
        }

        public SerialPool Clone()
        {
            return (SerialPool)this.MemberwiseClone();
        }
        public static bool InsertSingle(JobOrder order, JobOrderDetail dtl, Product prod, string serial, User user, EnResourceType resourceType)
        {
            try
            {
                Local.Dmn_SerialPool tmp = new Local.Dmn_SerialPool()
                {
                    ProdStdCode = order.ProductStdCode,
                    SerialNum = serial,
                    JobDetailType = dtl.JobDetailType,
                    OrderNo = order.OrderNo,
                    SeqNo = order.SeqNo,
                    ResourceType = ResourceTypeTostring(resourceType),
                    SerialType = prod.ProductDetail.Single(q => q.JobDetailType.Equals(dtl.JobDetailType)).SnType,
                    Status = "RE",
                    InsertUser = user.UserID,
                    InsertDate = DateTime.Now,
                };
                if(resourceType == EnResourceType.Local)
                {
                    tmp.UseYN = "Y";
                    tmp.UseDate = DateTime.Now;
                }

                DominoDatabase.Controls.SerialpoolController.InsertLocal(tmp, true);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
