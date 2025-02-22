﻿using System;
using System.Net;
using log4net;
using Cognex.DataMan.SDK;
using Cognex.DataMan.SDK.Utils;
using System.Threading;
using System.Threading.Tasks;

namespace FixedMountScanner
{
    public class Dataman : FixedMountScannerBase
    {
        protected static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataManSystem _dataman = null;
        XmlResult _result;

        public XmlResult Result
        {
            get 
            {
                return _result; 
            }
            set
            {
                _result?.Dispose();
                _result = value;
            }
        }
        public override bool IsConnected
        {
            get
            {
                if (_dataman != null)
                    return (_dataman.Connector.State == ConnectionState.Connected);
                return false;
            }
        }
        public Dataman(string name, string iPAddress, int delay)
        {
            Name = name;
            IPAddress = iPAddress;
            Delay = delay;
            Initialize(IPAddress);
        }
        public override bool Connect()
        {
            if (_dataman != null)
            {
                if (_dataman.State == ConnectionState.Connected)
                    return true;
                _dataman.Connect(100);
                SetTriggerDelay();
                return true;
            }
            return false;
        }
        public override void Disconnect()
        {
            _dataman.Disconnect();
        }
        bool Initialize(string ip)
        {
            IPAddress datamanIp = System.Net.IPAddress.Parse(ip);
            EthSystemConnector con = new EthSystemConnector(datamanIp);
            _dataman = new DataManSystem(con);
            _dataman.SystemDisconnected += Dataman_SystemDisconnected;
            _dataman.SystemConnected += Dataman_SystemConnected;
#if !DEBUG
            _dataman.KeepAliveResponseMissed += Dataman_KeepAliveResponseMissed;
#endif
            return true;
        }
        #region Data
        private void Dataman_ImageGraphicsArrived(object sender, ImageGraphicsArrivedEventArgs e)
        {
            try
            {
                if (Result != null)
                {
                    log.DebugFormat("Dataman_ImageGraphicsArrived");
                    Result?.SetGraphics(e);
                    OnImageRecieved(this, new ImageReadStringArrivedEventArgs(Result));
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Dataman_ImageGraphicsArrived : {0}", ex.ToString());
            }
        }
        private void Dataman_XmlResultArrived(object sender, XmlResultArrivedEventArgs e)
        {
            try
            {
                log.DebugFormat("Dataman_XMLResultArrived");
                Result = new XmlResult(e.XmlResult);
                if (ScanMultipleBarcodes)
                    OnMultiDataRecieved(this, new MultiStringArrivedEventArgs(Result));
                else
                    OnDataRecieved(this, new StringArrivedEventArgs(Result));
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Dataman_XmlResultArrived : {0}", ex.ToString());
            }
        }
        private void Dataman_ImageArrived(object sender, ImageArrivedEventArgs e)
        {
            try
            {
                Result?.SetImage(e);
                e.Image.Dispose();
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Dataman_ImageArrived : {0}", ex.ToString());
            }
        }
        #endregion
        #region Connection
        private void Dataman_ConnectionChanged()
        {
            try
            {
                OnConnectionChanged(this, new ConnectionChangedArgs(IsConnected));
            }
            catch { }
        }
        void Dataman_SystemConnected(object sender, EventArgs args)
        {
            try
            {
                Dataman_ConnectionChanged();
                _dataman.ImageArrived += Dataman_ImageArrived;
                _dataman.ImageGraphicsArrived += Dataman_ImageGraphicsArrived;
                _dataman.XmlResultArrived += Dataman_XmlResultArrived;
                _dataman.SetResultTypes(ResultTypes.ReadString | ResultTypes.ReadXml | ResultTypes.Image | ResultTypes.ImageGraphics | ResultTypes.TrainingResults);
                _dataman.SetKeepAliveOptions(true, 5000, 5000);
            }
            catch { }
        }
        async void Dataman_SystemDisconnected(object sender, EventArgs args)
        {
            try
            {
                Dataman_ConnectionChanged();
                _dataman.ImageArrived -= Dataman_ImageArrived;
                _dataman.ImageGraphicsArrived -= Dataman_ImageGraphicsArrived;
                _dataman.XmlResultArrived -= Dataman_XmlResultArrived;
                while (IsConnected == false)
                {
                    try
                    {
                        if (_dataman != null)
                            _dataman.Connect(500);
                    }
                    catch { await Task.Delay(1000); }
                }
            }
            catch { }
        }
        void Dataman_KeepAliveResponseMissed(object sender, EventArgs args)
        {
            try
            {
                _dataman.Disconnect();
            }
            catch { }
        }
        #endregion
        public override void Dispose()
        {
            if (_dataman != null)
            {
                _dataman.SystemDisconnected -= Dataman_SystemDisconnected;
                _dataman.SystemConnected -= Dataman_SystemConnected;
                _dataman.KeepAliveResponseMissed -= Dataman_KeepAliveResponseMissed;
                _dataman.ImageArrived -= Dataman_ImageArrived;
                _dataman.ImageGraphicsArrived -= Dataman_ImageGraphicsArrived;
                _dataman.XmlResultArrived -= Dataman_XmlResultArrived;
                try
                {
                    _dataman.Dispose();
                    _dataman = null;
                }
                catch { }
            }
        }
        public override bool OneShot(int millisecondsTimeout = 2000)
        {
            if (IsConnected == false)
            {
                return false;
            }

            try
            {
                if (_dataman != null)
                {
                    _dataman.DefaultTimeout = millisecondsTimeout;
                    _dataman.SendCommand("TRIGGER ON");
                }
                return true;
            }
            catch { return false; }
        }
        public override bool SetTriggerType(int type)
        {
            try
            {
                var result = _dataman?.SendCommand(string.Format("SET TRIGGER.DELAY-TYPE {0}", type));
            }
            catch (Exception ex)
            {
                log.InfoFormat("{0} {1}", ex.Message, ex.StackTrace);
                return false;
            }
            return true;
        }
        public override bool SetTriggerDelay(int delay)
        {
            try
            {
                var result = _dataman?.SendCommand(string.Format("SET TRIGGER.DELAY-TIME {0}", delay));
            }
            catch (Exception ex)
            {
                log.InfoFormat("{0} {1}", ex.Message, ex.StackTrace);
                return false;
            }
            return true;
        }
        public override bool SetTriggerDelay()
        {
            try
            {
                var result = _dataman?.SendCommand(string.Format("SET TRIGGER.DELAY-TIME {0}", Delay));
            }
            catch (Exception ex)
            {
                log.InfoFormat("{0} {1}", ex.Message, ex.StackTrace);
                return false; 
            }
            return true;
        }
        public override int GetTriggerDelay()
        {
            try
            {
                var val = _dataman?.SendCommand(string.Format("GET TRIGGER.DELAY-TIME"))?.PayLoad;
                return Convert.ToInt32(val);
            }
            catch { return 0; }
        }        
        public override int GetTimeOut()
        {
            try
            {
                var val = _dataman?.SendCommand("GET DECODER.TIMEOUT").PayLoad;
                return Convert.ToInt32(val);
            }
            catch { return 0; }
        }
        public override void SetTimeOut(int timeout)
        {
            try
            {
                var val = _dataman?.SendCommand("SET DECODER.TIMEOUT {0}", timeout);
            }
            catch { }
        }
        public override void Tune(bool train = true)
        {
            try
            {
                Func<bool> isTunning = () =>
                {
                    return _dataman?.SendCommand(string.Format("GET TUNE.STATUS"))?.PayLoad == "ON";
                };

                if (isTunning())
                    TuneCancel();
                else
                    _dataman?.SendCommand(string.Format("TUNE.START"));
            }
            catch { }
        }
        public override void TuneCancel()
        {
            try
            {
                var result = _dataman?.SendCommand(string.Format("TUNE.CANCEL"));
            }
            catch { }
        }
        public override void SetReadCount(int count, MAXNUMCODESYMBOL symbol = MAXNUMCODESYMBOL.DataMatrix)
        {
            try
            {
                var result = _dataman?.SendCommand(string.Format("SET MULTICODE.NUM-CODES {0}", count));
                var result2 = _dataman?.SendCommand(string.Format("SET MULTICODE.MAX-NUM-CODES {0} {1}", (int)symbol, count));
            }
            catch { }
        }
    }
}
