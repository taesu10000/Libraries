using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DominoFunctions.ExtensionMethod;

namespace DominoDatabase.Local
{
    public class Dmn_Product_AG
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string ProdCode { get; set; }
        [Key]
        [Column(Order = 1)]
        [StringLength(3)]
        public string JobDetailType { get; set; }
        [Required]
        [StringLength(1)]
        public string ResourceType { get; set; }
        [StringLength(20)]
        public string BarcodeType { get; set; }
        [StringLength(20)]
        public string BarcodeDataFormat { get; set; }
        [StringLength(1)]
        public string SnType { get; set; }
        [StringLength(20)]
        public string SnExpressionID { get; set; }
        [StringLength(50)]
        public string DesignID { get; set; }
        [StringLength(50)]
        public string DesignID2 { get; set; }
        [StringLength(20)]
        public string Capacity { get; set; }
        public int? PackingCount { get; set; }
        public int? ContentCount { get; set; }
        [StringLength(20)]
        public string Prefix_SSCC { get; set; }
        [StringLength(50)]
        public string PrinterName { get; set; }
        [StringLength(50)]
        public string PrinterName2 { get; set; }

        public int? LabelPrintCount { get; set; }
        [StringLength(1)]
        public string GS1ExtensionCode { get; set; }
        [StringLength(30)]
        public string ProdStdCode { get; set; }
        [StringLength(30)]
        public string ProdStdCodeChild { get; set; }
        [StringLength(20)]
        public string Condition { get; set; }
        [StringLength(1)]
        public string MachineUseYN { get; set; }
        [StringLength(90)]
        public string Reserved1 { get; set; }
        [StringLength(90)]
        public string Reserved2 { get; set; }
        [StringLength(90)]
        public string Reserved3 { get; set; }
        [StringLength(90)]
        public string Reserved4 { get; set; }
        [StringLength(90)]
        public string Reserved5 { get; set; }
        [Required]
        [StringLength(50)]
        public string InsertUser { get; set; }
        [Required]
        public DateTime? InsertDate { get; set; }
        [StringLength(50)]
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        [StringLength(10)]
        public string Price { get; set; }
        public decimal? MinimumWeight { get; set; }
        public decimal? MaximumWeight { get; set; }
        public override string ToString()
        {
            return DominoFunctions.ClassFunctions.ClassToLocalizationString<Dmn_Product_AG>(this);
        }
        public Dmn_Product_AG()
        {

        }
        public Dmn_Product_AG(object obj)
            : this()
        {
            this.UnionClass(obj);
        }
    }
}
