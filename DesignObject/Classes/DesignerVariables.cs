using DesignObject.Resources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignObject
{
    public class DesignerVariables
    {
        static DesignerVariables instance;
        public DesignObjManager Previous { get; set; }
        public DesignObjManager DesignObjManager { get; set; }
        public float ZoomRatio { get { return Paper?.ZoomRatio ?? 1f; } set { Paper.ZoomRatio = value; } }
        public bool RequireSave
        {
            get
            {
                if (Previous == null)
                    return true;
                return !DesignObjManager.Equals(Previous);
            }
        }
        public Paper Paper
        {
            get { return DesignObjManager?.Paper; }
            set
            {
                DesignObjManager.Paper = value;
            }
        }
        public string LabelTittle
        {
            get
            {
				if (string.IsNullOrEmpty(DesignObjManager.Tittle))
                    DesignObjManager.Tittle = UserInterfaces.NoTittle;
                return DesignObjManager.Tittle;
            }
            set
            {
                DesignObjManager.Tittle = value;
            }
        }

        public static DesignerVariables Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DesignerVariables();
                }
                return instance;
            }
            set { instance = value; }
        }
        public void SaveDocument(string fileName)
        {
            DesignObjManager.Tittle = System.IO.Path.GetFileNameWithoutExtension(fileName);
            string json = DesignObjManager.SerializeObjectToJson();
            System.IO.File.WriteAllText(fileName, json);
        }
        public void LoadDocument(string fileName)
        {
            if (System.IO.File.Exists(fileName))
            {
                if (System.IO.Path.GetExtension(fileName).ToLower() == ".ddo")
                {
                    DesignObjManager?.ClearList();
                    string file = System.IO.File.ReadAllText(fileName);
                    file = Replace(file);

					var manager = DeserializeJson(file);
                    if (manager != null)
                    {
                        DesignObjManager = LoadDesign(manager);
                        DesignObjManager.Path = System.IO.Path.GetDirectoryName(fileName);
                    }
                }
                if (DesignObjManager != null)
                    Previous = DesignObjManager.Clone();
            }
        }
        public string Replace(string file)
        {
            return file.Replace("\"GetTextObj\"", "\"Text\"");
        }
        public DesignObjManager LoadManager(string path)
        {
            if (File.Exists(path))
            {
                string file = System.IO.File.ReadAllText(path);
                var manager = DeserializeJson(file);
                return LoadDesign(manager);
            }
            return null;
        }
        public DesignObjManager DeserializeJson(string file)
        {
            DesignObjManager documentObj = file.DeserializeJsonObject<DesignObjManager>();
            return documentObj;
        }
        public DesignObjManager LoadDesign(DesignObjManager documentObj)
        {
            var manager = new DesignObjManager();
            if (documentObj == null)
                return null;
            manager.Tittle = documentObj.Tittle;
            manager.Paper = documentObj.Paper;
            manager.Path = documentObj.Path;
            for (int i = 0; i < documentObj.MainObj.Count; i++)
            {
                if (documentObj.MainObj[i] is TableObj tableObj)
                {
                    manager.TableObjs.Add(new TableObj(tableObj));
                }
                else if (documentObj.MainObj[i] is BarcodeObj barcodeObj)
                {
                    manager.BarcodeObjs.Add(new BarcodeObj(barcodeObj));
                }
                else if (documentObj.MainObj[i] is TextBoxObj txtObj)
                {
                    manager.TextBoxObjs.Add(new TextBoxObj(txtObj));
                }
                else if (documentObj.MainObj[i] is CircleObj circleObj)
                {
                    manager.CircleObjs.Add(new CircleObj(circleObj));
                }
                else if (documentObj.MainObj[i] is RectObj rectObj)
                {
                    manager.RectObjs.Add(new RectObj(rectObj));
                }
                else if (documentObj.MainObj[i] is LineObj lineObj)
                {
                    manager.LineObjs.Add(new LineObj(lineObj));
                }
                else if (documentObj.MainObj[i] is ImageObj imageObj)
                {
                    manager.ImageObjs.Add(new ImageObj(imageObj));
                }
            }
            return manager;
        }
        public void NewDocument(NewDocumentArgs e)
        {
            DesignObjManager = new DesignObjManager();
            DesignObjManager.Paper = e.Paper;
            DesignerEvent.OnInvalidate();
        }
    }
}
