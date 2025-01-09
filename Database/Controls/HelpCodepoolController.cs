using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using System.Data.Entity.Migrations;
using DominoDatabase.Local;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DominoDatabase.Controls
{
    public class HelpCodepoolController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static List<HelpCodePool> SelectLocal(string helpCode = "", string orderNo = "" , string seqNo = "", string stdCode = "", string   serial = "", string childStdCode = "", string childSerial = "", bool RaiseException = false)
        {
            List<HelpCodePool> retList = new List<HelpCodePool>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_HelpCodePool_M
                               join b in db.Dmn_HelpCodePool_D on new { a.OrderNo, a.SeqNo, a.HelpCode } equals new { b.OrderNo, b.SeqNo, b.HelpCode } into bb
                               from b in bb.DefaultIfEmpty()
                               select new { a, b };
                    if (!string.IsNullOrEmpty(helpCode))
                        list = list.Where(q => q.a.HelpCode.Contains(helpCode));
                    if (!string.IsNullOrEmpty(orderNo))
                        list = list.Where(q => q.a.OrderNo.Contains(orderNo));
                    if (!string.IsNullOrEmpty(seqNo))
                        list = list.Where(q => q.a.SeqNo.Contains(seqNo));                    
                    if (!string.IsNullOrEmpty(stdCode))
                        list = list.Where(q => q.a.ProdStdCode.Contains(stdCode));                    
                    if (!string.IsNullOrEmpty(serial))
                        list = list.Where(q => q.a.SerialNum.Contains(serial));
                    if (!string.IsNullOrEmpty(childStdCode))
                        list = list.Where(q => q.b.ChildProdStdCode.Contains(childStdCode));
                    if (!string.IsNullOrEmpty(childSerial))
                        list = list.Where(q => q.b.ChildSerialNum.Contains(childSerial));

                    var groupByMaster = list.GroupBy(q => q.a);
                    foreach (var master in groupByMaster)
                    {
                        if (!retList.Any(q => q.OrderNo == master.Key.OrderNo && q.SeqNo == master.Key.SeqNo && q.HelpCode == master.Key.HelpCode))
                        {
                            var helpCodeObj = new HelpCodePool(master.Key);
                            foreach (var detail in master)
                            {
                                if (detail.b != null)
                                {
                                    var helpCodeDetailObj = new HelpCodePoolDetail(detail.b);
                                    helpCodeObj.HelpCodePoolDetail.Add(helpCodeDetailObj);
                                }
                            }
                            retList.Add(helpCodeObj);
                        }
                    }
                }
                return retList;
            }
            catch (Exception ex)
            {
                log.InfoFormat("HelpCodepoolController SelectLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("HelpCodepoolController SelectLocal Exception", ex);
            }
            return retList;
        }
        public static string SelectHistory(string childStdCode, string childSerial, bool RaiseException = false)
        {
            string history = "";
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_HelpCodePool_D
                               where a.ChildProdStdCode == childStdCode && a.ChildSerialNum == childSerial
                               orderby  a.UpdateDate descending
                               select a;

                    if (list.FirstOrDefault() != null)
                        history = list.FirstOrDefault().History;
                return history;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("HelpCodepoolController SelectLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("HelpCodepoolController SelectLocal Exception", ex);
            }
            return history;
        }
        public static HelpCodePool SelectLocalMasterQuery(string helpCode, string orderNo, string seqNo)
        {
            using (DominoDBLocal db = new DominoDBLocal())
            {
                string sql = @"SELECT TOP 1 * FROM [DOMINO_DB].[dbo].[Dmn_HelpCodePool_M] where OrderNo = @orderNo and SeqNo = @seqNo and HelpCode = @helpCode";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@orderNo", orderNo),
                    new SqlParameter("@seqNo", seqNo),
                    new SqlParameter("@helpCode", helpCode)
                };
                var result = db.Database.SqlQuery<Dmn_HelpCodePool_M>(sql, parameters).FirstOrDefault();
                if (result != null)
                    return new HelpCodePool(result);
                else
                    return null;
            }
        }

        public static bool IsExist(HelpCodePool item, bool RaiseException = false)
        {
            return IsExist(orderNo: item.OrderNo, seqNo: item.SeqNo, helpCode: item.HelpCode);
        }
        public static bool IsExist(string orderNo = "", string seqNo = "", string helpCode = "", string childStdCode = "", string childSerial = "", bool RaiseException = false)
        {
            try
            {
                return SelectLocal(helpCode: helpCode, orderNo: orderNo, seqNo: seqNo, childStdCode: childStdCode, childSerial: childSerial).Count > 0;
            }
            catch (Exception ex)
            {
                log.InfoFormat("HelpCodepoolController IsExist Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("HelpCodepoolController IsExist Exception", ex);
                return false;
            }
        }
        public static bool IsExistChild(HelpCodePool helpCode, bool RaiseException = false)
        {
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                { 
                    var list = from a in db.Dmn_HelpCodePool_M
                           join b in db.Dmn_HelpCodePool_D on new { a.OrderNo, a.SeqNo, a.HelpCode } equals new { b.OrderNo, b.SeqNo, b.HelpCode } into bb
                           from b in bb.DefaultIfEmpty()
                            where a.OrderNo == helpCode.OrderNo && a.SeqNo == helpCode.SeqNo && a.HelpCode == helpCode.HelpCode
                           select new { a, b };

                    if (helpCode.HelpCodePoolDetail == null || helpCode.HelpCodePoolDetail.Count == 0)
                        return false;

                    foreach (var item in helpCode.HelpCodePoolDetail)
                    {
                        bool notFound = list.Any(q => q.b.ChildSerialNum != item.ChildProdStdCode);
                        if (notFound)
                            return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("HelpCodepoolController IsExist Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("HelpCodepoolController IsExist Exception", ex);
                return false;
            }
        }
        public static bool InsertLocal(HelpCodePool helpCode, bool RaiseException = false)
        {
            try
            {
                log.InfoFormat("HelpCodepoolController InsertLocal by {0} : {1}", helpCode.UpdateUser, helpCode.HelpCode);
                var master = new Dmn_HelpCodePool_M(helpCode);
                List<Dmn_HelpCodePool_D> details = new List<Dmn_HelpCodePool_D>();
                foreach (var item in helpCode.HelpCodePoolDetail)
                {
                    var detail = new Dmn_HelpCodePool_D(item)
                    {
                        HelpCode = master.HelpCode,
                        OrderNo = master.OrderNo,
                        SeqNo = master.SeqNo,
                    };
                    details.Add(detail);
                }

                using (var db = new DominoDBLocal())
                {
                    db.Dmn_HelpCodePool_M.Add(master);
                    db.Dmn_HelpCodePool_D.AddRange(details);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("HelpCodepoolController InsertLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("HelpCodepoolController InsertLocal Exception", ex);
                return false;
            }
        }
        public static bool UpdateMasterLocal(HelpCodePool helpCode, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    string updateSQL = @"UPDATE [DOMINO_DB].[dbo].[Dmn_HelpCodePool_M] SET
                                     ProdStdCode = @ProdStdCode,
                                     SerialNum = @SerialNum,
                                     UseYN = @UseYN,
                                     Status = @Status,
                                     FilePath = @FilePath,
                                     InsertUser = @InsertUser,
                                     InsertDate = @InsertDate,
                                     UpdateUser = @UpdateUser,
                                     UpdateDate = @UpdateDate
                                     WHERE OrderNo = @OrderNo AND SeqNo = @SeqNo AND HelpCode = @HelpCode AND idx_Insert = @idx_Insert";
                    SqlParameter[] updateParameters = new SqlParameter[]
                    {
                    new SqlParameter("@OrderNo", helpCode.OrderNo),
                    new SqlParameter("@SeqNo", helpCode.SeqNo),
                    new SqlParameter("@HelpCode", helpCode.HelpCode),
                    new SqlParameter("@idx_Insert", helpCode.idx_Insert.HasValue ? (object)helpCode.idx_Insert : DBNull.Value),
                    new SqlParameter("@ProdStdCode", helpCode.ProdStdCode),
                    new SqlParameter("@SerialNum", helpCode.SerialNum),
                    new SqlParameter("@UseYN", helpCode.UseYN),
                    new SqlParameter("@Status", helpCode.Status),
                    new SqlParameter("@FilePath", helpCode.FilePath),
                    new SqlParameter("@InsertUser", helpCode.InsertUser),
                    new SqlParameter("@InsertDate", helpCode.InsertDate.HasValue ? (object)helpCode.InsertDate : DBNull.Value),
                    new SqlParameter("@UpdateUser", helpCode.UpdateUser),
                    new SqlParameter("@UpdateDate", helpCode.UpdateDate.HasValue ? (object)helpCode.UpdateDate : DBNull.Value)
                    };
                    db.Database.ExecuteSqlCommand(updateSQL, updateParameters);
                    return true;
                }
            }
            catch(Exception ex)
            {
                log.InfoFormat("SerialpoolController ExtractSerialPool Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public static bool UpdateLocal(HelpCodePool helpCode, bool RaiseException = false)
        {
            try
            {
                log.InfoFormat("HelpCodepoolController UpdateLocal by {0} : {1}", helpCode.UpdateUser, helpCode.HelpCode);
                var master = new Dmn_HelpCodePool_M(helpCode);
                List<Dmn_HelpCodePool_D> details = new List<Dmn_HelpCodePool_D>();
                foreach (var item in helpCode.HelpCodePoolDetail)
                {
                    var detail = new Dmn_HelpCodePool_D(item)
                    {
                        HelpCode = master.HelpCode,
                        OrderNo = master.OrderNo,
                        SeqNo = master.SeqNo,
                    };
                    details.Add(detail);
                }

                using (var db = new DominoDBLocal())
                {
                    db.Dmn_HelpCodePool_M.AddOrUpdate(master);
                    foreach (var item in details)
                        db.Dmn_HelpCodePool_D.AddOrUpdate(item);

                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("HelpCodepoolController UpdateLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("HelpCodepoolController UpdateLocal Exception", ex);
                return false;
            }
        }
        public static bool DeleteLocal(string orderNo, string seqNo, string helpCode = "", string userID = "System", bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("HelpCodepoolController DeleteLocal {0} by {1}", helpCode, userID);
                    var list = from a in db.Dmn_HelpCodePool_M
                               join b in db.Dmn_HelpCodePool_D on new { a.OrderNo, a.SeqNo, a.HelpCode } equals new { b.OrderNo, b.SeqNo, b.HelpCode } into bb
                               from b in bb.DefaultIfEmpty()
                               where a.OrderNo == orderNo && a.SeqNo == seqNo
                               select new { a, b };

                    if (string.IsNullOrEmpty(helpCode) == false)
                        list = list.Where(q => q.a.HelpCode == helpCode);

                    var master = list.Select(q => q.a);
                    var detail = list.Where(q => q.b != null).Select(q => q.b);
                    db.Dmn_HelpCodePool_M.RemoveRange(master);
                    db.Dmn_HelpCodePool_D.RemoveRange(detail);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("HelpCodepoolController DeleteLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("HelpCodepoolController DeleteLocal Exception", ex);
                return false;
            }
        }
        public static bool DeleteLocalByChild(string userID, string childStdCode, string childSerial, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("HelpCodepoolController DeleteLocal {0}{1} by {2}", childStdCode, childSerial, userID);
                    var list = from a in db.Dmn_HelpCodePool_M
                               join b in db.Dmn_HelpCodePool_D on new { a.OrderNo, a.SeqNo, a.HelpCode } equals new { b.OrderNo, b.SeqNo, b.HelpCode } into bb
                               from b in bb.DefaultIfEmpty()
                               where b.ChildProdStdCode == childStdCode && b.ChildSerialNum == childSerial
                               select new { a, b };

                    var master = list.Select(q => q.a);
                    var detail = list.Select(q => q.b);
                    db.Dmn_HelpCodePool_M.RemoveRange(master);
                    db.Dmn_HelpCodePool_D.RemoveRange(detail);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("HelpCodepoolController DeleteLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("HelpCodepoolController DeleteLocal Exception", ex);
                return false;
            }
        }
        public static bool GetSerialIndex(string orderNo, string seqNo, out int index)
        {
            index = 0;
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_HelpCodePool_M
                               where a.OrderNo.Equals(orderNo) && a.SeqNo.Equals(seqNo)
                               select a;
                    if (list.Count() > 0 && list.Select(q => q.idx_Insert) != null)
                        index = (int)list.Select(q => q.idx_Insert).Max();

                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("HelpCodepoolController GetSerialIndex Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
    }
}
