using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DesignObject
{
    [Serializable]
    public class CellObj : DrawingObjBase, IWritable, IComponent, IDisplayRect, IDocumentParam
    {
        [JsonIgnore]
        public TableObj Table
        {
            get { return Parent as TableObj; }
        }
        [JsonIgnore]
        float Width
        {
            get
            {
                return Right - Left;
            }
        }
        [JsonIgnore]
        float Height
        {
            get
            {
                return Bottom - Top;
            }
        }
        [JsonIgnore]
        PointF Location
        {
            get
            {
                return new PointF(Left, Top);
            }
        }
		[JsonIgnore] float _Left;
		[JsonIgnore] float _Right;
		[JsonIgnore] float _Top;
        [JsonIgnore] float _Bottom;

		[JsonIgnore] float Left { get => _Left; set => _Left = (int)Math.Round(value, 0); }
        [JsonIgnore] float Right { get => _Right; set => _Right = (int)Math.Round(value, 0); }
        [JsonIgnore] float Top { get => _Top; set => _Top = (int)Math.Round(value, 0); }
        [JsonIgnore] float Bottom { get => _Bottom; set => _Bottom = (int)Math.Round(value, 0); }
        SizeF Size
        {
            get
            {
                return new SizeF(Width, Height);
            }
        }
        [JsonIgnore]
        public override RectangleF Rect
        {
            get
            {
                Left = Table.ColSizeHandler[LeftIndex, CellMatrix.Row]?.Location.X ?? Table.Rect.Left;
                Right = Table.ColSizeHandler[RightIndex, CellMatrix.Row]?.Location.X ?? Table.Rect.Right;
                Top = Table.RowSizeHandler[CellMatrix.Col, TopIndex]?.Location.Y ?? Table.Rect.Top;
                Bottom = Table.RowSizeHandler[CellMatrix.Col, BottomIndex]?.Location.Y ?? Table.Rect.Bottom;
                return new RectangleF(Location, Size);
            }
        }
        [JsonIgnore]
        public RectangleF DisplayRect
        {
            get
            {
                PointF pt = new PointF(Rect.Left + Paper.PaperRect.X, Rect.Top + Paper.PaperRect.Y);
                return new RectangleF(pt, Size);
            }
        }
        [JsonIgnore]
        public PointF CenterPoint => (new PointF(DisplayRect.Left + (DisplayRect.Width / 2), DisplayRect.Top + (DisplayRect.Height / 2)));
		[JsonIgnore]
		public object TagParam { get => Text.TagParam; set => Text.TagParam = value; }
		public TextObj Text { get; set; }
        public int LineThickness { get; set; } = 1;
        public int LeftIndex { get; set; }
        public int TopIndex { get; set; }
        public int RightIndex { get; set; }
        public int BottomIndex { get; set; }

        public MainObj MainObj { get; set; }

		public CellMatrix CellMatrix { get; set; }
        public Color Color { get; set; }

        public CellObj()
        { }
        public CellObj(TableObj parent, CellMatrix cellMatrix)
        {
            Parent = parent;
            CellMatrix = cellMatrix;
            LeftIndex = CellMatrix.Col;
            TopIndex = CellMatrix.Row;
            RightIndex = CellMatrix.Col + 1;
            BottomIndex = CellMatrix.Row + 1;
			Text = new TextObj(this);
        }
        public CellObj(TableObj parent, CellObj cellObj)
        {
            Parent = parent;
            CellMatrix = cellObj.CellMatrix;
            LeftIndex = cellObj.LeftIndex;
            TopIndex = cellObj.TopIndex;
            RightIndex = cellObj.RightIndex;
            BottomIndex = cellObj.BottomIndex;
			Text = new TextObj(this, cellObj.Text);
			Text.String = cellObj.Text.String;
			Text.StringFormat = cellObj.Text.StringFormat;
            TagParam = cellObj.Text.TagParam;
        }
        public override void Select(bool select)
        {
            Parent.Select(select);
            base.Select(select);
            //if (Table.CellObjs.Count(q => q.IsSelected) > 1 && select)
            //    Table.CellObjs.ForEach(q => q.SetFocus(false));
        }
        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public bool Contains(RectangleF rect)
        {
            return rect.Contains(CenterPoint);
        }
    }
}