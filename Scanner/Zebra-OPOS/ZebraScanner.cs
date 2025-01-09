using CoreScanner;
using log4net;
using System;
using System.Xml;

namespace HandScanner.Zebra_OPOS
{
    public partial class ZebraScanner
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        CCoreScanner _coreScanner;
        short[] _scannerType;
        int _status = Defines.STATUS_FALSE;
        bool _bSuccessOpen;
        public event EventHandler<ScannerReadEventArgs> ScannerRead;
        protected void OnScannerRead(ScannerReadEventArgs e)
        {
            EventHandler<ScannerReadEventArgs> handler;
            handler = ScannerRead;
            if (handler != null)
                handler(this, e);
        }

        public ZebraScanner()
        {
            _scannerType = new short[Defines.TOTAL_SCANNER_TYPES];
            _coreScanner = new CCoreScanner();
            _coreScanner.BarcodeEvent += _coreScanner_BarcodeEvent;
            _coreScanner.PNPEvent += _coreScanner_PNPEvent;
        }

        private void _coreScanner_PNPEvent(short eventType, ref string ppnpData)
        {
            //0 > PlugIn
            //1 > Unplug
        }

        private void _coreScanner_BarcodeEvent(short eventType, ref string pscanData)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(pscanData);
            string strData = string.Empty;
            string barcode = xmlDoc.DocumentElement.GetElementsByTagName("datalabel").Item(0).InnerText;
            int Symbology = Convert.ToInt32(xmlDoc.DocumentElement.GetElementsByTagName("datatype").Item(0).InnerText);
            string[] numbers = barcode.Split(' ');
            foreach (string num in numbers)
            {
                if (string.IsNullOrEmpty(num))
                {
                    break;
                }
                strData += ((char)Convert.ToInt32(num, 16)).ToString();
            }
            OnScannerRead(new ScannerReadEventArgs(new Barcode() { Int_Symbology = Symbology, ReadData = strData, Scanner = EnScanner.Zebra_OPOS }));
        }


        public void Connect()
        {
            _bSuccessOpen = false;
            int appHandle = 0;
            _scannerType[Defines.SCANNER_TYPES_ALL - 1] = 1;
            try
            {
                _coreScanner.Open(appHandle, _scannerType, 1, out _status);
                if (_status == Defines.STATUS_SUCCESS)
                {
                    _bSuccessOpen = true;
                    int nEvents = 0;
                    string strEvtIDs = GetRegUnregIDs(out nEvents);
                    string inXml = string.Format("<inArgs><cmdArgs>" +
                        "<arg-int>{0}</arg-int>" +
                        "<arg-int>{1}</arg-int>" +
                        "</cmdArgs></inArgs>", nEvents, strEvtIDs
                        );
                    int opCode = Defines.REGISTER_FOR_EVENTS;
                    string outXml = "";
                    int status = Defines.STATUS_FALSE;
                    ExecCmd(opCode, ref inXml, out outXml, out status);
                }
            }
            catch { }
        }
        public void DisConnect()
        {
            int status = Defines.STATUS_FALSE;
            try
            {
                _coreScanner.Close(0, out status);
            }
            catch
            {

            }
        }
        private void ExecCmd(int opCode, ref string inXml, out string outXml, out int status)
        {
            outXml = "";
            status = Defines.STATUS_FALSE;
            if (_bSuccessOpen)
            {
                try
                {
                    _coreScanner.ExecCommand(opCode, ref inXml, out outXml, out status);
                }
                catch { }
            }
        }
        string GetRegUnregIDs(out int nEvents)
        {
            string strIDs = "";
            nEvents = Defines.NUM_SCANNER_EVENTS;
            strIDs = Defines.SUBSCRIBE_BARCODE.ToString();
            strIDs += "," + Defines.SUBSCRIBE_IMAGE.ToString();
            strIDs += "," + Defines.SUBSCRIBE_VIDEO.ToString();
            strIDs += "," + Defines.SUBSCRIBE_RMD.ToString();
            strIDs += "," + Defines.SUBSCRIBE_PNP.ToString();
            strIDs += "," + Defines.SUBSCRIBE_OTHER.ToString();
            return strIDs;
        }


    }
    public partial class Defines
    {
        public const int ST_NOT_APP = 0x00;
        public const int ST_CODE_39 = 0x01;
        public const int ST_CODABAR = 0x02;
        public const int ST_CODE_128 = 0x03;
        public const int ST_D2OF5 = 0x04;
        public const int ST_IATA = 0x05;
        public const int ST_I2OF5 = 0x06;
        public const int ST_CODE93 = 0x07;
        public const int ST_UPCA = 0x08;
        public const int ST_UPCE0 = 0x09;
        public const int ST_EAN8 = 0x0a;
        public const int ST_EAN13 = 0x0b;
        public const int ST_CODE11 = 0x0c;
        public const int ST_CODE49 = 0x0d;
        public const int ST_MSI = 0x0e;
        public const int ST_EAN128 = 0x0f;
        public const int ST_UPCE1 = 0x10;
        public const int ST_PDF417 = 0x11;
        public const int ST_CODE16K = 0x12;
        public const int ST_C39FULL = 0x13;
        public const int ST_UPCD = 0x14;
        public const int ST_TRIOPTIC = 0x15;
        public const int ST_BOOKLAND = 0x16;
        public const int ST_COUPON = 0x17;
        public const int ST_NW7 = 0x18;
        public const int ST_ISBT128 = 0x19;
        public const int ST_MICRO_PDF = 0x1a;
        public const int ST_DATAMATRIX = 0x1b;
        public const int ST_QR_CODE = 0x1c;
        public const int ST_MICRO_PDF_CCA = 0x1d;
        public const int ST_POSTNET_US = 0x1e;
        public const int ST_PLANET_CODE = 0x1f;
        public const int ST_CODE_32 = 0x20;
        public const int ST_ISBT128_CON = 0x21;
        public const int ST_JAPAN_POSTAL = 0x22;
        public const int ST_AUS_POSTAL = 0x23;
        public const int ST_DUTCH_POSTAL = 0x24;
        public const int ST_MAXICODE = 0x25;
        public const int ST_CANADIN_POSTAL = 0x26;
        public const int ST_UK_POSTAL = 0x27;
        public const int ST_MACRO_PDF = 0x28;
        public const int ST_MACRO_QR_CODE = 0x29;
        public const int ST_MICRO_QR_CODE = 0x2c;
        public const int ST_AZTEC = 0x2d;
        public const int ST_AZTEC_RUNE = 0x2e;
        public const int ST_DISTANCE = 0x2f;
        public const int ST_GS1_DATABAR = 0x30;
        public const int ST_GS1_DATABAR_LIMITED = 0x31;
        public const int ST_GS1_DATABAR_EXPANDED = 0x32;
        public const int ST_PARAMETER = 0x33;
        public const int ST_USPS_4CB = 0x34;
        public const int ST_UPU_FICS_POSTAL = 0x35;
        public const int ST_ISSN = 0x36;
        public const int ST_SCANLET = 0x37;
        public const int ST_CUECODE = 0x38;
        public const int ST_MATRIX2OF5 = 0x39;
        public const int ST_UPCA_2 = 0x48;
        public const int ST_UPCE0_2 = 0x49;
        public const int ST_EAN8_2 = 0x4a;
        public const int ST_EAN13_2 = 0x4b;
        public const int ST_UPCE1_2 = 0x50;
        public const int ST_CCA_EAN128 = 0x51;
        public const int ST_CCA_EAN13 = 0x52;
        public const int ST_CCA_EAN8 = 0x53;
        public const int ST_CCA_RSS_EXPANDED = 0x54;
        public const int ST_CCA_RSS_LIMITED = 0x55;
        public const int ST_CCA_RSS14 = 0x56;
        public const int ST_CCA_UPCA = 0x57;
        public const int ST_CCA_UPCE = 0x58;
        public const int ST_CCC_EAN128 = 0x59;
        public const int ST_TLC39 = 0x5A;
        public const int ST_CCB_EAN128 = 0x61;
        public const int ST_CCB_EAN13 = 0x62;
        public const int ST_CCB_EAN8 = 0x63;
        public const int ST_CCB_RSS_EXPANDED = 0x64;
        public const int ST_CCB_RSS_LIMITED = 0x65;
        public const int ST_CCB_RSS14 = 0x66;
        public const int ST_CCB_UPCA = 0x67;
        public const int ST_CCB_UPCE = 0x68;
        public const int ST_SIGNATURE_CAPTURE = 0x69;
        public const int ST_MOA = 0x6A;
        public const int ST_PDF417_PARAMETER = 0x70;
        public const int ST_CHINESE2OF5 = 0x72;
        public const int ST_KOREAN_3_OF_5 = 0x73;
        public const int ST_DATAMATRIX_PARAM = 0x74;
        public const int ST_CODE_Z = 0x75;
        public const int ST_UPCA_5 = 0x88;
        public const int ST_UPCE0_5 = 0x89;
        public const int ST_EAN8_5 = 0x8a;
        public const int ST_EAN13_5 = 0x8b;
        public const int ST_UPCE1_5 = 0x90;
        public const int ST_MACRO_MICRO_PDF = 0x9A;
        public const int ST_OCRB = 0xA0;
        public const int ST_OCR = 0xA1;
        public const int ST_PARSED_DRIVER_LICENSE = 0xB1;
        public const int ST_PARSED_UID = 0xB2;
        public const int ST_PARSED_NDC = 0xB3;
        public const int ST_DATABAR_COUPON = 0xB4;
        public const int ST_PARSED_XML = 0xB6;
        public const int ST_HAN_XIN_CODE = 0xB7;
        public const int ST_CALIBRATION = 0xC0;
        public const int ST_GS1_DATAMATRIX = 0xC1;
        public const int ST_GS1_QR = 0xC2;
        public const int BT_MAINMARK = 0xC3;
        public const int BT_DOTCODE = 0xC4;


        //End Symbology Types

        public const string APP_TITLE = "Scanner Multi-Interface Test Utility";
        public const string STR_OPEN = "Start";
        public const string STR_CLOSE = "Stop";
        public const string STR_REFRESH = "Rediscover Scanners";
        public const string STR_FIND = "Discover Scanners";
        public const int NUM_SCANNER_EVENTS = 6;


        // Scanner types
        public const short SCANNER_TYPES_ALL = 1;
        public const short SCANNER_TYPES_SNAPI = 2;
        public const short SCANNER_TYPES_SSI = 3;
        public const short SCANNER_TYPES_RSM = 4;
        public const short SCANNER_TYPES_IMAGING = 5;
        public const short SCANNER_TYPES_IBMHID = 6;
        public const short SCANNER_TYPES_NIXMODB = 7;
        public const short SCANNER_TYPES_HIDKB = 8;
        public const short SCANNER_TYPES_IBMTT = 9;
        public const short SCALE_TYPES_IBM = 10;
        public const short SCALE_TYPES_SSI_BT = 11;

        // Total number of scanner types
        public const short TOTAL_SCANNER_TYPES = SCALE_TYPES_SSI_BT;

        //as in RMD_CMD_OPCODE_T of FUD_RMDTypes.h //
        public const int ATTR_GETALL = 0x01;
        public const int ATTR_GET = 0x02;
        public const int ATTR_GETNEXT = 0x03;
        public const int ATTR_GETOFFSET = 0x04;
        public const int ATTR_SET = 0x05;
        public const int ATTR_STORE = 0x06;
        // end of RMD_CMD_OPCODE_T //

        public const int SUBSCRIBE_BARCODE = 1;
        public const int SUBSCRIBE_IMAGE = 2;
        public const int SUBSCRIBE_VIDEO = 4;
        public const int SUBSCRIBE_RMD = 8;
        public const int SUBSCRIBE_PNP = 16;
        public const int SUBSCRIBE_OTHER = 32;

        // available values for 'status' //
        public const int STATUS_SUCCESS = 0;
        public const int STATUS_FALSE = 1;
        public const int STATUS_LOCKED = 10;


        // Barcode, Image & Video event types //
        public const int SCANNER_DECODE_GOOD = 1;
        public const int IMAGE_COMPLETE = 1;
        public const int IMAGE_TRAN_STATUS = 2;
        public const int VIDEO_FRAME_COMPLETE = 1;

        //for WM_DEVICE_NOTIFICATION//
        public const int SCANNER_ATTACHED = 0;
        public const int SCANNER_DETTACHED = 1;

        // Scanner Notification Event Types

        public const int BARCODE_MODE = 1;
        public const int IMAGE_MODE = 2;
        public const int VIDEO_MODE = 3;
        public const int DEVICE_ENABLED = 13;
        public const int DEVICE_DISABLED = 14;

        // Firmware download events //
        public const int SCANNER_UF_SESS_START = 11; // Triggered when flash download session starts 
        public const int SCANNER_UF_DL_START = 12; // Triggered when component download starts 
        public const int SCANNER_UF_DL_PROGRESS = 13; // Triggered when block(s) of flash completed 
        public const int SCANNER_UF_DL_END = 14; // Triggered when component download ends 
        public const int SCANNER_UF_SESS_END = 15; // Triggered when flash download session ends 

        public const int SCANNER_UF_STATUS = 16; // Triggered when update error or status
                                                 //------FW------------------//


        public const int MAX_NUM_DEVICES = 255;/* Maximum number of scanners to be connected*/

        public const int MAX_BUFF_SIZE = 1024;
        public const int MAX_PARAM_LEN = 2;        /* Maximum number of bytes per parameter     */
        public const uint MAX_SERIALNO_LEN = 255;      /* Maximum number of bytes for serail number */

        public const ushort PARAM_USE_HID = 1004;
        public const ushort PARAM_USE_HID_OLD = 122;
        public const ushort PARAM_CLEAR_MEMORY = 806;
        public const ushort IMAGE_FILETYPE_PARAMNUM = 0x0130;   /* These values may change with the scanner  */
        public const ushort BMP_FILE_SELECTION = 0x0003;   /* models. Please refer scanner PRGs for     */
        public const ushort TIFF_FILE_SELECTION = 0x0004;   /* more information on scanner parameters.   */
        public const ushort JPEG_FILE_SELECTION = 0x0001;
        public const ushort VIDEOVIEWFINDER_PARAMNUM = 0x0144;
        public const ushort VIDEOVIEWFINDER_ON = 0x0001;   /* Video view finder on                      */
        public const ushort VIDEOVIEWFINDER_OFF = 0x0000;   /* Video view finder off                     */
        public const int PARAM_PERSISTANCE_ON = 0x0001;   /* Parameters persistance on                 */
        public const int PARAM_PERSISTANCE_OFF = 0x0000;   /* Parameters persistance off                */

        public const int LED_1_ON = 43;   /* Green  LED On                            */
        public const int LED_2_ON = 45;   /* Yellow  LED On                           */
        public const int LED_3_ON = 47;   /* Red  LED On                              */

        public const int LED_1_OFF = 42;   /* Green  LED Off                            */
        public const int LED_2_OFF = 46;   /* Yellow  LED Off                           */
        public const int LED_3_OFF = 48;   /* Red  LED Off                              */

        //****** CORESCANNER PROTOCOL ******//
        public const int GET_VERSION = 1000;
        public const int REGISTER_FOR_EVENTS = 1001;
        public const int UNREGISTER_FOR_EVENTS = 1002;
        public const int GET_PAIRING_BARCODE = 1005;    // Get  Blue tooth scanner pairing bar code
        public const int CLAIM_DEVICE = 1500;
        public const int RELEASE_DEVICE = 1501;
        public const int ABORT_MACROPDF = 2000;
        public const int ABORT_UPDATE_FIRMWARE = 2001;
        public const int DEVICE_AIM_OFF = 2002;
        public const int DEVICE_AIM_ON = 2003;
        public const int FLUSH_MACROPDF = 2005;
        public const int GET_ALL_PARAMETERS = 2006;
        public const int GET_PARAMETERS = 2007;
        public const int DEVICE_GET_SCANNER_CAPABILITIES = 2008;
        public const int DEVICE_LED_OFF = 2009;
        public const int DEVICE_LED_ON = 2010;
        public const int DEVICE_PULL_TRIGGER = 2011;
        public const int DEVICE_RELEASE_TRIGGER = 2012;
        public const int DEVICE_SCAN_DISABLE = 2013;
        public const int DEVICE_SCAN_ENABLE = 2014;
        public const int SET_PARAMETER_DEFAULTS = 2015;
        public const int DEVICE_SET_PARAMETERS = 2016;
        public const int SET_PARAMETER_PERSISTANCE = 2017;
        public const int DEVICE_BEEP_CONTROL = 2018;
        public const int REBOOT_SCANNER = 2019;
        public const int DISCONNECT_BT_SCANNER = 2023;
        public const int DEVICE_CAPTURE_IMAGE = 3000;
        public const int ABORT_IMAGE_XFER = 3001;
        public const int DEVICE_CAPTURE_BARCODE = 3500;
        public const int DEVICE_CAPTURE_VIDEO = 4000;
        public const int RSM_ATTR_GETALL = 5000;
        public const int RSM_ATTR_GET = 5001;
        public const int RSM_ATTR_GETNEXT = 5002;
        public const int RSM_ATTR_SET = 5004;
        public const int RSM_ATTR_STORE = 5005;
        public const int GET_DEVICE_TOPOLOGY = 5006;
        public const int START_NEW_FIRMWARE = 5014;
        public const int UPDATE_ATTRIB_META_FILE = 5015;
        public const int UPDATE_FIRMWARE = 5016;
        public const int UPDATE_FIRMWARE_FROM_PLUGIN = 5017;
        public const int UPDATE_DECODE_TONE = 5050;
        public const int ERASE_DECODE_TONE = 5051;
        public const int SET_ACTION = 6000;

        public const int KEYBOARD_EMULATOR_ENABLE = 6300;//6300
        public const int KEYBOARD_EMULATOR_SET_LOCALE = 6301;   //6301
        public const int KEYBOARD_EMULATOR_GET_CONFIG = 6302;   //6302

        public const int CONFIGURE_DADF = 6400;
        public const int RESET_DADF = 6401;

        // Serial //
        public const int DEVICE_SET_SERIAL_PORT_SETTINGS = 6101;
        // Serial - end //

        // USBHIDKB //
        public const int DEVICE_SWITCH_HOST_MODE = 6200;
        public const int SWITCH_CDC_DEVICES = 6201;
        // USBHIDKB - end //

        //Scale Commands //
        public const int SCALE_READ_WEIGHT = 0x1b58;    //7000
        public const int SCALE_ZERO_SCALE = 0X1B5A; //7002
        public const int SCALE_SYSTEM_RESET = 0X1B67;   //7015
                                                        //Scale Commands //

        //Wave file Buffer Size (Default File Size is 10KB)//
        public const int WAV_FILE_MAX_SIZE = 10240;

        //****** END OF CORESCANNER PROTOCOL *********//
        /**************************************************/

        public enum CodeTypes
        {
            ///<summary>An unknown, with respect to this enumeration, code type</summary>
            Unknown = 0,
            ///<summary>Code 39 symbology</summary>
            Code39 = 1,
            ///<summary>Codabar symbology</summary>
            Codabar = 2,
            ///<summary>Code 128 symbology</summary>
            Code128 = 3,
            ///<summary>Dinscrete 2 of 5 symbology</summary>
            Discrete2of5 = 4,
            ///<summary>IATA symbology</summary>
            Iata = 5,
            ///<summary>Interleaved 2 of 5 symbology</summary>
            Interleaved2of5 = 6,
            ///<summary>Code 93 symbology</summary>
            Code93 = 7,
            ///<summary>UPC-A symbology</summary>
            UpcA = 8,
            ///<summary>UPC-E0 symbology</summary>
            UpcE0 = 9,
            ///<summary>EAN-8 symbology</summary>
            Ean8 = 10,
            ///<summary>EAN-13 symbology</summary>
            Ean13 = 11,
            ///<summary>Code 11symbology</summary>
            Code11 = 12,
            ///<summary>Code 49symbology</summary>
            Code49 = 13,
            ///<summary>MSI Plessey symbology</summary>
            Msi = 14,
            ///<summary>EAN-128 symbology</summary>
            Ean128 = 15,
            ///<summary>UPC-E1 symbology</summary>
            UpcE1 = 16,
            ///<summary>PDF-417 symbology</summary>
            Pdf417 = 17,
            ///<summary>Code 16K symbology</summary>
            Code16K = 18,
            ///<summary>Code 39 symbology, with full-ASCII expansion applied</summary>
            Code39FullAscii = 19,
            ///<summary>UPC-D symbology</summary>
            UpcD = 20,
            ///<summary>symbology</summary>
            Code39Trioptic = 21,
            ///<summary>symbology</summary>
            Bookland = 22,
            ///<summary>symbology</summary>
            CouponCode = 23,
            ///<summary>symbology</summary>
            Nw7 = 24,
            ///<summary>symbology</summary>
            Isbt128 = 25,
            ///<summary>symbology</summary>
            Micro_Pdf = 26,
            ///<summary>symbology</summary>
            DataMatrix = 27,
            ///<summary>symbology</summary>
            QrCode = 28,
            ///<summary>symbology</summary>
            MicroPdfCca = 29,
            ///<summary>symbology</summary>
            PostNetUS = 30,
            ///<summary>symbology</summary>
            PlanetCode = 31,
            ///<summary>symbology</summary>
            Code32 = 32,
            ///<summary>symbology</summary>
            Isbt128Con = 33,
            ///<summary>symbology</summary>
            JapanPostal = 34,
            ///<summary>symbology</summary>
            AustralianPostal = 35,
            ///<summary>symbology</summary>
            DutchPostal = 36,
            ///<summary>symbology</summary>
            MaxiCode = 37,
            ///<summary>symbology</summary>
            CanadianPostal = 38,
            ///<summary>symbology</summary>
            UkPostal = 39,
            ///<summary>symbology</summary>
            MacroPdf = 40,
            ///<summary>symbology</summary>
            Aztec = 45,
            ///<summary>symbology</summary>
            Rss14 = 48,
            ///<summary>symbology</summary>
            RssLimited = 49,
            ///<summary>symbology</summary>
            RssExpanded = 50,
            ///<summary>symbology</summary>
            Scanlet = 55,
            ///<summary>symbology</summary>
            UpcAPlus2 = 72,
            ///<summary>symbology</summary>
            UpcE0Plus2 = 73,
            ///<summary>symbology</summary>
            Ean8Plus2 = 74,
            ///<summary>symbology</summary>
            Ean13Plus2 = 75,
            ///<summary>symbology</summary>
            UpcE1Plus2 = 80,
            ///<summary>symbology</summary>
            CcaEAN128 = 81,
            ///<summary>symbology</summary>
            CcaEAN13 = 82,
            ///<summary>symbology</summary>
            CcaEAN8 = 83,
            ///<summary>symbology</summary>
            CcaRssExpanded = 84,
            ///<summary>symbology</summary>
            CcaRssLimited = 85,
            ///<summary>symbology</summary>
            CcaRss14 = 86,
            ///<summary>symbology</summary>
            CcaUpcA = 87,
            ///<summary>symbology</summary>
            CcaUpcE = 88,
            ///<summary>symbology</summary>
            CccEAN128 = 89,
            ///<summary>symbology</summary>
            Tlc39 = 90,
            ///<summary>symbology</summary>
            CcbEan128 = 97,
            ///<summary>symbology</summary>
            CcbEan13 = 98,
            ///<summary>symbology</summary>
            CcbEan8 = 99,
            ///<summary>symbology</summary>
            CcbRssExpanded = 100,
            ///<summary>symbology</summary>
            CcbRssLimited = 101,
            ///<summary>symbology</summary>
            CcbRss14 = 102,
            ///<summary>symbology</summary>
            CcbUpcA = 103,
            ///<summary>symbology</summary>
            CcbUpcE = 104,
            ///<summary>symbology</summary>
            SignatureCapture = 105,
            ///<summary>symbology</summary>
            Matrix2of5 = 113,
            ///<summary>symbology</summary>
            Chinese2of5 = 114,
            ///<summary>symbology</summary>
            UpcAPlus5 = 136,
            ///<summary>symbology</summary>
            UpcE0Plus5 = 137,
            ///<summary>symbology</summary>
            Ean8Plus5 = 138,
            ///<summary>symbology</summary>
            Ean13Plus5 = 139,
            ///<summary>symbology</summary>
            UpcE1Plus5 = 144,
            ///<summary>symbology</summary>
            MacroMicroPDF = 154,
            ///<summary>symbology</summary>
            MailMark = 195,
            ///<summary>symbology</summary>
            DotCode = 196,
            ///<summary>The no-decode symbol</summary>
            NoDecode = 255,
        }
        public enum RSMDataTypes
        {
            /// <summary>
            /// Byte ?unsigned char
            /// </summary>
            B,
            /// <summary>
            /// Char ?signed byte
            /// </summary>
            C,
            /// <summary>
            /// Bit Flags
            /// </summary>
            F,
            /// <summary>
            /// WORD ?short unsigned integer (16 bits)
            /// </summary>
            W,
            /// <summary>
            /// SWORD ?short signed integer (16 bits)
            /// </summary>
            I,
            /// <summary>
            /// DWORD ?long unsigned integer (32 bits)
            /// </summary>
            D,
            /// <summary>
            /// SDWORD ?long signed integer (32 bits)
            /// </summary>
            L,
            /// <summary>
            /// Array
            /// </summary>
            A,
            /// <summary>
            /// String
            /// </summary>
            S,
            /// <summary>
            /// Action
            /// </summary>
            X
        }
    }
}
