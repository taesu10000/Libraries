using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;


namespace DominoFunctions.ExtensionMethod
{
    public static class ClassExtenstion
    {
        public static void UnionClass(this object obj, object target)
        {
            if (obj != null && target != null)
            {
                PropertyInfo[] toProperties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                PropertyInfo[] fromProperties = target.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                for (int i = 0; i < toProperties.Count(); i++)
                {
                    if (fromProperties.Any(x => x.Name.Equals(toProperties[i].Name)))
                    {
                        toProperties[i].SetValue(obj, fromProperties[fromProperties.ToList().FindIndex(x => x.Name.Equals(toProperties[i].Name))].GetValue(target, null));
                    }
                }
            }
        }
        public static void WriteExistings(this object obj, object target)
        {
            PropertyInfo[] toProperties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo[] fromProperties = target.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            for (int i = 0; i < toProperties.Count(); i++)
            {
                if (fromProperties.Any(x => x.Name.Equals(toProperties[i].Name)))
                {
                    if (fromProperties[fromProperties.ToList().FindIndex(x => x.Name.Equals(toProperties[i].Name))]?.GetValue(target, null) != null)
                        toProperties[i].SetValue(obj, fromProperties[fromProperties.ToList().FindIndex(x => x.Name.Equals(toProperties[i].Name))].GetValue(target, null));
                }
            }
        }
        public static T WriteExistings<T>(this object obj, object target)
        {
            PropertyInfo[] toProperties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo[] fromProperties = target.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            for (int i = 0; i < toProperties.Count(); i++)
            {
                if (fromProperties.Any(x => x.Name.Equals(toProperties[i].Name)))
                {
                    if (fromProperties[fromProperties.ToList().FindIndex(x => x.Name.Equals(toProperties[i].Name))].GetValue(target, null) != null)
                        toProperties[i].SetValue(obj, fromProperties[fromProperties.ToList().FindIndex(x => x.Name.Equals(toProperties[i].Name))].GetValue(target, null));
                }
            }
            return (T)obj;
        }
        public static T DeepCopy<T>(this T obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }
        public static List<string> GetProperties(this object obj)
        {
            PropertyInfo[] Properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo pro in Properties)
            {
                if (pro is IEnumerable)
                {

                }
            }

            return Properties.Where(q => !q.PropertyType.Name.Contains("Collection")).Select(q => q.Name).ToList();
        }
        public class KeyComparer<T> : IEqualityComparer<T>
        {
            private readonly Func<T, object> _keySelector;

            public KeyComparer(Func<T, object> keySelector)
            {
                _keySelector = keySelector;
            }

            public bool Equals(T x, T y)
            {
                return _keySelector(x).Equals(_keySelector(y));
            }

            public int GetHashCode(T obj)
            {
                return _keySelector(obj).GetHashCode();
            }
        }
    }
}