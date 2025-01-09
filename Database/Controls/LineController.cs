using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using System.Data.Entity.Migrations;

namespace DominoDatabase.Controls
{
    public class LineController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static long TotalPageCount { get; set; }
        public static long TotalCount { get; set; }

        public static List<Line> SelectServer(string plantCode, string lineID, string JobDetailType, string lineName, bool? use, int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            List<Line> retList = new List<Line>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_Line_M
                               join b in db.Dmn_Line_D on new { a.PlantCode, a.LineID } equals new { b.PlantCode, b.LineID }
                               join c in db.Dmn_Machine on new { b.PlantCode, b.MachineID } equals new { c.PlantCode, c.MachineID } into cc
                               from c in cc.DefaultIfEmpty()
                               join d in db.Dmn_JobOrder_D on new { a.PlantCode, b.LastOrderNo, b.LastSeqNo, b.JobDetailType } equals new { d.PlantCode, LastOrderNo = d.OrderNo, LastSeqNo = d.SeqNo, d.JobDetailType } into dd
                               from d in dd.DefaultIfEmpty()
                               join e in db.Dmn_JobOrder_M on new { a.PlantCode, a.LineID, d.OrderNo, d.SeqNo } equals new { e.PlantCode, e.LineID, e.OrderNo, e.SeqNo } into ee
                               from e in ee.DefaultIfEmpty()
                               join f in db.Dmn_Product_M on new { a.PlantCode, e.ProdCode } equals new { f.PlantCode, f.ProdCode } into ff
                               from f in ff.DefaultIfEmpty()
                               join g in db.Dmn_Product_D on new { a.PlantCode, e.ProdCode, b.JobDetailType } equals new { g.PlantCode, g.ProdCode, g.JobDetailType } into gg
                               from g in gg.DefaultIfEmpty()
                               select new { a, b, c, d, e, f, g };
                    if (!string.IsNullOrEmpty(plantCode))
                        list = list.Where(q => q.a.PlantCode.Equals(plantCode));
                    if (!string.IsNullOrEmpty(JobDetailType))
                        list = list.Where(q => q.b.JobDetailType.Equals(JobDetailType));
                    if (!string.IsNullOrEmpty(lineID))
                        list = list.Where(q => q.a.LineID.Contains(lineID));
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Y") : q.a.UseYN.Equals("N"));
                    list = list.OrderBy(q => q.a.LineID).OrderBy(q => q.b.SeqNo);
                    foreach (var item in list)
                    {
                        if (!retList.Any(x => x.LineID.Equals(item.a.LineID)))
                            retList.Add(new Line(item.a) { Cnt_JobPlan = item.e == null ? null : item.e.Cnt_JobPlan });
                        retList.Single(x => x.LineID.Equals(item.a.LineID)).LineDetail.Add(new LineDetail(item.b)
                        {

                            MachineName = item.c == null? null : item.c.MachineName,
                            MachineStatus = item.c == null ? null : item.c.MachineStatus,
                            Cnt_Good = item.d == null ? null : item.d.Cnt_Good,
                            Cnt_Error = item.d == null ? null : item.d.Cnt_Error,
                            Cnt_Sample = item.d == null ? null : item.d.Cnt_Sample,
                            Cnt_Destroy = item.d == null ? null : item.d.Cnt_Destroy,
                            Cnt_Child = item.d == null ? null : item.d.Cnt_Child,
                            Cnt_Work = item.d == null ? null : item.d.Cnt_Work,
                            Cnt_Parent = item.d == null ? null : item.d.Cnt_Parent,
                            LotNo = item.e == null ? null : item.e.LotNo,
                            ProdName = item.f == null ? null : item.f.ProdName,
                            PackingCount = item.g == null ? null : item.g.PackingCount,
                        }); 
                    }
                    if (pageIndex >= 0)
                    {
                        TotalCount = retList.Count();
                        TotalPageCount = Convert.ToInt64(Math.Ceiling(Convert.ToDouble(TotalCount) / Convert.ToDouble(pageSize)));
                        retList = retList.OrderBy(x => x.LineID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    }
                }
                return retList;
            }
            catch (Exception ex)
            {
                log.InfoFormat("LineController SelectServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("LineController SelectServer Exception", ex);
            }
            return retList;
        }
        public static Line SelectServerSingle(string plantCode, string lineID, string JobDetailType, string lineName, bool? use, bool RaiseException = false)
        {
            List<Line> retList = new List<Line>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_Line_M
                               join b in db.Dmn_Line_D on new { a.PlantCode, a.LineID } equals new { b.PlantCode, b.LineID } 
                               join c in db.Dmn_Machine on new { a.PlantCode, b.MachineID } equals new { c.PlantCode, c.MachineID } 
                               select new { a, b, c };
                    if (!string.IsNullOrEmpty(plantCode))
                        list = list.Where(q => q.a.PlantCode.Equals(plantCode));
                    if (!string.IsNullOrEmpty(JobDetailType))
                        list = list.Where(q => q.b.JobDetailType.Equals(JobDetailType));
                    if (!string.IsNullOrEmpty(lineID))
                        list = list.Where(q => q.a.LineID.Contains(lineID));
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.a.UseYN.Equals("Y") : q.a.UseYN.Equals("N"));
                    foreach (var item in list)
                    {
                        if (!retList.Any(x => x.LineID.Equals(item.a.LineID)))
                            retList.Add(new Line(item.a));
                        retList.Single(x => x.LineID.Equals(item.a.LineID)).LineDetail.Add(new LineDetail(item.b) 
                        { 
                            MachineName = item.c == null? null : item.c.MachineName 
                        });
                    }
                }
                return retList.FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.InfoFormat("LineController SelectServerSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("LineController SelectServerSingle Exception", ex);
            }
            return retList.FirstOrDefault();
        }
        public static bool InsertServer(DSM.Dmn_Line_M item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("LineController InsertServer by {0}, {1}", item.InsertUser, item.ToString());
                    db.Dmn_Line_M.AddOrUpdate(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("LineController InsertServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("LineController InsertServer Exception", ex);
                return false;
            }
        }
        public static bool InsertServer(DSM.Dmn_Line_D item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("LineController InsertServerDetail by {0}, {1}", item.UpdateUser ?? item.InsertUser, item.ToString());
                    db.Dmn_Line_D.AddOrUpdate(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("LineController InsertServerDetail Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("LineController InsertServerDetail Exception", ex);
                return false;
            }
        }
        public static bool UpdateServer(DSM.Dmn_Line_M item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("LineController UpdateServer by {0}, {1}", item.UpdateUser, item.ToString());
                    var tmp = db.Dmn_Line_M.First(q => q.PlantCode.Equals(item.PlantCode) && q.LineID.Equals(item.LineID));
                    tmp.WriteExistings(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("LineController UpdateServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("LineController UpdateServer Exception", ex);
                return false;
            }
        }
        public static bool UpdateServer(DSM.Dmn_Line_D item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("LineController UpdateServerDetail by {0}, {1}", item.UpdateUser, item.ToString());
                    var tmp = db.Dmn_Line_M.First(q => q.PlantCode.Equals(item.PlantCode) && q.LineID.Equals(item.LineID));
                    tmp.WriteExistings(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("LineController UpdateServerDetail Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("LineController UpdateServerDetail Exception", ex);
                return false;
            }
        }
        public static bool UpdateStatusServer(string plantcode, string lineid, string machineid, string lastorderno, string lastseqno, bool RaiseException = false)
        {
            try
            {
                log.InfoFormat("LineController UpdateStatusServer from {0}, {1}, {2}", plantcode, lineid, machineid);
                var updated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                using (var db = new DominoDBServer())
                {
                    var rowcount = db.Database.SqlQuery<int>($@"DECLARE @RCNT INT = 0

                                                                UPDATE	Dmn_Line_M
                                                                SET		UpdateUser = 'System'
		                                                                ,UpdateDate = '{updated}'
                                                                WHERE	PlantCode = '{plantcode}'
		                                                                AND LineID = '{lineid}'

                                                                SET @RCNT = @RCNT + @@ROWCOUNT

                                                                UPDATE	Dmn_Line_D
                                                                SET		UpdateUser = 'System'
		                                                                ,UpdateDate = '{updated}'
		                                                                ,LastOrderNo = '{lastorderno}'
		                                                                ,LastSeqNo = '{lastseqno}'
                                                                WHERE	PlantCode = '{plantcode}'
		                                                                AND LineID = '{lineid}'
		                                                                AND MachineID = '{machineid}'

                                                                SET @RCNT = @RCNT + @@ROWCOUNT

                                                                UPDATE	Dmn_Line_D
                                                                SET		UpdateUser = 'System'
		                                                                ,UpdateDate = '{updated}'
                                                                WHERE	PlantCode = '{plantcode}'
		                                                                AND LineID = '{lineid}'
		                                                                AND MachineID != '{machineid}'

                                                                SET @RCNT = @RCNT + @@ROWCOUNT

                                                                SELECT @RCNT").FirstOrDefault();


                    log.InfoFormat("LineController UpdateStatusServer updated {0} rows.", rowcount);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("LineController UpdateStatusServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("LineController UpdateStatusServer Exception", ex);
                return false;
            }
        }
        public static bool DeleteServerAll(string plantCode, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("LineController DeleteServerAll{0} by {1}", plantCode, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Line where PlantCode = {0}", plantCode);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("LineController DeleteServerAll Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("LineController DeleteServerAll Exception", ex);
                return false;
            }

        }
        public static bool DeleteServerDetail(string plantCode, string lineID, string jobDetailType, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("LineController DeleteServerDetail {0} by {1}", plantCode, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Line_D where PlantCode = {0} AND LineID = {1} AND JobDetailType = {2}", plantCode, lineID, jobDetailType);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("LineController DeleteServerDetail Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("LineController DeleteServerDetail Exception", ex);
                return false;
            }
        }
        public static bool DeleteServerDetailAll(string plantCode, string lineID, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("LineController DeleteServerDetail {0} by {1}", plantCode, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Line_D where PlantCode = {0} AND LineID = {1}", plantCode, lineID);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("LineController DeleteServerDetail Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("LineController DeleteServerDetail Exception", ex);
                return false;
            }
        }
        public static bool DeleteServerSingle(string plantCode, string lineId, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("LineController DeleteServerSingle {0} {1} by {2}", plantCode, lineId, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Line_D where PlantCode = {0} AND LineID = {1}", plantCode, lineId);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Line_M where PlantCode = {0} AND LineID = {1}", plantCode, lineId);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("LineController DeleteServerSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("LineController DeleteServerSingle Exception", ex);
                return false;
            }
        }
    }
}
