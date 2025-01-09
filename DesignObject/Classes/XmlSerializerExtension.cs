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
	public static class XmlSerializerExtension
	{
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		public static bool SerializeObjectToXMLFile2<T>(object toSerialize, string _fileName) where T : class, new()
		{
			return SerializeObjectToXMLFile<T>(toSerialize as T, _fileName);
		}
		public static bool SerializeObjectToXMLFile<T>(this T toSerialize, string _fileName) where T : class, new()
		{
			try
			{
				string dir = System.IO.Path.GetDirectoryName(_fileName);
				try { Directory.CreateDirectory(dir); }
				catch (Exception ex)
				{
					log.DebugFormat("{0} : {1}", "Failed Create Directory", ex);
					return false;
				}
				string backupFile = Path.ChangeExtension(_fileName, "bak");
				if (File.Exists(backupFile))
					File.Delete(backupFile);
				try { File.WriteAllText(backupFile, toSerialize.SerializeObjectToXML<T>()); }
				catch (Exception ex)
				{
					log.DebugFormat("{0} : {1}", "Failed Serialize Object To xml", ex);
					return false;
				}
				FileInfo info = new FileInfo(backupFile);
				if (info.Length == 0)
				{
					log.DebugFormat("{0}", "Failed Serialize Object To xml File");
					return false;
				}
				if (File.Exists(_fileName))
					File.Delete(_fileName);
				File.Move(backupFile, _fileName);
				return true;
			}
			catch (Exception ex)
			{
				log.DebugFormat("{0} : {1}", "Failed Serialize Object To xml", ex);
				return false;
			}
		}
		public static string SerializeObjectToXML<T>(this T toSerialize)
		{
			try
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
				XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
				using (var ms = new MemoryStream())
				{
					using (var xw = XmlWriter.Create(ms, new XmlWriterSettings()
					{
						Encoding = new UTF8Encoding(false),
						Indent = true,
					}))
					{
						xmlSerializer.Serialize(xw, toSerialize, ns);
						return Encoding.UTF8.GetString(ms.ToArray());
					}
				}
			}
			catch (Exception ex)
			{
				log.DebugFormat("{0} : {1}", "Failed Serialize Object To xml", ex);
				return string.Empty;
			}
		}
		public static object DeserializeXMLToObject<T>(this string toDeserialize) where T : class, new()
		{
			try
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
				using (StringReader txtReader = new StringReader(toDeserialize))
				{
					return xmlSerializer.Deserialize(txtReader);
				}
			}
			catch (Exception ex)
			{
				log.DebugFormat("{0} : {1}", "Failed Deserialize xml To Object", ex);
				return default(T);
			}
		}
		public static T DeserializeXMLFileToObject<T>(string _fileName) where T : class, new()
		{
			try
			{
				string xml = File.ReadAllText(_fileName);
				return xml.DeserializeXMLToObject<T>() as T;
			}
			catch (Exception ex)
			{
				log.DebugFormat("{0} : {1}", "Failed Deserialize xml File To Object", ex);
				return default(T);
			}
		}

	}
}
