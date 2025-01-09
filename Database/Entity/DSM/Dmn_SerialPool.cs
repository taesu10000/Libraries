using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DominoFunctions.ExtensionMethod;

namespace DominoDatabase.DSM
{
    public class Dmn_SerialPool
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
        [Key]
        [Column(Order = 3)]
        [StringLength(3)]
        public string JobDetailType { get; set; }
        [Key]
        [Column(Order = 4)]
        public int Duplicate_Cnt { get; set; }
        [StringLength(20)]
        public string MachineID { get; set; }
        [StringLength(50)]
        public string OrderNo { get; set; }
        [StringLength(4)]
        public string SeqNo { get; set; }
        [Required]
        [StringLength(1)]
        public string ResourceType { get; set; }
        [StringLength(20)]
        public string BarcodeDataFormat { get; set; }
        [StringLength(20)]
        public string BarcodeType { get; set; }
        [Required]
        [StringLength(1)]
        public string SerialType { get; set; }
        public long? idx_Group { get; set; }
        public long? idx_Insert { get; set; }
        public DateTime? UseDate { get; set; }
        public DateTime? InspectedDate { get; set; }
        [Required]
        [StringLength(1)]
        public string UseYN { get; set; }
        [StringLength(2)]
        public string Status { get; set; }
        [StringLength(512)]
        public string FileName { get; set; }
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
        public DateTime? AssignDate { get; set; }
        public override string ToString()
        {
            return DominoFunctions.ClassFunctions.ClassToLocalizationString<Dmn_SerialPool>(this);
        }
        public Dmn_SerialPool()
        {

        }
        public Dmn_SerialPool(object obj)
            : this()
        {
            this.UnionClass(obj);
        }
    }
}
