using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DesignObject
{
    public class DesignObjManager : IEquatable<DesignObjManager>
    {
        [JsonIgnore]
        public Paper _paper { get; set; }
        public string Tittle { get; set; }
        public string Path { get; set; }
        public List<TableObj> TableObjs = new List<TableObj>();
        public List<TextBoxObj> TextBoxObjs = new List<TextBoxObj>();
        public List<BarcodeObj> BarcodeObjs = new List<BarcodeObj>();
        public List<CircleObj> CircleObjs = new List<CircleObj>();
        public List<ImageObj> ImageObjs = new List<ImageObj>();
        public List<RectObj> RectObjs = new List<RectObj>();
        public List<LineObj> LineObjs = new List<LineObj>();
        [JsonIgnore]
        public BarcodeObj MasterBarcode
        {
            get
            {
                if (BarcodeObjs?.Count == 0)
                    return null;

                BarcodeObj barcodeObj = BarcodeObjs.FirstOrDefault(q => q.PrimaryBarcode);
                if (barcodeObj != null)
                    return barcodeObj;

                barcodeObj = BarcodeObjs.FirstOrDefault(q => q.BarcodeType == enBarcodeType.DataMatrix)
                    ?? BarcodeObjs.FirstOrDefault(q => q.GetMessage().Contains("#sscc#"))
                    ?? BarcodeObjs.OrderByDescending(q => q.GetMessage().Length).FirstOrDefault(q => (q.GetMessage().Contains("#gtin#") && q.GetMessage().Contains("#ser#")))
                    ?? BarcodeObjs.FirstOrDefault(q => q.GetMessage().Contains("#ser#"));
                return barcodeObj;
            }
        }
        public Paper Paper
        {
            get { return _paper; }
            set
            {
                _paper = value;
                DesignerEvent.OnNewPaper(_paper);
            }
        }
        public DesignObjManager()
        {
        }
        public bool Equals(DesignObjManager other)
        {
            try
            {
                string origin = AllObjs.SerializeObjectToJson<List<DrawingObjBase>>();
                string dest = other.AllObjs.SerializeObjectToJson<List<DrawingObjBase>>();
                bool ret = string.Equals(origin, dest);
                return ret;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public DesignObjManager(DesignObjManager obj)
        {

        }
        #region Objs
        public List<MainObj> MainObj
        {
            get
            {
                return TableObjs.Cast<MainObj>().Union(TextBoxObjs.Cast<MainObj>()).Union(BarcodeObjs.Cast<MainObj>()).Union(Figures.Cast<MainObj>()).Union(ImageObjs.Cast<MainObj>()).Union(LineObjs.Cast<MainObj>()).OrderByDescending(q => q.ZPos).ToList();
            }
        }
        [JsonIgnore]
        public List<DrawingObjBase> AllObjs
        {
            get
            {
                List<DrawingObjBase> objs = new List<DrawingObjBase>();
                MainObj.ForEach(q =>
                {
                    if (q is TableObj)
                    {
                        var obj = q as TableObj;
                        objs.AddRange(obj.AdjustHandlers);
                        objs.AddRange(obj.MoveHandlers);
                        objs.AddRange(obj.AllHandlers);
                        objs.AddRange(obj.CellObjs);
                        objs.Add(obj);
                    }
                    else
                    {
                        objs.AddRange(q.AdjustHandlers);
                        objs.AddRange(q.MoveHandlers);
                        objs.Add(q);
                    }
                });
                return objs;
            }
        }
        [JsonIgnore]
        public List<DrawingObjBase> AllHandlers
        {
            get
            {
                List<DrawingObjBase> objs = AllObjs.FindAll(q => q is DrawingHandler).Cast<DrawingObjBase>().ToList();
                return objs;
            }
        }

        [JsonIgnore]
        public List<MainObj> SelectedMainObj
        {
            get
            {
                return MainObj?.FindAll(q => q.IsSelected);
            }
        }
        [JsonIgnore]
        public List<DrawingObjBase> SelectedObj
        {
            get
            {
                return AllObjs?.FindAll(q => q.IsSelected);
            }
        }
        [JsonIgnore]
        public List<MainObj> Figures
        {
            get
            {
                return CircleObjs.Cast<MainObj>().Union(RectObjs.Cast<MainObj>()).ToList();
            }
        }
        [JsonIgnore]
        public List<IWritable> TextObjs
        {
            get
            {
                List<IWritable> objs = MainObj.FindAll(q => q is TextBoxObj).Cast<IWritable>().ToList();
                List<IWritable> CellText = new List<IWritable>();
                TableObjs.ForEach(q =>
                {
                    CellText.AddRange(q.CellObjs);
                });

                return objs.Union(CellText).ToList();
            }
        }
        [JsonIgnore]
        public List<IVariableText> VariableText
        {
            get
            {
                List<IVariableText> objs = MainObj.FindAll(q => q is IVariableText).Cast<IVariableText>().ToList();
                return objs;
            }
        }
        #endregion
        public void InitializeSelection()
        {
            MainObj.ForEach(obj =>
            {
                obj.Select(false);
                if (obj is TableObj)
                {
                    (obj as TableObj).CellObjs.ForEach(q => q.Select(false));
                }
            });
        }
        public void DeleteObj(MainObj obj)
        {
            if (obj is TableObj && TableObjs.Contains(obj as TableObj))
            {
                TableObjs.Remove(obj as TableObj);
            }
            else if (obj is TextBoxObj && TextBoxObjs.Contains(obj as TextBoxObj))
            {
                TextBoxObjs.Remove(obj as TextBoxObj);
            }
            else if (obj is BarcodeObj && BarcodeObjs.Contains(obj as BarcodeObj))
            {
                BarcodeObjs.Remove(obj as BarcodeObj);
            }
            else if (obj is CircleObj && CircleObjs.Contains(obj as CircleObj))
            {
                CircleObjs.Remove(obj as CircleObj);
            }
            else if (obj is ImageObj && ImageObjs.Contains(obj as ImageObj))
            {
                ImageObjs.Remove(obj as ImageObj);
            }
            else if (obj is RectObj && RectObjs.Contains(obj as RectObj))
            {
                RectObjs.Remove(obj as RectObj);
            }
            else if (obj is LineObj && LineObjs.Contains(obj as LineObj))
            {
                LineObjs.Remove(obj as LineObj);
            }

        }
        public void DeleteObj()
        {
            SelectedMainObj.ForEach(q =>
            {
                if (TableObjs.Contains(q))
                {
                    TableObjs.Remove(q as TableObj);
                }
                else if (TextBoxObjs.Contains(q))
                {
                    TextBoxObjs.Remove(q as TextBoxObj);
                }
                else if (BarcodeObjs.Contains(q))
                {
                    BarcodeObjs.Remove(q as BarcodeObj);
                }
                else if (CircleObjs.Contains(q as CircleObj))
                {
                    CircleObjs.Remove(q as CircleObj);
                }
                else if (ImageObjs.Contains(q as ImageObj))
                {
                    ImageObjs.Remove(q as ImageObj);
                }
                else if (RectObjs.Contains(q as RectObj))
                {
                    RectObjs.Remove(q as RectObj);
                }
                else if (LineObjs.Contains(q as LineObj))
                {
                    LineObjs.Remove(q as LineObj);
                }
            });
        }
		public DrawingObjBase GetClickedMainObjs(PointF pt)
		{
            if (MainObj.Count(q => q.IsSelected) == 1)
            {
                var selObj = SelectedMainObj.First();
                var handler = selObj.AllHandlers.FirstOrDefault(q => q.Rect.Contains(pt));
                if (handler != null)
                    return (DrawingObjBase)handler;
            }
			DrawingObjBase obj = MainObj.OrderByDescending(q => q.ZPos).FirstOrDefault(q => q.Rect.Contains(pt));
            if (obj is MainObj mainObj)
            {
                var main = mainObj?.Components.FirstOrDefault(q => q.Rect.Contains(pt));
                return main ?? obj;
            }
            else
            {
                var hd = AllHandlers.FirstOrDefault(q => q.Rect.Contains(pt));
                return hd ?? obj;
            }
		}
		public float GetClosestSideOfRect(RectangleF rect, PointF pt)
        {
            return (new List<float> { pt.X - rect.Left, rect.Right - pt.X, pt.Y - rect.Top, rect.Bottom - pt.Y }).Min();
        }
        public void ClearList()
        {
            TableObjs.Clear();
            TextBoxObjs.Clear();
            BarcodeObjs.Clear();
            CircleObjs.Clear();
            ImageObjs.Clear();
            RectObjs.Clear();
            LineObjs.Clear();
        }
        public MainObj GetNextMainObj(MainObj obj)
        {
            int index = MainObj.FindIndex(q => q.Equals(obj));
            if (MainObj.Count > 0)
            {
                if (MainObj.Count > index + 1)
                    return MainObj[index + 1];
                else
                    return MainObj[0];
            }
            return null;
        }
        public MainObj GetPreviousMainObj(MainObj obj)
        {
            int index = MainObj.FindIndex(q => q.Equals(obj));
            if (MainObj.Count > 0)
            {
                if (index > 0)
                    return MainObj[index - 1];
                else
                    return MainObj[MainObj.Count -1];
            }
            return null;
        }
        public DesignObjManager Clone()
        {
            DesignObjManager manager = new DesignObjManager();
            manager.Tittle = this.Tittle;
            manager.Path = this.Path;
            manager.Paper = this.Paper.Clone();
			NormalizeZPos(MainObj);
			MainObj.ForEach(q =>
                {
                    if (q is TableObj table)
                    {
                        manager.TableObjs.Add(table.Clone() as TableObj);
                    }
                    else if (q is TextBoxObj textObj)
                    {
                        manager.TextBoxObjs.Add(textObj.Clone() as TextBoxObj);
                    }
                    else if (q is BarcodeObj barcodObj)
                    {
                        manager.BarcodeObjs.Add(barcodObj.Clone() as BarcodeObj);
                    }
                    else if (q is CircleObj circleObj)
                    {
                        manager.CircleObjs.Add(circleObj.Clone() as CircleObj);
                    }
                    else if (q is ImageObj imageObj)
                    {
                        manager.ImageObjs.Add(imageObj.Clone() as ImageObj);
                    }
                    else if (q is RectObj rectObj)
                    {
                        manager.RectObjs.Add(rectObj.Clone() as RectObj);
                    }
                    else if (q is LineObj lineObj)
                    {
                        manager.LineObjs.Add(lineObj.Clone() as LineObj);
                    }
                });
            return manager;
        }
		public void NormalizeZPos(List<MainObj> mainObj)
		{
			var sortedList = mainObj.OrderBy(o => o.ZPos).ToList();
			for (int i = 0; i < sortedList.Count; i++)
			{
				sortedList[i].ZPos = i;
			}
		}
        public void AddTable(TableObj mainObj)
        {
            mainObj.ZPos = MainObj.Count;
            TableObjs.Add(mainObj);
        }
        public void AddBarcode(BarcodeObj mainObj)
        {
            mainObj.ZPos = MainObj.Count;
            BarcodeObjs.Add(mainObj);
        }
        public void AddImage(ImageObj mainObj)
        {
            mainObj.ZPos = MainObj.Count;
            ImageObjs.Add(mainObj);
        }           
        public void AddTextBox(TextBoxObj mainObj)
        {
            mainObj.ZPos = MainObj.Count;
            TextBoxObjs.Add(mainObj);
        }            
        public void AddCircle(CircleObj mainObj)
        {
            mainObj.ZPos = MainObj.Count;
            CircleObjs.Add(mainObj);
        }
        public void AddRect(RectObj mainObj)
        {
            mainObj.ZPos = MainObj.Count;
            RectObjs.Add(mainObj);
        }
        public void AddLine(LineObj mainObj)
        {
            mainObj.ZPos = MainObj.Count;
            LineObjs.Add(mainObj);
        }
    }

}
