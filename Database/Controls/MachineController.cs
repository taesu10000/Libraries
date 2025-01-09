using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using System.Data.Entity.Migrations;

namespace DominoDatabase.Controls
{
    public class MachineController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static long TotalPageCount { get; set; }
        public static long TotalCount { get; set; }

        public static List<Machine> SelectServer(string plantCode, string machineID, string machineStatus, string machineType, string machineName, bool? use, int pageIndex = -1, int pageSize = 20, bool RaiseException = false)
        {
            List<Machine> retList = new List<Machine>();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_Machine
                               select a;
                    if (!string.IsNullOrEmpty(plantCode))
                        list = list.Where(q => q.PlantCode.Equals(plantCode));
                    if (!string.IsNullOrEmpty(machineID))
                        list = list.Where(q => q.MachineID.Equals(machineID));
                    if (!string.IsNullOrEmpty(machineStatus))
                        list = list.Where(q => q.MachineStatus.Equals(machineStatus));
                    if (!string.IsNullOrEmpty(machineType))
                        list = list.Where(q => q.MachineType.Equals(machineType));
                    if (!string.IsNullOrEmpty(machineName))
                        list = list.Where(q => q.MachineName.Contains(machineName));
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.UseYN.Equals("Y") : q.UseYN.Equals("N"));
                    foreach (var item in list)
                    {
                        retList.Add(new Machine(item));
                    }
                    if (pageIndex >= 0)
                    {
                        TotalCount = retList.Count();
                        TotalPageCount = Convert.ToInt64(Math.Ceiling(Convert.ToDouble(TotalCount) / Convert.ToDouble(pageSize)));
                        retList = retList.OrderBy(x => x.MachineID.Length).ThenBy(x => x.MachineID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    }
                }
                return retList;
            }
            catch (Exception ex)
            {
                log.InfoFormat("MachineController SelectServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("MachineController SelectServer Exception", ex);
            }
            return retList;
        }
        public static Machine SelectServerSingle(string plantCode, string machineID, string machineStatus, string machineType, string machineName, bool? use, bool RaiseException = false)
        {
            Machine retList = new Machine();
            try
            {
                using (DominoDBServer db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_Machine
                               select a;
                    if (!string.IsNullOrEmpty(plantCode))
                        list = list.Where(q => q.PlantCode.Equals(plantCode));
                    if (!string.IsNullOrEmpty(machineID))
                        list = list.Where(q => q.MachineID.Equals(machineID));
                    if (!string.IsNullOrEmpty(machineStatus))
                        list = list.Where(q => q.MachineStatus.Equals(machineStatus));
                    if (!string.IsNullOrEmpty(machineType))
                        list = list.Where(q => q.MachineType.Equals(machineType));
                    if (!string.IsNullOrEmpty(machineName))
                        list = list.Where(q => q.MachineName.Contains(machineName));
                    if (use != null)
                        list = list.Where(q => (bool)use ? q.UseYN.Equals("Y") : q.UseYN.Equals("N"));
                    var result = list.FirstOrDefault();
                    if (result != null)
                        retList = new Machine(result);
                }
                return retList;
            }
            catch (Exception ex)
            {
                log.InfoFormat("MachineController SelectServerSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("MachineController SelectServerSingle Exception", ex);
            }
            return retList;
        }
        public static bool InsertServer(DSM.Dmn_Machine item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("MachineController InsertServer by {0}, {1}", item.InsertUser, item.ToString());
                    db.Dmn_Machine.AddOrUpdate(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("MachineController InsertServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("MachineController InsertServer Exception", ex);
                return false;
            }
        }
        public static bool UpdateServer(DSM.Dmn_Machine item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("MachineController UpdateServer by {0}, {1}", item.UpdateUser, item.ToString());
                    var tmp = db.Dmn_Machine.First(q => q.PlantCode.Equals(item.PlantCode) && q.MachineID.Equals(item.MachineID));
                    if (tmp.PlantCode.Equals(item.PlantCode) == false && tmp.PlantCode.ToLower().Equals(item.PlantCode.ToLower()))
                            item.PlantCode = tmp.PlantCode;
                    tmp.WriteExistings(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("MachineController UpdateServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("MachineController UpdateServer Exception", ex);
                return false;
            }
        }
        public static bool DeleteServerAll(string plantCode, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("MachineController DeleteServerAll{0} by {1}", plantCode, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Machine where PlantCode = {0}", plantCode);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("MachineController DeleteServerAll Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("MachineController DeleteServerAll Exception", ex);
                return false;
            }

        }
        public static bool DeleteServerSingle(string plantCode, string machineID, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("MachineController DeleteServerSingle {0} {1} by {2}", plantCode, machineID, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Machine where PlantCode = {0} AND MachineID = {1}", plantCode, machineID);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("MachineController DeleteServerSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("MachineController DeleteServerSingle Exception", ex);
                return false;
            }
        }

        public static bool ContainsServer(string plantCode, string machineID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("MachineController ContainsServer {0} {1}", plantCode, machineID);
                    var list = from a in db.Dmn_Machine
                               where a.PlantCode.Equals(plantCode) && a.MachineID.Equals(machineID)
                               select a;
                    return list.Count() != 0;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("MachineController ContainsServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("MachineController ContainsServer Exception", ex);
                return false;
            }
        }
    }
}
