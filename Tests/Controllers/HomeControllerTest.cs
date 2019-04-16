//using System.Web.Mvc;

//using NUnit.Framework;

//using BI.Web.Controllers;
//using Contec.Bootstrapper;

//namespace BI.Web.Tests.Controllers
//{
//    [TestFixture]
//    public class HomeControllerTest
//    {
//        [Test]
//        public void Index()
//        {
//            var controller = (UserController)IocWrapper.Instance.GetService(typeof(UserController));
//            var result = controller.Index() as ViewResult;

//            Assert.IsNotNull(result);
//            Assert.AreEqual("Home Page", result.ViewBag.Title);
//        }
//    }
//}
