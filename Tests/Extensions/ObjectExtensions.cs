using NUnit.Framework;
using KellermanSoftware.CompareNetObjects;

namespace BI.Web.Tests.Extensions
{
    public static class ObjectExtensions
    {
        public static void IsSameAs(this object firstObj, object secondObj)
        {
            var compareLogic = new CompareLogic {Config = {MaxMillisecondsDateDifference = 1000}};

            Assert.IsTrue(compareLogic.Compare(firstObj, secondObj).AreEqual);
        }
    }
}