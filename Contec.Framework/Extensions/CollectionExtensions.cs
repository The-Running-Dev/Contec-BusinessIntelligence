using System.Collections.Generic;
using System.Collections.Specialized;

namespace Contec.Framework.Extensions
{
    public static class CollectionExtensions
    {
        public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.Get(key, default(TValue));
        }

        public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            if (dictionary.ContainsKey(key)) return dictionary[key];
            return defaultValue;
        }
        #region GetString(this NameValueCollection collection, string name, string defaultValue = "")

        public static string GetString(this NameValueCollection collection, string name, string defaultValue = "")
        {
            return collection.Get(name) ?? defaultValue;
        }

        #endregion

        #region GetShort(this NameValueCollection collection, string name, long defaultValue = "")

        public static short GetShort(this NameValueCollection collection, string name, short defaultValue = 0)
        {
            return collection.GetString(name).ToShort(defaultValue);
        }

        #endregion

        #region GetLong(this NameValueCollection collection, string name, long defaultValue = "")

        public static long GetLong(this NameValueCollection collection, string name, long defaultValue = 0)
        {
            return collection.GetString(name).ToLong(defaultValue);
        }

        #endregion

        #region GetInteger(this NameValueCollection collection, string name, int defaultValue = "")

        public static int GetInteger(this NameValueCollection collection, string name, int defaultValue = 0)
        {
            return collection.GetString(name).ToInteger(defaultValue);
        }

        #endregion

        #region GetBoolean(this NameValueCollection collection, string name, int defaultValue)

        public static bool GetBoolean(this NameValueCollection collection, string name, bool defaultValue = false)
        {
            return collection.GetString(name).ToBoolean(defaultValue);
        }

        #endregion
    }
}
