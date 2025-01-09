using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using ZXing;
using ZXing.Common;

namespace DesignObject
{
    [Serializable]
    public class MainObj : DrawingObjBase, IDisplayRect
    {
        [JsonIgnore]
        public override RectangleF Rect
        {
            get
            {
                return new RectangleF(Location, Size);
            }
        }
        [JsonIgnore]
        public override RectangleF DisplayRect
        {
            get
            {
                float x = Location.X + Paper.PaperRect.X;
                float y = Location.Y + Paper.PaperRect.Y;
                float width = Size.Width;
                float height = Size.Height;
                return new RectangleF(new PointF(x, y), Size);
            }
        }
        [JsonIgnore]
        public float DisplayLeft { get { return DisplayRect.Location.X; } }
        [JsonIgnore]
        public float DisplayRight { get { return DisplayRect.Location.X + Size.Width; } }
        [JsonIgnore]
        public float DisplayTop { get { return DisplayRect.Location.Y; } }
        [JsonIgnore]
        public float DisplayBottom { get { return DisplayRect.Location.Y + Size.Height; } }
        [JsonIgnore]
        public List<AdjustHandler> AdjustHandlers { get; set; } = new List<AdjustHandler>();
        [JsonIgnore]
        public List<MoveHandler> MoveHandlers { get; set; } = new List<MoveHandler>();
        [JsonIgnore]
        public List<DrawingHandler> AllHandlers { get { return AdjustHandlers.OfType<DrawingHandler>().Union(MoveHandlers.OfType<DrawingHandler>()).ToList(); } }
        [JsonIgnore]
        public virtual List<DrawingObjBase> Components
        {
            get
            {
                return AdjustHandlers.Union<DrawingObjBase>(MoveHandlers).ToList();
            }
        }
        [JsonIgnore]
        public virtual GraphicsPath GraphicsPath
        {
            get
            {
                GraphicsPath path = new GraphicsPath();
                path.AddRectangle(Rect);
                return path;
            }
        }
        [JsonIgnore]
        public virtual GraphicsPath DisplayGraphicsPath
        {
            get
            {
                GraphicsPath path = new GraphicsPath();
                path.AddRectangle(DisplayRect);
                return path;
            }
        }
        [JsonIgnore]
        public GraphicsPath HandlerGraphicsPath
        {
            get
            {
                GraphicsPath path = new GraphicsPath();
                path.AddRectangles(this.AdjustHandlers.Select(x => x.Rect).ToArray());
                return path;
            }
        }
        [JsonIgnore]
        public GraphicsPath DisplayHandlerGraphicsPath
        {
            get
            {
                GraphicsPath path = new GraphicsPath();
                path.AddRectangles(this.AdjustHandlers.Select(x => x.DisplayRect).ToArray());
                return path;
            }
        }
        [JsonIgnore]
        public int _angle { get; set; } = 0;
        public int Angle
        {
            get { return _angle; }
            set
            {
                if (value > 360)
                    _angle = Math.Abs(value) - 360;
                else if (value < 0)
                    _angle = 360 - Math.Abs(value);
                else if (value == 360)
                    _angle = 0;
                else
                    _angle = value;
            }
        }
        public PointF Location { get; set; }
        public SizeF Size { get; set; }
        public bool IsLocked { get; set; } = false;
        public int LineThickness { get; set; } = 1;
		public int ZPos { get; set; } = 0;
		public override void Select(bool fbool = true)
        {
            base.Select(fbool);
        }
        public virtual AdjustHandler AdjustHandler(PointF pt)
        {
            return this.AdjustHandlers.FirstOrDefault(q => q.Rect.Contains(pt));
        }
        public virtual void MoveMainObj(PointF offset)
        {
            if (IsLocked == true)
                return;

            Location = new PointF(offset.X, offset.Y);
            Size = Rect.Size;
        }
        public void ReSize(RectangleF rect)
        {
            if (IsLocked == true)
                return;


            Location = GetRectOnPaper(rect).Location;
            if (this is TableObj tb)
            {
                float width = rect.Size.Width / Size.Width;
                float height = rect.Size.Height / Size.Height;
                for (int i = 0; i < tb.ColWidth.Count; i++)
                {
                    tb.ColWidth[i] *= width;
                }
                for (int i = 0; i < tb.RowHeight.Count; i++)
                {
                    tb.RowHeight[i] *= height;
                }
                Size = rect.Size;
            }
            else
            {
                Size = rect.Size;
                if (this is BarcodeObj)
                {
                    var obj = this as BarcodeObj;
                    obj.DrawBarcode();
                }
                else if (this is ImageObj)
                {
                    var obj = this as ImageObj;
                    obj.DrawImage();
                }
            }
        }
        public RectangleF GetRectOnPaper(RectangleF rect)
        {
            var location = new PointF(rect.Location.X - Paper.PaperRect.X, rect.Location.Y - Paper.PaperRect.Y);
            return new RectangleF(location, rect.Size);
        }
        public virtual void Rotate() { }
        public override object Clone()
        {
            object clone = null;
            if (this is TableObj tableObj)
            {
                clone = new TableObj(tableObj);
            }
            else if (this is BarcodeObj barcodeObj)
            {
                clone = new BarcodeObj(barcodeObj);
            }
            else if (this is TextBoxObj textObj)
            {
                clone = new TextBoxObj(textObj);
            }
            else if (this is CircleObj circleObj)
            {
                clone = new CircleObj(circleObj);
            }
            else if (this is RectObj rectObj)
            {
                clone = new RectObj(rectObj);
            }
            else if (this is ImageObj imageObj)
            {
                clone = new ImageObj(imageObj);
            }
            else if (this is LineObj lineObj)
            {
                clone = new LineObj(lineObj);
            }

            return clone;
        }
        public virtual bool Contains(PointF pt)
        {
            return DisplayRect.Contains(pt);
        }
    }
}
