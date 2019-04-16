using System.Linq;
using System.Collections.Generic;
using BI.Web.Tests.Extensions;
using Dapper;
using NUnit.Framework;

using Contec.Data.Models;
using Contec.Bootstrapper;
using Contec.Data.Connections;
using Contec.Data.Repositories;
using Should;
using Should.Fluent;

namespace BI.Web.Tests.Repositories
{
    [TestFixture]
    public class SitesRepositoryTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            var connectionFactory = IocWrapper.Instance.GetService<ICTGConnection>();

            _repository = IocWrapper.Instance.GetService<ISitesRepository>();

            using (var rconnection = ((MySqlConnectionFactory)connectionFactory).Create()) {
                _sites = rconnection.Query<Site>("select SiteId, SiteName, ParentId from WorkSites");
            }
        }

        [Test]
        public void Should_Get_All()
        {
            var dbResult = _repository.GetAll();

            Assert.IsNotNull(dbResult);
            Assert.IsTrue(dbResult.Result.Any());

            dbResult.Result.IsSameAs(_sites);
        }

        private ISitesRepository _repository;
        private IEnumerable<Site> _sites;
    }
}