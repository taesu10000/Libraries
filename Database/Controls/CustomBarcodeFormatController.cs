using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using System.Data.Entity.Migrations;

namespace DominoDatabase.Controls
{
    public class CustomBarcodeFormatController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static long TotalPageCount { get; set; }
        public static long TotalCount { get; set; }
        public static List<CustomBarcodeFormat> SelectLocal(string formatId, int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            List<CustomBarcodeFormat> retList = new List<CustomBarcodeFormat>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_CustomBarcodeFormat
                               select a;
                    if (!string.IsNullOrEmpty(formatId))
                        list = list.Where(q => q.CustomBarcodeFormatID.Contains(formatId));
                    foreach(var item in list)
                    {
                        retList.Add(new CustomBarcodeFormat(item));
                    }
                    if(pageIndex >= 0)
                    {
                        TotalCount = retList.Count();
                        TotalPageCount = Convert.ToInt64(Math.Ceiling(Convert.ToDouble(TotalCount) / Convert.ToDouble(pageSize)));
                        retList = retList.OrderBy(q => q.CustomBarcodeFormatID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    }
                    return retList;
                }
            }
            catch(Exception ex)
            {
                log.InfoFormat("CustomBarcodeFormatController SelectLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("CustomBarcodeFormatController SelectLocal Exception", ex);
            }
            return retList;
        }
        public static CustomBarcodeFormat SelectLocalSingle(string formatID, bool RaiseException = false)
        {
            List<CustomBarcodeFormat> retList = new List<CustomBarcodeFormat>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_CustomBarcodeFormat
                               select a;
                    if (!string.IsNullOrEmpty(formatID))
                        list = list.Where(q => q.CustomBarcodeFormatID.Equals(formatID));
                    foreach(var item in list)
                    {
                        retList.Add(new CustomBarcodeFormat(item));
                    }
                    return retList.FirstOrDefault();
                }
            }
            catch(Exception ex)
            {
                log.InfoFormat("CustomBarcodeFormatController SelectLocalSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("CustomBarcodeFormatController SelectLocalSingle Exception", ex);
            }
            return retList.FirstOrDefault();
        }
        public static bool InsertLocal(Local.Dmn_CustomBarcodeFormat item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("CustomBarcodeFormatController InsertLocal by {0}, {1}", item.InsertUser, item.ToString());
                    db.Dmn_CustomBarcodeFormat.Add(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch(Exception ex)
            {
                log.InfoFormat("CustomBarcodeFormatController InsertLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("CustomBarcodeFormatController InsertLocal Exception", ex);
                return false;
            }
        }
        public static bool UpdateLocal(Local.Dmn_CustomBarcodeFormat item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("CustomBarcodeFormatController UpdateLocal by {0}, {1}", item.UpdateUser, item.ToString());
                    var tmp = db.Dmn_CustomBarcodeFormat.First(q => q.CustomBarcodeFormatID.Equals(item.CustomBarcodeFormatID));
                    tmp.WriteExistings(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch(Exception ex)
            {
                log.InfoFormat("CustomBarcodeFormatController UpdateLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("CustomBarcodeFormatController UpdateLocal Exception", ex);
                return false;
            }
        }
        public static bool DeleteLocalAll(string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("CustomBarcodeFormatController DeleteLocalAll by {0}", userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_CustomBarcodeFormat");
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("CustomBarcodeFormatController DeleteLocalAll Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("CustomBarcodeFormatController DeleteLocalAll Exception", ex);
                return false;
            }
        }
        public static bool DeleteLocalSingle(string expressionID, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("CustomBarcodeFormatController DeleteLocalSingle {0} by {1}", expressionID, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_CustomBarcodeFormat where CustomBarcodeFormatID = {0}", expressionID);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("CustomBarcodeFormatController DeleteLocalSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("CustomBarcodeFormatController DeleteLocalSingle Exception", ex);
                return false;
            }
        }
        public static DateTime? GetLastInsertDateLocal(bool RaiseException = false)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    return db.Dmn_CustomBarcodeFormat.Select(q => q.InsertDate).Max();
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("CustomBarcodeFormatController GetLastInsertDateLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("CustomBarcodeFormatController GetLastInsertDateLocal Exception", ex);
            }
            return null;
        }
        public static DateTime? GetLastUpdateDateLocal(bool RaiseException = false)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    return db.Dmn_CustomBarcodeFormat.Select(q => q.UpdateDate).Max();
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("CustomBarcodeFormatController GetLastUpdateDateLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("CustomBarcodeFormatController GetLastUpdateDateLocal Exception", ex);
            }
            return null;
        }
        public static List<CustomBarcodeFormat> SelectServer(string plantCode, string formatID, DateTime? insertDateStart, DateTime? insertDateEnd,
            DateTime? updateDateStart, DateTime? updateDateEnd, int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            List<CustomBarcodeFormat> retList = new List<CustomBarcodeFormat>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_CustomBarcodeFormat
                               select a;
                    if (!string.IsNullOrEmpty(plantCode))
                        list = list.Where(q => q.PlantCode.Equals(plantCode));
                    if (!string.IsNullOrEmpty(formatID))
                        list = list.Where(q => q.CustomBarcodeFormatID.Contains(formatID));
                    if (insertDateStart != null)
                        list = list.Where(q => q.InsertDate >= insertDateStart.Value);
                    if (insertDateEnd != null)
                        list = list.Where(q => q.InsertDate <= insertDateEnd.Value);
                    if (updateDateStart != null)
                        list = list.Where(q => q.UpdateDate != null && q.UpdateDate >= updateDateStart.Value);
                    if (updateDateEnd != null)
                        list = list.Where(q => q.UpdateDate != null && q.UpdateDate <= updateDateEnd.Value);
                    foreach(var item in list)
                    {
                        retList.Add(new CustomBarcodeFormat(item));
                    }
                    if (pageIndex >= 0)
                    {
                        TotalCount = retList.Count();
                        TotalPageCount = Convert.ToInt64(Math.Ceiling(Convert.ToDouble(TotalCount) / Convert.ToDouble(pageSize)));
                        retList = retList.OrderBy(q => q.CustomBarcodeFormatID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    }
                    else
                        retList = retList.OrderBy(q => q.CustomBarcodeFormatID).ToList();
                }
                return retList;
            }
            catch(Exception ex)
            {
                log.InfoFormat("CustomBarcodeFormatController SelectServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("CustomBarcodeFormatController SelectServer Exception", ex);
            }
            return retList;
        }
        public static CustomBarcodeFormat SelectServerSingle(string plantCode, string formatID, bool RaiseException = false)
        {
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_CustomBarcodeFormat
                               select a;
                    if (!string.IsNullOrEmpty(plantCode))
                        list = list.Where(q => q.PlantCode.Equals(plantCode));
                    if (!string.IsNullOrEmpty(formatID))
                        list = list.Where(q => q.CustomBarcodeFormatID.Contains(formatID));
                    return new CustomBarcodeFormat(list.FirstOrDefault());
                }
            }
            catch(Exception ex)
            {
                log.InfoFormat("CustomBarcodeFormatController SelectServerSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("CustomBarcodeFormatController SelectServerSingle Exception", ex);
            }
            return new CustomBarcodeFormat();
        }
        public static bool InsertServer(DSM.Dmn_CustomBarcodeFormat item, bool RaiseException =false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("CustomBarcodeFormatController InsertServer by {0}, {1}", item.InsertUser, item.ToString());
                    db.Dmn_CustomBarcodeFormat.Add(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch(Exception ex)
            {
                log.InfoFormat("CustomBarcodeFormatController InsertServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("CustomBarcodeFormatController InsertServer Exception", ex);
                return false;
            }
        }
        public static bool UpdateServer(DSM.Dmn_CustomBarcodeFormat item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("CustomBarcodeFormatController UpdateServer by {0}, {1}", item.UpdateUser, item.ToString());
                    var tmp = db.Dmn_CustomBarcodeFormat.First(q => q.PlantCode.Equals(item.PlantCode) && q.CustomBarcodeFormatID.Equals(item.CustomBarcodeFormatID));
                    tmp.WriteExistings(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch(Exception ex)
            {
                log.InfoFormat("CustomBarcodeFormatController UpdateServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("CustomBarcodeFormatController UpdateServer Exception", ex);
                return false;
            }
        }
        public static bool DeleteServerAll(string plantCode, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("CustomBarcodeFormatController DeleteServerAll {0}", userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_CustomBarcodeFormat where PlantCode = {0}", plantCode);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("CustomBarcodeFormatController DeleteServerAll Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("CustomBarcodeFormatController DeleteServerAll Exception", ex);
                return false;
            }
        }
        public static bool DeleteServerSingle(string plantCode, string formatID, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("CustomBarcodeFormatController DeleteServerSingle {0} {1} by {2}", plantCode, formatID, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_CustomBarcodeFormat where PlantCode = {0} AND CustomBarcodeFormatID = {1}", plantCode, formatID);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("CustomBarcodeFormatController DeleteServerSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("CustomBarcodeFormatController DeleteServerSingle Exception", ex);
                return false;
            }
        }
    }
}
