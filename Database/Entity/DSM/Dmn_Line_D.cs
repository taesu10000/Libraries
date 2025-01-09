using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DominoFunctions.ExtensionMethod;

namespace DominoDatabase.DSM
{
    public class Dmn_Line_D
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string PlantCode { get; set; }
        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string LineID { get; set; }
        [Key]
        [Column(Order = 2)]
        [StringLength(3)]
        public string JobDetailType { get; set; }
        [StringLength(20)]
        public string MachineID { get; set; }
        public int? SeqNo { get; set; }
        [StringLength(50)]
        public string LastOrderNo { get; set; }
        [StringLength(4)]
        public string LastSeqNo { get; set; }
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
        public override string ToString()
        {
            return DominoFunctions.ClassFunctions.ClassToLocalizationString<Dmn_Line_D>(this);
        }
        public Dmn_Line_D()
        {

        }
        public Dmn_Line_D(object obj)
            : this()
        {
            this.UnionClass(obj);
        }
    }
}
