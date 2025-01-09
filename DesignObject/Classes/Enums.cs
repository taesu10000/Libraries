using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignObject
{
    public enum enHandlerMatrix
    {
        Row = 0,
        Column
    }
    public enum enRectSIde
    {
        Left = 1,
        Right = Left << 1,
        Top = Left << 2,
        Bottom = Left << 3,
        LT = Left | Top, // 4
        RT = Right | Top,
        LB = Left | Bottom,
        RB = Right | Bottom
    }
    public enum enMode : long
    {
        None = 1, // 1
        IsDragging = None << 1, //
        SizeAdjust = None << 2,
        CellSizeAdjust = None << 3,
        OnClick = None << 4,
        SingleCellSelected = None << 5,
        MultiCellsSelected = None << 6,
        Draw = None << 7,
    }
    public enum enSelectionState
    {
        None = 1,
        MultiSelect = None << 1,
        SingleSelect = None << 2,
    }
    public enum enDraw : long
    {
        None = 1,
        DrawBarcode = None << 1,
        DrawMessageBox = None << 2,
        DrawCircle = None << 3,
        DrawRect = None << 4,
        DrawLine = None << 5,
        DrawImage = None << 6,
        DrawTable = None << 7,
    }
    public enum enModifiers : long
    {
        None = 1,
        Shift = None << 1,
        Control = None << 2,
        ControlShift = Control | Shift,
    }
    public enum enHit : long
    {
        None = 1,
        MainObjAdjustHandler = None << 1,
        MainObjMoveHandler = None << 2,
        CellObj = None << 3,
        CellSizeHandler = None << 4,
        BarcodeObj = None << 5,
        TextObj = None << 6,
        Figures = None << 7,
        ImageObj = None << 8,
    }
    public enum enBarcodeType : long
    {
        DataMatrix = 1,
        Code128 = DataMatrix << 1,
        QR = DataMatrix << 2,
        ITF = DataMatrix << 3,

        OneDBarcode = Code128 | ITF,
        TwoDBarcode = DataMatrix | QR
    }
    public enum enPaperSizeType : long
    {
        A0 = 1,
        A1,
        A2,
        A3,
        A4,
        A5,
        Custom,
    }
    public enum enCustomDocumentVariable
    {
        None = 0,
        ShipmentOrderSeqNo,
        CorrentUser,
        Details,
        HumanReadable,
        Index,
        Barcode,
    }
    public enum enButtons
    {
        NewDocument,
        Save,
        Load,
        NewTable,
        TextBox,
        Barcode,
        Ellipse,
        Rectangle,
        Line,
        Image,
        Print,
        PaperSize,
        Property,
        AlignLeft,
        AlignRight,
        AlignTop,
        AlignBottom,
        Redo,
        Undo,
        Copy,
        Cut,
        Paste,

        GenBarcode,
    }
    public enum enPopupDialog
    {
        Printer,
    }
    public enum enAlign
    {
        Top,
        Bottom,
        Left,
        Right,
    }
    public enum enRotate
    {
        ClockWise,
        AntiClockWise,

    }
    public enum PaperDisplayMode
    {
        Scale = 1,
        Wheel = Scale << 1,
        Rotation = Scale << 2,

        Default = Scale | Wheel,
        Print = Scale | Rotation,
    }
    public enum Unit
    {
        mm,
        inch,
    }
	public enum EnDateOrder : long
	{
		yyyyMMdd,
		MMddyyyy,
		ddMMyyyy,
		yyyyddMM,
		ddyyyyMM,
		MMyyyydd,
	}
	public enum EnDateFormat : long
	{
		Custom = 0,
		Year = 1 << 1,
		Month = 1 << 2,
		Day = 1 << 3,
		Hour = 1 << 4,
		Minute = 1 << 5,
		Second = 1 << 6,
		Millisecond = 1 << 7,
		AMPM = 1 << 8,
		TwentyfourHourSystem = 1 << 9,
	}

	public enum EnDateYearMonthFormat : long
	{
		yyMM,
		yyyyMM,
		yyMMM,
		yyyyMMM,
	}
	public enum EnDateTimeSeparator : long
	{
		None,
		Dash,
		Space,
		Slash,
		Colon,
		Dot,
	}
	public enum UpDown
	{
		Up = 0,
		Down,
	}
	public enum EnLanguage
	{
		English = 1033, //English (United States) = 1033, en-US
		Korean = 1042, //Korean (Korea) = 1042, ko-KR
	}
}
