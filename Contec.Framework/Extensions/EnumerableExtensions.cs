using System;
using System.Linq;
using System.Web.Mvc;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using Contec.Framework.Models;

namespace Contec.Framework.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool Exists<T>(this IEnumerable<T> values, Func<T, bool> evaluator)
        {
            return values.Count(evaluator) > 0;
        }

        public static void CallOnEach<T>(this IEnumerable enumerable, Action<T> action) where T : class
        {
            foreach (object o in enumerable)
            {
                o.CallOn(action);
            }
        }

        [DebuggerStepThrough]
        public static IEnumerable<T> Each<T>(this IEnumerable<T> values, Action<T> eachAction)
        {
            foreach (var item in values)
            {
                eachAction(item);
            }

            return values;
        }

        [DebuggerStepThrough]
        public static IEnumerable Each(this IEnumerable values, Action<object> eachAction)
        {
            foreach (var item in values)
            {
                eachAction(item);
            }

            return values;
        }

        public static IList<T> AddMany<T>(this IList<T> list, params T[] items)
        {
            return list.AddRange(items);
        }

        public static IList<T> AddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            items.Each(list.Add);

            return list;
        }

        public static T[] RemoveAt<T>(this T[] array, int index)
        {
            var rtn = new T[array.Length - 1];

            if (index > 0)
                Array.Copy(array, 0, rtn, 0, index);

            if (index < array.Length - 1)
                Array.Copy(array, index + 1, rtn, index, array.Length - index - 1);

            return rtn;
        }

        public static List<ListItem> ToListItems<T>(this IEnumerable<T> enumerable, Func<T, int> getValue, Func<T, string> getText)
        {
            return enumerable
                .Select(x => new ListItem() { Id = getValue(x), Name = getText(x) })
                .ToList();
        }

        public static List<SelectListItem> ToSelectList<T>(this IEnumerable<T> enumerable, Func<T, string> getValue, Func<T, string> getText)
        {
            return enumerable
                .Select(x => new SelectListItem { Value = getValue(x), Text = getText(x)})
                .ToList();
        }
    }
}