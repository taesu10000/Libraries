using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using log4net;

namespace DesignObject
{
	public static class JsonSerializerExtension
	{
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		public static string SerializeObjectToJson<T>(this T _obj, bool isIndented = true)
		{
			try
			{
				if (isIndented)
				{
					//return JsonConvert.SerializeObject(_obj, Formatting.Indented);
					return JsonConvert.SerializeObject(_obj, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
				}
				else
					return JsonConvert.SerializeObject(_obj, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, DateFormatString = "yyyy-MM-dd HH:mm:ss" });
			}
			catch (Exception ex)
			{
				log.ErrorFormat("JsonSerializerExtension SerializeObjectToJson Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
				return string.Empty;
			}
		}

		public static T DeserializeJsonObject<T>(this string _contents)
		{
			try
			{
				JsonSerializerSettings jss = new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Ignore };

				if (_contents is null)
					return default;

				_contents = _contents.Replace(@"\u001d", ((char)29).ToString());
				return JsonConvert.DeserializeObject<T>(_contents, jss);

			}
			catch (Exception ex)
			{
				log.ErrorFormat("JsonSerializerExtension DeserializeJsonObject Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
				return default(T);
			}
		}
	}
}
