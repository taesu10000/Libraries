using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using log4net;

namespace DominoDatabase
{

    public partial class DominoDBLocal : DbContext
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DominoDBLocal()
            : base(System.Configuration.ConfigurationManager.ConnectionStrings["Local"].ToString())
        {
            var type = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
            if (type == null)
                throw new Exception("Do not remove, enures static reference to System.Data.Entity.SqlServer");
        }
        public DominoDBLocal(string ip)
            : base(string.Format(System.Configuration.ConfigurationManager.ConnectionStrings["OtherEquipment"].ToString(), ip))// App=EntityFramework")
        {
            var type = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
            if (type == null)
                throw new Exception("Do not remove, enures static reference to System.Data.Entity.SqlServer");
        }
        public virtual DbSet<Local.Dmn_Auth_D> Dmn_Auth_D { get; set; }
        public virtual DbSet<Local.Dmn_Auth_M> Dmn_Auth_M { get; set; }
        public virtual DbSet<Dmn_CommonCode_D> Dmn_CommonCode_D { get; set; }
        public virtual DbSet<Dmn_CommonCode_M> Dmn_CommonCode_M { get; set; }
        public virtual DbSet<Local.Dmn_JobOrder_AG> Dmn_JobOrder_AG { get; set; }
        public virtual DbSet<Local.Dmn_JobOrder_M> Dmn_JobOrder_M { get; set; }
        public virtual DbSet<Local.Dmn_JobOrder_PM> Dmn_JobOrder_PM { get; set; }
        public virtual DbSet<Local.Dmn_Product_AG> Dmn_Product_AG { get; set; }
        public virtual DbSet<Local.Dmn_Product_M> Dmn_Product_M { get; set; }
        public virtual DbSet<Local.Dmn_Product_PM> Dmn_Product_PM { get; set; }
        public virtual DbSet<Local.Dmn_ReadBarcode> Dmn_ReadBarcode { get; set; }
        public virtual DbSet<Local.Dmn_Serial_Expression> Dmn_Serial_Expression { get; set; }
        public virtual DbSet<Local.Dmn_SerialPool> Dmn_SerialPool { get; set; }
        public virtual DbSet<Local.Dmn_HelpCodePool_M> Dmn_HelpCodePool_M { get; set; }
        public virtual DbSet<Local.Dmn_HelpCodePool_D> Dmn_HelpCodePool_D { get; set; }
        public virtual DbSet<Local.Dmn_User> Dmn_User { get; set; }
        public virtual DbSet<Local.Dmn_VisionResult> Dmn_VisionResult { get; set; }
        public virtual DbSet<Local.Dmn_CustomBarcodeFormat> Dmn_CustomBarcodeFormat { get; set; }
        public virtual DbSet<Local.Dmn_View_PMData> Dmn_View_PMData { get; set; }
        public virtual DbSet<Local.Dmn_View_AGData> Dmn_View_AGData { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Local.Dmn_Auth_D>()
               .HasKey(p => new { p.AuthID, p.MenuID });

            modelBuilder.Entity<Local.Dmn_Auth_M>()
               .HasKey(p => new { p.AuthID });

            modelBuilder.Entity<Dmn_CommonCode_D>()
               .HasKey(p => new { p.CDCode, p.CDCode_Dtl });

            modelBuilder.Entity<Dmn_CommonCode_M>()
               .HasKey(p => new { p.CDCode });

            modelBuilder.Entity<Local.Dmn_JobOrder_AG>()
               .HasKey(p => new { p.OrderNo, p.SeqNo, p.JobDetailType });

            modelBuilder.Entity<Local.Dmn_JobOrder_M>()
               .HasKey(p => new { p.OrderNo, p.SeqNo });

            modelBuilder.Entity<Local.Dmn_JobOrder_PM>()
               .HasKey(p => new { p.OrderNo, p.SeqNo, p.JobDetailType });

            modelBuilder.Entity<Local.Dmn_Product_AG>()
               .HasKey(p => new { p.ProdCode, p.JobDetailType });

            modelBuilder.Entity<Local.Dmn_Product_M>()
               .HasKey(p => new { p.ProdCode });

            modelBuilder.Entity<Local.Dmn_Product_PM>()
               .HasKey(p => new { p.ProdCode, p.JobDetailType });

            modelBuilder.Entity<Local.Dmn_ReadBarcode>()
               .HasKey(p => new { p.ProdStdCode, p.SerialNum });

            modelBuilder.Entity<Local.Dmn_Serial_Expression>()
               .HasKey(p => new { p.SnExpressionID });

            modelBuilder.Entity<Local.Dmn_SerialPool>()
               .HasKey(p => new { p.ProdStdCode, p.SerialNum, p.JobDetailType });

            modelBuilder.Entity<Local.Dmn_HelpCodePool_M>()
                .HasKey(p => new { p.OrderNo, p.SeqNo, p.HelpCode });

            modelBuilder.Entity<Local.Dmn_HelpCodePool_D>()
                .HasKey(p => new { p.ChildProdStdCode, p.ChildSerialNum });

            modelBuilder.Entity<Local.Dmn_User>()
               .HasKey(p => new { p.UserID });

            modelBuilder.Entity<Local.Dmn_VisionResult>()
               .HasKey(p => new { p.OrderNo, p.SeqNo, p.JobDetailType, p.Idx_Insert, p.InsertDate });

            modelBuilder.Entity<Local.Dmn_CustomBarcodeFormat>()
               .HasKey(p => new { p.CustomBarcodeFormatID });

            modelBuilder.Entity<Local.Dmn_Product_PM>()
                .Property(p => p.MinimumWeight)
                .HasPrecision(13, 3);

            modelBuilder.Entity<Local.Dmn_Product_PM>()
                .Property(p => p.MaximumWeight)
                .HasPrecision(13, 3);

            modelBuilder.Entity<Local.Dmn_Product_AG>()
            .Property(p => p.MinimumWeight)
            .HasPrecision(13, 3);

            modelBuilder.Entity<Local.Dmn_Product_AG>()
                .Property(p => p.MaximumWeight)
                .HasPrecision(13, 3);

            base.OnModelCreating(modelBuilder);
        }
    }
    public class MigrateLocalDBConfiguration : System.Data.Entity.Migrations.DbMigrationsConfiguration<DominoDBLocal>
    {
        public MigrateLocalDBConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
    }
}
