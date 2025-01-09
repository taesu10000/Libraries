using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using log4net;

namespace DesignObject
{
	public class SingletonObject<T> where T : class, new()
	{
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		[XmlIgnore]
		protected static object _Lock = new object();
		[XmlIgnore]
		protected static T _PrevInstance = null;
		[XmlIgnore]
		public static T PrevInstance { get { return _PrevInstance; } }
		[XmlIgnore]
		protected static T _Instance = null;
		[XmlIgnore]
		public static T Instance
		{
			get
			{
				if (_Instance == null)
					_Instance = new T();
				return _Instance;
			}
		}
		public void Capture()
		{
			try
			{
				var seriaized = JsonConvert.SerializeObject(_Instance, Newtonsoft.Json.Formatting.Indented);
				JsonSerializerSettings jss = new JsonSerializerSettings();
				jss.MissingMemberHandling = MissingMemberHandling.Ignore;
			}
			catch (Exception ex)
			{
				log.DebugFormat("{0} : {1}", "Failed Capture Json", ex);
			}

		}
		public bool ComparePervInstance()
		{
			return string.Compare(_Instance.SerializeObjectToJson<T>(), _PrevInstance.SerializeObjectToJson<T>()) == 0;
		}
		public virtual bool SaveXML(string fileName)
		{
			lock (_Lock)
			{
				return XmlSerializerExtension.SerializeObjectToXMLFile(_Instance, fileName);
			}
		}
		public virtual void LoadXML(string fileName)
		{
			if (string.IsNullOrEmpty(fileName))
				return;
			_Instance = XmlSerializerExtension.DeserializeXMLFileToObject<T>(fileName);
			if (_Instance == null)
			{
				_Instance = Activator.CreateInstance(typeof(T)) as T;
				lock (_Lock)
				{
					XmlSerializerExtension.SerializeObjectToXMLFile<T>(_Instance, fileName);
				}
			}
			Capture();
		}
	}


}
