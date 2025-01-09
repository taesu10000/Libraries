using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using DominoDatabase.BTS;

namespace DominoDatabase
{
    public class DominoDBMiddleware : DbContext
    {
        public DominoDBMiddleware(string connectionStr)
          : base(connectionStr)
        {
            var type = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
            if (type == null)
                throw new Exception("Do not remove, enures static reference to System.Data.Entity.SqlServer");
        }
        public DominoDBMiddleware(string connectionStr, string ip)
            : base(string.Format(connectionStr, ip))
        {
            var type = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
            if (type == null)
                throw new Exception("Do not remove, enures static reference to System.Data.Entity.SqlServer");
        }
    }
    public class Movilitas : DominoDBMiddleware
    {
        public Movilitas()
            : base(System.Configuration.ConfigurationManager.ConnectionStrings["Movilitas"].ToString())
        {
            var type = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
            if (type == null)
                throw new Exception("Do not remove, enures static reference to System.Data.Entity.SqlServer");
        }
    }
    public class DominoBTS : DominoDBMiddleware
    {
        public DominoBTS(string ip)
            : base(System.Configuration.ConfigurationManager.ConnectionStrings["BTS"].ToString(), ip)
        {
            var type = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
            if (type == null)
                throw new Exception("Do not remove, enures static reference to System.Data.Entity.SqlServer");
        }
        public virtual DbSet<tDsmSerialPool> tDsmSerialPool { get; set; }
    }
}
