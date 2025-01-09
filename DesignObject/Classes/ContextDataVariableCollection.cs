using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace DesignObject
{
    public class ContextValue
    {
        public object Value { get; set; }
        public string Name { get; set; }
        public string Parent { get; set; } = null;
        public bool IsList { get; set; } = false;
        public bool IsDateTime = false;
        protected ContextValue(PropertyInfo prop, object obj)
        {
            if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                IsDateTime = true;

            Value = prop.GetValue(obj);
            if (Value != null && IsDateTime)
            {
                DateTimeFormatter dtFormatter = new DateTimeFormatter();
                Value = dtFormatter.GetValue((DateTime)Value);
            }
        }
        public ContextValue(string name, PropertyInfo prop, object obj) : this(prop, obj)
        {
            Name = name;
        }
        public ContextValue(string name, string parent, PropertyInfo prop, object obj) : this(prop, obj)
        {
            Name = name;
            Parent = parent;
            IsList = true;
        }
        public ContextValue(string name, string parent, object value, bool isList = false)
        {
            Name = name;
            Parent = parent;
            Value = value;
            IsList = isList;
        }
    }
    public class ContextMap : IEnumerable<ContextValue>
    {
        const string Default = "Default";
        Dictionary<string, ContextValue> _items;
        public Dictionary<string, ContextValue> Items { get { return _items; } }
        public Dictionary<string, ContextValue> DetailItems { get { return _items.Where(q => q.Value.IsList).ToDictionary(q => q.Key, q => q.Value); } }
        public Dictionary<string, ContextValue> SignleItems { get { return _items.Where(q => !q.Value.IsList).ToDictionary(q => q.Key, q => q.Value); } }
        public ContextValue this[string key]
        {
            get
            {
                return _items[key];
            }
            set
            {
                _items[key] = value;
            }
        }
        public ContextMap(object obj)
        {
            CreateMap(obj);
        }
        public void CreateMap(object obj)
        {
            _items = new Dictionary<string, ContextValue>();
            if (obj == null)
                return;

            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var val = prop.GetValue(obj);
                if (IsEnumable(val, out IEnumerable<object> enumable))
                {
                    PropertyInfo[] subProps = enumable.First().GetType().GetProperties();
                    foreach (PropertyInfo subProp in subProps)
                    {
                        for (int i = 0; i < enumable.Count(); i++)
                        {
                            Add(subProp, i, val.GetType().Name, enumable.ElementAt(i));
                        }
                    }
                    AddAdditinal(enumable.Count(), val.GetType().Name);
                }
                else
                {
                    Add(prop, obj);
                }
            }
        }
        public void AddAdditinal(int count, string parentName)
        {
            for (int i = 0; i < count; i++)
            {
                _items.Add(CreateKey("No", i), new ContextValue("No", parentName, i + 1, true));
            }
        }
        public bool IsEnumable(object obj, out IEnumerable<object> result)
        {
            result = null;
            if (obj is IEnumerable<object>)
            {
                result = obj as IEnumerable<object>;
                return true;
            }
            return false;
        }
        public bool ContainsKey(string key)
        {
            if (string.IsNullOrEmpty(key))
                return false;
            return _items.ContainsKey(key);
        }
        public bool ContainsName(string key)
        {
            if (string.IsNullOrEmpty(key))
                return false;
            return _items.Any(q => q.Value.Name.Equals(key));
        }
        public bool IsDetail(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;
            return DetailItems.Any(q => q.Value.Name.Equals(name));
        }
        public ContextValue GetItemByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;
            return _items.FirstOrDefault(q => q.Value.Name.Equals(name)).Value;
        }
        public void Add(PropertyInfo propInfo, object obj)
        {
            if (!_items.ContainsKey(propInfo.Name))
                _items.Add(propInfo.Name, new ContextValue(propInfo.Name, propInfo, obj));
        }
        public void Add(PropertyInfo propInfo, int idx, string parent, object obj)
        {
            string key = CreateKey(propInfo.Name, idx);
            if (!_items.ContainsKey(key))
                _items.Add(key, new ContextValue(propInfo.Name, parent, propInfo, obj));
        }
        public string CreateKey(string name, int idx)
        {
            return name + idx.ToString("00");
        }
        public IEnumerator<ContextValue> GetEnumerator()
        {
            return _items.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
    public class DateTimeFormatter
    {
        public string GetValue(DateTime dateTime,
        EnDateFormat format = EnDateFormat.Year | EnDateFormat.Month | EnDateFormat.Day,
        EnDateOrder dateOrder = EnDateOrder.yyyyMMdd,
        EnDateTimeSeparator dateSeparator = EnDateTimeSeparator.Dash,
        EnDateTimeSeparator timeSeparator = EnDateTimeSeparator.Colon,
        EnDateYearMonthFormat dateYearMonthFomat = EnDateYearMonthFormat.yyyyMM)
        {
            string year = string.Empty; string month = string.Empty; string day = string.Empty;
            string hour = string.Empty; string minute = string.Empty; string second = string.Empty;
            string period = string.Empty;
            if (Convert.ToBoolean(format & EnDateFormat.Year))
            {
                switch (dateOrder)
                {
                    case EnDateOrder.MMyyyydd:
                        if (dateYearMonthFomat == EnDateYearMonthFormat.yyMM || dateYearMonthFomat == EnDateYearMonthFormat.yyMMM)
                            year = Convert.ToBoolean(format & EnDateFormat.Month) ? GetSeparator(dateSeparator) + "yy" : "yy";
                        else
                            year = Convert.ToBoolean(format & EnDateFormat.Month) ? GetSeparator(dateSeparator) + "yyyy" : "yyyy";
                        break;
                    case EnDateOrder.ddyyyyMM:
                        if (dateYearMonthFomat == EnDateYearMonthFormat.yyMM || dateYearMonthFomat == EnDateYearMonthFormat.yyMMM)
                            year = Convert.ToBoolean(format & EnDateFormat.Day) ? GetSeparator(dateSeparator) + "yy" : "yy";
                        else
                            year = Convert.ToBoolean(format & EnDateFormat.Day) ? GetSeparator(dateSeparator) + "yyyy" : "yyyy";
                        break;
                    case EnDateOrder.MMddyyyy:
                    case EnDateOrder.ddMMyyyy:
                        if (dateYearMonthFomat == EnDateYearMonthFormat.yyMM || dateYearMonthFomat == EnDateYearMonthFormat.yyMMM)
                            year = Convert.ToBoolean(format & EnDateFormat.Month) || Convert.ToBoolean(format & EnDateFormat.Day) ? GetSeparator(dateSeparator) + "yy" : "yy";
                        else
                            year = Convert.ToBoolean(format & EnDateFormat.Month) || Convert.ToBoolean(format & EnDateFormat.Day) ? GetSeparator(dateSeparator) + "yyyy" : "yyyy";
                        break;
                    case EnDateOrder.yyyyddMM:
                    case EnDateOrder.yyyyMMdd:
                        if (dateYearMonthFomat == EnDateYearMonthFormat.yyMM || dateYearMonthFomat == EnDateYearMonthFormat.yyMMM)
                            year = "yy";
                        else
                            year = "yyyy";
                        break;
                }

            }
            if (Convert.ToBoolean(format & EnDateFormat.Month))
            {
                switch (dateOrder)
                {
                    case EnDateOrder.ddMMyyyy:
                        if (dateYearMonthFomat == EnDateYearMonthFormat.yyMM || dateYearMonthFomat == EnDateYearMonthFormat.yyyyMM)
                            month = Convert.ToBoolean(format & EnDateFormat.Day) ? GetSeparator(dateSeparator) + "MM" : "MM";
                        else
                            month = Convert.ToBoolean(format & EnDateFormat.Day) ? GetSeparator(dateSeparator) + "MMM" : "MMM";
                        break;
                    case EnDateOrder.yyyyMMdd:
                        if (dateYearMonthFomat == EnDateYearMonthFormat.yyMM || dateYearMonthFomat == EnDateYearMonthFormat.yyyyMM)
                            month = Convert.ToBoolean(format & EnDateFormat.Year) ? GetSeparator(dateSeparator) + "MM" : "MM";
                        else
                            month = Convert.ToBoolean(format & EnDateFormat.Year) ? GetSeparator(dateSeparator) + "MMM" : "MMM";
                        break;
                    case EnDateOrder.ddyyyyMM:
                    case EnDateOrder.yyyyddMM:
                        if (dateYearMonthFomat == EnDateYearMonthFormat.yyMM || dateYearMonthFomat == EnDateYearMonthFormat.yyyyMM)
                            month = Convert.ToBoolean(format & EnDateFormat.Year) || Convert.ToBoolean(format & EnDateFormat.Day) ? GetSeparator(dateSeparator) + "MM" : "MM";
                        else
                            month = Convert.ToBoolean(format & EnDateFormat.Year) || Convert.ToBoolean(format & EnDateFormat.Day) ? GetSeparator(dateSeparator) + "MMM" : "MMM";
                        break;
                    case EnDateOrder.MMddyyyy:
                    case EnDateOrder.MMyyyydd:
                        if (dateYearMonthFomat == EnDateYearMonthFormat.yyMM || dateYearMonthFomat == EnDateYearMonthFormat.yyyyMM)
                            month = "MM";
                        else
                            month = "MMM";
                        break;
                }
            }
            if (Convert.ToBoolean(format & EnDateFormat.Day))
            {
                switch (dateOrder)
                {
                    case EnDateOrder.MMddyyyy:
                        day = Convert.ToBoolean(format & EnDateFormat.Month) ? GetSeparator(dateSeparator) + "dd" : "dd";
                        break;
                    case EnDateOrder.yyyyddMM:
                        day = Convert.ToBoolean(format & EnDateFormat.Year) ? GetSeparator(dateSeparator) + "dd" : "dd";
                        break;
                    case EnDateOrder.MMyyyydd:
                    case EnDateOrder.yyyyMMdd:
                        day = Convert.ToBoolean(format & EnDateFormat.Year) || Convert.ToBoolean(format & EnDateFormat.Month) ? GetSeparator(dateSeparator) + "dd" : "dd";
                        break;
                    case EnDateOrder.ddMMyyyy:
                    case EnDateOrder.ddyyyyMM:
                        day = "dd";
                        break;
                }
            }
            if (Convert.ToBoolean(format & EnDateFormat.AMPM))
            {
                period = " tt";
            }
            if (Convert.ToBoolean(format & EnDateFormat.Hour))
            {
                if (Convert.ToBoolean(format & EnDateFormat.AMPM))
                    hour = Convert.ToBoolean(format & EnDateFormat.TwentyfourHourSystem) ? " HH" : " hh";
                else
                    hour = Convert.ToBoolean(format & EnDateFormat.TwentyfourHourSystem) ? "HH" : "hh";
            }
            if (Convert.ToBoolean(format & EnDateFormat.Minute))
            {
                minute = Convert.ToBoolean(format & EnDateFormat.Hour) ? GetSeparator(timeSeparator) + "mm" : " mm";
            }
            if (Convert.ToBoolean(format & EnDateFormat.Second))
            {
                second = Convert.ToBoolean(format & EnDateFormat.Hour) || Convert.ToBoolean(format & EnDateFormat.Minute) ? GetSeparator(timeSeparator) + "ss" : " ss";
            }
            switch (dateOrder)
            {
                case EnDateOrder.yyyyMMdd:
                    return dateTime.ToString(year + month + day + period + hour + minute + second, System.Globalization.CultureInfo.InvariantCulture);
                case EnDateOrder.yyyyddMM:
                    return dateTime.ToString(year + day + month + period + hour + minute + second, System.Globalization.CultureInfo.InvariantCulture);
                case EnDateOrder.MMddyyyy:
                    return dateTime.ToString(month + day + year + period + hour + minute + second, System.Globalization.CultureInfo.InvariantCulture);
                case EnDateOrder.ddMMyyyy:
                    return dateTime.ToString(day + month + year + period + hour + minute + second, System.Globalization.CultureInfo.InvariantCulture);
                case EnDateOrder.ddyyyyMM:
                    return dateTime.ToString(day + year + month + period + hour + minute + second, System.Globalization.CultureInfo.InvariantCulture);
                case EnDateOrder.MMyyyydd:
                    return dateTime.ToString(month + year + day + period + hour + minute + second, System.Globalization.CultureInfo.InvariantCulture);
            }
            return string.Empty;
        }

        private string GetSeparator(EnDateTimeSeparator sep)
        {
            switch (sep)
            {
                case EnDateTimeSeparator.Colon:
                    return ":";
                case EnDateTimeSeparator.Dash:
                    return "-";
                case EnDateTimeSeparator.Dot:
                    return ".";
                case EnDateTimeSeparator.Slash:
                    return "/";
                case EnDateTimeSeparator.Space:
                    return " ";
            }
            return string.Empty;
        }
    }
}
