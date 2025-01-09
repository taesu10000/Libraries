using log4net;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;

namespace DominoDatabase
{

	public partial class DominoDBServer : DbContext
	{
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		public DominoDBServer()
			: base(System.Configuration.ConfigurationManager.ConnectionStrings["DSM"].ToString())
		{
			((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 600;
			var type = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
			if (type == null)
				throw new Exception("Do not remove, enures static reference to System.Data.Entity.SqlServer");
		}
		public virtual DbSet<DSM.Dmn_Auth_D> Dmn_Auth_D { get; set; }
		public virtual DbSet<DSM.Dmn_Auth_M> Dmn_Auth_M { get; set; }
		public virtual DbSet<Dmn_CommonCode_D> Dmn_CommonCode_D { get; set; }
		public virtual DbSet<Dmn_CommonCode_M> Dmn_CommonCode_M { get; set; }
		public virtual DbSet<DSM.Dmn_Configure> Dmn_Configure { get; set; }
		public virtual DbSet<DSM.Dmn_JobOrder_D> Dmn_JobOrder_D { get; set; }
		public virtual DbSet<DSM.Dmn_JobOrder_M> Dmn_JobOrder_M { get; set; }
		public virtual DbSet<DSM.Dmn_Product_D> Dmn_Product_D { get; set; }
		public virtual DbSet<DSM.Dmn_Product_M> Dmn_Product_M { get; set; }
		public virtual DbSet<DSM.Dmn_ReadBarcode> Dmn_ReadBarcode { get; set; }
		public virtual DbSet<DSM.Dmn_Serial_Expression> Dmn_Serial_Expression { get; set; }
		public virtual DbSet<DSM.Dmn_SerialPool> Dmn_SerialPool { get; set; }
		public virtual DbSet<DSM.Dmn_User_D> Dmn_User_D { get; set; }
		public virtual DbSet<DSM.Dmn_User_M> Dmn_User_M { get; set; }
		public virtual DbSet<DSM.Dmn_VisionResult> Dmn_VisionResult { get; set; }
		public virtual DbSet<DSM.Dmn_Line_M> Dmn_Line_M { get; set; }
		public virtual DbSet<DSM.Dmn_Line_D> Dmn_Line_D { get; set; }
		public virtual DbSet<DSM.Dmn_Machine> Dmn_Machine { get; set; }
		public virtual DbSet<DSM.Dmn_Plant> Dmn_Plant { get; set; }
        public virtual DbSet<DSM.Dmn_CustomBarcodeFormat> Dmn_CustomBarcodeFormat { get; set; }
        public virtual DbSet<DSM.Dmn_View_DSMData> Dmn_View_DSMDatas { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
            modelBuilder.Entity<DSM.Dmn_Auth_D>()
			   .HasKey(p => new { p.PlantCode, p.AuthDiv, p.AuthID, p.MenuID });

			modelBuilder.Entity<DSM.Dmn_Auth_M>()
			   .HasKey(p => new { p.PlantCode, p.AuthID });

			modelBuilder.Entity<Dmn_CommonCode_D>()
			   .HasKey(p => new { p.CDCode, p.CDCode_Dtl });

			modelBuilder.Entity<Dmn_CommonCode_M>()
			   .HasKey(p => new { p.CDCode });

			modelBuilder.Entity<DSM.Dmn_Configure>()
			   .HasKey(p => new { p.PlantCode, p.Configure_ID });

			modelBuilder.Entity<DSM.Dmn_JobOrder_D>()
			   .HasKey(p => new { p.PlantCode, p.OrderNo, p.SeqNo, p.JobDetailType });

			modelBuilder.Entity<DSM.Dmn_JobOrder_M>()
			   .HasKey(p => new { p.PlantCode, p.OrderNo, p.SeqNo });

			modelBuilder.Entity<DSM.Dmn_Line_D>()
			   .HasKey(p => new { p.PlantCode, p.LineID, p.JobDetailType });

			modelBuilder.Entity<DSM.Dmn_Line_M>()
			   .HasKey(p => new { p.PlantCode, p.LineID });

			modelBuilder.Entity<DSM.Dmn_Machine>()
			   .HasKey(p => new { p.PlantCode, p.MachineID });

			modelBuilder.Entity<DSM.Dmn_Plant>()
			   .HasKey(p => new { p.PlantCode });

			modelBuilder.Entity<DSM.Dmn_Product_D>()
			   .HasKey(p => new { p.PlantCode, p.ProdCode, p.JobDetailType });
            
			modelBuilder.Entity<DSM.Dmn_Product_D>()
                .Property(p => p.MinimumWeight)
                .HasPrecision(13, 3);

            modelBuilder.Entity<DSM.Dmn_Product_D>()
                .Property(p => p.MinimumWeight)
                .HasPrecision(13, 3);

            modelBuilder.Entity<DSM.Dmn_Product_M>()
			   .HasKey(p => new { p.PlantCode, p.ProdCode });

			modelBuilder.Entity<DSM.Dmn_ReadBarcode>()
			   .HasKey(p => new { p.PlantCode, p.ProdStdCode, p.SerialNum });

			modelBuilder.Entity<DSM.Dmn_ReadBarcode>()
			   .HasIndex(p => new { p.ParentSerialNum });

			modelBuilder.Entity<DSM.Dmn_Serial_Expression>()
			   .HasKey(p => new { p.PlantCode, p.SnExpressionID });

			modelBuilder.Entity<DSM.Dmn_SerialPool>()
			   .HasKey(p => new { p.PlantCode, p.ProdStdCode, p.SerialNum, p.JobDetailType });

			modelBuilder.Entity<DSM.Dmn_SerialPool>()
			   .HasIndex(p => new { p.InspectedDate });

			modelBuilder.Entity<DSM.Dmn_User_D>()
			   .HasKey(p => new { p.PlantCode, p.UserID, p.MachineID });

			modelBuilder.Entity<DSM.Dmn_User_M>()
			   .HasKey(p => new { p.PlantCode, p.UserID });

			modelBuilder.Entity<DSM.Dmn_VisionResult>()
			   .HasKey(p => new { p.PlantCode, p.MachineID, p.OrderNo, p.SeqNo, p.JobDetailType, p.Idx_Insert, p.InsertDate });

            modelBuilder.Entity<DSM.Dmn_CustomBarcodeFormat>()
                .HasKey(p => new { p.PlantCode, p.CustomBarcodeFormatID });

			modelBuilder.Entity<DSM.Dmn_VisionResult>()
			   .HasIndex(p => new { p.InsertDate });

            modelBuilder.Entity<DSM.Dmn_Product_D>()
                .Property(p => p.MinimumWeight)
                .HasPrecision(13, 3);

            modelBuilder.Entity<DSM.Dmn_Product_D>()
                .Property(p => p.MaximumWeight)
                .HasPrecision(13, 3);

            base.OnModelCreating(modelBuilder);
		}

		public static bool HasObject(string _entityName)
		{
			try
			{
				using (var db = new DominoDBServer())
				{
					return db.HasDBObject(_entityName);
				}
			}
			catch (Exception ex)
			{
				log.InfoFormat("Exception : {0}", ex.InnerException ?? ex);
			}
			return false;
		}

		public static bool HasObject(string _entityName, string type)
		{
			try
			{
				int ret = 0;
				using (var db = new DominoDBServer())
					ret = db.Database.SqlQuery<object>($"SELECT * FROM SYS.OBJECTS WHERE TYPE = '{type}' AND NAME LIKE '%{_entityName}%'").Count();
				return ret > 0;
			}
			catch (Exception ex)
			{
				log.InfoFormat("Exception : {0}", ex.InnerException ?? ex);
			}
			return false;
		}

		public static void CreateDbObject(string _objectName)
		{
			try
			{
				string DatabaseName = "DSM";
				string SqlScriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin\");
				string ScriptName = _objectName + ".sql";

				if (string.IsNullOrEmpty(SqlScriptPath))
					return;

				foreach (string eachqry in DBExtension.GetQryArray(Path.Combine(SqlScriptPath, ScriptName)))
				{
					string szQry = string.Empty;
					szQry = eachqry.Replace("@@Database", DatabaseName);
					using (var db = new DominoDBServer())
					{
						db.ExecuteQuery(szQry);
					}
				}
				return;
			}
			catch (Exception ex)
			{
                log.InfoFormat("Exception : {0}", ex.InnerException ?? ex);
				return;
			}
		}

		public static void CreateDbObjectDSM(string _objectName)
		{
			try
			{
				string DatabaseName = "DSM";
				string SqlScriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin\");
				string ScriptName = _objectName + ".sql";

				if (string.IsNullOrEmpty(SqlScriptPath))
					return;

				string sql = File.ReadAllText(Path.Combine(SqlScriptPath, ScriptName)).Replace("@@Database", DatabaseName); ;
				using (var db = new DominoDBServer())
				{
					db.ExecuteQuery(sql);
				}
				return;
			}
			catch (Exception ex)
			{
				log.InfoFormat("Exception : {0}", ex.InnerException ?? ex);
				return;
			}
		}

		/// <summary>
		/// Creates a database object from resource file.
		/// </summary>
		/// <param name="exactEntityName">Resource file name, also an exact entity name. You must name a sql resource file with exact same to the entity name.</param>
		/// <param name="bDropAndCreate">Set this true to drop and create this object.</param>
		/// <param name="type">
		/// "FN" : Function<br />
		/// "P"  : Procedure<br />
		/// "IT" : Table<br />
		/// "S"  : Table, System Table<br />
		/// "U"  : Table, User Defined<br />
		/// "PK" : Primary key constraint<br />
		/// "V"  : View
		/// </param>
		/// <exception cref="ArgumentNullException">Argument "type" must be set when drop and create flag was set.</exception>
		public static void CreateDbObjectDSMFromResource(string exactEntityName, bool bDropAndCreate = false, string type = null)
		{
			if (bDropAndCreate && string.IsNullOrWhiteSpace(type))
			{
				throw new ArgumentNullException($"Drop and create flag was set but the type was null.");
			}

			try
			{
				if (DominoFunctions.ResourceFiles.TryGetResFileContentByPartialName(exactEntityName, out string sql))
				{
					if (bDropAndCreate)
					{
						DropDbObject(exactEntityName, type);
					}

					using (var db = new DominoDBServer())
					{
						db.ExecuteQuery(sql);
						log.Info($"{nameof(CreateDbObjectDSMFromResource)} : Created {exactEntityName} successfully. ");
					}
				}
				else
				{
					throw new FileNotFoundException($"Could not found the resource file conatins this string:{exactEntityName}");
				}
				
				return;
			}
			catch (Exception ex)
			{
				log.InfoFormat("Exception : {0}", ex.InnerException ?? ex);
				return;
			}
		}

		public static void DropDbObject(string exactEntityName, string type)
		{
			using (var ctx = new DominoDBServer())
			{
				string exactName = ctx.Database.SqlQuery<string>($"SELECT NAME FROM SYS.OBJECTS WHERE TYPE = '{type}' AND NAME = '{exactEntityName}' --;").FirstOrDefault();

				if (string.IsNullOrWhiteSpace(exactName))
				{
					log.Info($"[{nameof(DropDbObject)}] : Object [{exactEntityName}] does not exist.");
					return;
				}

				string header = "DROP ";
				switch (type)
				{
					case "FN":
						header += "FUNCTION [dbo].";
						break;
					case "U":
					case "IT":
					case "S":
						header += "TABLE ";
						break;
					case "PK":
						header += "CONSTRAINT ";
						break;
					case "V":
						header += "VIEW ";
						break;
					case "P":
						header += "PROCEDURE [dbo].";
						break;
				}

				ctx.ExecuteQuery(header + exactName);
				log.Info($"{nameof(DropDbObject)} : Dropped {exactEntityName} successfully. ");
			}
		}
	}
}
