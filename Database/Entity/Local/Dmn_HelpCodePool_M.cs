using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DominoDatabase.Local
{
    public class Dmn_HelpCodePool_M
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
        [StringLength(20)]
        public string HelpCode { get; set; }
        [StringLength(14)]
        public string ProdStdCode { get; set; }
        [StringLength(90)]
        public string SerialNum { get; set; }
        [Required]
        [StringLength(1)]
        public string UseYN { get; set; }
        [Required]
        [StringLength(2)]
        public string Status { get; set; }
        public long? idx_Insert { get; set; }
        [StringLength(512)]
        public string FilePath { get; set; }
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
            return DominoFunctions.ClassFunctions.ClassToLocalizationString<Dmn_HelpCodePool_M>(this);
        }
        public Dmn_HelpCodePool_M()
        {

        }
        public Dmn_HelpCodePool_M(object obj)
            : this()
        {
            this.UnionClass(obj);
        }
    }
}
