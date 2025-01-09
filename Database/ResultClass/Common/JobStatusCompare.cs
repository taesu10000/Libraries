using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using DominoFunctions.ExtensionMethod;
using log4net;
using Newtonsoft.Json;
using DominoFunctions.Enums;

namespace DominoDatabase
{
    [Serializable]
    public class JobStatusCompare
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public enErrorPriority RtnCode { get; set; }

        public string RtnMsg { get; set; }


        public List<JobStatusCompare_Item> JobStatusCompare_Items { get; set; } = new List<JobStatusCompare_Item>();

        public JobStatusCompare()
        {
        }
    }


    public class JobStatusCompare_Item
    {
        public string SerialNum { get; set; }
        public string JobDetailType { get; set; }
        public string StatusAG { get; set; }
        public string StatusPM { get; set; }
        public string RtnMessage { get; set; }
        public enErrorPriority RtnCode { get; set; } = enErrorPriority.None;
    }
    public enum enErrorPriority
    {
        None = 0,
        NoPMData = 100,
        NoPMDataInAG = 110,
        RequirCheckPMVisionResult = 120,
        PMDataDuplicated = 130,
        DifferentStatus = 140,
        Unknown = 999
    }
}
