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
    public abstract class DrawingObjBase : ICloneable
    {
        [JsonIgnore]
        public virtual DrawingObjBase Parent { get; set; }
        [JsonIgnore]
        public virtual RectangleF Rect { get; set; }
        [JsonIgnore]
        public virtual RectangleF DisplayRect { get; set; }
        [JsonIgnore]
        public bool _isSelected = false;
        [JsonIgnore]
        public virtual bool IsSelected { get { return _isSelected; } }
        [JsonIgnore]
        public virtual Paper Paper => DesignerVariables.Instance.Paper;
        public abstract object Clone();
        public virtual void Select(bool fbool = true)
        {
            _isSelected = fbool;
        }
        public DrawingObjBase GetMainObj()
        {
            DrawingObjBase obj = null;
            if (this is DrawingHandler)
                obj = this.Parent as MainObj;
            else if (this is CellObj)
                obj = this as CellObj;
            else
                obj = this as MainObj;
            return obj;
        }
        //public virtual bool Contains(RectangleF rect)
        //{
        //    return rect.Contains(CenterPoint);
        //}

    }
}
