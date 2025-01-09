using Cognex.DataMan.SDK;
using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixedMountScanner
{
    public class StringArrivedEventArgs : EventArgs
    {
        public int ResultId { get; private set; }
        public string ReadString { get; private set; }
        public DateTime InspectionTime { get; }
        public bool Result
        {
            get
            {
                bool result = false;
                if (string.IsNullOrEmpty(ReadString) == false && ReadString.ToLower().Contains("no read") == false)
                    result = true;
                return result;
            }
        }

        internal StringArrivedEventArgs(int resultId, string readString)
        {
            ResultId = resultId;
            ReadString = readString;
        }
        internal StringArrivedEventArgs(ReadStringArrivedEventArgs args)
        {
            ResultId = args.ResultId;
            ReadString = args.ReadString;
        }
        internal StringArrivedEventArgs(XmlResult result)
        {
            ResultId = result.ResultID;
            ReadString = result.FullString;
            InspectionTime = result.GrabTime;
        }
#if DEBUG
        public StringArrivedEventArgs(string readString)
        {
            ReadString = readString;
            InspectionTime = DateTime.Now;
        }
#endif
    }
    public class MultiStringArrivedEventArgs : EventArgs
    {
        public int ResultId { get; private set; }
        public List<string> ReadStrings { get; private set; }
        public bool Result { get; private set; }
        public DateTime InspectionTime { get; }
        internal MultiStringArrivedEventArgs(XmlResult result)
        {
            ResultId = result.ResultID;
            ReadStrings = result.FullString.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            Result = result.Status;
            InspectionTime = result.GrabTime;
        }
    }
    public class ImageReadStringArrivedEventArgs : EventArgs
    {
        string _readBarcode;
        Bitmap _image;
        Bitmap _originalImage;
        int _resultID;
        bool _result;
        internal ImageReadStringArrivedEventArgs(XmlResult result)
        {
            _resultID = result.ResultID;
            _readBarcode = result.FullString;
            _result = result.Status;
            _image = new Bitmap(result.OverlayImage);
            _originalImage = new Bitmap(result.Image);
            InspectionTime = result.GrabTime;
        }
        public string ReadBarcode { get { return _readBarcode; } }
        public Bitmap Image { get { return _image; } }
        public Bitmap OriginalImage { get { return _originalImage; } }
        public int ResultID { get { return _resultID; } }
        public bool Result { get { return _result; } }
        public DateTime InspectionTime { get; }
        public void Dispose()
        {
            _image?.Dispose();
            _originalImage?.Dispose();
        }
    }
    public class ConnectionChangedArgs : EventArgs
    {
        bool _connected;
        public ConnectionChangedArgs(bool connected)
        {
            _connected = connected;
        }
        public bool Connected
        {
            get
            {
                return _connected;
            }
            protected set
            {
                _connected = value;
            }
        }
    }
}
