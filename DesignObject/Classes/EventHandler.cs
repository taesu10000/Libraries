using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesignObject
{
    public class DesignerEvent
    {
        public static event EventHandler SelectObject;
        public static event EventHandler Copy;
        public static event EventHandler Paste;
        public static event EventHandler Cut;
        public static event EventHandler Undo;
        public static event EventHandler Redo;
        public static event EventHandler Invalidate;
        public static event EventHandler PopUpPrinterSetting;
		public static event EventHandler FocusDesigner;
		public static event EventHandler Property;

		public static event EventHandler<ButtonEventArgs> ButtonPushEventHandler;
		public static event EventHandler ButtonReleaseEventHandler;
		public static event EventHandler<PathArgs> LoadEventHandler;
        public static event EventHandler<PathArgs> SaveEventHandler;
        public static event EventHandler<PathArgs> LoadImageEventHandler;

        public static event EventHandler<Paper> PopUpPaperSetting;
        public static event EventHandler<MainObjProp> MoveHandlerDoubleClick;
        public static event EventHandler<TextObjProp> PopUpTextObjPropSetting;
        public static event EventHandler<BarcodeProp> PopUpBarcodePropSetting;
        public static event EventHandler<MainObjProp> PopUpCreateTable;
        public static event EventHandler<TableProp> CreateTable;
		public static event EventHandler<CellObjProp> CellObjDoubleClick;
		public static event EventHandler<CellObjContextProp> PopupCellProp;
        public static event EventHandler<ContextMenuProp> PopupContextMenu;
        public static event EventHandler<Paper> NewPaper;
        public static event EventHandler<TagProp> TagPropClick;
		public static event EventHandler<ZPosProp> ZPosChanged;
		public static void OnObjectSelected()
		{
			SelectObject?.Invoke(null, EventArgs.Empty);
		}
		public static void OnCopy()
        {
            Copy?.Invoke(null, EventArgs.Empty);
        }
        public static void OnPaste()
        {
            Paste?.Invoke(null, EventArgs.Empty);
        }
        public static void OnCut()
        {
            Cut?.Invoke(null, EventArgs.Empty);
        }
        public static void OnUndo()
        {
            Undo?.Invoke(null, EventArgs.Empty);
        }
        public static void OnRedo()
        {
            Redo?.Invoke(null, EventArgs.Empty);
        }
        public static void OnInvalidate()
        {
            Invalidate?.Invoke(null, EventArgs.Empty);
        }        
        public static void OnProperty()
        {
            Property?.Invoke(null, EventArgs.Empty);
        }
        public static void OnButtonPush(ButtonEventArgs e)
        {
            ButtonPushEventHandler?.Invoke(null, e);
        }
		public static void OnButtonRelease()
		{
			ButtonReleaseEventHandler?.Invoke(null, EventArgs.Empty);
		}
		public static void OnPopUpPrinterSetting()
        {
            PopUpPrinterSetting?.Invoke(null, EventArgs.Empty);
		}
		public static void OnFocusDesigner()
		{
			FocusDesigner?.Invoke(null, EventArgs.Empty);
		}
		public static void OnPopUpTextObjPropSetting(TextObjProp e)
        {
            PopUpTextObjPropSetting?.Invoke(null, e);
        }
        public static void OnPopUpBarcodePropSetting(BarcodeProp e)
        {
            PopUpBarcodePropSetting?.Invoke(null, e);
        }
        public static void OnLoad(PathArgs e)
        {
            LoadEventHandler?.Invoke(null, e);
        }
        public static void OnSave(PathArgs e)
        {
            SaveEventHandler?.Invoke(null, e);
        }
        public static void OnPopUpPaperSetting(Paper e)
        {
            PopUpPaperSetting?.Invoke(null, e);
        }
        public static void OnLoadImage(PathArgs e)
        {
            LoadImageEventHandler?.Invoke(null, e);
        }
        public static void OnMoveHandlerDoubleClick(MainObjProp mainObjProp)
        {
            MoveHandlerDoubleClick?.Invoke(null, mainObjProp);
        }
        public static void OnPopUpCreateTable(MainObjProp mainObjProp)
        {
            PopUpCreateTable?.Invoke(null, mainObjProp);
        }
        public static void OnCreateTable(TableProp tableProp)
        {
            CreateTable?.Invoke(null, tableProp);
		}
		public static void OnPopUpCellObjProp(List<CellObj> cells, CellObjContextProp tableProp)
		{
			PopupCellProp?.Invoke(cells, tableProp);
		}
		public static void OnCellObjDoubleClick(CellObj cells, CellObjProp cellProp)
        {
			CellObjDoubleClick?.Invoke(cells, cellProp);
        }
        public static void OnNewPaper(Paper e)
        {
            NewPaper?.Invoke(null, e);
        }
        public static void OnClickTagProp(TagProp e)
        {
            TagPropClick?.Invoke(null, e);
        }
        public static void OnPopupContextMenu(object sender, ContextMenuProp e)
        {
            PopupContextMenu?.Invoke(sender, e);
        }
		public static void OnZPosChanged(object sender, ZPosProp e)
		{
			ZPosChanged?.Invoke(sender, e);
		}
		public static void Dispose()
        {
            SelectObject = null;
            Copy = null;
            Paste = null;
            Cut = null;
            Redo = null;
            Undo = null;
            Invalidate = null;
            PopUpPrinterSetting = null;
			FocusDesigner = null;
            Property = null;

			ButtonPushEventHandler = null;
            ButtonReleaseEventHandler = null;
            CellObjDoubleClick = null;
            CreateTable = null;
            LoadEventHandler = null;
            LoadImageEventHandler = null;
            MoveHandlerDoubleClick = null;
            NewPaper = null;
            PopUpBarcodePropSetting = null;
            PopupCellProp = null;
            PopupContextMenu = null;
            PopUpCreateTable = null;
            PopUpPaperSetting = null;
			PopUpTextObjPropSetting = null;
            SaveEventHandler = null;
            TagPropClick = null;
            ZPosChanged = null;
		}
    }
    public class ButtonEventArgs : EventArgs
    {
        public enButtons EnButtons;
        public ButtonEventArgs(enButtons enButtons)
        {
            EnButtons = enButtons;
        }
    }
    public class NewDocumentArgs : ButtonEventArgs
    {
        public Paper Paper { get; set; }
        public NewDocumentArgs(enButtons enButtons, Paper paper) : base(enButtons)
        {
            Paper = paper;
        }
    }
    public class PathArgs : ButtonEventArgs
    {
        public string DesignPath;
        public PathArgs(enButtons enButtons, string designPath) : base(enButtons)
        {
            DesignPath = designPath;
        }
    }
    public class NewTableEventArgs : EventArgs
    {
        public int ColCnt { get; set; }
        public int RowCnt { get; set; }
        public NewTableEventArgs(int colCnt, int rowCnt)
        {
            ColCnt = colCnt;
            RowCnt = rowCnt;
        }
    }
    public class ContextMenuProp : EventArgs
    {
        public PointF CurrentPt { get; set; }
        public ContextMenuProp(PointF pt)
        {
            CurrentPt = pt;
        }
    }
    public class TableProp : EventArgs
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public RectangleF Rect { get; set; }
        public TableProp(RectangleF rect, int row, int col)
        {
            Rect = rect;
            Row = row;
            Col = col;
        }
    }
    public class MainObjProp : ContextMenuProp
    {
        public RectangleF Rect { get; set; }
        public MainObjProp(PointF curPt, RectangleF rect) : base(curPt)
        {
            Rect = rect;
        }
    }
    public class BarcodeProp : EventArgs
    {
        public BarcodeObj Barcode { get; set; }
        public BarcodeProp(BarcodeObj barcode)
        {
            Barcode = barcode;
        }
    }
    public class TextObjProp : EventArgs
    {
        public int Angle { get; set; }
        public Font Font { get; set; }
        public string Text { get; set; }
        public bool NoWrap { get; set; }
        public eAlignment Alignment { get; set; }
        public eLineAlignment LineAlignment { get; set; }
        public List<IWritable> TextObjs { get; set; }
        public TextObjProp(List<IWritable> textObjs)
        {
            TextObjs = textObjs;

            var font = TextObjs.GroupBy(q => q.Text.Font);
            var text = TextObjs.GroupBy(q => q.Text.String);
            var noWrap = TextObjs.GroupBy(q => q.Text.NoWrap);
            var alignment = TextObjs.GroupBy(q => q.Text.Alignment);
            var lineAlignment = TextObjs.GroupBy(q => q.Text.LineAlignment);

            if (font.Count() >= 1)
                Font = font.First().Key;
            if (text.Count() >= 1)
                Text = text.First().Key;
            if (noWrap.Count() >= 1)
                NoWrap = noWrap.First().Key;
            if (alignment.Count() >= 1)
                Alignment = alignment.First().Key;
            if (lineAlignment.Count() >= 1)
                LineAlignment = lineAlignment.First().Key;
        }
    }
    public class CellObjProp : EventArgs
	{
        public CellObj CellObj { get; set; }
        public CellObjProp(CellObj cellObj) 
        { 
            CellObj = cellObj;
        }
    }
    public class CellObjContextProp : ContextMenuProp
    {
        public Font Font { get; set; }
        public string Text { get; set; }
        public eAlignment Alignment { get; set; }
        public eLineAlignment LineAlignment { get; set; }
        public List<CellObj> SelCellObjs { get; set; }
        public CellObjContextProp(PointF pt) : base(pt)
        { }
    }
    public class TagProp : EventArgs, IDocumentParam
    {
        public string Text { get; set; }
        public object TagParam { get; set; }
        public bool Cancel { get; set; } = false;
        public bool IsSelItemDetail { get; set; } = false;
        public TagProp(string text, object tagParam)
        {
            Text = text;
            TagParam = tagParam;
        }
    }
	public class ZPosProp : EventArgs
	{
		public UpDown UpDown { get; set; }
		public List<MainObj> MainObjs { get; set; }
		public ZPosProp(List<MainObj> mainObjs, UpDown upDown)
		{
			MainObjs = mainObjs;
			UpDown = upDown;
		}
	}
}
