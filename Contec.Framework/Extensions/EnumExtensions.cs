using System;
using System.Linq;
using System.Web.Mvc;
using System.ComponentModel;
using System.Collections.Generic;
using Contec.Framework.Models;

namespace Contec.Framework.Extensions
{
    public static class EnumExtension
    {
        /// <summary>
        /// This method returns the string value attribute of an enum
        /// </summary>
        /// <param name="value">The enum to verify</param>
        /// <returns>The string value of the enum</returns>
        public static string GetDescription(this Enum value)
        {
            // Get the type
            var type = value.GetType();

            // Get fieldinfo for this type
            var fi = type.GetField(value.ToString());

            // Get the description attributes
            var attribs = new DescriptionAttribute[0];

            if (fi != null && fi.IsDefined(typeof(DescriptionAttribute), true))
            {
                attribs = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
            }

            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].Description : value.ToString().ToSeparatedWords();
        }

        /// <summary>
        /// This method attempts to convert an integer value into an Enum value
        /// </summary>
        /// <typeparam name="T">The enum to convert to</typeparam>
        /// <param name="value">The value to convert</param>
        /// <returns>Converted enum or its default value</returns>
        public static T CastToEnum<T>(this int value)
        {
            return value.CastToEnum<T>(default(T));
        }

        /// <summary>
        /// This method attempts to convert an integer value into an Enum value
        /// </summary>
        /// <typeparam name="T">The enum to convert to</typeparam>
        /// <param name="value">The value to convert</param>
        /// <param name="defaultValue">The default value to return if the given value can't be
        /// converted.</param>
        /// <returns>Converted enum or its default value</returns>
        public static T CastToEnum<T>(this int value, T defaultValue)
        {
            var errorMsg = String.Format("Failed Convert Value: {0} to Enum: {1}",
                                         value, typeof(T));
            var retval = defaultValue;

            if (retval is Enum)
            {
                try
                {
                    retval = (T)Enum.ToObject(typeof(T), value);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Write(ex.ToString());
                }
            }

            return retval;
        }

        /// <summary>
        /// This method attempts to convert a string value into an Enum value
        /// </summary>
        /// <typeparam name="T">The enum to convert to</typeparam>
        /// <param name="value">The value to convert</param>
        /// <returns>Converted enum or its default value</returns>
        public static T CastToEnum<T>(this string value)
        {
            return value.CastToEnum<T>(false);
        }

        /// <summary>
        /// This method attempts to convert a string value into an Enum value
        /// </summary>
        /// <typeparam name="T">The enum to convert to</typeparam>
        /// <param name="value">The value to convert</param>
        /// <param name="containsCheck">Flag to perform a contains check</param>
        /// <returns>Converted enum or its default value</returns>
        public static T CastToEnum<T>(this string value, bool containsCheck)
        {
            var errorMsg = String.Format("Failed Convert Value: {0} to Enum: {1}",
                                         value, typeof(T));
            var retval = default(T);

            // to be safe, lets check against both the string value and the actual value of the enum
            // to verify a match.  Just trying to parse won't always guarantee success
            if (!string.IsNullOrWhiteSpace(value) && retval is Enum)
            {
                try
                {
                    // get the type of enum
                    var enumType = typeof(T);

                    // cast to lower
                    value = value.ToLower();

                    // loop on all possible values in the enum
                    foreach (T item in Enum.GetValues(enumType))
                    {
                        // convert from a generic enum to a the object Enum
                        var temp = (Enum)Enum.Parse(enumType, item.ToString());
                        var enumName = temp.ToString().ToLower();
                        var enumDescription = temp.GetDescription().ToLower();

                        // now perform a check against the name and the display name
                        if (enumName == value || enumDescription == value)
                        {
                            retval = item;
                            break;
                        }

                        // perform a contains check.
                        // See if the value passed in, is contained in
                        if (containsCheck &&
                            (enumName.Contains(value) || enumDescription.Contains(value)))
                        {
                            retval = item;
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Write(ex.ToString());
                }
            }

            return retval;
        }

        /// <summary>
        /// This method accepts a string value and checks if a provided value exists in an enum
        /// </summary>
        /// <typeparam name="T">The enum type to check</typeparam>
        /// <param name="value">The value to check against all enum properties</param>
        /// <returns>True if it exsits</returns>
        public static bool ExistsInEnum<T>(this string value)
        {
            return value.ExistsInEnum<T>(false);
        }

        /// <summary>
        /// This method accepts a string value and checks if a provided value exists in an enum
        /// </summary>
        /// <typeparam name="T">The enum type to check</typeparam>
        /// <param name="value">The value to check against all enum properties</param>
        /// <param name="containsCheck">Flag to perform a contains check</param>
        /// <returns>True if it exsits</returns>
        public static bool ExistsInEnum<T>(this string value, bool containsCheck)
        {
            var retval = false;
            var dummy = default(T);

            // to be safe, lets check against both the string value and the actual value of the enum
            // to verify a match.  Just trying to parse won't always guarantee success
            if (!string.IsNullOrWhiteSpace(value) && dummy is Enum)
            {
                try
                {
                    // get the type of enum
                    var enumType = typeof(T);

                    // cast to lower
                    value = value.ToLower();

                    // loop on all possible values in the enum
                    foreach (T item in Enum.GetValues(enumType))
                    {
                        // convert from a generic enum to a the object Enum
                        var temp = (Enum)Enum.Parse(enumType, item.ToString());
                        var enumName = temp.ToString().ToLower();
                        var enumStringvalue = temp.GetDescription().ToLower();

                        // now perform a check against the name and the display name
                        if (enumName == value || enumStringvalue == value)
                        {
                            retval = true;
                            break;
                        }

                        // perform a contains check.
                        // See if the value passed in, is contained in
                        if (containsCheck &&
                            (enumName.Contains(value) || enumStringvalue.Contains(value)))
                        {
                            retval = true;
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Write(ex.ToString());
                }
            }

            return retval;
        }

        public static List<SelectListItem> ToSelectList<T>(this Type type, List<T> ignoreList = null, T selectedItem = default(T))
        {
            if (ignoreList != null)
            {
                return Enum.GetValues(type).Cast<object>()
                    .Where(item => !ignoreList.Exists(y => y.Equals((T)item))).Select(
                        item =>
                            new SelectListItem()
                            {
                                Selected = (item.Equals(selectedItem)),
                                Text = ((Enum)item).GetDescription(),
                                Value = ((int)item).ToString()
                            })
                    .ToList();
            }

            return Enum.GetValues(type).Cast<object>()
                .Select(
                    item =>
                        new SelectListItem()
                        {
                            Selected = (item.Equals(selectedItem)),
                            Text = ((Enum)item).GetDescription(),
                            Value = ((int)item).ToString()
                        })
                .ToList();
        }

        public static List<ListItem> ToListItems<T>(this Type type, List<T> ignoreList = null)
        {
            if (ignoreList != null)
            {
                return Enum.GetValues(type).Cast<object>()
                    .Where(item => !ignoreList.Exists(y => y.Equals((T)item))).Select(
                        item =>
                            new ListItem()
                            {
                                Id = ((int)item),
                                Name = ((Enum)item).GetDescription()
                            })
                    .ToList();
            }

            return Enum.GetValues(type).Cast<object>()
                .Select(
                    item =>
                        new ListItem()
                        {
                            Id = ((int)item),
                            Name = ((Enum)item).GetDescription()
                        })
                .ToList();
        }
    }
}