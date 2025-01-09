using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominoDatabase
{
    public class DasanResult
    {
        public string gtin_cd { get; set; }
        public string valid_date_cd { get; set; }
        public string lot_no { get; set; }
        public string serial_no { get; set; }
        public string first_box_barcode { get; set; }
        public string second_box_barcode { get; set; }
        public string third_box_barcode { get; set; }
        public string fourth_box_barcode { get; set; }
        public string fifth_box_barcode { get; set; }
    }
}
