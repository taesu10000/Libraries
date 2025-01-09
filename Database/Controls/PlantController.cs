using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using System.Data.Entity.Migrations;

namespace DominoDatabase.Controls
{
    public class PlantController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static long TotalPageCount { get; set; }
        public static long TotalCount { get; set; }

        public static List<Plant> SelectServer(string plantCode, string plantName, bool? use, int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            List<Plant> retList = new List<Plant>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_Plant
                               select a;
                    if (!string.IsNullOrEmpty(plantCode))
                        list = list.Where(q => q.PlantCode.Contains(plantCode));
                    if (!string.IsNullOrEmpty(plantName))
                        list = list.Where(q => q.PlantName.Contains(plantName));
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.UseYN.Equals("Y") : q.UseYN.Equals("N"));
                    foreach (var item in list)
                    {
                        retList.Add(new Plant(item));
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
                log.InfoFormat("PlantController SelectServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("PlantController SelectServer Exception", ex);
            }
            return retList;
        }
        public static Plant SelectServerSingle(string plantCode, string plantName, bool? use, bool RaiseException = false)
        {
            Plant retList = new Plant();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_Plant
                               select a;
                    if (!string.IsNullOrEmpty(plantCode))
                        list = list.Where(q => q.PlantCode.Contains(plantCode));
                    if (!string.IsNullOrEmpty(plantName))
                        list = list.Where(q => q.PlantName.Contains(plantName));
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.UseYN.Equals("Y") : q.UseYN.Equals("N"));
                    var result = list.FirstOrDefault();
                    if (result != null)
                        retList = new Plant(result);
                }
                return retList;
            }
            catch (Exception ex)
            {
                log.InfoFormat("PlantController SelectServerSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("PlantController SelectServerSingle Exception", ex);
            }
            return retList;
        }
        public static bool InsertServer(DSM.Dmn_Plant item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("PlantController InsertServer by {0}, {1}", item.InsertUser, item.ToString());
                    db.Dmn_Plant.AddOrUpdate(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("PlantController InsertServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("PlantController InsertServer Exception", ex);
                return false;
            }
        }
        public static bool UpdateServer(DSM.Dmn_Plant item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("PlantController UpdateServer by {0}, {1}", item.UpdateUser, item.ToString());
                    var tmp = db.Dmn_Plant.First(q => q.PlantCode.Equals(item.PlantCode));
                    tmp.WriteExistings(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("PlantController UpdateServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("PlantController UpdateServer Exception", ex);
                return false;
            }
        }
        public static bool DeleteServerAll(string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("PlantController DeleteServerAll by {0}", userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Plant");
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("PlantController DeleteServerAll Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("PlantController DeleteServerAll Exception", ex);
                return false;
            }

        }
        public static bool DeleteServerSingle(string plantCode, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("PlantController DeleteServerSingle {0} by {1}", plantCode, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Plant where PlantCode = {0}", plantCode);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("PlantController DeleteServerSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("PlantController DeleteServerSingle Exception", ex);
                return false;
            }
        }
    }
}
