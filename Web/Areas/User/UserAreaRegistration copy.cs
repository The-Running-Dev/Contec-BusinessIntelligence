using System.Web.Mvc;

namespace BI.Web.Areas.User
{
    public class UserAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "user";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.MapMvcAttributeRoutes();

            context.MapRoute(
                "userDefault",
                "user/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}