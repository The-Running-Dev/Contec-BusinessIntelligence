using System;
using System.Data;
using System.Linq;

using Dapper;
using Should.Fluent;
using NUnit.Framework;

using Contec.Data.Models;
using Contec.Bootstrapper;
using Contec.Data.Connections;
using Contec.Data.Repositories;

using BI.Web.Tests.Data;
using BI.Web.Tests.Extensions;

namespace BI.Web.Tests.Repositories
{
    [TestFixture]
    public class BIReportsToSitesRepositoryTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            var connectionFactory = IocWrapper.Instance.GetService<IBusinessIntelligenceConnection>();

            _repository = IocWrapper.Instance.GetService<IBIReportsToSitesRepository>();
            _connection = ((MySqlConnectionFactory)connectionFactory).Create();

            BIReportsToSites.Data.ForEach(item =>
            {
                _connection.Execute("insert into BIReportsToSites(Id, ReportId, SiteId, CreatedBy, CreatedOn) Values(@Id, @ReportId, @SiteId, @CreatedBy, @CreatedOn)", item);
            });
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            _connection.Execute("delete from BIReportsToSites where CreatedBy = 'TestUser'");
            _connection.Close();
        }

        [Test]
        public void Should_Get_All()
        {
            var dbResult = _repository.GetAll();

            dbResult.Should().Not.Be.Null();
            dbResult.Result.Should().Not.Be.Empty();
        }

        [Test]
        public void Should_Get_By_Id()
        {
            var sample = BIReportsToSites.Data.FirstOrDefault();
            var dbResult = _repository.GetById(sample.Id);

            Verify(dbResult.Result);
        }

        [Test]
        public void Should_Create()
        {
            var sampleData = BIReportsToSites.New(Guid.NewGuid());

            _repository.Create(sampleData);

            Verify(sampleData);
        }

        [Test]
        public void Should_Update()
        {
            var sampleData = BIReportsToSites.Data.FirstOrDefault();

            sampleData.ReportId = Guid.Empty;
            sampleData.SiteId = 1000;

            _repository.Update(sampleData);

            Verify(sampleData);
        }

        [Test]
        public void Should_Delete()
        {
            var sampleData = BIReportsToSites.Data.FirstOrDefault();
            BIReportsToSites.Data.Remove(sampleData);

            _repository.Delete(sampleData.Id);

            var dbResult = GetById(sampleData.Id);

            dbResult.Should().Be.Null();
        }

        private void Verify(BIReportToSite sampleData)
        {
            var dbResult = GetById(sampleData.Id);

            dbResult.Should().Not.Be.Null();
            dbResult.IsSameAs(sampleData);
        }

        private BIReportToSite GetById(Guid id)
        {
            return _connection
                .Query<BIReportToSite>("select Id, ReportId, SiteId, CreatedBy, CreatedOn from BIReportsToSites where Id = @Id", new { id })
                .SingleOrDefault();
        }

        private IBIReportsToSitesRepository _repository;
        private IDbConnection _connection;
    }
}