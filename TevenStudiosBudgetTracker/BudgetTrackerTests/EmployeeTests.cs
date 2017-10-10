using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TevenStudiosBudgetTracker.Controllers;

namespace BudgetTrackerTests
{
    [TestClass]
    public class EmployeeTests
    {
        [TestMethod]
        public void TestViewName()
        {
            var controller = new EmployeeController();
            var result = controller.Index() as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
        }
    }
}
