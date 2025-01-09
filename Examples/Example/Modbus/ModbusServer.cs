using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Diagnostics.Eventing.Reader;
using System.Net.NetworkInformation;
using log4net;
using Functions.TCP;

namespace Functions.Modbus
{

    public class ModbusFrame
    {
        public enum ProtocolType { ModbusTCP = 0, ModbusUDP = 1, ModbusRTU = 2 };
        public DateTime TimeStamp;
        public bool Request;
        public bool Response;
        public UInt16 Transaction_Identifier;
        public UInt16 Protocol_Identifier;
        public UInt16 Protocol_Length;
        public byte UnitIdentifier;
        public byte FunctionCode;
        public UInt16 StartAddress;
        public UInt16 Read_StartAddress;
        public UInt16 Write_StartAddress;
        public UInt16 Quantity;
        public UInt16 Read_Qty;
        public UInt16 Write_Qty;
        public byte ByteCount;
        public byte ExceptionCode;
        public byte ErrorCode;
        public UInt16[] ReceiveCoilValues;
        public UInt16[] ReceiveRegisterValues;
        public Int16[] SendRegisterValues;
        public bool[] SendCoilValues;
        public UInt16 CRC;
    }

    struct NetworkConnectionParameter
    {
        public NetworkStream stream;        //For TCP-Connection only
        public Byte[] bytes;
        public int portIn;                  //For UDP-Connection only
        public IPAddress ipAddressIn;       //For UDP-Connection only
    }

