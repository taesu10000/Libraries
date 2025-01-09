using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DominoFunctions.ExtensionMethod;


namespace DominoDatabase.Local
{
    public class Dmn_CustomBarcodeFormat
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string CustomBarcodeFormatID { get; set; }
        [StringLength(300)]
        public string CustomBarcodeFormatStr { get; set; }
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
        public DateTime InsertDate { get; set; }
        [StringLength(50)]
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public override string ToString()
        {
            return DominoFunctions.ClassFunctions.ClassToLocalizationString<Dmn_CustomBarcodeFormat>(this);
        }
        public Dmn_CustomBarcodeFormat()
        {

        }
        public Dmn_CustomBarcodeFormat(object obj)
            : this()
        {
            this.UnionClass(obj);
        }
    }
}
