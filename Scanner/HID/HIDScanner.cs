using System;
using System.Management;
using System.Windows.Forms;


namespace HandScanner.HID
{
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public class HIDScanner : NativeWindow
    {
        Barcode _barcode;
        private ManagementEventWatcher watcherAttach;
        private ManagementEventWatcher watcherRemove;
        public event EventHandler ConnectionChanged;
        protected void OnConnectionChanged(EventArgs e)
        {
            ConnectionChanged?.Invoke(this, e);
        }
        public event EventHandler StartScan;
        protected void OnStartScan(EventArgs e)
        {
            EventHandler handler;
            handler = StartScan;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler<ScannerReadEventArgs> ScannerRead;
        protected void OnScannerRead(ScannerReadEventArgs e)
        {
            EventHandler<ScannerReadEventArgs> handler;
            handler = ScannerRead;
            if (handler != null)
                handler(this, e);
        }

        System.Timers.Timer _tm;
        private const int WM_ACTIVATEAPP = 0x001C;
        private Form parent;
        public const string ZebraScanner = "VID_05E0";
        public const string HoneywellScanner = "VID_0C2E";
        public const string DataLogicScanner = "VID_05F9";
        InputDevice _id;
        int NumberOfKeyboards;
        public int ScannerCount
        {
            get
            {
                return _id.DeviceList.Count;
            }
        }
        public string Tail { get; set; }
        public bool Scan { get; set; }
        public HIDScanner(Form frm)
        {
            frm.HandleCreated += Frm_HandleCreated;
            frm.HandleDestroyed += Frm_HandleDestroyed;
            this.parent = frm;
            _id = new InputDevice(parent.Handle);
            _id.HardWareID.Add(ZebraScanner);
            _id.HardWareID.Add(HoneywellScanner);
            _id.HardWareID.Add(DataLogicScanner);
            _id.KeyPressed += Id_KeyPressed;

            NumberOfKeyboards = _id.EnumerateDevices();
            _tm = new System.Timers.Timer();
            _tm.Interval = 200;
            _tm.Elapsed += _tm_Elapsed;
            _tm.AutoReset = false;
            InitializeWatcher();
        }
        private void InitializeWatcher()
        {
            watcherAttach = new ManagementEventWatcher();
            watcherRemove = new ManagementEventWatcher();
            watcherAttach.EventArrived += new EventArrivedEventHandler(watcher_EventArrived);
            watcherRemove.EventArrived += new EventArrivedEventHandler(watcher_EventArrived);
            watcherAttach.Query = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity'");
            watcherRemove.Query = new WqlEventQuery("SELECT * FROM __InstanceDeletionEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity'");
            watcherAttach.Start();
            watcherRemove.Start();
        }
        private void watcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            _id.EnumerateDevices();
            OnConnectionChanged(EventArgs.Empty);
        }
        public void Dispose()
        {
            watcherAttach.Stop();
            watcherRemove.Stop();
            watcherAttach.Dispose();
            watcherRemove.Dispose();
        }
        private void _tm_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                OnScannerRead(new ScannerReadEventArgs(_barcode));
                _barcode = null;
            }
            catch { }
        }

        internal void Frm_HandleDestroyed(object sender, EventArgs e)
        {
            ReleaseHandle();
        }

        private void Frm_HandleCreated(object sender, EventArgs e)
        {
            AssignHandle(((Form)sender).Handle);
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            try
            {
                if (_id != null && Scan)
                {
                    if (_id.ProcessMessage(m))
                        return;
                }
                base.WndProc(ref m);
            }
            catch { }
        }
        private void Id_KeyPressed(object sender, InputDevice.KeyControlEventArgs e)
        {
            try
            {
                if (_barcode == null)
                {
                    OnStartScan(EventArgs.Empty);
                    _barcode = new Barcode();
                }
                if (e.Keyboard.deviceName == HoneywellScanner)
                    _barcode.Scanner = EnScanner.HoneyWell_HID;
                else if (e.Keyboard.deviceName == ZebraScanner)
                    _barcode.Scanner = EnScanner.Zebra_HID;
                else
                    _barcode.Scanner = EnScanner.NotDefined;
                _barcode.ReadData += e.Keyboard.Ascii;
                if (string.IsNullOrEmpty(Tail))
                {
                    _tm.Stop();
                    _tm.Start();
                }
                else if (e.Keyboard.Ascii.Equals(Tail))
                {
                    OnScannerRead(new ScannerReadEventArgs(_barcode));
                    _barcode = null;
                }
            }
            catch (Exception ex)
            {
                _barcode = null;
                string msg = ex.Message;
            }
        }
    }
}