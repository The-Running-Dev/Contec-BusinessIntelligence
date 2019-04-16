using System;
using System.IO;
using System.Web;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Contec.Framework.Models;
using Contec.Framework.Strings;

using Ganss.XSS;

namespace Contec.Framework.Extensions
{
    public static class StringExtensions
    {
        private static readonly Regex SlugMeDisallowedCharactersRegex = new Regex(@"[^a-z0-9_-]+", RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex SlugMeRemoveDuplicatedDashes = new Regex("-+", RegexOptions.Singleline | RegexOptions.Compiled);
        public const int MaxSlugLength = 140;

        public static string Slugify(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            var slug = value.ToLowerInvariant();

            // First convert all "&" to "-and-"
            slug = slug.Replace(@"&", @"-and-");

            // Then replace all disallowed characters with "-";
            slug = SlugMeDisallowedCharactersRegex.Replace(slug, "-");

            // Replace consecutive dashes with single 
            slug = SlugMeRemoveDuplicatedDashes.Replace(slug, "-");

            slug = slug.Trim('-');

            if (slug.Length > MaxSlugLength)
                slug = slug.Substring(0, MaxSlugLength);

            return slug;
        }

        /// <summary>
        /// This helper string method performs a mass replace on a string of data.
        /// </summary>
        /// <param name="data">The string information to perform search/replace on</param>
        /// <param name="replacements">The key/value pairs to replace with</param>
        /// <returns></returns>
        public static string MassReplace(this string data, Dictionary<string, string> replacements)
        {
            if (string.IsNullOrEmpty(data) || replacements == null || replacements.Count == 0)
            {
                return data;
            }

            return replacements.Aggregate(data, (current, info) => current.Replace(info.Key, info.Value));
        }

        public static string If(this string html, Expression<Func<bool>> modelBooleanValue)
        {
            return GetBooleanPropertyValue(modelBooleanValue) ? html : string.Empty;
        }

        public static string IfNot(this string html, Expression<Func<bool>> modelBooleanValue)
        {
            return !GetBooleanPropertyValue(modelBooleanValue) ? html : string.Empty;
        }

        public static string ToNullSafeString(this object value)
        {
            return (value != null) ? value.ToString() : string.Empty;
        }

        public static bool IsNullOrWhiteSpace(this string stringValue)
        {
            return string.IsNullOrWhiteSpace(stringValue);
        }

        public static bool IsNullOrEmpty(this string stringValue)
        {
            return string.IsNullOrEmpty(stringValue);
        }

        public static bool IsNotEmpty(this string stringValue)
        {
            return !string.IsNullOrWhiteSpace(stringValue);
        }

        public static bool IsEmpty(this string stringValue)
        {
            return string.IsNullOrWhiteSpace(stringValue);
        }

        public static bool ToBool(this string stringValue)
        {
            return !string.IsNullOrEmpty(stringValue) && bool.Parse(stringValue);
        }

        public static string ToFormat(this string stringFormat, params object[] args)
        {
            return String.Format(stringFormat, args);
        }

        public static string MakeEndWith(this string item, string endingValue)
        {
            if (item.IsNull()) item = string.Empty;

            if (item.EndsWith(endingValue) == false)
                item = item + endingValue;

            return item;
        }

        public static string MakeStartWith(this string item, string startwith)
        {
            if (item.IsNull()) item = string.Empty;

            if (item.ToLowerInvariant().StartsWith(startwith.ToLowerInvariant()))
                item = "{0}{1}".ToFormat(startwith, item);

            return item;
        }

        public static T To<T>(this string value, T defaultValue)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                try
                {
                    if (typeof(T) == typeof(Guid))
                    {
                        var defGuid = (Guid)Convert.ChangeType(defaultValue, typeof(Guid));

                        return (T)Convert.ChangeType(value.ToGuid(defGuid), typeof(T));
                    }

                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch
                {
                    return defaultValue;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Get the requested string as a Guid value.
        /// </summary>
        /// <param name="stringValue">The string to convert to a Guid value</param>
        /// <param name="defaultValue">The default value to return if we are not able to convert
        /// the given string to a Guid value.</param>
        /// <returns>The Guid value for the given string or the given default value if we are
        /// not able to convert the given string to a Guid value.</returns>
        public static Guid ToGuid(this string stringValue, Guid defaultValue = default(Guid))
        {
            if (!string.IsNullOrWhiteSpace(stringValue))
            {
                Guid value;

                if (Guid.TryParse(stringValue, out value))
                {
                    return value;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Get the requested string as a short value.
        /// </summary>
        /// <param name="stringValue">The string to convert to a short value</param>
        /// <param name="defaultValue">The default value to return if we are not able to convert
        /// the given string to a short value.</param>
        /// <returns>The short value for the given string or the given default value if we are
        /// not able to convert the given string to a short value.</returns>
        public static short ToShort(this string stringValue, short defaultValue = (short)0)
        {
            if (!string.IsNullOrWhiteSpace(stringValue))
            {
                short value;

                if (Int16.TryParse(stringValue, out value))
                {
                    return value;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Get the requested string as a long value.
        /// </summary>
        /// <param name="stringValue">The string to convert to a long value</param>
        /// <param name="defaultValue">The default value to return if we are not able to convert
        /// the given string to a long value.</param>
        /// <returns>The long value for the given string or the given default value if we are
        /// not able to convert the given string to a long value.</returns>
        public static long ToLong(this string stringValue, long defaultValue = 0)
        {
            if (!string.IsNullOrWhiteSpace(stringValue))
            {
                long value;

                if (Int64.TryParse(stringValue, out value))
                {
                    return value;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Get the requested string as a integer value.
        /// </summary>
        /// <param name="stringValue">The string to convert to a integer value</param>
        /// <param name="defaultValue">The default value to return if we are not able to convert
        /// the given string to a integer value.</param>
        /// <returns>The integer value for the given string or the given default value if we are
        /// not able to convert the given string to a integer value.</returns>
        public static int ToInteger(this string stringValue, int defaultValue = 0)
        {
            if (!string.IsNullOrWhiteSpace(stringValue))
            {
                int value;

                if (Int32.TryParse(stringValue, out value))
                {
                    return value;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Get the requested string as a decimal value.
        /// </summary>
        /// <param name="stringValue">The string to convert to a decimal value</param>
        /// <param name="defaultValue">The default value to return if we are not able to convert
        /// the given string to a decimal value.</param>
        /// <returns>The integer value for the given string or the given default value if we are
        /// not able to convert the given string to a decimal value.</returns>
        public static decimal ToDecimal(this string stringValue, decimal defaultValue = 0)
        {
            if (!string.IsNullOrWhiteSpace(stringValue))
            {
                decimal value;

                if (Decimal.TryParse(stringValue, out value))
                {
                    return value;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Get the requested string as a double value.
        /// </summary>
        /// <param name="stringValue">The string to convert to a double value</param>
        /// <param name="defaultValue">The default value to return if we are not able to convert
        /// the given string to a double value.</param>
        /// <returns>The integer value for the given string or the given default value if we are
        /// not able to convert the given string to a double value.</returns>
        public static double ToDouble(this string stringValue, double defaultValue = 0)
        {
            if (!string.IsNullOrWhiteSpace(stringValue))
            {
                double value;

                if (Double.TryParse(stringValue, out value))
                {
                    return value;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Get the requested string as a float value.
        /// </summary>
        /// <param name="stringValue">The string to convert to a float value</param>
        /// <param name="defaultValue">The default value to return if we are not able to convert
        /// the given string to a float value.</param>
        /// <returns>The integer value for the given string or the given default value if we are
        /// not able to convert the given string to a float value.</returns>
        public static double ToFloat(this string stringValue, float defaultValue = 0)
        {
            if (!string.IsNullOrWhiteSpace(stringValue))
            {
                float value;

                if (float.TryParse(stringValue, out value))
                {
                    return value;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Get the requested string as a boolean value.
        /// </summary>
        /// <param name="stringValue">The string to convert to a boolean value</param>
        /// <param name="defaultValue">The default value to return if we are not able to convert
        /// the given string to a boolean value.</param>
        /// <returns>The boolean value for the given string or the given default value if we are
        /// not able to convert the given string to a boolean value.</returns>
        public static bool ToBoolean(this string stringValue, bool defaultValue = false)
        {
            if (!string.IsNullOrWhiteSpace(stringValue))
            {
                if (Constants.TrueValues.Includes(stringValue))
                {
                    return true;
                }

                if (Constants.FalseValues.Includes(stringValue))
                {
                    return false;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Get an array of bytes for the given string in the given encoding.
        /// </summary>
        /// <param name="value">The string to get the array of bytes for.</param>
        /// <param name="encoding">The encoding to use when generating the array of bytes.</param>
        /// <returns>An array of bytes for the given string in the given encoding.</returns>
        public static byte[] ToBytes(this string value, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.Default;

            return encoding.GetBytes(value);
        }

        /// <summary>
        /// Get a string given an array of bytes in a given encoding.
        /// </summary>
        /// <param name="obj">The byte array to return a string for</param>
        /// <param name="encoding">The encoding to use when generating the string.</param>
        /// <returns>An string from an array of bytes</returns>
        public static string FromBytes(this byte[] obj, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.Default;

            return encoding.GetString(obj);
        }

        public static decimal BytesToGigabytes(this string value)
        {
            return value.ToDecimal().BytesToGigabytes();
        }

        public static string BytesToDisplay(this decimal value)
        {
            var valueInGigabytes = value.BytesToGigabytes();
            var valueInMegabytes = value.BytesToMegabytes();
            var valueInKilobytes = value.BytesToKilobytes();

            if (valueInGigabytes >= 1)
            {
                return string.Format("{0} GB", Decimal.Round(valueInGigabytes, 2));
            }

            if (valueInMegabytes >= 1)
            {
                return string.Format("{0} MB", Decimal.Round(valueInMegabytes, 2));
            }

            if (valueInKilobytes >= 1)
            {
                return string.Format("{0} KB", Decimal.Round(valueInKilobytes, 2));
            }

            return string.Format("{0} Bytes", Decimal.Round(value, 2));
        }

        public static decimal BytesToKilobytes(this decimal value)
        {
            return (value) / (1024);
        }

        public static decimal BytesToMegabytes(this decimal value)
        {
            return (value) / (1024 * 1024);
        }

        public static decimal BytesToGigabytes(this decimal value)
        {
            return (value) / (1024 * 1024 * 1024);
        }

        public static bool IsMegabytes(this decimal value)
        {
            return value >= (1024 * 1024) && value < (1024 * 1024 * 1024);
        }

        public static decimal GigabytesToBytes(this string value)
        {
            return value.ToDecimal().GigabytesToBytes();
        }

        public static decimal GigabytesToBytes(this decimal value)
        {
            return (value) * (1024 * 1024 * 1024);
        }

        public static bool IsGigabytes(this decimal value)
        {
            return value >= (1024 * 1024 * 1024);
        }

        public static bool IsEqualTo(this string firstString, string secondString)
        {
            if (firstString.IsEmpty())
            {
                firstString = string.Empty;
            }

            return firstString.Equals(secondString, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool IsNotEqualTo(this string firstString, string secondString)
        {
            if ((firstString.IsEmpty()))
            {
                firstString = string.Empty;
            }

            return !firstString.IsEqualTo(secondString);
        }

        public static List<string> ToList(this string delimitedList, string delimiter = ",")
        {
            var listOfStrings = new List<string>();

            if (delimitedList.Contains(delimiter))
            {
                // Add each split string to the list of strings
                listOfStrings.AddRange(delimitedList.Split(delimiter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            }
            else
            {
                listOfStrings.Add(delimitedList);
            }

            return listOfStrings;
        }

        public static bool IsEditCommand(this string commandName)
        {
            return commandName.IsEqualTo("Edit") | commandName.IsEqualTo("EditItem");
        }

        public static bool IsEnableCommand(this string commandName)
        {
            return commandName.IsEqualTo("Enable") | commandName.IsEqualTo("EnableItem");
        }

        public static bool IsMoveUpCommand(string commandName)
        {
            return commandName.IsEqualTo("MoveUp") | commandName.IsEqualTo("MoveItemUp");
        }

        public static bool IsMoveDownCommand(this string commandName)
        {
            return commandName.IsEqualTo("MoveDown") | commandName.IsEqualTo("MoveItemDown");
        }

        public static bool IsDeleteCommand(this string commandName)
        {
            return commandName.IsEqualTo("Delete") | commandName.IsEqualTo("DeleteItem");
        }

        public static bool IsPublicCommand(this string commandName)
        {
            return commandName.IsEqualTo("Public") | commandName.IsEqualTo("PublicItem");
        }

        /// <summary>
        /// Converts a delimited string list to an array list
        /// </summary>
        /// <param name="delimitedList">The string list to convert</param>
        /// <param name="delimiter">The delimiter to use ("," by default)</param>
        /// <returns>The ArrayList of strings</returns>
        /// <remarks></remarks>
        public static ArrayList ToArrayList(this string delimitedList, string delimiter = ",")
        {
            var arrayOfStrings = new ArrayList();

            if (delimitedList.Contains(delimiter))
            {
                foreach (var id in delimitedList.Split(delimiter.ToCharArray()))
                {
                    arrayOfStrings.Add(id);
                }
            }

            return arrayOfStrings;
        }

        /// <summary>
        /// Converts a Guid list to a string list of Guids
        /// </summary>
        /// <param name="listOfGuid">The list of Guid to convert</param>
        /// <returns>The List(of String)</returns>
        /// <remarks></remarks>
        public static List<string> GuidToShortGuidStringList(this List<Guid> listOfGuid)
        {
            var listOfGuidStrings = new List<string>();

            // For each string, convert it to a ShortGuid string
            listOfGuid.ForEach(x => listOfGuidStrings.Add(x.ToString()));

            return listOfGuidStrings;
        }

        public static Guid ToGuid(this string uniqueID)
        {
            var returnValue = Guid.Empty;

            try
            {
                returnValue = new Guid(uniqueID.IsNotEmpty() ? uniqueID : Guid.Empty.ToString());
            }
            catch
            {

            }

            return returnValue;
        }

        public static bool BeginsWithString(this string firstString, string secondString)
        {
            return firstString.StartsWith(secondString, StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool EndsWithString(this string firstString, string secondString)
        {
            return firstString.EndsWith(secondString, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Checks if the provided url is the same as the one being compared to
        /// </summary>
        /// <param name="firstUrl">The base URL</param>
        /// <param name="secondUrl">The URL to compare to</param>
        /// <returns>True or false for the equality of the two URL</returns>
        /// <remarks></remarks>
        public static bool IsEqualToUrl(this string firstUrl, string secondUrl)
        {
            return !firstUrl.IsNullOrEmpty() &&
                Path.GetFileNameWithoutExtension(firstUrl.Replace("~/", "")).IsEqualTo(Path.GetFileNameWithoutExtension(secondUrl.Replace("~/", "")));
        }

        /// <summary>
        /// Trims a string to the specified size
        /// </summary>
        /// <param name="stringValue">The string to trim</param>
        /// <param name="maxSize">The length to trim to</param>
        /// <param name="suffix">The suffix to append</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string TrimTo(this string stringValue, int maxSize, string suffix = "...")
        {
            return stringValue.Length >= maxSize ? stringValue.Substring(0, maxSize) + suffix : stringValue;
        }

        /// <summary>
        /// Converts the provided string to ProperCase
        /// </summary>
        /// <param name="stringValue">The string to convert</param>
        /// <returns>The converted string</returns>
        /// <remarks></remarks>
        public static string ToProperCase(this string stringValue)
        {
            return string.Empty;
            //return SubSonic.Sugar.Strings.ToProper(stringValue);
        }

        /// <summary>
        /// Checks if the string contains another string
        /// </summary>
        /// <param name="stringValue">The string to search in</param>
        /// <param name="searchTerm">The search term</param>
        /// <returns>True or false for the containment of the search term</returns>
        /// <remarks></remarks>
        public static bool Includes(this string stringValue, string searchTerm)
        {
            return !string.IsNullOrEmpty(searchTerm) && stringValue.ToLower().Contains(searchTerm.ToLower());
        }

        /// <summary>
        /// Returns the trimmed value of a string
        /// if the string is not null or empty
        /// </summary>
        /// <param name="stringValue">The value to return if not empty</param>
        /// <param name="defaultValue"></param>
        /// <returns>The value or empty string</returns>
        /// <remarks></remarks>
        public static string SetIfNotEmpty(this string stringValue, string defaultValue = "")
        {
            return !string.IsNullOrEmpty(stringValue) ? stringValue.Trim() : defaultValue;
        }

        /// <summary>
        /// Removes a string whitin the string
        /// </summary>
        /// <param name="stringValue"></param>
        /// <param name="stringToRemove">The value of the string to be removed</param>
        /// <returns>The original string without the removed value</returns>
        /// <remarks></remarks>
        public static string Remove(this string stringValue, string stringToRemove)
        {
            return stringValue.IsNotEmpty() ? stringValue.Replace(stringToRemove, string.Empty).Trim() : stringValue;
        }

        /// <summary>
        /// Removes any extra spaces from the beginning or end of the string
        /// </summary>
        /// <returns>The original string without the spaces</returns>
        /// <remarks></remarks>
        public static string TrimSpaces(this string stringValue)
        {
            return stringValue.IsNotEmpty() ? stringValue.Trim() : stringValue;
        }

        /// <summary>
        /// Returns the DateTime representation of a string
        /// </summary>
        /// <param name="stringValue">The string value to convert</param>
        /// <returns>The DateTime value of the string</returns>
        /// <remarks></remarks>
        public static DateTime ToDate(this string stringValue, string valueIfEmpty = "")
        {
            var defaultValue = Convert.ToDateTime(valueIfEmpty);
            var dateTimeValue = default(DateTime);

            if (!stringValue.IsEmpty())
            {
                if (!DateTime.TryParse(stringValue, out dateTimeValue))
                {
                    dateTimeValue = defaultValue;
                }
            }

            return dateTimeValue;
        }

        /// <summary>
        /// Returns the true/false for the boolean representation of a string
        /// </summary>
        /// <param name="stringValue">The string value to test</param>
        /// <returns>The true/false value of the string</returns>
        /// <remarks></remarks>
        public static bool IsTrue(this string stringValue)
        {
            return stringValue.ToBoolean();
        }

        /// <summary>
        /// Returns the long representation of a string
        /// </summary>
        /// <param name="stringValue">The string to convert</param>
        /// <returns>The long value of the string</returns>
        /// <remarks></remarks>
        public static Uri ToUri(this string stringValue)
        {
            Uri valueAsUri = null;

            try
            {
                // Replace spaces with +
                stringValue = stringValue.Replace(" ", "+");
                valueAsUri = new Uri(stringValue);
            }
            catch
            {

            }

            return valueAsUri;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringValue"></param>
        /// <param name="part"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string UrlPart(this string stringValue, string part)
        {
            return Regex.Replace(stringValue, Constants.UrlPartsRegEx, part, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intpuString"></param>
        /// <param name="regularExpression"></param>
        /// <param name="replacementString"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string RegExReplace(this string intpuString, string regularExpression, string replacementString)
        {
            return Regex.Replace(intpuString, regularExpression, replacementString, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intpuString"></param>
        /// <param name="regularExpression"></param>
        /// <param name="replacementString"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string RegExReplaceLine(this string intpuString, string regularExpression, string replacementString)
        {
            return Regex.Replace(intpuString, regularExpression, replacementString, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intpuString"></param>
        /// <param name="regularExpression"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool RegExMatch(this string intpuString, string regularExpression)
        {
            var isMatch = false;

            if (!string.IsNullOrEmpty(intpuString))
            {
                isMatch = Regex.IsMatch(intpuString, regularExpression, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline);
            }

            return isMatch;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intpuString"></param>
        /// <param name="regularExpression"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool RegExMatchLine(this string intpuString, string regularExpression)
        {
            var isMatch = false;

            if (intpuString.IsNotEmpty())
            {
                isMatch = Regex.IsMatch(intpuString, regularExpression, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            }

            return isMatch;
        }

        public static string RemoveVariables(this string value)
        {
            return value.RegExReplace(@"\s\-\s{{.*}}", string.Empty);
        }

        /// <summary>
        /// Converts a partial URL (no schema) to a full URL with the schema
        /// </summary>
        /// <param name="partialUrl"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ResolveProtocol(this string partialUrl)
        {
            return string.Format("{0}://{1}", new Uri(HttpContext.Current.Request.Url.ToString()).Scheme, partialUrl);
        }

        /// <summary>
        /// Converts a local URL to a full URL
        /// based on the web applicaiton URL defined in the application settings
        /// </summary>
        /// <param name="relativeUrl"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ResolveUrl(this string relativeUrl)
        {
            //var fullUrl = string.Format("{0}{1}", Installation.Application.WebApplicationUrl, relativeUrl.Replace("~/", "/"));

            //if ((HttpContext.Current != null))
            //{
            //    fullUrl = HttpContext.Current.Request.IsSecureConnection ? fullUrl.Replace("http://", "https://") : fullUrl;
            //}

            //return fullUrl;
            return string.Empty;
        }

        /// <summary>
        /// Converts a local URL to a full URL
        /// based on the web applicaiton URL defined in the application settings
        /// </summary>
        /// <param name="relativeUrl"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ResolveBaseUrl(this string relativeUrl)
        {
            //var fullUrl = string.Format("{0}{1}", Installation.Application.Hostname, relativeUrl.Replace("~/", "/"));

            //if ((HttpContext.Current != null))
            //{
            //    fullUrl = HttpContext.Current.Request.IsSecureConnection ? fullUrl.Replace("http://", "https://") : fullUrl;
            //}

            //return fullUrl;
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="redirectUrl">The URL to redirect to</param>
        /// <param name="unlessRule"></param>
        public static void RedirectToUrlUnless(this string redirectUrl, bool unlessRule)
        {
            if ((HttpContext.Current == null)) return;

            if (!unlessRule)
            {
                HttpContext.Current.Response.Redirect(redirectUrl);
            }
        }

        /// <summary>
        /// Returns just the alphanumeric portion of the string
        /// </summary>
        /// <param name="stringValue">The Base Text String</param>
        /// <returns>Alphanumeric Only String</returns>
        /// <remarks></remarks>
        public static string AlphaNumeric(this string stringValue)
        {
            return Regex.Replace(stringValue, Constants.DirectoryNameReplaceRegEx, string.Empty);
        }

        public static bool ExistsInList(this string stringValue, List<string> stringList)
        {
            return stringList.Exists(currentItem => currentItem.ToLower() == stringValue.ToLower());
        }

        /// <summary>
        /// Validates the regular expression string by trying to create a RegEx object with it
        /// </summary>
        /// <param name="stringValue">The string containing the regular expression</param>
        /// <returns>True or false for the validity of the regular expression</returns>
        /// <remarks></remarks>
        public static bool IsRegEx(this string stringValue)
        {
            var isValid = false;

            if (!string.IsNullOrWhiteSpace(stringValue))
            {
                try
                {
                    new Regex(stringValue);
                    isValid = true;
                }
                catch
                {

                }
            }

            return isValid;
        }

        public static string ToSeparatedWords(this string value)
        {
            return Regex.Replace(value, "([A-Z][a-z])", " $1").Trim();
        }

        public static string TextToHtml(this string value)
        {
            return value.IsNotEmpty() ? HttpUtility.HtmlEncode(value.Replace(Environment.NewLine, "<br />").Replace("\n", "<br />")) : string.Empty;
        }

        public static string HtmlToText(this string value)
        {
            return value.IsNotEmpty() ? HttpUtility.HtmlDecode(value).Replace("<br />", Environment.NewLine).Replace("<br />", "\n") : string.Empty;
        }

        public static string DomainFromUsername(this string username)
        {
            return !username.IsNullOrEmpty() ? RegExReplace(username, ".*@(.*)", "$1") : string.Empty;
        }

        public static byte[] ToByteArray(this string value)
        {
            var numberChars = value.Length;
            var bytes = new byte[numberChars / 2];

            for (var i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(value.Substring(i, 2), 16);
            }

            return bytes;
        }

        private static bool GetBooleanPropertyValue(Expression<Func<bool>> modelBooleanValue)
        {
            var prop = modelBooleanValue.Body as MemberExpression;
            if (prop != null)
            {
                var info = prop.Member as PropertyInfo;
                if (info != null)
                {
                    return modelBooleanValue.Compile().Invoke();
                }
            }

            throw new ArgumentException("The modelBooleanValue parameter should be a single property, validation logic is not allowed, only 'x => x.BooleanValue' usage is allowed,");
        }

        /// <summary>
        /// Clean html from XSS
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string SanitizeHtml(this string html)
        {
            string cleanHtml = string.Empty;

            if (string.IsNullOrWhiteSpace(html))
                return cleanHtml;

            // https://github.com/mganss/HtmlSanitizer/
            var sanitizer = new HtmlSanitizer();

            return sanitizer.Sanitize(html).Replace("<br>", "<br />");
        }

        public static void ToConsole(this string contents)
        {
            Console.WriteLine(contents);
        }

        /// <summary>
        /// Converts XML to a dictionary of simple object types
        /// </summary>
        /// <param name="xml"></param>
        /// <returns>A dictionary of &amp;lt;string, object&amp;gt;</returns>
        public static Dictionary<string, object> XmlToDictionary(this string xml)
        {
            var thePayload = new Dictionary<string, object>();
            var xmlSerializer = new XmlSerializer(typeof(List<DataItem>));
            var stringReader = new StringReader(xml);

            foreach (var dataItem in (List<DataItem>)xmlSerializer.Deserialize(stringReader))
            {
                switch (dataItem.Type.ToLower())
                {
                    case "system.string":
                        {
                            thePayload.Add(dataItem.Key, dataItem.Value);
                            break;
                        }
                    case "system.int32":
                        {
                            thePayload.Add(dataItem.Key, int.Parse(dataItem.Value));
                            break;
                        }
                    case "system.int64":
                        {
                            thePayload.Add(dataItem.Key, long.Parse(dataItem.Value));
                            break;
                        }
                    case "system.boolean":
                        {
                            thePayload.Add(dataItem.Key, Convert.ToBoolean(dataItem.Value));
                            break;
                        }
                    default:
                        {
                            throw new Exception(string.Format("DeserializePayload(). Type Unknown: {0}", dataItem.Type));
                        }

                }
            }

            return thePayload;
        }
    }
}