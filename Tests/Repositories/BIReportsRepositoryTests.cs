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
    public class BIReportsRepositoryTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            var connectionFactory = IocWrapper.Instance.GetService<IBusinessIntelligenceConnection>();

            _repository = IocWrapper.Instance.GetService<IBIReportsRepository>();
            _connection = ((MySqlConnectionFactory)connectionFactory).Create();

            BIReports.Data.ForEach(item =>
            {
                _connection.Execute("insert into BIReports(Id, Name, Description, EmbedSource, CreatedBy, CreatedOn) Values(@Id, @Name, @Description, @EmbedSource, @CreatedBy, @CreatedOn)",
                    item);
            });
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            _connection.Execute("delete from BIReports where CreatedBy = 'TestUser'");
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
            var sample = BIReports.Data.FirstOrDefault();
            var dbResult = _repository.GetById(sample.Id);

            Verify(dbResult.Result);
        }

        [Test]
        public void Should_Create()
        {
            var sampleData = BIReports.New(Guid.NewGuid());

            _repository.Create(sampleData);

            Verify(sampleData);
        }

        [Test]
        public void Should_Update()
        {
            var sampleData = BIReports.Data.FirstOrDefault();

            sampleData.Name = "New Name";
            sampleData.Description = "New Description";

            _repository.Update(sampleData);

            Verify(sampleData);
        }

        [Test]
        public void Should_Delete()
        {
            var sampleData = BIReports.Data.FirstOrDefault();
            BIReports.Data.Remove(sampleData);

            _repository.Delete(sampleData.Id);

            var dbResult = GetById(sampleData.Id);

            dbResult.Should().Be.Null();
        }

        private void Verify(BIReport sampleData)
        {
            var dbResult = GetById(sampleData.Id);

            dbResult.Should().Not.Be.Null();
            dbResult.IsSameAs(sampleData);
        }

        private BIReport GetById(Guid id)
        {
            return _connection
                .Query<BIReport>("select Id, Name, Description, EmbedSource, CreatedBy, CreatedOn from BiReports where Id = @Id", new { id })
                .SingleOrDefault();
        }

        private IBIReportsRepository _repository;
        private IDbConnection _connection;
    }
}