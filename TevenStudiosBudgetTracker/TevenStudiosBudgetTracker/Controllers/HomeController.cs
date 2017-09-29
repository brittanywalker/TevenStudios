﻿using System;
using Microsoft.AspNetCore.Mvc;
using TevenStudiosBudgetTracker.Models;
using System.Dynamic;
using Microsoft.AspNetCore.Http;

namespace TevenStudiosBudgetTracker.Controllers
{

    public class HomeController : Controller
    {
        //Set Session names
        const string SessionKeyId = "_ID";
        const string SessionKeyRoleId = "_RoleId";
        const string SessionKeyName = "_Name";
        const string SessionKeyEmail = "_Email";

        public IActionResult GoogleLogin(string userEmail)
        {
            UserContext context = HttpContext.RequestServices.GetService(typeof(TevenStudiosBudgetTracker.Models.UserContext)) as UserContext;
            User result = context.GetUserByEmail(userEmail);
            if (result.Name != null)
            {
                HttpContext.Session.SetInt32(SessionKeyId, result.ID);
                HttpContext.Session.SetInt32(SessionKeyRoleId, result.RoleId);
                HttpContext.Session.SetString(SessionKeyName, result.Name);
                HttpContext.Session.SetString(SessionKeyEmail, result.Email);
            }
            else
            {
                return this.Json(new { success = false, message = "Failed login, please try again" });
            }

            return RedirectToAction("LoginSuccessful");
        }

        public JsonResult LoginSuccessful()
        {
            string Name = HttpContext.Session.GetString(SessionKeyName);
            int Roleid = (int)HttpContext.Session.GetInt32(SessionKeyRoleId);

            if (Roleid == 0)
            {
                return this.Json(new { success = true, redirect = "Admin" });
            }
            else if (Roleid == 1)
            {
                return this.Json(new { success = true, redirect = "Employee" });
            }
            else if (Roleid == 2)
            {
                return this.Json(new { success = true, redirect = "Manager" });
            }
            return this.Json(new { success = true, redirect = "Index" });
        }

        public IActionResult Employee()
        {
            ViewData["Message"] = "Employee page.";

            dynamic mymodel = new ExpandoObject();

            // set user and transaction contexts
            TransactionContext transactionContext = HttpContext.RequestServices.GetService(typeof(TransactionContext)) as TransactionContext;
            UserContext userContext = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;

            // gets the current user's details
            User user = userContext.retrieveUserDetails((int)HttpContext.Session.GetInt32(SessionKeyId));
            mymodel.CurrentUser = user;

            // get and set the UI's budget
            double budget = transactionContext.getCurrentBudget(user.ID, user.ChangeAnnualBudgetDate, user.StartBudget, user.AnnualBudget, user.ChangeAnnualBudget);
            mymodel.Budget = budget;

            //get the user's max budget spend so they can't spend more than they currently have and will
            //accrue for the year
            var futureAccruedBudget = getUserMaxBudgetRequest(user);
            var maxBudget = futureAccruedBudget + budget;
            if (maxBudget > user.AnnualBudget)
            {
                maxBudget = user.AnnualBudget;
            }
            mymodel.MaxBudgetRequest = maxBudget;

            // pending request
            PendingRequestsContext context = HttpContext.RequestServices.GetService(typeof(PendingRequestsContext)) as PendingRequestsContext;
            mymodel.PendingRequests = context.GetAllPendingRequests(user.ID);

            // past requests
            mymodel.PastRequests = transactionContext.GetAllPastRequests((int)HttpContext.Session.GetInt32(SessionKeyId));

            return View(mymodel);
        }

        public IActionResult Index()
        {
            ViewData["Message"] = "Home page.";

            return View();
        }

        public IActionResult Admin()
        {
            ViewData["Message"] = "Admin page.";

            UserContext context = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;

            AdminViewData data = new AdminViewData();
            data.Users = context.GetAllUsers();
            data.Managers = context.GetAllManagers();
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                data.CurrentUser = context.retrieveUserDetails((int)HttpContext.Session.GetInt32(SessionKeyId));
            }

            return View(data);
        }

        public IActionResult Manager()
        {
            ViewData["Message"] = "Management page.";

            UserContext context = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;
            ManagerViewData data = new ManagerViewData();
            User user = context.retrieveUserDetails((int)HttpContext.Session.GetInt32(SessionKeyId));
            data.Employees = context.GetEmployeesForManager(user.ID);
            data.CurrentUser = user;

            return View(data);
        }

