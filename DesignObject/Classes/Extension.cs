using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace DesignObject
{
    public static class Extension
    {
        const float _inch = 25.4f;
        public static Point GetZoomedPoint(this Point point)
        {
            return new Point((int)GetZoomedLength(point.X), (int)GetZoomedLength(point.Y));
        }
        public static Point GetActualPoint(this Point point)
        {
            return new Point((int)GetActualLength(point.X), (int)GetActualLength(point.Y));
        }
        public static float GetZoomedLength(this float point)
        {
            return (point / DesignerVariables.Instance.ZoomRatio);
        }
        public static float GetActualLength(this float point)
        {
            return (point * DesignerVariables.Instance.ZoomRatio);
        }
        public static int GetZoomedLength(this int point)
        {
            return (int)(point / DesignerVariables.Instance.ZoomRatio);
        }
        public static int GetActualLength(this int point)
        {
            return (int)(point * DesignerVariables.Instance.ZoomRatio);
        }
        public static float mmToinch(this float mm)
        {
            return mm / _inch;
        }
        public static float inchTomm(this float inch)
        {
            return inch * _inch;
        }
        static Tuple<float, float> GetDpi()
        {
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                float dpiX = g.DpiX;
                float dpiY = g.DpiY;
                return new Tuple<float, float>(dpiX, dpiY);
            }
        }
        public static SizeMm PixelToMilliimeter(this SizeF size)
        {
            Tuple<float, float> dpi = GetDpi();
            SizeMm sizeMm = new SizeMm();
            sizeMm.X = size.Width.inchTomm() / dpi.Item1;
            sizeMm.Y = size.Height.inchTomm() / dpi.Item2;

            return sizeMm;
        }
        public static SizeMm PixelToInch(this SizeF size)
        {
            Tuple<float, float> dpi = GetDpi();
            SizeMm sizeMm = new SizeMm();
            sizeMm.X = size.Width / dpi.Item1;
            sizeMm.Y = size.Height / dpi.Item2;

            return sizeMm;
        }
        public static SizeF MilliimeterToPixel(this SizeMm size)
        {
            Tuple<float, float> dpi = GetDpi();
            SizeF newSize = new SizeF();
            newSize.Width = size.X.mmToinch() * dpi.Item1;
            newSize.Height = size.Y.mmToinch() * dpi.Item2;

            return newSize;
        }
        public static Point GetFarthest(this Point pt, Point[] args)
        {
            var min = args.Min(q => Math.Sqrt(Math.Pow(pt.X - q.X, 2) + Math.Pow(pt.Y - q.Y, 2)));
            var max = args.Max(q => Math.Sqrt(Math.Pow(pt.X - q.X, 2) + Math.Pow(pt.Y - q.Y, 2)));
            var farthest = args.FirstOrDefault(q => Math.Sqrt(Math.Pow(pt.X - q.X, 2) + Math.Pow(pt.Y - q.Y, 2)) == max);
            return farthest;
        }
        public static PointF GetFarthest(this PointF pt, PointF[] args)
        {
            var min = args.Min(q => Math.Sqrt(Math.Pow(pt.X - q.X, 2) + Math.Pow(pt.Y - q.Y, 2)));
            var max = args.Max(q => Math.Sqrt(Math.Pow(pt.X - q.X, 2) + Math.Pow(pt.Y - q.Y, 2)));
            var farthest = args.FirstOrDefault(q => Math.Sqrt(Math.Pow(pt.X - q.X, 2) + Math.Pow(pt.Y - q.Y, 2)) == max);
            return farthest;
        }
        public static Point GetClisest(this Point pt, Point[] args)
        {
            var min = args.Min(q => Math.Sqrt(Math.Pow(pt.X - q.X, 2) + Math.Pow(pt.Y - q.Y, 2)));
            var max = args.Max(q => Math.Sqrt(Math.Pow(pt.X - q.X, 2) + Math.Pow(pt.Y - q.Y, 2)));
            var farthest = args.FirstOrDefault(q => Math.Sqrt(Math.Pow(pt.X - q.X, 2) + Math.Pow(pt.Y - q.Y, 2)) == min);
            return farthest;
        }
        public static PointF GetClosest(this PointF pt, PointF[] args)
        {
            var min = args.Min(q => Math.Sqrt(Math.Pow(pt.X - q.X, 2) + Math.Pow(pt.Y - q.Y, 2)));
            var max = args.Max(q => Math.Sqrt(Math.Pow(pt.X - q.X, 2) + Math.Pow(pt.Y - q.Y, 2)));
            var farthest = args.FirstOrDefault(q => Math.Sqrt(Math.Pow(pt.X - q.X, 2) + Math.Pow(pt.Y - q.Y, 2)) == min);
            return farthest;
        }
    }
}
