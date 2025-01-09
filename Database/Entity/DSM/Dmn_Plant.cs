using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DominoFunctions.ExtensionMethod;


namespace DominoDatabase.DSM
{
    public class Dmn_Plant
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string PlantCode { get; set; }
        [StringLength(100)]
        public string PlantName { get; set; }
        [Required]
        [StringLength(1)]
        public string UseYN { get; set; }
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
            return DominoFunctions.ClassFunctions.ClassToLocalizationString<Dmn_Plant>(this);
        }
        public Dmn_Plant()
        {

        }
        public Dmn_Plant(object obj)
            : this()
        {
            this.UnionClass(obj);
        }
    }
}
