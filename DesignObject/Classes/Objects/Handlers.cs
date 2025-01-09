using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DesignObject
{
    public class DrawingHandler : DrawingObjBase, IComponent
    {
        [JsonIgnore]
        public MainObj _parent
        {
            get { return base.Parent as MainObj; }
            set { base.Parent = value; }
        }
        [JsonIgnore]
        public override DrawingObjBase Parent
        {
            get { return _parent; }
            set { _parent = (value as MainObj); }
        }
        [JsonIgnore]
        protected float _offset = 6f;
        public enRectSIde enRectSIde { get; set; }
        [JsonIgnore]
        public PointF CenterPoint => (new PointF(Rect.Left + (Rect.Width / 2), Rect.Top + (Rect.Height / 2)));
        public virtual object Clone(MainObj parent)
        {
            Parent = parent;
            return this.MemberwiseClone();
        }
        public override object Clone()
        {
            return this.MemberwiseClone();
        }
        public override void Select(bool fbool = true)
        {
            base.Select(fbool);
            Parent.Select();
        }
        public bool Contains(RectangleF rect)
        {
            return rect.Contains(CenterPoint);
        }
    }
    public class AdjustHandler : DrawingHandler
    {
        public AdjustHandler()
        {
        }
        public AdjustHandler(MainObj parent, enRectSIde side)
        {
            Parent = parent;
            enRectSIde = side;
        }
        [JsonIgnore]
        public override RectangleF Rect
        {
            get
            {
                if (Parent is MainObj main)
                {
                    SizeF size = new SizeF(_offset * 2, _offset * 2);
                    var thickness = main.LineThickness / 2;
                    if (Parent is LineObj line)
                    {
                        switch (enRectSIde)
                        {
                            case enRectSIde.Left:
                                return new RectangleF(new PointF(line.Pt1.X - _offset, line.Pt1.Y - _offset), size);
                            case enRectSIde.Right:
                                return new RectangleF(new PointF(line.Pt2.X - _offset, line.Pt2.Y - _offset), size);
                        }
                    }
                    else
                    {
                        switch (enRectSIde)
                        {
                            case enRectSIde.LT:
                                return new RectangleF(new PointF(Parent.Rect.Left - _offset - thickness, Parent.Rect.Y - _offset - thickness), size);
                            case enRectSIde.RT:
                                return new RectangleF(new PointF(Parent.Rect.Right - _offset + thickness, Parent.Rect.Y - _offset - thickness), size);
                            case enRectSIde.LB:
                                return new RectangleF(new PointF(Parent.Rect.Left - _offset - thickness, Parent.Rect.Y + Parent.Rect.Height - _offset + thickness), size);
                            case enRectSIde.RB:
                                return new RectangleF(new PointF(Parent.Rect.Right - _offset + thickness, Parent.Rect.Y + Parent.Rect.Height - _offset + thickness), size);
                        }
                    }
                }
                return new RectangleF();
            }
        }
        [JsonIgnore]
        public override RectangleF DisplayRect
        {
            get
            {
                if (Parent is MainObj main)
                {
                    SizeF size = new SizeF(_offset * 2, _offset * 2);
                    var thickness = main.LineThickness / 2;

                    if (Parent is LineObj line)
                    {
                        switch (enRectSIde)
                        {
                            case enRectSIde.Left:
                                return new RectangleF(new PointF(line.DisplayPt1.X - _offset, line.DisplayPt1.Y - _offset), size);
                            case enRectSIde.Right:
                                return new RectangleF(new PointF(line.DisplayPt2.X - _offset, line.DisplayPt2.Y - _offset), size);
                        }
                    }
                    else
                    {
                        switch (enRectSIde)
                        {
                            case enRectSIde.LT:
                                return new RectangleF(new PointF(Parent.DisplayRect.Left - _offset - thickness, Parent.DisplayRect.Y - _offset - thickness), size);
                            case enRectSIde.RT:
                                return new RectangleF(new PointF(Parent.DisplayRect.Right - _offset + thickness, Parent.DisplayRect.Y - _offset - thickness), size);
                            case enRectSIde.LB:
                                return new RectangleF(new PointF(Parent.DisplayRect.Left - _offset - thickness, Parent.DisplayRect.Y + Parent.DisplayRect.Height - _offset + thickness), size);
                            case enRectSIde.RB:
                                return new RectangleF(new PointF(Parent.DisplayRect.Right - _offset + thickness, Parent.DisplayRect.Y + Parent.DisplayRect.Height - _offset + thickness), size);
                        }
                    }
                }
                return new RectangleF();
            }
        }
    }
    public class MoveHandler : DrawingHandler
    {
        [JsonIgnore]
        public const float offset = 6f;
        [JsonIgnore]
        public override RectangleF Rect
        {
            get
            {
                switch (enRectSIde)
                {
                    case enRectSIde.Left:
                        return new RectangleF(new PointF(Parent.Rect.X - offset, Parent.Rect.Y - offset), new SizeF(offset * 2, Parent.Rect.Height + (offset * 2)));
                    case enRectSIde.Right:
                        return new RectangleF(new PointF(Parent.Rect.X + Parent.Rect.Width - offset, Parent.Rect.Y - offset), new SizeF(offset * 2, Parent.Rect.Height + (offset * 2)));
                    case enRectSIde.Top:
                        return new RectangleF(new PointF(Parent.Rect.X - offset, Parent.Rect.Y - offset), new SizeF(Parent.Rect.Width + (offset * 2), offset * 2));
                    case enRectSIde.Bottom:
                        return new RectangleF(new PointF(Parent.Rect.X - offset, Parent.Rect.Y + Parent.Rect.Height - offset), new SizeF(Parent.Rect.Width + (offset * 2), offset * 2));
                }
                return new RectangleF();
            }
        }
        [JsonIgnore]
        public override RectangleF DisplayRect
        {
            get
            {
                switch (enRectSIde)
                {
                    case enRectSIde.Left:
                        return new RectangleF(new PointF(Parent.DisplayRect.X - offset, Parent.DisplayRect.Y - offset), new SizeF(offset * 2, Parent.DisplayRect.Height + (offset * 2)));
                    case enRectSIde.Right:
                        return new RectangleF(new PointF(Parent.DisplayRect.X + Parent.DisplayRect.Width - offset, Parent.DisplayRect.Y - offset), new SizeF(offset * 2, Parent.DisplayRect.Height + (offset * 2)));
                    case enRectSIde.Top:
                        return new RectangleF(new PointF(Parent.DisplayRect.X - offset, Parent.DisplayRect.Y - offset), new SizeF(Parent.DisplayRect.Width + (offset * 2), offset * 2));
                    case enRectSIde.Bottom:
                        return new RectangleF(new PointF(Parent.DisplayRect.X - offset, Parent.DisplayRect.Y + Parent.DisplayRect.Height - offset), new SizeF(Parent.DisplayRect.Width + (offset * 2), offset * 2));
                }
                return new RectangleF();
            }
        }
        public MoveHandler()
        { }
        public MoveHandler(MainObj parent, enRectSIde side)
        {
            Parent = parent;
            enRectSIde = side;
        }
    }
    public class CellSizeHandlerCollection : IEnumerable<CellSizeHandler>
    {
        protected readonly List<CellSizeHandler> _item = new List<CellSizeHandler>();
        public int Count { get { return _item.Count; } }
        public CellSizeHandler this[int col, int row]
        {
            get
            {
                var item = _item.FirstOrDefault(q => q.CellMatrix.Col.Equals(col) && q.CellMatrix.Row.Equals(row));
                return item;
            }
        }
        public void Add(TableObj parent, CellMatrix cellMatrix, enHandlerMatrix matrix)
        {
            _item.Add(new CellSizeHandler(parent, cellMatrix, matrix));
        }
        public IEnumerator<CellSizeHandler> GetEnumerator()
        {
            return ((IEnumerable<CellSizeHandler>)_item).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_item).GetEnumerator();
        }
    }
    public class CellSizeHandler : MoveHandler
    {
        public new TableObj Parent { get { return base.Parent as TableObj; } set { base.Parent = value; } }
        [JsonIgnore]
        public override RectangleF Rect
        {
            get
            {
                PointF location = new PointF(Location.X - (offset / 2), Location.Y - (offset / 2));
                switch (enHandlerMatrix)
                {
                    case enHandlerMatrix.Row:
                        {
                            float width = Parent.ColWidth[CellMatrix.Col];
                            return new RectangleF(location, new SizeF(width + offset, offset));
                        }
                    case enHandlerMatrix.Column:
                        {
                            float height = Parent.RowHeight[CellMatrix.Row];
                            return new RectangleF(location, new SizeF(offset, height + offset));
                        }
                }
                return new RectangleF(location, new SizeF(0, 0));
            }
        }
		[JsonIgnore]
		public PointF[] DisplayPoints
		{
			get
			{
				switch (enHandlerMatrix)
				{
					case enHandlerMatrix.Row:
						float width = Parent.ColWidth[CellMatrix.Col];
                        var x1 = Location.X + Paper.PaperRect.X;
                        var y = Location.Y + Paper.PaperRect.Y;
						var x2 = x1 + width;

						return new PointF[2] { new PointF(x1, y), new PointF(x2, y) };
						break;
					case enHandlerMatrix.Column:
						float height = Parent.RowHeight[CellMatrix.Row];
						var x = Location.X + Paper.PaperRect.X;
						var y1 = Location.Y + Paper.PaperRect.Y;
						var y2 = y1 + height;

						return new PointF[2] { new PointF(x, y1), new PointF(x, y2) };
						break;
				}
				return Array.Empty<PointF>();
			}
		}
        [JsonIgnore]
        public enHandlerMatrix enHandlerMatrix { get; set; }
        public PointF Location
        {
            get
            {
                float X = 0, Y = 0;
                switch (enHandlerMatrix)
                {
                    case enHandlerMatrix.Row:
                        {
                            X = Parent.ColPosition[CellMatrix.Col];
                            Y = Parent.RowPosition[CellMatrix.Row];
                        }
                        break;
                    case enHandlerMatrix.Column:
                        {
                            X = Parent.ColPosition[CellMatrix.Col];
                            Y = Parent.RowPosition[CellMatrix.Row];
                        }
                        break;
                    default:
                        break;
                }

                return new PointF(X, Y);
            }
        }

        public CellMatrix CellMatrix { get; set; }
        public CellSizeHandler(TableObj parent, CellMatrix cellMatrix, enHandlerMatrix matrix)
        {
            base.Parent = parent;
            CellMatrix = cellMatrix;
            enHandlerMatrix = matrix;
        }
    }

}