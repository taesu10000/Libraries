using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DominoFunctions.ExtensionMethod;

namespace DominoDatabase.Local
{
    public class Dmn_JobOrder_AG
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string OrderNo { get; set; }
        [Key]
        [Column(Order = 1)]
        [StringLength(4)]
        public string SeqNo { get; set; }
        [Key]
        [Column(Order = 2)]
        [StringLength(3)]
        public string JobDetailType { get; set; }
        [Required]
        [StringLength(2)]
        public string JobStatus { get; set; }
        public int? Cnt_Good { get; set; }
        public int? Cnt_Error { get; set; }
        public int? Cnt_Sample { get; set; }
        public int? Cnt_Destroy { get; set; }
        public int? Cnt_Child { get; set; }
        public int? Cnt_Work { get; set; }
        public int? Cnt_Parent { get; set; }
        public int? Cnt_SNLast { get; set; }
        public int? Cnt_SNPrintLast { get; set; }
        public int? Cnt_SN_Movil { get; set; }
        public int? Cnt_SN_DSM { get; set; }
        public int? Cnt_SN_Lot_O { get; set; }
        public int? Cnt_SN_Lot_X { get; set; }
        public int? Cnt_Status1 { get; set; }
        public int? Cnt_Status2 { get; set; }
        public int? Cnt_Status3 { get; set; }
        public int? Cnt_Status4 { get; set; }
        public int? Cnt_Status5 { get; set; }
        [StringLength(50)]
        public string UserDefineData1 { get; set; }
        [StringLength(50)]
        public string UserDefineData2 { get; set; }
        [StringLength(100)]
        public string PrinterVariable1 { get; set; }
        [StringLength(100)]
        public string PrinterVariable2 { get; set; }
        [StringLength(100)]
        public string PrinterVariable3 { get; set; }
        [StringLength(100)]
        public string PrinterVariable4 { get; set; }
        [StringLength(100)]
        public string PrinterVariable5 { get; set; }
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
        public string StartUser { get; set; }
        public DateTime? StartDate { get; set; }
        [StringLength(50)]
        public string CompleteUser { get; set; }
        public DateTime? CompleteDate { get; set; }
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
            return DominoFunctions.ClassFunctions.ClassToLocalizationString<Dmn_JobOrder_AG>(this);
        }
        public Dmn_JobOrder_AG()
        {

        }
        public Dmn_JobOrder_AG(object obj)
            : this()
        {
            this.UnionClass(obj);
        }
    }

}
