using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixedMountScanner
{
    public enum MAXNUMCODESYMBOL
    {
        DataMatrix = 1,
        QRCode = 2,
        MaxiCode = 2,
        AztecCode = 2,
        Linear = 3,
        Postal = 3,
        Stacked = 3,
        VeriCode = 4,
        DotCode = 5,
    }
    public enum EnDisplyParams : long
    {
        None = 0,
        ReadBarcodes = 1,
        Name = ReadBarcodes << 1,
    }
}
