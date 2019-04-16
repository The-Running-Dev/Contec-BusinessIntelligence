using System.Web.Mvc;
using BI.Web.Models;

namespace BI.Web.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewData["Header"] = new Header()
            {
                UserLogin = Request.IsAuthenticated ? User.Identity.Name : string.Empty,
                LoginModel = new Login()
                //Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()
            }; ;

            base.OnActionExecuting(filterContext);
        }
    }
}