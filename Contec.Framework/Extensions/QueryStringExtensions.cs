using System;

using Contec.Framework.Utilities;

namespace Contec.Framework.Extensions
{
    //public static class QueryStringExtensions
    //{
    //    private static readonly IWebUtils WebUtils = IocWrapper.Instance.GetService<IWebUtils>();

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <param name="defaultValue"></param>
    //    /// <remarks></remarks>
    //    public static int FromQuery(this string key, int defaultValue = 0)
    //    {
    //        return WebUtils.QueryString<int>(key);
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <param name="defaultValue"></param>
    //    /// <remarks></remarks>
    //    public static void FromQuery(this int value, string key, int defaultValue = 0)
    //    {
    //        value = WebUtils.QueryString<int>(key);

    //        value = (value != 0) ? value : defaultValue;
    //    }

    //    public static void FromQuery(this string value, string key, string defaultValue = "")
    //    {
    //        value = WebUtils.QueryString<string>(key);

    //        value = value.IsNotEmpty() ? value : defaultValue;
    //    }

    //    public static void FromQuery(this bool value, string key, bool defaultValue = false)
    //    {
    //        value = WebUtils.QueryString<bool>(key);

    //        value = (!value) ? value : defaultValue;
    //    }

    //    public static void FromQuery(this Guid value, string key)
    //    {
    //        value = WebUtils.QueryString<Guid>(key);

    //        value = value.IsNotEmpty() ? value : Guid.Empty;
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <param name="defaultValue"></param>
    //    /// <remarks></remarks>
    //    public static void FromPost(this int value, string key, int defaultValue = 0)
    //    {
    //        value = WebUtils.PostValue<int>(key);

    //        value = (value != 0) ? value : defaultValue;
    //    }

    //    public static void FromPost(ref string value, string key, string defaultValue = "")
    //    {
    //        value = WebUtils.PostValue<string>(key);

    //        value = value.IsNotEmpty() ? value : defaultValue;
    //    }

    //    public static void FromPost(ref bool value, string key, bool defaultValue = false)
    //    {
    //        value = WebUtils.PostValue<bool>(key);

    //        value = value ? value : defaultValue;
    //    }
    //}
}