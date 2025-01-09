using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominoDatabase
{
    public class DominoJobOrder
    {
        [Required]
        public string ORDER_NO {  get; set; }
        public string LINE_ID { get; set; }
        [Required]
        public string PRODUCT_CODE { get; set; }
        [Required]
        public string LOTNO { get; set; }
        [Required]
        public string MFDDATE { get; set; }
        [Required]
        public string EXPDATE { get; set; }
        [Required]
        public string QTY { get; set; }
    }
}
