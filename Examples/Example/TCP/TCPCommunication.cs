using log4net;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Functions.TCP
{
    public class StateObject
    {
        public Socket workSoket = null;
        public const int BufferSize = 4096;
        public byte[] buffer = new byte[BufferSize];
    }
    public class TCPCommunication : ICoummunication
    {
        private readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int _tcpPort;
        private IPAddress _ipAddress;
        private bool _isServer;
        private System.Timers.Timer _timeoutTimer;
        private System.Timers.Timer _trackingTimer;
        private string _lastData;
        private EnCommuincationStatus _connectionstatus;
        private string _tail;
        private string _header;
        private bool _autoReconnection;
        private int _autoReconnectionInterval;
        private Socket _socket;
        private string _data;
        private object _sendAndResponseLock = new object();
        private object _sendLock = new object();

        private object _sendAndResponseLockModbus = new object();
        public enum TypeProtocol { Socket = 0, ModbusServer = 1, ModbusClient = 2, Mectec_MxCommand = 3, };
        TypeProtocol _typeProc;
        public delegate void ModbusDataReceivedEventArgs(object modbusParam);
        public event ModbusDataReceivedEventArgs ModbusDataReceived;

        public TCPCommunication(IPAddress ModbusIP, int ModbusPort, TypeProtocol pProtocol)
            : this()
        {
            Protocol_Type = pProtocol;
            _ipAddress = ModbusIP;
            _tcpPort = ModbusPort;
            if (pProtocol == TypeProtocol.ModbusServer)
            {
                _isServer = true;
            }
            else if (pProtocol == TypeProtocol.ModbusClient)
            {
                _isServer = false;
            }
            _connectionstatus = EnCommuincationStatus.NeverConnected;
        }
        public TCPCommunication(IPAddress ip, int port, bool server = false, TypeProtocol pProtocol = TypeProtocol.Socket)
            : this()
        {
            _ipAddress = ip;
            _tcpPort = port;
            _isServer = server;
            Protocol_Type = pProtocol;
            _connectionstatus = EnCommuincationStatus.NeverConnected;
        }
        public TCPCommunication(string ip, int port, bool server = false, TypeProtocol pProtocol = TypeProtocol.Socket)
            : this()
        {
            _ipAddress = IPAddress.Parse(ip);
            _tcpPort = port;
            _isServer = server;
            Protocol_Type = pProtocol;
            _connectionstatus = EnCommuincationStatus.NeverConnected;
        }
        protected TCPCommunication()
        {
            _autoReconnection = true;
            _autoReconnectionInterval = 2000;
            _timeoutTimer = new System.Timers.Timer();
            _timeoutTimer.Interval = 5000;
            _timeoutTimer.Elapsed += _timeoutTimer_Elapsed;
            _trackingTimer = new System.Timers.Timer();
            _trackingTimer.Interval = 200;
            _trackingTimer.Elapsed += _trackingTimer_Elapsed;
        }

        private void _trackingTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_data))
            {
                _trackingTimer.Stop();
                OnDataRecieved(new DataRecievedEventArgs(_data));
                _lastData = _data;
                _data = string.Empty;
            }
        }
        private void _timeoutTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _timeoutTimer.Stop();
            if (_socket != null && Protocol_Type == TypeProtocol.ModbusServer) DisConnect();
            Connect();
        }

        public void Connect()
        {
            if (_connectionstatus == EnCommuincationStatus.Connected)
                return;
            try
            {
                ConnectionStatus = EnCommuincationStatus.Connecting;
                IPEndPoint endPoint = new IPEndPoint(_ipAddress, _tcpPort);
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
                if (_isServer)
                {
                    if (Protocol_Type == TypeProtocol.ModbusServer) _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                    _socket.Bind(endPoint);
                    _socket.Listen(100);
                    _socket.BeginAccept(new AsyncCallback(AcceptCallback), _socket);
                }
                else
                {
                    _socket.BeginConnect(endPoint, new AsyncCallback(ConnectCallback), _socket);
                }
            }
            catch
            {
                ConnectionStatus = EnCommuincationStatus.Error;
            }
        }

        #region callBack
        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                Socket _client = (Socket)ar.AsyncState;
                if (!_client.Connected && Protocol_Type != TypeProtocol.ModbusServer)
                {
                    System.Threading.Thread.Sleep(_autoReconnectionInterval);
                    return;
                }
                Socket handler = _client.EndAccept(ar);
                ConnectionStatus = EnCommuincationStatus.Connected;
                StateObject state = new StateObject();
                state.workSoket = handler;
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(RecieveCallback), state);
            }
            catch (Exception ex)
            {
                log.DebugFormat("Exception On AcceptCallback", ex);
                ConnectionStatus = EnCommuincationStatus.Error;
            }
        }
        private void RecieveCallback(IAsyncResult ar)
        {
            try
            {
                StateObject state = (StateObject)ar.AsyncState;
                SocketError err = new SocketError();
                Socket client = state.workSoket;
                int byteRead = client.EndReceive(ar, out err);
                if (err != SocketError.Success)
                {
                    ConnectionStatus = EnCommuincationStatus.Error;
                    _timeoutTimer.Start();
                    return;
                }
                else if (byteRead == 0 && err == SocketError.Success)
                {
                    ConnectionStatus = EnCommuincationStatus.DisconnectedByHostOrClient;
                    _timeoutTimer.Start();
                    return;
                }
                else
                {
                    if (Protocol_Type == TypeProtocol.ModbusServer)
                    {
                        NetworkConnectionParameter modbusParam = new NetworkConnectionParameter();
                        NetworkStream networkStream = new NetworkStream(client);
                        byte[] data = new byte[byteRead];

                        Buffer.BlockCopy(state.buffer, 0, data, 0, byteRead);
                        modbusParam.bytes = data;
                        modbusParam.stream = networkStream;

                        if (ModbusDataReceived != null)
                            ModbusDataReceived(modbusParam);
                    }
                    else
                    {
                        _timeoutTimer.Stop();
                        _data += Encoding.UTF8.GetString(state.buffer, 0, byteRead);
                        if (Protocol_Type == TypeProtocol.Mectec_MxCommand)
                            _data += _tail;
                        if (string.IsNullOrEmpty(_tail))
                        {
                            _trackingTimer.Stop();
                            _trackingTimer.Start();
                        }
                        else
                        {
                            //while (_data.Contains(_tail))
                            //{
                            //    string tmp = _data.Split(new string[] { _tail }, StringSplitOptions.None)[0];
                            //    _data = _data.Remove(0, tmp.Length  + _tail.Length);
                            //    _lastData = tmp;
                            //    OnDataRecieved(new DataRecievedEventArgs(tmp));
                            //}
                            if (_data.Contains(_tail))
                            {
                                _trackingTimer.Stop();
                                _lastData = _data;
                                if (_data.Split(new string[] { _tail }, StringSplitOptions.RemoveEmptyEntries).Length > 0)
                                {
                                    foreach (string s in _data.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                                    {
                                        OnDataRecieved(new DataRecievedEventArgs(s));
                                    }
                                }
                                _data = string.Empty;
                            }
                        }
                    }
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(RecieveCallback), state);
                }
            }
            catch (Exception ex)
            {
                log.DebugFormat("Exception On RecieveCallback", ex);
                ConnectionStatus = EnCommuincationStatus.Error;

            }
        }
        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                _timeoutTimer.Stop();
                Socket handler = (Socket)ar.AsyncState;
                handler.EndSend(ar);
            }
            catch (Exception ex)
            {
                log.DebugFormat("Exception On SendCallback", ex);
                ConnectionStatus = EnCommuincationStatus.Error;
            }
        }
        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket _client = (Socket)ar.AsyncState;
                if (!_client.Connected)
                {
                    System.Threading.Thread.Sleep(_autoReconnectionInterval);
                    Action reconnect = new Action(Connect);
                    reconnect.Invoke();
                    return;
                }
                _client.EndConnect(ar);
                ConnectionStatus = EnCommuincationStatus.Connected;
                StateObject state = new StateObject();
                state.workSoket = _client;
                if (Protocol_Type != TypeProtocol.ModbusClient) _client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(RecieveCallback), state);
            }
            catch (Exception ex)
            {
                log.DebugFormat("Exception On ConnectCallback", ex);
                ConnectionStatus = EnCommuincationStatus.Error;
            }
        }
        #endregion

        #region Events
        public event EventHandler<DataRecievedEventArgs> DataRecieved;
        protected virtual void OnDataRecieved(DataRecievedEventArgs e)
        {
            DataRecieved?.Invoke(this, e);
            log.DebugFormat("Data Recieved :IP : {0} Port : {1} Data : {2}", _ipAddress, _tcpPort, e.Message);
        }
        public void RemoveEventHandler()
        {
            StatusChanged = null;
            DataRecieved = null;
        }
        public event EventHandler<StatusChangedEventArgs> StatusChanged;
        protected virtual void OnStatusChanged(StatusChangedEventArgs e)
        {
            StatusChanged?.Invoke(this, e);
        }
        #endregion

        public int SocketReceiveTImeOut
        {
            get { return _socket.ReceiveTimeout; }
            set { _socket.ReceiveTimeout = value; }
        }
        public TypeProtocol Protocol_Type
        {
            get { return _typeProc; }
            set { _typeProc = value; }
        }
        public bool AutoReconnection
        {
            get { return _autoReconnection; }
            set
            {
                log.DebugFormat("Auto Reconnection Value Changed {0} => {1}", _autoReconnection, value);
                _autoReconnection = value;
            }
        }
        public int TCPPort
        {
            get { return _tcpPort; }
            set
            {
                if (ConnectionStatus == EnCommuincationStatus.Connected || ConnectionStatus == EnCommuincationStatus.Connecting)
                {
                    log.DebugFormat("Invalid TCP Port Change While it is Connted");
                    throw new InvalidOperationException();
                }
                log.DebugFormat("TCPPort Value Changed {0} => {1}", _tcpPort, value);
                _tcpPort = value;
            }
        }
        public IPAddress IPAddress
        {
            get { return _ipAddress; }
            set
            {
                if (ConnectionStatus == EnCommuincationStatus.Connected || ConnectionStatus == EnCommuincationStatus.Connecting)
                {
                    log.DebugFormat("Invalid IP Address Change While it is Connted");
                    throw new InvalidOperationException();
                }
                log.DebugFormat("IPAddress Value Changed {0} => {1}", _ipAddress, value);
                _ipAddress = value;
            }
        }
        public string Tail
        {
            get { return _tail; }
            set
            {
                log.DebugFormat("Tail Value Changed {0} => {1}", _tail, value);
                _tail = value;
            }
        }
        public string Header
        {
            get { return _header; }
            set
            {
                log.DebugFormat("Header Value Changed {0} => {1}", _header, value);
                _header = value;
            }
        }
        public byte[] SendAndResponseModbus(byte[] _sendData)
        {
            lock (_sendAndResponseLockModbus)
            {
                byte[] _receiveData = new byte[_sendData.Length];

                try
                {
                    byte[] tmp = new byte[4096];

                    TcpClient tcpModbus = new TcpClient();
                    tcpModbus.Client = _socket;
                    NetworkStream streamModbus = tcpModbus.GetStream();

                    streamModbus.Write(_sendData, 0, _sendData.Length);
                    streamModbus.Read(tmp, 0, tmp.Length);

                    _receiveData = tmp;
                }
                catch (Exception ex)
                {
                    log.DebugFormat("Exception On SendAndResponseModbus", ex);
                    ConnectionStatus = EnCommuincationStatus.Error;
                }

                return _receiveData;
            }
        }
        public void Send(string data, bool headerAndTail = true, bool debugLog = true)
        {
            lock (_sendLock)
            {
                if (this.ConnectionStatus != EnCommuincationStatus.Connected)
                    return;
                if (headerAndTail)
                {
                    if (!string.IsNullOrEmpty(_header))
                        data = _header + data;
                    if (!string.IsNullOrEmpty(_tail))
                        data = data + _tail;
                }
                byte[] byteData;
                byteData = Encoding.GetEncoding("iso-8859-1").GetBytes(data);
                _timeoutTimer.Start();
                if (_socket != null)
                    _socket.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), _socket);
                if (debugLog)
                {
                    log.DebugFormat("Data Send :IP : {0} Port : {1} Data : {2}", _ipAddress, _tcpPort, data);
                    log.DebugFormat("Data Send :{0}", Encoding.Default.GetString(byteData));
                }
                else
                {
                    log.InfoFormat("Data Send :IP : {0} Port : {1} Data : {2}", _ipAddress, _tcpPort, data);
                    log.InfoFormat("Data Send :{0}", Encoding.Default.GetString(byteData));
                }
            }
        }
        public string SendAndResponse(string data, int sleepTime = 50, bool headerAndTail = true, bool debugLog = false)
        {
            lock (_sendAndResponseLock)
            {
                _lastData = string.Empty;
                this.Send(data, headerAndTail);
                DateTime start = DateTime.Now;
                log.DebugFormat("StartWaiting");
                while (start.AddMilliseconds(sleepTime) > DateTime.Now)
                {
                    if (!string.IsNullOrEmpty(_lastData))
                        break;
                    //System.Windows.Forms.Application.DoEvents();
                }
                if (debugLog)
                    log.DebugFormat("Data Send And Response : IP : {0} Port : {1} Data : {2} Last Data : {3}", _ipAddress, _tcpPort, data, _lastData);
                else
                    log.InfoFormat("Data Send And Response : IP : {0} Port : {1} Data : {2} Last Data : {3}", _ipAddress, _tcpPort, data, _lastData);
                return _lastData;
            }
        }
        public async Task<string> SendAndResponseAsync(string data, int sleepTime = 50, bool headerAndTail = true, bool debugLog = false)
        {
            _lastData = string.Empty;
            this.Send(data, headerAndTail);

            DateTime start = DateTime.Now;
            log.DebugFormat("StartWaiting");

            while (start.AddMilliseconds(sleepTime) > DateTime.Now)
            {
                if (!string.IsNullOrWhiteSpace(_lastData))
                    break;
                await Task.Delay(5);
            }

            if (debugLog)
                log.DebugFormat("Data Send And Response : IP : {0} Port : {1} Data : {2} Last Data : {3}", _ipAddress, _tcpPort, data, _lastData);
            else
                log.InfoFormat("Data Send And Response : IP : {0} Port : {1} Data : {2} Last Data : {3}", _ipAddress, _tcpPort, data, _lastData);

            return _lastData;
        }
        public double TimeOut
        {
            get { return _timeoutTimer.Interval; }
            set
            {
                log.DebugFormat("Time Out Value Changed {0} => {1}", _timeoutTimer.Interval, value);
                _timeoutTimer.Interval = value;
            }
        }
        public int AutoReconnectionInterval
        {
            get { return _autoReconnectionInterval; }
            set
            {
                log.DebugFormat("AutoReconnectionInterval Value Changed {0} => {1}", _autoReconnectionInterval, value);
                _autoReconnectionInterval = value;
            }
        }
        public double TrackingTimerInterval
        {
            get { return _trackingTimer.Interval; }
            set
            {
                log.DebugFormat("TrackingTimerInterval Value Changed {0} => {1}", _trackingTimer.Interval, value);
                _trackingTimer.Interval = value;
            }
        }
        public EnCommuincationStatus ConnectionStatus
        {
            get { return _connectionstatus; }
            set
            {
                if (_connectionstatus == value)
                    return;
                log.DebugFormat("ConnectionStatus Value Changed {0} => {1}", _connectionstatus, value);
                _connectionstatus = value;
                OnStatusChanged(new StatusChangedEventArgs(value));
            }
        }
        public void DisConnect()
        {
            if (_socket != null)
            {
                if (_socket.Connected)
                    _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
                _socket = null;
                log.DebugFormat("Disconnected IP : {0} Port : {1}", _ipAddress, _tcpPort);
            }
        }
        public void Dispose()
        {
            if (_socket != null)
            {
                if (_socket.Connected)
                    _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
                _socket.Dispose();
                _socket = null;
            }
        }
    }
}
