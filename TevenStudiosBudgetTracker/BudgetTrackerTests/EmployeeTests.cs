using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TevenStudiosBudgetTracker.Controllers;
using TevenStudiosBudgetTracker.Models;

namespace BudgetTrackerTests
{
    [TestClass]
    public class EmployeeTests
    {
        [TestMethod]
        public void UserTest()
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

            user.Name = "Christina";
            Assert.AreEqual("Christina", user.Name);
            Assert.AreEqual(1, user.ManagerId);
            Assert.AreEqual(1000, user.AnnualBudget);
        }
        

        [TestMethod]
        public void PendingRequestTest()
        {
            var pendingRequest = new PendingRequest {
                Date = "11/10/2017",
                Cost = "200",
                ID = "1",
                Description = "java book",
                UserID = 1,
                UserName = "Brittany",
                UserEmail = "brittmwalker23@gmail.com",
            };

            Assert.AreEqual("11/10/2017", pendingRequest.Date);
            Assert.AreEqual("Brittany", pendingRequest.UserName);
            Assert.AreEqual(1, pendingRequest.UserID);
        }

        [TestMethod]
        public void MaxUserBudgetRequestTest()
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

            var controller = new EmployeeController();
            var result = controller.getUserMaxBudgetRequest(user);

            DateTime today = DateTime.Today;
            String year = today.ToString("yyyy");
            String date = user.StartDate.ToString("dd/MM");
            String time = user.StartDate.ToString("HH:mm:ss tt");
            String budgetChange = date + "/" + year + " " + time;
            DateTime budgetChangeDate = Convert.ToDateTime(budgetChange);
            int daysDifference = (int)(budgetChangeDate - today).TotalDays;

            double futureAccruedBudget;
            if (daysDifference > 0)
            {
                futureAccruedBudget = daysDifference * (user.AnnualBudget / 365);
            }
            else
            {
                var nextYearBudgetDate = budgetChangeDate.AddYears(1);
                int daysToBudgetYear = (int)(nextYearBudgetDate - today).TotalDays;
                futureAccruedBudget = daysToBudgetYear * (user.AnnualBudget / 365);
            }

            Assert.AreEqual(futureAccruedBudget, result);
        }

        [TestMethod]
        public void RoleTypeTests()
        {
            var roletype = new RoleType
            {
                ID = 0,
                Type = "Administrator"
            };

            var roletype1 = new RoleType
            {
                ID = 1,
                Type = "Employee"
            };

            var roletype2 = new RoleType
            {
                ID = 2,
                Type = "Manager"
            };

            Assert.AreEqual(0, roletype.ID);
            Assert.AreEqual("Employee", roletype1.Type);
            Assert.AreNotEqual("Administrator", roletype2.Type);
        }
    }
}