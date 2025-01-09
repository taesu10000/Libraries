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
    public class TextObj : IDisplayRect, IDocumentParam
    {
        [JsonIgnore]
        public const char NULLCHAR = (char)0;
        [JsonIgnore]
        public StringFormat StringFormat
        {
            get
            {
                StringFormat stringFormat = new StringFormat();
                Enum.TryParse(_alignment, out StringAlignment alignment);
                Enum.TryParse(_lineAlignment, out StringAlignment lAlignment);
                stringFormat.Alignment = alignment;
                stringFormat.LineAlignment = lAlignment;
                stringFormat.Trimming = StringTrimming.None;
                stringFormat.FormatFlags = StringFormatFlags.NoClip | StringFormatFlags.MeasureTrailingSpaces;
                if (NoWrap)
                    stringFormat.FormatFlags |= StringFormatFlags.NoWrap;

                return stringFormat;
            }
            set
            {
                _alignment = value.Alignment.ToString();
                _lineAlignment = value.LineAlignment.ToString();
            }
        }
        [JsonIgnore]
        public eLineAlignment LineAlignment
        {
            get
            {
                Enum.TryParse(_lineAlignment, out StringAlignment lAlignment);
                switch (lAlignment)
                {
                    case StringAlignment.Near:
                        return eLineAlignment.TopAlign;
                    case StringAlignment.Center:
                        return eLineAlignment.CenterAlign;
                    case StringAlignment.Far:
                        return eLineAlignment.BottomAlign;
                }
                return eLineAlignment.CenterAlign;
            }
            set
            {
                switch (value)
                {
                    case eLineAlignment.TopAlign:
                        _lineAlignment = StringAlignment.Near.ToString();
                        break;
                    case eLineAlignment.CenterAlign:
                        _lineAlignment = StringAlignment.Center.ToString();
                        break;
                    case eLineAlignment.BottomAlign:
                        _lineAlignment = StringAlignment.Far.ToString();
                        break;
                }
            }
        }
        [JsonIgnore]
        public eAlignment Alignment
        {
            get
            {
                Enum.TryParse(_alignment, out StringAlignment alignment);
                switch (alignment)
                {
                    case StringAlignment.Near:
                        return eAlignment.LeftAlign;
                    case StringAlignment.Center:
                        return eAlignment.CenterAlign;
                    case StringAlignment.Far:
                        return eAlignment.RightAlign;
                }
                return eAlignment.CenterAlign;
            }
            set
            {
                switch (value)
                {
                    case eAlignment.LeftAlign:
                        _alignment = StringAlignment.Near.ToString();
                        break;
                    case eAlignment.CenterAlign:
                        _alignment = StringAlignment.Center.ToString();
                        break;
                    case eAlignment.RightAlign:
                        _alignment = StringAlignment.Far.ToString();
                        break;
                }
            }
        }
        [JsonIgnore]
        public Font Font
        {
            get
            {
                return new Font(FontName, FontSize, FontStyle);
            }
            set
            {
                FontName = value.FontFamily.Name;
                FontSize = (int)value.Size;
                FontStyle = value.Style;
            }
        }
        [JsonIgnore]
        public DrawingObjBase Parent { get; set; }

        [JsonIgnore]
        public int Length
        {
            get
            {
                if (string.IsNullOrEmpty(String))
                    return 0;
                return String.Length;
            }
        }
        [JsonIgnore]
        public RectangleF DisplayRect
        {
            get
            {
                var obj = Parent as IDisplayRect;
                return obj.DisplayRect;
            }
        }
        public string String { get; set; } = "";
        public string _lineAlignment { get; set; }
        public string _alignment { get; set; }
        public bool NoWrap { get; set; } = true;
        public string FontName { get; set; } = "Arial";
        public float FontSize { get; set; } = 15f;
        public FontStyle FontStyle { get; set; } = FontStyle.Regular;
        public object TagParam { get; set; } = null;
        public TextObj()
        { }
        public TextObj(DrawingObjBase parent)
        {
            Parent = parent;
            if (parent is CellObj)
            {
                LineAlignment = eLineAlignment.CenterAlign;
                Alignment = eAlignment.CenterAlign;
            }
            else if (parent is TextObj)
            {
                LineAlignment = eLineAlignment.CenterAlign;
                Alignment = eAlignment.LeftAlign;
            }
        }
        public TextObj(DrawingObjBase parent, TextObj text)
        {
            Parent = parent;
            LineAlignment = text.LineAlignment;
            Alignment = text.Alignment;
            FontName = text.Font.Name;
            FontSize = text.Font.Size;
            FontStyle = text.Font.Style;
            String = text.String;
            StringFormat = text.StringFormat;
            NoWrap = text.NoWrap;
        }
        public TextObj Clone()
        {
            TextObj text = this.MemberwiseClone() as TextObj;
            return text;
        }
    }
}
