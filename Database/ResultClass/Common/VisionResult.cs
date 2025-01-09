using System;
using DominoFunctions.ExtensionMethod;
using log4net;
using DominoFunctions.Enums;
using Newtonsoft.Json;

namespace DominoDatabase
{
    [Serializable]
    public class VisionResult
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string PlantCode { get; set; }
        public string OrderNo { get; set; }
        public string SeqNo { get; set; }
        public string JobDetailType { get; set; }
        public string DecodedBarcode { get; set; }
        public long Idx_Insert { get; set; }
        public string Read_OCR { get; set; }
        public string Grade_Barcode { get; set; }
        public string FilePath { get; set; }
        public int CameraIndex { get; set; }
        public string Status { get; set; }
        public string BarcodeType { get; set; }
        public string BarcodeDataFormat { get; set; }
        public string Reserved1 { get; set; }
        public string Reserved2 { get; set; }
        public string Reserved3 { get; set; }
        public string Reserved4 { get; set; }
        public string Reserved5 { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string SerialNumber { get; set; }
        public string ProdStdCode { get; set; }
        public string ProductName { get; set; }
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
        [JsonIgnore]
        public EnBarcodeGrade EnGrade_Barcode
        {
            get
            {
                switch(Grade_Barcode)
                {
                    case "A":
                        return EnBarcodeGrade.A;
                    case "B":
                        return EnBarcodeGrade.B;
                    case "C":
                        return EnBarcodeGrade.C;
                    case "D":
                        return EnBarcodeGrade.D;
                    case "F":
                        return EnBarcodeGrade.F;
                    case "N":
                        return EnBarcodeGrade.NotComputed;
                    default:
                        throw new NotImplementedException();
                }
            }
            set
            {
                switch(value)
                {
                    case EnBarcodeGrade.A:
                        Grade_Barcode = "A";
                        break;
                    case EnBarcodeGrade.B:
                        Grade_Barcode = "B";
                        break;
                    case EnBarcodeGrade.C:
                        Grade_Barcode = "C";
                        break;
                    case EnBarcodeGrade.D:
                        Grade_Barcode = "D";
                        break;
                    case EnBarcodeGrade.F:
                        Grade_Barcode = "F";
                        break;
                    case EnBarcodeGrade.NotComputed:
                        Grade_Barcode = "N";
                        break;
                }
            }
        }
        public string Weight { get; set; }
        public VisionResult()
        {

        }
        public VisionResult(object list)
            :this()
        {
            this.UnionClass(list);
        }
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.OrderNo, OrderNo);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.SeqNo, SeqNo);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.JobDetailType, JobDetailType);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.DecodedBarcode, DecodedBarcode);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Idx_Insert, Idx_Insert);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Read_OCR, Read_OCR);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Barcode_Grade, Grade_Barcode);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.BarcodeType, BarcodeType);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.FilePath, FilePath);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.CameraIndex, CameraIndex);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Status, Status);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved1, Reserved1);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved2, Reserved2);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved3, Reserved3);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved4, Reserved4);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved5, Reserved5);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.InsertUser, InsertUser);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.InsertDate, InsertDate);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.UpdateUser, UpdateUser);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.UpdateDate, UpdateDate);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Weight, Weight);
            return sb.ToString();
        }
        public bool InsertServer()
        {
            try
            {
                Controls.VisionResultController.InsertServer(new DSM.Dmn_VisionResult(this), true);
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("VisionResult InsertServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool UpdateServer()
        {
            try
            {
                Controls.VisionResultController.UpdateServer(new DSM.Dmn_VisionResult(this), true);
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("VisionResult UpdateServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool InsertLocal()
        {
            try
            {
                Controls.VisionResultController.InsertLocal(new Local.Dmn_VisionResult(this), true);
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("VisionResult InsertLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool UpdateLocal()
        {
            try
            {
                Controls.VisionResultController.UpdateLocal(new Local.Dmn_VisionResult(this), true);
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("VisionResult UpdateLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public VisionResult Clone()
        {
            return (VisionResult)this.MemberwiseClone();
        }
    }
}
