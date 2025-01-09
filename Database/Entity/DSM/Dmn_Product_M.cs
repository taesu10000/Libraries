using DominoFunctions.Enums;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DominoDatabase.DSM
{
	public class Dmn_Product_M
	{
		[Key]
		[Column(Order = 0)]
		[StringLength(20)]
		public string PlantCode { get; set; }
		[Key]
		[Column(Order = 1)]
		[StringLength(50)]
		public string ProdCode { get; set; }
		[StringLength(20)]
		public string LineID { get; set; }
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
        [StringLength(20)]
        public string CompanyPrefix { get; set; }
		[StringLength(20)]
		public string NDCType { get; set; }
		[StringLength(50)]
		public string NDCValue { get; set; }
		[StringLength(50)]
		public string InterfaceDetail { get; set; }
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
			try
			{
				return DominoFunctions.ClassFunctions.ClassToLocalizationString<Dmn_Product_M>(this);
			}
			catch
			{
				return string.Empty;
			}
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
