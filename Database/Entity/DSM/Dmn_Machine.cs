using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DominoFunctions.ExtensionMethod;

namespace DominoDatabase.DSM
{
    public class Dmn_Machine
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string PlantCode { get; set; }
        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string MachineID { get; set; }
        [StringLength(5)]
        public string MachineStatus { get; set; }
        [Required]
        [StringLength(3)]
        public string MachineType { get; set; }
        [StringLength(100)]
        public string MachineName { get; set; }
        [StringLength(50)]
        public string IPAddress { get; set; }
        [Required]
        [StringLength(1)]
        public string AGAdditionalYN { get; set; }
        public int? PrintVariable { get; set; }
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
            return DominoFunctions.ClassFunctions.ClassToLocalizationString<Dmn_Machine>(this);
        }
        public Dmn_Machine()
        {

        }
        public Dmn_Machine(object obj)
            : this()
        {
            this.UnionClass(obj);
        }
    }
}

