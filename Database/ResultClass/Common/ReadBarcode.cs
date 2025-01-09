using System;
using System.Collections.Generic;
using DominoFunctions.ExtensionMethod;
using log4net;
using DominoFunctions.Enums;
using Newtonsoft.Json;

namespace DominoDatabase
{
    [Serializable]
    public class ReadBarcode
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string PlantCode { get; set; }
        public string ProdStdCode { get; set; }
        public string SerialNum { get; set; }
        public string MachineID { get; set; }
        public string OrderNo { get; set; }
        public string SeqNo { get; set; }
        public string JobDetailType { get; set; } 
        public string FullBarcode_Read { get; set; }
        public string AI_FullBarcode_Read { get; set; }
        public string FullBarcode_Parent { get; set; }
        public string AI_FullBarcode_Parent { get; set; }
        public string ParentProdStdCode { get; set; }
        public string ParentSerialNum { get; set; }
        public string Status { get; set; }
        public string Reserved1 { get; set; }
        public string Reserved2 { get; set; }
        public string Reserved3 { get; set; }
        public string Reserved4 { get; set; }
        public string Reserved5 { get; set; }
        public string FilePath { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string BarcodeType { get; set; }
        public string BarcodeDataFormat { get; set; }

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
                    default:
                        throw new NotImplementedException();
                }

            }
        }
        public ReadBarcode()
        {

        }
        public ReadBarcode(object list)
            :this()
        {
            this.UnionClass(list);
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.ProdStdCode, ProdStdCode);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.SerialNum, SerialNum);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.OrderNo, OrderNo);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.SeqNo, SeqNo);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.JobDetailType, JobDetailType);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.FullBarcode_Read, FullBarcode_Read);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.AI_FullBarcode_Read, AI_FullBarcode_Read);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.FullBarcode_Parent, FullBarcode_Parent);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.AI_FullBarcode_Parent, AI_FullBarcode_Parent);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.ParentProdStdCode, ParentProdStdCode);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.ParentSerialNum, ParentSerialNum);
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
                Controls.ReadBarcodeController.InsertServer(new DSM.Dmn_ReadBarcode(this), true);
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("ReadBarcode InsertServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool UpdateServer()
        {
            try
            {
                Controls.ReadBarcodeController.UpdateServer(new DSM.Dmn_ReadBarcode(this), true);
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("ReadBarcode UpdateServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool InsertLocal()
        {
            try
            {
                Controls.ReadBarcodeController.InsertLocal(new Local.Dmn_ReadBarcode(this), true);
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("ReadBarcode InsertLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool DeleteLocal(string userID)
        {
            try
            {
                Controls.ReadBarcodeController.DeleteLocalSingle(ProdStdCode, SerialNum, userID, true);
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("ReadBarcode DeleteLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool UpdateLocal()
        {
            try
            {
                Controls.ReadBarcodeController.UpdateLocal(new Local.Dmn_ReadBarcode(this), true);
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("ReadBarcode UpdateLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public ReadBarcode Clone()
        {
            return (ReadBarcode)this.MemberwiseClone();
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
    }
}
