using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TevenStudiosBudgetTracker.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTrackerTests
{
    [TestClass]
    class StatusCodeTests
    {
        [TestMethod]
        public void CorrectStatusCodeTest()
        {
            var controller = new StatusCodeController();
            var result = controller.Index(404) as ViewResult;
            Assert.AreEqual(404, result.ViewData);

        }
    }
}
