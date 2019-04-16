using NUnit.Framework;
using Contec.Bootstrapper;

namespace BI.Web.Tests
{
    [SetUpFixture]
    public class Setup
    {
        [OneTimeSetUp]
        public void Boostrap()
        {
            Bootstrapper.Bootstrap(BootstrapType.Tests);
        }
    }
}