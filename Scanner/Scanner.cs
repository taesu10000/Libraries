using HandScanner.HID;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HandScanner
{

    public class Scanner
    {
        object lockObject;
        bool _connected;
        public bool Connected
        {
            get
            {
                return _connected;
            }
            protected set
            {
                if (_connected != value)
                {
                    _connected = value;
                    OnConnectionChanged(new ScannerConnectionChangedArgs(_connected));
                }
            }
        }
        HIDScanner _hid;
        Zebra_OPOS.ZebraScanner _zebra;
        public Scanner(Form frm)
        {
            _hid = new HIDScanner(frm);
            _hid.Tail = "\r";
            _hid.ScannerRead += Hid_ScannerRead;
            _hid.StartScan += _hid_StartScan;
            _hid.ConnectionChanged += _hid_ConnectionChanged;
            Connected = _hid?.ScannerCount > 0;

            _zebra = new Zebra_OPOS.ZebraScanner();
            _zebra.ScannerRead += _zebra_ScannerRead;
            lockObject = new object();
        }

        private void _hid_ConnectionChanged(object sender, EventArgs e)
        {
            if (Monitor.TryEnter(lockObject))
            {
                try
                {
                    Connected = _hid?.ScannerCount > 0;
                }
                finally
                {
                    Monitor.Exit(lockObject);
                }
            }
        }

        private void _hid_StartScan(object sender, EventArgs e)
        {
            try
            {
                OnScanStart(e);
            }
            catch { }
        }

        private void _zebra_ScannerRead(object sender, ScannerReadEventArgs e)
        {
            OnScanBarcode(e);
        }

        private void Hid_ScannerRead(object sender, ScannerReadEventArgs e)
        {
            OnScanBarcode(e);
        }
        public event EventHandler ScanStart;
        protected virtual void OnScanStart(EventArgs e)
        {
            EventHandler handler;
            handler = ScanStart;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler<ScannerConnectionChangedArgs> ConnectionChanged;
        protected virtual void OnConnectionChanged(ScannerConnectionChangedArgs e)
        {
            EventHandler<ScannerConnectionChangedArgs> handler;
            handler = this.ConnectionChanged;
            if (handler != null)
                handler(this, e);
        }
        public void InitializeScanBarcodeEventHandler()
        {
            ScanBarcode = null;
        }
        public event EventHandler<ScannerReadEventArgs> ScanBarcode;

        protected virtual void OnScanBarcode(ScannerReadEventArgs e)
        {
            Task.Run(() => ScanBarcode?.Invoke(this, e));
        }
#if DEBUG
        public virtual void OnScanBarcodeDebug(object sender, ScannerReadEventArgs e)
        {
            Task.Run(() => ScanBarcode?.Invoke(this, e));
        }
#endif
        public virtual void StartScan()
        {
            if (!_hid.Scan)
                _hid.Scan = true;
            _zebra.Connect();
        }
        public virtual void StopScan()
        {
            _hid.Scan = false;
            _zebra.DisConnect();
        }
        public virtual void Dispose()
        {
        }
    }
}