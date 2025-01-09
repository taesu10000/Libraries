using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DesignObject
{
    public interface IWritable : IDocumentParam
    {
        TextObj Text { get; set; }
    }
    public interface IVariableText
    {
        string String { get; set; }
    }
    public interface IComponent
    {
        PointF CenterPoint { get; }
        bool Contains(RectangleF rect);
    }
    public interface IBarcodeProp : IDocumentParam
    {
        char GroupSeparator { get; }
        enBarcodeType BarcodeType { get; set; }
        string String { get; set; }
        bool IsGS1 { get; set; }
        bool IsOneDBarcode { get; }
        bool ForceSquare { get; set; }
    }
    public interface IDisplayRect
    {
        RectangleF DisplayRect { get; }
    }
    public interface IDocumentParam
    {
        //string Text { get; set; }
        object TagParam { get; set; }
    }
    public interface IFigures
    {
        GraphicsPath GraphicsPath { get; }
        GraphicsPath DisplayGraphicsPath { get; }
    }
}
