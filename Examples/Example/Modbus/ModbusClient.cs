using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Functions.TCP;

namespace Functions.Modbus
{
    public class ModbusClient
    {
        private readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const int modbusTransaction = 0;
        private const int defaultLength = 1;
        TCPCommunication tcpHandler;
        private readonly int _maxLength = 2000;
        private readonly int _maxAddress = 65534;
        private int _tcpPort;
        private IPAddress _ipAddress;
        private object _sendAndResponseLock = new object();
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
        public ModbusClient(IPAddress ip, int port)
        {
            _ipAddress = ip;
            _tcpPort = port;
        }
        public ModbusClient(string ip, int port)
            : this()
        {
            _ipAddress = IPAddress.Parse(ip);
            _tcpPort = port;
        }
        protected ModbusClient()
        {
        }

        private class ModbusInfo
        {
            public EnModBusFunction ModbusFunction { get; set; }
            public int Read_StartAddress { get; set; }
            public int Read_Qty { get; set; }
            public int Write_StartAddress { get; set; }
            public int Write_Qty { get; set; }
            public object[] Write_Data { get; set; }

            public ModbusInfo()
            {
                ModbusFunction = EnModBusFunction.ReadWrite_Multiple_Registers;
                Read_StartAddress = 0;
                Read_Qty = 0;
                Write_StartAddress = 0;
                Write_Qty = 0;
                object[] tmp = new object[0];
                Write_Data = tmp;
            }
        }

        public IPAddress IP_Address
        {
            get { return _ipAddress; }
            set
            {
                _ipAddress = value;
            }
        }
        public int TCP_Port
        {
            get { return _tcpPort; }
            set
            {
                _tcpPort = value;
            }
        }
        public bool IsConnected
        {
            get { return tcpHandler != null && tcpHandler.ConnectionStatus == EnCommuincationStatus.Connected; }
        }
        public bool IsReverseInteger { get; set; }
        public bool IsReverseString { get; set; }

        public void Connect()
        {
            tcpHandler = new TCPCommunication(IP_Address, TCP_Port, TCPCommunication.TypeProtocol.ModbusClient);
            tcpHandler.Connect();
            tcpHandler.SocketReceiveTImeOut = 100;
        }
        public void DisConnect()
        {
            tcpHandler.DisConnect();
        }
        public void Dispose()
        {
            tcpHandler.Dispose();
        }

