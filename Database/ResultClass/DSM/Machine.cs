using DominoFunctions.Enums;
using log4net;
using Newtonsoft.Json;
using System;

namespace DominoDatabase
{
	[Serializable]
    public class Machine
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string PlantCode { get; set; }
        public string MachineID { get; set; }
        public string MachineStatus { get; set; }
        [JsonIgnore]
        public EnMachineStatus EnMachineStatus
        {
            get
            {
                if (string.IsNullOrEmpty(MachineStatus))
                    return EnMachineStatus.Idle;
                return (EnMachineStatus)Enum.Parse(typeof(EnMachineStatus), MachineStatus);
            }
            set
            {
                MachineStatus = value.ToString();
            }
        }
        public string MachineType { get; set; }
        public string MachineName { get; set; }
        public string IPAddress { get; set; }
        public string AGAdditionalYN { get; set; }
        public string LineInfo { get; set; }
        public string UseYN { get; set; }
        public string LastOrderNo { get; set; }
        public string LastSeqNo { get; set; }
        public string Reserved1 { get; set; }
        public string Reserved2 { get; set; }
        public string Reserved3 { get; set; }
        public string Reserved4 { get; set; }
        public string Reserved5 { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        [JsonIgnore]
        public EnMachineType EnMachineType
        {
            get
            {
                switch(MachineType)
                {
                    case "LBL":
                        return EnMachineType.Labeler;
                    case "PM":
                        return EnMachineType.PrintingStation;
                    case "AG":
                        return EnMachineType.AggregationStation;
                    case "DSM":
                        return EnMachineType.DSM;
                    case "PNL":
						return EnMachineType.PrintingStationAndLabeler;
					default:
                        throw new NotImplementedException();
                }
            }
            set
            {
                switch(value)
                {
                    case EnMachineType.Labeler:
                        MachineType = "LBL";
                        break;
                    case EnMachineType.PrintingStation:
                        MachineType = "PM";
                        break;
                    case EnMachineType.AggregationStation:
                        MachineType = "AG";
                        break;
                    case EnMachineType.DSM:
                        MachineType = "DSM";
                        break;
					case EnMachineType.PrintingStationAndLabeler:
						MachineType = "PNL";
						break;
				}
            }
        }


        public Machine()
        {

        }
        public Machine(object list)
        {
            this.UnionClass(list);
        }
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.PlantCode, PlantCode);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.MachineID, MachineID);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.MachineName, MachineName);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.AdditionalAG, AGAdditionalYN);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved1, Reserved1);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved2, Reserved2);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved3, Reserved3);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved4, Reserved4);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved5, Reserved5);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.InsertUser, InsertUser);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.InsertDate, InsertDate);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.UpdateUser, UpdateUser);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.UpdateDate, UpdateDate);
            return sb.ToString();
        }
        public bool InsertServer()
        {
            try
            {
                Controls.MachineController.InsertServer(new DSM.Dmn_Machine (this), true);
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("Machine InsertServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool UpdateServer()
        {
            try
            {
                Controls.MachineController.UpdateServer(new DSM.Dmn_Machine(this), true);
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("Machine UpdateServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public Machine Clone()
        {
            return (Machine)this.MemberwiseClone();
        }
    }
}

