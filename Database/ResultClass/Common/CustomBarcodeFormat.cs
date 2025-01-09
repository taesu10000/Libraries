using System;
using DominoFunctions.ExtensionMethod;
using log4net;

namespace DominoDatabase
{
    [Serializable]
    public class CustomBarcodeFormat
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string PlantCode { get; set; }
        public string CustomBarcodeFormatID { get; set; }
        public string CustomBarcodeFormatStr { get; set; }
        public string Reserved1 { get; set; }
        public string Reserved2 { get; set; }
        public string Reserved3 { get; set; }
        public string Reserved4 { get; set; }
        public string Reserved5 { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public CustomBarcodeFormat()
        {

        }
        public CustomBarcodeFormat(object other)
            : this()
        {
            this.UnionClass(other);
        }
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.CustomBarcodeFormatID, CustomBarcodeFormatID);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.CustomBarcodeFormatStr, CustomBarcodeFormatStr);
            if(!string.IsNullOrEmpty(Reserved1))
                sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved1, Reserved1);
            if (!string.IsNullOrEmpty(Reserved2))
                sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved2, Reserved2);
            if (!string.IsNullOrEmpty(Reserved3))
                sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved3, Reserved3);
            if (!string.IsNullOrEmpty(Reserved4))
                sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved4, Reserved4);
            if (!string.IsNullOrEmpty(Reserved5))
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
                Controls.CustomBarcodeFormatController.InsertServer(new DSM.Dmn_CustomBarcodeFormat(this), true);
                return true;
            }
            catch(Exception ex)
            {
                log.InfoFormat("CustomBarcodeFormat InsertServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool UpdateServer()
        {
            try
            {
                Controls.CustomBarcodeFormatController.UpdateServer(new DSM.Dmn_CustomBarcodeFormat(this), true);
                return true;
            }
            catch(Exception ex)
            {
                log.InfoFormat("CustomBarcodeFormat UpdateServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool InsertLocal()
        {
            try
            {
                Controls.CustomBarcodeFormatController.InsertLocal(new Local.Dmn_CustomBarcodeFormat(this), true);
                return true;
            }
            catch(Exception ex)
            {
                log.InfoFormat("CustomBarcodeFormat InsertLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool UpdateLocal()
        {
            try
            {
                Controls.CustomBarcodeFormatController.UpdateLocal(new Local.Dmn_CustomBarcodeFormat(this), true);
                return true;
            }
            catch(Exception ex)
            {
                log.InfoFormat("CustomBarcodeFormat UpdateLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public CustomBarcodeFormat Clone()
        {
            return (CustomBarcodeFormat)this.MemberwiseClone();
        }
    }
}
