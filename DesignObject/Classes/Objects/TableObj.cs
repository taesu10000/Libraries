using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignObject
{
    [Serializable]
    public class TableObj : MainObj
    {
        [JsonIgnore]
        public CellSizeHandlerCollection RowSizeHandler { get; set; }
        [JsonIgnore]
        public CellSizeHandlerCollection ColSizeHandler { get; set; }
        [JsonIgnore]
        public List<CellSizeHandler> AllHandlers
        {
            get
            {
                return ColSizeHandler.Union(RowSizeHandler).ToList();
            }
        }
        [JsonIgnore]
        public override bool IsSelected
        {
            get { return CellObjs.Any(q => q.IsSelected) || base.IsSelected; }
        }
        [JsonIgnore]
        public List<float> RowPosition
        {
            get
            {
                List<float> list = new List<float>();
                list.Add(Rect.Top);
                for (int i = 0; i < RowHeight.Count; i++)
                {
                    list.Add(list.Last() + RowHeight[i]);
                }
                return list;
            }
        }
        [JsonIgnore]
        public List<float> ColPosition
        {
            get
            {
                List<float> list = new List<float>();
                list.Add(Rect.Left);
                for (int i = 0; i < ColWidth.Count; i++)
                {
                    list.Add(list.Last() + ColWidth[i]);
                }
                return list;
            }
        }
        [JsonIgnore]
        public override List<DrawingObjBase> Components
        {
            get
            {
                return AdjustHandlers.Union<DrawingObjBase>(MoveHandlers).Union<DrawingObjBase>(ColSizeHandler).Union<DrawingObjBase>(RowSizeHandler).Union<DrawingObjBase>(CellObjs).ToList();
            }
        }
        public List<CellObj> CellObjs { get; set; }
        public int RowCnt { get; set; }
        public int ColCnt { get; set; }
        public List<float> RowHeight { get; set; }
        public List<float> ColWidth { get; set; }
        public TableObj()
        { }
        public TableObj(RectangleF objectRect, int col, int row)
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

            CellObjs = new List<CellObj>();
            RowCnt = row;
            ColCnt = col;
            InitializeMoveHandler();
            for (int row_ = 0; row_ < RowCnt; row_++)
            {
                for (int col_ = 0; col_ < ColCnt; col_++)
                {
                    CellObjs.Add(new CellObj(this, new CellMatrix(row_, col_)));
                }
            }

        }
        public TableObj(TableObj table)
        {
            Location = table.Location;
            Size = table.Size;
            ZPos = table.ZPos;

            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.LT));
            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.RT));
            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.LB));
            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.RB));

            this.MoveHandlers.Add(new MoveHandler(this, enRectSIde.Top));
            this.MoveHandlers.Add(new MoveHandler(this, enRectSIde.Bottom));
            this.MoveHandlers.Add(new MoveHandler(this, enRectSIde.Left));
            this.MoveHandlers.Add(new MoveHandler(this, enRectSIde.Right));

            CellObjs = new List<CellObj>();
            RowCnt = table.RowCnt;
            ColCnt = table.ColCnt;
            ColWidth = table.ColWidth;
            RowHeight = table.RowHeight;
            InitializeMoveHandler(table.ColWidth, table.RowHeight);
            for (int row_ = 0; row_ < RowCnt; row_++)
            {
                for (int col_ = 0; col_ < ColCnt; col_++)
                {
                    CellObj cellObj = table.CellObjs.FirstOrDefault(q => q.CellMatrix.Row.Equals(row_) && q.CellMatrix.Col.Equals(col_));
                    if (cellObj != null)
                        CellObjs.Add(new CellObj(this, cellObj));

                }
            }
        }
        //public CellObj GetObj(PointF pt)
        //{
        //    const int clickOffset = 6;
        //    Func<float, float, float> GetInnerRectSize = ((total, offset) =>
        //    {
        //        float val = total - (offset * 2);
        //        return val < 0 ? 0 : val;
        //    });
        //    Func<float, float, float> GetOuterRectSize = ((total, offset) =>
        //    {
        //        float val = total + (offset * 2);
        //        return val < 0 ? 0 : val;
        //    });

        //    return CellObjs.FirstOrDefault(q =>
        //    {
        //        RectangleF outer = new RectangleF(new PointF(q.RectOnPaper.X - clickOffset, q.RectOnPaper.Y - clickOffset), new SizeF(GetOuterRectSize(q.Rect.Width, clickOffset), GetOuterRectSize(q.Rect.Height, clickOffset)));
        //        return outer.Contains(pt);
        //    });
        //}
        //public MoveHandler GetHandler(PointF pt)
        //{
        //    List<CellSizeHandler> handlerLists = ColSizeHandler.Union(RowSizeHandler).ToList();
        //    foreach (CellSizeHandler handlers in handlerLists)
        //    {
        //        if (handlers.Rect.Contains(pt))
        //            return handlers;
        //    }
        //    return null;
        //}
        public void AdjustCellSize(PointF clickedPt, PointF curPt, CellSizeHandler handler)
        {
            switch (handler.enHandlerMatrix)
            {
                case enHandlerMatrix.Row:
                    {
                        float offset = (int)Math.Round(curPt.Y - clickedPt.Y, 0);
                        RowHeight[handler.CellMatrix.Row - 1] += offset;
                        RowHeight[handler.CellMatrix.Row] -= offset;
                        //Size = new SizeF(Rect.Width, Rect.Height + offset);
                    }
                    break;
                case enHandlerMatrix.Column:
                    {
                        float offset = (int)Math.Round(curPt.X - clickedPt.X, 0);
                        ColWidth[handler.CellMatrix.Col - 1] += offset;
                        ColWidth[handler.CellMatrix.Col] -= offset;
                    }
                    break;
            }
        }
        private void InitializeMoveHandler()
        {
            RowSizeHandler = new CellSizeHandlerCollection();
            ColSizeHandler = new CellSizeHandlerCollection();

            float cOffset = (int)Math.Round(Rect.Width / ColCnt, 0);
            float rOffset = (int)Math.Round(Rect.Height / RowCnt, 0);
            ColWidth = new List<float>();
            RowHeight = new List<float>();

            for (int i = 0; i < ColCnt; i++)
            {
                ColWidth.Add(cOffset);
            }
            for (int i = 0; i < RowCnt; i++)
            {
                RowHeight.Add(rOffset);
            }

            this.Size = new SizeF(ColWidth.Sum(), RowHeight.Sum());

            for (int row = 0; row < RowCnt; row++)
            {
                for (int col = 0; col < ColCnt; col++)
                {
                    ColSizeHandler.Add(this, new CellMatrix(row, col), enHandlerMatrix.Column);
                    RowSizeHandler.Add(this, new CellMatrix(row, col), enHandlerMatrix.Row);
                }
            }
            for (int row = 0; row < RowCnt; row++)
            {
                ColSizeHandler.Add(this, new CellMatrix(row, ColCnt), enHandlerMatrix.Column);
            }
            for (int col = 0; col < ColCnt; col++)
            {
                RowSizeHandler.Add(this, new CellMatrix(RowCnt, col), enHandlerMatrix.Row);
            }
        }
        private void InitializeMoveHandler(List<float> ColWidth, List<float> RowHeight)
        {
            RowSizeHandler = new CellSizeHandlerCollection();
            ColSizeHandler = new CellSizeHandlerCollection();

            for (int row = 0; row < RowCnt; row++)
            {
                for (int col = 0; col < ColCnt; col++)
                {
                    ColSizeHandler.Add(this, new CellMatrix(row, col), enHandlerMatrix.Column);
                    RowSizeHandler.Add(this, new CellMatrix(row, col), enHandlerMatrix.Row);
                }
            }
            for (int row = 0; row < RowCnt; row++)
            {
                ColSizeHandler.Add(this, new CellMatrix(row, ColCnt), enHandlerMatrix.Column);
            }
            for (int col = 0; col < ColCnt; col++)
            {
                RowSizeHandler.Add(this, new CellMatrix(RowCnt, col), enHandlerMatrix.Row);
            }
        }

        //TestSouce
        private List<CellSizeHandler> GetSizeHandlersInSelectedCells()
        {
            List<CellObj> objs = CellObjs.FindAll(q => q.IsSelected);
            return null;
        }
        //TestSouce

        public void Table_CellMerge()
        {
            List<CellObj> cells = CellObjs.FindAll(q => q.IsSelected == true);
            CellObj leftTop = cells.FirstOrDefault(q => q.CellMatrix.Col == cells.Min(w => w.CellMatrix.Col) && q.CellMatrix.Row == cells.Min(w => w.CellMatrix.Row));
            CellObj rightBottom = cells.FirstOrDefault(q => q.CellMatrix.Col == cells.Max(w => w.CellMatrix.Col) && q.CellMatrix.Row == cells.Max(w => w.CellMatrix.Row));
            if (leftTop == null || rightBottom == null)
                return;

            leftTop.RightIndex = rightBottom.RightIndex;
            leftTop.BottomIndex = rightBottom.BottomIndex;

            foreach (var item in cells)
            {
                if (item.Equals(leftTop))
                    continue;
                CellObjs.Remove(item);
            }
        }
        public void Table_CellSplit()
        {
            if (CellObjs.Count(q => q.IsSelected == true) != 1)
                return;

            CellObj cell = CellObjs.FirstOrDefault(q => q.IsSelected);
            int colCntToAdd = (cell.RightIndex - cell.LeftIndex);
            int rowCntToAdd = (cell.BottomIndex - cell.TopIndex);
            List<CellObj> cellsToAdd = new List<CellObj>();

            for (int i = 0; i < rowCntToAdd; i++)
            {
                for (int j = 0; j < 0 + colCntToAdd; j++)
                {
                    if (i == 0 && j == 0)
                        continue;
                    int row = cell.CellMatrix.Row + i;
                    int col = cell.CellMatrix.Col + j;
                    CellObj newcell = new CellObj(this, new CellMatrix(row, col));
                    cellsToAdd.Add(newcell);
                }
            }

            if (cellsToAdd.Count > 0)
            {
                cell.RightIndex = cell.LeftIndex + 1;
                cell.BottomIndex = cell.TopIndex + 1;
                CellObjs.AddRange(cellsToAdd);
            }
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
