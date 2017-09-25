<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TevenStudiosBudgetTracker.Models;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Collections;
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

        //This action Harry
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

            // get and set the UI's budget
            double budget = transactionContext.getCurrentBudget(user.ID, user.ChangeAnnualBudgetDate, user.StartBudget, user.AnnualBudget, user.ChangeAnnualBudget);
            mymodel.Budget = budget;
			
			// max budget
			mymodel.MaxBudgetRequest = user.AnnualBudget + budget;

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

            return View(data);
        }

        public IActionResult Manager()
        {
            ViewData["Message"] = "Management page.";

            UserContext context = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;
            ManagerViewData data = new ManagerViewData();
            User user = context.retrieveUserDetails((int)HttpContext.Session.GetInt32(SessionKeyId));
            data.Employees = context.GetEmployeesForManager(user.ID);
            data.Manager = user;

            return View(data);
        }

        public IActionResult Error()
        {
            return View();
        }

        [HttpPost]		 
       public IActionResult GetDetails()   //Harry pls rename this to something more intuitive :P xoxo 
        {	            
             // Build user model		
             User umodel = new User();		
             umodel.Name = HttpContext.Request.Form["name"].ToString();		
             umodel.Email = HttpContext.Request.Form["email"].ToString();		
             umodel.ManagerId = Int32.Parse(HttpContext.Request.Form["manager"].ToString());		
             umodel.RoleId = Int32.Parse(HttpContext.Request.Form["role"].ToString());		
             umodel.StartBudget = Int32.Parse(HttpContext.Request.Form["budget"].ToString());
             umodel.AnnualBudget = Int32.Parse(HttpContext.Request.Form["annualBudget"].ToString());
 		
             // Get context		
             UserContext context = HttpContext.RequestServices.GetService(typeof(TevenStudiosBudgetTracker.Models.UserContext)) as UserContext;		
 		
             //Save user to database, get result		
             int result = context.SaveUserDetails(umodel);		
             if (result > 0)		
             {		
                 ViewBag.Result = umodel.Name + " was successfully added";		
             }		
             else {		
                 ViewBag.Result = "Something went wrong";		
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
            }
            else
            {
                ViewBag.Result = "Something went wrong";
            }

            AdminViewData data = new AdminViewData();
            data.Users = context.GetAllUsers();
            data.Managers = context.GetAllManagers();
            return View("Admin", data);
        }

        public IActionResult EditUser()
        {
            UserContext context = HttpContext.RequestServices.GetService(typeof(TevenStudiosBudgetTracker.Models.UserContext)) as UserContext;

            Console.WriteLine("edit manager: " + HttpContext.Request.Form["editManager"].ToString());
            //// Build user model
            User umodel = new User();
            try
            {
                umodel.ManagerId = Int32.Parse(HttpContext.Request.Form["editManager"].ToString());
            }
            catch (FormatException)
            {
                umodel.ManagerId = -1;
            }
            umodel.ID = Int32.Parse(HttpContext.Request.Form["editID"].ToString());
            umodel.Name = HttpContext.Request.Form["editName"].ToString();
            umodel.Email = HttpContext.Request.Form["editEmail"].ToString();
            umodel.RoleId = Int32.Parse(HttpContext.Request.Form["editRole"].ToString());
            umodel.StartBudget = Int32.Parse(HttpContext.Request.Form["editBudget"].ToString());
            umodel.AnnualBudget = Int32.Parse(HttpContext.Request.Form["editAnnualBudget"].ToString());

            int result = context.EditUserSQL(umodel);

            if (result > 0)
            {
                ViewBag.Result = umodel.Name + " was successfully edited";
            }
            else
            {
                ViewBag.Result = "Something went wrong";
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

            Console.WriteLine("user name: " + currentUser.Name);

            return Json(new {ID = currentUser.ID, Name = currentUser.Name, Email = currentUser.Email, ManagerId = currentUser.ManagerId, RoleId = currentUser.RoleId, StartBudget = currentUser.StartBudget, AnnualBudget = currentUser.AnnualBudget});
        }

        public ActionResult SetCurrentUserIndex(int UserIndex)
        {
            UserContext context = HttpContext.RequestServices.GetService(typeof(TevenStudiosBudgetTracker.Models.UserContext)) as UserContext;
            AdminViewData data = new AdminViewData();
            data.Users = context.GetAllUsers();
            data.Managers = context.GetAllManagers();
            data.CurrentUserIndex = UserIndex;
            Console.WriteLine("data user index set to: " + data.CurrentUserIndex);

            User umodel = new User();
            umodel.Name = data.Users[UserIndex].Name;
            umodel.Email = data.Users[UserIndex].Email;
            umodel.ManagerId = data.Users[UserIndex].ManagerId;
            umodel.RoleId = data.Users[UserIndex].RoleId;
            umodel.StartBudget = data.Users[UserIndex].StartBudget;
            umodel.AnnualBudget = data.Users[UserIndex].AnnualBudget;

            data.currentEditUser = umodel;

            return View("Admin", data);
        }

        [HttpPost]
        public IActionResult SubmitRequest()   //Harry pls rename this to something more intuitive :P xoxo 
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
            }
            else
            {
                ViewBag.Result = "Something went wrong";
            }

            ViewData["Message"] = "Employee page.";

            dynamic mymodel = new ExpandoObject();

            TransactionContext transactionContext = HttpContext.RequestServices.GetService(typeof(TransactionContext)) as TransactionContext;
            UserContext userContext = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;
            User user = userContext.retrieveUserDetails((int)HttpContext.Session.GetInt32(SessionKeyId));
            double budget = transactionContext.getCurrentBudget(user.ID, user.ChangeAnnualBudgetDate, user.StartBudget, user.AnnualBudget, user.ChangeAnnualBudget);
            mymodel.Budget = budget;

            PendingRequestsContext context = HttpContext.RequestServices.GetService(typeof(PendingRequestsContext)) as PendingRequestsContext;
            mymodel.PendingRequests = context.GetAllPendingRequests(user.ID);

            return View("Employee", mymodel);
        }

        public IActionResult GetSelectedInfo(int UserID)
        {
            ViewData["Message"] = "Management page.";

            //gets manager and employee info
            UserContext context = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;
            ManagerViewData data = new ManagerViewData();
            User user = context.GetUser(UserID);
            data.Employees = context.GetEmployeesForManager(user.ID);
            data.Manager = user;
            data.SelectedEmployee = context.GetUser(UserID);

            //gets employee's pending requests
            PendingRequestsContext Pendingcontext = HttpContext.RequestServices.GetService(typeof(PendingRequestsContext)) as PendingRequestsContext;
            data.PendingRequests = Pendingcontext.GetAllPendingRequests(UserID);

            //gets employee's past requests
            TransactionContext transactionContext = HttpContext.RequestServices.GetService(typeof(TransactionContext)) as TransactionContext;
            data.PastRequests = transactionContext.GetAllPastRequests(UserID);

            return View("Manager", data);
        }
    }
}
