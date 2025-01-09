using System;

namespace HandScanner
{
    public class ScannerConnectionChangedArgs : EventArgs
    {
        bool _connected;
        public ScannerConnectionChangedArgs(bool connected)
        {
            _connected = connected;
        }
        public bool Connected
        {
            get { return _connected; }
            protected set { _connected = value; }
        }
    }
}