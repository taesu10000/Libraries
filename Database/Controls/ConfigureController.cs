using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace DominoDatabase.Controls
{
	public class ConfigureController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static List<Configure> SelectServer(string plantCode, bool RaiseException = false)
        {
            List<Configure> retList = new List<Configure>();
            try
            {
                using (var db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_Configure
                               where a.PlantCode.Equals(plantCode)
                               select a;
                    foreach (var item in list)
                        retList.Add(new Configure(item));
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ConfigureController SelectServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                {
                    throw new Exception("ConfigureController SelectServer Exception", ex);
                }
            }
            return retList;
        }
        public static List<Configure> SelectServer(bool RaiseException = false)
        {
            List<Configure> retList = new List<Configure>();
            try
            {
                using (var db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_Configure
                               select a;
                    foreach (var item in list)
                        retList.Add(new Configure(item));
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ConfigureController SelectServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                {
                    throw new Exception("ConfigureController SelectServer Exception", ex);
                }
            }
            return retList;
        }
        public static Configure SelectServerSingle(string plantCode, string configure_ID, bool RaiseException = false)
        {
            List<Configure> retList = new List<Configure>();
            try
            {
                using (var db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_Configure
                               select a;
                    if (!string.IsNullOrEmpty(plantCode))
                        list = list.Where(q => q.PlantCode.Equals(plantCode));
                    if (!string.IsNullOrEmpty(configure_ID))
                        list = list.Where(q => q.Configure_ID.Equals(configure_ID));
                    foreach (var item in list)
                        retList.Add(new Configure(item));
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ConfigureController SelectServerSingle Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                {
                    throw new Exception("ConfigureController SelectServerSingle Exception", ex);
                }
            }
            return retList.FirstOrDefault();
        }
        public static bool InsertServer(DSM.Dmn_Configure item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("ConfigureController InsertServer by {0}, {1}", item.InsertUser, item.ToString());
                    db.Dmn_Configure.AddOrUpdate(item);
					db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ConfigureController InsertServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ConfigureController InsertServer Exception", ex);
                return false;
            }
        }
        public static bool UpdateServer(DSM.Dmn_Configure item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("ConfigureController UpdateServer by {0}, {1}", item.UpdateUser, item.ToString());
                    var tmp = db.Dmn_Configure.First(q => q.PlantCode.Equals(item.PlantCode) && q.Configure_ID.Equals(item.Configure_ID));
                    tmp.WriteExistings(item);
					db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ConfigureController UpdateServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("ConfigureController UpdateServer Exception", ex);
                return false;
            }
        }
        public static bool DeleteServer(string plantCode, string userID, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("ConfigureController DeleteServer {0} by {1}", plantCode, userID);
                    db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Configure where PlantCode = {0}", plantCode);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("ConfigureController DeleteServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("PlantController DeleteServer Exception", ex);
                return false;
            }
        }
		public static bool DeleteServer(DSM.Dmn_Configure item, bool RaiseException = false)
		{
			try
			{
				using (var db = new DominoDBServer())
				{
					log.InfoFormat("ConfigureController DeleteServer {0} by {1}", item.UpdateUser, item.ToString());
					db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Configure where PlantCode = {0} and Configure_ID = {1} and Configure_Value = {2}", item.PlantCode, item.Configure_ID, item.Configure_Value);
					return true;
				}
			}
			catch (Exception ex)
			{
				log.InfoFormat("ConfigureController DeleteServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
				if (RaiseException)
					throw new Exception("PlantController DeleteServer Exception", ex);
				return false;
			}
		}
		public static bool ClearInterfaceDetail(string plantcode, bool RaiseException = false)
		{
			try
			{
				using (var db = new DominoDBServer())
				{
					db.Database.ExecuteSqlCommand("DELETE FROM Dmn_Configure where PlantCode = {0} and Configure_ID like 'InterfaceDetail%'", plantcode);
					return true;
				}
			}
			catch (Exception ex)
			{
				log.InfoFormat("ConfigureController ClearInterfaceDetail Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
				if (RaiseException)
					throw new Exception("PlantController DeleteServer Exception", ex);
				return false;
			}
		}
		public static bool SetMovilitas(string channel, string host, string key, string secret)
        {
            using (var db = new Movilitas())
            {
                try
                {
                    db.Database.ExecuteSqlCommand(string.Format("exec movilitas_key_update '{0}','{1}','{2}','{3}'", channel, host, key, secret));
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
