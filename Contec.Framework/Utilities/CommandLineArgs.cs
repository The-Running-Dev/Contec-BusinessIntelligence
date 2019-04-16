using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using Contec.Framework.Extensions;

namespace Contec.Framework.Utilities
{
    public class CommandLineArgs
    {
        private static readonly Regex NameValueRx = new Regex(@"^-{1,2}|^/|=|:", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex QuoteRx = new Regex(@"^['""]?(.*?)['""]?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private readonly StringDictionary _arguments = new StringDictionary();

        public CommandLineArgs(IEnumerable<string> args)
        {
            string argument = null;

            // Valid parameters forms:
            //      {-,/,--}param{ ,=,:}((",')value(",'))
            // Examples: 
            //      -param1 value1 --param2 /param3:"Test-:-work" 
            //      /param4=happy -param5 '--=nice=--'
            foreach (string txt in args)
            {
                // Look for new parameters (-,/ or --) and a possible enclosed value (=,:)
                var parts = NameValueRx.Split(txt, 3);

                switch (parts.Length)
                {
                    // Found a value (for the last parameter found (space separator))
                    case 1:
                        if (argument != null)
                        {
                            if (!_arguments.ContainsKey(argument))
                            {
                                parts[0] = QuoteRx.Replace(parts[0], "$1");

                                _arguments.Add(argument, parts[0]);
                            }
                            argument = null;
                        }
                        // else Error: no parameter waiting for a value (skipped)
                        break;

                    // Found just a parameter
                    case 2:
                        // The last parameter is still waiting.  With no value, set it to true.
                        if (argument != null)
                        {
                            if (!_arguments.ContainsKey(argument))
                                _arguments.Add(argument, "true");
                        }

                        argument = parts[1];
                        break;

                    // Parameter with enclosed value
                    case 3:
                        // The last parameter is still waiting. With no value, set it to true.
                        if (argument != null)
                        {
                            if (!_arguments.ContainsKey(argument))
                                _arguments.Add(argument, "true");
                        }

                        argument = parts[1];

                        // Remove possible enclosing characters (",')
                        if (!_arguments.ContainsKey(argument))
                        {
                            parts[2] = QuoteRx.Replace(parts[2], "$1");
                            _arguments.Add(argument, parts[2]);
                        }

                        argument = null;
                        break;
                }
            }

            // In case a parameter is still waiting
            if (argument != null)
            {
                if (!_arguments.ContainsKey(argument))
                    _arguments.Add(argument, "true");
            }
        }

        public virtual T Get<T>(string argName, T defaultValue = default(T))
        {
            if (!_arguments.ContainsKey(argName))
                return defaultValue;

            return _arguments[argName].To<T>(defaultValue);
        }
    }
}