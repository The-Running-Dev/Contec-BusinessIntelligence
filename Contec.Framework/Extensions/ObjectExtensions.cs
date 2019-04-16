using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Reflection;
using System.Xml.Serialization;
using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace Contec.Framework.Extensions
{
    public static class ObjectExtensions
    {
        public static bool Is<T>(this object target)
        {
            return target is T;
        }

        public static bool Is(this object target, Type other)
        {
            return target.GetType() == other ||
                other.IsInstanceOfType(target);
        }

        public static void CallOn<T>(this object target, Action<T> action) where T : class
        {
            var subject = target as T;

            if (subject != null)
            {
                action(subject);
            }
        }

        public static T As<T>(this object obj) where T : class
        {
            return obj as T;
        }

        public static string ToNullSafeString(this object value)
        {
            return value == null ? string.Empty : value.ToString();
        }

        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        public static bool NotNull(this object obj)
        {
            return !IsNull(obj);
        }

        public static string ToJson(this object data, string variableName = null)
        {
            string returnValue = null;

            var jss = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            var formatting = Formatting.None;

#if DEBUG
            formatting = Formatting.Indented;
#endif

            if (!string.IsNullOrEmpty(variableName))
            {
                returnValue = string.Format("var {0} = {1}", variableName, JsonConvert.SerializeObject(data, formatting, jss));
            }
            else
            {
                returnValue = JsonConvert.SerializeObject(data, formatting, jss);
            }

            return returnValue;
        }

        public static T FromJson<T>(this string jsonString)
        {
            var returnValue = default(T);

            try
            {
                returnValue = JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch { }

            return returnValue;
        }

        /// <summary>
        /// Serializes an object to an XML string
        /// </summary>
        /// <param name="objectToSerialize">The object to serialize</param>
        /// <returns>An Xml string representing the object</returns>
        public static string ToXml(this object objectToSerialize)
        {
            var xmlString = string.Empty;

            try
            {
                // Add an empty namespace and empty value
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");

                using (var writer = new StringWriter())
                {
                    // Get an instance of the schema
                    var serializer = new XmlSerializer(objectToSerialize.GetType());
                    serializer.Serialize(writer, objectToSerialize, ns);

                    xmlString = writer.ToString();
                }
            }
            catch
            {
                xmlString = null;
            }

            return xmlString;
        }

        /// <summary>
        /// Serializes an object to an XML document
        /// </summary>
        /// <param name="objectToSerialize">The object to serialize</param>
        /// <returns>An Xml document representing the object</returns>
        public static XmlDocument ToXmlDocument(this object objectToSerialize)
        {
            var xmlDocument = new XmlDocument();

            try
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");

                var nav = xmlDocument.CreateNavigator();
                using (var writer = nav.AppendChild())
                {
                    var serializer = new XmlSerializer(objectToSerialize.GetType());
                    serializer.Serialize(writer, objectToSerialize, ns);
                }
            }
            catch
            {
                xmlDocument = null;
            }

            return xmlDocument;
        }

        public static T FromXml<T>(this string xml)
        {
            T returnedXmlClass = default(T);

            try
            {
                using (TextReader reader = new StringReader(xml))
                {
                    try
                    {
                        returnedXmlClass =
                            (T)new XmlSerializer(typeof(T)).Deserialize(reader);
                    }
                    catch (InvalidOperationException)
                    {
                    }
                }
            }
            catch
            {
            }

            return returnedXmlClass;
        }

        public static Guid ToGuid(this object uniqueID)
        {
            var returnValue = Guid.Empty;

            if (uniqueID != null)
            {
                returnValue = uniqueID.ToString().ToGuid();
            }

            return returnValue;
        }

        public static TReturn GetPropertyValue<TReturn>(this object obj, string propertyName)
        {
            var returnValue = default(TReturn);
            var propertyInfo = obj.GetType().GetProperty(propertyName) ?? FindProperty(obj, propertyName);

            if (propertyInfo != null)
            {
                var rawValue = propertyInfo.GetValue(obj, new object[] { });

                if ((rawValue != null))
                {
                    returnValue = (TReturn)rawValue;
                }
            }

            return returnValue;
        }

        public static object GetPropValue(this object obj, string name)
        {
            foreach (var part in name.Split('.'))
            {
                if (obj == null) { return null; }

                var type = obj.GetType();
                var info = type.GetProperty(part);

                if (info == null) { return null; }

                obj = info.GetValue(obj, null);
            }
            return obj;
        }

        public static T GetPropValue<T>(this object obj, string name)
        {
            var retval = GetPropValue(obj, name);
            if (retval == null) { return default(T); }

            return (T)retval;
        }
        public static void SetPropertyValue(this object obj, string propertyName, object propertyValue)
        {
            var propertyInfo = obj.GetType().GetProperty(propertyName, BindingFlags.FlattenHierarchy);

            if ((propertyInfo != null))
            {
                propertyInfo.SetValue(obj, propertyValue);
            }
        }

        public static PropertyInfo FindProperty(this object obj, string propertyPath)
        {
            var currentType = obj.GetType();
            PropertyInfo p = null;

            foreach (var propertyName in propertyPath.Split('.'))
            {
                p = currentType.GetProperty(propertyName);

                if (p != null)
                {
                    currentType = p.PropertyType;
                }
            }

            return p;
        }

        /// <summary>
        /// Creates a CSV file from a IEnumerable list of objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values">The list of object to convert to a CSV stream</param>
        /// <param name="fileName"></param>
        /// <remarks></remarks>
        public static void ToCsv<T>(this IEnumerable<T> values, string fileName)
        {
            var enumerable = values as T[] ?? values.ToArray();

            // Try to get a CsvClassMap type from the assembly
            var mapperType =
                 typeof(T).Assembly
                    .GetTypes()
                    .FirstOrDefault(x => x.IsSubclassOf(typeof(CsvClassMap<T>)));

            using (var csvWriter = new CsvWriter(File.CreateText(fileName)))
            {
                csvWriter.Configuration.HasHeaderRecord = true;

                if (mapperType != null)
                {
                    csvWriter.Configuration.RegisterClassMap(mapperType);
                }

                csvWriter.WriteHeader(enumerable.First().GetType());
                csvWriter.WriteRecords(enumerable);
            }
        }

        /// <summary>
        /// Creates a CSV byte stream from a IEnumerable list
        /// </summary>
        /// <param name="values">The list of object to convert to a CSV stream</param>
        /// <remarks></remarks>
        public static byte[] ToCsv(this IEnumerable values, bool quoteAllFields = true)
        {
            var firstElement = values.GetType().GetGenericArguments().First();

            // Try to get a CsvClassMap type from the assembly
            // by looking at Subclasses of CsvClassMap where the
            // first generic type argument matches the type
            // of first element in the list
            var mapperType =
                firstElement.Assembly
                    .GetTypes()
                    .FirstOrDefault(
                        x =>
                            x.IsSubclassOf(typeof(CsvClassMap)) &&
                            x.BaseType.GetGenericArguments().First() == firstElement);

            using (var memoryStream = new MemoryStream())
            {
                using (var stream = new StreamWriter(memoryStream))
                {
                    using (var csvWriter = new CsvWriter(stream))
                    {
                        csvWriter.Configuration.UseExcelLeadingZerosFormatForNumerics = true;
                        csvWriter.Configuration.QuoteAllFields = quoteAllFields;
                        csvWriter.Configuration.HasHeaderRecord = true;

                        if (mapperType != null)
                        {
                            csvWriter.Configuration.RegisterClassMap(mapperType);
                        }

                        // We write the field names as the first line of output CSV
                        csvWriter.WriteHeader(firstElement);
                        csvWriter.WriteRecords(values);
                    }
                }

                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Creates a CSV byte stream from a list of objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values">The list of object to convert to a CSV stream</param>
        /// <remarks></remarks>
        public static MemoryStream ToCsv<T>(this IEnumerable<T> values)
        {
            var memoryStream = new MemoryStream();
            var enumerable = values as T[] ?? values.ToArray();

            // Try to get a CsvClassMap type from the assembly
            var mapperType =
                typeof(T).Assembly
                .GetTypes()
                .FirstOrDefault(x => x.IsSubclassOf(typeof(CsvClassMap<T>)));

            using (var writer = new StreamWriter(memoryStream))
            {
                writer.AutoFlush = true;

                using (var csvWriter = new CsvWriter(writer))
                {
                    csvWriter.Configuration.HasHeaderRecord = true;

                    if (mapperType != null)
                    {
                        csvWriter.Configuration.RegisterClassMap(mapperType);
                    }

                    csvWriter.WriteHeader(enumerable.First().GetType());
                    csvWriter.WriteRecords(enumerable);
                }
            }

            return memoryStream;
        }
    }
}
