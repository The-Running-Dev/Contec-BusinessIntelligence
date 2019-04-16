using System;

namespace Contec.Framework.Extensions
{
    /// <summary>
    /// A collection of Uri extensions
    /// </summary>
    public static class UriExtensions
    {
        /// <summary>
        /// Appends a object as a string to an existing Uri
        /// </summary>
        /// <param name="baseUri">The base Uri to which to append the string</param>
        /// <param name="objectAsStringToAppend"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Uri Append(this Uri baseUri, object objectAsStringToAppend)
        {
            var returnValue = baseUri;

            try
            {
                returnValue = new Uri(string.Format("{0}{1}{2}",
                    baseUri, !baseUri.ToString().EndsWith("/") ? "/" : string.Empty, objectAsStringToAppend));

                //returnValue = new Uri(
                //    $"{baseUri}{(!baseUri.ToString().EndsWith("/") ? "/" : string.Empty)}{objectAsStringToAppend}");
            }
            catch {}

            return returnValue;
        }
    }
}