        public IActionResult Error()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetDetails()
        {
            // Build user model
            User umodel = new User
            {
                Name = HttpContext.Request.Form["name"].ToString(),
                Email = HttpContext.Request.Form["email"].ToString(),
                ManagerId = Int32.Parse(HttpContext.Request.Form["manager"].ToString()),
                RoleId = Int32.Parse(HttpContext.Request.Form["role"].ToString()),
                StartBudget = Int32.Parse(HttpContext.Request.Form["budget"].ToString()),
                AnnualBudget = Int32.Parse(HttpContext.Request.Form["annualBudget"].ToString())
            };

            // Get context
            UserContext context = HttpContext.RequestServices.GetService(typeof(TevenStudiosBudgetTracker.Models.UserContext)) as UserContext;

            //Save user to database, get result
            int result = context.SaveUserDetails(umodel);
            if (result > 0)
            {
                ViewBag.Result = umodel.Name + " was successfully added";
                ViewBag.isSuccess = true;
            }
            else
            {
                ViewBag.Result = "Something went wrong, please try again.";
                ViewBag.isSuccess = false;
            }

            // Not sure if this is correct, but need to reload data some how
            // Maybe have this as a method as might be used multiple times
            AdminViewData data = new AdminViewData();
            data.Users = context.GetAllUsers();
            data.Managers = context.GetAllManagers();

            return View("Admin", data);
        }

        public IActionResult DeleteUser(int UserID)
        {
            UserContext context = HttpContext.RequestServices.GetService(typeof(TevenStudiosBudgetTracker.Models.UserContext)) as UserContext;
            int result = context.DeleteUserSQL(UserID);
            if (result > 0)
            {
                ViewBag.Result = "Successfully deleted";
                ViewBag.isSuccess = true;
            }
            else
            {
                ViewBag.Result = "Something went wrong, please try again.";
                ViewBag.isSuccess = false;
            }

            AdminViewData data = new AdminViewData();
            data.Users = context.GetAllUsers();
            data.Managers = context.GetAllManagers();
            return View("Admin", data);
        }

        public IActionResult EditUser()
        {
            UserContext context = HttpContext.RequestServices.GetService(typeof(TevenStudiosBudgetTracker.Models.UserContext)) as UserContext;

            // Build user model
            User umodel = new User
            {
                ID = Int32.Parse(HttpContext.Request.Form["editID"].ToString()),
                Name = HttpContext.Request.Form["editName"].ToString(),
                Email = HttpContext.Request.Form["editEmail"].ToString(),
                RoleId = Int32.Parse(HttpContext.Request.Form["editRole"].ToString()),
                StartBudget = Int32.Parse(HttpContext.Request.Form["editBudget"].ToString()),
                AnnualBudget = Int32.Parse(HttpContext.Request.Form["editAnnualBudget"].ToString())
            };
            try
            {
                umodel.ManagerId = Int32.Parse(HttpContext.Request.Form["editManager"].ToString());
            }
            catch (FormatException)
            {
                umodel.ManagerId = -1;
            }

            int result = context.EditUserSQL(umodel);
            if (result > 0)
            {
                ViewBag.Result = umodel.Name + " was successfully edited";
                ViewBag.isSuccess = true;
            }
            else
            {
                ViewBag.Result = "Something went wrong, please try again.";
                ViewBag.isSuccess = false;
            }

            AdminViewData data = new AdminViewData();
            data.Users = context.GetAllUsers();
            data.Managers = context.GetAllManagers();

            return View("Admin", data);
        }

        public IActionResult GetCurrentUserData(int UserID)
        {
            UserContext context = HttpContext.RequestServices.GetService(typeof(TevenStudiosBudgetTracker.Models.UserContext)) as UserContext;
            User currentUser = context.retrieveUserDetails(UserID);

            return Json(new { ID = currentUser.ID, Name = currentUser.Name, Email = currentUser.Email, ManagerId = currentUser.ManagerId, RoleId = currentUser.RoleId, StartBudget = currentUser.StartBudget, AnnualBudget = currentUser.AnnualBudget });
        }

        public ActionResult SetCurrentUserIndex(int UserIndex)
        {
            UserContext context = HttpContext.RequestServices.GetService(typeof(TevenStudiosBudgetTracker.Models.UserContext)) as UserContext;
            AdminViewData data = new AdminViewData();
            data.Users = context.GetAllUsers();
            data.Managers = context.GetAllManagers();
            data.CurrentUserIndex = UserIndex;

            User umodel = new User
            {
                Name = data.Users[UserIndex].Name,
                Email = data.Users[UserIndex].Email,
                ManagerId = data.Users[UserIndex].ManagerId,
                RoleId = data.Users[UserIndex].RoleId,
                StartBudget = data.Users[UserIndex].StartBudget,
                AnnualBudget = data.Users[UserIndex].AnnualBudget
            };

            data.currentEditUser = umodel;

            return View("Admin", data);
        }

