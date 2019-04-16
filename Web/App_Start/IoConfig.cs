using System.Web.Mvc;
using System.Web.Http;

using Contec.Bootstrapper;
using BI.Web.DependencyInjection;

namespace BI.Web
{
    public static class IoCConfig
    {
        public static void Initialize()
        {
            Bootstrapper.Bootstrap(BootstrapType.Web);

            DependencyResolver.SetResolver(new StructureMapResolver(IocWrapper.Instance.Container));
            GlobalConfiguration.Configuration.DependencyResolver = new StructureMapResolver(IocWrapper.Instance.Container);
        }
    }
}