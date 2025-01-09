using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DominoFunctions.ExtensionMethod;


namespace DominoDatabase.DSM
{
    public class Dmn_User_M
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string PlantCode { get; set; }
        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string UserID { get; set; }
        [Required]
        [StringLength(100)]
        public string UserName { get; set; }
        [Required]
        [StringLength(200)]
        public string UserPW { get; set; }
        public DateTime? LastLogIn { get; set; }
        public int? FailLogInCount { get; set; }
        [Required]
        [StringLength(1)]
        public string LockYN { get; set; }
        [Required]
        [StringLength(1)]
        public string UseYN { get; set; }
        [StringLength(200)]
        public string PrePW1 { get; set; }
        [StringLength(200)]
        public string PrePW2 { get; set; }
        [StringLength(200)]
        public string PrePW3 { get; set; }
        public DateTime? LastPWUpdate { get; set; }
        [StringLength(20)]
        public string DeptCode { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(20)]
        public string PhoneNum { get; set; }
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
            return DominoFunctions.ClassFunctions.ClassToLocalizationString<Dmn_User_M>(this);
        }
        public Dmn_User_M()
        {

        }
        public Dmn_User_M(object obj)
            : this()
        {
            this.UnionClass(obj);
        }
    }
}
