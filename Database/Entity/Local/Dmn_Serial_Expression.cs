using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DominoFunctions.ExtensionMethod;

namespace DominoDatabase.Local
{
    public class Dmn_Serial_Expression
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string SnExpressionID { get; set; }
        [Required]
        [StringLength(300)]
        public string SnExpressionStr { get; set; }
        [Required]
        [StringLength(1)]
        public string UseYN { get; set; }
        public int? SeqNum { get; set; }
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
            return DominoFunctions.ClassFunctions.ClassToLocalizationString<Dmn_Serial_Expression>(this);
        }
        public Dmn_Serial_Expression()
        {

        }
        public Dmn_Serial_Expression(object obj)
            : this()
        {
            this.UnionClass(obj);
        }
    }
}
