using Cognex.DataMan.SDK;
using Cognex.DataMan.SDK.Utils;
using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;

namespace FixedMountScanner
{
    public abstract class FixedMountScannerBase : IDisposable
    {
        public event EventHandler<StringArrivedEventArgs> DataReceived;
        public event EventHandler<MultiStringArrivedEventArgs> MultiDataReceived;
        public event EventHandler<ImageReadStringArrivedEventArgs> ImageReceived;
        public event EventHandler<ConnectionChangedArgs> ConnectionChanged;
        public string Name { get; set; }
        public string IPAddress { get; set; }
        public int Delay { get; set; }
        public bool ScanMultipleBarcodes { get; set; } = false;
        public abstract bool IsConnected { get; }
        public abstract bool Connect();
        public abstract void Disconnect();
        public abstract void Dispose();
        public virtual bool OneShot(int millisecondsTimeout = 2000) { return false; }
        public virtual bool SetTriggerType(int type) { return false; }
        public virtual bool SetTriggerDelay(int delay) { return false; }
        public virtual bool SetTriggerDelay() { return false; }
        public virtual int GetTriggerDelay() { return 0; }
        public virtual int GetTimeOut() { return 0; }
        public virtual void SetTimeOut(int timeout) { }
        public virtual void Tune(bool train = true) { }
        public virtual void TuneCancel() { }
        public void InitializeEvent()
        {
            DataReceived = null;
            MultiDataReceived = null;
            ImageReceived = null;
        }

        public virtual void SetReadCount(int count, MAXNUMCODESYMBOL symbol = MAXNUMCODESYMBOL.DataMatrix) { }
        public void OnImageRecieved(object sender, ImageReadStringArrivedEventArgs e)
        {
            Task.Run(() => ImageReceived?.Invoke(sender, e));
        }
        public void OnMultiDataRecieved(object sender, MultiStringArrivedEventArgs e)
        {
            Task.Run(() => MultiDataReceived?.Invoke(sender, e));
        }
        public void OnDataRecieved(object sender, StringArrivedEventArgs e)
        {
            Task.Run(() => DataReceived?.Invoke(sender, e));
        }
        public void OnConnectionChanged(object sender, ConnectionChangedArgs e)
        {
            ConnectionChanged?.Invoke(sender, e);
        }

    }
}