using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Contec.Framework.Extensions
{
    public static class HtmlExtensions
    {
        /// <summary>
        /// Implementation for MVC
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static IHtmlString IncludeLocalizationScripts(this HtmlHelper htmlHelper)
        {
            string jqueryUiGlobalization = GetGlobalizationLink();
            return new HtmlString(htmlHelper.Script(jqueryUiGlobalization).ToHtmlString());
        }

        /// <summary>
        /// Implementation for ASP.NET forms
        /// </summary>
        /// <returns></returns>
        public static string IncludeLocalizationScripts()
        {
            string jqueryUiGlobalization = GetGlobalizationLink();
            return Script(null, jqueryUiGlobalization).ToHtmlString();
        }

        private static string GetGlobalizationLink()
        {
            return string.Format("~/js/jquery-ui-i18n/jquery.ui.datepicker-{0}.js", Thread.CurrentThread.CurrentCulture.Name).ResolveUrl();
        }
        /// <summary>
        /// Renders a script tag referencing the javascript file. 
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="scriptUrl">The script file.</param>
        /// <returns></returns>
        public static IHtmlString Script(this HtmlHelper html, string scriptUrl)
        {
            return new HtmlString(string.Format("<script type=\"text/javascript\" src=\"{0}\" ></script>\n", scriptUrl));
        }
        /// <summary>
        /// Renders a link tag referencing the stylesheet.  
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="cssUrl">The CSS file.</param>
        /// <returns></returns>
        public static IHtmlString Stylesheet(this HtmlHelper html, string cssUrl)
        {
            return new HtmlString(string.Format("<link type=\"text/css\" rel=\"stylesheet\" href=\"{0}\" />\n", cssUrl));
        }
        /// <summary>
        /// Renders a link tag referencing the stylesheet.  
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="cssUrl">The CSS file.</param>
        /// <param name="media">The media.</param>
        /// <returns></returns>
        public static IHtmlString Stylesheet(this HtmlHelper html, string cssUrl, string media)
        {
            return new HtmlString(string.Format("<link type=\"text/css\" rel=\"stylesheet\" href=\"{0}\" media=\"{1}\" />\n", cssUrl, media));
        }
    }
}
