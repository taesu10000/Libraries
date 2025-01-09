using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DesignObject
{
    public class CopyPasteManager
    {
        List<MainObj> ClipBoard = new List<MainObj>();
        DesignObjManager designObjManager { get { return DesignerVariables.Instance.DesignObjManager; } }
        public CopyPasteManager()
        {
            DesignerEvent.Copy += ((sender, e) => Copy());
            DesignerEvent.Paste += ((sender, e) => Paste());
            DesignerEvent.Cut += ((sender, e) => Cut());
        }
        public void Copy()
        {
            ClipBoard.Clear();
            if (designObjManager.SelectedMainObj.Count > 0)
            {
                designObjManager.SelectedMainObj.ForEach(q => ClipBoard.Add(q.Clone() as MainObj));
            }
        }
        public void Cut()
        {
            ClipBoard.Clear();
            if (designObjManager.SelectedMainObj.Count > 0)
                designObjManager.SelectedMainObj.ForEach(q => ClipBoard.Add(q.Clone() as MainObj));
            designObjManager.DeleteObj();
        }

        public void Paste()
        {
            designObjManager.SelectedMainObj.ForEach(q => q.Select(false));

            if (ClipBoard.Count == 0)
                return;
            List<MainObj> pasted = new List<MainObj>();
            ClipBoard.ForEach(q =>
            {
                q.Location = new PointF(q.Location.X + 10, q.Location.Y + 10);
                if (q is TableObj)
                {
                    var obj = q.Clone() as TableObj;
                    designObjManager.AddTable(obj);
                    pasted.Add(obj);
                }
                else if (q is BarcodeObj)
                {
                    var obj = q.Clone() as BarcodeObj;
                    designObjManager.AddBarcode(obj);
                    pasted.Add(obj);
                }
                else if (q is TextBoxObj)
                {
                    var obj = q.Clone() as TextBoxObj;
                    designObjManager.AddTextBox(obj);
                    pasted.Add(obj);
                }
                else if (q is ImageObj)
                {
                    var obj = q.Clone() as ImageObj;
                    designObjManager.AddImage(obj);
                    pasted.Add(obj);
                }
                else if (q is RectObj)
                {
                    var obj = q.Clone() as RectObj;
                    designObjManager.AddRect(obj);
                    pasted.Add(obj);
                }
                else if (q is CircleObj)
                {
                    var obj = q.Clone() as CircleObj;
                    designObjManager.AddCircle(obj);
                    pasted.Add(obj);
                }
                else if (q is LineObj)
                {
                    var obj = q.Clone() as LineObj;
                    designObjManager.AddLine(obj);
                    pasted.Add(obj);
                }
            });
            pasted.ForEach(q => q.Select(true));
        }
    }
}