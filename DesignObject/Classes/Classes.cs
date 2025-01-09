using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace DesignObject
{
    public class Paper
	{
		[JsonIgnore]
		float _zoomRatio = 1.25f;
		[JsonIgnore]
		SizeF _sizePx = new SizeF(); 
        [JsonIgnore]
		enPaperSizeType _enPaperSize = 0;
		[JsonIgnore]
        public PaperDisplayMode _displayMode = PaperDisplayMode.Default;
        [JsonIgnore]
        public int _angle = 0;
        [JsonIgnore]
        public PointF _location = new PointF();
        [JsonIgnore]
        public Unit _unit = Unit.mm;
        public float ZoomRatio
		{
			get { return _zoomRatio; }
			set
			{
				if (value < 0.5f)
					_zoomRatio = 0.5f;
				else if (value > 10f)
					_zoomRatio = 10f;
				else if (value >= 0.9f && value <= 1.1f)
					_zoomRatio = 1f;
				else
					_zoomRatio = value;
			}
		}
		public RectangleF PaperRect
        {
            get
            {
                return new RectangleF(Location, SizePx);
            }
        }
		public SizeF SizePx { get => _sizePx; set => _sizePx = value; }
		[JsonIgnore]
        public PointF Location
        {
            get
            {
                //_displayMode = PaperDisplayMode.Print;
                PointF pt = _location;
                if (Convert.ToBoolean(DisplayMode & PaperDisplayMode.Wheel))
                    pt = new PointF(_location.X + WheelOffetX, _location.Y + WheelOffetY);

                if (Convert.ToBoolean(DisplayMode & PaperDisplayMode.Rotation))
                {
                    switch (Angle)
                    {
                        case 90:
                            pt = new PointF(pt.X + SizePx.Height, pt.Y);
                            break;
                        case 180:
                            pt = new PointF(pt.X + SizePx.Width, pt.Y + SizePx.Height);
                            break;
                        case 270:
                            pt = new PointF(pt.X, pt.Y + SizePx.Width);
                            break;
                    }
                }
                return pt;
            }
            set
            {
                _location = value;
            }
        }
		public enPaperSizeType enPaperSize { get => _enPaperSize; set => _enPaperSize = value; }
		public PaperDisplayMode DisplayMode { get => _displayMode; set => _displayMode = value; }
        public int Angle
        {
            get
            { return _angle; }
            set
            {
                if (value >= 360)
                    _angle = value - 360;
                else if (value < 0)
                    _angle = value + 360;
                else
                    _angle = value;
            }
        }
        [JsonIgnore]
        public float WheelOffetX = 0;
        [JsonIgnore]
        public float WheelOffetY = 0;
        public Unit Unit
        {
            get { return _unit; }
            set 
            {
                _unit = value;
            }
        }
        public Paper()
        { }
        public Paper(SizeMm sizemm)
        {
            Location = new PointF();
            SizePx = sizemm.MilliimeterToPixel();
            enPaperSize = enPaperSizeType.Custom;
        }        
        public Paper(SizeInch sizeInch)
        {
            Location = new PointF();
            SizePx = sizeInch.ToMm().MilliimeterToPixel();
            enPaperSize = enPaperSizeType.Custom;
            Unit = Unit.inch;
        }
        public Paper(RectangleF rect)
        {
            Location = rect.Location;
            SizePx = rect.Size;
            enPaperSize = enPaperSizeType.Custom;
        }
        public Paper(enPaperSizeType size)
        {
            enPaperSize = size;
            SizePx = GetPaper(enPaperSize).Size;
        }
        public RectangleF GetPaper(enPaperSizeType paperSize)
        {
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                enPaperSize = paperSize;
                switch (enPaperSize)
                {
                    case enPaperSizeType.A0:
                        //33.1 × 46.8inch
                        return new RectangleF(new PointF(0, 0), new SizeF(33.1f * g.DpiX, 36.8f * g.DpiY));
                    case enPaperSizeType.A1:
                        //23.4 × 33.1inch
                        return new RectangleF(new PointF(0, 0), new SizeF(23.4f * g.DpiX, 33.1f * g.DpiY));
                    case enPaperSizeType.A2:
                        //16.5 × 23.4
                        return new RectangleF(new PointF(0, 0), new SizeF(16.5f * g.DpiX, 23.4f * g.DpiY));
                    case enPaperSizeType.A3:
                        //11.7 × 16.5
                        return new RectangleF(new PointF(0, 0), new SizeF(11.7f * g.DpiX, 16.5f * g.DpiY));
                    case enPaperSizeType.A4:
                        //8.3 × 11.7
                        return new RectangleF(new PointF(0, 0), new SizeF(8.3f * g.DpiX, 11.7f * g.DpiY));
                    case enPaperSizeType.A5:
                        //5.8 × 8.3
                        return new RectangleF(new PointF(0, 0), new SizeF(5.8f * g.DpiX, 8.5f * g.DpiY));
                    default:
                        return new RectangleF();
                }
            }
        }
        public Paper Clone()
        {
            return this.MemberwiseClone() as Paper;
        }
    }
    public class PrinterSettingProp
    {
        public string PrinterName { get; set; }
        public int NumberToPrint { get; set; }
    }
    [Serializable]
    public struct CellMatrix
    {
        public int Row;
        public int Col;
        public CellMatrix(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public override bool Equals(object obj)
        {
            return obj is CellMatrix matrix &&
                   Row == matrix.Row &&
                   Col == matrix.Col;
        }

        public override int GetHashCode()
        {
            int hashCode = 1084646500;
            hashCode = hashCode * -1521134295 + Row.GetHashCode();
            hashCode = hashCode * -1521134295 + Col.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(CellMatrix cm1, CellMatrix cm2)
        {
            return cm1.Row == cm2.Row && cm1.Col == cm2.Col;
        }
        public static bool operator !=(CellMatrix cm1, CellMatrix cm2)
        {
            return cm1.Row != cm2.Row || cm1.Col != cm2.Col;
        }
    }
    public class CtrlCharConverter
    {
        static Dictionary<string, string> _list = new Dictionary<string, string>()
        {
            { "{GS}", ((char)29).ToString() },
        };
        public static string CtrlCharToStr(string barcode)
        {
            foreach (var item in _list.Keys)
            {
                barcode = barcode.Replace(_list[item], item);
            }
            return barcode;
        }
        public static string StrToCtrlChar(string barcode)
        {
            foreach (var item in _list.Keys)
            {
                barcode = barcode.Replace(item, _list[item]);
            }
            return barcode;
        }
    }
    public struct SizeMm
    {
        public float X { get; set; }
        public float Y { get; set; }
        public SizeMm(float x, float y)
        { 
            X = x;
            Y = y;
        }
        public SizeInch ToInch()
        {
            float x = X.mmToinch();
            float y = Y.mmToinch();
            SizeInch sizeInch = new SizeInch(x, y);
            return sizeInch;
        }
    }
    public struct SizeInch
    {
        public float X { get; set; }
        public float Y { get; set; }
        public SizeInch(float x, float y)
        {
            X = x;
            Y = y;
        }
        public SizeMm ToMm()
        {
            float x = X.inchTomm();
            float y = Y.inchTomm();
            SizeMm sizeMm = new SizeMm(x, y);
            return sizeMm;
        }
    }
}
