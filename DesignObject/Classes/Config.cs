using DesignObject.Resources;
using Newtonsoft.Json;
using System;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Xml.Serialization;

namespace DesignObject
{
    [Serializable]
    public class Config<T> : SingletonObject<T> where T : class, new()
    {
        protected string _lastPath;
        public string _configFileName;
        public string LastPrinterName { get; set; }
        [JsonIgnore]
        [XmlIgnore]
        public EnLanguage Language { get; set; }
		public string LastPath
        {
            get
            {
                return _lastPath;
            }
            set
            {
                _lastPath = value;
            }
        }
        public Config()
        {
            if (System.IO.Directory.Exists("D:\\"))
                _configFileName = "D:\\XML\\DesignerConfig.xml";
            else
                _configFileName = "C:\\XML\\DesignerConfig.xml";

            LastPrinterName = "";
            _lastPath = @"C:\";
        }
        public void LoadXML()
        {
            base.LoadXML(_configFileName);
            UserInterfaces.Culture = CultureInfo.InvariantCulture;
        }
        public bool SaveXML()
        {
            return base.SaveXML(_configFileName);
        }
    }
}
