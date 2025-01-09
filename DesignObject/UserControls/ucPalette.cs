using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesignObject.Controls
{
    public partial class ucPalette : UserControl
    {
        UndoRedoHistory<DesignObjManager> undoRedoHistory = new UndoRedoHistory<DesignObjManager>(50);
        CopyPasteManager copyPasteManager = new CopyPasteManager();
        DesignObjManager DesignManager { get { return DesignerVariables.Instance.DesignObjManager; } set { DesignerVariables.Instance.DesignObjManager = value; } }
        private enHit enHit { get; set; } = 0;
        protected DrawingObjBase _clickedObj;
        public enMode enMode { get; set; } = enMode.None;
        public enDraw enDraw { get; set; } = enDraw.None;
        public List<MainObj> copiedObjs = new List<MainObj>();

		public Point _clickPoint;
		public Point _curPoint;
		public Point? _cursorClickedPt;
		public Paper Paper { get { return DesignerVariables.Instance.DesignObjManager?.Paper; } }
		public Point ClickPoint { get { return new Point(_clickPoint.X - (int)Paper.Location.X, _clickPoint.Y - (int)Paper.Location.Y); } }
		public Point CurPoint { get { return new Point(_curPoint.X - (int)Paper.Location.X, _curPoint.Y - (int)Paper.Location.Y); } }
		public enSelectionState enSelState
        {
            get
            {
                if (DesignManager.SelectedMainObj.Count > 1)
                {
                    return enSelectionState.MultiSelect;
                }
                else if (DesignManager.SelectedMainObj.Count == 1)
                    return enSelectionState.SingleSelect;
                else
                    return enSelectionState.None;
            }
        }
        public ucPalette()
        {
            InitializeComponent();
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
			this.UpdateStyles();
			Invalidate();

			Load += UcPalette_Load;
        }
		private void UcPalette_Load(object sender, EventArgs e)
		{
			DesignerEvent.ZPosChanged += DesignerEvent_ZPosChanged;
            DesignerEvent.Property += DesignerEvent_Property;
		}

        #region Mode
        /// <summary>
        /// SetMode
        /// </summary>
        /// <param name="mode">enMode Mode</param>
        /// <param name="InOrOut"> True for In</param>
        public void SetMode(enMode mode, bool InOrOut)
        {
            foreach (enMode item in Enum.GetValues(typeof(enMode)))
            {
                if (Convert.ToBoolean(item & mode))
                {
                    if (InOrOut)
                    {
                        if (Convert.ToBoolean(enMode & mode) == false)
                            enMode |= item;
                    }
                    else
                    {
                        if (Convert.ToBoolean(enMode & mode))
                            enMode &= ~item;
                    }
                }
            }
        }
        public bool IsMode(enMode mode)
        {
            return Convert.ToBoolean(enMode & mode);
        }
        /// <summary>
        /// SetMode
        /// </summary>
        /// <param name="mode">enMode Mode</param>
        /// <param name="InOrOut"> True for In</param>
        public void SetDrawMode(enDraw mode, bool InOrOut)
        {
            enDraw = mode;
        }
        public bool IsDrawMode(enDraw mode)
        {
            return Convert.ToBoolean(enDraw & mode);
        }
        public void DrawDone()
        {
            enDraw = enDraw.None;
            DesignerEvent.OnButtonRelease();
		}
		#endregion

		#region Keyboard
		protected override bool IsInputKey(Keys keyData)
		{
			switch (keyData) //OnKeyDown 인식 시킬 리스트
			{
				case Keys.Right:
				case Keys.Left:
				case Keys.Up:
				case Keys.Down:
				case Keys.Tab:
				case Keys.Back:
				case Keys.Delete:
				case Keys.Enter:
				case Keys.Shift:
				case Keys.Shift | Keys.Right:
				case Keys.Shift | Keys.Left:
				case Keys.Shift | Keys.Up:
				case Keys.Shift | Keys.Down:
				case Keys.Shift | Keys.Tab:
				case Keys.Control:
				case Keys.Control | Keys.Right:
				case Keys.Control | Keys.Left:
				case Keys.Control | Keys.Up:
				case Keys.Control | Keys.Down:
				case Keys.Control | Keys.Y:
				case Keys.Control | Keys.A:
				case Keys.Control | Keys.S:
				case Keys.Control | Keys.L:
				case Keys.Control | Keys.Z:
				case Keys.Control | Keys.X:
				case Keys.Control | Keys.C:
				case Keys.Control | Keys.V:
                case Keys.Alt:
                case Keys.Alt | Keys.Right:
                case Keys.Alt | Keys.Left:
                case Keys.Alt | Keys.Up:
                case Keys.Alt | Keys.Down:
                    return true;
				default:
					return false;
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
        {
			KeyDownOnMainObj(e);
			Invalidate();
		}
        private void KeyDownOnMainObj(KeyEventArgs e)
        {
            switch (e.Modifiers)
            {
                case Keys.None:
                    {
                        switch (e.KeyCode)
                        {
                            case Keys.Left:
                            case Keys.Right:
                            case Keys.Up:
                            case Keys.Down:
                                {
                                    MoveByArrow(e.KeyCode, 1);
                                }
                                break;
                            case Keys.Delete:
                                {
                                    undoRedoHistory.AddState(DesignManager.Clone());
                                    DesignManager.DeleteObj();
                                }
                                break;
                            case Keys.Tab:
                                {
                                    if (Convert.ToBoolean(enSelState & enSelectionState.SingleSelect) ||
                                        (Convert.ToBoolean(enSelState & enSelectionState.SingleSelect) == false && Convert.ToBoolean(enSelState & enSelectionState.MultiSelect) == false))
                                    {
                                        var selObj = DesignManager.SelectedMainObj.FirstOrDefault();
                                        DesignManager.SelectedMainObj.ForEach(q => q.Select(false));
                                        var obj = DesignManager.GetNextMainObj(selObj);
                                        if (obj != null)
                                        {
                                            obj.Select();
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    break;
                case Keys.Control:
                    {
                        switch (e.KeyCode)
                        {
                            case Keys.Left:
                            case Keys.Right:
                            case Keys.Up:
                            case Keys.Down:
                                MoveByArrow(e.KeyCode, 50);
                                break;
                            case Keys.Z:
                                Undo();
                                break;
                            case Keys.Y:
                                Redo();
                                break;
                            case Keys.X:
                                undoRedoHistory.AddState(DesignManager.Clone());
                                copyPasteManager.Cut();
                                break;
                            case Keys.C:
                                undoRedoHistory.AddState(DesignManager.Clone());
                                copyPasteManager.Copy();
                                break;
                            case Keys.V:
                                undoRedoHistory.AddState(DesignManager.Clone());
                                //TextObj.String += Clipboard.GetText();
                                copyPasteManager.Paste();
                                break;
                            case Keys.S:
                                if (string.IsNullOrEmpty(DesignManager.Path))
                                    DesignerEvent.OnSave(null);
                                else
                                {
                                    string fileName = string.Format("{0}\\{1}.ddo", DesignManager.Path, DesignManager.Tittle);
                                    DesignerVariables.Instance.SaveDocument(fileName);
                                }
                                break;
                            case Keys.L:
                                DesignerEvent.OnLoad(null);
                                break;
                            case Keys.A:
                                DesignManager.MainObj.ForEach(q => q.Select());
                                break;
                        }

                    }
                    break;
                case Keys.Shift:
                    {
                        switch (e.KeyCode)
                        {
                            case Keys.Left:
                            case Keys.Right:
                            case Keys.Up:
                            case Keys.Down:
                                {
                                    MoveByArrow(e.KeyCode, 10);
                                }
                                break;
                            case Keys.Tab:
                                {
                                    if (Convert.ToBoolean(enSelState & enSelectionState.SingleSelect) ||
                                        (Convert.ToBoolean(enSelState & enSelectionState.SingleSelect) == false && Convert.ToBoolean(enSelState & enSelectionState.MultiSelect) == false))
                                    {
                                        var selObj = DesignManager.SelectedMainObj.FirstOrDefault();
                                        DesignManager.SelectedMainObj.ForEach(q => q.Select(false));
                                        var obj = DesignManager.GetPreviousMainObj(selObj);
                                        if (obj != null)
                                        {
                                            obj.Select();
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    break;
                case Keys.Alt:
                    {
                        switch(e.KeyCode)
                        {
                            case Keys.Left:
                            case Keys.Right:
                            case Keys.Up:
                            case Keys.Down:
                                {
                                    ResizeByArrow(e.KeyCode, 10);
                                }
                                break;
                        }
                    }
                    break;
                case Keys.Shift | Keys.Control:
                    break;
            }
        }
        public void ResizeByArrow(Keys key, int resizeOffset = 10)
        {
            undoRedoHistory.AddState(DesignManager.Clone());
            foreach (var item in DesignerVariables.Instance.DesignObjManager.SelectedMainObj)
            {
                if (item is LineObj)
                {
                    switch (key)
                    {
                        case Keys.Left:
                            {
                                float x = item.Size.Width - resizeOffset;
                                item.Size = new SizeF(x < 0 ? 0 : x, item.Size.Height);
                            }
                            break;
                        case Keys.Right:
                            {
                                item.Size = new SizeF(item.Size.Width + resizeOffset, item.Size.Height);
                            }
                            break;
                        case Keys.Up:
                            {
                                float y = item.Size.Height - resizeOffset;
                                item.Size = new SizeF(item.Size.Width, y < 0 ? 0 : y);
                            }
                            break;
                        case Keys.Down:
                            {
                                item.Size = new SizeF(item.Size.Width, item.Size.Height + resizeOffset);
                            }
                            break;
                    }
                }
                else
                {
                    switch (key)
                    {
                        case Keys.Left:
                            {
                                float x = item.Size.Width - resizeOffset;
                                item.Size = new SizeF(x < 0 ? 0 : x, item.Size.Height);
                            }
                            break;
                        case Keys.Right:
                            {
                                item.Size = new SizeF(item.Size.Width + resizeOffset, item.Size.Height);
                            }
                            break;
                        case Keys.Up:
                            {
                                float y = item.Size.Height - resizeOffset;
                                item.Size = new SizeF(item.Size.Width, y < 0 ? 0 : y);
                            }
                            break;
                        case Keys.Down:
                            {
                                item.Size = new SizeF(item.Size.Width, item.Size.Height + resizeOffset);
                            }
                            break;
                    }
                }
            }
            DesignerEvent.OnObjectSelected();
        }
        public void MoveByArrow(Keys key, int moveOffset = 1)
        {
            undoRedoHistory.AddState(DesignManager.Clone());
            foreach (var item in DesignerVariables.Instance.DesignObjManager.SelectedMainObj)
            {
                if (item is LineObj)
                {
                    switch (key)
                    {
                        case Keys.Left:
                            {
                                item.MoveMainObj(new PointF(item.Location.X - moveOffset, item.Location.Y));
                            }
                            break;
                        case Keys.Right:
                            {
                                item.MoveMainObj(new PointF(item.Location.X + moveOffset, item.Location.Y));
                            }
                            break;
                        case Keys.Up:
                            {
                                item.MoveMainObj(new PointF(item.Location.X, item.Location.Y - moveOffset));
                            }
                            break;
                        case Keys.Down:
                            {
                                item.MoveMainObj(new PointF(item.Location.X, item.Location.Y + moveOffset));
                            }
                            break;
                    }
                }
                else
                {
                    switch (key)
                    {
                        case Keys.Left:
                            {
                                item.MoveMainObj(new PointF(item.Rect.X - moveOffset, item.Rect.Y));
                            }
                            break;
                        case Keys.Right:
                            {
                                item.MoveMainObj(new PointF(item.Rect.X + moveOffset, item.Rect.Y));
                            }
                            break;
                        case Keys.Up:
                            {
                                item.MoveMainObj(new PointF(item.Rect.X, item.Rect.Y - moveOffset));
                            }
                            break;
                        case Keys.Down:
                            {
                                item.MoveMainObj(new PointF(item.Rect.X, item.Rect.Y + moveOffset));
                            }
                            break;
                    }
                }
            }
			DesignerEvent.OnObjectSelected();
		}
        #endregion

        #region Mouse
        public void SetHit(enHit mode)
        {
            enHit = mode;
        }
        public bool IsHit(enHit mode)
        {
            return Convert.ToBoolean(enHit & mode);
        }
        public void HitComponentObject(DrawingObjBase objBase, PointF pt)
        {
            if (objBase is CellObj)
            {
                SetHit(enHit.CellObj);
            }
            else if (objBase is CellSizeHandler)
            {
                SetHit(enHit.CellSizeHandler);
            }
            else if (objBase is MoveHandler)
            {
                SetHit(enHit.MainObjMoveHandler);
            }
            else if (objBase is AdjustHandler)
            {
                SetHit(enHit.MainObjAdjustHandler);
            }
        }
        public DrawingObjBase HitObject(PointF pt)
        {
            DrawingObjBase objOnPt = DesignManager?.GetClickedMainObjs(pt);
            if (objOnPt != null)
            {
                SetMode(enMode.OnClick, true);
                if (objOnPt is IComponent)
                {
                    HitComponentObject(objOnPt, pt);
                }
                else
                {
                    if (objOnPt is BarcodeObj)
                        SetHit(enHit.BarcodeObj);
                    else if (objOnPt is ImageObj)
                        SetHit(enHit.ImageObj);
                    else if (objOnPt is TextBoxObj)
                        SetHit(enHit.TextObj);
                    else if (objOnPt is IFigures)
                        SetHit(enHit.Figures);
                }
            }
            return objOnPt;
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.Focus();
            _clickPoint = e.Location.GetZoomedPoint();
            if (e.Button == MouseButtons.Left)
            {
                SetHit(enHit.None);
                SetMode(enMode.OnClick, true);
                _clickedObj = HitObject(ClickPoint);
                Invalidate();
            }
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            _curPoint = e.Location.GetZoomedPoint();
            if (e.Button == MouseButtons.Left)
            {
                if (IsMode(enMode.OnClick))
                    SetMode(enMode.IsDragging, true);

                Invalidate();
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            SetMode(enMode.OnClick, false);
            SetMode(enMode.IsDragging, false);
            switch (e.Button)
            {
                case MouseButtons.Left:
                    {
                        if (IsMode(enMode.Draw))
                        {
                            undoRedoHistory.AddState(DesignManager.Clone());
                            InsertObect();
                            SetMode(enMode.Draw, false);
                            DrawDone();
                        }
                        else
                        {
							undoRedoHistory.AddState(DesignManager.Clone());
							if (IsHit(enHit.None))
							{
								DesignManager.SelectedObj.ForEach(q => q.Select(false));
								List<MainObj> mainObjs = SelectMultiObjs();
								if (mainObjs.Count > 1) //MultiSelect
									SelectObj(mainObjs);
								else if (mainObjs.Count == 1) //Single
									SelectObj(mainObjs.First());
							}
							else
								MouseUpSelect();
							DesignerEvent.OnObjectSelected();
						}
                    }
                    break;
                case MouseButtons.Right:
                    _clickedObj = HitObject(ClickPoint);
                    ShowContext();
                    break;
                default:
                    break;
            }
            _clickedObj = null;
            SetHit(enHit.None);
            Invalidate();
        }
        private void MouseUpSelect()
        {
            DrawingObjBase mainObj = _clickedObj.GetMainObj();
            if (ModifierKeys == Keys.Shift)
            {
                mainObj.Select(!mainObj.IsSelected);
            }
            else
            {

                if (!mainObj?.IsSelected ?? false)
                {
                    DesignManager.SelectedObj.ForEach(q => q.Select(false));
                    if (IsHit(enHit.CellObj))
                        SelectCell();
                    else
                        SelectObj(_clickedObj);
                }
                else
                {
                    switch (enHit)
                    {
                        case enHit.MainObjAdjustHandler:
                            AdjustMainObjSize();
                            break;
                        case enHit.CellSizeHandler:
                            AdjustCellSize(ClickPoint, CurPoint);
                            break;
                        case enHit.MainObjMoveHandler:
                            MoveMainObj();
                            break;
                    }
                }
            }
        }
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (IsHit(enHit.TextObj))
            {
                try
                {
                    if (DesignManager.TextBoxObjs.Where(q => q.IsSelected).Count() > 0)
                    {
                        var textObj = DesignManager.TextBoxObjs.Where(q => q.IsSelected).Cast<IWritable>().ToList();
                        if (textObj.Count > 0)
                            DesignerEvent.OnPopUpTextObjPropSetting(new TextObjProp(textObj));
                    }
                }
                catch (Exception) { }
            }
            else if (Convert.ToBoolean(enSelState & enSelectionState.SingleSelect))
            {
                if (IsHit(enHit.MainObjMoveHandler))
                {
                    var mainObj = DesignManager.SelectedMainObj.First();
                    var mainProp = new MainObjProp(PointToScreen(CurPoint.GetActualPoint()), DesignManager.SelectedMainObj.First().DisplayRect);
                    DesignerEvent.OnMoveHandlerDoubleClick(mainProp);
                }
                else if (IsHit(enHit.BarcodeObj))
                {
                    if (DesignManager.SelectedMainObj.Count == 1 && DesignManager.SelectedMainObj.First() is BarcodeObj barcode)
                    {
                        var barcodeProp = new BarcodeProp(barcode);
                        DesignerEvent.OnPopUpBarcodePropSetting(barcodeProp);
                    }
                }
				else if (IsHit(enHit.CellObj))
				{
					if (DesignManager.SelectedMainObj.Count == 1 && DesignManager.SelectedMainObj.First() is TableObj tbl)
					{
                        var selectedCell = tbl.CellObjs.FirstOrDefault(x => x.IsSelected);
                        var cellProp = new CellObjProp(selectedCell);
                        DesignerEvent.OnCellObjDoubleClick(selectedCell, cellProp);
					}
				}
			}
        }
        private void DesignerEvent_Property(object sender, EventArgs e)
        {
            OnMouseDoubleClick(null);
            if (DesignManager.SelectedMainObj.All(q => q is IWritable))
            {
                var textObj = DesignManager.TextBoxObjs.Where(q => q.IsSelected).Cast<IWritable>().ToList();
                if (textObj.Count > 0)
                    DesignerEvent.OnPopUpTextObjPropSetting(new TextObjProp(textObj));
            }
            else if (Convert.ToBoolean(enSelState & enSelectionState.SingleSelect))
            {
                if (DesignManager.SelectedMainObj.Count == 1 && DesignManager.SelectedMainObj.First() is BarcodeObj barcode)
                {
                    var barcodeProp = new BarcodeProp(barcode);
                    if (barcodeProp != null)
                        DesignerEvent.OnPopUpBarcodePropSetting(barcodeProp);
                }
            }
            Invalidate();
        }
        #endregion

        #region Object
        public void Undo()
        {
            DesignManager = undoRedoHistory.Undo(DesignManager);
        }
        public void Redo()
        {
            DesignManager = undoRedoHistory.Redo(DesignManager);
        }
        public void InsertTableObect(TableObj table)
        {
            DesignManager.TableObjs.Add(table);
        }
        public void InsertImage(string path)
        {
            DesignManager.ImageObjs.Add(new ImageObj(GetDrawnRect(), path));
        }
        public void InsertObect()
        {
            switch (enDraw)
            {
                case enDraw.DrawTable:
                    DesignerEvent.OnPopUpCreateTable(new MainObjProp(PointToScreen(CurPoint.GetActualPoint()), GetDrawnRect()));
                    break;
                case enDraw.DrawBarcode:
                    DesignManager.AddBarcode(new BarcodeObj(GetDrawnRect(), enBarcodeType.DataMatrix.ToString(), "BarcodeData", false, true));
                    break;
                case enDraw.DrawMessageBox:
                    DesignManager.AddTextBox(new TextBoxObj(GetDrawnRect()));
                    break;
                case enDraw.DrawCircle:
                    DesignManager.AddCircle(new CircleObj(GetDrawnRect()));
                    break;
                case enDraw.DrawImage:
                    var designPath = new PathArgs(enButtons.Image, null);
					DesignerEvent.OnLoadImage(designPath);
					DesignManager.AddImage(new ImageObj(GetDrawnRect(), designPath.DesignPath));
                    break;
                case enDraw.DrawRect:
                    DesignManager.AddRect(new RectObj(GetDrawnRect()));
                    break;
                case enDraw.DrawLine:
                    Tuple<PointF, PointF> pt = DrawLine(ClickPoint, CurPoint);
                    DesignManager.AddLine(new LineObj(pt.Item1, pt.Item2));
                    break;
                case enDraw.None:
                default:
                    break;
            }
		}
		private void DesignerEvent_ZPosChanged(object sender, ZPosProp e)
		{
			e.MainObjs.ForEach(q =>
			{
				var tar = q.ZPos + (e.UpDown == UpDown.Up ? 1 : -1);
				if (e.UpDown == UpDown.Up)
				{
					var front = DesignManager.MainObj.Where(w => w.ZPos <= tar).ToList();
					front.ForEach(w => w.ZPos += -1);
				}
				else
				{
					var rear = DesignManager.MainObj.Where(w => w.ZPos >= tar).ToList();
					rear.ForEach(w => w.ZPos += 1);
				}
				q.ZPos = tar;
			});
			DesignManager.NormalizeZPos(DesignManager.MainObj);
            DesignerEvent.OnObjectSelected();
			Invalidate();
		}

		public void Context_FormClosed(object sender, NewTableEventArgs e)
        {
            //PointF location = new PointF((float)((rect.Width * 0.3) / 2), 50);

            DesignManager.TableObjs.Add(new TableObj(GetDrawnRect(), e.ColCnt, e.RowCnt));

            ContextMenuStrip.Close();
            Invalidate();
        }
        public void AdjustCellSize(PointF clickedPt, PointF curPt)
        {
            if (IsMode(enMode.CellSizeAdjust) == false || Convert.ToBoolean(enSelState & enSelectionState.MultiSelect))
                return;

            CellSizeHandler handler = _clickedObj as CellSizeHandler;
            if (handler == null)
                return;

            undoRedoHistory.AddState(DesignManager.Clone());
            handler.Parent.AdjustCellSize(clickedPt, curPt, handler);
            SetMode(enMode.CellSizeAdjust, false);
        }

        public void AdjustMainObjSize()
        {
            if (Convert.ToBoolean(enSelState & enSelectionState.MultiSelect))
                return;

            if (_clickedObj is AdjustHandler)
            {
                var handler = _clickedObj as AdjustHandler;
                var parent = handler.Parent as MainObj;
                if (parent == null || parent.IsSelected == false)
                    return;

                undoRedoHistory.AddState(DesignManager.Clone());
                SetMode(enMode.SizeAdjust, true);
                if (parent is LineObj line)
                {
                    PointF[] linePts = new PointF[2]{ line.Pt1, line.Pt2 };
                    var otherPt = handler.CenterPoint.GetFarthest(linePts);
                    var location = DrawLine(otherPt, CurPoint);
                    line.ReSize(location.Item1, location.Item2);
                }
                else if (parent is ImageObj)
                {
                    RectangleF rect = AdjustImageObj(handler);
                    (handler.Parent as MainObj)?.ReSize(rect);
                }
                else
                {
                    RectangleF rect = AdjustSize(handler);
                    (handler.Parent as MainObj)?.ReSize(rect);
                }
            }
        }
        public Tuple<PointF, PointF> AdjustLine(AdjustHandler handler, PointF curPt)
        {
            var line = handler.Parent as LineObj;
            PointF pt1 = line.Pt1;
            PointF pt2 = line.Pt2;
            switch (handler.enRectSIde)
            {
                case enRectSIde.Left:
                    pt1 = curPt;
                    break;
                case enRectSIde.Right:
                    pt2 = curPt;
                    break;
            }
            return new Tuple<PointF, PointF>(pt1, pt2);
        }
        public void MoveMainObj()
        {
            undoRedoHistory.AddState(DesignManager.Clone());
            SizeF offset = new SizeF(ClickPoint.X - CurPoint.X, ClickPoint.Y - CurPoint.Y);
            DesignManager.SelectedMainObj.ForEach(q =>
            {
                PointF point = new PointF(q.Rect.X - offset.Width, q.Rect.Y - offset.Height);
                //RectangleF rect = new RectangleF(point, q.DisplayRect.Size);
                //if (Convert.ToBoolean(enSelState & enSelectionState.SingleSelect))
                //    rect = Rectangle.Round(GetNearestDockedPos(rect));
                q.MoveMainObj(point);
            });
        }
        public RectangleF AdjustSize(AdjustHandler handler)
        {
            RectangleF temp = new RectangleF();
            var main = handler.Parent as MainObj;
            var mainRect = main.DisplayRect;

            if (main is BarcodeObj barcode && barcode.IsOneDBarcode == false) //2D바코드 정사각형
            {
                switch (handler.enRectSIde)
                {
                    case enRectSIde.LT:
                        {
                            //Bottom - curPt.Y == Right - curPt.X;
                            temp = RectangleF.FromLTRB(_curPoint.X, mainRect.Bottom - mainRect.Right + _curPoint.X, mainRect.Right, mainRect.Bottom);
                        }
                        break;
                    case enRectSIde.RT:
                        {
                            //curPt.X - Left == Bottom - curPt.Y;
                            temp = RectangleF.FromLTRB(mainRect.Left, mainRect.Bottom + mainRect.Left - _curPoint.X, _curPoint.X, mainRect.Bottom);
                        }
                        break;
                    case enRectSIde.LB:
                        {
                            //Right - curPt.X == curPt.Y - Top
                            temp = RectangleF.FromLTRB(mainRect.Right - _curPoint.Y + mainRect.Top, mainRect.Top, mainRect.Right, _curPoint.Y);
                        }
                        break;
                    case enRectSIde.RB:
                        {
                            //curPt.X - Left == curPt.Y - Top
                            temp = RectangleF.FromLTRB(mainRect.Left, mainRect.Top, _curPoint.Y - mainRect.Top + mainRect.Left, _curPoint.Y);
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                SizeF offset = new SizeF(ClickPoint.X - CurPoint.X, ClickPoint.Y - CurPoint.Y);
                switch (handler.enRectSIde)
                {
                    case enRectSIde.LT:
                        {
                            temp = RectangleF.FromLTRB(mainRect.Left - offset.Width, mainRect.Top - offset.Height, mainRect.Right, mainRect.Bottom);
                        }
                        break;
                    case enRectSIde.RT:
                        {
                            temp = RectangleF.FromLTRB(mainRect.Left, mainRect.Top - offset.Height, mainRect.Right - offset.Width, mainRect.Bottom);
                        }
                        break;
                    case enRectSIde.LB:
                        {
                            temp = RectangleF.FromLTRB(mainRect.Left - offset.Width, mainRect.Top, mainRect.Right, mainRect.Bottom - offset.Height);
                        }
                        break;
                    case enRectSIde.RB:
                        {
                            temp = RectangleF.FromLTRB(mainRect.Left, mainRect.Top, mainRect.Right - offset.Width, mainRect.Bottom - offset.Height);
                        }
                        break;
                    default:
                        break;
                }
            }
            return temp;
        }
        public RectangleF AdjustImageObj(AdjustHandler handler)
        {
            var main = handler.Parent as ImageObj;
            RectangleF rect = new RectangleF();

            var mainRect = main.DisplayRect;
            float heightRatio = main.OriginalBitmapSize.Width / main.OriginalBitmapSize.Height;
            if (Convert.ToBoolean(ModifierKeys & Keys.Control) == false)
            {
                PointF curpt = CurPoint;
                switch (handler.enRectSIde)
                {
                    case enRectSIde.LT:
                        {
                            PointF rb = new PointF(mainRect.Right, mainRect.Bottom);
                            if (curpt.X > rb.X)
                                curpt.X = rb.X - 1;
                            if (curpt.Y > rb.Y)
                                curpt.Y = rb.Y - 1;

                            float longerSide = GetLongerSide(rb, curpt);
                            PointF lt = new PointF(rb.X - longerSide, rb.Y - (longerSide / heightRatio));
                            rect = GetRectangle(lt, rb);
                        }
                        break;
                    case enRectSIde.RT:
                        {
                            PointF lb = new PointF(mainRect.Left, mainRect.Bottom);
                            if (curpt.X < lb.X)
                                curpt.X = lb.X + 1;
                            if (curpt.Y > lb.Y)
                                curpt.Y = lb.Y - 1;

                            float longerSide = GetLongerSide(lb, curpt);
                            PointF rt = new PointF(lb.X + longerSide, lb.Y - (longerSide / heightRatio));
                            rect = GetRectangle(rt, lb);
                        }
                        break;
                    case enRectSIde.LB:
                        {
                            PointF rt = new PointF(mainRect.Right, mainRect.Top);
                            if (curpt.X > rt.X)
                                curpt.X = rt.X - 1;
                            if (curpt.Y < rt.Y)
                                curpt.Y = rt.Y + 1;

                            float longerSide = GetLongerSide(rt, curpt);
                            PointF lb = new PointF(rt.X - longerSide, rt.Y + (longerSide / heightRatio));
                            rect = GetRectangle(lb, rt);
                        }
                        break;
                    case enRectSIde.RB:
                        {
                            PointF lt = new PointF(mainRect.Left, mainRect.Top);
                            if (curpt.X < lt.X)
                                curpt.X = lt.X + 1;
                            if (curpt.Y < lt.Y)
                                curpt.Y = lt.Y + 1;

                            float longerSide = GetLongerSide(lt, curpt);
                            PointF rb = new PointF(lt.X + longerSide, lt.Y + (longerSide / heightRatio));
                            rect = GetRectangle(rb, lt);
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                SizeF offset = new SizeF(ClickPoint.X - CurPoint.X, ClickPoint.Y - CurPoint.Y);
                switch (handler.enRectSIde)
                {
                    case enRectSIde.LT:
                        {
                            rect = GetRectangle(new PointF(mainRect.Left - offset.Width, mainRect.Top - offset.Height), new PointF(mainRect.Right, mainRect.Bottom));
                        }
                        break;
                    case enRectSIde.RT:
                        {
                            rect = GetRectangle(new PointF(mainRect.Left, mainRect.Top - offset.Height), new PointF(mainRect.Right - offset.Width, mainRect.Bottom));
                        }
                        break;
                    case enRectSIde.LB:
                        {
                            rect = GetRectangle(new PointF(mainRect.Left - offset.Width, mainRect.Top), new PointF(mainRect.Right, mainRect.Bottom - offset.Height));
                        }
                        break;
                    case enRectSIde.RB:
                        {
                            rect = GetRectangle(new PointF(mainRect.Left, mainRect.Top), new PointF(mainRect.Right - offset.Width, mainRect.Bottom - offset.Height));
                        }
                        break;
                    default:
                        break;
                }
            }
            return rect;
        }
        RectangleF GetRectangle(PointF p1, PointF p2)
        {
            float top = Math.Min(p1.Y, p2.Y);
            float bottom = Math.Max(p1.Y, p2.Y);
            float left = Math.Min(p1.X, p2.X);
            float right = Math.Max(p1.X, p2.X);

            RectangleF rect = RectangleF.FromLTRB(left, top, right, bottom);

            return rect;
        }
        float GetLongerSide(PointF p1, PointF p2)
        {
            float width = Math.Max(p1.X, p2.X) - Math.Min(p1.X, p2.X);
            float height = Math.Max(p1.Y, p2.Y) - Math.Min(p1.Y, p1.Y);
            return Math.Max(width, height);
        }
        public void SelectCell()
        {
            TableObj table = DesignManager.TableObjs.FirstOrDefault(q => q.DisplayRect.Contains(_clickPoint));
            if (table == null)
                return;

            DesignManager.TableObjs.ForEach(q => q.CellObjs.ForEach(w => w.Select(false)));
            CellObj _clickedCell = table?.CellObjs.Find(q => q.DisplayRect.Contains(_clickPoint));
            CellObj _curCell = table?.CellObjs.Find(q => q.DisplayRect.Contains(_curPoint));
            if (_clickedCell == null || _curCell == null)
                return;
            List<RectangleF> rect = new List<RectangleF>() { _clickedCell.DisplayRect, _curCell.DisplayRect };
            float left = rect.Min(q => q.Left);
            float right = rect.Max(q => q.Right);
            float top = rect.Min(q => q.Top);
            float bottom = rect.Max(q => q.Bottom);

            RectangleF area = RectangleF.FromLTRB(left, top, right, bottom);
            List<CellObj> cells = table?.CellObjs.FindAll(q => q.Contains(area));
            cells.ForEach(q => q.Select(true));
            if (cells.Count(q => q.IsSelected) == 1)
            {
                SetMode(enMode.SingleCellSelected, true);
                SetMode(enMode.MultiCellsSelected, false);
            }
            else if (cells.Count(q => q.IsSelected) > 1)
            {
                SetMode(enMode.MultiCellsSelected, true);
                SetMode(enMode.SingleCellSelected, false);
            }
        }
        public List<MainObj> GetObjsInDrawnRect()
        {
            RectangleF DrawnRect = GetDrawnRect();
            RectangleF ActualRect = GetActualRect();

            List<MainObj> mainObjs = new List<MainObj>();
            DesignManager.MainObj.ForEach(q =>
            {
                int count = q.Components.Count(w =>
                {
                    var obj = w as DrawingHandler;
                    if (obj?.Contains(ActualRect) ?? false)
                        return true;
                    return false;
                });
                bool isline = q is LineObj;
                if ((isline && count > 1) || (isline == false && count > 2))
                    mainObjs.Add(q);
            });
            return mainObjs;
        }
        public void SelectObj(List<MainObj> obj)
        {
            obj.ForEach(q =>
            {
                q.Select();
            });
        }
        public void SelectObj(DrawingObjBase obj)
        {
            obj.Select();
        }
        public List<MainObj> SelectMultiObjs()
        {
            return GetObjsInDrawnRect();
            //if (mainObjs.Count > 0  && mainObjs.Count == CurObj.Count && mainObjs.All(q => CurObj.Any(w => q.Equals(w))))
            //    return null;
            //return mainObjs;
        }
        public RectangleF GetDrawnRect()
        {
            float top = _curPoint.Y < _clickPoint.Y ? _curPoint.Y : _clickPoint.Y;
            float bottom = _curPoint.Y > _clickPoint.Y ? _curPoint.Y : _clickPoint.Y;
            float left = _curPoint.X < _clickPoint.X ? _curPoint.X : _clickPoint.X;
            float right = _curPoint.X > _clickPoint.X ? _curPoint.X : _clickPoint.X;
            RectangleF rect = RectangleF.FromLTRB(left, top, right, bottom);
            return rect;
        }

        public RectangleF GetActualRect()
        {
            float top = CurPoint.Y < ClickPoint.Y ? CurPoint.Y : ClickPoint.Y;
            float bottom = CurPoint.Y > ClickPoint.Y ? CurPoint.Y : ClickPoint.Y;
            float left = CurPoint.X < ClickPoint.X ? CurPoint.X : ClickPoint.X;
            float right = CurPoint.X > ClickPoint.X ? CurPoint.X : ClickPoint.X;
            RectangleF rect = RectangleF.FromLTRB(left, top, right, bottom);
            return rect;
        }

        //public MainObj GetNearestObj(RectangleF rect)
        //{
        //    DrawingObjBase ha = DesignManager.AllHandlers.Find(q =>
        //    {
        //        if (q.Parent.Equals(_clickedObj?.Parent))
        //            return false;

        //        var objsSides = new List<float>()
        //        {
        //            Math.Abs(Math.Abs(rect.Left) - Math.Abs(q.Rect.Left)),
        //            Math.Abs(Math.Abs(rect.Left) - Math.Abs(q.Rect.Right)),
        //            Math.Abs(Math.Abs(rect.Right) - Math.Abs(q.Rect.Left)),
        //            Math.Abs(Math.Abs(rect.Right) - Math.Abs(q.Rect.Right)),
        //            Math.Abs(Math.Abs(rect.Top) - Math.Abs(q.Rect.Top)),
        //            Math.Abs(Math.Abs(rect.Top) - Math.Abs(q.Rect.Bottom)),
        //            Math.Abs(Math.Abs(rect.Bottom) - Math.Abs(q.Rect.Top)),
        //            Math.Abs(Math.Abs(rect.Bottom) - Math.Abs(q.Rect.Bottom)),
        //        };

        //        return objsSides.Min() < 5f;
        //    });

        //    return ha?.Parent as MainObj;
        //}
        //public RectangleF GetNearestDockedPos(RectangleF rect)
        //{
        //    MainObj nearestObj = GetNearestObj(rect);
        //    if (nearestObj != null)
        //    {
        //        List<float> h = new List<float>()
        //        {
        //            Math.Abs(Math.Abs(rect.Left) - Math.Abs(nearestObj.DisplayRect.Left)),
        //            Math.Abs(Math.Abs(rect.Left) - Math.Abs(nearestObj.DisplayRect.Right)),
        //            Math.Abs(Math.Abs(rect.Right) - Math.Abs(nearestObj.DisplayRect.Left)),
        //            Math.Abs(Math.Abs(rect.Right) - Math.Abs(nearestObj.DisplayRect.Right)),
        //        };
        //        List<float> v = new List<float>()
        //        {
        //            Math.Abs(Math.Abs(rect.Top) - Math.Abs(nearestObj.DisplayRect.Top)),
        //            Math.Abs(Math.Abs(rect.Top) - Math.Abs(nearestObj.DisplayRect.Bottom)),
        //            Math.Abs(Math.Abs(rect.Bottom) - Math.Abs(nearestObj.DisplayRect.Top)),
        //            Math.Abs(Math.Abs(rect.Bottom) - Math.Abs(nearestObj.DisplayRect.Bottom)),
        //        };

        //        List<float> objsSides = h.Union(v).OrderBy(q => q).ToList();
        //        if (Math.Abs(Math.Abs(rect.Left) - Math.Abs(nearestObj.DisplayRect.Left)) == objsSides.Min())
        //        {
        //            if ((rect.Top > nearestObj.DisplayTop && rect.Top < nearestObj.DisplayBottom) ||
        //                (rect.Bottom > nearestObj.DisplayTop && rect.Bottom < nearestObj.DisplayBottom))
        //            {
        //                rect.Location = new PointF(nearestObj.DisplayRect.Left, rect.Top);
        //                return rect;
        //            }
        //        }
        //        else if (Math.Abs(Math.Abs(rect.Left) - Math.Abs(nearestObj.DisplayRect.Right)) == objsSides.Min())
        //        {
        //            if ((rect.Top > nearestObj.DisplayTop && rect.Top < nearestObj.DisplayBottom) ||
        //                (rect.Bottom > nearestObj.DisplayTop && rect.Bottom < nearestObj.DisplayBottom))
        //            {
        //                rect.Location = new PointF(nearestObj.DisplayRect.Right, rect.Top);
        //                return rect;
        //            }
        //        }
        //        else if (Math.Abs(Math.Abs(rect.Right) - Math.Abs(nearestObj.DisplayRect.Left)) == objsSides.Min())
        //        {
        //            if ((rect.Top > nearestObj.DisplayTop && rect.Top < nearestObj.DisplayBottom) ||
        //                (rect.Bottom > nearestObj.DisplayTop && rect.Bottom < nearestObj.DisplayBottom))
        //            {
        //                rect.Location = new PointF(nearestObj.DisplayRect.Left - rect.Width, rect.Top);
        //                return rect;
        //            }
        //        }
        //        else if (Math.Abs(Math.Abs(rect.Right) - Math.Abs(nearestObj.DisplayRect.Right)) == objsSides.Min())
        //        {
        //            if ((rect.Top > nearestObj.DisplayTop && rect.Top < nearestObj.DisplayBottom) ||
        //                (rect.Bottom > nearestObj.DisplayTop && rect.Bottom < nearestObj.DisplayBottom))
        //            {
        //                rect.Location = new PointF(nearestObj.DisplayRect.Right - rect.Width, rect.Top);
        //                return rect;
        //            }
        //        }
        //        else if (Math.Abs(Math.Abs(rect.Top) - Math.Abs(nearestObj.DisplayRect.Top)) == objsSides.Min())
        //        {
        //            if ((rect.Left > nearestObj.DisplayLeft && rect.Left < nearestObj.DisplayRight) ||
        //                (rect.Right > nearestObj.DisplayLeft && rect.Right < nearestObj.DisplayRight))
        //            {
        //                rect.Location = new PointF(rect.Left, nearestObj.DisplayRect.Top);
        //                return rect;
        //            }

        //        }
        //        else if (Math.Abs(Math.Abs(rect.Top) - Math.Abs(nearestObj.DisplayRect.Bottom)) == objsSides.Min())
        //        {
        //            if ((rect.Left > nearestObj.DisplayLeft && rect.Left < nearestObj.DisplayRight) ||
        //                (rect.Right > nearestObj.DisplayLeft && rect.Right < nearestObj.DisplayRight))
        //            {
        //                rect.Location = new PointF(rect.Left, nearestObj.DisplayRect.Bottom);
        //                return rect;
        //            }
        //        }
        //        else if (Math.Abs(Math.Abs(rect.Bottom) - Math.Abs(nearestObj.DisplayRect.Top)) == objsSides.Min())
        //        {
        //            if ((rect.Left > nearestObj.DisplayLeft && rect.Left < nearestObj.DisplayRight) ||
        //                (rect.Right > nearestObj.DisplayLeft && rect.Right < nearestObj.DisplayRight))
        //            {
        //                rect.Location = new PointF(rect.Left, nearestObj.DisplayRect.Top - rect.Height);
        //                return rect;
        //            }
        //        }
        //        else if (Math.Abs(Math.Abs(rect.Bottom) - Math.Abs(nearestObj.DisplayRect.Bottom)) == objsSides.Min())
        //        {
        //            if ((rect.Left > nearestObj.DisplayLeft && rect.Left < nearestObj.DisplayRight) ||
        //                (rect.Right > nearestObj.DisplayLeft && rect.Right < nearestObj.DisplayRight))
        //            {
        //                rect.Location = new PointF(rect.Left, nearestObj.DisplayRect.Bottom - rect.Height);
        //                return rect;
        //            }
        //        }
        //    }
        //    return rect;
        //}
        public void SetObjectAalign(enAlign enAlign)
        {
            var selList = DesignManager.SelectedMainObj;
            if (selList.Count > 0)
            {
                float position = 0;
                switch (enAlign)
                {
                    case enAlign.Top:
                        position = selList.Min(x => x.Rect.Top);
                        selList.ForEach(q => q.MoveMainObj(new PointF(q.Rect.Left, position)));
                        break;
                    case enAlign.Bottom:
                        position = selList.Max(x => x.Rect.Bottom);
                        selList.ForEach(q => q.MoveMainObj(new PointF(q.Rect.Left, position - q.DisplayRect.Height)));
                        break;
                    case enAlign.Left:
                        position = selList.Min(x => x.Rect.Left);
                        selList.ForEach(q => q.MoveMainObj(new PointF(position, q.Rect.Top)));
                        break;
                    case enAlign.Right:
                        position = selList.Max(x => x.Rect.Right);
                        selList.ForEach(q => q.MoveMainObj(new PointF(position - q.DisplayRect.Width, q.Rect.Top)));
                        break;
                    default:
                        break;
                }
                Invalidate();
            }
        }
        public Tuple<PointF, PointF> DrawLine(PointF pt1, PointF pt2)
        {
            PointF curPt = pt2;
            float xDist = Math.Abs(pt1.X - pt2.X);
            float yDist = Math.Abs(pt1.Y - pt2.Y);
            if (xDist < yDist)
            {
                curPt = new PointF(pt1.X, pt2.Y);
            }
            else if (xDist == yDist || Math.Abs(xDist - yDist) < 30)
            {
                if (pt1.X < pt2.X)
                {
                    curPt = pt1.Y < pt2.Y
                        ? new PointF(pt2.X, pt1.Y)  // 우측 상단에서 좌측 하단
                        : new PointF(pt2.X, pt1.Y); // 우측 하단에서 좌측 상단
                }
                else
                {
                    curPt = pt1.Y < pt2.Y
                        ? new PointF(pt1.X, pt2.Y)  // 좌측 상단에서 우측 하단
                        : new PointF(pt1.X, pt2.Y); // 좌측 하단에서 우측 상단
                }
            }
            else
            {
                curPt = new PointF(pt2.X, pt1.Y);
            }

            return new Tuple<PointF, PointF>(pt1, curPt);
        }
        #endregion

        #region OnPaint
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using (Matrix m = new Matrix())
            {
                SetMatrix(DesignerVariables.Instance.ZoomRatio, e.Graphics, m);
                DrawPaper(e.Graphics);
                DrawMainObj(e.Graphics);
                DrawAnimation(e.Graphics);
                base.OnPaint(e);
            }
        }
        private void SetMatrix(float scale, Graphics g, Matrix m)
        {
            m.Scale(scale, scale);
            g.Transform = m;
        }
        private void DrawMainObj(Graphics g)
        {
            DesignManager?.MainObj.OrderBy(q => q.ZPos).ToList().ForEach(q =>
            {
                using (Matrix m = new Matrix())
                {
                    var ori = g.Transform.Clone();
                    using (Pen p = new Pen(Color.Black, q.LineThickness))
                    {
                        if (q is TableObj table)
                        {
                            g.DrawRectangle(p, Rectangle.Round(q.DisplayRect));
                            DrawCell(table, g, p);
                        }
                        else if (q is BarcodeObj barcode)
                        {
                            g.DrawImage(barcode.BarcodeBitmap, barcode.DisplayRect);
                            if (barcode.PrimaryBarcode)
                            {
                                using (Pen p2 = new Pen(Color.Gold, 2))
                                    g.DrawRectangle(p2, barcode.DisplayRect.X, barcode.DisplayRect.Y, barcode.DisplayRect.Width, barcode.DisplayRect.Height);
                            }
                            else
                            {
                                using (Pen p2 = new Pen(Color.Red))
                                    g.DrawRectangle(p2, barcode.DisplayRect.X, barcode.DisplayRect.Y, barcode.DisplayRect.Width, barcode.DisplayRect.Height);
                            }

                        }
                        else if (q is IFigures figures)
                        {
                            g.DrawPath(p, figures.DisplayGraphicsPath);
                        }
                        else if (q is TextBoxObj text)
                        {
							using (SolidBrush brush = new SolidBrush(Color.Black))
							{
								g.DrawString(text.Text.String, text.Text.Font, brush, text.DisplayRect, text.Text.StringFormat);
							}

							using (Pen layout = new Pen(Color.LightGray, 2) { DashStyle = DashStyle.DashDot })
                                g.DrawPath(layout, text.DisplayGraphicsPath);
                        }
                        else if (q is ImageObj image)
                        {
                            if (image.Bitmap != null)
                                g.DrawImage(image.Bitmap, image.DisplayRect);
                        }
                        if (q.IsSelected)
                        {
                            using (Pen hp = new Pen(Color.Black, 1))
                            {
                                g.DrawPath(hp, q.DisplayHandlerGraphicsPath);
                                if ((q is LineObj) == false)
                                {
                                    g.DrawPath(hp, q.DisplayGraphicsPath);
                                    if (Convert.ToBoolean(enSelState & enSelectionState.SingleSelect)) //좌표 보조선
                                    {
                                        using (Pen p2 = new Pen(Color.CornflowerBlue, 0.8f) { DashStyle = DashStyle.Dot })
                                        {
                                            var clientRect = new RectangleF(new PointF(ClientRectangle.Location.X, ClientRectangle.Location.Y), new SizeF((ClientRectangle.Width) / Paper.ZoomRatio, (ClientRectangle.Height / Paper.ZoomRatio)));
                                            g.DrawLine(p2, new PointF(q.DisplayRect.Left, clientRect.Top), new PointF(q.DisplayRect.Left, clientRect.Bottom));
                                            g.DrawLine(p2, new PointF(q.DisplayRect.Right, 0), new PointF(q.DisplayRect.Right, clientRect.Bottom));
                                            g.DrawLine(p2, new PointF(clientRect.Left, q.DisplayRect.Top), new PointF(clientRect.Right, q.DisplayRect.Top));
                                            g.DrawLine(p2, new PointF(clientRect.Left, q.DisplayRect.Bottom), new PointF(clientRect.Right, q.DisplayRect.Bottom));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    g.Transform = ori;
                }
            });
        }
        private void DrawPaper(Graphics g)
        {
            if (Paper != null)
            {
                var clientLocation = new PointF(ClientRectangle.Location.X - 1, ClientRectangle.Location.Y - 1);
                var clientSize = new SizeF((ClientRectangle.Width + 2) / Paper.ZoomRatio, (ClientRectangle.Height + 2)/ Paper.ZoomRatio);
                var clientRect = new RectangleF(clientLocation, clientSize);

                var paperLocation = new PointF(Paper.PaperRect.Location.X - 1, Paper.PaperRect.Location.Y - 1);
                var paperSize = new SizeF(Paper.PaperRect.Width + 2, Paper.PaperRect.Height + 2);
                var paperRect = new RectangleF(paperLocation, paperSize);
                clientRect.Intersect(paperRect);
                g.Clip = new Region(clientRect);

                using (Pen p = new Pen(SystemColors.GrayText, 1))
                {
                    using (SolidBrush brush = new SolidBrush(Color.White))
                    {
                        RectangleF IntersectRect = new RectangleF(Paper.Location, Paper.SizePx);
                        paperRect.Intersect(clientRect);
                        g.FillRectangle(brush, IntersectRect);
                        g.DrawRectangle(p, Rectangle.Round(IntersectRect));
                    }
                }
            }
        }
        private void DrawCell(TableObj obj, Graphics g, Pen pen)
        {
            using (SolidBrush selectedBrush = new SolidBrush(Color.LightGray))
            using (SolidBrush strbrush = new SolidBrush(Color.Black))
            {
				foreach (var cellObj in obj.CellObjs)
                {
                    g.DrawRectangle(pen, Rectangle.Round(cellObj.DisplayRect));
					if (cellObj.IsSelected)
                        g.FillRectangle(selectedBrush, cellObj.DisplayRect);
                    g.DrawString(cellObj.Text.String, cellObj.Text.Font, strbrush, cellObj.DisplayRect, cellObj.Text.StringFormat);
                }
            }
        }
        private void DrawAnimation(Graphics g)
        {
            if (IsMode(enMode.IsDragging))
            {
                if (IsMode(enMode.Draw))
                {
                    using (Pen p = new Pen(Color.Red, 2) { DashStyle = DashStyle.DashDot })
                    {
                        if (IsDrawMode(enDraw.DrawLine))
                        {
                            Tuple<PointF, PointF> pt = DrawLine(_clickPoint, _curPoint);
                            g.DrawLine(p, pt.Item1, pt.Item2);
                        }
                        else
                            g.DrawRectangle(p, Rectangle.Round(GetDrawnRect()));
                    }
                }
                else
                {
                    switch (enHit)
                    {
                        case enHit.None:
                            {
                                using (Pen p = new Pen(Color.Red, 2) { DashStyle = DashStyle.DashDot })
                                    g.DrawRectangle(p, Rectangle.Round(GetDrawnRect()));
                            }
                            break;
                        case enHit.MainObjAdjustHandler:
                            {
                                if (_clickedObj is AdjustHandler handler && handler.Parent is MainObj parent)
                                {
                                    if (parent.IsLocked)
                                        return;

                                    if (parent is LineObj line)
                                    {
                                        PointF[] linePts = new PointF[2] { line.DisplayPt1, line.DisplayPt2};
                                        var otherPt = handler.CenterPoint.GetFarthest(linePts);
                                        var location = DrawLine(otherPt, CurPoint);
                                        using (Pen p = new Pen(Color.Red, 2) { DashStyle = DashStyle.DashDot })
                                            g.DrawLine(p, location.Item1, location.Item2);
                                    }
                                    else if (parent is ImageObj imageObj)
                                    {
                                        RectangleF rect = AdjustImageObj(handler);
                                        RectangleF rectToDraw = new RectangleF(new PointF(rect.X, rect.Y), rect.Size);

                                        using (Pen p = new Pen(Color.Red, 2) { DashStyle = DashStyle.DashDot })
                                            g.DrawRectangle(p, Rectangle.Round(rectToDraw));
                                    }
                                    else if (parent is MainObj mainObj)
                                    {
                                        RectangleF rect = AdjustSize(handler);
                                        RectangleF rectToDraw = new RectangleF(new PointF(rect.X, rect.Y), rect.Size);

                                        using (Pen p = new Pen(Color.Red, 2) { DashStyle = DashStyle.DashDot })
                                            g.DrawRectangle(p, Rectangle.Round(rectToDraw));
                                    }
                                }
                            }
                            break;
                        case enHit.MainObjMoveHandler:
                            {
                                if (_clickedObj.Parent is MainObj mainObj)
                                {
                                    if (mainObj.IsLocked)
                                        return;

                                    SizeF offset = new SizeF(ClickPoint.X - CurPoint.X, ClickPoint.Y - CurPoint.Y);
                                    PointF point = new PointF(mainObj.DisplayRect.X - offset.Width, mainObj.DisplayRect.Y - offset.Height);
                                    RectangleF rect = new RectangleF(point, mainObj.DisplayRect.Size);
                                    //if (Convert.ToBoolean(enSelState & enSelectionState.SingleSelect)
                                    //    && Convert.ToBoolean(ModifierKeys & Keys.Alt))
                                    //    rect = Rectangle.Round(GetNearestDockedPos(rect));

                                    using (Pen p = new Pen(Color.Red, 2) { DashStyle = DashStyle.DashDot })
                                        g.DrawRectangle(p, Rectangle.Round(rect));
                                }
                            }
                            break;
                        case enHit.CellSizeHandler:
                            {
                                if (_clickedObj is CellSizeHandler handler && handler.Parent is MainObj mainObj)
                                {
                                    using (Pen p = new Pen(Color.Red, 2) { DashStyle = DashStyle.DashDot })
                                    {
                                        switch (handler.enHandlerMatrix)
                                        {
                                            case enHandlerMatrix.Row:
                                                if (CurPoint.Y > handler.Parent.DisplayRect.Top && CurPoint.Y < handler.Parent.DisplayRect.Bottom &&
                                                    CurPoint.Y > handler.Parent.RowPosition[handler.CellMatrix.Row - 1] && CurPoint.Y < handler.Parent.RowPosition[handler.CellMatrix.Row + 1])
                                                {
                                                    g.DrawLine(p, new PointF(handler.Parent.DisplayRect.Left, CurPoint.Y), new PointF(handler.Parent.DisplayRect.Right, CurPoint.Y));
                                                    SetMode(enMode.CellSizeAdjust, true);
                                                }
                                                else
                                                    SetMode(enMode.CellSizeAdjust, false);
                                                break;
                                            case enHandlerMatrix.Column:
                                                if (CurPoint.X > handler.Parent.DisplayRect.Left && CurPoint.X < handler.Parent.DisplayRect.Right &&
                                                    CurPoint.X > handler.Parent.ColPosition[handler.CellMatrix.Col - 1] && CurPoint.X < handler.Parent.ColPosition[handler.CellMatrix.Col + 1])
                                                {
                                                    g.DrawLine(p, new PointF(CurPoint.X, handler.Parent.DisplayRect.Top), new PointF(CurPoint.X, handler.Parent.DisplayRect.Bottom));
                                                    SetMode(enMode.CellSizeAdjust, true);
                                                }
                                                else
                                                    SetMode(enMode.CellSizeAdjust, false);
                                                break;
                                        }
                                    }
                                }
                                else
                                    SetMode(enMode.CellSizeAdjust, false);
                            }
                            break;
                        case enHit.CellObj:
                            {
                                if (_clickedObj is CellObj _clickedCell)
                                {
                                    TableObj table = _clickedCell.Table;
                                    CellObj _curCell = table?.CellObjs.Find(q => q.DisplayRect.Contains(_curPoint));
                                    if (_clickedCell != null && _curCell != null)
                                    {
                                        List<RectangleF> rect = new List<RectangleF>() { _clickedCell.DisplayRect, _curCell.DisplayRect };
                                        float left = rect.Min(q => q.Left);
                                        float right = rect.Max(q => q.Right);
                                        float top = rect.Min(q => q.Top);
                                        float bottom = rect.Max(q => q.Bottom);

                                        using (Pen p = new Pen(Color.Red, 2) { DashStyle = DashStyle.DashDot })
                                            g.DrawRectangle(p, Rectangle.Round(RectangleF.FromLTRB(left, top, right, bottom)));
                                    }
                                }
                            }
                            break;
                    }
                }
            }
        }
        #endregion

        #region ContextMenu
        //private void CreateContext()
        //{
        //    if (ContextMenuStrip != null)
        //    {
        //        ContextMenuStrip.Close();
        //        ContextMenuStrip.Dispose();
        //        ContextMenuStrip = null;
        //    }
        //    ContextMenuStrip = new ContextMenuStrip();
        //}
        private void ShowContext()
        {
            try
            {
                if (IsHit(enHit.CellObj))
                {
                    if (_clickedObj.Parent is TableObj)
                    {
                        var table = _clickedObj.Parent as TableObj;
                        var cells = table.CellObjs.FindAll(q => q.IsSelected);
                        CellObjContextProp cellProp = null;
                        if (cells.Count > 1)
                        {
                            cellProp = new CellObjContextProp(PointToScreen(_curPoint.GetActualPoint()))
                            {
                                Alignment = eAlignment.CenterAlign,
                                LineAlignment = eLineAlignment.CenterAlign,
                                Font = new Font("Arial", 10f),
                                SelCellObjs = cells,
                            };
                        }
                        else
                        {
                            cellProp = new CellObjContextProp(PointToScreen(_curPoint.GetActualPoint()))
                            {
                                Alignment = cells.FirstOrDefault()?.Text.Alignment ?? eAlignment.LeftAlign,
                                LineAlignment = cells.FirstOrDefault()?.Text.LineAlignment ?? eLineAlignment.TopAlign,
                                Font = cells.FirstOrDefault()?.Text.Font,
                                Text = cells.FirstOrDefault()?.Text.String,
                                SelCellObjs = cells,
                            };
                        }
                        DesignerEvent.OnPopupContextMenu(this, cellProp);
                    }
                }
                else if (IsHit(enHit.TextObj))
                {

                }
                //if (IsHit(enHit.TextObj) || IsHit(enHit.CellObj))
                //{
                //    var textProp = new TextObjProp()
                //    {
                //        Alignment = IsHit(enHit.TextObj) ? eAlignment.LeftAlign : eAlignment.CenterAlign,
                //        LineAlignment = eLineAlignment.CenterAlign,
                //        Angle = 0,
                //        Font = new Font("Arial", 10f),
                //        Text = "",
                //    };
                //    if (DesignManager.TextObjs.Where(q => q.IsFocused).ToList().Count == 1)
                //    {
                //        var text = DesignManager.TextObjs.First();
                //        textProp = new TextObjProp()
                //        {
                //            Alignment = text.Alignment,
                //            LineAlignment = text.LineAlignment,
                //            Angle = 0,
                //            Font = text.Font,
                //        };
                //    }
                //    var context = new ucCellContextMenu()
                //    {
                //        ParentMenuStrip = ContextMenuStrip,
                //        TextObjProp = textProp,
                //    };

                //    ContextMenuStrip.Items.Add(new ContextBase(context));
                //    ContextMenuStrip.Show(PointToScreen(_curPoint.GetActualPoint()));
                //}

                //else if (CurObj is MsgBox)
                //{
                //    //iMETextBox.Add((CurObj as MsgBox).Text);
                //}
                //if (iMETextBox.Count > 0)
                //{
                //    CellContextMenu.InitializeForm(iMETextBox);
                //    ContextMenuStrip.Items.Add(new ContextBase(CellContextMenu));

                //    ucCellContextMenuDetail detailItem = new ucCellContextMenuDetail(enCustomDocumentVariable.Details.ToString());
                //    detailItem.InitializeDetail(iMETextBox);
                //    ContextMenuStrip.Items.Add(detailItem);
                //    CellContextMenu.ParentMenuStrip = ContextMenuStrip;

                //    if (Variables.DocumentInfo.Barcodes.Count > 0)
                //    {
                //        ucCellContextHumanReadable humanReadable = new ucCellContextHumanReadable(enCustomDocumentVariable.HumanReadable.ToString());
                //        humanReadable.InitializeDetail(iMETextBox, Variables.DocumentInfo.Barcodes);
                //        ContextMenuStrip.Items.Add(humanReadable);
                //        CellContextMenu.ParentMenuStrip = ContextMenuStrip;
                //    }
                //}
                //if (ContextMenuStrip.Items.Count > 0)
                //    ContextMenuStrip.Show(PointToScreen(PointConverter.GetActualPoint(_curPoint)));
            }
            catch (Exception ex)
            {
            }
        }
        #endregion
    }
}
