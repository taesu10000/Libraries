using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DominoFunctions.ExtensionMethod;

namespace DominoDatabase.Local
{
    public class Dmn_HelpCodePool_D
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(14)]
        public string ChildProdStdCode { get; set; }
        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string ChildSerialNum { get; set; }
        [Required]
        [StringLength(20)]
        public string HelpCode { get; set; }
        [Required]
        [StringLength(300)]
        public string FullChildBarcode_Read { get; set; }
        [Required]
        [StringLength(50)]
        public string OrderNo { get; set; }
        [Required]
        [StringLength(4)]
        public string SeqNo { get; set; }
        [StringLength(255)]
        public string History { get; set; }
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
            return DominoFunctions.ClassFunctions.ClassToLocalizationString<Dmn_HelpCodePool_D>(this);
        }
        public Dmn_HelpCodePool_D()
        {
        }
        public Dmn_HelpCodePool_D(object obj)
            : this()
        {
            this.UnionClass(obj);
        }
    }
}
