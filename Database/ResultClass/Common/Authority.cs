using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using DominoFunctions.Enums;
using DominoFunctions.ExtensionMethod;
using log4net;
using Newtonsoft.Json;
using LanguagePack;

namespace DominoDatabase
{
    [Serializable]
    public class Authority
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string PlantCode { get; set; }
        public string AuthID { get; set; }
        public string AuthName { get; set; }
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
        public AuthDetailCollection AuthDetailCollection { get; set; }
        public Authority()
        {
            AuthDetailCollection = new AuthDetailCollection();
        }
        public Authority(object list)
            : this()
        {
            this.UnionClass(list);
        }
        public override string ToString()
        {

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("[{0}:{1}]", UserInterfaces.Plant, PlantCode);
            sb.AppendFormat("[{0}:{1}]", UserInterfaces.AuthID, AuthID);
            sb.AppendFormat("[{0}:{1}]", UserInterfaces.AuthorityName, AuthName);
            sb.AppendFormat("[{0}:{1}]", UserInterfaces.UseYN, UseYN);
            sb.AppendFormat("[{0}:{1}]", UserInterfaces.Sequential, SeqNum);
            sb.AppendFormat("[{0}:{1}]", UserInterfaces.Reserved1, Reserved1);
            sb.AppendFormat("[{0}:{1}]", UserInterfaces.Reserved2, Reserved2);
            sb.AppendFormat("[{0}:{1}]", UserInterfaces.Reserved3, Reserved3);
            sb.AppendFormat("[{0}:{1}]", UserInterfaces.Reserved4, Reserved4);
            sb.AppendFormat("[{0}:{1}]", UserInterfaces.Reserved5, Reserved5);
            sb.AppendFormat("[{0}:{1}]", UserInterfaces.InsertUser, InsertUser);
            sb.AppendFormat("[{0}:{1}]", UserInterfaces.InsertDate, InsertDate);
            sb.AppendFormat("[{0}:{1}]", UserInterfaces.UpdateUser, UpdateUser);
            sb.AppendFormat("[{0}:{1}]", UserInterfaces.UpdateDate, UpdateDate);
            foreach(AuthorityDetail dt in this.AuthDetailCollection)
            {
                sb.AppendFormat("[{0}:{1}]", UserInterfaces.Detail, dt.ToString());
            }
            return sb.ToString();
        }
        [JsonIgnore]
        public bool IsDefault
        {
            get
            {
                return AuthID == "SysAdmin" || AuthID == "SysManager" || AuthID == "MfdUser" || AuthID == "MfdAdmin";
            }
        }
        public static bool IsDefaultByID(string authID)
        {
            return authID == "SysAdmin" || authID == "SysManager" || authID == "MfdUser" || authID == "MfdAdmin";
        }

