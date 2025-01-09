using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominoDatabase
{
    [Serializable]
    public class Common
    {
        public string CDCode { get; set; }
        public string CDName { get; set; }
        public string UseYN { get; set; }
        public DateTime? InsertDate { get; set; }
        public string InsertUser { get; set; }
        public DateTime? UpdateDate { get; set; }

        public string UpdateUser { get; set; }

        public static void CreateDefaultCommonCodeLocal()
        {
            List<Common> tmp = Controls.CommonCodeController.SelectLocal(null, null, null);
            DateTime insertTime = DateTime.Now;
            if (tmp.Count == 0)
            {
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_M
                {
                    CDCode = "101",
                    CDName = "포장타입",
                    UseYN = "Y",
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_M
                {
                    CDCode = "102",
                    CDName = "서버타입",
                    UseYN = "Y",
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_M
                {
                    CDCode = "103",
                    CDName = "시리얼고정생성키",
                    UseYN = "Y",
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_M
                {
                    CDCode = "190",
                    CDName = "시리얼타입",
                    UseYN = "Y",
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_M
                {
                    CDCode = "191",
                    CDName = "리소스타입",
                    UseYN = "Y",
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_M
                {
                    CDCode = "192",
                    CDName = "제품상태",
                    UseYN = "Y",
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_M
                {
                    CDCode = "193",
                    CDName = "사용유무",
                    UseYN = "Y",
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_M
                {
                    CDCode = "201",
                    CDName = "작업지시상태",
                    UseYN = "Y",
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "101",
                    CDCode_Dtl = "001",
                    CDCode_Name = "EA",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "101",
                    CDCode_Dtl = "002",
                    CDCode_Name = "BX1",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "101",
                    CDCode_Dtl = "003",
                    CDCode_Name = "BX2",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "101",
                    CDCode_Dtl = "004",
                    CDCode_Name = "BX3",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "101",
                    CDCode_Dtl = "005",
                    CDCode_Name = "BX4",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "101",
                    CDCode_Dtl = "006",
                    CDCode_Name = "BX5",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "101",
                    CDCode_Dtl = "007",
                    CDCode_Name = "BX6",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "101",
                    CDCode_Dtl = "008",
                    CDCode_Name = "BX7",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "101",
                    CDCode_Dtl = "009",
                    CDCode_Name = "BX8",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "102",
                    CDCode_Dtl = "001",
                    CDCode_Name = "DSM",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "102",
                    CDCode_Dtl = "002",
                    CDCode_Name = "STANDALONE",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "102",
                    CDCode_Dtl = "003",
                    CDCode_Name = "Kiedas",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "102",
                    CDCode_Dtl = "004",
                    CDCode_Name = "ERP",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "102",
                    CDCode_Dtl = "005",
                    CDCode_Name = "Movilitas",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "1",
                    CDCode_Name = "ProductCode",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "2",
                    CDCode_Name = "Lot",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "3",
                    CDCode_Name = "LineNo",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "4",
                    CDCode_Name = "FIX",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "5",
                    CDCode_Name = "FIX_ORDER",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "101",
                    CDCode_Name = "MFD_YY",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "102",
                    CDCode_Name = "MFD_YYMM",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "103",
                    CDCode_Name = "MFD_YYMMDD",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "104",
                    CDCode_Name = "MFD_YYYY",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "105",
                    CDCode_Name = "MFD_YYYYMM",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "106",
                    CDCode_Name = "MFD_YYYYMMDD",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "107",
                    CDCode_Name = "MFD_MM",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "108",
                    CDCode_Name = "MMFD_MMDDFD_MM",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "109",
                    CDCode_Name = "MFD_DD",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "201",
                    CDCode_Name = "EXP_YY",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "202",
                    CDCode_Name = "EXP_YYMM",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "203",
                    CDCode_Name = "EXP_YYMMDD",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "204",
                    CDCode_Name = "EXP_YYYY",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "205",
                    CDCode_Name = "EXP_YYYYMM",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "206",
                    CDCode_Name = "EXP_YYYYMMDD",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "207",
                    CDCode_Name = "EXP_MM",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "208",
                    CDCode_Name = "EXP_MMDD",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "209",
                    CDCode_Name = "EXP_DD",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "901",
                    CDCode_Name = "LEGACY",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "902",
                    CDCode_Name = "Pd_YY",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "903",
                    CDCode_Name = "Pd_YYMM",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "904",
                    CDCode_Name = "Pd_YYMMDD",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "905",
                    CDCode_Name = "Pd_YYYY",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "906",
                    CDCode_Name = "Pd_YYYYMM",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "907",
                    CDCode_Name = "Pd_YYYYMMDD",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "908",
                    CDCode_Name = "Order",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "909",
                    CDCode_Name = "Exp_YYMMDD",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "910",
                    CDCode_Name = "LineNo",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "911",
                    CDCode_Name = "FIX",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "912",
                    CDCode_Name = "ORDER_YY",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "913",
                    CDCode_Name = "ORDER_YYMM",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "914",
                    CDCode_Name = "ORDER_YYMMDD",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "915",
                    CDCode_Name = "ORDER_YYYY",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "916",
                    CDCode_Name = "ORDER_YYYYMM",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "917",
                    CDCode_Name = "ORDER_YYYYMMDD",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "918",
                    CDCode_Name = "ORDER_M_HEXA",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "919",
                    CDCode_Name = "ORDER_DD",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "920",
                    CDCode_Name = "ORDER_Seq",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "921",
                    CDCode_Name = "ORDER_YYYY_Code",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "922",
                    CDCode_Name = "Packing_Code",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "923",
                    CDCode_Name = "Packing_Code_2",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "924",
                    CDCode_Name = "Sub_Lot",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "925",
                    CDCode_Name = "ERP_ORDER_NO",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "926",
                    CDCode_Name = "GTIN_3",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "927",
                    CDCode_Name = "MFD_YY",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "190",
                    CDCode_Dtl = "N",
                    CDCode_Name = "None",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "190",
                    CDCode_Dtl = "R",
                    CDCode_Name = "ReceivedSerialNumber",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "190",
                    CDCode_Dtl = "S",
                    CDCode_Name = "ScanSerialNumber",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "190",
                    CDCode_Dtl = "C",
                    CDCode_Name = "CreateSerialNumber",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "191",
                    CDCode_Dtl = "L",
                    CDCode_Name = "LocalCreate",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "191",
                    CDCode_Dtl = "D",
                    CDCode_Name = "DSM",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "191",
                    CDCode_Dtl = "F",
                    CDCode_Name = "File",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "191",
                    CDCode_Dtl = "M",
                    CDCode_Name = "Movilitas",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "191",
                    CDCode_Dtl = "E",
                    CDCode_Name = "ERP",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "192",
                    CDCode_Dtl = "PA",
                    CDCode_Name = "Pass",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "192",
                    CDCode_Dtl = "RE",
                    CDCode_Name = "Reject",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "192",
                    CDCode_Dtl = "NU",
                    CDCode_Name = "Notused",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "192",
                    CDCode_Dtl = "OW",
                    CDCode_Name = "OverWeight",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "192",
                    CDCode_Dtl = "UW",
                    CDCode_Name = "UnderWeight",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "192",
                    CDCode_Dtl = "SN",
                    CDCode_Name = "Sample_Normal",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "192",
                    CDCode_Dtl = "ST",
                    CDCode_Name = "Sample_Test",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "192",
                    CDCode_Dtl = "SS",
                    CDCode_Name = "Sample_Storage",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "192",
                    CDCode_Dtl = "SC",
                    CDCode_Name = "Sample_QC",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "192",
                    CDCode_Dtl = "SA",
                    CDCode_Name = "Sample_QA",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "192",
                    CDCode_Dtl = "NF",
                    CDCode_Name = "NotForSale",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "192",
                    CDCode_Dtl = "DE",
                    CDCode_Name = "Destroy",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "192",
                    CDCode_Dtl = "CC",
                    CDCode_Name = "Cancel",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "193",
                    CDCode_Dtl = "Y",
                    CDCode_Name = "Yes",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "193",
                    CDCode_Dtl = "N",
                    CDCode_Name = "No",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "201",
                    CDCode_Dtl = "NM",
                    CDCode_Name = "Normal",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "201",
                    CDCode_Dtl = "TS",
                    CDCode_Name = "Test",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "201",
                    CDCode_Dtl = "MA",
                    CDCode_Name = "Manual",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "201",
                    CDCode_Dtl = "TP",
                    CDCode_Name = "TestPrint",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "201",
                    CDCode_Dtl = "MP",
                    CDCode_Name = "ManualPrint",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertLocal(new Dmn_CommonCode_D
                {
                    CDCode = "201",
                    CDCode_Dtl = "RE",
                    CDCode_Name = "Repack",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
            }
        }
        public static void CreateDefaultCommonCodeServer()
        {
            List<Common> tmp = Controls.CommonCodeController.SelectServer();
            DateTime insertTime = DateTime.Now;
            if (tmp.Count == 0)
            {
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_M
                {
                    CDCode = "101",
                    CDName = "포장타입",
                    UseYN = "Y",
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_M
                {
                    CDCode = "102",
                    CDName = "서버타입",
                    UseYN = "Y",
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_M
                {
                    CDCode = "103",
                    CDName = "시리얼고정생성키",
                    UseYN = "Y",
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_M
                {
                    CDCode = "190",
                    CDName = "시리얼타입",
                    UseYN = "Y",
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_M
                {
                    CDCode = "191",
                    CDName = "리소스타입",
                    UseYN = "Y",
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_M
                {
                    CDCode = "192",
                    CDName = "제품상태",
                    UseYN = "Y",
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_M
                {
                    CDCode = "193",
                    CDName = "사용유무",
                    UseYN = "Y",
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_M
                {
                    CDCode = "201",
                    CDName = "작업지시상태",
                    UseYN = "Y",
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "101",
                    CDCode_Dtl = "001",
                    CDCode_Name = "EA",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "101",
                    CDCode_Dtl = "002",
                    CDCode_Name = "BX1",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "101",
                    CDCode_Dtl = "003",
                    CDCode_Name = "BX2",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "101",
                    CDCode_Dtl = "004",
                    CDCode_Name = "BX3",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "101",
                    CDCode_Dtl = "005",
                    CDCode_Name = "BX4",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "101",
                    CDCode_Dtl = "006",
                    CDCode_Name = "BX5",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "101",
                    CDCode_Dtl = "007",
                    CDCode_Name = "BX6",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "101",
                    CDCode_Dtl = "008",
                    CDCode_Name = "BX7",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "101",
                    CDCode_Dtl = "009",
                    CDCode_Name = "BX8",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "102",
                    CDCode_Dtl = "001",
                    CDCode_Name = "DSM",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "102",
                    CDCode_Dtl = "002",
                    CDCode_Name = "STANDALONE",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "102",
                    CDCode_Dtl = "003",
                    CDCode_Name = "Kiedas",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "102",
                    CDCode_Dtl = "004",
                    CDCode_Name = "ERP",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "102",
                    CDCode_Dtl = "005",
                    CDCode_Name = "Movilitas",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "1",
                    CDCode_Name = "ProductCode",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "2",
                    CDCode_Name = "Lot",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "3",
                    CDCode_Name = "LineNo",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "4",
                    CDCode_Name = "FIX",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "5",
                    CDCode_Name = "FIX_ORDER",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "101",
                    CDCode_Name = "MFD_YY",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "102",
                    CDCode_Name = "MFD_YYMM",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "103",
                    CDCode_Name = "MFD_YYMMDD",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "104",
                    CDCode_Name = "MFD_YYYY",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "105",
                    CDCode_Name = "MFD_YYYYMM",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "106",
                    CDCode_Name = "MFD_YYYYMMDD",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "107",
                    CDCode_Name = "MFD_MM",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "108",
                    CDCode_Name = "MMFD_MMDDFD_MM",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "109",
                    CDCode_Name = "MFD_DD",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "201",
                    CDCode_Name = "EXP_YY",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "202",
                    CDCode_Name = "EXP_YYMM",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "203",
                    CDCode_Name = "EXP_YYMMDD",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "204",
                    CDCode_Name = "EXP_YYYY",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "205",
                    CDCode_Name = "EXP_YYYYMM",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "206",
                    CDCode_Name = "EXP_YYYYMMDD",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "207",
                    CDCode_Name = "EXP_MM",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "208",
                    CDCode_Name = "EXP_MMDD",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "209",
                    CDCode_Name = "EXP_DD",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "901",
                    CDCode_Name = "LEGACY",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "902",
                    CDCode_Name = "Pd_YY",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "903",
                    CDCode_Name = "Pd_YYMM",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "904",
                    CDCode_Name = "Pd_YYMMDD",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "905",
                    CDCode_Name = "Pd_YYYY",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "906",
                    CDCode_Name = "Pd_YYYYMM",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "907",
                    CDCode_Name = "Pd_YYYYMMDD",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "908",
                    CDCode_Name = "Order",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "909",
                    CDCode_Name = "Exp_YYMMDD",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "910",
                    CDCode_Name = "LineNo",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "911",
                    CDCode_Name = "FIX",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "912",
                    CDCode_Name = "ORDER_YY",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "913",
                    CDCode_Name = "ORDER_YYMM",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "914",
                    CDCode_Name = "ORDER_YYMMDD",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "915",
                    CDCode_Name = "ORDER_YYYY",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "916",
                    CDCode_Name = "ORDER_YYYYMM",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "917",
                    CDCode_Name = "ORDER_YYYYMMDD",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "918",
                    CDCode_Name = "ORDER_M_HEXA",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "919",
                    CDCode_Name = "ORDER_DD",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "920",
                    CDCode_Name = "ORDER_Seq",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "921",
                    CDCode_Name = "ORDER_YYYY_Code",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "922",
                    CDCode_Name = "Packing_Code",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "923",
                    CDCode_Name = "Packing_Code_2",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "924",
                    CDCode_Name = "Sub_Lot",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "925",
                    CDCode_Name = "ERP_ORDER_NO",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "926",
                    CDCode_Name = "GTIN_3",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "103",
                    CDCode_Dtl = "927",
                    CDCode_Name = "MFD_YY",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "190",
                    CDCode_Dtl = "N",
                    CDCode_Name = "None",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "190",
                    CDCode_Dtl = "R",
                    CDCode_Name = "ReceivedSerialNumber",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "190",
                    CDCode_Dtl = "S",
                    CDCode_Name = "ScanSerialNumber",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "190",
                    CDCode_Dtl = "C",
                    CDCode_Name = "CreateSerialNumber",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "191",
                    CDCode_Dtl = "L",
                    CDCode_Name = "LocalCreate",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "191",
                    CDCode_Dtl = "D",
                    CDCode_Name = "DSM",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "191",
                    CDCode_Dtl = "F",
                    CDCode_Name = "File",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "191",
                    CDCode_Dtl = "M",
                    CDCode_Name = "Movilitas",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "191",
                    CDCode_Dtl = "E",
                    CDCode_Name = "ERP",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "192",
                    CDCode_Dtl = "PA",
                    CDCode_Name = "Pass",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "192",
                    CDCode_Dtl = "RE",
                    CDCode_Name = "Reject",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "192",
                    CDCode_Dtl = "NU",
                    CDCode_Name = "Notused",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "192",
                    CDCode_Dtl = "OW",
                    CDCode_Name = "OverWeight",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "192",
                    CDCode_Dtl = "UW",
                    CDCode_Name = "UnderWeight",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "192",
                    CDCode_Dtl = "SN",
                    CDCode_Name = "Sample_Normal",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "192",
                    CDCode_Dtl = "ST",
                    CDCode_Name = "Sample_Test",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "192",
                    CDCode_Dtl = "SS",
                    CDCode_Name = "Sample_Storage",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "192",
                    CDCode_Dtl = "SC",
                    CDCode_Name = "Sample_QC",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "192",
                    CDCode_Dtl = "SA",
                    CDCode_Name = "Sample_QA",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "192",
                    CDCode_Dtl = "NF",
                    CDCode_Name = "NotForSale",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "192",
                    CDCode_Dtl = "DE",
                    CDCode_Name = "Destroy",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "192",
                    CDCode_Dtl = "CC",
                    CDCode_Name = "Cancel",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "193",
                    CDCode_Dtl = "Y",
                    CDCode_Name = "Yes",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "193",
                    CDCode_Dtl = "N",
                    CDCode_Name = "No",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "201",
                    CDCode_Dtl = "NM",
                    CDCode_Name = "Normal",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "201",
                    CDCode_Dtl = "TS",
                    CDCode_Name = "Test",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "201",
                    CDCode_Dtl = "MA",
                    CDCode_Name = "Manual",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "201",
                    CDCode_Dtl = "TP",
                    CDCode_Name = "TestPrint",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "201",
                    CDCode_Dtl = "MP",
                    CDCode_Name = "ManualPrint",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
                Controls.CommonCodeController.InsertServer(new Dmn_CommonCode_D
                {
                    CDCode = "201",
                    CDCode_Dtl = "RE",
                    CDCode_Name = "Repack",
                    UseYN = "Y",
                    Code_Value1 = null,
                    Code_Value2 = null,
                    Code_Value3 = null,
                    InsertDate = insertTime,
                    InsertUser = "System",
                    UpdateDate = insertTime,
                    UpdateUser = "System"
                });
            }
        }


        public Common()
        { }
        public Common(object list)
            : this()
        {
            this.UnionClass(list);
        }
    }
}
