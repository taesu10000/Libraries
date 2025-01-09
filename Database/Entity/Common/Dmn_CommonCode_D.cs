using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DominoDatabase
{
    public class Dmn_CommonCode_D
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string CDCode { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string CDCode_Dtl { get; set; }

        [Required]
        [StringLength(200)]
        public string CDCode_Name { get; set; }

        [Required]
        [StringLength(1)]
        public string UseYN { get; set; }

        public int? SeqNum { get; set; }

        [StringLength(200)]
        public string Code_Value1 { get; set; }

        [StringLength(200)]
        public string Code_Value2 { get; set; }

        [StringLength(200)]
        public string Code_Value3 { get; set; }

        public DateTime InsertDate { get; set; }

        [Required]
        [StringLength(50)]
        public string InsertUser { get; set; }

        public DateTime? UpdateDate { get; set; }

        [StringLength(50)]
        public string UpdateUser { get; set; }

        public override string ToString()
        {
            return DominoFunctions.ClassFunctions.ClassToLocalizationString<Dmn_CommonCode_D>(this);
        }
    }
}
