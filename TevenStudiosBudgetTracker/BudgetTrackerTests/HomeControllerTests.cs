using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TevenStudiosBudgetTracker.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTrackerTests
{
    [TestClass]
    class HomeControllerTests
    {
        [TestMethod]
        public void TestIndexView()
        {
            var controller = new HomeController();
            var result = controller.Index() as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void TestErrorView()
        {
            var controller = new HomeController();
            var result = controller.Error() as ViewResult;
            Assert.AreEqual("Error", result.ViewName);
        }

    }
}