        // Modbus Protocol
        private byte[] GetByte_Modbus_Protocol(ModbusInfo pModbusInfo)
        {
            pModbusInfo.Read_StartAddress = pModbusInfo.Read_StartAddress - 1;
            pModbusInfo.Write_StartAddress = pModbusInfo.Write_StartAddress - 1;
            byte[] pValue = new byte[1024];
            if (pModbusInfo.Read_StartAddress > 65535 | pModbusInfo.Read_Qty > 2000 | pModbusInfo.Write_StartAddress > 65535 | pModbusInfo.Write_Qty > 2000)
                return pValue;

            #region 변수
            int[] pIntData = new int[pModbusInfo.Write_Qty];
            bool[] pBoolData = new bool[pModbusInfo.Write_Qty];
            byte[] crc = new byte[2];
            byte[] pByteAddressR = new byte[2];
            byte[] pByteAddressW = new byte[2];
            byte[] ProtocolLength = new byte[2];
            byte[] byteCntArray = new byte[2];
            byte byteCnt = 0;
            byte[] pQtyOutputsR = new byte[2];
            byte[] pQtyOutputsW = new byte[2];

            bool YN_Write = pModbusInfo.ModbusFunction == EnModBusFunction.Write_Single_Coil || pModbusInfo.ModbusFunction == EnModBusFunction.Write_Single_Register ||
                pModbusInfo.ModbusFunction == EnModBusFunction.Write_Multiple_Coils || pModbusInfo.ModbusFunction == EnModBusFunction.Write_Multiple_Register ||
                pModbusInfo.ModbusFunction == EnModBusFunction.ReadWrite_Multiple_Registers;

            if (YN_Write)
            {
                object tmpData;
                if (pModbusInfo.Write_Data.Length > 0)
                    tmpData = pModbusInfo.Write_Data[0];
                else
                    return pValue;
                if (tmpData is bool)
                {
                    for (int i = 0; i < pModbusInfo.Write_Qty; i++)
                    {
                        pBoolData[i] = (bool)pModbusInfo.Write_Data[i];
                    }
                }
                else if (tmpData is short)
                {
                    for (int i = 0; i < pModbusInfo.Write_Qty; i++)
                    {
                        pIntData[i] = (short)pModbusInfo.Write_Data[i];
                    }
                }
                else
                {
                    return pValue;
                }
            }
            #endregion

            switch (pModbusInfo.ModbusFunction)
            {
                case EnModBusFunction.Read_Coils:
                    pByteAddressR = BitConverter.GetBytes(pModbusInfo.Read_StartAddress);
                    ProtocolLength = BitConverter.GetBytes((int)0x0006);
                    byteCntArray = BitConverter.GetBytes(pModbusInfo.Read_Qty);

                    pValue = new byte[]
                    {
                        (byte)(modbusTransaction / 256),
                        (byte)(modbusTransaction % 256),
                        0,
                        0,
                        ProtocolLength[1],
                        ProtocolLength[0],
                        1,
                        Convert.ToByte(pModbusInfo.ModbusFunction),
                        pByteAddressR[1],
                        pByteAddressR[0],
                        byteCntArray[1],
                        byteCntArray[0],
                        0,
                        0
                    };
                    crc = BitConverter.GetBytes(calculateCRC(pValue, 6, 6));
                    pValue[12] = crc[0];
                    pValue[13] = crc[1];
                    break;
                case EnModBusFunction.Read_Discrete_Inputs:
                    pByteAddressR = BitConverter.GetBytes(pModbusInfo.Read_StartAddress);
                    ProtocolLength = BitConverter.GetBytes((int)0x0006);
                    byteCntArray = BitConverter.GetBytes(pModbusInfo.Read_Qty);

                    pValue = new byte[]
                    {
                        (byte)(modbusTransaction / 256),
                        (byte)(modbusTransaction % 256),
                        0,
                        0,
                        ProtocolLength[1],
                        ProtocolLength[0],
                        1,
                        Convert.ToByte(pModbusInfo.ModbusFunction),
                        pByteAddressR[1],
                        pByteAddressR[0],
                        byteCntArray[1],
                        byteCntArray[0],
                        0,
                        0
                    };
                    crc = BitConverter.GetBytes(calculateCRC(pValue, 6, 6));
                    pValue[12] = crc[0];
                    pValue[13] = crc[1];
                    break;
                case EnModBusFunction.Read_Holding_Registers:
                    pByteAddressR = BitConverter.GetBytes(pModbusInfo.Read_StartAddress);
                    ProtocolLength = BitConverter.GetBytes((int)0x0006);
                    byteCntArray = BitConverter.GetBytes(pModbusInfo.Read_Qty);

                    pValue = new byte[]
                    {
                        (byte)(modbusTransaction / 256),
                        (byte)(modbusTransaction % 256),
                        0,
                        0,
                        ProtocolLength[1],
                        ProtocolLength[0],
                        1,
                        Convert.ToByte(pModbusInfo.ModbusFunction),
                        pByteAddressR[1],
                        pByteAddressR[0],
                        byteCntArray[1],
                        byteCntArray[0],
                        0,
                        0
                    };
                    crc = BitConverter.GetBytes(calculateCRC(pValue, 6, 6));
                    pValue[12] = crc[0];
                    pValue[13] = crc[1];
                    break;
                case EnModBusFunction.Read_Input_Registers:
                    pByteAddressR = BitConverter.GetBytes(pModbusInfo.Read_StartAddress);
                    ProtocolLength = BitConverter.GetBytes((int)0x0006);
                    byteCntArray = BitConverter.GetBytes(pModbusInfo.Read_Qty);

                    pValue = new byte[]
                    {
                        (byte)(modbusTransaction / 256),
                        (byte)(modbusTransaction % 256),
                        0,
                        0,
                        ProtocolLength[1],
                        ProtocolLength[0],
                        1,
                        Convert.ToByte(pModbusInfo.ModbusFunction),
                        pByteAddressR[1],
                        pByteAddressR[0],
                        byteCntArray[1],
                        byteCntArray[0],
                        0,
                        0
                    };
                    crc = BitConverter.GetBytes(calculateCRC(pValue, 6, 6));
                    pValue[12] = crc[0];
                    pValue[13] = crc[1];
                    break;
                case EnModBusFunction.Write_Single_Coil:
                    pByteAddressW = BitConverter.GetBytes(pModbusInfo.Write_StartAddress);
                    ProtocolLength = BitConverter.GetBytes((int)0x0006);

                    byte[] pBitValueArray = new byte[2];
                    pBitValueArray = BitConverter.GetBytes(pBoolData[0] == true ? (int)0xFF00 : (int)0x0000);

                    pValue = new byte[]
                    {
                        (byte)(modbusTransaction / 256),
                        (byte)(modbusTransaction % 256),
                        0,
                        0,
                        ProtocolLength[1],
                        ProtocolLength[0],
                        1,
                        Convert.ToByte(pModbusInfo.ModbusFunction),
                        pByteAddressW[1],
                        pByteAddressW[0],
                        pBitValueArray[1],
                        pBitValueArray[0],
                        0,
                        0
                    };
                    crc = BitConverter.GetBytes(calculateCRC(pValue, 6, 6));
                    pValue[12] = crc[0];
                    pValue[13] = crc[1];
                    break;
                case EnModBusFunction.Write_Single_Register:
                    pByteAddressW = BitConverter.GetBytes(pModbusInfo.Write_StartAddress);
                    ProtocolLength = BitConverter.GetBytes((int)0x0006);

                    byte[] pItrValue = new byte[2];
                    pItrValue = BitConverter.GetBytes((int)pIntData[0]);

                    pValue = new byte[]
                    {
                        (byte)(modbusTransaction / 256),
                        (byte)(modbusTransaction % 256),
                        0,
                        0,
                        ProtocolLength[1],
                        ProtocolLength[0],
                        1,
                        Convert.ToByte(pModbusInfo.ModbusFunction),
                        pByteAddressW[1],
                        pByteAddressW[0],
                        pItrValue[1],
                        pItrValue[0],
                        0,
                        0
                    };
                    crc = BitConverter.GetBytes(calculateCRC(pValue, 6, 6));
                    pValue[12] = crc[0];
                    pValue[13] = crc[1];
                    break;
                case EnModBusFunction.Write_Multiple_Coils:
                    pByteAddressW = BitConverter.GetBytes(pModbusInfo.Write_StartAddress);
                    ProtocolLength = BitConverter.GetBytes((int)(7 + (byteCnt)));

                    byteCnt = (byte)((pBoolData.Length % 8 != 0 ? pBoolData.Length / 8 + 1 : (pBoolData.Length / 8)));
                    pQtyOutputsW = BitConverter.GetBytes((int)pBoolData.Length);

                    byte pBitValue = 0;
                    pValue = new byte[14 + 2 + (pBoolData.Length % 8 != 0 ? pBoolData.Length / 8 : (pBoolData.Length / 8) - 1)];

                    // Transaction
                    pValue[0] = (byte)(modbusTransaction / 256);
                    pValue[1] = (byte)(modbusTransaction % 256);

                    // Protocol ID
                    pValue[2] = 0;
                    pValue[3] = 0;

                    // Length
                    pValue[4] = ProtocolLength[1];
                    pValue[5] = ProtocolLength[0];

                    // Slave ID
                    pValue[6] = 1;

                    // Function
                    pValue[7] = Convert.ToByte(pModbusInfo.ModbusFunction);

                    // Address
                    pValue[8] = pByteAddressW[1];
                    pValue[9] = pByteAddressW[0];

                    // Etc
                    pValue[10] = pQtyOutputsW[1];
                    pValue[11] = pQtyOutputsW[0];
                    pValue[12] = byteCnt;

                    for (int i = 0; i < pBoolData.Length; i++)
                    {
                        if ((i % 8) == 0)
                            pBitValue = 0;

                        byte CoilValue;
                        if (pBoolData[i] == true)
                            CoilValue = 1;
                        else
                            CoilValue = 0;

                        pBitValue = (byte)((int)CoilValue << (i % 8) | (int)pBitValue);
                        pValue[13 + (i / 8)] = pBitValue;
                    }

                    crc = BitConverter.GetBytes(calculateCRC(pValue, (ushort)(pValue.Length - 8), 6));
                    pValue[pValue.Length - 2] = crc[0];
                    pValue[pValue.Length - 1] = crc[1];
                    break;
                case EnModBusFunction.Write_Multiple_Register:
                    pByteAddressW = BitConverter.GetBytes(pModbusInfo.Write_StartAddress);
                    ProtocolLength = BitConverter.GetBytes((int)(7 + pIntData.Length * 2));

                    byteCnt = (byte)(pIntData.Length * 2);
                    pQtyOutputsW = BitConverter.GetBytes((int)pIntData.Length);

                    pValue = new byte[13 + 2 + pIntData.Length * 2];

                    // Transaction
                    pValue[0] = (byte)(modbusTransaction / 256);
                    pValue[1] = (byte)(modbusTransaction % 256);

                    // Protocol ID
                    pValue[2] = 0;
                    pValue[3] = 0;

                    // Length
                    pValue[4] = ProtocolLength[1];
                    pValue[5] = ProtocolLength[0];

                    // Slave ID
                    pValue[6] = 1;

                    // Function
                    pValue[7] = Convert.ToByte(pModbusInfo.ModbusFunction);

                    // Address
                    pValue[8] = pByteAddressW[1];
                    pValue[9] = pByteAddressW[0];

                    // Etc
                    pValue[10] = pQtyOutputsW[1];
                    pValue[11] = pQtyOutputsW[0];
                    pValue[12] = byteCnt;

                    for (int i = 0; i < pIntData.Length; i++)
                    {
                        byte[] pRegValue = BitConverter.GetBytes((int)pIntData[i]);
                        pValue[13 + i * 2] = pRegValue[1];
                        pValue[14 + i * 2] = pRegValue[0];
                    }

                    crc = BitConverter.GetBytes(calculateCRC(pValue, (ushort)(pValue.Length - 8), 6));
                    pValue[pValue.Length - 2] = crc[0];
                    pValue[pValue.Length - 1] = crc[1];
                    break;
                case EnModBusFunction.ReadWrite_Multiple_Registers:
                    pByteAddressR = BitConverter.GetBytes(pModbusInfo.Read_StartAddress);
                    pByteAddressW = BitConverter.GetBytes(pModbusInfo.Write_StartAddress);
                    ProtocolLength = BitConverter.GetBytes((int)11 + pIntData.Length * 2);

                    pQtyOutputsR = BitConverter.GetBytes(pModbusInfo.Read_Qty);
                    pQtyOutputsW = BitConverter.GetBytes((int)pIntData.Length);
                    byte writeByteCountLocal = Convert.ToByte(pIntData.Length * 2);

                    pValue = new byte[17 + 2 + pIntData.Length * 2];

                    // Transaction
                    pValue[0] = (byte)(modbusTransaction / 256);
                    pValue[1] = (byte)(modbusTransaction % 256);

                    // Protocol ID
                    pValue[2] = 0;
                    pValue[3] = 0;

                    // Length
                    pValue[4] = ProtocolLength[1];
                    pValue[5] = ProtocolLength[0];

                    // Slave ID
                    pValue[6] = 1;

                    // Function
                    pValue[7] = Convert.ToByte(pModbusInfo.ModbusFunction);

                    // Reading Address
                    pValue[8] = pByteAddressR[1];
                    pValue[9] = pByteAddressR[0];

                    // Reading Qty
                    pValue[10] = pQtyOutputsR[1];
                    pValue[11] = pQtyOutputsR[0];

                    // Writing Address
                    pValue[12] = pByteAddressW[1];
                    pValue[13] = pByteAddressW[0];

                    // Writing Qty
                    pValue[14] = pQtyOutputsW[1];
                    pValue[15] = pQtyOutputsW[0];

                    pValue[16] = writeByteCountLocal;

                    for (int i = 0; i < pIntData.Length; i++)
                    {
                        byte[] pRegValue = BitConverter.GetBytes((int)pIntData[i]);
                        pValue[17 + i * 2] = pRegValue[1];
                        pValue[18 + i * 2] = pRegValue[0];
                    }

                    crc = BitConverter.GetBytes(calculateCRC(pValue, (ushort)(pValue.Length - 8), 6));
                    pValue[pValue.Length - 2] = crc[0];
                    pValue[pValue.Length - 1] = crc[1];
                    break;
                default:
                    break;
            }

            return pValue;
        }
        private UInt16 calculateCRC(byte[] data, UInt16 numberOfBytes, int startByte)
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