        public static void CreateDefaultAuthorityLocal()
        {
            List<Authority> tmp = Controls.AuthorityController.SelectLocal("SysAdmin", null, null);
            DateTime insertTime = DateTime.Now;
            if (tmp.Count == 0)
            {
                Controls.AuthorityController.InsertLocal(new Local.Dmn_Auth_M
                {
                    AuthID = "SysAdmin",
                    AuthName = "SystemManager",
                    InsertUser = "System",
                    InsertDate = insertTime,
                    SeqNum = 0,
                    UseYN = "Y"
                });
                Controls.AuthorityController.InsertLocal(new Local.Dmn_Auth_D
                {
                    AuthID = "SysAdmin",
                    MenuID = EnMenuID.Job.ToString(),
                    MenuAuth = (long)EnUserAuthority_Job.SysAdmin,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertLocal(new Local.Dmn_Auth_D
                {
                    AuthID = "SysAdmin",
                    MenuID = EnMenuID.Product.ToString(),
                    MenuAuth = (long)EnUserAuthority_Product.SysAdmin,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertLocal(new Local.Dmn_Auth_D
                {
                    AuthID = "SysAdmin",
                    MenuID = EnMenuID.Printer.ToString(),
                    MenuAuth = (long)EnUserAuthority_Printer.SysAdmin,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertLocal(new Local.Dmn_Auth_D
                {
                    AuthID = "SysAdmin",
                    MenuID = EnMenuID.Setting.ToString(),
                    MenuAuth = (long)EnUserAuthority_Setting.SysAdmin,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertLocal(new Local.Dmn_Auth_D
                {
                    AuthID = "SysAdmin",
                    MenuID = EnMenuID.Vision.ToString(),
                    MenuAuth = (long)EnUserAuthority_Vision.SysAdmin,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertLocal(new Local.Dmn_Auth_D
                {
                    AuthID = "SysAdmin",
                    MenuID = EnMenuID.Line.ToString(),
                    MenuAuth = (long)EnUserAuthority_Line.SysAdmin,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
            }
            tmp = Controls.AuthorityController.SelectLocal("SysManager", null);
            if (tmp.Count == 0)
            {
                Controls.AuthorityController.InsertLocal(new Local.Dmn_Auth_M
                {
                    AuthID = "SysManager",
                    AuthName = "Supervisor",
                    InsertUser = "System",
                    InsertDate = insertTime,
                    SeqNum = 0,
                    UseYN = "Y"
                });
                Controls.AuthorityController.InsertLocal(new Local.Dmn_Auth_D
                {
                    AuthID = "SysManager",
                    MenuID = EnMenuID.Job.ToString(),
                    MenuAuth = (long)EnUserAuthority_Job.SysManager,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertLocal(new Local.Dmn_Auth_D
                {
                    AuthID = "SysManager",
                    MenuID = EnMenuID.Product.ToString(),
                    MenuAuth = (long)EnUserAuthority_Product.SysManager,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertLocal(new Local.Dmn_Auth_D
                {
                    AuthID = "SysManager",
                    MenuID = EnMenuID.Printer.ToString(),
                    MenuAuth = (long)EnUserAuthority_Printer.SysManager,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertLocal(new Local.Dmn_Auth_D
                {
                    AuthID = "SysManager",
                    MenuID = EnMenuID.Setting.ToString(),
                    MenuAuth = (long)EnUserAuthority_Setting.SysManager,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertLocal(new Local.Dmn_Auth_D
                {
                    AuthID = "SysManager",
                    MenuID = EnMenuID.Vision.ToString(),
                    MenuAuth = (long)EnUserAuthority_Vision.SysManager,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertLocal(new Local.Dmn_Auth_D
                {
                    AuthID = "SysManager",
                    MenuID = EnMenuID.Line.ToString(),
                    MenuAuth = (long)EnUserAuthority_Line.SysManager,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
            }
            tmp = Controls.AuthorityController.SelectLocal("MfdAdmin", null);
            if (tmp.Count == 0)
            {
                Controls.AuthorityController.InsertLocal(new Local.Dmn_Auth_M
                {
                    AuthID = "MfdAdmin",
                    AuthName = "ProductionManager",
                    InsertUser = "System",
                    InsertDate = insertTime,
                    SeqNum = 0,
                    UseYN = "Y"
                });
                Controls.AuthorityController.InsertLocal(new Local.Dmn_Auth_D
                {
                    AuthID = "MfdAdmin",
                    MenuID = EnMenuID.Job.ToString(),
                    MenuAuth = (long)EnUserAuthority_Job.MfdAdmin,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertLocal(new Local.Dmn_Auth_D
                {
                    AuthID = "MfdAdmin",
                    MenuID = EnMenuID.Product.ToString(),
                    MenuAuth = (long)EnUserAuthority_Product.MfdAdmin,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertLocal(new Local.Dmn_Auth_D
                {
                    AuthID = "MfdAdmin",
                    MenuID = EnMenuID.Printer.ToString(),
                    MenuAuth = (long)EnUserAuthority_Printer.MfdAdmin,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertLocal(new Local.Dmn_Auth_D
                {
                    AuthID = "MfdAdmin",
                    MenuID = EnMenuID.Setting.ToString(),
                    MenuAuth = (long)EnUserAuthority_Setting.MfdAdmin,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertLocal(new Local.Dmn_Auth_D
                {
                    AuthID = "MfdAdmin",
                    MenuID = EnMenuID.Vision.ToString(),
                    MenuAuth = (long)EnUserAuthority_Vision.MfdAdmin,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertLocal(new Local.Dmn_Auth_D
                {
                    AuthID = "MfdAdmin",
                    MenuID = EnMenuID.Line.ToString(),
                    MenuAuth = (long)EnUserAuthority_Line.MfdAdmin,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
            }
            tmp = Controls.AuthorityController.SelectLocal("MfdUser", null);
            if (tmp.Count == 0)
            {
                Controls.AuthorityController.InsertLocal(new Local.Dmn_Auth_M
                {
                    AuthID = "MfdUser",
                    AuthName = "Operator",
                    InsertUser = "System",
                    InsertDate = insertTime,
                    SeqNum = 0,
                    UseYN = "Y"
                });
                Controls.AuthorityController.InsertLocal(new Local.Dmn_Auth_D
                {
                    AuthID = "MfdUser",
                    MenuID = EnMenuID.Job.ToString(),
                    MenuAuth = (long)EnUserAuthority_Job.MfdUser,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertLocal(new Local.Dmn_Auth_D
                {
                    AuthID = "MfdUser",
                    MenuID = EnMenuID.Product.ToString(),
                    MenuAuth = (long)EnUserAuthority_Product.MfdUser,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertLocal(new Local.Dmn_Auth_D
                {
                    AuthID = "MfdUser",
                    MenuID = EnMenuID.Printer.ToString(),
                    MenuAuth = (long)EnUserAuthority_Printer.MfdUser,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertLocal(new Local.Dmn_Auth_D
                {
                    AuthID = "MfdUser",
                    MenuID = EnMenuID.Setting.ToString(),
                    MenuAuth = (long)EnUserAuthority_Setting.MfdUser,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertLocal(new Local.Dmn_Auth_D
                {
                    AuthID = "MfdUser",
                    MenuID = EnMenuID.Vision.ToString(),
                    MenuAuth = (long)EnUserAuthority_Vision.MfdUser,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertLocal(new Local.Dmn_Auth_D
                {
                    AuthID = "MfdUser",
                    MenuID = EnMenuID.Line.ToString(),
                    MenuAuth = (long)EnUserAuthority_Line.MfdUser,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
            }
        }
        public static void CreateDefaultAuthorityServer(string plant, string authDiv)
        {
            List<Authority> tmp = Controls.AuthorityController.SelectServer(plant, "SysAdmin", null, authDiv, null, null, null, null);
            DateTime insertTime = DateTime.Now;
            if (tmp.Count == 0)
            {
                Controls.AuthorityController.InsertServer(new DSM.Dmn_Auth_M
                {
                    PlantCode = plant,
                    AuthID = "SysAdmin",
                    AuthName = "SystemManager",
                    InsertUser = "System",
                    InsertDate = insertTime,
                    SeqNum = 0,
                    UseYN = "Y"
                });
                Controls.AuthorityController.InsertServer(new DSM.Dmn_Auth_D
                {
                    PlantCode = plant,
                    AuthID = "SysAdmin",
                    AuthDiv = authDiv,
                    MenuID = EnMenuID.Job.ToString(),
                    MenuAuth = (long)EnUserAuthority_Job.SysAdmin,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertServer(new DSM.Dmn_Auth_D
                {
                    PlantCode = plant,
                    AuthID = "SysAdmin",
                    AuthDiv = authDiv,
                    MenuID = EnMenuID.Product.ToString(),
                    MenuAuth = (long)EnUserAuthority_Product.SysAdmin,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertServer(new DSM.Dmn_Auth_D
                {
                    PlantCode = plant,
                    AuthID = "SysAdmin",
                    AuthDiv = authDiv,
                    MenuID = EnMenuID.Printer.ToString(),
                    MenuAuth = authDiv == "DSM" ? 0 : (long)EnUserAuthority_Printer.SysAdmin,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertServer(new DSM.Dmn_Auth_D
                {
                    PlantCode = plant,
                    AuthID = "SysAdmin",
                    AuthDiv = authDiv,
                    MenuID = EnMenuID.Setting.ToString(),
                    MenuAuth = (long)EnUserAuthority_Setting.SysAdmin,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertServer(new DSM.Dmn_Auth_D
                {
                    PlantCode = plant,
                    AuthID = "SysAdmin",
                    AuthDiv = authDiv,
                    MenuID = EnMenuID.Vision.ToString(),
                    MenuAuth = authDiv == "DSM" ? 0 : (long)EnUserAuthority_Vision.SysAdmin,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertServer(new DSM.Dmn_Auth_D
                {
                    PlantCode = plant,
                    AuthID = "SysAdmin",
                    AuthDiv = authDiv,
                    MenuID = EnMenuID.Line.ToString(),
                    MenuAuth = authDiv != "DSM" ? 0 : (long)EnUserAuthority_Line.SysAdmin,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
            }
            tmp = Controls.AuthorityController.SelectServer(plant, "SysManager", null, authDiv, null, null, null, null);
            if (tmp.Count == 0)
            {
                Controls.AuthorityController.InsertServer(new DSM.Dmn_Auth_M
                {
                    PlantCode = plant,
                    AuthID = "SysManager",
                    AuthName = "Supervisor",
                    InsertUser = "System",
                    InsertDate = insertTime,
                    SeqNum = 0,
                    UseYN = "Y"
                });
                Controls.AuthorityController.InsertServer(new DSM.Dmn_Auth_D
                {
                    PlantCode = plant,
                    AuthID = "SysManager",
                    AuthDiv = authDiv,
                    MenuID = EnMenuID.Job.ToString(),
                    MenuAuth = (long)EnUserAuthority_Job.SysManager,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertServer(new DSM.Dmn_Auth_D
                {
                    PlantCode = plant,
                    AuthID = "SysManager",
                    AuthDiv = authDiv,
                    MenuID = EnMenuID.Product.ToString(),
                    MenuAuth = (long)EnUserAuthority_Product.SysManager,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertServer(new DSM.Dmn_Auth_D
                {
                    PlantCode = plant,
                    AuthID = "SysManager",
                    AuthDiv = authDiv,
                    MenuID = EnMenuID.Printer.ToString(),
                    MenuAuth = authDiv == "DSM" ? 0 : (long)EnUserAuthority_Printer.SysManager,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertServer(new DSM.Dmn_Auth_D
                {
                    PlantCode = plant,
                    AuthID = "SysManager",
                    AuthDiv = authDiv,
                    MenuID = EnMenuID.Setting.ToString(),
                    MenuAuth = (long)EnUserAuthority_Setting.SysManager,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertServer(new DSM.Dmn_Auth_D
                {
                    PlantCode = plant,
                    AuthID = "SysManager",
                    AuthDiv = authDiv,
                    MenuID = EnMenuID.Vision.ToString(),
                    MenuAuth = authDiv == "DSM" ? 0 : (long)EnUserAuthority_Vision.SysManager,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertServer(new DSM.Dmn_Auth_D
                {
                    PlantCode = plant,
                    AuthID = "SysManager",
                    AuthDiv = authDiv,
                    MenuID = EnMenuID.Line.ToString(),
                    MenuAuth = authDiv != "DSM" ? 0 : (long)EnUserAuthority_Line.SysManager,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
            }
            tmp = Controls.AuthorityController.SelectServer(plant, "MfdAdmin", null, authDiv, null, null, null, null);
            if (tmp.Count == 0)
            {
                Controls.AuthorityController.InsertServer(new DSM.Dmn_Auth_M
                {
                    PlantCode = plant,
                    AuthID = "MfdAdmin",
                    AuthName = "ProductionManager",
                    InsertUser = "System",
                    InsertDate = insertTime,
                    SeqNum = 0,
                    UseYN = "Y"
                });
                Controls.AuthorityController.InsertServer(new DSM.Dmn_Auth_D
                {
                    PlantCode = plant,
                    AuthID = "MfdAdmin",
                    AuthDiv = authDiv,
                    MenuID = EnMenuID.Job.ToString(),
                    MenuAuth = (long)EnUserAuthority_Job.MfdAdmin,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertServer(new DSM.Dmn_Auth_D
                {
                    PlantCode = plant,
                    AuthID = "MfdAdmin",
                    AuthDiv = authDiv,
                    MenuID = EnMenuID.Product.ToString(),
                    MenuAuth = (long)EnUserAuthority_Product.MfdAdmin,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertServer(new DSM.Dmn_Auth_D
                {
                    PlantCode = plant,
                    AuthID = "MfdAdmin",
                    AuthDiv = authDiv,
                    MenuID = EnMenuID.Printer.ToString(),
                    MenuAuth = authDiv == "DSM" ? 0 : (long)EnUserAuthority_Printer.MfdAdmin,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertServer(new DSM.Dmn_Auth_D
                {
                    PlantCode = plant,
                    AuthID = "MfdAdmin",
                    AuthDiv = authDiv,
                    MenuID = EnMenuID.Setting.ToString(),
                    MenuAuth = (long)EnUserAuthority_Setting.MfdAdmin,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertServer(new DSM.Dmn_Auth_D
                {
                    PlantCode = plant,
                    AuthID = "MfdAdmin",
                    AuthDiv = authDiv,
                    MenuID = EnMenuID.Vision.ToString(),
                    MenuAuth = authDiv == "DSM" ? 0 : (long)EnUserAuthority_Vision.MfdAdmin,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertServer(new DSM.Dmn_Auth_D
                {
                    PlantCode = plant,
                    AuthID = "MfdAdmin",
                    AuthDiv = authDiv,
                    MenuID = EnMenuID.Line.ToString(),
                    MenuAuth = authDiv != "DSM" ? 0 : (long)EnUserAuthority_Line.MfdAdmin,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
            }
            tmp = Controls.AuthorityController.SelectServer(plant, "MfdUser", null, authDiv, null, null, null, null);
            if (tmp.Count == 0)
            {
                Controls.AuthorityController.InsertServer(new DSM.Dmn_Auth_M
                {
                    PlantCode = plant,
                    AuthID = "MfdUser",
                    AuthName = "Operator",
                    InsertUser = "System",
                    InsertDate = insertTime,
                    SeqNum = 0,
                    UseYN = "Y"
                });
                Controls.AuthorityController.InsertServer(new DSM.Dmn_Auth_D
                {
                    PlantCode = plant,
                    AuthID = "MfdUser",
                    AuthDiv = authDiv,
                    MenuID = EnMenuID.Job.ToString(),
                    MenuAuth = (long)EnUserAuthority_Job.MfdUser,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertServer(new DSM.Dmn_Auth_D
                {
                    PlantCode = plant,
                    AuthID = "MfdUser",
                    AuthDiv = authDiv,
                    MenuID = EnMenuID.Product.ToString(),
                    MenuAuth = (long)EnUserAuthority_Product.MfdUser,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertServer(new DSM.Dmn_Auth_D
                {
                    PlantCode = plant,
                    AuthID = "MfdUser",
                    AuthDiv = authDiv,
                    MenuID = EnMenuID.Printer.ToString(),
                    MenuAuth = authDiv == "DSM" ? 0 : (long)EnUserAuthority_Printer.MfdUser,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertServer(new DSM.Dmn_Auth_D
                {
                    PlantCode = plant,
                    AuthID = "MfdUser",
                    AuthDiv = authDiv,
                    MenuID = EnMenuID.Setting.ToString(),
                    MenuAuth = (long)EnUserAuthority_Setting.MfdUser,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertServer(new DSM.Dmn_Auth_D
                {
                    PlantCode = plant,
                    AuthID = "MfdUser",
                    AuthDiv = authDiv,
                    MenuID = EnMenuID.Vision.ToString(),
                    MenuAuth = authDiv == "DSM" ? 0 : (long)EnUserAuthority_Vision.MfdUser,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
                Controls.AuthorityController.InsertServer(new DSM.Dmn_Auth_D
                {
                    PlantCode = plant,
                    AuthID = "MfdUser",
                    AuthDiv = authDiv,
                    MenuID = EnMenuID.Line.ToString(),
                    MenuAuth = authDiv != "DSM" ? 0 : (long)EnUserAuthority_Line.MfdUser,
                    InsertDate = insertTime,
                    InsertUser = "System"
                });
            }
        }
        
        public bool InsertServer()
        {
            try
            {
                Controls.AuthorityController.InsertServer(new DSM.Dmn_Auth_M(this), true);
                foreach (AuthorityDetail dtl in this.AuthDetailCollection)
                {
                    Controls.AuthorityController.InsertServer(new DSM.Dmn_Auth_D(dtl) { PlantCode = this.PlantCode, AuthID = this.AuthID , InsertUser = this.InsertUser, InsertDate = this.InsertDate});
                }
                return true;
            }
            catch(Exception ex)
            {
                log.InfoFormat("Authority InsertServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }

        

        public bool UpdateServer()
        {
            try
            {
                Controls.AuthorityController.UpdateServer(new DSM.Dmn_Auth_M(this) , true);
                foreach (AuthorityDetail dtl in this.AuthDetailCollection)
                {
                    Controls.AuthorityController.UpdateServer(new DSM.Dmn_Auth_D(dtl) { PlantCode = this.PlantCode, AuthID = this.AuthID, UpdateDate = this.UpdateDate, UpdateUser = this.UpdateUser });
                }
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("Authority UpdateServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool InsertLocal()
        {
            try
            {
                Controls.AuthorityController.InsertLocal(new Local.Dmn_Auth_M(this), true);
                foreach (AuthorityDetail dtl in this.AuthDetailCollection)
                {
                    Controls.AuthorityController.InsertLocal(new Local.Dmn_Auth_D(dtl) { AuthID = this.AuthID, InsertUser = this.InsertUser, InsertDate = this.InsertDate });
                }
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("Authority InsertLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }



        public bool UpdateLocal()
        {
            try
            {
                Controls.AuthorityController.UpdateLocal(new Local.Dmn_Auth_M(this), true);
                foreach (AuthorityDetail dtl in this.AuthDetailCollection)
                {
                    Controls.AuthorityController.UpdateLocal(new Local.Dmn_Auth_D(dtl) {  AuthID = this.AuthID, UpdateDate = this.UpdateDate, UpdateUser = this.UpdateUser });
                }
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("Authority UpdateLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public Authority Clone()
        {
            Authority authority = (Authority)this.MemberwiseClone();
            authority.AuthDetailCollection = (AuthDetailCollection)this.AuthDetailCollection.Clone();
            return authority;
        }
    }
    [Serializable]
    public class AuthDetailCollection : ICollection<AuthorityDetail>
    {

        public AuthDetailCollection()
        {
            _item = new List<AuthorityDetail>();
        }
        public static AuthDetailCollection EmptyDetailCollection()
        {
            return new AuthDetailCollection
            {
                new AuthorityDetail
                {
                    AuthDiv = "PM",
                    MenuAuth = 0,
                    MenuID = EnMenuID.Job.ToString()
                },
                new AuthorityDetail
                {
                    AuthDiv = "PM",
                    MenuAuth = 0,
                    MenuID = EnMenuID.Line.ToString()
                },
                new AuthorityDetail
                {
                    AuthDiv = "PM",
                    MenuAuth = 0,
                    MenuID = EnMenuID.Printer.ToString()
                },
                new AuthorityDetail
                {
                    AuthDiv = "PM",
                    MenuAuth = 0,
                    MenuID = EnMenuID.Product.ToString()
                },
                new AuthorityDetail
                {
                    AuthDiv = "PM",
                    MenuAuth = 0,
                    MenuID = EnMenuID.Setting.ToString()
                },
                new AuthorityDetail
                {
                    AuthDiv = "PM",
                    MenuAuth = 0,
                    MenuID = EnMenuID.Vision.ToString()
                },
                new AuthorityDetail
                {
                    AuthDiv = "AG",
                    MenuAuth = 0,
                    MenuID = EnMenuID.Job.ToString()
                },
                new AuthorityDetail
                {
                    AuthDiv = "AG",
                    MenuAuth = 0,
                    MenuID = EnMenuID.Line.ToString()
                },
                new AuthorityDetail
                {
                    AuthDiv = "AG",
                    MenuAuth = 0,
                    MenuID = EnMenuID.Printer.ToString()
                },
                new AuthorityDetail
                {
                    AuthDiv = "AG",
                    MenuAuth = 0,
                    MenuID = EnMenuID.Product.ToString()
                },
                new AuthorityDetail
                {
                    AuthDiv = "AG",
                    MenuAuth = 0,
                    MenuID = EnMenuID.Setting.ToString()
                },
                new AuthorityDetail
                {
                    AuthDiv = "AG",
                    MenuAuth = 0,
                    MenuID = EnMenuID.Vision.ToString()
                },
                new AuthorityDetail
                {
                    AuthDiv = "DSM",
                    MenuAuth = 0,
                    MenuID = EnMenuID.Job.ToString()
                },
                new AuthorityDetail
                {
                    AuthDiv = "DSM",
                    MenuAuth = 0,
                    MenuID = EnMenuID.Line.ToString()
                },
                new AuthorityDetail
                {
                    AuthDiv = "DSM",
                    MenuAuth = 0,
                    MenuID = EnMenuID.Printer.ToString()
                },
                new AuthorityDetail
                {
                    AuthDiv = "DSM",
                    MenuAuth = 0,
                    MenuID = EnMenuID.Product.ToString()
                },
                new AuthorityDetail
                {
                    AuthDiv = "DSM",
                    MenuAuth = 0,
                    MenuID = EnMenuID.Setting.ToString()
                },
                new AuthorityDetail
                {
                    AuthDiv = "DSM",
                    MenuAuth = 0,
                    MenuID = EnMenuID.Vision.ToString()
                },

            };
        }

        public AuthDetailCollection(List<AuthorityDetail> list)
            : this()
        {
            _item.AddRange(list);
        }


        public AuthorityDetail this[int index]
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
        public AuthorityDetail this[string menuID]
        {
            get
            {
                return _item.Where(x => x.MenuID.Equals(menuID)).FirstOrDefault();
            }
            set
            {
                var tmp = _item.Where(x => x.MenuID.Equals(menuID)).FirstOrDefault();
                tmp = value;
            }
        }
        public AuthorityDetail this[EnMenuID id]
        {
            get
            {
                return _item.Where(x => x.MenuID.Equals(id.ToString())).FirstOrDefault();
            }
            set
            {
                var tmp = _item.Where(x => x.MenuID.Equals(id.ToString())).FirstOrDefault();
                tmp = value;
            }
        }
        public void Add(AuthorityDetail detail)
        {
            _item.Add(detail);
        }
        public void AddRange(IEnumerable<AuthorityDetail> list)
        {
            _item.AddRange(list);
        }
        public void RemoveAt(int index)
        {
            _item.RemoveAt(index);
        }
        public bool Remove(AuthorityDetail detail)
        {
            return _item.Remove(detail);
        }
        public void RemoveRange(int index, int count)
        {
            _item.RemoveRange(index, count);
        }

        protected readonly List<AuthorityDetail> _item;
        public IEnumerator<AuthorityDetail> GetEnumerator()
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
        public bool Contains(AuthorityDetail detail)
        {
            return _item.Contains(detail);
        }
        public bool Contains(EnMenuID menu)
        {
            return _item.Any(item => item.MenuID.Equals(menu.ToString()));
        }
        public void CopyTo(AuthorityDetail[] array, int arrayIndex)
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
        public void AddIfNotExists(EnMenuID menu)
        {
            if (!this.Contains(menu))
                _item.Add(new AuthorityDetail
                {
                    MenuID = menu.ToString(),
                    MenuAuth = 0,
                });
        }

        public AuthDetailCollection Clone()
        {
            AuthDetailCollection retVal = new AuthDetailCollection();
            foreach(AuthorityDetail dt in this)
            {
                retVal.Add(dt.Clone());
            }
            return retVal;
        }

    }
    [Serializable]
    public class AuthorityDetail
    {
        public AuthorityDetail()
        {

        }
        public AuthorityDetail(object list)
        {
            this.UnionClass(list);
        }
        public string AuthDiv { get; set; }
        public string MenuID { get; set; }
        public long? MenuAuth { get; set; }
        public string Reserved1 { get; set; }
        public string Reserved2 { get; set; }
        public string Reserved3 { get; set; }
        public string Reserved4 { get; set; }
        public string Reserved5 { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.AuthorityDivision, AuthDiv);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.MenuID, MenuID);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.MenuAuthority, MenuAuth);
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

        public AuthorityDetail Clone()
        {
            return (AuthorityDetail)this.MemberwiseClone();
        }
    }
}
