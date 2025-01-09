using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DominoFunctions.ExtensionMethod;

namespace DominoDatabase.Local
{
    public class Dmn_JobOrder_M
    {
        
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string OrderNo { get; set; }
        [Key]
        [Column(Order = 1)]
        [StringLength(4)]
        public string SeqNo { get; set; }
        [StringLength(20)]
        public string LineID { get; set; }

        [StringLength(20)]
        public string CorCode { get; set; }
        [StringLength(20)]
        public string PlantCode { get; set; }
        [StringLength(50)]
        public string ErpOrderNo { get; set; }
        [Required]
        [StringLength(2)]
        public string OrderType { get; set; }
        public DateTime? MfdDate { get; set; }
        public DateTime? ExpDate { get; set; }
        [Required]
        [StringLength(50)]
        public string ProdCode { get; set; }
        [Required]
        [StringLength(20)]
        public string LotNo { get; set; }
        [StringLength(20)]
        public string LotNo_Sub { get; set; }
        [Required]
        [StringLength(1)]
        public string UseYN { get; set; }
        [Required]
        public int Cnt_JobPlan { get; set; }
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
        [StringLength(50)]
        public string AssignUser { get; set; }
        public DateTime? AssignDate { get; set; }
        [Required]
        [StringLength(50)]
        public string InsertUser { get; set; }
        [Required]
        public DateTime? InsertDate { get; set; }
        [StringLength(50)]
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DateOfTest { get; set; }
        public override string ToString()
        {
            return DominoFunctions.ClassFunctions.ClassToLocalizationString<Dmn_JobOrder_M>(this);
        }
        public Dmn_JobOrder_M()
        {

        }
        public Dmn_JobOrder_M(object obj)
            : this()
        {
            this.UnionClass(obj);
        }
    }

}
