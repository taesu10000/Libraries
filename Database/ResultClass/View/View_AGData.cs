using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DominoFunctions.ExtensionMethod;

namespace DominoDatabase.Local
{
    public class View_AGData
    {
        public string StandardCode { get; set; }
        public string SerialNum { get; set; }
        public string OrderNo { get; set; }
        public string SeqNo { get; set; }
        public string JobDetailType { get; set; }
        public string FullBarcode_Read { get; set; }
        public string FullBarcode_Read_Parent { get; set; }
        public string FullBarcode_Read_GParent { get; set; }
        public string FullBarcode_Read_GGParent { get; set; }
        public string AI_FullBarcode_Read { get; set; }
        public string AI_FullBarcode_Read_Parent { get; set; }
        public string AI_FullBarcode_Read_GParent { get; set; }
        public string AI_FullBarcode_Read_GGParent { get; set; }
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
        public string RB_Reserved1 { get; set; }
        public string RB_Reserved2 { get; set; }
        public string RB_Reserved3 { get; set; }
        public string RB_Reserved4 { get; set; }
        public string RB_Reserved5 { get; set; }
        public string FilePath { get; set; }
        public string RB_InsertUser { get; set; }
        public DateTime? RB_InsertDate { get; set; }
        public string RB_UpdateUser { get; set; }
        public DateTime? RB_UpdateDate { get; set; }
        public string RB_IsPass { get; set; }
        public string RB_IsSample { get; set; }
        public string RB_IsReject { get; set; }
        public string ResourceType { get; set; }
        public string BarcodeDataFormat { get; set; }
        public string BarcodeType { get; set; }
        public string SerialType { get; set; }
        public long? idx_Group { get; set; }
        public long? idx_Insert { get; set; }
        public DateTime? UseDate { get; set; }
        public DateTime? InspectedDate { get; set; }
        public string SP_UseYN { get; set; }
        public string SP_Status { get; set; }
        public string FileName { get; set; }
        public string SP_Reserved1 { get; set; }
        public string SP_Reserved2 { get; set; }
        public string SP_Reserved3 { get; set; }
        public string SP_Reserved4 { get; set; }
        public string SP_Reserved5 { get; set; }
        public DateTime? SP_InsertDate { get; set; }
        public string SP_InsertUser { get; set; }
        public DateTime? SP_UpdateDate { get; set; }
        public string SP_UpdateUser { get; set; }
        public DateTime? SP_AssignDate { get; set; }
        public string SP_IsPass { get; set; }
        public string SP_IsSample { get; set; }
        public string SP_IsReject { get; set; }
        public string LineID { get; set; }
        public string CorCode { get; set; }
        public string PlantCode { get; set; }
        public string ErpOrderNo { get; set; }
        public DateTime? MfdDate { get; set; }
        public DateTime? ExpDate { get; set; }
        public string ProdCode { get; set; }
        public string LotNo { get; set; }
        public string LotNo_Sub { get; set; }
        public string ProdName { get; set; }
        public string ProdName2 { get; set; }
        public string SnType { get; set; }
        public string SnExpressionID { get; set; }
        public string DesignID { get; set; }
        public string Prefix_SSCC { get; set; }
        public string PrinterName { get; set; }
        public string GS1ExtensionCode { get; set; }

        public override string ToString()
        {
            return DominoFunctions.ClassFunctions.ClassToLocalizationString<View_AGData>(this);
        }
        public View_AGData()
        {

        }
        public View_AGData(object obj)
            : this()
        {
            this.UnionClass(obj);
        }
    }
}

