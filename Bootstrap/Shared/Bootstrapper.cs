using StructureMap;

using Framework;
using Contec.Bootstrapper.Shared;

namespace Contec.Bootstrapper
{
    public static class Bootstrapper
    {
        public static IIocWrapper Bootstrap(BootstrapType type)
        {
            var container = new Container();

            container.Configure(cfg => cfg.For<IIocWrapper>().Use(IocWrapper.Instance));

            switch (type)
            {
                case BootstrapType.Web:
                    ConfigureWebRegistries(container);

                    break;
                case BootstrapType.Api:
                    ConfigureApiRegistries(container);

                    break;
                case BootstrapType.Tests:
                    ConfigureTestRegistries(container);

                    break;
            }

            IocWrapper.Instance = new IocWrapper(container);

            return IocWrapper.Instance;
        }

        private static void ConfigureWebRegistries(IContainer container)
        {
            container.Configure(cfg =>
            {
                cfg.AddRegistry<ConnectionRegistry>();
                cfg.AddRegistry<DataRegistry>();
                cfg.AddRegistry<FrameworkRegistry>();
            });
        }

        private static void ConfigureApiRegistries(IContainer container)
        {
            container.Configure(cfg =>
            {
                cfg.AddRegistry<ConnectionRegistry>();
                cfg.AddRegistry<DataRegistry>();
                cfg.AddRegistry<FrameworkRegistry>();
            });
        }

        private static void ConfigureTestRegistries(IContainer container)
        {
            container.Configure(cfg =>
            {
                cfg.AddRegistry<ConnectionRegistry>();
                cfg.AddRegistry<DataRegistry>();
                cfg.AddRegistry<FrameworkRegistry>();
            });
        }
    }
}