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
    public class TextBoxObj : MainObj, IWritable
    {
        public TextObj Text { get; set; }
        [JsonIgnore]
        public object TagParam { get => Text.TagParam; set => Text.TagParam = value; }
        public TextBoxObj()
        { }
        public TextBoxObj(RectangleF objectRect)
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

            Text = new TextObj(this);
        }
        public TextBoxObj(TextBoxObj textBoxObj)
        {
            Location = textBoxObj.Location;
            Size = textBoxObj.Size;
            ZPos = textBoxObj.ZPos;

            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.LT));
            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.RT));
            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.LB));
            this.AdjustHandlers.Add(new AdjustHandler(this, enRectSIde.RB));

            this.MoveHandlers.Add(new MoveHandler(this, enRectSIde.Top));
            this.MoveHandlers.Add(new MoveHandler(this, enRectSIde.Bottom));
            this.MoveHandlers.Add(new MoveHandler(this, enRectSIde.Left));
            this.MoveHandlers.Add(new MoveHandler(this, enRectSIde.Right));

            Text = new TextObj(this, textBoxObj.Text);
        }
        public override void Select(bool fbool = true)
        {
            base.Select(fbool);
        }
    }
}
