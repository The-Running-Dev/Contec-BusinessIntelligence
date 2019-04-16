using System.Web.Security;

using StructureMap;
using StructureMap.Graph;
using StructureMap.Graph.Scanning;

using Contec.Data.Connections;

namespace Contec.Bootstrapper.Shared
{
    public class DataRegistry : Registry
    {
        public DataRegistry()
        {
            Scan(scan =>
            {
                // Scan all the assemblies
                scan.AssembliesFromApplicationBaseDirectory();

                // The default convention is IClassName being implemented by ClassName
                scan.WithDefaultConventions();
            });

            For<IWebAuthConnection>().Add<WebAuthConnection>().Ctor<string>().Is("WebAuthConnection");
            For<ICTGConnection>().Add<CTGConnection>().Ctor<string>().Is("CTGConnection");
            For<MembershipProvider>().Use(Membership.Provider);
        }
    }

    public class DataAssemblyScanner : DefaultConventionScanner
    {
        public override void ScanTypes(TypeSet type, Registry registry)
        {
            base.ScanTypes(type, registry);
        }
    }
}