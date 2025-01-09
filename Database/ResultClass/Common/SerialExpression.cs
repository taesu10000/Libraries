using System;
using DominoFunctions.ExtensionMethod;
using log4net;

namespace DominoDatabase
{
    [Serializable]
    public class SerialExpression 
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string PlantCode { get; set; }
        public string SnExpressionID { get; set; } 
        public string SnExpressionStr { get; set; }
        public string ExceptChar { get; set; }
        public string UseYN { get; set; }
        public int SeqNum { get; set; }
        public string Reserved1 { get; set; }
        public string Reserved2 { get; set; }
        public string Reserved3 { get; set; }
        public string Reserved4 { get; set; }
        public string Reserved5 { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public SerialExpression()
        {

        }
        public SerialExpression(object list)
            :this()
        {
            this.UnionClass(list);
        }
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.SnExpressID, SnExpressionID);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.SerialExpression, SnExpressionStr);
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
                Controls.SerialExpressionController.InsertServer(new DSM.Dmn_Serial_Expression(this), true);
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialExpression InsertServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool UpdateServer()
        {
            try
            {
                Controls.SerialExpressionController.UpdateServer(new DSM.Dmn_Serial_Expression(this), true);
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialExpression UpdateServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool InsertLocal()
        {
            try
            {
                Controls.SerialExpressionController.InsertLocal(new Local.Dmn_Serial_Expression(this), true);
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialExpression InsertLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool UpdateLocal()
        {
            try
            {
                Controls.SerialExpressionController.UpdateLocal(new Local.Dmn_Serial_Expression(this), true);
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialExpression UpdateLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public static bool CreateExpressionDefaultLocal()
        {
            try
            {
                if(DominoDatabase.Controls.SerialExpressionController.SelectLocalSingle("Default") != null)
                    return true;
                Local.Dmn_Serial_Expression tmp = new Local.Dmn_Serial_Expression
                {
                    SnExpressionID = "Default",
                    SnExpressionStr = "[12:0:Left]&[LotNumber::5:Right:0:Left]|[SN:SequentialNumeric:0:7:]",
                    UseYN = "Y",
                    SeqNum = 0,
                    InsertUser = "System",
                    InsertDate = DateTime.Now,
                };
                Controls.SerialExpressionController.InsertLocal(tmp, true);
                return true;
            }
            catch(Exception ex)
            {
                log.InfoFormat("SerialExpression CreateExpressionDefaultLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }

        public static bool CreateExpressionDefaultServer(string plantCode)
        {
            try
            {
                DSM.Dmn_Serial_Expression tmp = new DSM.Dmn_Serial_Expression
                {
                    PlantCode = plantCode,
                    SnExpressionID = "Default",
                    SnExpressionStr = "[12:0:Left]&[LotNumber::5:Right:0:Left]|[SN:SequentialNumeric:0:7:]",
                    UseYN = "Y",
                    SeqNum = 0,
                    InsertUser = "System",
                    InsertDate = DateTime.Now,
                };
                Controls.SerialExpressionController.InsertServer(tmp, true);
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialExpression CreateExpressionDefaultServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public SerialExpression Clone()
        {
            return (SerialExpression)this.MemberwiseClone();
        }
    }
}