        [HttpPost]
        public IActionResult SubmitRequest()
        {
            // Build user model		
            PendingRequest newRequest = new PendingRequest();
            newRequest.Description = HttpContext.Request.Form["description"].ToString();
            newRequest.Cost = HttpContext.Request.Form["requestCost"].ToString();
            DateTime dateTimeNow = DateTime.Now;
            newRequest.Date = dateTimeNow.ToString("yyyy-MM-dd HH:mm:ss");

            // Get context		
            PendingRequestsContext pendingRequestContext = HttpContext.RequestServices.GetService(typeof(TevenStudiosBudgetTracker.Models.PendingRequestsContext)) as PendingRequestsContext;

            //Save user to database, get result		
            int result = pendingRequestContext.SubmitPendingRequest(newRequest, (int)HttpContext.Session.GetInt32(SessionKeyId));
            if (result > 0)
            {
                ViewBag.Result = " Request was successfully submitted";
                ViewBag.isSuccess = true;
            }
            else
            {
                ViewBag.Result = "Something went wrong";
                ViewBag.isSuccess = false;
            }

            ViewData["Message"] = "Employee page.";

            dynamic mymodel = new ExpandoObject();

            // set user and transaction contexts
            TransactionContext transactionContext = HttpContext.RequestServices.GetService(typeof(TransactionContext)) as TransactionContext;
            UserContext userContext = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;

            // gets the current user's details
            User user = userContext.retrieveUserDetails((int)HttpContext.Session.GetInt32(SessionKeyId));
            mymodel.CurrentUser = user;

            // get and set the UI's budget
            double budget = transactionContext.getCurrentBudget(user.ID, user.ChangeAnnualBudgetDate, user.StartBudget, user.AnnualBudget, user.ChangeAnnualBudget);
            mymodel.Budget = budget;

            //get the user's max budget spend so they can't spend more than they currently have and will
            //accrue for the year
            var futureAccruedBudget = getUserMaxBudgetRequest(user);
            var maxBudget = futureAccruedBudget + budget;
            if (maxBudget > user.AnnualBudget)
            {
                maxBudget = user.AnnualBudget;
            }
            mymodel.MaxBudgetRequest = maxBudget;

            // pending request
            PendingRequestsContext context = HttpContext.RequestServices.GetService(typeof(PendingRequestsContext)) as PendingRequestsContext;
            mymodel.PendingRequests = context.GetAllPendingRequests(user.ID);

            // past requests
            mymodel.PastRequests = transactionContext.GetAllPastRequests((int)HttpContext.Session.GetInt32(SessionKeyId));

            return View("Employee", mymodel);
        }

        // This function gets information about a selected user to be displayed on the right hand side of the manager screen
        public IActionResult GetSelectedInfo(int UserID)
        {
            ViewData["Message"] = "Management page.";

            // gets selected employee
            UserContext context = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;
            User selectedEmployee = context.retrieveUserDetails(UserID);

            // gets employee's pending requests
            PendingRequestsContext Pendingcontext = HttpContext.RequestServices.GetService(typeof(PendingRequestsContext)) as PendingRequestsContext;
            var pendingRequests = Pendingcontext.GetAllPendingRequests(UserID);

            // gets employee's past requests
            TransactionContext transactionContext = HttpContext.RequestServices.GetService(typeof(TransactionContext)) as TransactionContext;
            var pastRequests = transactionContext.GetAllPastRequests(UserID);

            // gets the employees current budget 
            double budget = transactionContext.getCurrentBudget(selectedEmployee.ID, selectedEmployee.ChangeAnnualBudgetDate, selectedEmployee.StartBudget, selectedEmployee.AnnualBudget, selectedEmployee.ChangeAnnualBudget);

            return Json(new { id = UserID, selectedEmployee = selectedEmployee, currentBudget = budget, pendingRequests = pendingRequests, pastRequests = pastRequests });
        }

        private double getUserMaxBudgetRequest(User user)
        {
            DateTime today = DateTime.Today;
            Console.WriteLine("today: " + today);
            String year = today.ToString("yyyy");
            String date = user.StartDate.ToString("dd/MM");
            String time = user.StartDate.ToString("HH:mm:ss tt");
            String budgetChange = date + "/" + year + " " + time;
            DateTime budgetChangeDate = Convert.ToDateTime(budgetChange);
            int daysDifference = (int)(budgetChangeDate - today).TotalDays;
            Console.WriteLine("days different: " + daysDifference);
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
            return futureAccruedBudget;
        }
    }
}