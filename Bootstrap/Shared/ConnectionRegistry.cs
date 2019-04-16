using System.Web.Security;

using StructureMap;

using Contec.Data.Connections;

namespace Contec.Bootstrapper.Shared
{
    public class ConnectionRegistry : Registry
    {
        public ConnectionRegistry()
        {
            //For<IBusinessIntelligenceConnection>().Add<BusinessIntelligenceConnection>();
            //For<IWebAuthConnection>().Add<WebAuthConnection>();
            //For<ICTGConnection>().Add<CTGConnection>();
            For<MembershipProvider>().Use(Membership.Provider);
        }
    }
}