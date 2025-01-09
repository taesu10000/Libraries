using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using System.Data.Entity.Migrations;

namespace DominoDatabase.Controls
{
    public class AuthorityController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static long TotalPageCount { get; set; }
        public static long TotalCount { get; set; }
        public static List<Authority> SelectLocal(string authID, string authName, bool? use = true, int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            List<Authority> retList = new List<Authority>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_Auth_M
                               join b in db.Dmn_Auth_D on a.AuthID equals b.AuthID into bb
                               from b in bb.DefaultIfEmpty()
                               select new { a, b };

                    if (!string.IsNullOrEmpty(authID))
                        list = list.Where(q => q.a.AuthID.Equals(authID));
                    if (!string.IsNullOrEmpty(authName))
                        list = list.Where(q => q.a.AuthName.Equals(authName));
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Y") : q.a.UseYN.Equals("N"));
                    foreach (var item in list)
                    {
                        if (!retList.Any(x => x.AuthID.Equals(item.a.AuthID)))
                            retList.Add(new Authority(item.a));
                        retList.Single(x => x.AuthID.Equals(item.a.AuthID)).AuthDetailCollection.Add(new AuthorityDetail(item.b));
                        
                    }
                    if (pageIndex >= 0)
                    {
                        TotalCount = retList.Count();
                        TotalPageCount = Convert.ToInt64(Math.Ceiling(Convert.ToDouble(TotalCount) / Convert.ToDouble(pageSize)));
                        retList = retList.OrderBy(x => x.SeqNum).ThenBy(x => x.AuthID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    }
                    else
                        retList = retList.OrderBy(x => x.SeqNum).ToList();
                }
                return retList;
            }
            catch (Exception ex)
            {
                log.InfoFormat("AuthorityController SelectLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("AuthorityController SelectLocal Exception", ex);
            }
            return retList;
        }
        public static Authority SelectLocalSingle(string authID, string authName, bool? use = true, bool RaiseException = false)
        {
            List<Authority> retList = new List<Authority>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_Auth_M
                               join b in db.Dmn_Auth_D on a.AuthID equals b.AuthID into bb
                               from b in bb.DefaultIfEmpty()
                               select new { a, b };

                    if (!string.IsNullOrEmpty(authID))
                        list = list.Where(q => q.a.AuthID.Equals(authID));
                    if (!string.IsNullOrEmpty(authName))
                        list = list.Where(q => q.a.AuthName.Equals(authName));
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Y") : q.a.UseYN.Equals("N"));
                    foreach (var item in list)
                    {
                        if (!retList.Any(x => x.AuthID.Equals(item.a.AuthID)))
                            retList.Add(new Authority(item.a));
                        retList.Single(x => x.AuthID.Equals(item.a.AuthID)).AuthDetailCollection.Add(new AuthorityDetail(item.b));

                    }
                }
                return retList.FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.InfoFormat("AuthorityController SelectLocalSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("AuthorityController SelectLocalSingle Exception", ex);
            }
            return retList.FirstOrDefault();
        }
        public static bool InsertLocal(Local.Dmn_Auth_M item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("AuthorityController InsertLocalMaster by {0}", item.InsertUser);
                    db.Dmn_Auth_M.Add(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch(Exception ex)
            {
                log.InfoFormat("AuthorityController InsertLocalMaster Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("AuthorityController InsertLocalMaster Exception" , ex);
                return false;
            }
        }
        public static bool InsertLocal(Local.Dmn_Auth_D item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("AuthorityController InsertLocalDetail by {0}", item.InsertUser);
                    db.Dmn_Auth_D.Add(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("AuthorityController InsertLocalDetail Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("AuthorityController InsertLocalDetail Exception", ex);
                return false;
            }
        }
        public static bool UpdateLocal(Local.Dmn_Auth_M item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("AuthorityController UpdateLocalMaster by {0}", item.UpdateUser);
                    var tmp = db.Dmn_Auth_M.First(q => q.AuthID.Equals(item.AuthID));
                    tmp.WriteExistings(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("AuthorityController UpdateLocalMaster Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("AuthorityController UpdateLocalMaster Exception", ex);
                return false;
            }
        }
        public static bool UpdateLocal(Local.Dmn_Auth_D item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("AuthorityController UpdateLocalDetail by {0}", item.UpdateUser);
                    var tmp = db.Dmn_Auth_D.First(q => q.AuthID.Equals(item.AuthID) && q.MenuID.Equals(item.MenuID));
                    tmp.WriteExistings(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("AuthorityController UpdateLocalDetail Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("AuthorityController UpdateLocalDetail Exception", ex);
                return false;
            }
        }
        public static bool DeleteLocalAll(string userID ,bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("AuthorityController DeleteLocalAll by {0}", userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Auth_D");
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Auth_M");
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("AuthorityController DeleteLocalAll Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("AuthorityController DeleteLocalAll Exception", ex);
                return false;
            }

        }
        public static bool DeleteLocalSingle(string authorityID, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("AuthorityController DeleteLocalSingle {0} by {1}", authorityID, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Auth_D where AuthID = {0}", authorityID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Auth_M where AuthID = {0}", authorityID);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("AuthorityController DeleteLocalSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("AuthorityController DeleteLocalSingle Exception", ex);
                return false;
            }
        }
        public static DateTime? GetLastInsertDateLocal(bool RaiseException = false)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    return db.Dmn_Auth_M.Where(q => q.AuthID != "MfdAdmin" && q.AuthID != "MfdUser" && q.AuthID != "SysAdmin" && q.AuthID != "SysManager").Select(q => q.InsertDate).Max();
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("AuthorityController GetLastInsertDateLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("AuthorityController GetLastInsertDateLocal Exception", ex);
            }
            return null;
        }
        public static DateTime? GetLastUpdateDateLocal(bool RaiseException = false)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    return db.Dmn_Auth_M.Select(q => q.UpdateDate).Max();
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("AuthorityController GetLastUpdateDateLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("AuthorityController GetLastUpdateDateLocal Exception", ex);
            }
            return null;
        }


        public static List<Authority> SelectServer(string plantCode, string authID, string authName, string authDiv, DateTime? insertDateStart, DateTime? insertDateEnd, 
            DateTime? updateDateStart, DateTime? updateDateEnd, bool? use = true, int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            List<Authority> retList = new List<Authority>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_Auth_M
                               join b in db.Dmn_Auth_D on new { a.PlantCode, a.AuthID }  equals new { b.PlantCode, b.AuthID } into bb
                               from b in bb.DefaultIfEmpty()
                               select new { a, b };
                    if (!string.IsNullOrEmpty(plantCode))
                        list = list.Where(q => q.a.PlantCode.Equals(plantCode) && q.b.PlantCode.Equals(plantCode));
                    if (!string.IsNullOrEmpty(authID))
                        list = list.Where(q => q.a.AuthID.Equals(authID));
                    if (!string.IsNullOrEmpty(authName))
                        list = list.Where(q => q.a.AuthName.Equals(authName));
                    if (!string.IsNullOrEmpty(authDiv))
                        list = list.Where(q => q.b.AuthDiv.Equals(authDiv));
                    if (insertDateStart != null)
                        list = list.Where(q => q.a.InsertDate >= insertDateStart.Value);
                    if (insertDateEnd != null)
                        list = list.Where(q => q.a.InsertDate <= insertDateEnd.Value);
                    if (updateDateStart != null)
                        list = list.Where(q => q.a.UpdateDate != null &&  q.a.UpdateDate >= updateDateStart.Value);
                    if (updateDateEnd != null)
                        list = list.Where(q => q.a.UpdateDate != null &&  q.a.UpdateDate <= updateDateEnd.Value);
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Y") : q.a.UseYN.Equals("N"));
                    foreach (var item in list)
                    {
                        if (!retList.Any(x => x.AuthID.Equals(item.a.AuthID)))
                            retList.Add(new Authority(item.a));
                        retList.Single(x => x.AuthID.Equals(item.a.AuthID)).AuthDetailCollection.Add(new AuthorityDetail(item.b));
                    }
                    if (pageIndex >= 0)
                    {
                        TotalCount = retList.Count();
                        TotalPageCount = Convert.ToInt64(Math.Ceiling(Convert.ToDouble(TotalCount) / Convert.ToDouble(pageSize)));
                        retList = retList.OrderBy(x => x.SeqNum).ThenBy(x => x.AuthID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    }
                    else
                        retList = retList.OrderBy(x => x.SeqNum).ToList();
                }
                return retList;
            }
            catch (Exception ex)
            {
                log.InfoFormat("AuthorityController SelectServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("AuthorityController SelectServer Exception", ex);
            }
            return retList;
        }
        public static Authority SelectServerSingle(string plantCode, string authID, string authName, string authDiv, bool? use = true, bool RaiseException = false)
        {
            List<Authority> retList = new List<Authority>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_Auth_M
                               join b in db.Dmn_Auth_D
                               on new { a.PlantCode, a.AuthID } equals new { b.PlantCode, b.AuthID }
                               select new { a, b };

                    if (!string.IsNullOrEmpty(plantCode))
                        list = list.Where(q => q.a.PlantCode.Equals(plantCode) && q.b.PlantCode.Equals(plantCode));
                    if (!string.IsNullOrEmpty(authID))
                        list = list.Where(q => q.a.AuthID.Equals(authID));
                    if (!string.IsNullOrEmpty(authName))
                        list = list.Where(q => q.a.AuthName.Equals(authName));
                    if (!string.IsNullOrEmpty(authDiv))
                        list = list.Where(q => q.b.AuthDiv.Equals(authDiv));
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Y") : q.a.UseYN.Equals("N"));
                    foreach (var item in list)
                    {
                        if (!retList.Any(x => x.AuthID.Equals(item.a.AuthID)))
                            retList.Add(new Authority(item.a));
                        retList.Single(x => x.AuthID.Equals(item.a.AuthID)).AuthDetailCollection.Add(new AuthorityDetail(item.b));
                    }
                }
                return retList.FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.InfoFormat("AuthorityController SelectServerSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("AuthorityController SelectServerSingle Exception", ex);
            }
            return retList.FirstOrDefault();
        }
        public static bool InsertServer(DSM.Dmn_Auth_M item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("AuthorityController InsertServerMaster by {0}, {1}", item.InsertUser, item.ToString());
                    db.Dmn_Auth_M.AddOrUpdate(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("AuthorityController InsertServerMaster Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("AuthorityController InsertServerMaster Exception", ex);
                return false;
            }
        }
        public static bool InsertServer(DSM.Dmn_Auth_D item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("AuthorityController InsertServerDetail by {0}, {1}", item.InsertUser, item.ToString());
                    db.Dmn_Auth_D.AddOrUpdate(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("AuthorityController InsertServerDetail Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("AuthorityController InsertServerDetail Exception", ex);
                return false;
            }
        }
        public static bool UpdateServer(DSM.Dmn_Auth_M item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("AuthorityController UpdateServerMaster by {0}, {1}", item.UpdateUser, item.ToString());
                    var tmp = db.Dmn_Auth_M.First(q => q.PlantCode.Equals(item.PlantCode) && q.AuthID.Equals(item.AuthID));
                    tmp.WriteExistings(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("AuthorityController UpdateServerMaster Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("AuthorityController UpdateServerMaster Exception", ex);
                return false;
            }
        }
        public static bool UpdateServer(DSM.Dmn_Auth_D item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("AuthorityController UpdateServerDetail by {0}, {1}", item.UpdateUser, item.ToString());
                    var tmp = db.Dmn_Auth_D.First(q => q.PlantCode.Equals(item.PlantCode) && q.AuthID.Equals(item.AuthID) && q.AuthDiv.Equals(item.AuthDiv) && q.MenuID.Equals(item.MenuID));
                    tmp.WriteExistings(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("AuthorityController UpdateServerDetail Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("AuthorityController UpdateServerDetail Exception", ex);
                return false;
            }
        }

        public static bool DeleteServerAll(string plantCode, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("AuthorityController DeleteServerAll{0} by {1}", plantCode, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Auth_D where PlantCode = {0}", plantCode);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Auth_M where PlantCode = {0}", plantCode);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("AuthorityController DeleteServerAll Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("AuthorityController DeleteServerAll Exception", ex);
                return false;
            }

        }
        public static bool DeleteServerSingle(string plantCode,  string authorityID, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("AuthorityController DeleteServerSingle {0} {1} by {2}",plantCode , authorityID, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Auth_D where PlantCode = {0} AND AuthID = {1}", plantCode, authorityID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Auth_M where PlantCode = {0} AND AuthID = {1}", plantCode, authorityID);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("AuthorityController DeleteServerSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("AuthorityController DeleteServerSingle Exception", ex);
                return false;
            }
        }
    }
}