using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DominoFunctions.Enums;
using DominoFunctions.ExtensionMethod;

namespace DominoDatabase.Local
{
    public class Dmn_Product_M
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string ProdCode { get; set; }
        [Required]
        [StringLength(30)]
        public string ProdStdCode { get; set; }
        [Required]
        [StringLength(200)]
        public string ProdName { get; set; }
        [StringLength(200)]
        public string ProdName2 { get; set; }
		public int? MedicineType { get; set; }
		public int? AGLevel { get; set; }
        [StringLength(200)]
        public string Remark { get; set; }
        public int? Exp_Day { get; set; }
        [Required]
        [StringLength(1)]
        public string UseYN { get; set; }
        public int? Delay_Print { get; set; }
        public int? Delay_Print2 { get; set; }
        public int? Delay_Shot1 { get; set; }
        public int? Delay_Shot2 { get; set; }
        public int? Delay_NG { get; set; }
        public int? Delay_NG2 { get; set; }
        public int? Delay_Shot3 { get; set; }
        public int? Delay_Shot4 { get; set; }
		/// <summary>
		/// <list type="bullet">
		/// <item>Usage 1: Used for China XML Report; PackageSpec, cascade, physicDetailType or such. (SCD)</item> 
		/// <item>Usage 2: --</item> 
		/// </list>
		/// </summary>
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
            return DominoFunctions.ClassFunctions.ClassToLocalizationString<Dmn_Product_M>(this);
        }
        public Dmn_Product_M()
        {

        }
        public Dmn_Product_M(object obj)
            : this()
        {
            this.UnionClass(obj);
        }
    }
}
