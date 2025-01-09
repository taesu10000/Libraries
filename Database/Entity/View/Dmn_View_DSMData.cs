using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DominoDatabase.DSM
{
	public class Dmn_View_DSMData
	{
		[Key]
		[Column(Order = 0)]
		public string ProdStdCode { get; set; }
		[Key]
		[Column(Order = 1)]
		public string SerialNum { get; set; }
		public string SP_SerialNum { get; set; }
		public string RB_SerialNum { get; set; }
		public string MachineID { get; set; }
		public string SP_MachineID { get; set; }
		public string RB_MachineID { get; set; }
		[Key]
		[Column(Order = 2)]
		public string JobDetailType { get; set; }
		[Required]
		public string OrderNo { get; set; }
		[Required]
		public string SeqNo { get; set; }
		public string ResourceType { get; set; }
		public string SerialType { get; set; }
		public long? Idx_Group { get; set; }
		public long? Idx_Insert { get; set; }
		public DateTime? UseDate { get; set; }
		public DateTime? InspectedDate { get; set; }
		public string UseYN { get; set; }
		public string Status { get; set; }
		public string FileName { get; set; }
		public string SP_Reserved1 { get; set; }
		public string SP_Reserved2 { get; set; }
		public string SP_Reserved3 { get; set; }
		public string SP_Reserved4 { get; set; }
		public string SP_Reserved5 { get; set; }
		[Required]
		public string SP_InsertUser { get; set; }
		[Required]
		public DateTime? SP_InsertDate { get; set; }
		public string SP_UpdateUser { get; set; }
		public DateTime? SP_UpdateDate { get; set; }
		public string IsPass { get; set; }
		public string IsReject { get; set; }
		public string IsSample { get; set; }
		public string DecodedBarcode { get; set; }
		public string Read_OCR { get; set; }
		public string Grade_Barcode { get; set; }
		public string FilePath { get; set; }
		public string VR_FilePath { get; set; }
		public string DB_FilePath { get; set; }
		public int? CameraIndex { get; set; }
		public string VR_Reserved1 { get; set; }
		public string VR_Reserved2 { get; set; }
		public string VR_Reserved3 { get; set; }
		public string VR_Reserved4 { get; set; }
		public string VR_Reserved5 { get; set; }
		[Required]
		public string VR_InsertUser { get; set; }
		[Required]
		public DateTime? VR_InsertDate { get; set; }
		public string VR_UpdateUser { get; set; }
		public DateTime? VR_UpdateDate { get; set; }
		public string LineID { get; set; }
		public string CorCode { get; set; }
		public string PlantCode { get; set; }
		public string ErpOrderNo { get; set; }
		public string OrderType { get; set; }
		public DateTime? MfdDate { get; set; }
		public DateTime? ExpDate { get; set; }
        public DateTime? DateOfTest { get; set; }
		public string LotNo { get; set; }
		public string LotNo_Sub { get; set; }
		public int? Cnt_JobPlan { get; set; }
		public string JM_Reserved1 { get; set; }
		public string JM_Reserved2 { get; set; }
		public string JM_Reserved3 { get; set; }
		public string JM_Reserved4 { get; set; }
		public string JM_Reserved5 { get; set; }
		public string DSMReportUser { get; set; }
		public DateTime? DSMReportDate { get; set; }
		public DateTime? AssignDate { get; set; }
		public string AssignUser { get; set; }
		public DateTime? JM_InsertDate { get; set; }
		public string JM_InsertUser { get; set; }
		public DateTime? JM_UpdateDate { get; set; }
		public string JM_UpdateUser { get; set; }
		public int? Cnt_Good { get; set; }
		public int? Cnt_Error { get; set; }
		public int? Cnt_Sample { get; set; }
		public int? Cnt_Destroy { get; set; }
		public int? Cnt_Status1 { get; set; }
		public int? Cnt_Status2 { get; set; }
		public int? Cnt_Status3 { get; set; }
		public int? Cnt_Status4 { get; set; }
		public int? Cnt_Status5 { get; set; }
		public string UserDefineData1 { get; set; }
		public string UserDefineData2 { get; set; }
		public string PrinterVariable1 { get; set; }
		public string PrinterVariable2 { get; set; }
		public string PrinterVariable3 { get; set; }
		public string PrinterVariable4 { get; set; }
		public string PrinterVariable5 { get; set; }

		public string JD_Reserved1 { get; set; }
		public string JD_Reserved2 { get; set; }
		public string JD_Reserved3 { get; set; }
		public string JD_Reserved4 { get; set; }
		public string JD_Reserved5 { get; set; }
		public string StartUser { get; set; }
		public DateTime? StartDate { get; set; }
		public string CompleteUser { get; set; }
		public DateTime? CompleteDate { get; set; }
		public string ProdCode { get; set; }
		public string ProdName { get; set; }
		public string ProdName2 { get; set; }
		public int? AGLevel { get; set; }
		public string Remark { get; set; }
		public int? EXP_OFFSET { get; set; }
		public string PM_Reserved1 { get; set; }
		public string PM_Reserved2 { get; set; }
		public string PM_Reserved3 { get; set; }
		public string PM_Reserved4 { get; set; }
		public string PM_Reserved5 { get; set; }
		public string PM_InsertUser { get; set; }
		public DateTime? PM_InsertDate { get; set; }
		public string PM_UpdateUser { get; set; }
		public DateTime? PM_UpdateDate { get; set; }
		public string BarcodeType { get; set; }
		public string BarcodeDataFormat { get; set; }
		public string SerialNumberType { get; set; }
		public string SnExpressionID { get; set; }
		public string Capacity { get; set; }
		public string LIC { get; set; }
		public string PCN { get; set; }
		public string Condition { get; set; }
		public string ProdStdCodeChild { get; set; }
		public int? PackingCount { get; set; }
		public string Prefix_SSCC { get; set; }
        public string Price { get; set; }
        public decimal? MinimumWeight { get; set; }
        public decimal? MaximumWeight { get; set; }
		public string PD_Reserved1 { get; set; }
		public string PD_Reserved2 { get; set; }
		public string PD_Reserved3 { get; set; }
		public string PD_Reserved4 { get; set; }
		public string PD_Reserved5 { get; set; }
		public string PD_InsertUser { get; set; }
		public DateTime? PD_InsertDate { get; set; }
		public string PD_UpdateUser { get; set; }
		public DateTime? PD_UpdateDate { get; set; }
		public string EA_Serial { get; set; }
		public string BX1_Serial { get; set; }
		public string BX2_Serial { get; set; }
		public string BX3_Serial { get; set; }
		public string BX4_Serial { get; set; }
		public string BX5_Serial { get; set; }
		public string BX6_Serial { get; set; }
		public string EA_FullBarcode_Read { get; set; }
		public string BX1_FullBarcode_Read { get; set; }
		public string BX2_FullBarcode_Read { get; set; }
		public string BX3_FullBarcode_Read { get; set; }
		public string BX4_FullBarcode_Read { get; set; }
		public string BX5_FullBarcode_Read { get; set; }
		public string BX6_FullBarcode_Read { get; set; }
		public string EA_AI_FullBarcode_Read { get; set; }
		public string BX1_AI_FullBarcode_Read { get; set; }
		public string BX2_AI_FullBarcode_Read { get; set; }
		public string BX3_AI_FullBarcode_Read { get; set; }
		public string BX4_AI_FullBarcode_Read { get; set; }
		public string BX5_AI_FullBarcode_Read { get; set; }
		public string BX6_AI_FullBarcode_Read { get; set; }
		public string FullBarcode_Read { get; set; }
		public string AI_FullBarcode_Read { get; set; }
		public string FullBarcode_Parent { get; set; }
		public string AI_FullBarcode_Parent { get; set; }
		public string ParentProdStdCode { get; set; }
		public string ParentSerialNum { get; set; }
		public string RB_Reserved1 { get; set; }
		public string RB_Reserved2 { get; set; }
		public string RB_Reserved3 { get; set; }
		public string RB_Reserved4 { get; set; }
		public string RB_Reserved5 { get; set; }
		public string RB_InsertUser { get; set; }
		public DateTime? RB_InsertDate { get; set; }
		public string RB_UpdateUser { get; set; }
		public DateTime? RB_UpdateDate { get; set; }
		public string UI_UserName { get; set; }
		public string UU_UserName { get; set; }
		public override string ToString()
		{
			return DominoFunctions.ClassFunctions.ClassToLocalizationString<Dmn_View_DSMData>(this);
		}
		public Dmn_View_DSMData()
		{
		}
		public Dmn_View_DSMData(object obj)
			: this()
		{
			this.UnionClass(obj);
		}
		public Dmn_View_DSMData Clone()
		{
			return (Dmn_View_DSMData)this.MemberwiseClone();
		}

	}
}
