using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DominoFunctions.Enums;
using DominoFunctions.ExtensionMethod;

namespace DominoDatabase.Local
{
    public class Dmn_View_AGData
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string StandardCode { get; set; }
        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string SerialNum { get; set; }
        [Required]
        [StringLength(50)]
        public string OrderNo { get; set; }
        [Required]
        [StringLength(4)]
        public string SeqNo { get; set; }
        [Required]
        [StringLength(3)]
        public string JobDetailType { get; set; }
        public string FullBarcode_Read { get; set; }
        public string FullBarcode_Parent { get; set; }
        public string FullBarcode_GParent { get; set; }
        public string FullBarcode_GGParent { get; set; }
        public string AI_FullBarcode_Read { get; set; }
        public string AI_FullBarcode_Parent { get; set; }
        public string AI_FullBarcode_GParent { get; set; }
        public string AI_FullBarcode_GGParent { get; set; }
        public string StandardCode_Parent { get; set; }
        public string StandardCode_GParent { get; set; }
        public string StandardCode_GGParent { get; set; }
        public string SerialNumer_Parent { get; set; }
        public string SerialNumer_GParent { get; set; }
        public string SerialNumer_GGParent { get; set; }
        public string JobDetailType_Parent { get; set; }
        public string JobDetailType_GParent { get; set; }
        public string JobDetailType_GGParent { get; set; }
        public string RB_Status { get; set; }
        public string FilePath { get; set; }
        public string RB_InsertUser { get; set; }
        public DateTime? RB_InsertDate { get; set; }
        public string RB_UpdateUser { get; set; }
        public DateTime? RB_UpdateDate { get; set; }
        [NotMapped]
        public bool RB_IsPass { get { return RB_Status == EnProductStatus.Pass.ToString(); } }
        [NotMapped]
        public bool RB_IsSample
        {
            get
            {
                return RB_Status == EnProductStatus.Sample_Normal.ToString()
                    || RB_Status == EnProductStatus.Sample_QA.ToString()
                    || RB_Status == EnProductStatus.Sample_QC.ToString()
                    || RB_Status == EnProductStatus.Sample_Storage.ToString()
                    || RB_Status == EnProductStatus.Sample_Test.ToString();
            }
        }
        [NotMapped]
        public bool RB_IsReject { get { return RB_Status == EnProductStatus.Reject.ToString(); } }
        public string ResourceType { get; set; }
        public string BarcodeDataFormat { get; set; }
        public string BarcodeType { get; set; }
        public string SerialType { get; set; }
        public long? idx_Group { get; set; }
        public long? idx_Insert { get; set; }
        public DateTime? UseDate { get; set; }
        public DateTime? InspectedDate { get; set; }
        public string SP_UseYN { get; set; }
        public bool? fSP_UseYN 
        { 
            get 
            {
                if (SP_UseYN == null)
                    return null;
                else
                    return SP_UseYN == "Y"; 
            } 
        }
        public string SP_Status { get; set; }
        public string FileName { get; set; }
        public string ConfirmedYN { get; set; }
        [NotMapped]
        public bool fConfirmedYN { get { return ConfirmedYN == "Y"; } }
        public string SP_InsertUser { get; set; }
        public DateTime? SP_InsertDate { get; set; }
        public string SP_UpdateUser { get; set; }
        public DateTime? SP_UpdateDate { get; set; }
        public DateTime? AssignDate { get; set; }
        [NotMapped]
        public bool SP_IsPass { get { return SP_Status == EnProductStatus.Pass.ToString(); } }
        [NotMapped]
        public bool SP_IsSample
        {
            get
            {
                return SP_Status == EnProductStatus.Sample_Normal.ToString()
                    || SP_Status == EnProductStatus.Sample_QA.ToString()
                    || SP_Status == EnProductStatus.Sample_QC.ToString()
                    || SP_Status == EnProductStatus.Sample_Storage.ToString()
                    || SP_Status == EnProductStatus.Sample_Test.ToString();
            }
        }
        [NotMapped]
        public bool SP_IsReject { get { return SP_Status == EnProductStatus.Reject.ToString(); } }
        [NotMapped]
        public string Status
        {
            get
            {
                if (string.IsNullOrEmpty(SP_Status))
                    return RB_Status;
                return SP_Status;
            }
        }
        public string LineID { get; set; }
        public string CorCode { get; set; }
        public string PlantCode { get; set; }
        public string ErpOrderNo { get; set; }
        public DateTime? MfdDate { get; set; }
        public DateTime? ExpDate { get; set; }
        public DateTime? JM_AssignDate { get; set; }
        public string ProdCode { get; set; }
        public string LotNo { get; set; }
        public string LotNo_Sub { get; set; }
        public string ProdName { get; set; }
        public string ProdName2 { get; set; }
        public string PD_SnType { get; set; }
        public string SnExpressionID { get; set; }
        public string DesignID { get; set; }
        public string Prefix_SSCC { get; set; }
        public string PrinterName { get; set; }
        public string GS1ExtensionCode { get; set; }

        public override string ToString()
        {
            return DominoFunctions.ClassFunctions.ClassToLocalizationString<Dmn_View_AGData>(this);
        }
        public Dmn_View_AGData()
        {

        }
        public Dmn_View_AGData(object obj)
            : this()
        {
            this.UnionClass(obj);
        }
    }
}

