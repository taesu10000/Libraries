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
    public class CircleObj : MainObj, IFigures
    {
        [JsonIgnore]
        public GraphicsPath GraphicsPath
        {
            get
            {
                GraphicsPath path = new GraphicsPath();
                path.AddEllipse(Rect);
                return path;
            }
        }
        [JsonIgnore]
        public GraphicsPath DisplayGraphicsPath
        {
            get
            {
                GraphicsPath path = new GraphicsPath();
                path.AddEllipse(DisplayRect);
                return path;
            }
        }
        public CircleObj()
        {
        }
        public CircleObj(RectangleF objectRect)
        {
            Location = new PointF(objectRect.Location.X - Paper.PaperRect.Location.X, objectRect.Location.Y - Paper.PaperRect.Location.Y);
            Size = objectRect.Size;

            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.LT));
            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.RT));
            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.LB));
            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.RB));

            this.MoveHandlers.Add(new MoveHandler(this, enRectSIde.Top));
            this.MoveHandlers.Add(new MoveHandler(this, enRectSIde.Bottom));
            this.MoveHandlers.Add(new MoveHandler(this, enRectSIde.Left));
            this.MoveHandlers.Add(new MoveHandler(this, enRectSIde.Right));
        }
        public CircleObj(CircleObj circle)
        {
            Location = circle.Location;
            LineThickness = circle.LineThickness;
            Size = circle.Size;
            ZPos = circle.ZPos;

            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.LT));
            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.RT));
            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.LB));
            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.RB));

            this.MoveHandlers.Add(new MoveHandler(this, enRectSIde.Top));
            this.MoveHandlers.Add(new MoveHandler(this, enRectSIde.Bottom));
            this.MoveHandlers.Add(new MoveHandler(this, enRectSIde.Left));
            this.MoveHandlers.Add(new MoveHandler(this, enRectSIde.Right));
        }
        public override bool Contains(PointF pt)
        {
            float rmajorAxis = DisplayRect.Width / 2;
            float rminorAxis = DisplayRect.Height / 2;
            double f1Length, f2Length;
            double x = 0f, y = 0f;
            double a = 0;
            int _ellipseC = 0;
            PointF centerPt = new PointF(DisplayRect.X + DisplayRect.Width / 2, DisplayRect.Y + DisplayRect.Height / 2);
            if (rmajorAxis > rminorAxis)
            {
                _ellipseC = (int)Math.Sqrt(rmajorAxis * rmajorAxis - rminorAxis * rminorAxis);

                // x장축, y단축
                x = Math.Abs((centerPt.X - _ellipseC) - pt.X);
                y = Math.Abs(centerPt.Y - pt.Y);
                f1Length = Math.Sqrt(x * x + y * y);

                x = Math.Abs((centerPt.X + _ellipseC) - pt.X);
                f2Length = Math.Sqrt(x * x + y * y);
                a = (2 * rmajorAxis) - (f1Length + f2Length);
            }
            else
            {
                _ellipseC = (int)Math.Sqrt(rminorAxis * rminorAxis - rmajorAxis * rmajorAxis);

                // x단축, y장축
                y = Math.Abs((centerPt.Y - _ellipseC) - pt.Y);
                x = Math.Abs(pt.X - centerPt.X);
                f1Length = Math.Sqrt(x * x + y * y);

                y = Math.Abs((centerPt.Y + _ellipseC) - pt.Y);
                f2Length = Math.Sqrt(x * x + y * y);
                a = (2 * rminorAxis) - (f1Length + f2Length);
            }

            if (Math.Abs(a) < 3)
                return true;
            return false;
        }
    }
}
