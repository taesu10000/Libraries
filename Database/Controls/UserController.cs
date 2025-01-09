using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using DominoFunctions.ExtensionMethod;
using log4net;

namespace DominoDatabase.Controls
{
    public class UserController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static long TotalPageCount { get; set; }
        public static long TotalCount { get; set; }
        public static List<User> SelectLocal(string userID, string userName, bool? locked, string deptCode, bool? use, int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            List<User> retList = new List<User>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_User
                               join b in db.Dmn_Auth_M on a.AuthID equals b.AuthID into bb
                               from b in bb.DefaultIfEmpty()
                               select new { a, b };
                    if (!string.IsNullOrEmpty(userID))
                        list = list.Where(q => q.a.UserID.Contains(userID));
                    if (!string.IsNullOrEmpty(userName))
                        list = list.Where(q => q.a.UserName.Contains(userName));
                    if (locked != null)
                        list = list.Where(q => (bool)locked ? q.a.LockYN.Equals("Y") : q.a.LockYN.Equals("N"));
                    if (!string.IsNullOrEmpty(deptCode))
                        list = list.Where(q => q.a.DeptCode.Contains(deptCode));
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Y") : q.a.UseYN.Equals("N"));
                    foreach (var item in list)
                    {
                        retList.Add(new User(item.a) { AuthorityName = item.b == null ? null : item.b.AuthName });
                    }
                    if (pageIndex >= 0)
                    {
                        TotalCount = retList.Count();
                        TotalPageCount = Convert.ToInt64(Math.Ceiling(Convert.ToDouble(TotalCount) / Convert.ToDouble(pageSize)));
                        retList = retList.OrderBy(x => x.InsertDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    }
                }
                return retList;
            }
            catch (Exception ex)
            {
                log.InfoFormat("UserController SelectLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("UserController SelectLocal Exception", ex);
            }
            return retList;
        }
        public static User SelectLocalSingle(string userID, string userName, bool? locked, string deptCode, bool? use, bool RaiseException = false)
        {
            List<User> retList = new List<User>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_User
                               join b in db.Dmn_Auth_M on a.AuthID equals b.AuthID into bb
                               from b in bb.DefaultIfEmpty()
                               select new { a, b };
                    if (!string.IsNullOrEmpty(userID))
                        list = list.Where(q => q.a.UserID.Equals(userID));
                    if (!string.IsNullOrEmpty(userName))
                        list = list.Where(q => q.a.UserName.Contains(userName));
                    if (locked != null)
                        list = list.Where(q => (bool)locked ? q.a.LockYN.Equals("Y") : q.a.LockYN.Equals("N"));
                    if (!string.IsNullOrEmpty(deptCode))
                        list = list.Where(q => q.a.DeptCode.Contains(deptCode));
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Y") : q.a.UseYN.Equals("N"));
                    foreach (var item in list)
                    {
                        retList.Add(new User(item.a) { AuthorityName = item.b.AuthName });
                    }
                }
                return retList.FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.InfoFormat("UserController SelectLocalSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("UserController SelectLocalSingle Exception", ex);
            }
            return retList.FirstOrDefault();
        }
        public static bool ChangePasswordLocal(Local.Dmn_User item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("UserController ChangePasswordLocal by {0}", item.UpdateUser);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("UserController ChangePasswordLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("UserController ChangePasswordLocal Exception", ex);
                return false;
            }
        }
        public static bool InsertLocal(Local.Dmn_User item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("UserController InsertLocal by {0}", item.InsertUser);
                    db.Dmn_User.Add(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("UserController InsertLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("UserController InsertLocal Exception", ex);
                return false;
            }
        }
        public static bool UpdateLocal(Local.Dmn_User item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("UserController UpdateLocal {0} Updated.", item.UpdateUser);
                    var tmp = db.Dmn_User.FirstOrDefault(q => q.UserID.Equals(item.UserID));
                    if (tmp != null)
                    {
                        tmp.WriteExistings(item);
                        db.SaveChanges();
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("UserController UpdateLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("UserController UpdateLocal Exception", ex);
                return false;
            }
        }
        public static bool DeleteLocalAll(string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("UserController DeleteLocalAll by {0}", userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_User");
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("UserController DeleteLocalAll Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("UserController DeleteLocalAll Exception", ex);
                return false;
            }

        }
        public static bool DeleteLocalSingle(string accountID, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("UserController DeleteLocalSingle {0} by {1}", accountID, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_User where UserID = {0}", accountID);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("UserController DeleteLocalSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("UserController DeleteLocalSingle Exception", ex);
                return false;
            }
        }
        public static DateTime? GetLastInsertDateLocal(bool RaiseException = false)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    return db.Dmn_User.Select(q => q.InsertDate).Max();
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("UserController GetLastInsertDateLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("UserController GetLastInsertDateLocal Exception", ex);
            }
            return null;
        }
        public static DateTime? GetLastUpdateDateLocal(bool RaiseException = false)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    return db.Dmn_User.Select(q => q.UpdateDate).Max();
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("UserController GetLastUpdateDateLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("UserController GetLastUpdateDateLocal Exception", ex);
            }
            return null;
        }
        public static List<User> SelectServer(string plantCode, string userID, string userName, bool? locked, string deptCode, string machineID, DateTime? insertDateStart, DateTime? insertDateEnd,
             DateTime? updateDateStart, DateTime? updateDateEnd, bool? use, int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            List<User> retList = new List<User>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_User_M
                               join b in db.Dmn_User_D on new { a.PlantCode , a.UserID } equals new { b.PlantCode , b.UserID } into bb
                               from b in bb.DefaultIfEmpty()
                               select new { a, b };
                    if (!string.IsNullOrEmpty(plantCode))
                        list = list.Where(q => q.a.PlantCode.Equals(plantCode) && q.b.PlantCode.Equals(plantCode));
                    if (!string.IsNullOrEmpty(userID))
                        list = list.Where(q => q.a.UserID.Contains(userID));
                    if (!string.IsNullOrEmpty(userName))
                        list = list.Where(q => q.a.UserName.Contains(userName));
                    if (locked != null)
                        list = list.Where(q => (bool)locked ? q.a.LockYN.Equals("Y") : q.a.LockYN.Equals("N"));
                    if (!string.IsNullOrEmpty(deptCode))
                        list = list.Where(q => q.a.DeptCode.Contains(deptCode));
                    if (!string.IsNullOrEmpty(machineID))
                        list = list.Where(q => q.b.MachineID.Contains(machineID));
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
                    foreach (var item in list)
                    {
                        if (!retList.Any(x => x.UserID.Equals(item.a.UserID)))
                            retList.Add(new User(item.a));
                        retList.Single(q => q.UserID.Equals(item.a.UserID)).UserDetail.Add(new UserDetail(item.b));
                    }
                    if (pageIndex >= 0)
                    {
                        TotalCount = retList.Count();
                        TotalPageCount = Convert.ToInt64(Math.Ceiling(Convert.ToDouble(TotalCount) / Convert.ToDouble(pageSize)));
                        retList = retList.OrderBy(x => x.InsertDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    }
                }
                return retList;
            }
            catch (Exception ex)
            {
                log.InfoFormat("UserController SelectServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("UserController SelectServer Exception", ex);
            }
            return retList;
        }
        public static User SelectServerSingle(string plantCode, string userID, string userName,  bool? locked, string deptCode, string machineID, bool? use, bool RaiseException = false)
        {
            List<User> retList = new List<User>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_User_M
                               join b in db.Dmn_User_D on new { a.PlantCode, a.UserID } equals new { b.PlantCode, b.UserID } into bb
                               from b in bb.DefaultIfEmpty()
                               select new { a, b };
                    if (!string.IsNullOrEmpty(plantCode))
                        list = list.Where(q => q.a.PlantCode.Equals(plantCode) && q.b.PlantCode.Equals(plantCode));
                    if (!string.IsNullOrEmpty(userID))
                        list = list.Where(q => q.a.UserID.Equals(userID));
                    if (!string.IsNullOrEmpty(userName))
                        list = list.Where(q => q.a.UserName.Contains(userName));
                    if (locked != null)
                        list = list.Where(q => (bool)locked ? q.a.LockYN.Equals("Y") : q.a.LockYN.Equals("N"));
                    if (!string.IsNullOrEmpty(deptCode))
                        list = list.Where(q => q.a.DeptCode.Contains(deptCode));
                    if (!string.IsNullOrEmpty(machineID))
                        list = list.Where(q => q.b.MachineID.Contains(machineID));
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Y") : q.a.UseYN.Equals("N"));
                    foreach (var item in list)
                    {
                        if (!retList.Any(x => x.UserID.Equals(item.a.UserID)))
                            retList.Add(new User(item.a));
                        retList.Single(q => q.UserID.Equals(item.a.UserID)).UserDetail.Add(new UserDetail(item.b));
                    }
                }
                return retList.FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.InfoFormat("UserController SelectServerSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("UserController SelectServerSingle Exception", ex);
            }
            return retList.FirstOrDefault();
        }
        

        public static bool InsertServer(DSM.Dmn_User_D item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("UserController InsertServerDetail by {0}, {1}", item.UpdateUser ?? item.InsertUser, item.ToString());
                    db.Dmn_User_D.AddOrUpdate(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("UserController InsertServerDetail Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("UserController InsertServerDetail Exception", ex);
                return false;
            }
        }
        public static bool InsertServer(IEnumerable<DSM.Dmn_User_D> item, bool RaiseException = false)
        {
            try
            {
                log.InfoFormat("UserController InsertServerDetail by {0}, {1}", item.ElementAt(0).UpdateUser ?? item.ElementAt(0).InsertUser, $"{Environment.NewLine}{item.SerializeObjectToJson()}");
                using (var db = new DominoDBServer())
                {
                    foreach (var i in item)
                    {
                        db.Dmn_User_D.AddOrUpdate(i);
                    }
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("UserController InsertServerDetail Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("UserController InsertServerDetail Exception", ex);
                return false;
            }
        }

        public static bool InsertServer(DSM.Dmn_User_M item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("UserController InsertServerMaster by {0}, {1}", item.InsertUser, item.UserID);
                    db.Dmn_User_M.AddOrUpdate(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("UserController InsertServerMaster Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("UserController InsertServerMaster Exception", ex);
                return false;
            }
        }
        public static bool UpdateServer(DSM.Dmn_User_D item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("UserController UpdateServerDetail by {0}, {1}", item.UpdateUser, item.ToString());
                    var tmp = db.Dmn_User_D.First(q => q.PlantCode.Equals(item.PlantCode) && q.UserID.Equals(item.UserID) && q.MachineID.Equals(item.MachineID));
                    tmp.WriteExistings(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("UserController UpdateServerDetail Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("UserController UpdateServerDetail Exception", ex);
                return false;
            }
        }
        public static bool UpdateServer(IEnumerable<DSM.Dmn_User_D> items, bool RaiseException = false)
        {
            try
            {
                log.InfoFormat("UserController UpdateServerDetail by {0}, {1}", items.ElementAt(0)?.UpdateUser, $"{Environment.NewLine}{items.SerializeObjectToJson()}");
                using (var db = new DominoDBServer())
                {
                    foreach (var item in items)
                    {
                        var tmp = db.Dmn_User_D.First(q => q.PlantCode.Equals(item.PlantCode) && q.UserID.Equals(item.UserID) && q.MachineID.Equals(item.MachineID));
                        tmp.WriteExistings(item);
                    }
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("UserController UpdateServerDetail Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("UserController UpdateServerDetail Exception", ex);
                return false;
            }
        }
        public static bool UpdateServer(DSM.Dmn_User_M item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("UserController UpdateServerMaster by {0}, {1}", item.UpdateUser, item.UserID);
                    var tmp = db.Dmn_User_M.First(q => q.PlantCode.Equals(item.PlantCode) && q.UserID.Equals(item.UserID));
                    tmp.WriteExistings(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("UserController UpdateServerMaster Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("UserController UpdateServerMaster Exception", ex);
                return false;
            }
        }
        public static bool UpdateServerLogin(DSM.Dmn_User_M item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("UserController UpdateServerLogin by {0} {1} {2}", item.PlantCode, item.UserID, item.UserName);
                    var tmp = db.Dmn_User_M.First(q => q.PlantCode.Equals(item.PlantCode) && q.UserID.Equals(item.UserID));
                    tmp.WriteExistings(item);

                    var details = db.Dmn_User_D.Where(x => x.PlantCode == item.PlantCode && x.UserID == item.UserID);
                    foreach(var d in details)
                        d.UpdateDate = item.UpdateDate;

                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("UserController UpdateServerMaster Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("UserController UpdateServerMaster Exception", ex);
                return false;
            }
        }
        public static bool UpdatePasswordServer(DSM.Dmn_User_M item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("UserController UpdatePasswordServer by {0}, {1}", item.UpdateUser, item.UserID);
                    var tmp = db.Dmn_User_M.First(q => q.PlantCode.Equals(item.PlantCode) && q.UserID.Equals(item.UserID));
                    tmp.WriteExistings(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("UserController UpdatePasswordServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("UserController UpdatePasswordServer Exception", ex);
                return false;
            }
        }
        public static bool DeleteServerAll(string plantCode, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("UserController DeleteServerAll {0} by {1}", plantCode,userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_User_M where PlantCode = {0}", plantCode);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_User_D where PlantCode = {0}", plantCode);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("UserController DeleteServerAll Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("UserController DeleteServerAll Exception", ex);
                return false;
            }
        }
        public static bool DeleteServerDetail(string plantCode, string detail, string userID,  bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("UserController DeleteServerDetail {0} by {1}", plantCode, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_User_D where PlantCode = {0} AND UserID = {1}", plantCode, detail);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("UserController DeleteServerDetail Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("UserController DeleteServerDetail Exception", ex);
                return false;
            }
        }

        public static bool DeleteServerSingle(string plantCode, string accountID, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("UserController DeleteServerSingle {0} {1} by {2}", plantCode, accountID, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_User_M where PlantCode = {0} UserID = {1}", plantCode, accountID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_User_D where PlantCode = {0} UserID = {1}", plantCode, accountID);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("UserController DeleteServerSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("UserController DeleteServerSingle Exception", ex);
                return false;
            }
        }

        public static bool DeleteServer(string plantcode, string userid, string deleteuser, IEnumerable<string> machineids, bool RaiseException = false)
        {
            try
            {
                log.InfoFormat("UserController DeleteServerDetail by {0}, {1} {2}", deleteuser, userid, string.Join(", ", machineids));
                var sql = $"DELETE FROM Dmn_User_D where PlantCode = '{plantcode}' and UserID = '{userid}' and MachineID in ('{string.Join("', '", machineids)}')";
                using (var db = new DominoDBServer())
                {
                    db.Database.ExecuteSqlCommand(sql);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("UserController DeleteServerDetail Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("UserController DeleteServerDetail Exception", ex);
                return false;
            }
        }
    }
}
