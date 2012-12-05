using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Hypertek.IOffice.Common.Helpers;
using System.Collections;
using Microsoft.SharePoint.Utilities;

namespace Hypertek.IOffice.Common.Extensions
{
    public static class HashtableExtensions
    {
        public static void SyncProperties(this object destination, object source)
        {
            var type = destination.GetType();
            var properties = type.GetProperties(System.Reflection.BindingFlags.SetProperty);

            foreach (var item in properties)
            {
                var data = source.GetPropertyValue(item.Name);
                destination.SetProperty(item.Name, data);
            }
        }

        public static bool IsCollection(object o)
        {
            return typeof(ICollection).IsAssignableFrom(o.GetType())
                || typeof(ICollection<>).IsAssignableFrom(o.GetType());
        }

        public static void PopulaResources(this object obj)
        {
            if (IsCollection(obj))
            {
                if (obj is IEnumerable)
                {
                    var ctype = obj.GetType();
                    if (ctype == typeof(List<string>))
                    {
                        //TODO - How ugly this code (.)(.)
                        List<string> arr = obj as List<string>;

                        for (int i = 0; i < arr.Count; i++)
                        {
                            string resource = arr[i];
                            resource = resource.TrimStart("\r\n ".ToCharArray()).TrimEnd("\r\n ".ToCharArray());
                            if (resource.StartsWith("$Resources:"))
                            {
                                arr[i] = SPUtility.GetLocalizedString(resource, string.Empty, 1033);
                            }

                        }
                        
                    }
                    else
                    foreach (object o in (obj as IEnumerable))
                    {
                        o.PopulaResources();
                    }
                }
                else
                {
                    // reflect over item
                }
            }
            var type = obj.GetType();
            var properties = type.GetProperties();

            if (obj != null && obj is string)
            {
                string resource = obj.ToString();
                resource = resource.TrimStart("\r\n ".ToCharArray()).TrimEnd("\r\n ".ToCharArray());
                if (resource.StartsWith("$Resources:"))
                {
                    obj = SPUtility.GetLocalizedString(resource, string.Empty, 1033);
                }
            }
            else
            {
                foreach (var item in properties)
                {
                    try
                    {

                        var data = obj.GetPropertyValue(item.Name);
                        if (IsCollection(data))
                        {
                            data.PopulaResources();
                        }
                        if (data != null && data is string)
                        {
                            string resource = data.ToString();
                            resource = resource.TrimStart("\r\n ".ToCharArray()).TrimEnd("\r\n ".ToCharArray());
                            if (resource.StartsWith("$Resources:"))
                            {
                                obj.SetProperty(item.Name, SPUtility.GetLocalizedString(resource, string.Empty, 1033));
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }


            
        }

        public static object GetPropertyValue(this object destination, string name)
        {
            var type = destination.GetType();
            var pi = type.GetProperty(name);
            object result = null;
            if (pi != null)
            {
                result = pi.GetValue(destination, null);
            }
            return result;
        }

        public static void UpdateWith(this Hashtable first, Hashtable second)
        {
            foreach (DictionaryEntry item in second)
            {
                first[item.Key] = item.Value;
            }
        }

        

         public static T FromHash<T>(this Hashtable obj )  
        {
            var type = typeof(T);
            T result =(T)Activator.CreateInstance(typeof(T), new object[] {  });

            foreach (var item in type.GetProperties())
            {
                object value = obj[item.Name];
                //if (item.GetType() == typeof(Boolean))
                //{
                //    bool boolValue = bool.Parse(value.ToString());

                //    item.SetValue(result, boolValue, null);
                //}
                //else
                {
                    item.SetValue(result, Convert.ChangeType(value, item.PropertyType), null);
                }

            }
            return result;
        }
        
    }

    public static class ObjectExtesions
    {
        public static T Clone<T>(this object obj )  
        {
            string xml = SerializationHelper.SerializeToXml<T>((T) obj);
            return SerializationHelper.DeserializeFromXml<T>(xml);
        }

        public static void SetProperty(this object obj, string property, object value)
        {

            var type = obj.GetType();
            var pi = type.GetProperty(property);
            if (pi != null)
            {
                Object objValue = Convert.ChangeType(value, pi.PropertyType);

                pi.SetValue(obj, objValue, null);
            }
        }

        public static Hashtable ToHashtable(this object obj)
        {
            Hashtable hs = new Hashtable();
            var type = obj.GetType();
            var progs = type.GetProperties();
            foreach (var item in progs)
            {
                hs.Add(item.Name, item.GetValue(obj,null));
            }
            return hs;

        }
    }
}
