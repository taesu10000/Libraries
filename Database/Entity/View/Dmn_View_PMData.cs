using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DominoFunctions.ExtensionMethod;

namespace DominoDatabase.Local
{
    public class Dmn_View_PMData
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string ProdStdCode { get; set; }
        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string SerialNum { get; set; }
        [Key]
        [Column(Order = 2)]
        [StringLength(3)]
        public string JobDetailType { get; set; }
        [Required]
        [StringLength(50)]
        public string OrderNo { get; set; }
        [Required]
        [StringLength(4)]
        public string SeqNo { get; set; }
        [StringLength(200)]
        public string ResourceType { get; set; }
        [StringLength(200)]
        public string SerialType { get; set; }
        public long? idx_Group { get; set; }
        public long? idx_Insert { get; set; }
        public DateTime? UseDate { get; set; }
        public DateTime? InspectedDate { get; set; }
        [StringLength(200)]
        public string UseYN { get; set; }
        [StringLength(200)]
        public string Status { get; set; }
        [StringLength(512)]
        public string FileName { get; set; }
        [StringLength(90)]
        public string SP_Reserved1 { get; set; }
        [StringLength(90)]
        public string SP_Reserved2 { get; set; }
        [StringLength(90)]
        public string SP_Reserved3 { get; set; }
        [StringLength(90)]
        public string SP_Reserved4 { get; set; }
        [StringLength(90)]
        public string SP_Reserved5 { get; set; }
        [Required]
        [StringLength(50)]
        public string SP_InsertUser { get; set; }
        [Required]
        public DateTime? SP_InsertDate { get; set; }
        [StringLength(50)]
        public string SP_UpdateUser { get; set; }
        public DateTime? SP_UpdateDate { get; set; }
        [StringLength(300)]
        public string DecodedBarcode { get; set; }
        [StringLength(512)]
        public string Read_OCR { get; set; }
        [StringLength(1)]
        public string Grade_Barcode { get; set; }
        [StringLength(512)]
        public string FilePath { get; set; }
        public int? CameraIndex { get; set; }
        [StringLength(90)]
        public string VR_Reserved1 { get; set; }
        [StringLength(90)]
        public string VR_Reserved2 { get; set; }
        [StringLength(90)]
        public string VR_Reserved3 { get; set; }
        [StringLength(90)]
        public string VR_Reserved4 { get; set; }
        [StringLength(90)]
        public string VR_Reserved5 { get; set; }
        [Required]
        [StringLength(50)]
        public string VR_InsertUser { get; set; }
        [Required]
        public DateTime? VR_InsertDate { get; set; }
        [StringLength(50)]
        public string VR_UpdateUser { get; set; }
        public DateTime? VR_UpdateDate { get; set; }
        [StringLength(20)]
        public string LineID { get; set; }
        [StringLength(20)]
        public string CorCode { get; set; }
        [StringLength(20)]
        public string PlantCode { get; set; }
        [StringLength(50)]
        public string ErpOrderNo { get; set; }
        [StringLength(200)]
        public string OrderType { get; set; }
        public DateTime? MfdDate { get; set; }
        public DateTime? ExpDate { get; set; }
        [StringLength(20)]
        public string LotNo { get; set; }
        [StringLength(20)]
        public string LotNo_Sub { get; set; }
        public int? Cnt_JobPlan { get; set; }
        [StringLength(90)]
        public string JM_Reserved1 { get; set; }
        [StringLength(90)]
        public string JM_Reserved2 { get; set; }
        [StringLength(90)]
        public string JM_Reserved3 { get; set; }
        [StringLength(90)]
        public string JM_Reserved4 { get; set; }
        [StringLength(90)]
        public string JM_Reserved5 { get; set; }
        public DateTime? AssignDate { get; set; }
        [StringLength(50)]
        public string AssignUser { get; set; }
        public DateTime? JM_InsertDate { get; set; }
        [StringLength(50)]
        public string JM_InsertUser { get; set; }
        public DateTime? JM_UpdateDate { get; set; }
        [StringLength(50)]
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
        [StringLength(50)]
        public string UserDefineData1 { get; set; }
        [StringLength(50)]
        public string UserDefineData2 { get; set; }
        [StringLength(100)]
        public string PrinterVariable1 { get; set; }
        [StringLength(100)]
        public string PrinterVariable2 { get; set; }
        [StringLength(100)]
        public string PrinterVariable3 { get; set; }
        [StringLength(100)]
        public string PrinterVariable4 { get; set; }
        [StringLength(100)]
        public string PrinterVariable5 { get; set; }
        [StringLength(90)]
        public string JD_Reserved1 { get; set; }
        [StringLength(90)]
        public string JD_Reserved2 { get; set; }
        [StringLength(90)]
        public string JD_Reserved3 { get; set; }
        [StringLength(90)]
        public string JD_Reserved4 { get; set; }
        [StringLength(90)]
        public string JD_Reserved5 { get; set; }
        [StringLength(50)]
        public string StartUser { get; set; }
        public DateTime? StartDate { get; set; }
        [StringLength(50)]
        public string CompleteUser { get; set; }
        public DateTime? CompleteDate { get; set; }
        [StringLength(50)]
        public string ProdCode { get; set; }
        [StringLength(200)]
        public string ProdName { get; set; }
        [StringLength(200)]
        public string ProdName2 { get; set; }
        public int? AGLevel { get; set; }
        [StringLength(200)]
        public string Remark { get; set; }
        public int? EXP_OFFSET { get; set; }
        public int? Delay_Print { get; set; }
        public int? Delay_Shot1 { get; set; }
        public int? Delay_Shot2 { get; set; }
        public int? Delay_NG { get; set; }
        [StringLength(90)]
        public string PM_Reserved1 { get; set; }
        [StringLength(90)]
        public string PM_Reserved2 { get; set; }
        [StringLength(90)]
        public string PM_Reserved3 { get; set; }
        [StringLength(90)]
        public string PM_Reserved4 { get; set; }
        [StringLength(90)]
        public string PM_Reserved5 { get; set; }
        [StringLength(50)]
        public string PM_InsertUser { get; set; }
        public DateTime? PM_InsertDate { get; set; }
        [StringLength(50)]
        public string PM_UpdateUser { get; set; }
        public DateTime? PM_UpdateDate { get; set; }
        [StringLength(20)]
        public string BarcodeType { get; set; }
        public string BarcodeDataFormat { get; set; }
        [StringLength(200)]
        public string SerialNumberType { get; set; }
        [StringLength(20)]
        public string SnExpressionID { get; set; }
        [StringLength(20)]
        public string DesignID { get; set; }
        [StringLength(20)]
        public string Capacity { get; set; }
        [StringLength(20)]
        public string LIC { get; set; }
        [StringLength(20)]
        public string PCN { get; set; }
        [StringLength(20)]
        public string Condition { get; set; }
        [StringLength(90)]
        public string PD_Reserved1 { get; set; }
        [StringLength(90)]
        public string PD_Reserved2 { get; set; }
        [StringLength(90)]
        public string PD_Reserved3 { get; set; }
        [StringLength(90)]
        public string PD_Reserved4 { get; set; }
        [StringLength(90)]
        public string PD_Reserved5 { get; set; }
        [StringLength(50)]
        public string PD_InsertUser { get; set; }
        public DateTime? PD_InsertDate { get; set; }
        [StringLength(50)]
        public string PD_UpdateUser { get; set; }
        public DateTime? PD_UpdateDate { get; set; }
        public string IsPass { get; set; }
        public string IsReject { get; set; }
        public string IsSample { get; set; }
        public override string ToString()
        {
            return DominoFunctions.ClassFunctions.ClassToLocalizationString<Dmn_View_PMData>(this);
        }
        public Dmn_View_PMData()
        {

        }
        public Dmn_View_PMData(object obj)
            : this()
        {
            this.UnionClass(obj);
        }
    }
}
