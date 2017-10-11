using Microsoft.VisualStudio.TestTools.UnitTesting;
using TevenStudiosBudgetTracker.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BudgetTrackerTests
{
    [TestClass]
    public class StatusCodeTests
    {
        [TestMethod]
        public void CorrectStatusCodeTest()
        {
            var controller = new StatusCodeController();
            var result = controller.Index(404) as ViewResult;
            var code = result.ViewData.Model;
            Assert.AreEqual(404, code);
        }
    }
}