        // Read Coil
        public bool ReadBool(int pAddress)
        {
            bool pValue = false;
            pValue = ReadBool(pAddress, defaultLength)[defaultLength - 1];
            return pValue;
        }
        public bool[] ReadBool(int pAddress, int pLength)
        {
            lock (_sendAndResponseLock)
            {
                if (pAddress > _maxAddress)
                    pAddress = _maxAddress;
                if (pLength > _maxLength)
                    pLength = _maxLength;
                if (pAddress + pLength > _maxAddress + 1)
                    pLength = _maxAddress + 1 - pAddress;

                bool[] pValue = new bool[pLength];

                if (tcpHandler.ConnectionStatus == EnCommuincationStatus.Connected)
                {
                    try
                    {
                        byte[] pSend = new byte[4096];
                        byte[] pRecev = new byte[4096];

                        ModbusInfo ReadCoil = new ModbusInfo();
                        ReadCoil.Read_StartAddress = pAddress;
                        ReadCoil.Read_Qty = pLength;
                        ReadCoil.ModbusFunction = EnModBusFunction.Read_Coils;

                        pSend = GetByte_Modbus_Protocol(ReadCoil);
                        pRecev = tcpHandler.SendAndResponseModbus(pSend);

                        bool[] tmp = new bool[pLength];
                        for (int i = 0; i < pLength; i++)
                        {
                            int intData = pRecev[9 + i / 8];
                            int mask = Convert.ToInt32(Math.Pow(2, (i % 8)));
                            tmp[i] = Convert.ToBoolean((intData & mask) / mask);
                        }
                        pValue = tmp;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
                return pValue;
            }
        }

        // Read Register - Integer
        public int ReadInt(int pAddress)
        {
            int pValue = 0;
            pValue = ReadInt(pAddress, defaultLength)[defaultLength - 1];
            return pValue;
        }
        public int[] ReadInt(int pAddress, int pLength)
        {
            lock (_sendAndResponseLock)
            {
                if (pAddress > _maxAddress)
                    pAddress = _maxAddress;
                if (pLength > _maxLength)
                    pLength = _maxLength;
                if (pAddress + pLength > _maxAddress + 1)
                    pLength = _maxAddress + 1 - pAddress;

                int[] pValue = new int[pLength];

                if (tcpHandler.ConnectionStatus == EnCommuincationStatus.Connected)
                {
                    try
                    {
                        byte[] pSend = new byte[4096];
                        byte[] pRecev = new byte[4096];

                        ModbusInfo ReadHoldingReg = new ModbusInfo();
                        ReadHoldingReg.Read_StartAddress = pAddress;
                        ReadHoldingReg.Read_Qty = pLength;
                        ReadHoldingReg.ModbusFunction = EnModBusFunction.Read_Holding_Registers;

                        pSend = GetByte_Modbus_Protocol(ReadHoldingReg);
                        pRecev = tcpHandler.SendAndResponseModbus(pSend);

                        int[] tmp = new int[pLength];
                        for (int i = 0; i < pLength; i++)
                        {
                            byte lowByte;
                            byte highByte;
                            highByte = pRecev[9 + i * 2];
                            lowByte = pRecev[9 + i * 2 + 1];

                            pRecev[9 + i * 2] = lowByte;
                            pRecev[9 + i * 2 + 1] = highByte;

                            tmp[i] = BitConverter.ToInt16(pRecev, (9 + i * 2));
                        }
                        if (IsReverseInteger) 
                            Array.Reverse(tmp);
                        pValue = tmp;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }

                return pValue;
            }
        }

        // Read Register - String
        public string ReadString(int pAddress)
        {
            string pValue = string.Empty;
            pValue = ReadString(pAddress, defaultLength);
            return pValue;
        }
        public string ReadString(int pAddress, int pLength)
        {
            lock (_sendAndResponseLock)
            {
                if (pAddress > _maxAddress)
                    pAddress = _maxAddress;
                if (pLength > _maxLength)
                    pLength = _maxLength;
                if (pAddress + pLength > _maxAddress + 1)
                    pLength = _maxAddress + 1 - pAddress;

                string pValue = string.Empty;

                if (tcpHandler.ConnectionStatus == EnCommuincationStatus.Connected)
                {
                    try
                    {
                        byte[] pSend = new byte[4096];
                        byte[] pRecev = new byte[4096];

                        ModbusInfo ReadHoldingReg = new ModbusInfo();
                        ReadHoldingReg.Read_StartAddress = pAddress;
                        ReadHoldingReg.Read_Qty = pLength;
                        ReadHoldingReg.ModbusFunction = EnModBusFunction.Read_Holding_Registers;

                        pSend = GetByte_Modbus_Protocol(ReadHoldingReg);
                        pRecev = tcpHandler.SendAndResponseModbus(pSend);

                        byte[] tmp = new byte[pLength * 2];
                        for (int i = 0; i < pLength * 2; i++)
                        {
                            tmp[i] = pRecev[9 + i];
                        }
                        pValue = Encoding.ASCII.GetString(tmp).Replace(Convert.ToChar(0).ToString(), string.Empty);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }

                return pValue;
            }
        }

        // Write
        public bool Write(int pAddress, bool pData)
        {
            bool pSuccess = false;

            bool[] tmp = new bool[1];
            tmp[0] = pData;
            pSuccess = Write(pAddress, tmp);

            return pSuccess;
        }
        public bool Write(int pAddress, bool[] pData)
        {
            lock (_sendAndResponseLock)
            {
                bool pSuccess = false;
                int pLength = pData.Length;

                if (pAddress > _maxAddress)
                    pAddress = _maxAddress;
                if (pLength > _maxLength)
                    pLength = _maxLength;
                if (pAddress + pLength > _maxAddress + 1)
                    pLength = _maxAddress + 1 - pAddress;

                if (tcpHandler.ConnectionStatus == EnCommuincationStatus.Connected)
                {
                    try
                    {
                        byte[] pSend = new byte[4096];
                        byte[] pRecev = new byte[4096];
                        EnModBusFunction pWriteCoil = pLength > 1 ? EnModBusFunction.Write_Multiple_Coils : EnModBusFunction.Write_Single_Coil;
                        object[] tmp = new object[pLength];
                        for (int i = 0; i < pLength; i++)
                        {
                            tmp[i] = pData[i];
                        }

                        ModbusInfo WriteCoils = new ModbusInfo();
                        WriteCoils.Write_StartAddress = pAddress;
                        WriteCoils.Write_Qty = pLength;
                        WriteCoils.Write_Data = tmp;
                        WriteCoils.ModbusFunction = pLength > 1 ? EnModBusFunction.Write_Multiple_Coils : EnModBusFunction.Write_Single_Coil;

                        pSend = GetByte_Modbus_Protocol(WriteCoils);
                        pRecev = tcpHandler.SendAndResponseModbus(pSend);

                        pSuccess = pRecev[7] != 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }

                return pSuccess;
            }
        }
        public bool Write(int pAddress, short pData)
        {
            bool pSuccess = false;

            short[] tmp = new short[1];
            tmp[0] = pData;
            pSuccess = Write(pAddress, tmp);

            return pSuccess;
        }
        public bool Write(int pAddress, short[] pData)
        {
            lock (_sendAndResponseLock)
            {
                bool pSuccess = false;
                int pLength = pData.Length;

                if (pAddress > _maxAddress)
                    pAddress = _maxAddress;
                if (pLength > _maxLength)
                    pLength = _maxLength;
                if (pAddress + pLength > _maxAddress + 1)
                    pLength = _maxAddress + 1 - pAddress;

                if (tcpHandler.ConnectionStatus == EnCommuincationStatus.Connected)
                {
                    try
                    {
                        byte[] pSend = new byte[4096];
                        byte[] pRecev = new byte[4096];
                        EnModBusFunction pWriteCoil = pLength > 1 ? EnModBusFunction.Write_Multiple_Register : EnModBusFunction.Write_Single_Register;
                        object[] tmp = new object[pLength];
                        for (int i = 0; i < pLength; i++)
                        {
                            tmp[i] = (short)pData[i];
                        }
                        if (IsReverseInteger)
                            Array.Reverse(tmp);

                        ModbusInfo WriteReg = new ModbusInfo();
                        WriteReg.Write_StartAddress = pAddress;
                        WriteReg.Write_Qty = pLength;
                        WriteReg.Write_Data = tmp;
                        WriteReg.ModbusFunction = pLength > 1 ? EnModBusFunction.Write_Multiple_Register : EnModBusFunction.Write_Single_Register;

                        pSend = GetByte_Modbus_Protocol(WriteReg);
                        pRecev = tcpHandler.SendAndResponseModbus(pSend);

                        pSuccess = pRecev[7] != 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }

                return pSuccess;
            }
        }
        public bool Write(int pAddress, string pData, int pLength)
        {
            lock (_sendAndResponseLock)
            {
                bool pSuccess = false;

                if (tcpHandler.ConnectionStatus == EnCommuincationStatus.Connected)
                {
                    try
                    {
                        byte[] pSend = new byte[4096];
                        byte[] pRecev = new byte[4096];
                        EnModBusFunction pWriteCoil = pData.Length > 2 ? EnModBusFunction.Write_Multiple_Register : EnModBusFunction.Write_Single_Register;

                        if (pData.Length % 2 == 1) pData = string.Format("{0}{1}", pData, Convert.ToChar(0));

                        int pArrayLength = pLength;
                        object[] tmp = new object[pArrayLength]; 
                        short[] tmpShort = new short[pArrayLength];
                        byte[] bytesStr = Encoding.UTF8.GetBytes(pData);
                        int byteIndex = 0;
                        for (int i = 0; i < pArrayLength && byteIndex < bytesStr.Length; i++)
                        {
                            if (IsReverseString)
                            {
                                tmpShort[i] = (short)((byteIndex + 1 < bytesStr.Length ? bytesStr[byteIndex + 1] << 8 : 0) | bytesStr[byteIndex]);
                            }
                            else
                            {
                                tmpShort[i] = (short)((bytesStr[byteIndex] << 8) | (byteIndex + 1 < bytesStr.Length ? bytesStr[byteIndex + 1] : 0));
                            }
                            byteIndex += 2;
                        }
                        for (int i = 0; i < pArrayLength; i++)
                        {
                            tmp[i] = (short)tmpShort[i];
                        }

                        if (pAddress > _maxAddress)
                            pAddress = _maxAddress;
                        if (pArrayLength > _maxLength)
                            pArrayLength = _maxLength;
                        if (pAddress + pArrayLength > _maxAddress + 1)
                            pArrayLength = _maxAddress + 1 - pAddress;

                        ModbusInfo WriteReg = new ModbusInfo();
                        WriteReg.Write_StartAddress = pAddress;
                        WriteReg.Write_Qty = pArrayLength;
                        WriteReg.Write_Data = tmp;
                        WriteReg.ModbusFunction = pArrayLength > 1 ? EnModBusFunction.Write_Multiple_Register : EnModBusFunction.Write_Single_Register;

                        pSend = GetByte_Modbus_Protocol(WriteReg);
                        pRecev = tcpHandler.SendAndResponseModbus(pSend);

                        pSuccess = pRecev[7] != 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }

                return pSuccess;
            }
        }
    }
}
