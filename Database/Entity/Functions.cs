using DominoDatabase.DSM;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Permissions;

namespace DominoDatabase
{
	public static class Functions
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private static readonly object BulkInsObj = new object();
		private static Dictionary<string, DataTable> TableSchemas = new Dictionary<string, DataTable>();

		private class ColumnDefinition
		{
			public string ColumnName { get; set; }
			public bool Key { get; set; }
			public int Order { get; set; }
			public bool Nullable { get; set; }
			public string DataType { get; set; }
			public string StringLength { get; set; }

			public override string ToString()
			{
				return $"\"ColumnName\":\"{ColumnName}\", \"Order\" = {Order}, \"Nullable\":{Nullable}, \"DataType\":\"{DataType}\", \"StringLength\":{StringLength}";
			}
		}

		private class IndexDefinition
		{
			public string TableName { get; set; }
			public string IndexName { get; set; }
			public string IndexType { get; set; }
			public string IndexColumnAscending { get; set; }
			public bool IsIncludedColumn { get; set; }
			public int IndexColumnOrder { get; set; }
			public string ColumnName { get; set; }
			public bool bDrop { get; set; }

			public override string ToString()
			{
				return $"\"TableName\":\"{TableName}\", \"IndexName\" = \"{IndexName}\", \"IndexType\":\"{IndexType}\", \"IndexColumnAscending\":\"{IndexColumnAscending}\", \"IsIncludedColumn\":\"{IsIncludedColumn}\", \"IndexColumnOrder\":\"{IndexColumnOrder}\", \"ColumnName\":\"{ColumnName}\", \"bDrop\":\"{bDrop}\"";
			}
		}

		public static string ClassToString<T>(object oriData) where T : class
		{
			string retValue = string.Empty;
			T tmp = oriData as T;
			foreach (PropertyInfo property in oriData.GetType().GetProperties())
			{
				retValue += string.Format("[{0}:{1}]", property.Name, Convert.ToString(property.GetValue(tmp, null)));
			}
			return retValue;
		}

		public static bool ContainsIgnoreCase(this IEnumerable<string> arg1, string arg2) => arg1.Any(x => x.SameIgnoreCase(arg2));
		public static bool ContainsIgnoreCase(this string arg1, string arg2) => arg1.IndexOf(arg2, StringComparison.OrdinalIgnoreCase) >= 0;
		public static bool SameIgnoreCase(this string arg1, string arg2) => arg1.IndexOf(arg2, StringComparison.OrdinalIgnoreCase) >= 0 && arg1.Length == arg2.Length;
		
		public static IEnumerable<string> ExceptIgnoreCase(this IEnumerable<string> left, IEnumerable<string> right) => left.Where(x => !right.ContainsIgnoreCase(x));
		public static IEnumerable<string> IntersectIgnoreCase(this IEnumerable<string> left, IEnumerable<string> right) => left.Where(x => right.ContainsIgnoreCase(x));

		public static string GetConfigureValue(string connectionString, string plantcode, string configure_id)
		{
			string sql = string.Empty, ret = string.Empty;

			if (string.IsNullOrWhiteSpace(connectionString))
			{
				log.InfoFormat("{0}", "ConnectionString has not been set.");
				return null;
			}

			try
			{
				sql = $"SELECT TOP 1 [Configure_Value] FROM [Dmn_Configure] WHERE [PlantCode] = '{plantcode}' AND [Configure_ID] = '{configure_id}' --; ";

				using (var conn = new SqlConnection(connectionString))
				using (var cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandText = sql;

					ret = $"{cmd.ExecuteScalar()}";
				}

				return ret;
			}
			catch (Exception ex)
			{
				log.Error("", ex.InnerException ?? ex);
				return null;
			}
		}

		public static bool SetConfigureValue(string connectionString, string plantcode, string configure_id, string configure_value, string updateUserId = "System", bool bInsertIfNotExists = false)
		{
			string sql = string.Empty, ret = string.Empty;

			if (string.IsNullOrWhiteSpace(connectionString))
			{
				log.InfoFormat("{0}", "ConnectionString has not been set.");
				return false;
			}

			try
			{
				sql = $@"
						DECLARE			@OPER NVARCHAR(10) = 'FAILED'

						IF EXISTS (SELECT TOP 1 [Configure_Value] FROM [Dmn_Configure] WHERE [PlantCode] = '{plantcode}' AND [Configure_ID] = '{configure_id}')
						BEGIN
							UPDATE		[Dmn_Configure] 
							SET			[Configure_Value] = '{configure_value}', 
										UpdateUser = '{updateUserId}', 
										UpdateDate = GETDATE() 
							WHERE		[PlantCode] = '{plantcode}' AND 
										[Configure_ID] = '{configure_id}'

							SET			@OPER =	'UPDATED'
						END";

				if (bInsertIfNotExists)
				{
					sql += $@"
						ELSE
						BEGIN
							INSERT INTO [Dmn_Configure] (PlantCode, Configure_ID, Configure_Value, InsertUser, InsertDate)
							SELECT		'{plantcode}' [PlantCode], 
										'{configure_id}' [Configure_ID], 
										'{configure_value}' [Configure_Value], 
										'{updateUserId}' [InsertUser], 
										GETDATE() [InsertDate]
				
							SET			@OPER =	'INSERTED'
						END";
				}
						

				sql +=	"\nSELECT @OPER";


				using (var conn = new SqlConnection(connectionString))
				using (var cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandText = sql;

					ret = $"{cmd.ExecuteScalar()}";
				}

				switch (ret)
				{
					case "INSERTED":
						log.Info($"Insert Configure {configure_id}:{configure_value} successful");
						return true;
					case "UPDATED":
						log.Info($"Update Configure {configure_id}:{configure_value} successful");
						return true;
					case "FAILED":
						log.Info($"{(bInsertIfNotExists ? "Insert" : "Update")} Configure {configure_id}:{configure_value} failed.");
						return false;
					default:
						log.Info("DB error state");
						return false;
				}

			}
			catch (Exception ex)
			{
				log.Error("", ex.InnerException ?? ex);
				return false;
			}
		}

		public static string GetConfigureValueWDSM(string connectionString, string plantcode, string configure_id)
		{
			string sql = string.Empty, ret = string.Empty;

			if (string.IsNullOrWhiteSpace(connectionString))
			{
				log.InfoFormat("{0}", "ConnectionString has not been set.");
				return null;
			}

			try
			{
				sql = $"SELECT TOP 1 [Configure_Value] FROM [WDSM_Configure] WHERE [PlantCode] = '{plantcode}' AND [Configure_ID] = '{configure_id}' --; ";

				using (var conn = new SqlConnection(connectionString))
				using (var cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandText = sql;

					ret = $"{cmd.ExecuteScalar()}";
				}

				return ret;
			}
			catch (Exception ex)
			{
				log.Error("", ex.InnerException ?? ex);
				return null;
			}
		}

