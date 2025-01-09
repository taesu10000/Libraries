using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using System.Data.Entity.Migrations;

namespace DominoDatabase.Controls
{
    public class SerialExpressionController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static long TotalPageCount { get; set; }
        public static long TotalCount { get; set; }
        public static List<SerialExpression> SelectLocal(string expressionID, int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            List<SerialExpression> retList = new List<SerialExpression>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_Serial_Expression
                               select a;
                    if (!string.IsNullOrEmpty(expressionID))
                        list = list.Where(q => q.SnExpressionID.Contains(expressionID));
                    foreach (var item in list)
                    {
                        retList.Add(new SerialExpression(item));
                    }
                    if (pageIndex >= 0)
                    {
                        TotalCount = retList.Count();
                        TotalPageCount = Convert.ToInt64(Math.Ceiling(Convert.ToDouble(TotalCount) / Convert.ToDouble(pageSize)));
                        retList = retList.OrderBy(x => x.SeqNum).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    }
                }
                return retList;
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialExpressionController SelectLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialExpressionController SelectLocal Exception", ex);
            }
            return retList;
        }
        public static SerialExpression SelectLocalSingle(string expressionID, bool RaiseException = false)
        {
            List<SerialExpression> retList = new List<SerialExpression>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_Serial_Expression
                               select a;
                    if (!string.IsNullOrEmpty(expressionID))
                        list = list.Where(q => q.SnExpressionID.Contains(expressionID));
                    foreach (var item in list)
                    {
                        retList.Add(new SerialExpression(item));
                    }
                }
                return retList.FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialExpressionController SelectLocalSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialExpressionController SelectLocalSingle Exception", ex);
            }
            return retList.FirstOrDefault();
        }
        public static bool InsertLocal(Local.Dmn_Serial_Expression item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("SerialExpressionController InsertLocal by {0}, {1}", item.InsertUser, item.ToString());
                    db.Dmn_Serial_Expression.Add(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialExpressionController InsertLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialExpressionController InsertLocal Exception", ex);
                return false;
            }
        }
        public static bool UpdateLocal(Local.Dmn_Serial_Expression item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("SerialExpressionController UpdateLocal by {0}, {1}", item.UpdateUser, item.ToString());
                    var tmp = db.Dmn_Serial_Expression.First(q => q.SnExpressionID.Equals(item.SnExpressionID));
                    tmp.WriteExistings(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialExpressionController UpdateLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialExpressionController UpdateLocal Exception", ex);
                return false;
            }
        }
        public static bool DeleteLocalAll(string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("SerialExpressionController DeleteLocalAll by {0}", userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Serial_Expression");
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialExpressionController DeleteLocalAll Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialExpressionController DeleteLocalAll Exception", ex);
                return false;
            }
        }
        public static bool DeleteLocalSingle(string expressionID, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("SerialExpressionController DeleteLocalSingle {0} by {1}", expressionID, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Serial_Expression where SnExpressionID = {0}", expressionID);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialExpressionController DeleteLocalSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialExpressionController DeleteLocalSingle Exception", ex);
                return false;
            }
        }
        public static DateTime? GetLastInsertDateLocal(bool RaiseException = false)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    return db.Dmn_Serial_Expression.Where(q => q.SnExpressionID != "Default").Select(q => q.InsertDate).Max();
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialExpressionController GetLastInsertDateLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialExpressionController GetLastInsertDateLocal Exception", ex);
            }
            return null;
        }
        public static DateTime? GetLastUpdateDateLocal(bool RaiseException = false)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    return db.Dmn_Serial_Expression.Where(q => q.SnExpressionID != "Default").Select(q => q.UpdateDate).Max();
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialExpressionController GetLastUpdateDateLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialExpressionController GetLastUpdateDateLocal Exception", ex);
            }
            return null;
        }
        public static List<SerialExpression> SelectServer(string plantCode, string expressionID, DateTime? insertDateStart, DateTime? insertDateEnd,
            DateTime? updateDateStart, DateTime? updateDateEnd, int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            List<SerialExpression> retList = new List<SerialExpression>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_Serial_Expression
                               select a;
                    if (!string.IsNullOrEmpty(plantCode))
                        list = list.Where(q => q.PlantCode.Equals(plantCode));
                    if (!string.IsNullOrEmpty(expressionID))
                        list = list.Where(q => q.SnExpressionID.Contains(expressionID));
                    if (insertDateStart != null)
                        list = list.Where(q => q.InsertDate >= insertDateStart.Value);
                    if (insertDateEnd != null)
                        list = list.Where(q => q.InsertDate <= insertDateEnd.Value);
                    if (updateDateStart != null)
                        list = list.Where(q => q.UpdateDate != null && q.UpdateDate >= updateDateStart.Value);
                    if (updateDateEnd != null)
                        list = list.Where(q => q.UpdateDate != null && q.UpdateDate <= updateDateEnd.Value);
                    foreach (var item in list)
                    {
                        retList.Add(new SerialExpression(item));
                    }
                    if (pageIndex >= 0)
                    {
                        TotalCount = retList.Count();
                        TotalPageCount = Convert.ToInt64(Math.Ceiling(Convert.ToDouble(TotalCount) / Convert.ToDouble(pageSize)));
                        retList = retList.OrderBy(x => x.SeqNum).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    }
                    else
                        retList = retList.OrderBy(x => x.SeqNum).ToList();
                }
                return retList;
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialExpressionController SelectServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialExpressionController SelectServer Exception", ex);
            }
            return retList;
        }
        public static SerialExpression SelectServerSingle(string plantCode, string expressionID, bool RaiseException = false)
        {
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_Serial_Expression
                               select a;
                    if (!string.IsNullOrEmpty(plantCode))
                        list = list.Where(q => q.PlantCode.Equals(plantCode));
                    if (!string.IsNullOrEmpty(expressionID))
                        list = list.Where(q => q.SnExpressionID.Contains(expressionID));
                    return new SerialExpression(list.FirstOrDefault());
				}
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialExpressionController SelectServerSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialExpressionController SelectServerSingle Exception", ex);
            }
            return new SerialExpression();
        }
        public static bool InsertServer(DSM.Dmn_Serial_Expression item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("SerialExpressionController InsertServer by {0}, {1}", item.InsertUser, item.ToString());
                    db.Dmn_Serial_Expression.AddOrUpdate(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialExpressionController InsertServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialExpressionController InsertServer Exception", ex);
                return false;
            }
        }
        public static bool UpdateServer(DSM.Dmn_Serial_Expression item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("SerialExpressionController UpdateServer by {0}, {1}", item.UpdateUser, item.ToString());
                    var tmp = db.Dmn_Serial_Expression.First(q => q.PlantCode.Equals(item.PlantCode) && q.SnExpressionID.Equals(item.SnExpressionID));
                    tmp.WriteExistings(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialExpressionController UpdateServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialExpressionController UpdateServer Exception", ex);
                return false;
            }
        }

        public static bool DeleteServerAll(string plantCode, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("SerialExpressionController DeleteServerAll {0}", userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Serial_Expression where PlantCode = {0}", plantCode);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialExpressionController DeleteServerAll Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialExpressionController DeleteServerAll Exception", ex);
                return false;
            }

        }
        public static bool DeleteServerSingle(string plantCode, string expressionID, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("SerialExpressionController DeleteServerSingle {0} {1} by {2}", plantCode, expressionID, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Serial_Expression where PlantCode = {0} AND SnExpressionID = {1}", plantCode, expressionID);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("SerialExpressionController DeleteServerSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SerialExpressionController DeleteServerSingle Exception", ex);
                return false;
            }
        }

    }
}
