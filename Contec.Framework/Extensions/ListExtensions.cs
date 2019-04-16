using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

using Contec.Framework.Models;

namespace Contec.Framework.Extensions
{
    public static class ListExtensions
    {
        public static string AsString(this List<string> stringList, string formatString = "{0}{1}{2}", string delimiter = ",")
        {
            var listAsString = string.Empty;

            if (stringList.Any())
            {
                listAsString = stringList.Aggregate((x, y) => string.Format(formatString, x, delimiter, y));
            }

            return listAsString;
        }

        public static string AsString(this List<Guid> guidList, string formatString = "{0}{1}{2}", string delimiter = ",")
        {
            var listAsString = string.Empty;

            if (guidList.Count == 1)
            {
                formatString = formatString.Replace("{2}", string.Empty);
                listAsString = string.Format(formatString, guidList.First(), delimiter);
            }
            else if (guidList.Count > 1)
            {
                listAsString = guidList.Select(x => x.ToString()).Aggregate((x, y) => string.Format(formatString, x, delimiter, y));
            }

            return listAsString;
        }

        public static List<string> ToStringList(this List<Guid> guidList)
        {
            var listOfStrings = new List<string>();

            listOfStrings.AddRange(guidList.Select(x => x.ToString()));

            return listOfStrings;
        }

        public static string ValuesAsString(List<KeyValuePair<Guid, string>> keyValueList)
        {
            return
                keyValueList.Aggregate(string.Empty,
                                       ((intialString, pair) => string.Format("{0}, {1}", intialString, pair.Value)))
                            .RegExReplace("^,\\s", string.Empty);
        }

        public static string NamesAsString(this List<IdNamePair<Guid>> keyValueList)
        {
            return
                keyValueList.Aggregate(string.Empty,
                                       ((intialString, pair) => string.Format("{0}, {1}", intialString, pair.Name)))
                            .RegExReplace("^,\\s", string.Empty);
        }

        public static string AsDisplayString(this List<string> stringList)
        {
            return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(stringList.AsString("{0}{1}{2}", ", "));
        }
    }
}