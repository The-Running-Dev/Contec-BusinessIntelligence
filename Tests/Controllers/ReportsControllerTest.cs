using System.Collections.Generic;

using Should.Fluent;
using NUnit.Framework;

using BI.Web.Controllers;
using Contec.Data.Models;
using Contec.Data.Services;

namespace BI.Web.Tests.Controllers
{
    [TestFixture]
    public class ReportsControllerTest
    {
        [Test]
        public void Get()
        {
            //var controller = new ReportsController(new BIReportsService());
            //var reports = controller.GetAll() as IEnumerable<BIReport>;

            //Assert.IsNotNull(reports);
            //reports.Should().Not.Be.Empty();
        }

        //[Test]
        //public void GetById()
        //{
        //    var controller = new ReportsController();

        //    var result = controller.Get(5);

        //    Assert.AreEqual("value", result);
        //}

        //[Test]
        //public void Create()
        //{
        //    var controller = new ReportsController();

        //    controller.Create("value");
        //}

        //[Test]
        //public void Put()
        //{
        //    var controller = new ReportsController();

        //    controller.Put(5, "value");

        //    // Assert
        //}

        //[Test]
        //public void Delete()
        //{
        //    var controller = new ReportsController();

        //    controller.Delete(5);
        //}
    }
}