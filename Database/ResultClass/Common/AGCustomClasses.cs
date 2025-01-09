using DominoFunctions.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominoDatabase
{
    public class SpRb : SerialPool
    {
        public List<ReadBarcode> ReadBarcodes { get; set; }
        public string FullBarcode
        {
            get { return ReadBarcodes.FirstOrDefault()?.FullBarcode_Parent; }
        }
        public string AI_FullBarcode
        {
            get { return ReadBarcodes.FirstOrDefault()?.AI_FullBarcode_Parent; }
        }
        public SpRb()
        {
            ReadBarcodes = new List<ReadBarcode>();
        }
        public SpRb(object list)
            : this()
        {
            this.UnionClass(list);
        }
    }
}
