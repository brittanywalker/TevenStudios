using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TevenStudiosBudgetTracker.Controllers;
using TevenStudiosBudgetTracker.Models;

namespace BudgetTrackerTests
{
    [TestClass]
    public class EmployeeTests
    {
        User user = new User
        {
            Name = "Brittany",
            Email = "brittmwalker23@gmail.com",
            ManagerId = 1,
            RoleId = 0,
            StartBudget = 200,
            AnnualBudget = 1000,
            StartDate = Convert.ToDateTime("18/02/2017")

        };

        [TestMethod]
        public void MaxUserBudgetRequestTest()
        {            
            var controller = new EmployeeController();
            var result = controller.getUserMaxBudgetRequest(user);
            Assert.AreEqual(356.16, Math.Round(result, 2));
        }
    }
}