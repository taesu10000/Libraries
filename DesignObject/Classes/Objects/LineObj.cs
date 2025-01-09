using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignObject
{
    [Serializable]
    public class LineObj : MainObj, IFigures
    {
        protected float _offset = 6f;
        [JsonIgnore]
        public PointF Pt1
        {
            get
            {
                return new PointF(Location.X, Location.Y);
            }
            set { Location = new PointF(value.X, value.Y); }
        }
        [JsonIgnore]
        public PointF DisplayPt1
        {
            get
            {
                return new PointF(Location.X + Paper.PaperRect.Left, Location.Y + Paper.PaperRect.Top);
            }
            set { Location = new PointF(value.X - Paper.PaperRect.Left, value.Y - Paper.PaperRect.Top); }
        }
        [JsonIgnore]
        public PointF Pt2
        {
            get
            {
                return new PointF(Location.X + Size.Width, Location.Y + Size.Height);
            }
        }
        [JsonIgnore]
        public PointF DisplayPt2
        {
            get
            {
                return new PointF(Location.X + Size.Width + Paper.PaperRect.Left, Location.Y + Size.Height + Paper.PaperRect.Top);
            }
        }
        [JsonIgnore]
        public override RectangleF Rect
        {
            get
            {
                float left = Math.Min(Pt1.X, Pt2.X);
                float right = Math.Max(Pt1.X, Pt2.X);
                float top = Math.Min(Pt1.Y, Pt2.Y);
                float bottom = Math.Max(Pt1.Y, Pt2.Y);
                var rectf = RectangleF.FromLTRB(left, top, right, bottom);
                if (rectf.Width == 0)
                    rectf = RectangleF.FromLTRB(left - (_offset / 2), top, right + (_offset / 2), bottom);
                else if (rectf.Height == 0)
                    rectf = RectangleF.FromLTRB(left, top - (_offset / 2), right, bottom + (_offset / 2));

                return rectf;
            }
        }
        [JsonIgnore]
        public override RectangleF DisplayRect
        {
            get
            {
                float left = Math.Min(DisplayPt1.X, DisplayPt2.X);
                float right = Math.Max(DisplayPt1.X, DisplayPt2.X);
                float top = Math.Min(DisplayPt1.Y, DisplayPt2.Y);
                float bottom = Math.Max(DisplayPt1.Y, DisplayPt2.Y);

                var rectf = RectangleF.FromLTRB(left, top, right, bottom);
                if (rectf.Width == 0)
                    rectf = RectangleF.FromLTRB(left - (_offset / 2), top + (_offset / 2), right, bottom);
                else if (rectf.Height == 0)
                    rectf = RectangleF.FromLTRB(left, top, right - (_offset / 2), bottom + (_offset / 2));

                return RectangleF.FromLTRB(left, top, right, bottom);
            }
        }
        [JsonIgnore]
        public GraphicsPath GraphicsPath
        {
            get
            {
                GraphicsPath path = new GraphicsPath();
                path.AddLine(Pt1, Pt2);
                return path;
            }
        }
        [JsonIgnore]
        public GraphicsPath DisplayGraphicsPath
        {
            get
            {
                GraphicsPath path = new GraphicsPath();
                path.AddLine(DisplayPt1, DisplayPt2);
                return path;
            }
        }
        public LineObj()
        {
        }
        public LineObj(PointF pt1, PointF pt2)
        {
            float left = Math.Min(pt1.X, pt2.X);
            float right = Math.Max(pt1.X, pt2.X);
            float top = Math.Min(pt1.Y, pt2.Y);
            float bottom = Math.Max(pt1.Y, pt2.Y);

            Pt1 = new PointF(pt1.X, pt1.Y);
            Size = new Size(Convert.ToInt32(right - left), Convert.ToInt32(bottom - top));
            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.Left));
            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.Right));
        }
        public LineObj(LineObj line)
        {
            Size = line.Size;
            ZPos = line.ZPos;
            Pt1 = line.Pt1;
            LineThickness = line.LineThickness;
            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.Left));
            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.Right));
        }
        public void ReSize(PointF pt1, PointF pt2)
        {
            float left = Math.Min(pt1.X, pt2.X);
            float right = Math.Max(pt1.X, pt2.X);
            float top = Math.Min(pt1.Y, pt2.Y);
            float bottom = Math.Max(pt1.Y, pt2.Y);

            Pt1 = new Point((int)left, (int)top);
            Size = new Size(Convert.ToInt32(right - left), Convert.ToInt32(bottom - top));
        }
        public override void MoveMainObj(PointF offset)
        {
            Pt1 = new PointF(offset.X, offset.Y);
            //Pt2 = new PointF(Pt2.X + offset.X, Pt2.Y + offset.Y);
        }
        public override bool Contains(PointF pt)
        {
            double diameter1 = Math.Sqrt(Math.Pow(DisplayRect.Width, 2) + Math.Pow(DisplayRect.Height, 2));
            double diameter2 = Math.Sqrt(Math.Pow(pt.X - DisplayRect.Left, 2) + Math.Pow(DisplayRect.Bottom - pt.Y, 2))
                    + Math.Sqrt(Math.Pow((DisplayRect.Right - pt.X), 2) + Math.Pow(pt.Y - DisplayRect.Top, 2)); //1,3사분면

            double angle = Math.Atan2(Pt1.Y - Pt2.Y, Pt2.X - Pt1.X);
            double degree = angle * 180 / Math.PI;
            if ((degree > 90 && degree < 180) || (degree > -90 && degree < 0))
            {
                diameter2 = Math.Sqrt(Math.Pow(pt.X - DisplayRect.Left, 2) + Math.Pow(pt.Y - DisplayRect.Top, 2))
                    + Math.Sqrt(Math.Pow((DisplayRect.Right - pt.X), 2) + Math.Pow(DisplayRect.Bottom - pt.Y, 2)); //2,4사분면
            }

            return Math.Abs(diameter1 - diameter2) < 0.5;
        }
    }
}
