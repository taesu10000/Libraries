using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DominoFunctions.ExtensionMethod;


namespace DominoDatabase.Local
{
    public class Dmn_VisionResult
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
        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Idx_Insert { get; set; }
        [StringLength(1000)]
        public string DecodedBarcode { get; set; }
        [StringLength(512)]
        public string Read_OCR { get; set; }
        [StringLength(1)]
        public string Grade_Barcode { get; set; }
        [StringLength(512)]
        public string FilePath { get; set; }
        public int? CameraIndex { get; set; }
        [StringLength(2)]
        public string Status { get; set; }
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
        [Key]
        [Column(Order = 4)]
        public DateTime? InsertDate { get; set; }
        [StringLength(50)]
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        [StringLength(30)]
        public string Weight { get; set; }
        public override string ToString()
        {
            return DominoFunctions.ClassFunctions.ClassToLocalizationString<Dmn_VisionResult>(this);
        }
        public Dmn_VisionResult()
        {

        }
        public Dmn_VisionResult(object obj)
            : this()
        {
            this.UnionClass(obj);
        }
    }
}
