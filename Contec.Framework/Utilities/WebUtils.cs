using System;
using System.Net;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

using Contec.Framework.Logging;
using Contec.Framework.Strings;
using Contec.Framework.Extensions;

namespace Contec.Framework.Utilities
{
    public class WebUtils : IWebUtils
    {
        private readonly ILogService _logService;
        private readonly HttpContextBase _context;
        private readonly HttpRequestBase _request;

        public WebUtils(HttpContextBase context, ILogService logService)
        {
            _context = context;
            _request = _context.Request;
            _logService = logService;
        }

        public string Hostname(string requestUri)
        {
            var hostName = string.Empty;
            try
            {
                var uri = new Uri(requestUri.Trim());
                hostName = requestUri.IsEqualTo(Constants.LocalHostname) ? Dns.GetHostName() : uri.Authority.ToLower();
            }
            catch (Exception ex)
            {
                _logService.Warn(ex, ex.Message);
            }
            return hostName;
        }

        public T QueryString<T>(string key, bool validateInput = true)
        {
            return RequestValue<T>(_request.QueryString, key, validateInput);
        }

        public T PostValue<T>(string key)
        {
            return RequestValue<T>(_request.Form, key, false);
        }

        public T HeaderValue<T>(string key)
        {
            return RequestValue<T>(_request.Headers, key, false);
        }

        public bool IsValidUrl(string url)
        {
            var isValid = false;

            if (url.ToLower().StartsWith("www."))
            {
                url = "http://" + url;
            }

            try
            {
                var request = WebRequest.CreateHttp(url);
                request.ServerCertificateValidationCallback += delegate { return true; };
                request.GetResponse();
                isValid = true;
            }
            catch (Exception ex)
            {
                _logService.Warn(ex, ex.Message);
            }

            return isValid;
        }

        private static T RequestValue<T>(NameValueCollection collection, string key, bool validateInput = true)
        {
            var value = default(T);

            if (collection[key].IsNotEmpty())
            {
                var tempValue = collection[key].Trim();

                if (typeof(T).IsAssignableFrom(typeof(List<Guid>)))
                {
                    if (tempValue.Contains(","))
                    {
                        var guids = (from val in tempValue.Split(',').ToList() select ParseGuid<Guid>(val)).ToList<Guid>();
                        value = (T) Convert.ChangeType(guids, typeof (T));
                    }
                    else
                    {
                        var guids = new List<Guid> {ParseGuid<Guid>(tempValue)};

                        value = (T) Convert.ChangeType(guids, typeof (T));
                    }
                }
                else if (typeof (T).IsAssignableFrom(typeof (Guid)))
                {
                    value = ParseGuid<T>(tempValue);
                }
                else if (typeof (T).IsAssignableFrom(typeof(int)))
                {
                    if (Regex.IsMatch(tempValue, @"^\d+$"))
                    {
                        int tempInt;

                        if (int.TryParse(tempValue, out tempInt))
                        {
                            value = (T) Convert.ChangeType(tempInt, typeof (T));
                        }
                    }
                }
                else if (typeof (T).IsAssignableFrom(typeof (bool)))
                {
                    value = (T) Convert.ChangeType("true,1".Includes(tempValue), typeof (T));
                }
                else
                {
                    //if (!validateInput || Regex.IsMatch(tempValue, Installation.Application.ValidateSearchInputRegEx))
                    //{
                        value = (T) Convert.ChangeType(tempValue, typeof (T));
                    //}
                }
            }

            return value;
        }

        private static T ParseGuid<T>(string value)
        {
            T parsed;

            //if (GuidHelper.isValidGuid(value))
            //{
            //    parsed = (T) ((Object) new Guid(value));
            //}
            //else if (GuidHelper.isValidShortGuid(value))
            //{
            //    parsed = (T) ((Object) new ShortGuid(value).Guid);
            //}
            //else
            //{
            //    parsed = (T) ((Object) Guid.Empty);
            //}

            parsed = (T)((object)Guid.Empty);

            return parsed;
        }
    }
}