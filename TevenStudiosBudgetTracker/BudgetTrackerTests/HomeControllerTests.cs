using Microsoft.VisualStudio.TestTools.UnitTesting;
using TevenStudiosBudgetTracker.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTrackerTests
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void IndexViewTest()
        {
            var controller = new HomeController();
            var result = controller.Index() as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void ErrorViewTest()
        {
            var controller = new HomeController();
            var result = controller.Error() as ViewResult;
            Assert.AreEqual("Error", result.ViewName);
        }

    }
}
