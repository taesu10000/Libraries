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
    public class ImageObj : MainObj
    {
        [JsonIgnore]
        public Bitmap Bitmap { get; set; }
        [JsonIgnore]
        public SizeF OriginalBitmapSize { get; private set; }
        public string BitmapPath { get; set; }
        public ImageObj()
        {
        }

        public ImageObj(RectangleF objectRect, string bitmapPath)
        {
            SetBitmap(bitmapPath);
            Location = new PointF(objectRect.Location.X - Paper.PaperRect.Location.X, objectRect.Location.Y - Paper.PaperRect.Location.Y);
            float ratio = OriginalBitmapSize.Width / OriginalBitmapSize.Height;
            float longerSide = objectRect.Width < objectRect.Height ? objectRect.Height : objectRect.Width;
            Size = new SizeF(longerSide, longerSide / ratio);
            DrawImage();

            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.LT));
            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.RT));
            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.LB));
            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.RB));

            this.MoveHandlers.Add(new MoveHandler(this, enRectSIde.Top));
            this.MoveHandlers.Add(new MoveHandler(this, enRectSIde.Bottom));
            this.MoveHandlers.Add(new MoveHandler(this, enRectSIde.Left));
            this.MoveHandlers.Add(new MoveHandler(this, enRectSIde.Right));
        }
        public ImageObj(ImageObj mainObj)
        {
            Location = mainObj.Location;
            Size = mainObj.Size;
            SetBitmap(mainObj.BitmapPath);

            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.LT));
            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.RT));
            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.LB));
            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.RB));

            this.MoveHandlers.Add(new MoveHandler(this, enRectSIde.Top));
            this.MoveHandlers.Add(new MoveHandler(this, enRectSIde.Bottom));
            this.MoveHandlers.Add(new MoveHandler(this, enRectSIde.Left));
            this.MoveHandlers.Add(new MoveHandler(this, enRectSIde.Right));
        }
        public void SetBitmap(string bitmapPath)
        {
            if (!string.IsNullOrEmpty(bitmapPath) && System.IO.File.Exists(bitmapPath))
            {
                try
                {
                    BitmapPath = bitmapPath;
                    //var bitmap = new Bitmap(bitmapPath);
                    if (Bitmap != null)
                    {
                        Bitmap.Dispose();
                        Bitmap = null;
                    }
                    Bitmap = new Bitmap(BitmapPath);
                    OriginalBitmapSize = Bitmap.Size;
                    //if (Convert.ToInt32(Size.Width) > 0 && Convert.ToInt32(Size.Height) > 0)
                    //    Bitmap = new Bitmap(bitmap, new Size(Convert.ToInt32(Size.Width), Convert.ToInt32(Size.Height)));
                }
                catch { }
            }
        }
        public void DrawImage()
        {
            if (System.IO.File.Exists(BitmapPath))
            {
                if (Bitmap != null)
                {
                    Bitmap.Dispose();
                    Bitmap = null;
                }

                Bitmap = new Bitmap(BitmapPath);
                OriginalBitmapSize = Bitmap.Size;
                //if (Convert.ToInt32(Size.Width) > 0 && Convert.ToInt32(Size.Height) > 0)
                //    Bitmap = new Bitmap(bitmap, new Size(Convert.ToInt32(Size.Width), Convert.ToInt32(Size.Height)));
            }
        }
        public Bitmap GetImageSource()
        {
            if (string.IsNullOrEmpty(BitmapPath) || System.IO.File.Exists(BitmapPath))
                return Bitmap;
            return null;
        }
    }
}
