using System;
using System.Linq;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DominoDatabase
{
    public class Dmn_CommonCode_M
    {
        [Key]
        [StringLength(30)]
        public string CDCode { get; set; }

        [Required]
        [StringLength(200)]
        public string CDName { get; set; }

        [Required]
        [StringLength(1)]
        public string UseYN { get; set; }
        [Required]
        public DateTime? InsertDate { get; set; }

        [Required]
        [StringLength(50)]
        public string InsertUser { get; set; }

        public DateTime? UpdateDate { get; set; }

        [StringLength(50)]
        public string UpdateUser { get; set; }

        public override string ToString()
        {
            return DominoFunctions.ClassFunctions.ClassToLocalizationString<Dmn_CommonCode_M>(this);
        }
    }
}
