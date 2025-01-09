using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using DominoFunctions.ExtensionMethod;
using log4net;
using DominoFunctions.Enums;
using Newtonsoft.Json;

namespace DominoDatabase
{
    [Serializable]
    public class User
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string PlantCode { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string UserPW { get; set; }
        public DateTime? LastLogIn { get; set; }
        public int? FailLogInCount { get; set; }
        public string LockYN { get; set; }
        public string UseYN { get; set; }
        public string PrePW1 { get; set; }
        public string PrePW2 { get; set; }
        public string PrePW3 { get; set; }
        public DateTime? LastPWUpdate { get; set; }
        public string AuthID { get; set; }
        public string DeptCode { get; set; }
        public string Email { get; set; }
        public string PhoneNum { get; set; }
        public string Reserved1 { get; set; }
        public string Reserved2 { get; set; }
        public string Reserved3 { get; set; }
        public string Reserved4 { get; set; }
        public string Reserved5 { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public UserDetailCollection UserDetail { get; set; }
        public string AuthorityName { get; set; }
        [JsonIgnore]
        public bool IsMaster { get; set; }
        [JsonIgnore]
        public string DecryptedPassword
        {
            get { return DominoFunctions.SimpleAES.SimpleAES.Instance.DecryptString(UserPW); }
        }
        public User()
        {
            UserDetail = new UserDetailCollection();
        }
        public User(object list)
            : this()
        {
            this.UnionClass(list);
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Plant, PlantCode);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.UserID, UserID);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.UserName, UserName);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.LastLogIn, LastLogIn);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.FailLogInAttemptsCount, FailLogInCount);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Locked, LockYN);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.LastPasswordUpdate, LastPWUpdate);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.AuthID, AuthID);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.DeptCode, DeptCode);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Email, Email);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.PhoneNumber, PhoneNum);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved1, Reserved1);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved2, Reserved1);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved3, Reserved1);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved4, Reserved1);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved5, Reserved1);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.InsertUser, InsertUser);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.InsertDate, InsertDate);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.UpdateUser, UpdateUser);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.UpdateDate, UpdateDate);
            foreach (UserDetail dt in this.UserDetail)
            {
                sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Detail, dt.ToString());
            }
            return sb.ToString();
        }
        public string ToSplitString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.Plant, PlantCode);
            sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.UserID, UserID);
            sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.UserName, UserName);
            sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.LastLogIn, LastLogIn);
            sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.FailLogInAttemptsCount, FailLogInCount);
            sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.Locked, LockYN);
            sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.LastPasswordUpdate, LastPWUpdate);
            sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.AuthID, AuthID);
            sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.DeptCode, DeptCode);
            sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.Email, Email);
            sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.PhoneNumber, PhoneNum);
            sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.Reserved1, Reserved1);
            sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.Reserved2, Reserved1);
            sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.Reserved3, Reserved1);
            sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.Reserved4, Reserved1);
            sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.Reserved5, Reserved1);
            sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.InsertUser, InsertUser);
            sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.InsertDate, InsertDate);
            sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.UpdateUser, UpdateUser);
            sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.UpdateDate, UpdateDate);
            foreach (UserDetail dt in this.UserDetail)
            {
                sb.AppendFormat("[{0}:{1}]\r\n", LanguagePack.UserInterfaces.Detail, dt.ToString());
            }
            return sb.ToString();
        }
        public bool InsertServer()
        {
            try
            {
                Controls.UserController.InsertServer(new DSM.Dmn_User_M(this), true);
                foreach (UserDetail dtl in this.UserDetail)
                {
                    Controls.UserController.InsertServer(new DSM.Dmn_User_D(dtl) { PlantCode = this.PlantCode, UserID = this.UserID, InsertUser = this.InsertUser, InsertDate = this.InsertDate });
                }
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("User InsertServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool UpdateServer()
        {
            try
            {
                var svr = Controls.UserController.SelectServer(this.PlantCode, this.UserID, null, null, null, null, null, null, null, null, null)?.FirstOrDefault() ?? new User();
                var details = this.UserDetail.Select(x => new DSM.Dmn_User_D(x) { PlantCode = this.PlantCode, UserID = this.UserID, InsertUser = this.InsertUser, InsertDate = this.InsertDate, UpdateUser = this.UpdateUser, UpdateDate = this.UpdateDate });

                var svrkeys = svr.UserDetail.Select(x => x.MachineID);
                var localkeys = details.Select(x => x.MachineID);

                var toinsert = details.Where(x => localkeys.Except(svrkeys).Contains(x.MachineID));
                var toupdate = details.Where(x => localkeys.Intersect(svrkeys).Contains(x.MachineID));
                var deletekeys = svrkeys.Where(x => svrkeys.Except(localkeys).Contains(x));
                
                Controls.UserController.UpdateServer(new DSM.Dmn_User_M(this), true);
                Controls.UserController.UpdateServer(toupdate, true);
                if (toinsert.Count() > 0) 
                    Controls.UserController.InsertServer(toinsert, true);
                if (deletekeys.Count() > 0)
                {
                    Controls.UserController.DeleteServer(PlantCode, UserID, UpdateUser, deletekeys);
                }
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("User UpdateServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool UpdateServerLoginSuccess()
        {
            try
            {
                FailLogInCount = 0;
                UpdateDate = DateTime.Now;
                LastLogIn = DateTime.Now;
                var u = new DSM.Dmn_User_M(this);
                Controls.UserController.UpdateServerLogin(u, true);
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("User UpdateServerLogin Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool UpdateServerLoginFailed()
        {
            try
            {
                FailLogInCount++;
                UpdateDate = DateTime.Now;
                var u = new DSM.Dmn_User_M(this);
                Controls.UserController.UpdateServerLogin(u, true);
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("User UpdateServerLogin Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool UpdateServerLoginLockedout()
        {
            try
            {
                LockYN = "Y";
                FailLogInCount = 0;
                UpdateDate = DateTime.Now;
                var u = new DSM.Dmn_User_M(this);
                Controls.UserController.UpdateServerLogin(u, true);
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("User UpdateServerLogin Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool UpdateServerSingle()
        {
            try
            {
                Controls.UserController.UpdateServer(new DSM.Dmn_User_M(this), true);
                foreach (UserDetail dtl in this.UserDetail)
                {
                    Controls.UserController.UpdateServer(new DSM.Dmn_User_D(dtl) { PlantCode = this.PlantCode, UserID = this.UserID, InsertUser = this.InsertUser, InsertDate = this.InsertDate, UpdateUser = this.UpdateUser, UpdateDate = this.UpdateDate });
                }
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("User UpdateServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool UpdatePasswordServer()
        {
            try
            {
                Controls.UserController.UpdatePasswordServer(new DSM.Dmn_User_M(this), true);
                log.InfoFormat("User UpdatePasswordServer Finished : {0}", new { PlantCode, UserID, UserName });
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("User UpdatePasswordServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool InsertLocal()
        {
            try
            {
                Controls.UserController.InsertLocal(new Local.Dmn_User(this) { AuthID = UserDetail == null ? null : UserDetail[0].AuthID }, true);
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("User InsertLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool UpdateLocal()
        {
            try
            {
                Controls.UserController.UpdateLocal(new Local.Dmn_User(this), true);
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("User UpdateLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }

        public static bool CheckPasswordServer(string plantCode, string userID, string password)
        {
            if (userID == "Master" || userID == "System")
                return true;
            User tmp = Controls.UserController.SelectServerSingle(plantCode, userID, null, null, null, null, true);
            if (tmp == null)
                return false;
            return Controls.UserController.SelectServerSingle(plantCode, userID, null, null, null, null, true).UserPW.Equals(password);
        }
        public static bool CheckPasswordLocal(string userID, string password)
        {
            if (userID == "Master")
                return true;
            return Controls.UserController.SelectLocalSingle(userID, null, null, null, true).UserPW.Equals(password);
        }

        public EnUserAccountError ConfirmPassword(string password)
        {
            if (IsMaster)
                return EnUserAccountError.None;
            return UserPW.Equals(DominoFunctions.SimpleAES.SimpleAES.Instance.EncryptToString(password)) ? EnUserAccountError.None : EnUserAccountError.WrongPassword;
        }

        public static User MasterLogIn()
        {
            User tmp = new User();
            tmp.IsMaster = true;
            tmp.UserID = "Master";
            tmp.UserName = "Master";
            DominoFunctions.RegistryControl.Instance.BlockDeskTop = false;
            return tmp;
        }
        public static EnUserAccountError LogInServer(string plant, string ID, string password, string machineID, out User user)
        {
            user = Controls.UserController.SelectServerSingle(plant, ID, null, null, null,null, null);
            int passwordAge = Convert.ToInt32(Controls.ConfigureController.SelectServerSingle(plant, EnDSMConfigure.PasswordAge.ToString()).Configure_Value);
            int passwordErrorLimit = Convert.ToInt32(Controls.ConfigureController.SelectServerSingle(plant, EnDSMConfigure.PasswordErrorLimit.ToString()).Configure_Value);
            int passwordWarning = Convert.ToInt32(Controls.ConfigureController.SelectServerSingle(plant, EnDSMConfigure.PasswordExpireWarning.ToString()).Configure_Value);
            if (user == null)
            {
                return EnUserAccountError.IDNotExists;
            }
            if (user.UseYN == "N")
            {
                return EnUserAccountError.AccountDeleted;
            }
            if (user.LockYN == "Y")
            {
                return EnUserAccountError.AccountLocked;
            }
            if (!user.UserPW.Equals(password))
            {
                if (passwordErrorLimit != 0 && user.FailLogInCount >= passwordErrorLimit - 1)
                {
                    user.UpdateServerLoginLockedout();
                    return EnUserAccountError.PasswordExceededErrorLimit;
                }
                else
                    user.UpdateServerLoginFailed();
                return EnUserAccountError.WrongPassword;
            }
            if (passwordAge != 0 && ((DateTime)user.LastPWUpdate).AddDays(passwordAge) < DateTime.Now)
            {
                return EnUserAccountError.PasswordHasExpired;
            }
            
            user.UpdateServerLoginSuccess();
            user.GetAuthority(machineID);
            if (passwordAge != 0 && ((DateTime)user.LastPWUpdate).AddDays(passwordAge - passwordWarning) < DateTime.Now)
            {
                return EnUserAccountError.PasswordExpireImminent;
            }
            return EnUserAccountError.None;
        }
        public static EnUserAccountError LogInLocal(string ID, string password, int passwordAge, int passwordErrorLimit, int passwordWarning , out User user)
        {
            user = Controls.UserController.SelectLocalSingle(ID, null, null, null, null);
            if (user == null)
            {
                return EnUserAccountError.IDNotExists;
            }
            if (user.UseYN == "N")
            {
                return EnUserAccountError.AccountDeleted;
            }
            if (user.LockYN == "Y")
            {
                return EnUserAccountError.AccountLocked;
            }
            if (!user.UserPW.Equals(DominoFunctions.SimpleAES.SimpleAES.Instance.EncryptToString(password)))
            {
                user.FailLogInCount = user.FailLogInCount == null? 1 : user.FailLogInCount + 1;
                if (passwordErrorLimit != 0 && user.FailLogInCount >= passwordErrorLimit)
                {
                    user.LockYN = "Y";
                    user.FailLogInCount = 0;
                    user.UpdateLocal();
                    return EnUserAccountError.PasswordExceededErrorLimit;
                }
                else
                    user.UpdateLocal();
                return EnUserAccountError.WrongPassword;
            }
            if (passwordAge != 0 && ((DateTime)user.LastPWUpdate).AddDays(passwordAge) < DateTime.Now)
            {
                user.LockYN = "Y";
                user.UpdateLocal();
                return EnUserAccountError.PasswordExpirationPopup;
            }
            user.FailLogInCount = 0;
            user.LastLogIn = DateTime.Now;
            user.UpdateLocal();
            user.GetAuthorityLocal();
            if (passwordAge != 0 && ((DateTime)user.LastPWUpdate).AddDays(passwordAge - passwordWarning) < DateTime.Now)
            {
                return EnUserAccountError.PasswordExpireImminent;
            }
            return EnUserAccountError.None;
        }

        public User Clone()
        {
            return (User)this.MemberwiseClone();
        }
        public void GetAuthority(string machineID)
        {
            try
            {
                string machineDiv = machineID == "DSM" ? "DSM" : Controls.MachineController.SelectServerSingle(this.PlantCode, machineID, null, null, null, null).MachineType;
                if (machineDiv == "LBL" || machineDiv == "PNL") machineDiv = "PM";
                Authority tmp = Controls.AuthorityController.SelectServerSingle(this.PlantCode, this.UserDetail.Single(q => q.MachineID.Equals(machineID)).AuthID, null, machineDiv, null);
                if (tmp != null)
                {
                    this.Authority_Product = (long)tmp.AuthDetailCollection.SingleOrDefault(q => q.MenuID.Equals(EnMenuID.Product.ToString())).MenuAuth;
                    this.Authority_Job = (long)tmp.AuthDetailCollection.SingleOrDefault(q => q.MenuID.Equals(EnMenuID.Job.ToString())).MenuAuth;
                    this.Authority_Printer = (long)tmp.AuthDetailCollection.SingleOrDefault(q => q.MenuID.Equals(EnMenuID.Printer.ToString())).MenuAuth;
                    if (tmp.AuthDetailCollection.SingleOrDefault(q => q.MenuID.Equals(EnMenuID.Vision.ToString())) != null)
                        this.Authority_Vision = (long)tmp.AuthDetailCollection.SingleOrDefault(q => q.MenuID.Equals(EnMenuID.Vision.ToString())).MenuAuth;
                    this.Authority_Setting = (long)tmp.AuthDetailCollection.SingleOrDefault(q => q.MenuID.Equals(EnMenuID.Setting.ToString())).MenuAuth;
                    this.Authority_Line = (long)tmp.AuthDetailCollection.SingleOrDefault(q => q.MenuID.Equals(EnMenuID.Line.ToString())).MenuAuth;
                }
            }
            catch (Exception)
            {
            }
        }
        public void GetAuthorityLocal()
        {
            try
            {
                Authority tmp = Controls.AuthorityController.SelectLocalSingle(this.AuthID, null);
                if (tmp != null)
                {
                    this.Authority_Product = (long)tmp.AuthDetailCollection.SingleOrDefault(q => q.MenuID.Equals(EnMenuID.Product.ToString())).MenuAuth;
                    this.Authority_Job = (long)tmp.AuthDetailCollection.SingleOrDefault(q => q.MenuID.Equals(EnMenuID.Job.ToString())).MenuAuth;
                    this.Authority_Printer = (long)tmp.AuthDetailCollection.SingleOrDefault(q => q.MenuID.Equals(EnMenuID.Printer.ToString())).MenuAuth;
                    if (tmp.AuthDetailCollection.SingleOrDefault(q => q.MenuID.Equals(EnMenuID.Vision.ToString())) != null)
                        this.Authority_Vision = (long)tmp.AuthDetailCollection.SingleOrDefault(q => q.MenuID.Equals(EnMenuID.Vision.ToString())).MenuAuth;
                    this.Authority_Setting = (long)tmp.AuthDetailCollection.SingleOrDefault(q => q.MenuID.Equals(EnMenuID.Setting.ToString())).MenuAuth;
                    this.Authority_Line = (long)tmp.AuthDetailCollection.SingleOrDefault(q => q.MenuID.Equals(EnMenuID.Line.ToString())).MenuAuth;
                }
            }
            catch (Exception)
            {
            }
        }

        public void GetAuthority(EnMachineType type)
        {
            try
            {
                string machineDiv = type == EnMachineType.AggregationStation ? "AG" : "PM";
                Authority tmp = Controls.AuthorityController.SelectLocalSingle(AuthID, null);
                if (tmp != null)
                {
                    this.Authority_Product = (long)tmp.AuthDetailCollection.SingleOrDefault(q => q.MenuID.Equals(EnMenuID.Product.ToString())).MenuAuth;
                    this.Authority_Job = (long)tmp.AuthDetailCollection.SingleOrDefault(q => q.MenuID.Equals(EnMenuID.Job.ToString())).MenuAuth;
                    this.Authority_Printer = (long)tmp.AuthDetailCollection.SingleOrDefault(q => q.MenuID.Equals(EnMenuID.Printer.ToString())).MenuAuth;
                    if (tmp.AuthDetailCollection.SingleOrDefault(q => q.MenuID.Equals(EnMenuID.Vision.ToString())) != null)
                        this.Authority_Vision = (long)tmp.AuthDetailCollection.SingleOrDefault(q => q.MenuID.Equals(EnMenuID.Vision.ToString())).MenuAuth;
                    this.Authority_Setting = (long)tmp.AuthDetailCollection.SingleOrDefault(q => q.MenuID.Equals(EnMenuID.Setting.ToString())).MenuAuth;
                    this.Authority_Line = (long)tmp.AuthDetailCollection.SingleOrDefault(q => q.MenuID.Equals(EnMenuID.Line.ToString())).MenuAuth;
                }
            }
            catch (Exception)
            {
            }
        }
        public long Authority_Product { get; set; }
        public long Authority_Job { get; set; }
        public long Authority_Printer { get; set; }
        public long Authority_Vision { get; set; }
        public long Authority_Setting { get; set; }
        public long Authority_Line { get; set; }

        public bool CheckAuthority(EnUserAuthority_Product product)
        {
            if (IsMaster)
                return true;
            return Convert.ToBoolean(product & (EnUserAuthority_Product)Authority_Product);
        }

        public bool CheckAuthority(EnUserAuthority_Job job)
        {
            if (IsMaster)
                return true;
            return Convert.ToBoolean(job & (EnUserAuthority_Job)Authority_Job);
        }
        public bool CheckAuthority(EnUserAuthority_Printer printer)
        {
            if (IsMaster)
                return true;
            return Convert.ToBoolean(printer & (EnUserAuthority_Printer)Authority_Printer);
        }
        public bool CheckAuthority(EnUserAuthority_Vision vision)
        {
            if (IsMaster)
                return true;
            return Convert.ToBoolean(vision & (EnUserAuthority_Vision)Authority_Vision);
        }
        public bool CheckAuthority(EnUserAuthority_Setting setting)
        {
            if (IsMaster)
                return true;
            return Convert.ToBoolean(setting & (EnUserAuthority_Setting)Authority_Setting);
        }
        public bool CheckAuthority(EnUserAuthority_Line line)
        {
            if (IsMaster)
                return true;
            return Convert.ToBoolean(line & (EnUserAuthority_Line)Authority_Line);
        }
        public bool CheckAuthorityProduct()
        {
            if (IsMaster)
                return true;
            return Authority_Product > 0;
        }

        public bool CheckAuthorityJob()
        {
            if (IsMaster)
                return true;
            return Authority_Job > 0;
        }
        public bool CheckAuthorityPrinter()
        {
            if (IsMaster)
                return true;
            return Authority_Printer > 0;
        }
        public bool CheckAuthorityVision()
        {
            if (IsMaster)
                return true;
            return Authority_Vision > 0;
        }
        public bool CheckAuthoritySetting()
        {
            if (IsMaster)
                return true;
            return Authority_Setting > 0;
        }
        public bool CheckAuthorityLine()
        {
            if (IsMaster)
                return true;
            return Authority_Line > 0;
        }

    }
    [Serializable]
    public class UserDetailCollection : ICollection<UserDetail>
    {

        public UserDetailCollection()
        {
            _item = new List<UserDetail>();
        }
        public UserDetailCollection(UserDetailCollection other)
        {
            _item = new List<UserDetail>(other);
        }
        public UserDetail this[int index]
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
        public UserDetail this[string machineId]
        {
            get
            {
                return _item.Where(x => x.MachineID.Equals(machineId)).FirstOrDefault();
            }
            set
            {
                var tmp = _item.Where(x => x.MachineID.Equals(machineId)).FirstOrDefault();
                tmp = value;
            }
        }
        public void Add(UserDetail detail)
        {
            _item.Add(detail);
        }
        public void AddRange(IEnumerable<UserDetail> list)
        {
            _item.AddRange(list);
        }
        public void RemoveAt(int index)
        {
            _item.RemoveAt(index);
        }
        public bool Remove(UserDetail detail)
        {
            return _item.Remove(detail);
        }
        public void RemoveRange(int index, int count)
        {
            _item.RemoveRange(index, count);
        }

        protected readonly List<UserDetail> _item;
        public IEnumerator<UserDetail> GetEnumerator()
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
        public bool Contains(UserDetail detail)
        {
            return _item.Contains(detail);
        }
        public void CopyTo(UserDetail[] array, int arrayIndex)
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
    }
    [Serializable]
    public class UserDetail
    {
        public string MachineID { get; set; }
        public string AuthID { get; set; }
        public string Reserved1 { get; set; }
        public string Reserved2 { get; set; }
        public string Reserved3 { get; set; }
        public string Reserved4 { get; set; }
        public string Reserved5 { get; set; }
        public UserDetail()
        {

        }
        public UserDetail(object list)
        {
            this.UnionClass(list);
        }
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.MachineID, MachineID);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.AuthID, AuthID);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved1, Reserved1);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved2, Reserved2);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved3, Reserved3);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved4, Reserved4);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved5, Reserved5);
            return sb.ToString();

        }
    }
}
