using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DominoFunctions.ExtensionMethod;

namespace DominoDatabase.DSM
{
    public class Dmn_ReadBarcode
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string PlantCode { get; set; }
        [Key]
        [Column(Order = 1)]
        [StringLength(30)]
        public string ProdStdCode { get; set; }
        [Key]
        [Column(Order = 2)]
        [StringLength(20)]
        public string SerialNum { get; set; }
        [StringLength(20)]
        public string MachineID { get; set; }
        [Required]
        [StringLength(50)]
        public string OrderNo { get; set; }
        [Required]
        [StringLength(4)]
        public string SeqNo { get; set; }
        [Required]
        [StringLength(3)]
        public string JobDetailType { get; set; }
        [StringLength(300)]
        public string FullBarcode_Read { get; set; }
        [StringLength(300)]
        public string AI_FullBarcode_Read { get; set; }
        [StringLength(300)]
        public string FullBarcode_Parent { get; set; }
        [StringLength(300)]
        public string AI_FullBarcode_Parent { get; set; }
        [StringLength(30)]
        public string ParentProdStdCode { get; set; }
        [StringLength(20)]
        public string ParentSerialNum { get; set; }
        [Required]
        [StringLength(2)]
        public string Status { get; set; }
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
        [StringLength(512)]
        public string FilePath { get; set; }
        [Required]
        [StringLength(50)]
        public string InsertUser { get; set; }
        [Required]
        public DateTime? InsertDate { get; set; }
        [StringLength(50)]
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public override string ToString()
        {
            return DominoFunctions.ClassFunctions.ClassToLocalizationString<Dmn_ReadBarcode>(this);
        }
        public Dmn_ReadBarcode()
        {

        }
        public Dmn_ReadBarcode(object obj)
            : this()
        {
            this.UnionClass(obj);
        }
    }
}