    public class ModbusData
    {
        public class HoldingRegisters
        {
            static HoldingRegisters _instance = null;
            public static HoldingRegisters Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        _instance = new HoldingRegisters();
                    }
                    return _instance;
                }
                set
                {
                    _instance = value;
                }
            }

            public Int16[] Content = new Int16[ModbusServer._maxAddressCnt];
            public Int16 this[int x]
            {
                get { return this.Content[x]; }
                set { this.Content[x] = value; }
            }
        }

        public class InputRegisters
        {
            static InputRegisters _instance = null;
            public static InputRegisters Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        _instance = new InputRegisters();
                    }
                    return _instance;
                }
                set
                {
                    _instance = value;
                }
            }

            public Int16[] Content = new Int16[ModbusServer._maxAddressCnt];
            public Int16 this[int x]
            {
                get { return this.Content[x]; }
                set { this.Content[x] = value; }
            }
        }

        public class Coils
        {
            static Coils _instance = null;
            public static Coils Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        _instance = new Coils();
                    }
                    return _instance;
                }
                set
                {
                    _instance = value;
                }
            }

            public bool[] Content = new bool[ModbusServer._maxAddressCnt];
            public bool this[int x]
            {
                get { return this.Content[x]; }
                set { this.Content[x] = value; }
            }
        }

        public class DiscreteInputs
        {
            static DiscreteInputs _instance = null;
            public static DiscreteInputs Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        _instance = new DiscreteInputs();
                    }
                    return _instance;
                }
                set
                {
                    _instance = value;
                }
            }

            public bool[] Content = new bool[ModbusServer._maxAddressCnt];
            public bool this[int x]
            {
                get { return this.Content[x]; }
                set { this.Content[x] = value; }
            }
        }
    }

    public class ModbusServer
    {
        static ModbusServer _instance = null;
        public static ModbusServer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ModbusServer();
                }
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        private class CommunicationInfo
        {
            public ModbusType CommunicationType { get; set; }
            public Stream Stream { get; set; }
            public int Port { get; set; }
            public IPAddress IPAddress { get; set; }
            public CommunicationInfo()
            {
                CommunicationType = ModbusType.ModbusTCP;
            }
        }

        private class ModbusFrameInfo
        {
            public EnModBusFunction ModbusFunction { get; set; }
            public ModbusFrame ReceivedFrame { get; set; }
            public ModbusFrame SendFrame { get; set; }
            public byte[] SendData { get; set; }
            public CommunicationInfo ModbusCommunicationInfo { get; set; }
            public ModbusFrameInfo()
            {
                ModbusFunction = EnModBusFunction.ReadWrite_Multiple_Registers;
                ReceivedFrame = new ModbusFrame();
                SendFrame = new ModbusFrame();
                SendData = new byte[4096];
                ModbusCommunicationInfo = new CommunicationInfo();
            }
        }

        public enum EnModBusFunction
        {
            Read_Coils = 1,
            Read_Discrete_Inputs = 2,
            Read_Holding_Registers = 3,
            Read_Input_Registers = 4,
            Write_Single_Coil = 5,
            Write_Single_Register = 6,
            Write_Multiple_Coils = 15,
            Write_Multiple_Register = 16,
            ReadWrite_Multiple_Registers = 23
        }
        public enum ModbusType
        {
            ModbusUDP,
            ModbusTCP,
            ModbusRTU
        }
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Int32 _port = 502;
        Byte[] bytes = new Byte[2100];
        public static readonly int _maxAddressCnt = 65534;
        private int numberOfConnections = 0;
        ModbusType _connectionType;
        private bool _isStartZero;
        public static int _isStartZeroNum;
        private int baudrate = 9600;
        private System.IO.Ports.Parity parity = Parity.Even;
        private System.IO.Ports.StopBits stopBits = StopBits.One;
        private string serialPort = "COM1";
        private SerialPort modbusRTU;
        private byte unitIdentifier = 1;
        private int portIn;
        private IPAddress ipAddressIn;
        private UdpClient modbusUDP;
        private IPEndPoint iPEndPoint;
        private TCPCommunication modbusTCP;
        Thread listenerThread;
        private ModbusFrame[] modbusLogData = new ModbusFrame[100];
        public bool FunctionCode1Disabled { get; set; }
        public bool FunctionCode2Disabled { get; set; }
        public bool FunctionCode3Disabled { get; set; }
        public bool FunctionCode4Disabled { get; set; }
        public bool FunctionCode5Disabled { get; set; }
        public bool FunctionCode6Disabled { get; set; }
        public bool FunctionCode15Disabled { get; set; }
        public bool FunctionCode16Disabled { get; set; }
        public bool FunctionCode23Disabled { get; set; }
        public bool PortChanged { get; set; }
        object lockCoils = new object();
        object lockHoldingRegisters = new object();
        private volatile bool shouldStop;

        private IPAddress localIPAddress = IPAddress.Any;
        public int AddressCount
        {
            get { return _maxAddressCnt; }
        }
        public bool IsStartZero
        {
            get { return _isStartZero; }
            set
            {
                _isStartZeroNum = value ? -1 : 0;
                _isStartZero = value;
            }
        }
        public EnCommuincationStatus TcpConnectionStatus
        {
            get 
            {
                if (modbusTCP == null)
                    return EnCommuincationStatus.NeverConnected;
                else
                    return modbusTCP.ConnectionStatus; 
            }
        }
        public bool IsConnected
        {
            get
            {
                bool _Connected = false;
                if (_connectionType == ModbusType.ModbusRTU)
                {
                    _Connected = modbusRTU != null && modbusRTU.IsOpen;
                }
                else if (_connectionType == ModbusType.ModbusTCP)
                {
                    _Connected = modbusTCP != null && modbusTCP.ConnectionStatus == EnCommuincationStatus.Connected;
                }
                else if (_connectionType == ModbusType.ModbusUDP)
                {
                    _Connected = modbusUDP != null && modbusUDP.Client != null && modbusUDP.Client.IsBound;
                }
                return _Connected;
            }
        }
        public ModbusType ConnectionType
        {
            get { return _connectionType; }
            set { _connectionType = value; }
        }

        public IPAddress LocalIPAddress
        {
            get { return localIPAddress; }
            set { if (listenerThread == null) localIPAddress = value; }
        }

        public ModbusServer()
        {
            // 펑션 코드 비활성화 기능 Default Off
            this.FunctionCode1Disabled = false;
            this.FunctionCode2Disabled = false;
            this.FunctionCode3Disabled = false;
            this.FunctionCode4Disabled = false;
            this.FunctionCode5Disabled = false;
            this.FunctionCode6Disabled = false;
            this.FunctionCode15Disabled = false;
            this.FunctionCode16Disabled = false;
            this.FunctionCode23Disabled = false;

            this.ConnectionType = ModbusType.ModbusTCP;
            this.IsStartZero = false;
        }

        #region events
        public delegate void CoilsChangedHandler(int coil, int numberOfCoils);
        public event CoilsChangedHandler CoilsChanged;

        public delegate void HoldingRegistersChangedHandler(int register, int numberOfRegisters);
        public event HoldingRegistersChangedHandler HoldingRegistersChanged;

        public delegate void NumberOfConnectedClientsChangedHandler();
        public event NumberOfConnectedClientsChangedHandler NumberOfConnectedClientsChanged;

        public delegate void LogDataChangedHandler();
        public event LogDataChangedHandler LogDataChanged;
        #endregion

        public void Listen()
        {
            listenerThread = new Thread(ListenerThread);
            listenerThread.Start();
        }

        public void StopListening()
        {
            if ((ConnectionType == ModbusType.ModbusRTU) & (modbusRTU != null))
            {
                if (modbusRTU.IsOpen)
                    modbusRTU.Close();
                shouldStop = true;
            }
            if (modbusTCP == null || listenerThread == null) return;
            try
            {
                modbusTCP.DisConnect();
                listenerThread.Abort();
            }
            catch (Exception) { }
            listenerThread.Join();
        }

        private void ListenerThread()
        {
            if (ConnectionType == ModbusType.ModbusTCP)
            {
                if (modbusUDP != null)
                {
                    try
                    {
                        modbusUDP.Close();
                    }
                    catch (Exception) { }
                }
                modbusTCP = new TCPCommunication(LocalIPAddress, _port, TCPCommunication.TypeProtocol.ModbusServer);
                modbusTCP.Connect();
                log.DebugFormat("Modbus Open Server (TCP) _ Port {0}, IP : {1}", _port, LocalIPAddress);
                modbusTCP.ModbusDataReceived += new TCPCommunication.ModbusDataReceivedEventArgs(ReceivedModbusData);
            }
            else if (ConnectionType == ModbusType.ModbusRTU)
            {
                if (modbusRTU == null)
                {
                    log.DebugFormat("Modbus Open Server (Serial) _ Port {0}", serialPort);
                    modbusRTU = new SerialPort();
                    modbusRTU.PortName = serialPort;
                    modbusRTU.BaudRate = this.baudrate;
                    modbusRTU.Parity = this.parity;
                    modbusRTU.StopBits = stopBits;
                    modbusRTU.WriteTimeout = 10000;
                    modbusRTU.ReadTimeout = 1000;
                    modbusRTU.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                    modbusRTU.Open();
                }
            }
            else
            {
                while (!shouldStop)
                {
                    if (ConnectionType == ModbusType.ModbusUDP)
                    {
                        if (modbusUDP == null | PortChanged)
                        {
                            IPEndPoint localEndoint = new IPEndPoint(LocalIPAddress, _port);
                            modbusUDP = new UdpClient(localEndoint);
                            log.DebugFormat("Modbus Open Server (UDP) _ Port {0}, IP : {1}", _port, LocalIPAddress);
                            modbusUDP.Client.ReceiveTimeout = 1000;
                            iPEndPoint = new IPEndPoint(IPAddress.Any, _port);
                            PortChanged = false;
                        }
                        if (modbusTCP != null)
                            modbusTCP.DisConnect();
                        try
                        {
                            bytes = modbusUDP.Receive(ref iPEndPoint);
                            portIn = iPEndPoint.Port;
                            NetworkConnectionParameter networkConnectionParameter = new NetworkConnectionParameter();
                            networkConnectionParameter.bytes = bytes;
                            ipAddressIn = iPEndPoint.Address;
                            networkConnectionParameter.portIn = portIn;
                            networkConnectionParameter.ipAddressIn = ipAddressIn;
                            ParameterizedThreadStart pts = new ParameterizedThreadStart(this.ReceivedModbusData);
                            Thread processDataThread = new Thread(pts);
                            processDataThread.Start(networkConnectionParameter);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }

        #region SerialHandler
        private bool dataReceived = false;
        private byte[] readBuffer = new byte[2094];
        private DateTime lastReceive;
        private int nextSign = 0;
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            int silence = 4000 / baudrate;
            if ((DateTime.Now.Ticks - lastReceive.Ticks) > TimeSpan.TicksPerMillisecond * silence)
                nextSign = 0;

            SerialPort sp = (SerialPort)sender;

            int numbytes = sp.BytesToRead;
            byte[] rxbytearray = new byte[numbytes];

            sp.Read(rxbytearray, 0, numbytes);

            Array.Copy(rxbytearray, 0, readBuffer, nextSign, rxbytearray.Length);
            lastReceive = DateTime.Now;
            nextSign = numbytes + nextSign;
            if (DetectValidModbusFrame(readBuffer, nextSign))
            {
                dataReceived = true;
                nextSign = 0;

                NetworkConnectionParameter networkConnectionParameter = new NetworkConnectionParameter();
                networkConnectionParameter.bytes = readBuffer;
                ParameterizedThreadStart pts = new ParameterizedThreadStart(this.ReceivedModbusData);
                Thread processDataThread = new Thread(pts);
                processDataThread.Start(networkConnectionParameter);
                dataReceived = false;
            }
            else
                dataReceived = false;
        }
        #endregion

        object lockReceivedModbusData = new object();
        private void ReceivedModbusData(object networkConnectionParameter)
        {
            lock (lockReceivedModbusData)
            {
                EnModBusFunction _functionCode;
                Byte[] bytes = new byte[((NetworkConnectionParameter)networkConnectionParameter).bytes.Length];
                log.DebugFormat("Received Data : {0}", BitConverter.ToString(bytes));
                NetworkStream pStream = ((NetworkConnectionParameter)networkConnectionParameter).stream;
                int pPort = ((NetworkConnectionParameter)networkConnectionParameter).portIn;
                IPAddress pIPaddress = ((NetworkConnectionParameter)networkConnectionParameter).ipAddressIn;

                Array.Copy(((NetworkConnectionParameter)networkConnectionParameter).bytes, 0, bytes, 0, ((NetworkConnectionParameter)networkConnectionParameter).bytes.Length);

                CommunicationInfo pModbusInfo = new CommunicationInfo();
                pModbusInfo.CommunicationType = _connectionType;
                pModbusInfo.Stream = pStream;
                pModbusInfo.Port = pPort;
                pModbusInfo.IPAddress = pIPaddress;

                ModbusFrame receiveFrame = new ModbusFrame();

                try
                {
                    UInt16[] wordData = new UInt16[1];
                    byte[] byteData = new byte[2];
                    receiveFrame.TimeStamp = DateTime.Now;
                    receiveFrame.Request = true;
                    if (ConnectionType != ModbusType.ModbusRTU)
                    {
                        //Lese Transaction identifier
                        byteData[1] = bytes[0];
                        byteData[0] = bytes[1];
                        Buffer.BlockCopy(byteData, 0, wordData, 0, 2);
                        receiveFrame.Transaction_Identifier = wordData[0];

                        //Lese Protocol identifier
                        byteData[1] = bytes[2];
                        byteData[0] = bytes[3];
                        Buffer.BlockCopy(byteData, 0, wordData, 0, 2);
                        receiveFrame.Protocol_Identifier = wordData[0];

                        //Lese length
                        byteData[1] = bytes[4];
                        byteData[0] = bytes[5];
                        Buffer.BlockCopy(byteData, 0, wordData, 0, 2);
                        receiveFrame.Protocol_Length = wordData[0];
                    }

                    //Lese unit identifier
                    receiveFrame.UnitIdentifier = bytes[6 - 6 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                    //Check UnitIdentifier
                    if ((receiveFrame.UnitIdentifier != this.unitIdentifier) & (receiveFrame.UnitIdentifier != 0))
                        return;

                    // Lese function code
                    receiveFrame.FunctionCode = bytes[7 - 6 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                    _functionCode = (EnModBusFunction)(receiveFrame.FunctionCode);

                    // Lese starting address 
                    byteData[1] = bytes[8 - 6 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                    byteData[0] = bytes[9 - 6 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                    Buffer.BlockCopy(byteData, 0, wordData, 0, 2);
                    receiveFrame.StartAddress = wordData[0];

                    if (receiveFrame.FunctionCode <= 4)
                    {
                        // Lese quantity
                        byteData[1] = bytes[10 - 6 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                        byteData[0] = bytes[11 - 6 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                        Buffer.BlockCopy(byteData, 0, wordData, 0, 2);
                        receiveFrame.Quantity = wordData[0];
                    }
                    if (receiveFrame.FunctionCode == 5)
                    {
                        receiveFrame.ReceiveCoilValues = new ushort[1];
                        // Lese Value
                        byteData[1] = bytes[10 - 6 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                        byteData[0] = bytes[11 - 6 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                        Buffer.BlockCopy(byteData, 0, receiveFrame.ReceiveCoilValues, 0, 2);
                    }
                    if (receiveFrame.FunctionCode == 6)
                    {
                        receiveFrame.ReceiveRegisterValues = new ushort[1];
                        // Lese Value
                        byteData[1] = bytes[10 - 6 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                        byteData[0] = bytes[11 - 6 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                        Buffer.BlockCopy(byteData, 0, receiveFrame.ReceiveRegisterValues, 0, 2);
                    }
                    if (receiveFrame.FunctionCode == 15)
                    {
                        // Lese quantity
                        byteData[1] = bytes[10 - 6 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                        byteData[0] = bytes[11 - 6 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                        Buffer.BlockCopy(byteData, 0, wordData, 0, 2);
                        receiveFrame.Quantity = wordData[0];

                        receiveFrame.ByteCount = bytes[12 - 6 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];

                        if ((receiveFrame.ByteCount % 2) != 0)
                            receiveFrame.ReceiveCoilValues = new ushort[receiveFrame.ByteCount / 2 + 1];
                        else
                            receiveFrame.ReceiveCoilValues = new ushort[receiveFrame.ByteCount / 2];
                        // Lese Value
                        Buffer.BlockCopy(bytes, 13 - 6 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU), receiveFrame.ReceiveCoilValues, 0, receiveFrame.ByteCount);
                    }
                    if (receiveFrame.FunctionCode == 16)
                    {
                        // Lese quantity
                        byteData[1] = bytes[10 - 6 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                        byteData[0] = bytes[11 - 6 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                        Buffer.BlockCopy(byteData, 0, wordData, 0, 2);
                        receiveFrame.Quantity = wordData[0];

                        receiveFrame.ByteCount = bytes[12 - 6 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                        receiveFrame.ReceiveRegisterValues = new ushort[receiveFrame.Quantity];
                        for (int i = 0; i < receiveFrame.Quantity; i++)
                        {
                            // Lese Value
                            byteData[1] = bytes[13 + i * 2 - 6 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                            byteData[0] = bytes[14 + i * 2 - 6 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                            Buffer.BlockCopy(byteData, 0, receiveFrame.ReceiveRegisterValues, i * 2, 2);
                        }
                    }
                    if (receiveFrame.FunctionCode == 23)
                    {
                        // Lese starting Address Read
                        byteData[1] = bytes[8 - 6 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                        byteData[0] = bytes[9 - 6 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                        Buffer.BlockCopy(byteData, 0, wordData, 0, 2);
                        receiveFrame.Read_StartAddress = wordData[0];
                        // Lese quantity Read
                        byteData[1] = bytes[10 - 6 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                        byteData[0] = bytes[11 - 6 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                        Buffer.BlockCopy(byteData, 0, wordData, 0, 2);
                        receiveFrame.Read_Qty = wordData[0];
                        // Lese starting Address Write
                        byteData[1] = bytes[12 - 6 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                        byteData[0] = bytes[13 - 6 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                        Buffer.BlockCopy(byteData, 0, wordData, 0, 2);
                        receiveFrame.Write_StartAddress = wordData[0];
                        // Lese quantity Write
                        byteData[1] = bytes[14 - 6 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                        byteData[0] = bytes[15 - 6 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                        Buffer.BlockCopy(byteData, 0, wordData, 0, 2);
                        receiveFrame.Write_Qty = wordData[0];

                        receiveFrame.ByteCount = bytes[16 - 6 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                        receiveFrame.ReceiveRegisterValues = new ushort[receiveFrame.Write_Qty];
                        for (int i = 0; i < receiveFrame.Write_Qty; i++)
                        {
                            // Lese Value
                            byteData[1] = bytes[17 + i * 2 - 6 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                            byteData[0] = bytes[18 + i * 2 - 6 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                            Buffer.BlockCopy(byteData, 0, receiveFrame.ReceiveRegisterValues, i * 2, 2);
                        }
                    }
                }
                catch (Exception exc)
                {
                    _functionCode = (EnModBusFunction)(receiveFrame.FunctionCode);
                }

                ModbusFrameInfo ReceivedFrameInfo = new ModbusFrameInfo();
                ReceivedFrameInfo.ModbusFunction = _functionCode;
                ReceivedFrameInfo.ReceivedFrame = receiveFrame;
                ReceivedFrameInfo.ModbusCommunicationInfo = pModbusInfo;

                ModbusFrameInfo TotalFrameInfo = Get_TotalFrame_Info(ReceivedFrameInfo);
                bool YN_Success = SendAnswer(TotalFrameInfo);

                CreateLogData(TotalFrameInfo.ReceivedFrame, TotalFrameInfo.SendFrame);
                if (LogDataChanged != null)
                    LogDataChanged();
            }
        }

        #region Method CreateAnswer

        private ModbusFrameInfo Get_TotalFrame_Info(ModbusFrameInfo pReceivedFrameInfo)
        {
            ModbusFrameInfo pTotalInfo = pReceivedFrameInfo;

            switch (pReceivedFrameInfo.ModbusFunction)
            {
                // Read Coils
                case EnModBusFunction.Read_Coils:
                    if (!FunctionCode1Disabled)
                        pTotalInfo = Get_ReadCoils_Data(pReceivedFrameInfo);
                    else
                    {
                        pTotalInfo.SendFrame.ErrorCode = (byte)(pReceivedFrameInfo.ReceivedFrame.FunctionCode + 0x80);
                        pTotalInfo.SendFrame.ExceptionCode = 1;
                    }
                    break;
                // Read Discrete Inputs
                case EnModBusFunction.Read_Discrete_Inputs:
                    if (!FunctionCode2Disabled)
                        pTotalInfo = Get_ReadDiscreteInputs_Data(pReceivedFrameInfo);
                    else
                    {
                        pTotalInfo.SendFrame.ErrorCode = (byte)(pReceivedFrameInfo.ReceivedFrame.FunctionCode + 0x80);
                        pTotalInfo.SendFrame.ExceptionCode = 1;
                    }
                    break;
                // Read Holding Registers
                case EnModBusFunction.Read_Holding_Registers:
                    if (!FunctionCode3Disabled)
                        pTotalInfo = Get_ReadHoldingRegisters_Data(pReceivedFrameInfo);
                    else
                    {
                        pTotalInfo.SendFrame.ErrorCode = (byte)(pReceivedFrameInfo.ReceivedFrame.FunctionCode + 0x80);
                        pTotalInfo.SendFrame.ExceptionCode = 1;
                    }
                    break;
                // Read Input Registers
                case EnModBusFunction.Read_Input_Registers:
                    if (!FunctionCode4Disabled)
                        pTotalInfo = Get_ReadInputRegisters_Data(pReceivedFrameInfo);
                    else
                    {
                        pTotalInfo.SendFrame.ErrorCode = (byte)(pReceivedFrameInfo.ReceivedFrame.FunctionCode + 0x80);
                        pTotalInfo.SendFrame.ExceptionCode = 1;
                    }
                    break;
                // Write single coil
                case EnModBusFunction.Write_Single_Coil:
                    if (!FunctionCode5Disabled)
                        pTotalInfo = Get_WriteSingleCoil_Data(pReceivedFrameInfo);
                    else
                    {
                        pTotalInfo.SendFrame.ErrorCode = (byte)(pReceivedFrameInfo.ReceivedFrame.FunctionCode + 0x80);
                        pTotalInfo.SendFrame.ExceptionCode = 1;
                    }
                    break;
                // Write single register
                case EnModBusFunction.Write_Single_Register:
                    if (!FunctionCode6Disabled)
                        pTotalInfo = Get_WriteSingleRegister_Data(pReceivedFrameInfo);
                    else
                    {
                        pTotalInfo.SendFrame.ErrorCode = (byte)(pReceivedFrameInfo.ReceivedFrame.FunctionCode + 0x80);
                        pTotalInfo.SendFrame.ExceptionCode = 1;
                    }
                    break;
                // Write Multiple coils
                case EnModBusFunction.Write_Multiple_Coils:
                    if (!FunctionCode15Disabled)
                        pTotalInfo = Get_WriteMultipleCoils_Data(pReceivedFrameInfo);
                    else
                    {
                        pTotalInfo.SendFrame.ErrorCode = (byte)(pReceivedFrameInfo.ReceivedFrame.FunctionCode + 0x80);
                        pTotalInfo.SendFrame.ExceptionCode = 1;
                    }
                    break;
                // Write Multiple registers
                case EnModBusFunction.Write_Multiple_Register:
                    if (!FunctionCode16Disabled)
                        pTotalInfo = Get_WriteMultipleRegisters_Data(pReceivedFrameInfo);
                    else
                    {
                        pTotalInfo.SendFrame.ErrorCode = (byte)(pReceivedFrameInfo.ReceivedFrame.FunctionCode + 0x80);
                        pTotalInfo.SendFrame.ExceptionCode = 1;
                    }
                    break;
                // Error: Function Code not supported
                case EnModBusFunction.ReadWrite_Multiple_Registers:
                    if (!FunctionCode23Disabled)
                        pTotalInfo = Get_ReadWriteMultipleRegisters_Data(pReceivedFrameInfo);
                    else
                    {
                        pTotalInfo.SendFrame.ErrorCode = (byte)(pReceivedFrameInfo.ReceivedFrame.FunctionCode + 0x80);
                        pTotalInfo.SendFrame.ExceptionCode = 1;
                    }
                    break;
                // Error: Function Code not supported
                default:
                    pTotalInfo.SendFrame.ErrorCode = (byte)(pReceivedFrameInfo.ReceivedFrame.FunctionCode + 0x80);
                    pTotalInfo.SendFrame.ExceptionCode = 1;
                    break;
            }
            pTotalInfo.SendFrame.TimeStamp = DateTime.Now;

            return pTotalInfo;
        }

        private ModbusFrameInfo Get_ReadCoils_Data(ModbusFrameInfo pFrame)
        {
            ModbusFrameInfo pInfo = pFrame;

            // SendFrame 설정
            try
            {
                pInfo.SendFrame = new ModbusFrame();

                pInfo.SendFrame.Response = true;

                pInfo.SendFrame.Transaction_Identifier = pFrame.ReceivedFrame.Transaction_Identifier;
                pInfo.SendFrame.Protocol_Identifier = pFrame.ReceivedFrame.Protocol_Identifier;

                pInfo.SendFrame.UnitIdentifier = this.unitIdentifier;
                pInfo.SendFrame.FunctionCode = pFrame.ReceivedFrame.FunctionCode;
                if ((pFrame.ReceivedFrame.Quantity < 1) | (pFrame.ReceivedFrame.Quantity > 0x07D0))  //Invalid quantity
                {
                    pInfo.SendFrame.ErrorCode = (byte)(pFrame.ReceivedFrame.FunctionCode + 0x80);
                    pInfo.SendFrame.ExceptionCode = 3;
                }
                if (((pFrame.ReceivedFrame.StartAddress + _isStartZeroNum + 1 + pFrame.ReceivedFrame.Quantity) > _maxAddressCnt) | (pFrame.ReceivedFrame.StartAddress < 0))     //Invalid Starting adress or Starting address + quantity
                {
                    pInfo.SendFrame.ErrorCode = (byte)(pFrame.ReceivedFrame.FunctionCode + 0x80);
                    pInfo.SendFrame.ExceptionCode = 2;
                }
                if (pInfo.SendFrame.ExceptionCode == 0)
                {
                    if ((pFrame.ReceivedFrame.Quantity % 8) == 0)
                        pInfo.SendFrame.ByteCount = (byte)(pFrame.ReceivedFrame.Quantity / 8);
                    else
                        pInfo.SendFrame.ByteCount = (byte)(pFrame.ReceivedFrame.Quantity / 8 + 1);

                    pInfo.SendFrame.SendCoilValues = new bool[pFrame.ReceivedFrame.Quantity];
                    lock (lockCoils)
                        Array.Copy(ModbusData.Coils.Instance.Content, pFrame.ReceivedFrame.StartAddress + _isStartZeroNum + 1, pInfo.SendFrame.SendCoilValues, 0, pFrame.ReceivedFrame.Quantity);
                }
            }
            catch (Exception) { }

            // SendData 설정
            try
            {
                if (pInfo.SendFrame.ExceptionCode > 0)
                    pInfo.SendData = new byte[9 + 2 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                else
                    pInfo.SendData = new byte[9 + pInfo.SendFrame.ByteCount + 2 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];

                Byte[] byteData = new byte[2];

                pInfo.SendFrame.Protocol_Length = (byte)(pInfo.SendData.Length - 6);

                //Send Transaction identifier
                byteData = BitConverter.GetBytes((int)pInfo.SendFrame.Transaction_Identifier);
                pInfo.SendData[0] = byteData[1];
                pInfo.SendData[1] = byteData[0];

                //Send Protocol identifier
                byteData = BitConverter.GetBytes((int)pInfo.SendFrame.Protocol_Identifier);
                pInfo.SendData[2] = byteData[1];
                pInfo.SendData[3] = byteData[0];

                //Send length
                byteData = BitConverter.GetBytes((int)pInfo.SendFrame.Protocol_Length);
                pInfo.SendData[4] = byteData[1];
                pInfo.SendData[5] = byteData[0];

                //Unit Identifier
                pInfo.SendData[6] = pInfo.SendFrame.UnitIdentifier;

                //Function Code
                pInfo.SendData[7] = pInfo.SendFrame.FunctionCode;

                //ByteCount
                pInfo.SendData[8] = pInfo.SendFrame.ByteCount;

                if (pInfo.SendFrame.ExceptionCode > 0)
                {
                    pInfo.SendData[7] = pInfo.SendFrame.ErrorCode;
                    pInfo.SendData[8] = pInfo.SendFrame.ExceptionCode;
                    pInfo.SendFrame.SendCoilValues = null;
                }

                if (pInfo.SendFrame.SendCoilValues != null)
                {
                    for (int i = 0; i < (pInfo.SendFrame.ByteCount); i++)
                    {
                        byteData = new byte[2];
                        for (int j = 0; j < 8; j++)
                        {

                            byte boolValue;
                            if (pInfo.SendFrame.SendCoilValues[i * 8 + j] == true)
                                boolValue = 1;
                            else
                                boolValue = 0;
                            byteData[1] = (byte)((byteData[1]) | (boolValue << j));
                            if ((i * 8 + j + 1) >= pInfo.SendFrame.SendCoilValues.Length)
                                break;
                        }
                        pInfo.SendData[9 + i] = byteData[1];
                    }
                }

                if (pInfo.ModbusCommunicationInfo.CommunicationType == ModbusType.ModbusRTU)
                {
                    if (modbusRTU.IsOpen)
                    {
                        //Create CRC
                        pInfo.SendFrame.CRC = CalculateCRC(pInfo.SendData, Convert.ToUInt16(pInfo.SendData.Length - 8), 6);
                        byteData = BitConverter.GetBytes((int)pInfo.SendFrame.CRC);
                        pInfo.SendData[pInfo.SendData.Length - 2] = byteData[0];
                        pInfo.SendData[pInfo.SendData.Length - 1] = byteData[1];
                    }
                }
            }
            catch (Exception) { }

            return pInfo;
        }

        private ModbusFrameInfo Get_ReadDiscreteInputs_Data(ModbusFrameInfo pFrame)
        {
            ModbusFrameInfo pInfo = pFrame;

            // SendFrame 설정
            try
            {
                pInfo.SendFrame = new ModbusFrame();

                pInfo.SendFrame.Response = true;

                pInfo.SendFrame.Transaction_Identifier = pFrame.ReceivedFrame.Transaction_Identifier;
                pInfo.SendFrame.Protocol_Identifier = pFrame.ReceivedFrame.Protocol_Identifier;

                pInfo.SendFrame.UnitIdentifier = this.unitIdentifier;
                pInfo.SendFrame.FunctionCode = pFrame.ReceivedFrame.FunctionCode;
                if ((pFrame.ReceivedFrame.Quantity < 1) | (pFrame.ReceivedFrame.Quantity > 0x07D0))  //Invalid quantity
                {
                    pInfo.SendFrame.ErrorCode = (byte)(pFrame.ReceivedFrame.FunctionCode + 0x80);
                    pInfo.SendFrame.ExceptionCode = 3;
                }
                if (((pFrame.ReceivedFrame.StartAddress + _isStartZeroNum + 1 + pFrame.ReceivedFrame.Quantity) > _maxAddressCnt) | (pFrame.ReceivedFrame.StartAddress < 0))   //Invalid Starting adress or Starting address + quantity
                {
                    pInfo.SendFrame.ErrorCode = (byte)(pFrame.ReceivedFrame.FunctionCode + 0x80);
                    pInfo.SendFrame.ExceptionCode = 2;
                }
                if (pInfo.SendFrame.ExceptionCode == 0)
                {
                    if ((pFrame.ReceivedFrame.Quantity % 8) == 0)
                        pInfo.SendFrame.ByteCount = (byte)(pFrame.ReceivedFrame.Quantity / 8);
                    else
                        pInfo.SendFrame.ByteCount = (byte)(pFrame.ReceivedFrame.Quantity / 8 + 1);

                    pInfo.SendFrame.SendCoilValues = new bool[pFrame.ReceivedFrame.Quantity];
                    Array.Copy(ModbusData.DiscreteInputs.Instance.Content, pFrame.ReceivedFrame.StartAddress + _isStartZeroNum + 1, pInfo.SendFrame.SendCoilValues, 0, pFrame.ReceivedFrame.Quantity);
                }
            }
            catch (Exception) { }

            // SendData 설정
            try
            {
                if (pInfo.SendFrame.ExceptionCode > 0)
                    pInfo.SendData = new byte[9 + 2 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                else
                    pInfo.SendData = new byte[9 + pInfo.SendFrame.ByteCount + 2 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                Byte[] byteData = new byte[2];
                pInfo.SendFrame.Protocol_Length = (byte)(pInfo.SendData.Length - 6);

                //Send Transaction identifier
                byteData = BitConverter.GetBytes((int)pInfo.SendFrame.Transaction_Identifier);
                pInfo.SendData[0] = byteData[1];
                pInfo.SendData[1] = byteData[0];

                //Send Protocol identifier
                byteData = BitConverter.GetBytes((int)pInfo.SendFrame.Protocol_Identifier);
                pInfo.SendData[2] = byteData[1];
                pInfo.SendData[3] = byteData[0];

                //Send length
                byteData = BitConverter.GetBytes((int)pInfo.SendFrame.Protocol_Length);
                pInfo.SendData[4] = byteData[1];
                pInfo.SendData[5] = byteData[0];

                //Unit Identifier
                pInfo.SendData[6] = pInfo.SendFrame.UnitIdentifier;

                //Function Code
                pInfo.SendData[7] = pInfo.SendFrame.FunctionCode;

                //ByteCount
                pInfo.SendData[8] = pInfo.SendFrame.ByteCount;

                if (pInfo.SendFrame.ExceptionCode > 0)
                {
                    pInfo.SendData[7] = pInfo.SendFrame.ErrorCode;
                    pInfo.SendData[8] = pInfo.SendFrame.ExceptionCode;
                    pInfo.SendFrame.SendCoilValues = null;
                }

                if (pInfo.SendFrame.SendCoilValues != null)
                {
                    for (int i = 0; i < (pInfo.SendFrame.ByteCount); i++)
                    {
                        byteData = new byte[2];
                        for (int j = 0; j < 8; j++)
                        {
                            byte boolValue;
                            if (pInfo.SendFrame.SendCoilValues[i * 8 + j] == true)
                                boolValue = 1;
                            else
                                boolValue = 0;
                            byteData[1] = (byte)((byteData[1]) | (boolValue << j));
                            if ((i * 8 + j + 1) >= pInfo.SendFrame.SendCoilValues.Length)
                                break;
                        }
                        pInfo.SendData[9 + i] = byteData[1];
                    }
                }

                if (pInfo.ModbusCommunicationInfo.CommunicationType == ModbusType.ModbusRTU)
                {
                    if (modbusRTU.IsOpen)
                    {
                        //Create CRC
                        pInfo.SendFrame.CRC = CalculateCRC(pInfo.SendData, Convert.ToUInt16(pInfo.SendData.Length - 8), 6);
                        byteData = BitConverter.GetBytes((int)pInfo.SendFrame.CRC);
                        pInfo.SendData[pInfo.SendData.Length - 2] = byteData[0];
                        pInfo.SendData[pInfo.SendData.Length - 1] = byteData[1];
                    }
                }
            }
            catch (Exception) { }

            return pInfo;
        }

        private ModbusFrameInfo Get_ReadHoldingRegisters_Data(ModbusFrameInfo pFrame)
        {
            ModbusFrameInfo pInfo = pFrame;

            // SendFrame 설정
            try
            {
                pInfo.SendFrame = new ModbusFrame();

                pInfo.SendFrame.Response = true;

                pInfo.SendFrame.Transaction_Identifier = pFrame.ReceivedFrame.Transaction_Identifier;
                pInfo.SendFrame.Protocol_Identifier = pFrame.ReceivedFrame.Protocol_Identifier;

                pInfo.SendFrame.UnitIdentifier = this.unitIdentifier;
                pInfo.SendFrame.FunctionCode = pFrame.ReceivedFrame.FunctionCode;
                if ((pFrame.ReceivedFrame.Quantity < 1) | (pFrame.ReceivedFrame.Quantity > 0x007D))  //Invalid quantity
                {
                    pInfo.SendFrame.ErrorCode = (byte)(pFrame.ReceivedFrame.FunctionCode + 0x80);
                    pInfo.SendFrame.ExceptionCode = 3;
                }
                if (((pFrame.ReceivedFrame.StartAddress + 1 + _isStartZeroNum + pFrame.ReceivedFrame.Quantity) > _maxAddressCnt) | (pFrame.ReceivedFrame.StartAddress < 0))   //Invalid Starting adress or Starting address + quantity
                {
                    pInfo.SendFrame.ErrorCode = (byte)(pFrame.ReceivedFrame.FunctionCode + 0x80);
                    pInfo.SendFrame.ExceptionCode = 2;
                }
                if (pInfo.SendFrame.ExceptionCode == 0)
                {
                    pInfo.SendFrame.ByteCount = (byte)(2 * pFrame.ReceivedFrame.Quantity);
                    pInfo.SendFrame.SendRegisterValues = new Int16[pFrame.ReceivedFrame.Quantity];
                    lock (lockHoldingRegisters)
                        Buffer.BlockCopy(ModbusData.HoldingRegisters.Instance.Content, (pFrame.ReceivedFrame.StartAddress + _isStartZeroNum) * 2 + 2, pInfo.SendFrame.SendRegisterValues, 0, pFrame.ReceivedFrame.Quantity * 2);
                }
                if (pInfo.SendFrame.ExceptionCode > 0)
                    pInfo.SendFrame.Protocol_Length = 0x03;
                else
                    pInfo.SendFrame.Protocol_Length = (ushort)(0x03 + pInfo.SendFrame.ByteCount);
            }
            catch (Exception ex) { }

            // SendData 설정
            try
            {
                if (pInfo.SendFrame.ExceptionCode > 0)
                    pInfo.SendData = new byte[9 + 2 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                else
                    pInfo.SendData = new byte[9 + pInfo.SendFrame.ByteCount + 2 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                Byte[] byteData = new byte[2];
                pInfo.SendFrame.Protocol_Length = (byte)(pInfo.SendData.Length - 6);

                //Send Transaction identifier
                byteData = BitConverter.GetBytes((int)pInfo.SendFrame.Transaction_Identifier);
                pInfo.SendData[0] = byteData[1];
                pInfo.SendData[1] = byteData[0];

                //Send Protocol identifier
                byteData = BitConverter.GetBytes((int)pInfo.SendFrame.Protocol_Identifier);
                pInfo.SendData[2] = byteData[1];
                pInfo.SendData[3] = byteData[0];

                //Send length
                byteData = BitConverter.GetBytes((int)pInfo.SendFrame.Protocol_Length);
                pInfo.SendData[4] = byteData[1];
                pInfo.SendData[5] = byteData[0];

                //Unit Identifier
                pInfo.SendData[6] = pInfo.SendFrame.UnitIdentifier;

                //Function Code
                pInfo.SendData[7] = pInfo.SendFrame.FunctionCode;

                //ByteCount
                pInfo.SendData[8] = pInfo.SendFrame.ByteCount;

                if (pInfo.SendFrame.ExceptionCode > 0)
                {
                    pInfo.SendData[7] = pInfo.SendFrame.ErrorCode;
                    pInfo.SendData[8] = pInfo.SendFrame.ExceptionCode;
                    pInfo.SendFrame.SendRegisterValues = null;
                }

                if (pInfo.SendFrame.SendRegisterValues != null)
                {
                    for (int i = 0; i < (pInfo.SendFrame.ByteCount / 2); i++)
                    {
                        byteData = BitConverter.GetBytes((Int16)pInfo.SendFrame.SendRegisterValues[i]);
                        pInfo.SendData[9 + i * 2] = byteData[1];
                        pInfo.SendData[10 + i * 2] = byteData[0];
                    }
                }

                if (pInfo.ModbusCommunicationInfo.CommunicationType == ModbusType.ModbusRTU)
                {
                    if (modbusRTU.IsOpen)
                    {
                        //Create CRC
                        pInfo.SendFrame.CRC = CalculateCRC(pInfo.SendData, Convert.ToUInt16(pInfo.SendData.Length - 8), 6);
                        byteData = BitConverter.GetBytes((int)pInfo.SendFrame.CRC);
                        pInfo.SendData[pInfo.SendData.Length - 2] = byteData[0];
                        pInfo.SendData[pInfo.SendData.Length - 1] = byteData[1];
                    }
                }
            }
            catch (Exception ex) { }

            return pInfo;
        }

        private ModbusFrameInfo Get_ReadInputRegisters_Data(ModbusFrameInfo pFrame)
        {
            ModbusFrameInfo pInfo = pFrame;

            // SendFrame 설정
            try
            {
                pInfo.SendFrame = new ModbusFrame();

                pInfo.SendFrame.Response = true;

                pInfo.SendFrame.Transaction_Identifier = pFrame.ReceivedFrame.Transaction_Identifier;
                pInfo.SendFrame.Protocol_Identifier = pFrame.ReceivedFrame.Protocol_Identifier;

                pInfo.SendFrame.UnitIdentifier = this.unitIdentifier;
                pInfo.SendFrame.FunctionCode = pFrame.ReceivedFrame.FunctionCode;
                if ((pFrame.ReceivedFrame.Quantity < 1) | (pFrame.ReceivedFrame.Quantity > 0x007D))  //Invalid quantity
                {
                    pInfo.SendFrame.ErrorCode = (byte)(pFrame.ReceivedFrame.FunctionCode + 0x80);
                    pInfo.SendFrame.ExceptionCode = 3;
                }
                if (((pFrame.ReceivedFrame.StartAddress + _isStartZeroNum + 1 + pFrame.ReceivedFrame.Quantity) > _maxAddressCnt) | (pFrame.ReceivedFrame.StartAddress < 0))   //Invalid Starting adress or Starting address + quantity
                {
                    pInfo.SendFrame.ErrorCode = (byte)(pFrame.ReceivedFrame.FunctionCode + 0x80);
                    pInfo.SendFrame.ExceptionCode = 2;
                }
                if (pInfo.SendFrame.ExceptionCode == 0)
                {
                    pInfo.SendFrame.ByteCount = (byte)(2 * pFrame.ReceivedFrame.Quantity);
                    pInfo.SendFrame.SendRegisterValues = new Int16[pFrame.ReceivedFrame.Quantity];
                    Buffer.BlockCopy(ModbusData.InputRegisters.Instance.Content, (pFrame.ReceivedFrame.StartAddress + _isStartZeroNum) * 2 + 2, pInfo.SendFrame.SendRegisterValues, 0, pFrame.ReceivedFrame.Quantity * 2);
                }
                if (pInfo.SendFrame.ExceptionCode > 0)
                    pInfo.SendFrame.Protocol_Length = 0x03;
                else
                    pInfo.SendFrame.Protocol_Length = (ushort)(0x03 + pInfo.SendFrame.ByteCount);

            }
            catch (Exception) { }

            // SendData 설정
            try
            {
                if (pInfo.SendFrame.ExceptionCode > 0)
                    pInfo.SendData = new byte[9 + 2 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                else
                    pInfo.SendData = new byte[9 + pInfo.SendFrame.ByteCount + 2 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                Byte[] byteData = new byte[2];
                pInfo.SendFrame.Protocol_Length = (byte)(pInfo.SendData.Length - 6);

                //Send Transaction identifier
                byteData = BitConverter.GetBytes((int)pInfo.SendFrame.Transaction_Identifier);
                pInfo.SendData[0] = byteData[1];
                pInfo.SendData[1] = byteData[0];

                //Send Protocol identifier
                byteData = BitConverter.GetBytes((int)pInfo.SendFrame.Protocol_Identifier);
                pInfo.SendData[2] = byteData[1];
                pInfo.SendData[3] = byteData[0];

                //Send length
                byteData = BitConverter.GetBytes((int)pInfo.SendFrame.Protocol_Length);
                pInfo.SendData[4] = byteData[1];
                pInfo.SendData[5] = byteData[0];

                //Unit Identifier
                pInfo.SendData[6] = pInfo.SendFrame.UnitIdentifier;

                //Function Code
                pInfo.SendData[7] = pInfo.SendFrame.FunctionCode;

                //ByteCount
                pInfo.SendData[8] = pInfo.SendFrame.ByteCount;

                if (pInfo.SendFrame.ExceptionCode > 0)
                {
                    pInfo.SendData[7] = pInfo.SendFrame.ErrorCode;
                    pInfo.SendData[8] = pInfo.SendFrame.ExceptionCode;
                    pInfo.SendFrame.SendRegisterValues = null;
                }

                if (pInfo.SendFrame.SendRegisterValues != null)
                {
                    for (int i = 0; i < (pInfo.SendFrame.ByteCount / 2); i++)
                    {
                        byteData = BitConverter.GetBytes((Int16)pInfo.SendFrame.SendRegisterValues[i]);
                        pInfo.SendData[9 + i * 2] = byteData[1];
                        pInfo.SendData[10 + i * 2] = byteData[0];
                    }
                }

                if (pInfo.ModbusCommunicationInfo.CommunicationType == ModbusType.ModbusRTU)
                {
                    if (modbusRTU.IsOpen)
                    {
                        //Create CRC
                        pInfo.SendFrame.CRC = CalculateCRC(pInfo.SendData, Convert.ToUInt16(pInfo.SendData.Length - 8), 6);
                        byteData = BitConverter.GetBytes((int)pInfo.SendFrame.CRC);
                        pInfo.SendData[pInfo.SendData.Length - 2] = byteData[0];
                        pInfo.SendData[pInfo.SendData.Length - 1] = byteData[1];
                    }
                }
            }
            catch (Exception) { }

            return pInfo;
        }

        private ModbusFrameInfo Get_WriteSingleCoil_Data(ModbusFrameInfo pFrame)
        {
            ModbusFrameInfo pInfo = pFrame;

            // SendFrame 설정
            try
            {
                pInfo.SendFrame = new ModbusFrame();

                pInfo.SendFrame.Response = true;

                pInfo.SendFrame.Transaction_Identifier = pFrame.ReceivedFrame.Transaction_Identifier;
                pInfo.SendFrame.Protocol_Identifier = pFrame.ReceivedFrame.Protocol_Identifier;

                pInfo.SendFrame.UnitIdentifier = this.unitIdentifier;
                pInfo.SendFrame.FunctionCode = pFrame.ReceivedFrame.FunctionCode;
                pInfo.SendFrame.StartAddress = pFrame.ReceivedFrame.StartAddress;
                pInfo.SendFrame.ReceiveCoilValues = pFrame.ReceivedFrame.ReceiveCoilValues;
                if ((pFrame.ReceivedFrame.ReceiveCoilValues[0] != 0x0000) & (pFrame.ReceivedFrame.ReceiveCoilValues[0] != 0xFF00))  //Invalid Value
                {
                    pInfo.SendFrame.ErrorCode = (byte)(pFrame.ReceivedFrame.FunctionCode + 0x80);
                    pInfo.SendFrame.ExceptionCode = 3;
                }
                if (((pFrame.ReceivedFrame.StartAddress + _isStartZeroNum + 1) > _maxAddressCnt) | (pFrame.ReceivedFrame.StartAddress < 0))    //Invalid Starting adress or Starting address + quantity
                {
                    pInfo.SendFrame.ErrorCode = (byte)(pFrame.ReceivedFrame.FunctionCode + 0x80);
                    pInfo.SendFrame.ExceptionCode = 2;
                }
                if (pInfo.SendFrame.ExceptionCode == 0)
                {
                    if (pFrame.ReceivedFrame.ReceiveCoilValues[0] == 0xFF00)
                    {
                        lock (lockCoils)
                            ModbusData.Coils.Instance.Content[pFrame.ReceivedFrame.StartAddress + _isStartZeroNum + 1] = true;
                    }
                    if (pFrame.ReceivedFrame.ReceiveCoilValues[0] == 0x0000)
                    {
                        lock (lockCoils)
                            ModbusData.Coils.Instance.Content[pFrame.ReceivedFrame.StartAddress + _isStartZeroNum + 1] = false;
                    }
                }
                if (pInfo.SendFrame.ExceptionCode > 0)
                    pInfo.SendFrame.Protocol_Length = 0x03;
                else
                    pInfo.SendFrame.Protocol_Length = 0x06;

            }
            catch (Exception) { }

            // SendData 설정
            try
            {
                if (pInfo.SendFrame.ExceptionCode > 0)
                    pInfo.SendData = new byte[9 + 2 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                else
                    pInfo.SendData = new byte[12 + 2 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];

                Byte[] byteData = new byte[2];
                pInfo.SendFrame.Protocol_Length = (byte)(pInfo.SendData.Length - 6);

                //Send Transaction identifier
                byteData = BitConverter.GetBytes((int)pInfo.SendFrame.Transaction_Identifier);
                pInfo.SendData[0] = byteData[1];
                pInfo.SendData[1] = byteData[0];

                //Send Protocol identifier
                byteData = BitConverter.GetBytes((int)pInfo.SendFrame.Protocol_Identifier);
                pInfo.SendData[2] = byteData[1];
                pInfo.SendData[3] = byteData[0];

                //Send length
                byteData = BitConverter.GetBytes((int)pInfo.SendFrame.Protocol_Length);
                pInfo.SendData[4] = byteData[1];
                pInfo.SendData[5] = byteData[0];

                //Unit Identifier
                pInfo.SendData[6] = pInfo.SendFrame.UnitIdentifier;

                //Function Code
                pInfo.SendData[7] = pInfo.SendFrame.FunctionCode;

                if (pInfo.SendFrame.ExceptionCode > 0)
                {
                    pInfo.SendData[7] = pInfo.SendFrame.ErrorCode;
                    pInfo.SendData[8] = pInfo.SendFrame.ExceptionCode;
                    pInfo.SendFrame.SendRegisterValues = null;
                }
                else
                {
                    byteData = BitConverter.GetBytes((int)pFrame.ReceivedFrame.StartAddress);
                    pInfo.SendData[8] = byteData[1];
                    pInfo.SendData[9] = byteData[0];
                    byteData = BitConverter.GetBytes((int)pFrame.ReceivedFrame.ReceiveCoilValues[0]);
                    pInfo.SendData[10] = byteData[1];
                    pInfo.SendData[11] = byteData[0];
                }

                if (pInfo.ModbusCommunicationInfo.CommunicationType == ModbusType.ModbusRTU)
                {
                    if (modbusRTU.IsOpen)
                    {
                        //Create CRC
                        pInfo.SendFrame.CRC = CalculateCRC(pInfo.SendData, Convert.ToUInt16(pInfo.SendData.Length - 8), 6);
                        byteData = BitConverter.GetBytes((int)pInfo.SendFrame.CRC);
                        pInfo.SendData[pInfo.SendData.Length - 2] = byteData[0];
                        pInfo.SendData[pInfo.SendData.Length - 1] = byteData[1];
                    }
                }
            }
            catch (Exception) { }

            if (CoilsChanged != null)
                CoilsChanged(pFrame.ReceivedFrame.StartAddress + _isStartZeroNum + 1, 1);

            return pInfo;
        }

        private ModbusFrameInfo Get_WriteSingleRegister_Data(ModbusFrameInfo pFrame)
        {
            ModbusFrameInfo pInfo = pFrame;

            // SendFrame 설정
            try
            {
                pInfo.SendFrame = new ModbusFrame();

                pInfo.SendFrame.Response = true;

                pInfo.SendFrame.Transaction_Identifier = pFrame.ReceivedFrame.Transaction_Identifier;
                pInfo.SendFrame.Protocol_Identifier = pFrame.ReceivedFrame.Protocol_Identifier;

                pInfo.SendFrame.UnitIdentifier = this.unitIdentifier;
                pInfo.SendFrame.FunctionCode = pFrame.ReceivedFrame.FunctionCode;
                pInfo.SendFrame.StartAddress = pFrame.ReceivedFrame.StartAddress;
                pInfo.SendFrame.ReceiveRegisterValues = pFrame.ReceivedFrame.ReceiveRegisterValues;

                if ((pFrame.ReceivedFrame.ReceiveRegisterValues[0] < 0x0000) | (pFrame.ReceivedFrame.ReceiveRegisterValues[0] > 0xFFFF))  //Invalid Value
                {
                    pInfo.SendFrame.ErrorCode = (byte)(pFrame.ReceivedFrame.FunctionCode + 0x80);
                    pInfo.SendFrame.ExceptionCode = 3;
                }
                if (((pFrame.ReceivedFrame.StartAddress + 1 + _isStartZeroNum) > _maxAddressCnt) | (pFrame.ReceivedFrame.StartAddress < 0))    //Invalid Starting adress or Starting address + quantity
                {
                    pInfo.SendFrame.ErrorCode = (byte)(pFrame.ReceivedFrame.FunctionCode + 0x80);
                    pInfo.SendFrame.ExceptionCode = 2;
                }
                if (pInfo.SendFrame.ExceptionCode == 0)
                {
                    lock (lockHoldingRegisters)
                        ModbusData.HoldingRegisters.Instance.Content[pFrame.ReceivedFrame.StartAddress + 1 + _isStartZeroNum] = unchecked((short)pFrame.ReceivedFrame.ReceiveRegisterValues[0]);
                }
                if (pInfo.SendFrame.ExceptionCode > 0)
                    pInfo.SendFrame.Protocol_Length = 0x03;
                else
                    pInfo.SendFrame.Protocol_Length = 0x06;

            }
            catch (Exception) { }
            try
            {
                if (pInfo.SendFrame.ExceptionCode > 0)
                    pInfo.SendData = new byte[9 + 2 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                else
                    pInfo.SendData = new byte[12 + 2 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];

                Byte[] byteData = new byte[2];
                pInfo.SendFrame.Protocol_Length = (byte)(pInfo.SendData.Length - 6);

                //Send Transaction identifier
                byteData = BitConverter.GetBytes((int)pInfo.SendFrame.Transaction_Identifier);
                pInfo.SendData[0] = byteData[1];
                pInfo.SendData[1] = byteData[0];

                //Send Protocol identifier
                byteData = BitConverter.GetBytes((int)pInfo.SendFrame.Protocol_Identifier);
                pInfo.SendData[2] = byteData[1];
                pInfo.SendData[3] = byteData[0];

                //Send length
                byteData = BitConverter.GetBytes((int)pInfo.SendFrame.Protocol_Length);
                pInfo.SendData[4] = byteData[1];
                pInfo.SendData[5] = byteData[0];

                //Unit Identifier
                pInfo.SendData[6] = pInfo.SendFrame.UnitIdentifier;

                //Function Code
                pInfo.SendData[7] = pInfo.SendFrame.FunctionCode;

                if (pInfo.SendFrame.ExceptionCode > 0)
                {
                    pInfo.SendData[7] = pInfo.SendFrame.ErrorCode;
                    pInfo.SendData[8] = pInfo.SendFrame.ExceptionCode;
                    pInfo.SendFrame.SendRegisterValues = null;
                }
                else
                {
                    byteData = BitConverter.GetBytes((int)pFrame.ReceivedFrame.StartAddress);
                    pInfo.SendData[8] = byteData[1];
                    pInfo.SendData[9] = byteData[0];
                    byteData = BitConverter.GetBytes((int)pFrame.ReceivedFrame.ReceiveRegisterValues[0]);
                    pInfo.SendData[10] = byteData[1];
                    pInfo.SendData[11] = byteData[0];
                }

                if (pInfo.ModbusCommunicationInfo.CommunicationType == ModbusType.ModbusRTU)
                {
                    if (modbusRTU.IsOpen)
                    {
                        //Create CRC
                        pInfo.SendFrame.CRC = CalculateCRC(pInfo.SendData, Convert.ToUInt16(pInfo.SendData.Length - 8), 6);
                        byteData = BitConverter.GetBytes((int)pInfo.SendFrame.CRC);
                        pInfo.SendData[pInfo.SendData.Length - 2] = byteData[0];
                        pInfo.SendData[pInfo.SendData.Length - 1] = byteData[1];
                    }
                }
            }
            catch (Exception) { }

            if (HoldingRegistersChanged != null)
                HoldingRegistersChanged(pFrame.ReceivedFrame.StartAddress + 1 + _isStartZeroNum, 1);

            return pInfo;
        }

        private ModbusFrameInfo Get_WriteMultipleCoils_Data(ModbusFrameInfo pFrame)
        {
            ModbusFrameInfo pInfo = pFrame;

            // SendFrame 설정
            try
            {
                pInfo.SendFrame = new ModbusFrame();

                pInfo.SendFrame.Response = true;

                pInfo.SendFrame.Transaction_Identifier = pFrame.ReceivedFrame.Transaction_Identifier;
                pInfo.SendFrame.Protocol_Identifier = pFrame.ReceivedFrame.Protocol_Identifier;

                pInfo.SendFrame.UnitIdentifier = this.unitIdentifier;
                pInfo.SendFrame.FunctionCode = pFrame.ReceivedFrame.FunctionCode;
                pInfo.SendFrame.StartAddress = pFrame.ReceivedFrame.StartAddress;
                pInfo.SendFrame.Quantity = pFrame.ReceivedFrame.Quantity;

                if ((pFrame.ReceivedFrame.Quantity == 0x0000) | (pFrame.ReceivedFrame.Quantity > 0x07B0))  //Invalid Quantity
                {
                    pInfo.SendFrame.ErrorCode = (byte)(pFrame.ReceivedFrame.FunctionCode + 0x80);
                    pInfo.SendFrame.ExceptionCode = 3;
                }
                if ((((int)pFrame.ReceivedFrame.StartAddress + _isStartZeroNum + 1 + (int)pFrame.ReceivedFrame.Quantity) > _maxAddressCnt) | (pFrame.ReceivedFrame.StartAddress < 0))    //Invalid Starting adress or Starting address + quantity
                {
                    pInfo.SendFrame.ErrorCode = (byte)(pFrame.ReceivedFrame.FunctionCode + 0x80);
                    pInfo.SendFrame.ExceptionCode = 2;
                }
                if (pInfo.SendFrame.ExceptionCode == 0)
                {
                    lock (lockCoils)
                    {
                        for (int i = 0; i < pFrame.ReceivedFrame.Quantity; i++)
                        {
                            int shift = i % 16;
                            /*                if ((i == receiveData.quantity - 1) & (receiveData.quantity % 2 != 0))
                                            {
                                                if (shift < 8)
                                                    shift = shift + 8;
                                                else
                                                    shift = shift - 8;
                                            }*/
                            int mask = 0x1;
                            mask = mask << (shift);
                            if ((pFrame.ReceivedFrame.ReceiveCoilValues[i / 16] & (ushort)mask) == 0)
                                ModbusData.Coils.Instance.Content[pFrame.ReceivedFrame.StartAddress + _isStartZeroNum + i + 1] = false;
                            else
                                ModbusData.Coils.Instance.Content[pFrame.ReceivedFrame.StartAddress + _isStartZeroNum + i + 1] = true;
                        }
                    }
                }
                if (pInfo.SendFrame.ExceptionCode > 0)
                    pInfo.SendFrame.Protocol_Length = 0x03;
                else
                    pInfo.SendFrame.Protocol_Length = 0x06;
            }
            catch (Exception) { }

            // SendData 설정
            try
            {
                if (pInfo.SendFrame.ExceptionCode > 0)
                    pInfo.SendData = new byte[9 + 2 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                else
                    pInfo.SendData = new byte[12 + 2 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];

                Byte[] byteData = new byte[2];
                pInfo.SendFrame.Protocol_Length = (byte)(pInfo.SendData.Length - 6);

                //Send Transaction identifier
                byteData = BitConverter.GetBytes((int)pInfo.SendFrame.Transaction_Identifier);
                pInfo.SendData[0] = byteData[1];
                pInfo.SendData[1] = byteData[0];

                //Send Protocol identifier
                byteData = BitConverter.GetBytes((int)pInfo.SendFrame.Protocol_Identifier);
                pInfo.SendData[2] = byteData[1];
                pInfo.SendData[3] = byteData[0];

                //Send length
                byteData = BitConverter.GetBytes((int)pInfo.SendFrame.Protocol_Length);
                pInfo.SendData[4] = byteData[1];
                pInfo.SendData[5] = byteData[0];

                //Unit Identifier
                pInfo.SendData[6] = pInfo.SendFrame.UnitIdentifier;

                //Function Code
                pInfo.SendData[7] = pInfo.SendFrame.FunctionCode;

                if (pInfo.SendFrame.ExceptionCode > 0)
                {
                    pInfo.SendData[7] = pInfo.SendFrame.ErrorCode;
                    pInfo.SendData[8] = pInfo.SendFrame.ExceptionCode;
                    pInfo.SendFrame.SendRegisterValues = null;
                }
                else
                {
                    byteData = BitConverter.GetBytes((int)pFrame.ReceivedFrame.StartAddress);
                    pInfo.SendData[8] = byteData[1];
                    pInfo.SendData[9] = byteData[0];
                    byteData = BitConverter.GetBytes((int)pFrame.ReceivedFrame.Quantity);
                    pInfo.SendData[10] = byteData[1];
                    pInfo.SendData[11] = byteData[0];
                }

                if (pInfo.ModbusCommunicationInfo.CommunicationType == ModbusType.ModbusRTU)
                {
                    if (modbusRTU.IsOpen)
                    {
                        //Create CRC
                        pInfo.SendFrame.CRC = CalculateCRC(pInfo.SendData, Convert.ToUInt16(pInfo.SendData.Length - 8), 6);
                        byteData = BitConverter.GetBytes((int)pInfo.SendFrame.CRC);
                        pInfo.SendData[pInfo.SendData.Length - 2] = byteData[0];
                        pInfo.SendData[pInfo.SendData.Length - 1] = byteData[1];
                    }
                }
            }
            catch (Exception) { }

            if (CoilsChanged != null)
                CoilsChanged(pFrame.ReceivedFrame.StartAddress + 1 + _isStartZeroNum, pFrame.ReceivedFrame.Quantity);

            return pInfo;
        }

        private ModbusFrameInfo Get_WriteMultipleRegisters_Data(ModbusFrameInfo pFrame)
        {
            ModbusFrameInfo pInfo = pFrame;

            // SendFrame 설정
            try
            {
                pInfo.SendFrame = new ModbusFrame();

                pInfo.SendFrame.Response = true;

                pInfo.SendFrame.Transaction_Identifier = pFrame.ReceivedFrame.Transaction_Identifier;
                pInfo.SendFrame.Protocol_Identifier = pFrame.ReceivedFrame.Protocol_Identifier;

                pInfo.SendFrame.UnitIdentifier = this.unitIdentifier;
                pInfo.SendFrame.FunctionCode = pFrame.ReceivedFrame.FunctionCode;
                pInfo.SendFrame.StartAddress = pFrame.ReceivedFrame.StartAddress;
                pInfo.SendFrame.Quantity = pFrame.ReceivedFrame.Quantity;

                if ((pFrame.ReceivedFrame.Quantity == 0x0000) | (pFrame.ReceivedFrame.Quantity > 0x07B0))  //Invalid Quantity
                {
                    pInfo.SendFrame.ErrorCode = (byte)(pFrame.ReceivedFrame.FunctionCode + 0x80);
                    pInfo.SendFrame.ExceptionCode = 3;
                }
                if ((((int)pFrame.ReceivedFrame.StartAddress + _isStartZeroNum + 1 + (int)pFrame.ReceivedFrame.Quantity) > _maxAddressCnt) | (pFrame.ReceivedFrame.StartAddress < 0))   //Invalid Starting adress or Starting address + quantity
                {
                    pInfo.SendFrame.ErrorCode = (byte)(pFrame.ReceivedFrame.FunctionCode + 0x80);
                    pInfo.SendFrame.ExceptionCode = 2;
                }
                if (pInfo.SendFrame.ExceptionCode == 0)
                {
                    lock (lockHoldingRegisters)
                    {
                        for (int i = 0; i < pFrame.ReceivedFrame.Quantity; i++)
                        {
                            ModbusData.HoldingRegisters.Instance.Content[pFrame.ReceivedFrame.StartAddress + _isStartZeroNum + i + 1] = unchecked((short)pFrame.ReceivedFrame.ReceiveRegisterValues[i]);
                        }
                    }
                }
                if (pInfo.SendFrame.ExceptionCode > 0)
                    pInfo.SendFrame.Protocol_Length = 0x03;
                else
                    pInfo.SendFrame.Protocol_Length = 0x06;
            }
            catch (Exception) { }

            // SendData 설정
            try
            {
                if (pInfo.SendFrame.ExceptionCode > 0)
                    pInfo.SendData = new byte[9 + 2 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                else
                    pInfo.SendData = new byte[12 + 2 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];

                Byte[] byteData = new byte[2];
                pInfo.SendFrame.Protocol_Length = (byte)(pInfo.SendData.Length - 6);

                //Send Transaction identifier
                byteData = BitConverter.GetBytes((int)pInfo.SendFrame.Transaction_Identifier);
                pInfo.SendData[0] = byteData[1];
                pInfo.SendData[1] = byteData[0];

                //Send Protocol identifier
                byteData = BitConverter.GetBytes((int)pInfo.SendFrame.Protocol_Identifier);
                pInfo.SendData[2] = byteData[1];
                pInfo.SendData[3] = byteData[0];

                //Send length
                byteData = BitConverter.GetBytes((int)pInfo.SendFrame.Protocol_Length);
                pInfo.SendData[4] = byteData[1];
                pInfo.SendData[5] = byteData[0];

                //Unit Identifier
                pInfo.SendData[6] = pInfo.SendFrame.UnitIdentifier;

                //Function Code
                pInfo.SendData[7] = pInfo.SendFrame.FunctionCode;

                if (pInfo.SendFrame.ExceptionCode > 0)
                {
                    pInfo.SendData[7] = pInfo.SendFrame.ErrorCode;
                    pInfo.SendData[8] = pInfo.SendFrame.ExceptionCode;
                    pInfo.SendFrame.SendRegisterValues = null;
                }
                else
                {
                    byteData = BitConverter.GetBytes((int)pFrame.ReceivedFrame.StartAddress);
                    pInfo.SendData[8] = byteData[1];
                    pInfo.SendData[9] = byteData[0];
                    byteData = BitConverter.GetBytes((int)pFrame.ReceivedFrame.Quantity);
                    pInfo.SendData[10] = byteData[1];
                    pInfo.SendData[11] = byteData[0];
                }

                if (pInfo.ModbusCommunicationInfo.CommunicationType == ModbusType.ModbusRTU)
                {
                    if (modbusRTU.IsOpen)
                    {
                        //Create CRC
                        pInfo.SendFrame.CRC = CalculateCRC(pInfo.SendData, Convert.ToUInt16(pInfo.SendData.Length - 8), 6);
                        byteData = BitConverter.GetBytes((int)pInfo.SendFrame.CRC);
                        pInfo.SendData[pInfo.SendData.Length - 2] = byteData[0];
                        pInfo.SendData[pInfo.SendData.Length - 1] = byteData[1];
                    }
                }
            }
            catch (Exception) { }

            if (HoldingRegistersChanged != null)
                HoldingRegistersChanged(pFrame.ReceivedFrame.StartAddress + _isStartZeroNum + 1, pFrame.ReceivedFrame.Quantity);

            return pInfo;
        }

        private ModbusFrameInfo Get_ReadWriteMultipleRegisters_Data(ModbusFrameInfo pFrame)
        {
            ModbusFrameInfo pInfo = pFrame;

            // SendFrame 설정
            try
            {
                pInfo.SendFrame = new ModbusFrame();

                pInfo.SendFrame.Response = true;

                pInfo.SendFrame.Transaction_Identifier = pFrame.ReceivedFrame.Transaction_Identifier;
                pInfo.SendFrame.Protocol_Identifier = pFrame.ReceivedFrame.Protocol_Identifier;

                pInfo.SendFrame.UnitIdentifier = this.unitIdentifier;
                pInfo.SendFrame.FunctionCode = pFrame.ReceivedFrame.FunctionCode;

                if ((pFrame.ReceivedFrame.Read_Qty < 0x0001) | (pFrame.ReceivedFrame.Read_Qty > 0x007D) | (pFrame.ReceivedFrame.Write_Qty < 0x0001) | (pFrame.ReceivedFrame.Write_Qty > 0x0079) | (pFrame.ReceivedFrame.ByteCount != (pFrame.ReceivedFrame.Write_Qty * 2)))  //Invalid Quantity
                {
                    pInfo.SendFrame.ErrorCode = (byte)(pFrame.ReceivedFrame.FunctionCode + 0x80);
                    pInfo.SendFrame.ExceptionCode = 3;
                }
                if ((((int)pFrame.ReceivedFrame.Read_StartAddress + _isStartZeroNum + 1 + (int)pFrame.ReceivedFrame.Read_Qty) > _maxAddressCnt) | (((int)pFrame.ReceivedFrame.Write_StartAddress + _isStartZeroNum + 1 + (int)pFrame.ReceivedFrame.Write_Qty) > _maxAddressCnt) | (pFrame.ReceivedFrame.Write_Qty < 0) | (pFrame.ReceivedFrame.Read_Qty < 0))    //Invalid Starting adress or Starting address + quantity
                {
                    pInfo.SendFrame.ErrorCode = (byte)(pFrame.ReceivedFrame.FunctionCode + 0x80);
                    pInfo.SendFrame.ExceptionCode = 2;
                }
                if (pInfo.SendFrame.ExceptionCode == 0)
                {
                    pInfo.SendFrame.SendRegisterValues = new Int16[pFrame.ReceivedFrame.Read_Qty];
                    lock (lockHoldingRegisters)
                        Buffer.BlockCopy(ModbusData.HoldingRegisters.Instance.Content, (pFrame.ReceivedFrame.Read_StartAddress + _isStartZeroNum) * 2 + 2, pInfo.SendFrame.SendRegisterValues, 0, pFrame.ReceivedFrame.Read_Qty * 2);

                    lock (ModbusData.HoldingRegisters.Instance.Content)
                        for (int i = 0; i < pFrame.ReceivedFrame.Write_Qty; i++)
                        {
                            ModbusData.HoldingRegisters.Instance.Content[pFrame.ReceivedFrame.Write_StartAddress + _isStartZeroNum + i + 1] = unchecked((short)pFrame.ReceivedFrame.ReceiveRegisterValues[i]);
                        }
                    pInfo.SendFrame.ByteCount = (byte)(2 * pFrame.ReceivedFrame.Read_Qty);
                }
                if (pInfo.SendFrame.ExceptionCode > 0)
                    pInfo.SendFrame.Protocol_Length = 0x03;
                else
                    pInfo.SendFrame.Protocol_Length = Convert.ToUInt16(3 + 2 * pFrame.ReceivedFrame.Read_Qty);
            }
            catch (Exception) { }

            try
            {
                if (pInfo.SendFrame.ExceptionCode > 0)
                    pInfo.SendData = new byte[9 + 2 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                else
                    pInfo.SendData = new byte[9 + pInfo.SendFrame.ByteCount + 2 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];

                Byte[] byteData = new byte[2];

                //Send Transaction identifier
                byteData = BitConverter.GetBytes((int)pInfo.SendFrame.Transaction_Identifier);
                pInfo.SendData[0] = byteData[1];
                pInfo.SendData[1] = byteData[0];

                //Send Protocol identifier
                byteData = BitConverter.GetBytes((int)pInfo.SendFrame.Protocol_Identifier);
                pInfo.SendData[2] = byteData[1];
                pInfo.SendData[3] = byteData[0];

                //Send length
                byteData = BitConverter.GetBytes((int)pInfo.SendFrame.Protocol_Length);
                pInfo.SendData[4] = byteData[1];
                pInfo.SendData[5] = byteData[0];

                //Unit Identifier
                pInfo.SendData[6] = pInfo.SendFrame.UnitIdentifier;

                //Function Code
                pInfo.SendData[7] = pInfo.SendFrame.FunctionCode;

                //ByteCount
                pInfo.SendData[8] = pInfo.SendFrame.ByteCount;

                if (pInfo.SendFrame.ExceptionCode > 0)
                {
                    pInfo.SendData[7] = pInfo.SendFrame.ErrorCode;
                    pInfo.SendData[8] = pInfo.SendFrame.ExceptionCode;
                    pInfo.SendFrame.SendRegisterValues = null;
                }
                else
                {
                    if (pInfo.SendFrame.SendRegisterValues != null)
                    {
                        for (int i = 0; i < (pInfo.SendFrame.ByteCount / 2); i++)
                        {
                            byteData = BitConverter.GetBytes((Int16)pInfo.SendFrame.SendRegisterValues[i]);
                            pInfo.SendData[9 + i * 2] = byteData[1];
                            pInfo.SendData[10 + i * 2] = byteData[0];
                        }
                    }
                }

                if (pInfo.ModbusCommunicationInfo.CommunicationType == ModbusType.ModbusRTU)
                {
                    if (modbusRTU.IsOpen)
                    {
                        //Create CRC
                        pInfo.SendFrame.CRC = CalculateCRC(pInfo.SendData, Convert.ToUInt16(pInfo.SendData.Length - 8), 6);
                        byteData = BitConverter.GetBytes((int)pInfo.SendFrame.CRC);
                        pInfo.SendData[pInfo.SendData.Length - 2] = byteData[0];
                        pInfo.SendData[pInfo.SendData.Length - 1] = byteData[1];
                    }
                }
            }
            catch (Exception) { }

            if (HoldingRegistersChanged != null)
                HoldingRegistersChanged(pFrame.ReceivedFrame.Write_StartAddress + _isStartZeroNum + 1, pFrame.ReceivedFrame.Write_Qty);

            return pInfo;
        }

        private bool SendAnswer(ModbusFrameInfo pFrameInfo)
        {
            bool YN_Send = true;
            switch (pFrameInfo.ModbusFunction)
            {
                case EnModBusFunction.Read_Coils:
                    YN_Send = !FunctionCode1Disabled;
                    break;
                case EnModBusFunction.Read_Discrete_Inputs:
                    YN_Send = !FunctionCode2Disabled;
                    break;
                case EnModBusFunction.Read_Holding_Registers:
                    YN_Send = !FunctionCode3Disabled;
                    break;
                case EnModBusFunction.Read_Input_Registers:
                    YN_Send = !FunctionCode4Disabled;
                    break;
                case EnModBusFunction.Write_Single_Coil:
                    YN_Send = !FunctionCode5Disabled;
                    break;
                case EnModBusFunction.Write_Single_Register:
                    YN_Send = !FunctionCode6Disabled;
                    break;
                case EnModBusFunction.Write_Multiple_Coils:
                    YN_Send = !FunctionCode15Disabled;
                    break;
                case EnModBusFunction.Write_Multiple_Register:
                    YN_Send = !FunctionCode16Disabled;
                    break;
                case EnModBusFunction.ReadWrite_Multiple_Registers:
                    YN_Send = !FunctionCode23Disabled;
                    break;
                default:
                    YN_Send = false;
                    break;
            }
            if (YN_Send)
                SendResponse(pFrameInfo);
            else
                SendException(pFrameInfo);
            return YN_Send;
        }

        private void SendResponse(ModbusFrameInfo pFrameInfo)
        {
            try
            {
                switch (pFrameInfo.ModbusFunction)
                {
                    case EnModBusFunction.Read_Coils:
                    case EnModBusFunction.Read_Discrete_Inputs:
                    case EnModBusFunction.Read_Holding_Registers:
                    case EnModBusFunction.Read_Input_Registers:
                    case EnModBusFunction.Write_Single_Coil:
                    case EnModBusFunction.Write_Single_Register:
                    case EnModBusFunction.Write_Multiple_Coils:
                    case EnModBusFunction.Write_Multiple_Register:
                    case EnModBusFunction.ReadWrite_Multiple_Registers:
                        if (pFrameInfo.ModbusCommunicationInfo.CommunicationType == ModbusType.ModbusRTU)
                        {
                            if (modbusRTU.IsOpen)
                            {
                                modbusRTU.Write(pFrameInfo.SendData, 6, pFrameInfo.SendData.Length - 6);

                                byte[] debugData = new byte[pFrameInfo.SendData.Length - 6];
                                Array.Copy(pFrameInfo.SendData, 6, debugData, 0, pFrameInfo.SendData.Length - 6);
                                log.DebugFormat("Send Data (Serial) : {0}", BitConverter.ToString(debugData));
                            }
                        }
                        else if (pFrameInfo.ModbusCommunicationInfo.CommunicationType == ModbusType.ModbusUDP)
                        {
                            IPEndPoint endPoint = new IPEndPoint(pFrameInfo.ModbusCommunicationInfo.IPAddress, pFrameInfo.ModbusCommunicationInfo.Port);
                            log.DebugFormat("Send Data (UDP) : {0}", BitConverter.ToString(pFrameInfo.SendData));
                            if (pFrameInfo.ModbusFunction == EnModBusFunction.Read_Coils) log.DebugFormat("Send Data: {0}", BitConverter.ToString(pFrameInfo.SendData));
                            modbusUDP.Send(pFrameInfo.SendData, pFrameInfo.SendData.Length, endPoint);
                        }
                        else if (pFrameInfo.ModbusCommunicationInfo.CommunicationType == ModbusType.ModbusTCP)
                        {
                            pFrameInfo.ModbusCommunicationInfo.Stream.Write(pFrameInfo.SendData, 0, pFrameInfo.SendData.Length);
                            log.DebugFormat("Send Data (TCP) : {0}", BitConverter.ToString(pFrameInfo.SendData));
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception) { }
        }

        private void SendException(ModbusFrameInfo pFrameInfo)
        {
            pFrameInfo.SendFrame.Response = true;

            pFrameInfo.SendFrame.Transaction_Identifier = pFrameInfo.ReceivedFrame.Transaction_Identifier;
            pFrameInfo.SendFrame.Protocol_Identifier = pFrameInfo.ReceivedFrame.Protocol_Identifier;

            pFrameInfo.SendFrame.UnitIdentifier = pFrameInfo.ReceivedFrame.UnitIdentifier;
            pFrameInfo.SendFrame.ErrorCode = (byte)pFrameInfo.SendFrame.ErrorCode;
            pFrameInfo.SendFrame.ExceptionCode = (byte)pFrameInfo.SendFrame.ExceptionCode;

            if (pFrameInfo.SendFrame.ExceptionCode > 0)
                pFrameInfo.SendFrame.Protocol_Length = 0x03;
            else
                pFrameInfo.SendFrame.Protocol_Length = (ushort)(0x03 + pFrameInfo.SendFrame.ByteCount);

            if (true)
            {
                if (pFrameInfo.SendFrame.ExceptionCode > 0)
                    pFrameInfo.SendData = new byte[9 + 2 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                else
                    pFrameInfo.SendData = new byte[9 + pFrameInfo.SendFrame.ByteCount + 2 * Convert.ToInt32(ConnectionType == ModbusType.ModbusRTU)];
                Byte[] byteData = new byte[2];
                pFrameInfo.SendFrame.Protocol_Length = (byte)(pFrameInfo.SendData.Length - 6);

                //Send Transaction identifier
                byteData = BitConverter.GetBytes((int)pFrameInfo.SendFrame.Transaction_Identifier);
                pFrameInfo.SendData[0] = byteData[1];
                pFrameInfo.SendData[1] = byteData[0];

                //Send Protocol identifier
                byteData = BitConverter.GetBytes((int)pFrameInfo.SendFrame.Protocol_Identifier);
                pFrameInfo.SendData[2] = byteData[1];
                pFrameInfo.SendData[3] = byteData[0];

                //Send length
                byteData = BitConverter.GetBytes((int)pFrameInfo.SendFrame.Protocol_Length);
                pFrameInfo.SendData[4] = byteData[1];
                pFrameInfo.SendData[5] = byteData[0];

                //Unit Identifier
                pFrameInfo.SendData[6] = pFrameInfo.SendFrame.UnitIdentifier;
                pFrameInfo.SendData[7] = pFrameInfo.SendFrame.ErrorCode;
                pFrameInfo.SendData[8] = pFrameInfo.SendFrame.ExceptionCode;

                try
                {
                    if (pFrameInfo.ModbusCommunicationInfo.CommunicationType == ModbusType.ModbusRTU)
                    {
                        if (modbusRTU.IsOpen)
                        {
                            //Create CRC
                            pFrameInfo.SendFrame.CRC = CalculateCRC(pFrameInfo.SendData, Convert.ToUInt16(pFrameInfo.SendData.Length - 8), 6);
                            byteData = BitConverter.GetBytes((int)pFrameInfo.SendFrame.CRC);
                            pFrameInfo.SendData[pFrameInfo.SendData.Length - 2] = byteData[0];
                            pFrameInfo.SendData[pFrameInfo.SendData.Length - 1] = byteData[1];
                            modbusRTU.Write(pFrameInfo.SendData, 6, pFrameInfo.SendData.Length - 6);

                            byte[] debugData = new byte[pFrameInfo.SendData.Length - 6];
                            Array.Copy(pFrameInfo.SendData, 6, debugData, 0, pFrameInfo.SendData.Length - 6);
                            log.DebugFormat("Send Data (Serial) : {0}", BitConverter.ToString(debugData));
                        }
                    }
                    else if (pFrameInfo.ModbusCommunicationInfo.CommunicationType == ModbusType.ModbusUDP)
                    {
                        IPEndPoint endPoint = new IPEndPoint(pFrameInfo.ModbusCommunicationInfo.IPAddress, pFrameInfo.ModbusCommunicationInfo.Port);
                        modbusUDP.Send(pFrameInfo.SendData, pFrameInfo.SendData.Length, endPoint);
                    }
                    else if (pFrameInfo.ModbusCommunicationInfo.CommunicationType == ModbusType.ModbusTCP)
                    {
                        pFrameInfo.ModbusCommunicationInfo.Stream.Write(pFrameInfo.SendData, 0, pFrameInfo.SendData.Length);
                        log.DebugFormat("Send Data (TCP) : {0}", BitConverter.ToString(pFrameInfo.SendData));
                    }
                }
                catch (Exception) { }
            }
        }

        #endregion

        private void CreateLogData(ModbusFrame receiveFrame, ModbusFrame sendFrame)
        {
            for (int i = 0; i < 98; i++)
            {
                modbusLogData[99 - i] = modbusLogData[99 - i - 2];

            }
            modbusLogData[0] = receiveFrame;
            modbusLogData[1] = sendFrame;
        }

        public int NumberOfConnections
        {
            get
            {
                return numberOfConnections;
            }
        }

        public ModbusFrame[] ModbusLogData
        {
            get
            {
                return modbusLogData;
            }
        }

        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        public int Baudrate
        {
            get
            {
                return baudrate;
            }
            set
            {
                baudrate = value;
            }
        }

        public System.IO.Ports.Parity Parity
        {
            get
            {
                return parity;
            }
            set
            {
                parity = value;
            }
        }

        public System.IO.Ports.StopBits StopBits
        {
            get
            {
                return stopBits;
            }
            set
            {
                stopBits = value;
            }
        }

        public string SerialPort
        {
            get
            {
                return serialPort;
            }
            set
            {
                serialPort = value;
            }
        }

        public byte UnitIdentifier
        {
            get { return unitIdentifier; }
            set { unitIdentifier = value; }
        }

        public static UInt16 CalculateCRC(byte[] data, UInt16 numberOfBytes, int startByte)
        {
            byte[] auchCRCHi = {
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81,
            0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
            0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01,
            0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81,
            0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01,
            0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81,
            0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
            0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01,
            0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81,
            0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01,
            0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81,
            0x40
            };

            byte[] auchCRCLo = {
            0x00, 0xC0, 0xC1, 0x01, 0xC3, 0x03, 0x02, 0xC2, 0xC6, 0x06, 0x07, 0xC7, 0x05, 0xC5, 0xC4,
            0x04, 0xCC, 0x0C, 0x0D, 0xCD, 0x0F, 0xCF, 0xCE, 0x0E, 0x0A, 0xCA, 0xCB, 0x0B, 0xC9, 0x09,
            0x08, 0xC8, 0xD8, 0x18, 0x19, 0xD9, 0x1B, 0xDB, 0xDA, 0x1A, 0x1E, 0xDE, 0xDF, 0x1F, 0xDD,
            0x1D, 0x1C, 0xDC, 0x14, 0xD4, 0xD5, 0x15, 0xD7, 0x17, 0x16, 0xD6, 0xD2, 0x12, 0x13, 0xD3,
            0x11, 0xD1, 0xD0, 0x10, 0xF0, 0x30, 0x31, 0xF1, 0x33, 0xF3, 0xF2, 0x32, 0x36, 0xF6, 0xF7,
            0x37, 0xF5, 0x35, 0x34, 0xF4, 0x3C, 0xFC, 0xFD, 0x3D, 0xFF, 0x3F, 0x3E, 0xFE, 0xFA, 0x3A,
            0x3B, 0xFB, 0x39, 0xF9, 0xF8, 0x38, 0x28, 0xE8, 0xE9, 0x29, 0xEB, 0x2B, 0x2A, 0xEA, 0xEE,
            0x2E, 0x2F, 0xEF, 0x2D, 0xED, 0xEC, 0x2C, 0xE4, 0x24, 0x25, 0xE5, 0x27, 0xE7, 0xE6, 0x26,
            0x22, 0xE2, 0xE3, 0x23, 0xE1, 0x21, 0x20, 0xE0, 0xA0, 0x60, 0x61, 0xA1, 0x63, 0xA3, 0xA2,
            0x62, 0x66, 0xA6, 0xA7, 0x67, 0xA5, 0x65, 0x64, 0xA4, 0x6C, 0xAC, 0xAD, 0x6D, 0xAF, 0x6F,
            0x6E, 0xAE, 0xAA, 0x6A, 0x6B, 0xAB, 0x69, 0xA9, 0xA8, 0x68, 0x78, 0xB8, 0xB9, 0x79, 0xBB,
            0x7B, 0x7A, 0xBA, 0xBE, 0x7E, 0x7F, 0xBF, 0x7D, 0xBD, 0xBC, 0x7C, 0xB4, 0x74, 0x75, 0xB5,
            0x77, 0xB7, 0xB6, 0x76, 0x72, 0xB2, 0xB3, 0x73, 0xB1, 0x71, 0x70, 0xB0, 0x50, 0x90, 0x91,
            0x51, 0x93, 0x53, 0x52, 0x92, 0x96, 0x56, 0x57, 0x97, 0x55, 0x95, 0x94, 0x54, 0x9C, 0x5C,
            0x5D, 0x9D, 0x5F, 0x9F, 0x9E, 0x5E, 0x5A, 0x9A, 0x9B, 0x5B, 0x99, 0x59, 0x58, 0x98, 0x88,
            0x48, 0x49, 0x89, 0x4B, 0x8B, 0x8A, 0x4A, 0x4E, 0x8E, 0x8F, 0x4F, 0x8D, 0x4D, 0x4C, 0x8C,
            0x44, 0x84, 0x85, 0x45, 0x87, 0x47, 0x46, 0x86, 0x82, 0x42, 0x43, 0x83, 0x41, 0x81, 0x80,
            0x40
            };
            UInt16 usDataLen = numberOfBytes;
            byte uchCRCHi = 0xFF;
            byte uchCRCLo = 0xFF;
            int i = 0;
            int uIndex;
            while (usDataLen > 0)
            {
                usDataLen--;
                if ((i + startByte) < data.Length)
                {
                    uIndex = uchCRCLo ^ data[i + startByte];
                    uchCRCLo = (byte)(uchCRCHi ^ auchCRCHi[uIndex]);
                    uchCRCHi = auchCRCLo[uIndex];
                }
                i++;
            }
            return (UInt16)((UInt16)uchCRCHi << 8 | uchCRCLo);
        }

        public static bool DetectValidModbusFrame(byte[] readBuffer, int length)
        {
            // minimum length 6 bytes
            if (length < 6)
                return false;
            //SlaveID correct
            if ((readBuffer[0] < 1) | (readBuffer[0] > 247))
                return false;
            //CRC correct?
            byte[] crc = new byte[2];
            crc = BitConverter.GetBytes(CalculateCRC(readBuffer, (ushort)(length - 2), 0));
            if (crc[0] != readBuffer[length - 2] | crc[1] != readBuffer[length - 1])
                return false;
            return true;
        }
    }
}