		public static bool SyncEntities(bool isForLocalMachine = false, IEnumerable<Type> targetTypes = null, string connectionString = null)
		{
			log.Warn("Warning: Be sure to backup the database to avoid data loss.");

			string connStr = string.Empty;
			string sql = string.Empty;
			//string strColDropSql = string.Empty;

			if (connectionString is null)
			{
				if (isForLocalMachine)
					connStr = System.Configuration.ConfigurationManager.ConnectionStrings["Local"].ToString();
				else
					connStr = System.Configuration.ConfigurationManager.ConnectionStrings["DSM"].ToString();
			}
			else
			{
				connStr = connectionString;
			}

			try
			{
				using (SqlConnection conn = new SqlConnection(connStr))
				{
					conn.Open();
				}
			}
			catch
			{
				log.Warn("Unable to open SqlConnection. Check database connection string, existance, network connection. Sync process skipped.");
				return false;
			}

			bool bIsModified = false;
			List<Type> EFTypes;
			if (targetTypes is null)
			{
				// Defined Types from Entity
				EFTypes = AppDomain
								.CurrentDomain
								.GetAssemblies()
								.FirstOrDefault(x => x.GetName().Name.Equals("DominoDatabase"))
								.GetTypes()
								.Where(
									x =>
										(x?.Name ?? "").Contains("Dmn_") &&
										!(x?.Name ?? "").Contains("Dmn_View_")
								)
								.Where(
									x =>
										x.Namespace.Equals("DominoDatabase") ||
										(isForLocalMachine ? x.Namespace.Equals("DominoDatabase.Local") : x.Namespace.Equals("DominoDatabase.DSM"))
								)
								.ToList();
			}
			else
			{
				EFTypes = targetTypes.ToList();
			}
			
			var TypeNamesIn = "";
			foreach (Type type in EFTypes) 
			{ 
				TypeNamesIn += type.Name + "', '";
			}
			TypeNamesIn = "'" + TypeNamesIn + "'";

			var DBTableNames = new List<string>();
			var fromDB = new Dictionary<string, List<ColumnDefinition>>();
			var fromEF = new Dictionary<Type, List<ColumnDefinition>>();

			var restorableTblNames = new List<string>();
			string TranslatedDataType(string arg1) =>
							arg1.ContainsIgnoreCase("bool") ? "BIT" :
							arg1.ContainsIgnoreCase("DateTime") ? "DATETIME" :
							arg1.ContainsIgnoreCase("long") ? "BIGINT" :
							arg1.ContainsIgnoreCase("int64") ? "BIGINT" :
							arg1.ContainsIgnoreCase("int32") ? "INT" :
							arg1.ContainsIgnoreCase("Enum") ? "INT" :
							arg1.ContainsIgnoreCase("int") ? "INT" :
							arg1.ContainsIgnoreCase("String") ? "NVARCHAR" :
							arg1;

			try
			{
				foreach (var t in EFTypes)
				{
					int i = -1;
					var props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty).ToList();
					var orderedProps = props.Where(x => x.CustomAttributes.Any(y => y.NamedArguments.Any(z => z.MemberName.Equals("Order")))).ToList();
					var notOrderedProps = props.Except(orderedProps).ToList();

					// Order Configured Columns
					fromEF.Add(
						t,
						(
							from p in orderedProps
							let o = p.CustomAttributes
										.Where(x => x.AttributeType == typeof(ColumnAttribute))
										.FirstOrDefault().NamedArguments
										.FirstOrDefault(x => x.MemberName == "Order").TypedValue.Value
							let l = p.CustomAttributes
										.Where(x => x.AttributeType == typeof(StringLengthAttribute))
										.FirstOrDefault()?.ConstructorArguments
										.FirstOrDefault().Value ?? "MAX"
							let r = p.CustomAttributes
										.Any(x => x.AttributeType == typeof(RequiredAttribute) || x.AttributeType == typeof(KeyAttribute))
							let k = p.CustomAttributes
										.Any(x => x.AttributeType == typeof(KeyAttribute))
							select new ColumnDefinition()
							{
								ColumnName = p.Name,
								Key = k,
								Order = (int)o,
								Nullable = !r,
								DataType =
									TranslatedDataType(
										p.PropertyType.IsGenericParameter ? p.PropertyType.Name :
										p.PropertyType.IsGenericType ?
											p.PropertyType.GetGenericArguments().FirstOrDefault().BaseType == typeof(Enum) ?
												"Enum" :
												p.PropertyType.GetGenericArguments().FirstOrDefault().Name :
										p.PropertyType.FullName
									),
								StringLength = $"{l}"
							}
						).ToList()
					);

					// Not ordered columns
					i = fromEF[t].Count();
					fromEF[t].AddRange(
						(
							from p in notOrderedProps
							let o = i++
							let l = p.CustomAttributes
										.Where(x => x.AttributeType == typeof(StringLengthAttribute))
										.FirstOrDefault()?.ConstructorArguments
										.FirstOrDefault().Value ?? "MAX"
							let r = p.CustomAttributes
										.Any(x => x.AttributeType == typeof(RequiredAttribute) || x.AttributeType == typeof(KeyAttribute))
							let k = p.CustomAttributes
										.Any(x => x.AttributeType == typeof(KeyAttribute))
							select new ColumnDefinition()
							{
								ColumnName = p.Name,
								Key = k,
								Order = (int)o,
								Nullable = !r,
								DataType =
									TranslatedDataType(
										p.PropertyType.IsGenericParameter ? p.PropertyType.Name :
										p.PropertyType.IsGenericType ? 
											p.PropertyType.GetGenericArguments().FirstOrDefault().BaseType == typeof(Enum) ? 
												"Enum" :
												p.PropertyType.GetGenericArguments().FirstOrDefault().Name :
										p.PropertyType.FullName
									),
								StringLength = $"{l}"
							}
						).ToList()
					);
				}

				// Fetching current data from the database
				using (SqlConnection conn = new SqlConnection(connStr))
				{
					conn.Open();

					// Remove Migration Table
					using (SqlCommand cmd = new SqlCommand($"IF OBJECT_ID (N'__MigrationHistory', N'U') IS NOT NULL BEGIN DROP TABLE __MigrationHistory END --; ", conn))
						cmd.ExecuteNonQuery();

					// "Default Value" constraint drop
					using (SqlDataAdapter adap = new SqlDataAdapter(
						" SELECT " +
						"   OBJECT_NAME(dc.parent_object_id) [TableName], " +
						"   dc.name [ConstraintName] " +
						" FROM " +
						"   sys.default_constraints dc " +
						" WHERE " +
						$"   dc.parent_object_id in (select object_id from sys.objects tbl where tbl.type = 'U' and tbl.name in ({TypeNamesIn})) ", conn))
					{
						DataTable tbl = new DataTable();
						adap.Fill(tbl);

						string dropDfStr = string.Empty;
						foreach (DataRow r in tbl.Rows)
						{
							dropDfStr += $"\n ALTER TABLE {r["TableName"]} DROP CONSTRAINT {r["ConstraintName"]};";
						}

						if (!string.IsNullOrEmpty(dropDfStr))
						{
							using (SqlCommand cmd = new SqlCommand(dropDfStr, conn))
								cmd.ExecuteNonQuery();

							log.Info($"Default Value Constraint(s) Are Dropped:{dropDfStr}");
						}
					}

					// Fetching DB table names
					using (SqlDataAdapter adap = new SqlDataAdapter($" select name from sys.tables where type = 'U' and name <> 'sysdiagrams' and name not like '%BACKUP%' ", conn))
					{
						DataTable tbl = new DataTable();
						adap.Fill(tbl);

						foreach (DataRow r in tbl.Rows)
							DBTableNames.Add($"{r[0]}");
					}

					foreach (var t in DBTableNames)
					{
						DataTable tbl = new DataTable();


						/// CHARACTER_MAXIMUM_LENGTH
						/// positive integer > normal varchar, length equals to integer
						/// -1 > varchar(max)
						/// null > not a nvarchar; int, bigint, 
						using (SqlDataAdapter adap = new SqlDataAdapter($@"
							SELECT
	                            COLUMN_NAME													[ColumnName],
	                            CAST(
		                            (SELECT
			                            COUNT(*) 
		                            FROM
			                            INFORMATION_SCHEMA.KEY_COLUMN_USAGE 
		                            WHERE
			                            TABLE_NAME = '{t}' AND 
			                            COLUMN_NAME = C.COLUMN_NAME)
	                            AS BIT)														[Key],
	                            ORDINAL_POSITION - 1										[Order],
	                            CAST(CASE IS_NULLABLE WHEN 'YES' THEN 1 ELSE 0 END AS BIT)	[Nullable],
	                            DATA_TYPE													[DataType],
	                            CASE CHARACTER_MAXIMUM_LENGTH
                                    WHEN -1 THEN 'MAX'
                                    WHEN NULL THEN NULL
                                    ELSE CAST(CHARACTER_MAXIMUM_LENGTH AS NVARCHAR) 
                                END                                                         [StringLength]
                            FROM INFORMATION_SCHEMA.COLUMNS C
                            WHERE TABLE_NAME = '{t}' --; ", conn))
						{
							adap.Fill(tbl);
						}

						fromDB.Add(
							t,
							(
								from DataRow r in tbl.Rows
								select new ColumnDefinition()
								{
									ColumnName = $"{r["ColumnName"]}",
									Order = int.Parse($"{r["Order"]}"),
									Nullable = bool.Parse($"{r["Nullable"]}"),
									DataType = $"{r["DataType"]}",
									StringLength = $"{r["StringLength"]}"
								}
							).ToList()
						);
					}
				}

				// Table Rename, same table name but different case only
				var tablesToRename =
					fromDB.Keys.CaseInsensitiveIntersect(fromEF.Keys.Select(x => x.Name))
					.Except(fromDB.Keys.Intersect(fromEF.Keys.Select(x => x.Name)));
				string renameSql = string.Empty;
				foreach (var t in tablesToRename)
				{
					var DBTblName = fromDB.FirstOrDefault(x => x.Key.SameIgnoreCase(t)).Key;
					var EFTblName = fromEF.FirstOrDefault(x => x.Key.Name.SameIgnoreCase(t)).Key.Name;

					renameSql += $"\n EXEC SP_RENAME 'dbo.[{DBTblName}]', '{EFTblName}'; ";
				}
				renameSql = renameSql.TrimStart('\n');

				if (!string.IsNullOrEmpty(renameSql))
				{
					using (SqlConnection conn = new SqlConnection(connStr))
					{
						conn.Open();

						using (SqlCommand cmd = new SqlCommand(renameSql, conn))
							cmd.ExecuteNonQuery();
					}

					log.Warn($"\t<<Renamed table(s)>>:\n{renameSql}");

					return SyncEntities(isForLocalMachine, targetTypes, connectionString);
				}

				List<IndexDefinition> renamedIdxList = new List<IndexDefinition>();
				using (SqlConnection conn = new SqlConnection(connStr))
				{
					conn.Open();

					#region "Table(s) to drop"
					////Table Drop Deprecated
					//// Table Drop; It will be renamed with _BACKUP_YYMMDDHHmmss instead, including keys
					//var tablesToDrop = fromDB.Keys.ExceptIgnoreCase(fromEF.Keys.Select(x => x.Name)).ToList();
					//string dropSql = string.Empty;
					//foreach (var tt in tablesToDrop)
					//{
					//	var t = fromDB[tt];
					//	var constraintName = string.Empty;
					//	using (SqlCommand cmd = new SqlCommand("SELECT CONSTRAINT_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS" +
					//				$" WHERE TABLE_NAME = '{tt}' AND CONSTRAINT_TYPE = 'PRIMARY KEY'", conn))
					//	{
					//		constraintName = $"{cmd.ExecuteScalar()}";
					//	}

					//	var curTime = DateTime.Now.ToString("yyMMddHHmmss");
					//	dropSql += $"\n EXEC SP_RENAME 'dbo.[{tt}]', '{tt}_BACKUP_{curTime}'; ";

					//	if (!string.IsNullOrEmpty(constraintName))
					//		dropSql += $"\n EXEC SP_RENAME 'dbo.[{constraintName}]', '{constraintName}_BACKUP_{curTime}', 'OBJECT'; ";

					//	var indexTbl = new DataTable();
					//	using (SqlDataAdapter adap = new SqlDataAdapter(" select name from sys.indexes " +
					//				$" WHERE object_id = OBJECT_ID('{tt}') AND is_primary_key = 0 --; ", conn))
					//	{
					//		adap.Fill(indexTbl);
					//	}

					//	foreach (DataRow r in indexTbl.Rows)
					//	{
					//		dropSql += $"\n EXEC sp_rename N'{tt}.[{r["name"]}]', '{r["name"]}_BACKUP_{curTime}', 'INDEX' --; ";
					//	}

					//	// Index must be captured before rename
					//	var tblIndex = new DataTable();
					//	using (SqlDataAdapter adap = new SqlDataAdapter(
					//		$@"select
					//                         OBJECT_NAME(ic.object_id) COLLATE Korean_Wansung_CI_AS [TableName] , 
					//                         ix.name COLLATE Korean_Wansung_CI_AS [IndexName] ,
					//                         ix.type_desc COLLATE Korean_Wansung_CI_AS [IndexType],
					//                         CASE ic.is_descending_key WHEN 1 THEN 'DESC' ELSE 'ASC' END [IndexColumnAscending],
					//                         ic.is_included_column [IsIncludedColumn],
					//                         ic.index_column_id [IndexColumnOrder], 
					//                         c.name COLLATE Korean_Wansung_CI_AS [ColumnName] 
					//                        from sys.index_columns ic
					//                        join sys.indexes ix 
					//                        on ic.index_id = ix.index_id and ic.object_id = ix.object_id
					//                        join sys.columns c 
					//                        on c.object_id = ic.object_id and ic.column_id = c.column_id
					//                        WHERE ix.is_primary_key = 0 AND OBJECT_NAME(ic.object_id) = '{t}' --;", conn))
					//	{
					//		adap.Fill(tblIndex);
					//	}

					//	renamedIdxList.AddRange((from DataRow r in tblIndex.Rows
					//							 select new IndexDefinition()
					//							 {
					//								 TableName = $"{r["TableName"]}",
					//								 IndexName = $"{r["IndexName"]}",
					//								 IndexType = $"{r["IndexType"]}",
					//								 IndexColumnAscending = $"{r["IndexColumnAscending"]}",
					//								 IsIncludedColumn = $"{r["IsIncludedColumn"]}" == "1",
					//								 IndexColumnOrder = int.Parse($"{r["IndexColumnOrder"]}"),
					//								 ColumnName = $"{r["ColumnName"]}",
					//								 bDrop = false
					//							 }).ToList());

					//}

					//if (!string.IsNullOrEmpty(dropSql))
					//{
					//	using (SqlCommand cmd = new SqlCommand(dropSql, conn))
					//		cmd.ExecuteNonQuery();

					//	bIsModified = true;

					//	log.Warn($"Renamed table name and primary key of table:[{dropSql}]");
					//}
					#endregion

					#region "Table(s) to create"
					// Table Add
					var tablesToAdd = EFTypes.Select(x => x.Name).ExceptIgnoreCase(fromDB.Keys).ToList();
					string addSql = string.Empty;
					foreach (var tt in tablesToAdd)
					{
						var t = fromEF.FirstOrDefault(x => x.Key.Name.Equals(tt)).Value;

						addSql += $"\n CREATE TABLE {tt} ( ";
						var keyStr = string.Empty;

						for (int i = 0; i < t.Count(); i++)
						{
							var found = t.FirstOrDefault(x => x.Order.Equals(i));

							addSql += $" \n\t [{found.ColumnName}] {found.DataType} " +
										$" {(found.DataType.ContainsIgnoreCase("varchar") ? $"({found.StringLength})" : string.Empty)} " +
										$" {(!found.Key && !found.Nullable ? " NOT NULL " : string.Empty)},";

							if (found.Key) keyStr += $"{(string.IsNullOrEmpty(keyStr) ? string.Empty : ", ")}{found.ColumnName} ASC";
						}
						addSql += $"\n CONSTRAINT [PK_dbo.{tt}] PRIMARY KEY CLUSTERED ({keyStr})" +
									$"WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" +
									$") ON [PRIMARY]; \n";

						// Create index
						string strIdxCreateCmd = string.Empty;
						foreach (var l in renamedIdxList.Where(x => x.TableName == tt).Select(x => new { x.IndexName, x.TableName, x.IndexType }).Distinct())
						{
							// Index Base Columns With Ascending Information
							var strColNamesWithAsc = string.Empty;
							var listSortCols = new List<string>();
							foreach (var f in renamedIdxList.Where(x => x.IndexName == l.IndexName && x.TableName == l.TableName && x.IndexType == l.IndexType && !x.IsIncludedColumn))
								listSortCols.Add(f.ColumnName);

							if (listSortCols.Count == 0)
								continue;

							foreach (var c in listSortCols)
								strColNamesWithAsc += $"\n[{c}] {renamedIdxList.FirstOrDefault(x => x.IndexName == l.IndexName && x.TableName == l.TableName && x.IndexType == l.IndexType && x.ColumnName == c).IndexColumnAscending},";

							if (!string.IsNullOrWhiteSpace(strColNamesWithAsc))
								strColNamesWithAsc = $"( {strColNamesWithAsc.TrimEnd(',')} )";
							else
								strColNamesWithAsc = string.Empty;

							// Index Include Columns
							string strInclQry = string.Empty;
							var listInclCols = new List<string>();

							if (SameIgnoreCase(l.IndexType, "NONCLUSTERED"))
							{
								foreach (var f in renamedIdxList.Where(x => x.IndexName == l.IndexName && x.TableName == l.TableName && x.IndexType == l.IndexType && x.IsIncludedColumn))
									listInclCols.Add(f.ColumnName);

								foreach (var c in listInclCols)
									strInclQry += $"\n[{c}],";

								if (!string.IsNullOrEmpty(strInclQry))
									strInclQry = $" INCLUDE ({strInclQry.TrimEnd(',')})";
							}

							if (!string.IsNullOrWhiteSpace(strColNamesWithAsc) || !string.IsNullOrWhiteSpace(strInclQry))
								strIdxCreateCmd +=
									$"\n CREATE {l.IndexType} INDEX [{l.IndexName}] ON [dbo].[{l.TableName}] " +
									$"\n {strColNamesWithAsc} {strInclQry} " +
									$"\n WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]";
						}

						if (!string.IsNullOrEmpty(strIdxCreateCmd))
						{
							using (SqlCommand comm = new SqlCommand(strIdxCreateCmd, conn))
								comm.ExecuteNonQuery();

							bIsModified = true;
							log.Info($"\tIndex Created:{strIdxCreateCmd}");
						}
					}

					if (!string.IsNullOrEmpty(addSql))
					{
						using (SqlCommand cmd = new SqlCommand(addSql, conn))
							cmd.ExecuteNonQuery();

						bIsModified = true;

						log.Info($"Added table(s):\n{addSql}");
					}
					#endregion
				}

				var tablesToAlter = fromDB.Keys.IntersectIgnoreCase(fromEF.Keys.Select(x => x.Name)).ToList();
				foreach (var t in tablesToAlter)
				{
					sql = string.Empty;
					bool bDropKeyConstraint = false;
					var listAlterColumnNames = new List<string>();
					var listDropColumnNames = new List<string>();

					//strColDropSql = string.Empty;

					Type EFTypeName = fromEF.FirstOrDefault(x => x.Key.Name.SameIgnoreCase(t)).Key;
					var DBCols = fromDB[t].Select(x => x.ColumnName).ToList();
					var EFCols = fromEF[EFTypeName].Select(x => x.ColumnName).ToList();

					#region "Column Drop [Deprecated]"
					//// Drop Column [DEPRECATED]
					//var ColsToRemove = DBCols.ExceptIgnoreCase(EFCols);
					//var bBackupTblRequired = false;
					//using (SqlConnection conn = new SqlConnection(connStr))
					//{
					//	conn.Open();

					//	bool isDropCol = false;
					//	foreach (string colname in ColsToRemove)
					//	{
					//		var captured = fromEF[EFTypeName].FirstOrDefault(x => x.ColumnName == colname);
					//		var iValidRowCnt = 0;

					//		using (SqlCommand cmd = new SqlCommand($" SELECT COUNT(1) FROM {t} WHERE [{colname}] IS NOT NULL AND DATALENGTH([{colname}]) > 0 --;", conn))
					//		{
					//			iValidRowCnt = (int)cmd.ExecuteScalar();
					//		}

					//		if (iValidRowCnt > 0)
					//		{
					//			log.Warn($"This column seems to have valid values:[{colname}]. Backup table will be made.");
					//			bBackupTblRequired = true;
					//			isDropCol = true;
					//		}

					//		if (fromEF[EFTypeName].Any(x => x.ColumnName == colname && x.Key))
					//			bDropKeyConstraint = true;

					//		// Dropping a column will also drop index referencing it.
					//		strColDropSql += $"\n ALTER TABLE {t} DROP COLUMN [{colname}] ";
					//		listDropColumnNames.Add(colname);

					//	}
					//}
					#endregion

					// Add Column
					var ColsToAdd = EFCols.ExceptIgnoreCase(DBCols);
					foreach (string colname in ColsToAdd)
					{
						// Drop constraint if needed

						var captured = fromEF[EFTypeName].FirstOrDefault(x => x.ColumnName == colname);
						if (captured.Key)
							bDropKeyConstraint = true;

						string translatedType = TranslatedDataType(captured.DataType);
						if (ContainsIgnoreCase(translatedType, "varchar")) translatedType += $"({captured.StringLength})";
						string isnullable = !captured.Nullable ? "NOT NULL" : string.Empty;

						sql += $"\n ALTER TABLE {t} ADD [{colname}] {translatedType} {isnullable} ";
					}

					// Alter Column (Alter Table)
					var ColsToUpdate = EFCols.IntersectIgnoreCase(DBCols);
					foreach (string colname in ColsToUpdate)
					{
						var capturedEF = fromEF[EFTypeName].FirstOrDefault(x => x.ColumnName.SameIgnoreCase(colname));
						var capturedDB = fromDB[t].FirstOrDefault(x => x.ColumnName.SameIgnoreCase(colname));

						string strNotNull = !capturedEF.Nullable || capturedEF.Key ? " NOT NULL " : string.Empty;
						bool bAlterFlag = false;

						// If the column name is the only thing different between EF and DB, we should rename it, not alter it.
						// Renaming a column does not require to re-create primary key or indexes.

						// Compare points: ColumnName, DataType, StringLength, Nullable
						if (capturedDB.ColumnName != capturedEF.ColumnName)
						{
							sql += $"\n EXEC sp_rename '{t}.{capturedDB.ColumnName}', '{capturedEF.ColumnName}', 'COLUMN' ";
						}

						if (!SameIgnoreCase(capturedDB.DataType, capturedEF.DataType))
						{
							if (ContainsIgnoreCase(capturedEF.DataType, "varchar"))
								sql += $"\n ALTER TABLE {t} ALTER COLUMN [{colname}] {capturedEF.DataType}({capturedEF.StringLength}) {strNotNull} ";
							else
								sql += $"\n ALTER TABLE {t} ALTER COLUMN [{colname}] {capturedEF.DataType} {strNotNull} ";

							bAlterFlag = true;
						}
						else
						{
							if (SameIgnoreCase(capturedEF.DataType, "varchar") && capturedEF.StringLength != capturedDB.StringLength)
							{
								sql += $"\n ALTER TABLE {t} ALTER COLUMN [{colname}] {capturedEF.DataType}({capturedEF.StringLength}) {strNotNull} ";
								bAlterFlag = true;
							}
						}

						if (bAlterFlag)
						{
							if (capturedEF.Key)
								bDropKeyConstraint = true;

							listAlterColumnNames.Add(colname);
						}
					}

					using (SqlConnection conn = new SqlConnection(connStr))
					{
						conn.Open();

						#region "Column Drop [Deprecated]"
						//// Column Drop Deprecated
						//if (bBackupTblRequired)
						//{
						//	using (SqlCommand cmd = new SqlCommand($" SELECT * INTO [{t}_BACKUP_{DateTime.Now.ToString("yyMMddHHmmss")}] FROM {t}  --;", conn))
						//		cmd.ExecuteNonQuery();

						//	bIsModified = true;
						//}
						#endregion

						if (bDropKeyConstraint)
						{
							var constraintName = string.Empty;
							using (SqlCommand cmd = new SqlCommand(
										" SELECT CONSTRAINT_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS " +
										$" WHERE TABLE_NAME = '{t}' AND CONSTRAINT_TYPE = 'PRIMARY KEY'", conn))
							{
								constraintName = $"{cmd.ExecuteScalar()}";

								bIsModified = true;
							}

							if (!string.IsNullOrEmpty(constraintName))
							{
								string dropSql =
									$@" ALTER TABLE {t} DROP CONSTRAINT {constraintName} --; ";

								log.Warn($"Dropping [Primary key] constraint on table:{t}");
								log.Info("This query may take time up to 10 minutes.");
								var startTime = DateTime.Now;
								using (SqlCommand cmd = new SqlCommand(dropSql, conn))
								{
									cmd.CommandTimeout = 600;
									cmd.ExecuteNonQuery();
								}
								log.Info($"SqlCommand costed {(DateTime.Now - startTime).TotalSeconds} seconds.");

								bIsModified = true;
							}
						}

						// Index drop
						SqlDataAdapter adap = new SqlDataAdapter(
							$@"select
		                            OBJECT_NAME(ic.object_id) COLLATE Korean_Wansung_CI_AS [TableName] , 
		                            ix.name COLLATE Korean_Wansung_CI_AS [IndexName] ,
		                            ix.type_desc COLLATE Korean_Wansung_CI_AS [IndexType],
		                            CASE ic.is_descending_key WHEN 1 THEN 'DESC' ELSE 'ASC' END [IndexColumnAscending],
		                            ic.is_included_column [IsIncludedColumn],
		                            ic.index_column_id [IndexColumnOrder], 
		                            c.name COLLATE Korean_Wansung_CI_AS [ColumnName] 
	                            from sys.index_columns ic
	                            join sys.indexes ix 
	                            on ic.index_id = ix.index_id and ic.object_id = ix.object_id
	                            join sys.columns c 
	                            on c.object_id = ic.object_id and ic.column_id = c.column_id
	                            WHERE ix.is_primary_key = 0 AND OBJECT_NAME(ic.object_id) = '{t}' --;", conn);

						var tblIndex = new DataTable();
						adap.Fill(tblIndex);
						adap.Dispose();

						var listIdx = (from DataRow r in tblIndex.Rows
									   select new IndexDefinition()
									   {
										   TableName = $"{r["TableName"]}",
										   IndexName = $"{r["IndexName"]}",
										   IndexType = $"{r["IndexType"]}",
										   IndexColumnAscending = $"{r["IndexColumnAscending"]}",
										   IsIncludedColumn = $"{r["IsIncludedColumn"]}" == "1",
										   IndexColumnOrder = int.Parse($"{r["IndexColumnOrder"]}"),
										   ColumnName = $"{r["ColumnName"]}",
										   bDrop = false
									   }).ToList();

						foreach (var l in listIdx.Where(x => listDropColumnNames.Contains(x.ColumnName)))
							listIdx.Remove(l);

						foreach (var l in listIdx.Where(x => listAlterColumnNames.Contains(x.ColumnName)))
							l.bDrop = true;

						listIdx = listIdx.Where(x => x.bDrop == true).ToList();

						#region "Column Drop [Deprecated]"
						//// Column Drop Deprecated
						//if (!string.IsNullOrEmpty(strColDropSql))
						//{
						//	using (SqlCommand comm = new SqlCommand(strColDropSql, conn))
						//		comm.ExecuteNonQuery();

						//	bIsModified = true;
						//	log.Info($"\n\tColumn(s) Dropped:{strColDropSql}");
						//}
						#endregion

						string strIdxDropCmd = string.Empty;
						foreach (var l in listIdx.Where(x => x.bDrop).Select(x => new { x.IndexName, x.TableName }).Distinct())
						{
							strIdxDropCmd += $"\n DROP INDEX [{l.IndexName}] ON [dbo].[{l.TableName}]";
						}

						if (!string.IsNullOrEmpty(strIdxDropCmd))
						{
							log.Info("This query may take time up to 10 minutes.");
							log.Info($"\n\t<<INDEX DROP SQL QUERY>>:{strIdxDropCmd}");
							var startTime = DateTime.Now;
							using (SqlCommand comm = new SqlCommand(strIdxDropCmd, conn))
							{
								comm.CommandTimeout = 600;
								comm.ExecuteNonQuery();
							}
							log.Info($"SqlCommand costed {(DateTime.Now - startTime).TotalSeconds} seconds.");

							bIsModified = true;
							log.Info($"\n\tIndex Dropped:{strIdxDropCmd}");
						}


						if (!string.IsNullOrEmpty(sql))
						{
							bIsModified = true;
							log.Info("This query may take time up to 20+ minutes.");
							var globalStartTime = DateTime.Now;

							foreach (var s in sql.Split('\n'))
							{
								var startTime = DateTime.Now;
								using (SqlCommand comm = new SqlCommand(s, conn))
								{
									if (string.IsNullOrWhiteSpace(s))
										continue;

									log.Info($"<<DDL SQL QUERY>>:{s}");
									comm.CommandTimeout = 1200;
									comm.ExecuteNonQuery();
								}
								log.Info($"SqlCommand costed {(DateTime.Now - startTime).TotalSeconds} seconds.");
							}

							log.Info($"DDL query costed {(DateTime.Now - globalStartTime).TotalSeconds} seconds in total.");
						}

						string strIdxCreateCmd = string.Empty;
						foreach (var l in listIdx.Select(x => new { x.IndexName, x.TableName, x.IndexType }).Distinct())
						{
							// Index Base Columns With Ascending Information
							var strColNamesWithAsc = string.Empty;
							var listSortCols = new List<string>();
							foreach (var f in listIdx.Where(x => x.IndexName == l.IndexName && x.TableName == l.TableName && x.IndexType == l.IndexType && !x.IsIncludedColumn))
								listSortCols.Add(f.ColumnName);

							if (listSortCols.Count == 0)
								continue;

							foreach (var c in listSortCols)
								strColNamesWithAsc += $"\n\t[{c}] {listIdx.FirstOrDefault(x => x.IndexName == l.IndexName && x.TableName == l.TableName && x.IndexType == l.IndexType && x.ColumnName == c).IndexColumnAscending},";

							if (!string.IsNullOrWhiteSpace(strColNamesWithAsc))
								strColNamesWithAsc = $"( {strColNamesWithAsc.TrimEnd(',')} \n)";
							else
								strColNamesWithAsc = string.Empty;

							// Index Include Columns
							string strInclQry = string.Empty;
							var listInclCols = new List<string>();

							if (SameIgnoreCase(l.IndexType, "NONCLUSTERED"))
							{
								foreach (var f in listIdx.Where(x => x.IndexName == l.IndexName && x.TableName == l.TableName && x.IndexType == l.IndexType && x.IsIncludedColumn))
									listInclCols.Add(f.ColumnName);

								foreach (var c in listInclCols)
									strInclQry += $"\n[{c}],";

								if (!string.IsNullOrEmpty(strInclQry))
									strInclQry = $" INCLUDE ({strInclQry.TrimEnd(',')})";
							}

							if (!string.IsNullOrWhiteSpace(strColNamesWithAsc) || !string.IsNullOrWhiteSpace(strInclQry))
								strIdxCreateCmd +=
									$"\n CREATE {l.IndexType} INDEX [{l.IndexName}] ON [dbo].[{l.TableName}] " +
									$"\n {strColNamesWithAsc} {strInclQry} " +
									$"\n WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]";
						}

						if (!string.IsNullOrEmpty(strIdxCreateCmd))
						{
							log.Info("This query may take time up to 10 minutes.");
							log.Info($"\n\t<<INDEX CREATION SQL QUERY>>:{strIdxCreateCmd}");
							var startTime = DateTime.Now;
							using (SqlCommand comm = new SqlCommand(strIdxCreateCmd, conn))
							{
								comm.CommandTimeout = 600;
								comm.ExecuteNonQuery();
							}
							log.Info($"SqlCommand costed {(DateTime.Now - startTime).TotalSeconds} seconds.");

							bIsModified = true;
							log.Info($"\n\tIndex Created:{strIdxCreateCmd}");
						}

						bool bCreateKeyConstraint = false;
						using (SqlCommand cmd = new SqlCommand($" SELECT COUNT(1) FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE TABLE_NAME = '{t}' AND CONSTRAINT_TYPE = 'PRIMARY KEY'  --;", conn))
							bCreateKeyConstraint = (int)cmd.ExecuteScalar() == 0;

						if (bCreateKeyConstraint)
						{
							string keyStr = string.Empty;
							foreach (var c in fromEF[EFTypeName].Where(x => x.Key))
							{
								keyStr += (string.IsNullOrEmpty(keyStr) ? "" : ", ") + $"[{c.ColumnName}] ";
							}

							if (!string.IsNullOrEmpty(keyStr))
							{
								string addSql =
									$" ALTER TABLE {t} ADD CONSTRAINT [PK_dbo.{t}] PRIMARY KEY CLUSTERED ({keyStr}) WITH " +
									$" (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] --; ";

								log.Info($"Creating [Primary key] constraint on table:{t}");
								log.Info("This query may take time up to 10 minutes.");
								var startTime = DateTime.Now;
								using (SqlCommand cmd = new SqlCommand(addSql, conn))
								{
									cmd.CommandTimeout = 600;
									cmd.ExecuteNonQuery();
								}
								log.Info($"SqlCommand costed {(DateTime.Now - startTime).TotalSeconds} seconds.");

								bIsModified = true;
							}
						}
					}
				}

				// Index sync for DSM Svr only
				if (targetTypes is null)
				{
					using (SqlConnection conn = new SqlConnection(connStr))
					{
						conn.Open();

						//Please add indexes that configured in OnModelCreating(modelbuilder).
						int idx_chk = 0;
						using (var cmd = new SqlCommand("select 1 from sys.indexes where OBJECT_NAME(object_id) = 'Dmn_ReadBarcode' and name like 'IX_ParentSerialNum' --; ", conn))
						{
							idx_chk = (int?)cmd?.ExecuteScalar() ?? 0;
						}

						if (idx_chk != 1)
						{
							log.Info("A default index does not exist:[Dmn_ReadBarcode].[IX_ParentSerialNum]");

							using (var cmd = new SqlCommand("CREATE NONCLUSTERED INDEX [IX_ParentSerialNum] ON [dbo].[Dmn_ReadBarcode] ([ParentSerialNum] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] --; ", conn))
							{
								cmd.CommandTimeout = 500;
								cmd.ExecuteNonQuery();
							}

							log.Info("Created default index:[Dmn_ReadBarcode].[IX_ParentSerialNum]");
						}

						idx_chk = 0;
						using (var cmd = new SqlCommand("select 1 from sys.indexes where OBJECT_NAME(object_id) = 'Dmn_SerialPool' and name like 'IX_InspectedDate' --; ", conn))
						{
							idx_chk = (int?)cmd?.ExecuteScalar() ?? 0;
						}

						if (idx_chk != 1)
						{
							log.Info("A default index does not exist:[Dmn_SerialPool].[IX_InspectedDate]");

							using (var cmd = new SqlCommand("CREATE NONCLUSTERED INDEX [IX_InspectedDate] ON [dbo].[Dmn_SerialPool]([InspectedDate] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] --; ", conn))
							{
								cmd.CommandTimeout = 500;
								cmd.ExecuteNonQuery();
							}

							log.Info("Created default index:[Dmn_SerialPool].[IX_InspectedDate]");
						}

						idx_chk = 0;
						using (var cmd = new SqlCommand("select 1 from sys.indexes where OBJECT_NAME(object_id) = 'Dmn_VisionResult' and name like 'IX_InsertDate' --; ", conn))
						{
							idx_chk = (int?)cmd?.ExecuteScalar() ?? 0;
						}

						if (idx_chk != 1)
						{
							log.Info("A default index does not exist:[Dmn_VisionResult].[IX_InsertDate]");

							using (var cmd = new SqlCommand("CREATE NONCLUSTERED INDEX [IX_InsertDate] ON [dbo].[Dmn_VisionResult]([InsertDate] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] --; ", conn))
							{
								cmd.CommandTimeout = 500;
								cmd.ExecuteNonQuery();
							}

							log.Info("Created default index:[Dmn_VisionResult].[IX_InsertDate]");
						}
					}
				}

				if (bIsModified)
				{
					log.Info("Entity sync was successful. You must check the objects that might have changed during this sync: backup tables, dependencies, triggers etc.");
					return true;
				}
				else
				{
					log.Info("No changes detected. Entity sync finished.");
					return true;
				}
			}
			catch (Exception ex)
			{
				log.InfoFormat("Exception : {0}", ex.InnerException ?? ex);
				log.Error("Entity sync had failed.");
				return false;
			}
		}

		public static IEnumerable<string> CaseInsensitiveIntersect(this IEnumerable<string> left, IEnumerable<string> right)
		{
			var res = new List<string>();

			for (int i = 0; i < left.Count(); i++)
			{
				for (int j = 0; j < right.Count(); j++)
				{
					if (left.ElementAt(i).IndexOf(right.ElementAt(j), StringComparison.OrdinalIgnoreCase) >= 0 &&
						left.ElementAt(i).Length == right.ElementAt(j).Length)
					{
						res.Add(left.ElementAt(i));
						break;
					}
				}
			}

			return res;
		}

		/// <summary>
		/// Generic IDataReader to use DBNull. An alternative class to utilize DBNull which can't be used in DataTable.
		/// </summary>
		/// <typeparam name="T">Type of a target Entity</typeparam>
		public class EnumerableDataReader<T> : IDataReader
		{
			public EnumerableDataReader(IEnumerable<T> collection)
			{
				if (collection == null)
					throw new ArgumentNullException("collection");

				m_Enumerator = collection.GetEnumerator();

				if (m_Enumerator == null)
					throw new NullReferenceException("collection does not implement GetEnumerator");

				SetFields();
			}
			private IEnumerator<T> m_Enumerator;
			private T m_Current = default(T);
			private bool m_EnumeratorState = false;

			private void SetFields()
			{
				typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.DeclaredOnly)
					.ToList().ForEach(x => m_Fields.Add(new Property(x)));

				typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.DeclaredOnly)
					.ToList().ForEach(x => m_Fields.Add(new Field(x)));
			}
			private List<BaseField> m_Fields = new List<BaseField>();

			#region IDisposable Members
			public void Dispose()
			{
				if (m_Enumerator != null)
				{
					m_Enumerator.Dispose();
					m_Enumerator = null;
					m_Current = default(T);
					m_EnumeratorState = false;
				}
				m_Closed = true;
			}
			#endregion

			#region IDataReader Members
			public void Close()
			{
				m_Closed = true;
			}
			private bool m_Closed = false;

			public int Depth
			{
				get { return 0; }
			}

			public DataTable GetSchemaTable()
			{
				var dt = new DataTable();
				foreach (BaseField field in m_Fields)
				{
					dt.Columns.Add(new DataColumn(field.Name, field.Type));
				}
				return dt;
			}

			public bool IsClosed
			{
				get { return m_Closed; }
			}

			public bool NextResult()
			{
				return false;
			}

			public bool Read()
			{
				if (IsClosed)
					throw new InvalidOperationException("DataReader is closed");
				m_EnumeratorState = m_Enumerator.MoveNext();
				m_Current = m_EnumeratorState ? m_Enumerator.Current : default(T);
				return m_EnumeratorState;
			}

			public int RecordsAffected
			{
				get { return -1; }
			}
			#endregion

			#region IDataRecord Members
			public int FieldCount
			{
				get { return m_Fields.Count; }
			}

			public Type GetFieldType(int i)
			{
				if (i < 0 || i >= m_Fields.Count)
					throw new IndexOutOfRangeException();
				return m_Fields[i].Type;
			}

			public string GetDataTypeName(int i)
			{
				return GetFieldType(i).Name;
			}

			public string GetName(int i)
			{
				if (i < 0 || i >= m_Fields.Count)
					throw new IndexOutOfRangeException();
				return m_Fields[i].Name;
			}

			public int GetOrdinal(string name)
			{
				for (int i = 0; i < m_Fields.Count; i++)
					if (m_Fields[i].Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0 && m_Fields[i].Name.Length == name.Length)
						return i;
				throw new IndexOutOfRangeException("name");
			}

			public bool IsDBNull(int i)
			{
				return GetValue(i) == null;
			}

			public object this[string name]
			{
				get { return GetValue(GetOrdinal(name)); }
			}

			public object this[int i]
			{
				get { return GetValue(i); }
			}

			public object GetValue(int i)
			{
				if (IsClosed || !m_EnumeratorState)
					throw new InvalidOperationException("DataReader is closed or has reached the end of the enumerator");
				if (i < 0 || i >= m_Fields.Count)
					throw new IndexOutOfRangeException();

				if ((m_Fields[i].Type == typeof(DateTime) || m_Fields[i].Type == typeof(DateTime?)) && m_Current == null)
					return (object)DBNull.Value;

				return m_Fields[i].GetValue(m_Current);
			}

			public int GetValues(object[] values)
			{
				int length = Math.Min(m_Fields.Count, values.Length);
				for (int i = 0; i < length; i++)
					values[i] = GetValue(i);
				return length;
			}

			public bool GetBoolean(int i) { return (bool)GetValue(i); }
			public byte GetByte(int i) { return (byte)GetValue(i); }
			public char GetChar(int i) { return (char)GetValue(i); }
			public DateTime GetDateTime(int i) { return (DateTime)GetValue(i); }
			public decimal GetDecimal(int i) { return (decimal)GetValue(i); }
			public double GetDouble(int i) { return (double)GetValue(i); }
			public float GetFloat(int i) { return (float)GetValue(i); }
			public Guid GetGuid(int i) { return (Guid)GetValue(i); }
			public short GetInt16(int i) { return (short)GetValue(i); }
			public int GetInt32(int i) { return (int)GetValue(i); }
			public long GetInt64(int i) { return (long)GetValue(i); }
			public string GetString(int i) { return (string)GetValue(i); }

			public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length) { throw new NotSupportedException(); }
			public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length) { throw new NotSupportedException(); }
			public IDataReader GetData(int i) { throw new NotSupportedException(); }
			#endregion

			#region Helper Classes
			private abstract class BaseField
			{
				public abstract Type Type { get; }
				public abstract string Name { get; }
				public abstract object GetValue(T instance);

				protected static void AddGetter(Type classType, string fieldName, Func<T, object> getter)
				{
					m_GetterDictionary.Add(string.Concat(classType.FullName, fieldName), getter);
				}

				protected static Func<T, object> GetGetter(Type classType, string fieldName)
				{
					Func<T, object> getter = null;
					if (m_GetterDictionary.TryGetValue(string.Concat(classType.FullName, fieldName), out getter))
						return getter;
					return null;
				}
				private static Dictionary<string, Func<T, object>> m_GetterDictionary = new Dictionary<string, Func<T, object>>();
			}

			private class Property : BaseField
			{
				public Property(PropertyInfo info)
				{
					m_Info = info;
					m_DynamicGetter = CreateGetMethod(info);
				}
				private PropertyInfo m_Info;
				private Func<T, object> m_DynamicGetter;

				public override Type Type { get { return m_Info.PropertyType; } }
				public override string Name { get { return m_Info.Name; } }

				public override object GetValue(T instance)
				{
					//return m_Info.GetValue(instance, null); // Reflection is slow
					return m_DynamicGetter(instance);
				}

				// Create dynamic method for faster access instead via reflection
				private Func<T, object> CreateGetMethod(PropertyInfo propertyInfo)
				{
					Type classType = typeof(T);
					Func<T, object> dynamicGetter = GetGetter(classType, propertyInfo.Name);
					if (dynamicGetter == null)
					{
						ParameterExpression instance = Expression.Parameter(classType);
						MemberExpression property = Expression.Property(instance, propertyInfo);
						UnaryExpression convert = Expression.Convert(property, typeof(object));
						dynamicGetter = (Func<T, object>)Expression.Lambda(convert, instance).Compile();
						AddGetter(classType, propertyInfo.Name, dynamicGetter);
					}

					return dynamicGetter;
				}
			}

			private class Field : BaseField
			{
				public Field(FieldInfo info)
				{
					m_Info = info;
					m_DynamicGetter = CreateGetMethod(info);
				}
				private FieldInfo m_Info;
				private Func<T, object> m_DynamicGetter;

				public override Type Type { get { return m_Info.FieldType; } }
				public override string Name { get { return m_Info.Name; } }

				public override object GetValue(T instance)
				{
					//return m_Info.GetValue(instance); // Reflection is slow
					return m_DynamicGetter(instance);
				}

				// Create dynamic method for faster access instead via reflection
				private Func<T, object> CreateGetMethod(FieldInfo fieldInfo)
				{
					Type classType = typeof(T);
					Func<T, object> dynamicGetter = GetGetter(classType, fieldInfo.Name);
					if (dynamicGetter == null)
					{
						ParameterExpression instance = Expression.Parameter(classType);
						MemberExpression property = Expression.Field(instance, fieldInfo);
						UnaryExpression convert = Expression.Convert(property, typeof(object));
						dynamicGetter = (Func<T, object>)Expression.Lambda(convert, instance).Compile();
						AddGetter(classType, fieldInfo.Name, dynamicGetter);
					}

					return dynamicGetter;
				}
			}

			private class Self : BaseField
			{
				public Self()
				{
					m_Type = typeof(T);
				}
				private Type m_Type;

				public override Type Type { get { return m_Type; } }
				public override string Name { get { return string.Empty; } }
				public override object GetValue(T instance) { if (instance == null) return null; return instance; }
			}
			#endregion
		}

		public static bool SqlBulkInsert<T>(IEnumerable<T> list, SqlBulkCopyOptions? sqlBulkCopyOptions = null, int batchSize = -1, string connectionString = null)
		{
			log.Info($"SqlBulkInsert<{typeof(T).Name}> Started. Row count = {list.Count()}");

			try
			{
				lock (BulkInsObj)
				{
					DataTable tbl = new DataTable();
					var connstr = connectionString ?? ConfigurationManager.ConnectionStrings["DSM"].ConnectionString;

					using (var adap = new SqlDataAdapter($" SELECT TOP 0 * FROM [{typeof(T).Name}] --;", connstr))
						adap.Fill(tbl);

					tbl.Rows.Clear();

					var colNames = new List<string>();
					foreach (DataColumn c in tbl.Columns)
					{
						colNames.Add(c.ColumnName);
					}

					// TableLock = true, CheckConstraint = false, KeepIdentity = true, BatchSize = 10000, TimeOut = 60;
					var option = 
						sqlBulkCopyOptions ?? SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.KeepIdentity | SqlBulkCopyOptions.CheckConstraints;

					using (var bulk = new SqlBulkCopy(connstr, option))
					using (var reader = new EnumerableDataReader<T>(list))
					{

						bulk.DestinationTableName = $"dbo.{typeof(T).Name}";
						bulk.BulkCopyTimeout = 60;

						if (batchSize == -1)
							bulk.BatchSize = 10000;
						else
							bulk.BatchSize = batchSize;

						colNames.ForEach(c => bulk.ColumnMappings.Add(c, c));

						log.Info(
							$"SqlBulkCopy.WriteToServer() Start:" + Environment.NewLine +
							$"Row Count = {list.Count()}" + Environment.NewLine +
							$"BatchSize = {bulk.BatchSize}" + Environment.NewLine +
							$"Timeout = {bulk.BulkCopyTimeout}" + Environment.NewLine +
							$"Default = {option.HasFlag(SqlBulkCopyOptions.Default)}" + Environment.NewLine +
							$"KeepIdentity = {option.HasFlag(SqlBulkCopyOptions.KeepIdentity)}" + Environment.NewLine +
							$"CheckConstraints = {option.HasFlag(SqlBulkCopyOptions.CheckConstraints)}" + Environment.NewLine +
							$"TableLock = {option.HasFlag(SqlBulkCopyOptions.TableLock)}" + Environment.NewLine +
							$"KeepNulls = {option.HasFlag(SqlBulkCopyOptions.KeepNulls)}" + Environment.NewLine +
							$"FireTriggers = {option.HasFlag(SqlBulkCopyOptions.FireTriggers)}" + Environment.NewLine +
							$"UseInternalTransaction = {option.HasFlag(SqlBulkCopyOptions.UseInternalTransaction)}" + Environment.NewLine +
							$"AllowEncryptedValueModifications = {option.HasFlag(SqlBulkCopyOptions.AllowEncryptedValueModifications)}"
						);

						bulk.WriteToServer(reader);
						log.Info($"SqlBulkInsert<{typeof(T).Name}> Finished. Row count = {list.Count()}");
						return true;
					}
				}
			}
			catch (Exception ex)
			{
				log.Error(ex.InnerException ?? ex);
				return false;
			}
		}
	}

	public static class ClassExtension
	{
		public static void UnionClass(this object obj, object target)
		{
			if (target != null)
			{
				PropertyInfo[] toProperties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
				PropertyInfo[] fromProperties = target.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
				for (int i = 0; i < toProperties.Count(); i++)
				{
					if (fromProperties.Any(x => x.Name.Equals(toProperties[i].Name)))
					{
						if (toProperties[i].CanWrite && (toProperties[i].PropertyType.IsValueType || toProperties[i].PropertyType.IsEnum || toProperties[i].PropertyType.Equals(typeof(string))))
							toProperties[i].SetValue(obj, fromProperties[fromProperties.ToList().FindIndex(x => x.Name.Equals(toProperties[i].Name))].GetValue(target, null));
					}
				}
			}
		}
		public static void WriteExistings(this object obj, object target)
		{
			PropertyInfo[] toProperties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
			PropertyInfo[] fromProperties = target.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

			for (int i = 0; i < toProperties.Count(); i++)
			{
				if (fromProperties.Any(x => x.Name.Equals(toProperties[i].Name)))
				{
					try
					{
						if (fromProperties[fromProperties.ToList().FindIndex(x => x.Name.Equals(toProperties[i].Name))].GetValue(target, null) != null)
							toProperties[i].SetValue(obj, fromProperties[fromProperties.ToList().FindIndex(x => x.Name.Equals(toProperties[i].Name))].GetValue(target, null));
					}
					catch { }
				}
			}
		}
	}
}