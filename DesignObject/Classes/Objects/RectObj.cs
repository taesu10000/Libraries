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
    public class RectObj : MainObj, IFigures
    {
        [JsonIgnore]
        public GraphicsPath GraphicsPath
        {
            get
            {
                GraphicsPath path = new GraphicsPath();
                path.AddRectangle(Rect);
                return path;
            }
        }
        [JsonIgnore]
        public GraphicsPath DisplayGraphicsPath
        {
            get
            {
                GraphicsPath path = new GraphicsPath();
                path.AddRectangle(DisplayRect);
                return path;
            }
        }
        public RectObj()
        {
        }
        public RectObj(RectangleF objectRect)
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
        public RectObj(RectObj rectObj)
        {
            Location = rectObj.Location;
            LineThickness = rectObj.LineThickness;
            Size = rectObj.Size;
            ZPos = rectObj.ZPos;

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
            int clickOffset = 3;
            RectangleF outer = new RectangleF(new PointF(DisplayRect.X - clickOffset, DisplayRect.Y - clickOffset), new SizeF(DisplayRect.Width + (clickOffset * 2), DisplayRect.Height + (clickOffset * 2)));
            RectangleF inner = new RectangleF(new PointF(DisplayRect.X + clickOffset, DisplayRect.Y + clickOffset), new SizeF(DisplayRect.Width - (clickOffset * 2), DisplayRect.Height - (clickOffset * 2)));
            return outer.Contains(pt) && inner.Contains(pt) == false;
        }
    }
}
