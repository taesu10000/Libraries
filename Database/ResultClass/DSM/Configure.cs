using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using DominoFunctions.Enums;
using DominoFunctions.ExtensionMethod;
using log4net;
using System.Drawing;

namespace DominoDatabase
{
    [Serializable]
    public class Configure
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string PlantCode { get; set; }
        public string Configure_ID { get; set; }
        public string Configure_Value { get; set; }
        public string Reserved1 { get; set; }
        public string Reserved2 { get; set; }
        public string Reserved3 { get; set; }
        public string Reserved4 { get; set; }
        public string Reserved5 { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }

        public Configure()
        {

        }
        public Configure(object list)
        {
            this.UnionClass(list);
        }
        public override string ToString()
        {
            return DominoFunctions.ClassFunctions.ClassToLocalizationString<Configure>(this);
        }
        public bool InsertServer()
        {
            try
            {
                Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure(this), true);
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("Configure InsertServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool UpdateServer()
        {
            try
            {
                Controls.ConfigureController.UpdateServer(new DSM.Dmn_Configure(this), true);
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("Configure UpdateServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        } 
        public static bool InsertDefault(string plantCode)
        {
            try
            {
                List<Configure> _list = Controls.ConfigureController.SelectServer(plantCode);
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.PasswordAge.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.PasswordAge.ToString(),
                        Configure_Value = "90",
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.PasswordErrorLimit.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.PasswordErrorLimit.ToString(),
                        Configure_Value = "3",
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.PasswordExpireWarning.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.PasswordExpireWarning.ToString(),
                        Configure_Value = "30",
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.AutoLogOut.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.AutoLogOut.ToString(),
                        Configure_Value = "30",
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.AutoLogOutWarning.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.AutoLogOutWarning.ToString(),
                        Configure_Value = "5",
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.TimeOutServerSingle.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.TimeOutServerSingle.ToString(),
                        Configure_Value = "5",
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.TimeOutServerList.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.TimeOutServerList.ToString(),
                        Configure_Value = "20",
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.CSVSeparator.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.CSVSeparator.ToString(),
                        Configure_Value = ((int)',').ToString(),
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.PasswordRule.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.PasswordRule.ToString(),
                        Configure_Value = ((long)EnPasswordRule.Default).ToString(),
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.AuditTrailBackupInterval.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.AuditTrailBackupInterval.ToString(),
                        Configure_Value = "30",
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.DatabaseBackupInterval.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.DatabaseBackupInterval.ToString(),
                        Configure_Value = "30",
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.AuditTrailBackupPath.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.AuditTrailBackupPath.ToString(),
                        Configure_Value = @"D:\Backup\AuditTrail\",
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.DatabaseBackupPath.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.DatabaseBackupPath.ToString(),
                        Configure_Value = @"D:\Backup\Database\",
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.LastDatabaseBackup.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.LastDatabaseBackup.ToString(),
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.LastAuditTrailBackup.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.LastDatabaseBackup.ToString(),
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.StartRowOfImportingSerial.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.StartRowOfImportingSerial.ToString(),
                        Configure_Value = "0",
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.RowPerPageDataBaseSearch.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.RowPerPageDataBaseSearch.ToString(),
                        Configure_Value = "20",
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.RowPerPageAuditTrail.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.RowPerPageAuditTrail.ToString(),
                        Configure_Value = "20",
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.MovilitasHost.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.MovilitasHost.ToString(),
                        Configure_Value = "https://api-acc.movilitas.cloud/",
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.MovilitasKey.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.MovilitasKey.ToString(),
                        Configure_Value = "9SfR*_b-f7*D545HGh3j-e3MREcn5Nt7B-e43t**s9e!tDgmntGS-M8S9EMR_FmD",
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.MovilitasSecret.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.MovilitasSecret.ToString(),
                        Configure_Value = "Domino_2019",
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.MovilitasChannel.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.MovilitasChannel.ToString(),
                        Configure_Value = "UbiHLn8gKHm7ptdCDoJye2KoAx3i28aDbjcRyy2d",
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.UseMovilitas.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.UseMovilitas.ToString(),
                        Configure_Value = false.ToString(),
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.InitialStateColor.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.InitialStateColor.ToString(),
                        Configure_Value = Color.Black.ToArgb().ToString(),
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.AssignColor.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.AssignColor.ToString(),
                        Configure_Value = Color.Black.ToArgb().ToString(),
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.AppliedColor.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.AppliedColor.ToString(),
                        Configure_Value = Color.Black.ToArgb().ToString(),
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.PauseColor.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.PauseColor.ToString(),
                        Configure_Value = Color.ForestGreen.ToArgb().ToString(),
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.RunColor.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.RunColor.ToString(),
                        Configure_Value = Color.ForestGreen.ToArgb().ToString(),
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.ProductionCompleteColor.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.ProductionCompleteColor.ToString(),
                        Configure_Value = Color.Navy.ToArgb().ToString(),
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.ReportCompleteColor.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.ReportCompleteColor.ToString(),
                        Configure_Value = Color.Navy.ToArgb().ToString(),
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.AssembleCompleteColor.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.AssembleCompleteColor.ToString(),
                        Configure_Value = Color.Brown.ToArgb().ToString(),
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.DSM_ReportCompleteColor.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.DSM_ReportCompleteColor.ToString(),
                        Configure_Value = Color.Black.ToArgb().ToString(),
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.CancelColor.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.CancelColor.ToString(),
                        Configure_Value = Color.Red.ToArgb().ToString(),
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.Sender_GLN.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.Sender_GLN.ToString(),
                        Configure_Value = "8809861090012",
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.Receiver_GLN.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.Receiver_GLN.ToString(),
                        Configure_Value = "0380777000008",
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.Serial_Count.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.Serial_Count.ToString(),
                        Configure_Value = "100000",
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.SSCC_Count.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.SSCC_Count.ToString(),
                        Configure_Value = "100000",
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.ModelName.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.ModelName.ToString(),
                        Configure_Value = "Data Serialization Manager",
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.BX1_Count.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.BX1_Count.ToString(),
                        Configure_Value = "100000",
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.InterfaceType.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.InterfaceType.ToString(),
                        Configure_Value = "None",
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.UseWdsm.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.UseWdsm.ToString(),
                        Configure_Value = false.ToString(),
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.SyncTime.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.SyncTime.ToString(),
                        Configure_Value = true.ToString(),
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
				if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.ReportOption.ToString())))
				{
					Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
					{
						PlantCode = plantCode,
						Configure_ID = EnDSMConfigure.ReportOption.ToString(),
						Configure_Value = "6",
						InsertUser = "System",
						InsertDate = DateTime.Now
					}, true);
				}
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.InterfaceOption.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.InterfaceOption.ToString(),
                        Configure_Value = "None",
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.ShowSerialTree.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.ShowSerialTree.ToString(),
                        Configure_Value = false.ToString(),
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
				if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.EnableEntitySync.ToString())))
				{
					Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
					{
						PlantCode = "domino",
						Configure_ID = EnDSMConfigure.EnableEntitySync.ToString(),
						Configure_Value = $"{false}",
						InsertUser = "System",
						InsertDate = DateTime.Now
					}, true);
				}
				if (!_list.Any(q => q.Configure_ID.IndexOf($"{EnDSMConfigure.InterfaceDetail}") >= 0 && q.Configure_Value == $"{EnInterfaceDetail.None}"))
				{
					Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
					{
						PlantCode = "domino",
						Configure_ID = $"{EnDSMConfigure.InterfaceDetail}{_list.Count(q => q.Configure_ID.IndexOf($"{EnDSMConfigure.InterfaceDetail}") >= 0)}",
						Configure_Value = $"{EnInterfaceDetail.None}",
						InsertUser = "System",
						InsertDate = DateTime.Now
					}, true);
				}
                if (!_list.Any(q => q.Configure_ID.Equals(EnDSMConfigure.JobOrderFileType.ToString())))
                {
                    Controls.ConfigureController.InsertServer(new DSM.Dmn_Configure
                    {
                        PlantCode = plantCode,
                        Configure_ID = EnDSMConfigure.JobOrderFileType.ToString(),
                        Configure_Value = "None",
                        InsertUser = "System",
                        InsertDate = DateTime.Now
                    }, true);
                }
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("Configure InsertDefault Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public Configure Clone()
        {
            return (Configure)this.MemberwiseClone();
        }
    }
}
