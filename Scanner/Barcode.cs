using System;


namespace HandScanner
{
    public class Barcode
    {
        //public EnScannerSymbology SymBology
        //{
        //    get
        //    {
        //        return GetSymbology(Int_Symbology);
        //    }
        //    set
        //    {
        //        Int_Symbology = SetSymbology(value);
        //    }
        //}
        public int Int_Symbology { get; set; }
        public string ReadData { get; set; }

        public string ReadDataWithoutPrefix
        {
            get
            {
                if (ReadData.Substring(0, 1) == "]")
                    return ReadData.Substring(3, ReadData.Length - 3).Replace("\r", "").Replace("\n", "");
                else return ReadData.Replace("\r", "").Replace("\n", "");
            }
        }

        public EnScanner Scanner { get; set; }

        //private EnScannerSymbology GetSymbology(int value)
        //{
        //    switch (value)
        //    {

        //    }
        //}
        //private int SetSymbology(EnScannerSymbology value)
        //{

        //}
    }
    public class ScannerReadEventArgs : EventArgs
    {
        Barcode _barcode;
        public ScannerReadEventArgs(Barcode barcode)
        {
            _barcode = barcode;
        }
        public ScannerReadEventArgs(string ReadData)
        {
            _barcode = new Barcode();
            _barcode.Scanner = EnScanner.NotDefined;
            _barcode.ReadData = ReadData;
        }

        public Barcode Barcode
        {
            get { return _barcode; }
            protected set { _barcode = value; }
        }

    }
}