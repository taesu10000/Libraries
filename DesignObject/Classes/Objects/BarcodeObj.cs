using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing.Common;
using ZXing;

namespace DesignObject
{
    [Serializable]
    public class BarcodeObj : MainObj, IBarcodeProp, IDocumentParam
    {
        [JsonIgnore]
        public char GroupSeparator
        {
            get
            {
                if (IsGS1)
                {
                    Enum.TryParse(_barcodeType, out enBarcodeType barcodeType);
                    if (Convert.ToBoolean(enBarcodeType.OneDBarcode & barcodeType))
                        return (char)0x00F1;
                    else
                        return (char)29;
                }
                else
                    return '\0';
            }
        }
        [JsonIgnore]
        public BarcodeFormat BarcodeFormat
        {
            get
            {
                Enum.TryParse(_barcodeType, out enBarcodeType barcodeType);
                switch (barcodeType)
                {
                    case enBarcodeType.Code128:
                        return BarcodeFormat.CODE_128;
                    case enBarcodeType.QR:
                        return BarcodeFormat.QR_CODE;
                    case enBarcodeType.ITF:
                        return BarcodeFormat.ITF;
                    case enBarcodeType.DataMatrix:
                    default:
                        return BarcodeFormat.DATA_MATRIX;
                }
            }
            set
            {
                switch (value)
                {
                    case BarcodeFormat.CODE_128:
                        _barcodeType = enBarcodeType.Code128.ToString();
                        break;
                    case BarcodeFormat.QR_CODE:
                        _barcodeType = enBarcodeType.QR.ToString();
                        break;
                    case BarcodeFormat.ITF:
                        _barcodeType = enBarcodeType.ITF.ToString();
                        break;
                    case BarcodeFormat.DATA_MATRIX:
                    default:
                        _barcodeType = enBarcodeType.DataMatrix.ToString();
                        break;
                }
            }
        }
        [JsonIgnore]
        public enBarcodeType BarcodeType
        {
            get
            {
                Enum.TryParse(_barcodeType, out enBarcodeType barcodeType);
                return barcodeType;
            }
            set
            {
                _barcodeType = value.ToString();
            }
        }
        [JsonIgnore]
        public bool IsOneDBarcode
        {
            get
            {
                return Convert.ToBoolean(BarcodeFormat & BarcodeFormat.All_1D);
            }
        }
        [JsonIgnore]
        public Bitmap BarcodeBitmap { get; set; }
        [JsonIgnore]
        public BitMatrix BitMatrix { get; set; }
        [JsonIgnore]
        protected BarcodeWriter _zxing;
        [JsonIgnore]
        BarcodeWriter Zxing
        {
            get
            {
                if (_zxing == null)
                    _zxing = new BarcodeWriter();
                return _zxing;
            }
        }
        [JsonIgnore]
        public string String
        {
            get
            {
                string message = CtrlCharConverter.StrToCtrlChar(_barcodeMessage);
                return message;
            }
            set
            {
                _barcodeMessage = value;
                DrawBarcode();
            }
        }
        public int Get1DBarcodeSize(int min)
        {
            return min * 2;
        }
        public int Get2DBarcodeSize(int min)
        {
            int count = Convert.ToInt32(Math.Ceiling(this.Rect.Width / min));
            return min * count;
        }
        public string _barcodeType { get; set; }
        public string _barcodeMessage { get; set; } = "BarcodeData";
        public bool IsGS1 { get; set; }
        public bool ForceSquare { get; set; } = true;
        public object TagParam { get; set; } = null;
        public bool PrimaryBarcode { get; set; } = false;
        public BarcodeObj()
        { }
        public BarcodeObj(RectangleF objectRect, string barcodeformat, string barcodeMessage, bool isGS1, bool isForceSquare)
        {
            Location = new PointF(objectRect.Location.X - Paper.PaperRect.Location.X, objectRect.Location.Y - Paper.PaperRect.Location.Y);
            float size = objectRect.Size.Width < objectRect.Size.Height ? objectRect.Size.Width : objectRect.Size.Height;
            Size = new SizeF(size, size);
            _barcodeType = barcodeformat;
            _barcodeMessage = barcodeMessage;
            IsGS1 = isGS1;
            ForceSquare = isForceSquare;

            Zxing.Format = BarcodeFormat;
            Zxing.Options.PureBarcode = true;

            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.LT));
            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.RT));
            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.LB));
            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.RB));

            this.MoveHandlers.Add(new MoveHandler(this, enRectSIde.Top));
            this.MoveHandlers.Add(new MoveHandler(this, enRectSIde.Bottom));
            this.MoveHandlers.Add(new MoveHandler(this, enRectSIde.Left));
            this.MoveHandlers.Add(new MoveHandler(this, enRectSIde.Right));
            DrawBarcode();
        }
        public BarcodeObj(BarcodeObj barcode)
        {
            Location = barcode.Location;
            Size = barcode.Size;
            _barcodeType = barcode.GetFormat();
            _barcodeMessage = barcode.GetMessage();
            IsGS1 = barcode.IsGS1;
            PrimaryBarcode = barcode.PrimaryBarcode;
            ForceSquare = barcode.ForceSquare;
            TagParam = barcode.TagParam;
            Zxing.Format = BarcodeFormat;
            Zxing.Options.PureBarcode = true;
            Angle = barcode.Angle;
            ZPos = barcode.ZPos;

            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.LT));
            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.RT));
            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.LB));
            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.RB));

            this.MoveHandlers.Add(new MoveHandler(this, enRectSIde.Top));
            this.MoveHandlers.Add(new MoveHandler(this, enRectSIde.Bottom));
            this.MoveHandlers.Add(new MoveHandler(this, enRectSIde.Left));
            this.MoveHandlers.Add(new MoveHandler(this, enRectSIde.Right));

            DrawBarcode();
        }
        public void DrawBarcode()
        {
            Zxing.Format = BarcodeFormat;
            if (IsOneDBarcode)
            {
                if (Zxing.Options.Hints.ContainsKey(EncodeHintType.MARGIN) == false)
                    Zxing.Options.Hints.Add(EncodeHintType.MARGIN, 0);
            }
            else
            {
                if (ForceSquare)
                {
                    if (Zxing.Options.Hints.ContainsKey(EncodeHintType.DATA_MATRIX_SHAPE) == false)
                        Zxing.Options.Hints.Add(EncodeHintType.DATA_MATRIX_SHAPE, ZXing.Datamatrix.Encoder.SymbolShapeHint.FORCE_SQUARE);
                }
                else
                    Zxing.Options.Hints.Remove(EncodeHintType.DATA_MATRIX_SHAPE);
            }

            var rectWidth = Rect.Width == 0 ? 1 : Math.Abs((int)Rect.Width);
            var rectHeight = Rect.Height == 0 ? 1 : Math.Abs((int)Rect.Height);

            BitMatrix bitMatrix = new BitMatrix(rectWidth, rectHeight);
            Zxing.Options.Width = 0;
            Zxing.Options.Height = 0;


            if (string.IsNullOrEmpty(String) == false)
            {
                var strBarcode = String;
                if (IsGS1) { strBarcode = GroupSeparator + strBarcode; }

                bitMatrix = Zxing.Encode(strBarcode);
                int width = 0;
                int height = 0;
                if (IsOneDBarcode)
                {
                    width = Get1DBarcodeSize(bitMatrix.Width);
                    height = Convert.ToInt32(Rect.Height);
                    if (Angle == 90 || Angle == 270)
                    {
                        width = Get1DBarcodeSize(bitMatrix.Height);
                        height = Convert.ToInt32(bitMatrix.Width);
                    }
                }
                else
                {
                    width = Math.Abs(Get2DBarcodeSize(bitMatrix.Width));
                    height = Math.Abs(Get2DBarcodeSize(bitMatrix.Height));
                }

                Zxing.Options.Width = width;
                Zxing.Options.Height = height;
                BitMatrix = Zxing.Encode(strBarcode);
                if (BitMatrix != null)
                {
                    BarcodeBitmap?.Dispose();
                    BarcodeBitmap = null;
                    BarcodeBitmap = Zxing.Write(BitMatrix);
                    if (Angle != 0)
                    {
                        Bitmap barcode = new Bitmap(BarcodeBitmap.Width, BarcodeBitmap.Height);
                        using (Graphics g = Graphics.FromImage(barcode))
                        {
                            g.RotateTransform(Angle);
                            switch (Angle)
                            {
                                case 90:
                                    g.DrawImage(BarcodeBitmap, 0, -BarcodeBitmap.Height);
                                    break;
                                case 180:
                                    g.DrawImage(BarcodeBitmap, -BarcodeBitmap.Width, -BarcodeBitmap.Height);
                                    break;
                                case 270:
                                    g.DrawImage(BarcodeBitmap, -BarcodeBitmap.Width, 0);
                                    break;
                                case 0:
                                default:
                                    g.DrawImage(BarcodeBitmap, 0, 0);
                                    break;
                            }
                        }
                        BarcodeBitmap?.Dispose();
                        BarcodeBitmap = null;
                        BarcodeBitmap = barcode;
                    }
                }
            }
        }
        public override void Rotate()
        {
            DrawBarcode();
        }
        public string GetFormat()
        {
            return _barcodeType;
        }
        public string GetMessage()
        {
            return _barcodeMessage;
        }
        public override AdjustHandler AdjustHandler(PointF pt)
        {
            return this.AdjustHandlers.FirstOrDefault(q => q.Rect.Contains(pt));
        }
    }

}
