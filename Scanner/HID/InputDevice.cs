using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;


namespace HandScanner.HID
{
    public sealed class InputDevice
    {

        // The following constants are defined in Windows.h

        private const int RIDEV_INPUTSINK = 0x00000100;
        private const int RID_INPUT = 0x10000003;

        private const int FAPPCOMMAND_MASK = 0xF000;
        private const int FAPPCOMMAND_MOUSE = 0x8000;
        private const int FAPPCOMMAND_OEM = 0x1000;

        private const int RIM_TYPEMOUSE = 0;
        private const int RIM_TYPEKEYBOARD = 1;
        private const int RIM_TYPEHID = 2;

        private const int RIDI_DEVICENAME = 0x20000007;

        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_INPUT = 0x00FF;
        private const int VK_OEM_CLEAR = 0xFE;
        private const int VK_LAST_KEY = VK_OEM_CLEAR; // this is a made up value used as a sentinel

        public enum DeviceType
        {
            Key,
            Mouse,
            OEM
        }

        public class DeviceInfo
        {
            public string deviceName;
            public string deviceType;
            public IntPtr deviceHandle;
            public string Name;
            public string source;
            public ushort key;
            public string vKey;
            public string Ascii;
            public EnScanner scanner;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RAWINPUTDEVICELIST
        {
            public IntPtr hDevice;
            [MarshalAs(UnmanagedType.U4)]
            public int dwType;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct RAWINPUTHEADER
        {
            [MarshalAs(UnmanagedType.U4)]
            public int dwType;
            [MarshalAs(UnmanagedType.U4)]
            public int dwSize;
            public IntPtr hDevice;
            [MarshalAs(UnmanagedType.U4)]
            public int wParam;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct RAWHID
        {
            [MarshalAs(UnmanagedType.U4)]
            public int dwSizHid;
            [MarshalAs(UnmanagedType.U4)]
            public int dwCount;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct BUTTONSSTR
        {
            [MarshalAs(UnmanagedType.U2)]
            public ushort usButtonFlags;
            [MarshalAs(UnmanagedType.U2)]
            public ushort usButtonData;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct RAWKEYBOARD
        {
            [MarshalAs(UnmanagedType.U2)]
            public ushort MakeCode;
            [MarshalAs(UnmanagedType.U2)]
            public ushort Flags;
            [MarshalAs(UnmanagedType.U2)]
            public ushort Reserved;
            [MarshalAs(UnmanagedType.U2)]
            public ushort VKey;
            [MarshalAs(UnmanagedType.U4)]
            public uint Message;
            [MarshalAs(UnmanagedType.U4)]
            public uint ExtraInformation;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct RAWINPUTDEVICE
        {
            [MarshalAs(UnmanagedType.U2)]
            public ushort usUsagePage;
            [MarshalAs(UnmanagedType.U2)]
            public ushort usUsage;
            [MarshalAs(UnmanagedType.U4)]
            public int dwFlags;
            public IntPtr hwnedTaget;
        }
        [StructLayout(LayoutKind.Explicit)]
        internal struct RAWMOUSE
        {
            [MarshalAs(UnmanagedType.U2)]
            [FieldOffset(0)]
            public ushort usFlags;
            [MarshalAs(UnmanagedType.U4)]
            [FieldOffset(4)]
            public uint ulButtons;
            [FieldOffset(4)]
            public BUTTONSSTR buttonsStr;
            [MarshalAs(UnmanagedType.U4)]
            [FieldOffset(8)]
            public uint ulRawButtons;
            [FieldOffset(12)]
            public int ILastX;
            [FieldOffset(16)]
            public int ILastY;
            [MarshalAs(UnmanagedType.U4)]
            [FieldOffset(20)]
            public uint ulExtraInformation;

        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct RAWINPUT
        {
            public RAWINPUTHEADER header;
            public Union Data;
            [StructLayout(LayoutKind.Explicit)]
            public struct Union
            {
                [FieldOffset(0)]
                public RAWMOUSE mouse;
                [FieldOffset(0)]
                public RAWKEYBOARD keyboard;
                [FieldOffset(0)]
                public RAWHID hid;
            }


            //[FieldOffset(0)]
            //public RAWINPUTHEADER header;
            //[FieldOffset(16)]
            //public RAWMOUSE mouse;
            //[FieldOffset(16)]
            //public RAWKEYBOARD keyboard;
            //[FieldOffset(16)]
            //public RAWHID hid;
        }
        [DllImport("user32.dll")]
        extern static uint GetRawInputDeviceList(IntPtr pRawInputDeviceList, ref uint uiNumDevices, uint cbSize);
        [DllImport("user32.dll")]
        extern static uint GetRawInputDeviceInfo(IntPtr hDevice, uint uiCommand, IntPtr pData, ref uint pcbSize);
        [DllImport("user32.dll")]
        extern static bool RegisterRawInputDevices(RAWINPUTDEVICE[] pRawInputDevice, uint uiNumDevices, uint cbSize);
        [DllImport("user32.dll")]
        extern static uint GetRawInputData(IntPtr hRawInput, uint uiCommand, IntPtr pData, ref uint pcbSize, uint cbSizeHeader);
        [DllImport("user32.dll")]
        static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[] lpKeyState, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszBuff, int cchBuff, uint wFlags, IntPtr dwhkl);
        [DllImport("user32.dll")]
        static extern bool GetKeyboardState(byte[] lpKeyState);
        [DllImport("User32.dll")]
        static extern uint MapVirtualKey(uint uCode, uint uMapTYpe);
        [DllImport("user32.dll")]
        static extern IntPtr GetKeyboardLayout(uint idThread);

        public Hashtable DeviceList
        {
            get
            {
                return deviceList;
            }
        }

        private Hashtable deviceList;
        public delegate void DeviceEventHandler(object sender, KeyControlEventArgs e);
        public event DeviceEventHandler KeyPressed;
        private object _lock = new object();
        public class KeyControlEventArgs : EventArgs
        {
            private DeviceInfo m_deviceInfo;
            private DeviceType m_device;
            public KeyControlEventArgs(DeviceInfo dInfo, DeviceType device)
            {
                m_deviceInfo = dInfo;
                m_device = device;
            }
            public KeyControlEventArgs()
            {
            }
            public DeviceInfo Keyboard
            {
                get { return m_deviceInfo; }
                set { m_deviceInfo = value; }
            }
            public DeviceType Device
            {
                get { return m_device; }
                set { m_device = value; }
            }
        }
        public InputDevice(IntPtr hwnd)
        {
            RAWINPUTDEVICE[] rid = new RAWINPUTDEVICE[1];
            rid[0].usUsagePage = 0x01;
            rid[0].usUsage = 0x06;
            rid[0].dwFlags = RIDEV_INPUTSINK;
            rid[0].hwnedTaget = hwnd;
            HardWareID = new List<string>();
            deviceList = new Hashtable();
            if (!RegisterRawInputDevices(rid, (uint)rid.Length, (uint)Marshal.SizeOf(rid[0])))
            {
                throw new ApplicationException("Failed to register raw input devices(s).");
            }
        }
        public List<string> HardWareID { get; set; }

        private string ReadReg(string item, ref bool isKeyboard)
        {
            item = item.Substring(4);
            string[] split = item.Split('#');
            string id_01 = split[0]; //ACPI (Class code)
            string id_02 = split[1]; //PNP0303 (subclass code)
            string id_03 = split[2]; //3&13c0b0c5&0 (Protocol code)

            RegistryKey OurKey = Registry.LocalMachine;
            string findme = string.Format(@"System\CurrentControlSet\Enum\{0}\{1}\{2}", id_01, id_02, id_03);
            OurKey = OurKey.OpenSubKey(findme, false);
            string deviceDesc = string.Empty;
            string deviceClass = string.Empty;
            if (OurKey.GetValue("DeviceDesc") != null)
                deviceDesc = (string)OurKey.GetValue("DeviceDesc");
            if (OurKey.GetValue("Class") != null)
                deviceClass = (string)OurKey.GetValue("Class");
            isKeyboard = deviceClass.ToUpper().Equals("KEYBOARD");
            return deviceDesc;
        }

        public int EnumerateDevices()
        {
            int NumberOfDevices = 0;
            uint deviceCount = 0;
            int dwSize = (Marshal.SizeOf(typeof(RAWINPUTDEVICELIST)));
            if (GetRawInputDeviceList(IntPtr.Zero, ref deviceCount, (uint)dwSize) == 0)
            {
                IntPtr pRawInputDeviceList = Marshal.AllocHGlobal((int)(dwSize * deviceCount));
                GetRawInputDeviceList(pRawInputDeviceList, ref deviceCount, (uint)dwSize);
                lock (_lock)
                {
                    deviceList.Clear();
                    for (int i = 0; i < deviceCount; i++)
                    {
                        DeviceInfo dInfo;
                        string deviceName;
                        uint pcbSize = 0;
                        RAWINPUTDEVICELIST rid = (RAWINPUTDEVICELIST)Marshal.PtrToStructure(new IntPtr((pRawInputDeviceList.ToInt64() + (dwSize * i))), typeof(RAWINPUTDEVICELIST));
                        GetRawInputDeviceInfo(rid.hDevice, RIDI_DEVICENAME, IntPtr.Zero, ref pcbSize);
                        if (pcbSize > 0)
                        {
                            IntPtr pData = Marshal.AllocHGlobal((int)pcbSize);
                            GetRawInputDeviceInfo(rid.hDevice, RIDI_DEVICENAME, pData, ref pcbSize);
                            deviceName = (string)Marshal.PtrToStringAnsi(pData);

                            if (deviceName.ToUpper().Contains("ROOT"))
                                continue;
                            if ((deviceName.Split('#').Length < 2 || deviceName.Split('#')[1].Length < 8) || !HardWareID.Contains(deviceName.Split('#')[1].Substring(0, 8)))
                                continue;
                            if (rid.dwType == RIM_TYPEKEYBOARD || rid.dwType == RIM_TYPEHID)
                            {
                                dInfo = new DeviceInfo();
                                dInfo.deviceName = (string)Marshal.PtrToStringAnsi(pData).Split('#')[1].Substring(0, 8);
                                dInfo.deviceHandle = rid.hDevice;
                                dInfo.deviceType = GetDeviceType(rid.dwType);

                                bool IsKeyboardDevice = false;
                                string DeviceDesc = ReadReg(deviceName, ref IsKeyboardDevice);
                                dInfo.Name = DeviceDesc;
                                if ((!deviceList.Contains(rid.hDevice)) && ((rid.dwType == RIM_TYPEKEYBOARD) || (rid.dwType == RIM_TYPEHID)))
                                {
                                    NumberOfDevices++;
                                    deviceList.Add(rid.hDevice, dInfo);
                                }
                            }
                            Marshal.FreeHGlobal(pData);
                        }
                    }
                }
                Marshal.FreeHGlobal(pRawInputDeviceList);
                return NumberOfDevices;
            }
            else
            {
                throw new ApplicationException("An error occurred while retrieving the list of devices.");
            }
        }
        string _hexCode;
        public bool ProcessInputCommand(Message message)
        {
            uint dwSize = 0;
            bool bRet = false;
            GetRawInputData(message.LParam, RID_INPUT, IntPtr.Zero, ref dwSize, (uint)Marshal.SizeOf(typeof(RAWINPUTHEADER)));
            IntPtr buffer = Marshal.AllocHGlobal((int)dwSize);
            try
            {
                if (buffer != IntPtr.Zero && GetRawInputData(message.LParam, RID_INPUT, buffer, ref dwSize, (uint)Marshal.SizeOf(typeof(RAWINPUTHEADER))) == dwSize)
                {
                    RAWINPUT raw = (RAWINPUT)Marshal.PtrToStructure(buffer, typeof(RAWINPUT));

                    if (deviceList.Contains(raw.header.hDevice))
                    {
                        if (raw.header.dwType == RIM_TYPEKEYBOARD && (raw.Data.keyboard.Message == (int)WM_KEYDOWN || raw.Data.keyboard.Message == (int)WM_SYSKEYDOWN))
                        {
                            ushort key = raw.Data.keyboard.VKey;
                            if (key > VK_LAST_KEY)
                                return false;
                            StringBuilder result = new StringBuilder();
                            DeviceInfo dInfo = null;
                            byte[] keystate = new byte[256];
                            uint scancode = MapVirtualKey(key, 2);
                            IntPtr inputLocalIdentifier = GetKeyboardLayout(0);
                            GetKeyboardState(keystate);

                            Keys myKey;
                            dInfo = (DeviceInfo)deviceList[raw.header.hDevice];
                            myKey = (Keys)Enum.Parse(typeof(Keys), Enum.GetName(typeof(Keys), key));
                            dInfo.vKey = myKey.ToString();
                            dInfo.key = key;
                            ToUnicodeEx((uint)key, scancode, keystate, result, (int)5, (uint)0, inputLocalIdentifier);
                            if (key == 18)
                            {
                                _hexCode = string.Empty;
                                return true;
                            }
                            else
                            {
                                if (_hexCode == null)
                                    dInfo.Ascii = result.ToString();
                                else
                                {
                                    _hexCode += Convert.ToChar(scancode);
                                    if (_hexCode.Length >= 4)
                                    {
                                        dInfo.Ascii = Convert.ToChar(Convert.ToInt32(_hexCode)).ToString();
                                        _hexCode = null;
                                    }
                                    else
                                        return true;

                                }
                                if (KeyPressed != null && dInfo != null)
                                    KeyPressed(this, new KeyControlEventArgs(dInfo, GetDevice(message.LParam.ToInt32())));
                                bRet = true;
                            }
                        }
                        else
                        {
                            bRet = true;
                        }
                    }
                }
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
            return bRet;
        }

        private DeviceType GetDevice(int param)
        {
            DeviceType deviceType;
            switch ((int)(((ushort)(param >> 16)) & FAPPCOMMAND_MASK))
            {
                case FAPPCOMMAND_OEM:
                    deviceType = DeviceType.OEM;
                    break;
                case FAPPCOMMAND_MOUSE:
                    deviceType = DeviceType.Mouse;
                    break;
                default:
                    deviceType = DeviceType.Key;
                    break;
            }
            return deviceType;
        }
        public bool ProcessMessage(Message message)
        {
            switch (message.Msg)
            {
                case WM_INPUT:
                    return ProcessInputCommand(message);
            }
            return false;
        }
        private string GetDeviceType(int device)
        {
            string deviceType;
            switch (device)
            {
                case RIM_TYPEMOUSE:
                    deviceType = "MOUSE";
                    break;
                case RIM_TYPEKEYBOARD:
                    deviceType = "KEYBOARD";
                    break;
                case RIM_TYPEHID:
                    deviceType = "HID";
                    break;
                default:
                    deviceType = "UNKNOWN";
                    break;
            }
            return deviceType;

        }
    }
}