using System.Web.Http.Routing;
using System.Collections.Generic;
using System.Web.Http.Controllers;

namespace API
{
    /// <summary>
    /// Providers route inheritance from the base controller
    /// </summary>
    public class CustomRouteProvider : DefaultDirectRouteProvider
    {
        protected override IReadOnlyList<IDirectRouteFactory>GetActionRouteFactories(HttpActionDescriptor actionDescriptor)
        {
            return actionDescriptor.GetCustomAttributes<IDirectRouteFactory>(true);
        }
    }
}