using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TevenStudiosBudgetTracker.Models;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Collections;
using System.Dynamic;
using static TevenStudiosBudgetTracker.Models.UserContext;

namespace TevenStudiosBudgetTracker.Controllers
{

    public class HomeController : Controller
    {
        public int CurrentUserID = 1;

        public IActionResult Employee()
        {
            ViewData["Message"] = "Employee page.";

            dynamic mymodel = new ExpandoObject();

            TransactionContext transactionContext = HttpContext.RequestServices.GetService(typeof(TransactionContext)) as TransactionContext;
            UserContext userContext = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;
            User user = userContext.GetUser(CurrentUserID);
            double budget = transactionContext.getCurrentBudget(user.ID, user.StartDate, user.StartBudget, user.AnnualBudget);
            mymodel.Budget = budget;

            PendingRequestsContext context = HttpContext.RequestServices.GetService(typeof(PendingRequestsContext)) as PendingRequestsContext;
            mymodel.PendingRequests = context.GetAllPendingRequests(CurrentUserID);
            // TODO: Use the current user's actual ID number here
            return View(mymodel);
        }

        public IActionResult Index()
        {
            ViewData["Message"] = "Employee page.";

            UserContext context = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;

            AdminViewData data = new AdminViewData();
            data.Users = context.GetAllUsers();
            data.Managers = context.GetAllManagers();

            return View(data);
        }

        public IActionResult Manager()
        {
            ViewData["Message"] = "Management page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
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
            return View("Index", data);
        }

        public IActionResult EditUser(int UserID)
        {
            UserContext context = HttpContext.RequestServices.GetService(typeof(TevenStudiosBudgetTracker.Models.UserContext)) as UserContext;
            
            //// Build user model
            User umodel = new User();
            umodel.ID = UserID;
            umodel.Name = HttpContext.Request.Form["name"].ToString();
            umodel.Email = HttpContext.Request.Form["email"].ToString();
            umodel.ManagerId = Int32.Parse(HttpContext.Request.Form["manager"].ToString());
            umodel.RoleId = Int32.Parse(HttpContext.Request.Form["role"].ToString());
            umodel.StartBudget = Int32.Parse(HttpContext.Request.Form["budget"].ToString());

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

            return View("Index", data);
        }

        public IActionResult GetCurrentUserData(int UserID)
        {
            UserContext context = HttpContext.RequestServices.GetService(typeof(TevenStudiosBudgetTracker.Models.UserContext)) as UserContext;
            User currentUser = context.retrieveUserDetails(UserID);

            Console.WriteLine("user name: " + currentUser.Name);

            return Json(new {ID = currentUser.ID, Name = currentUser.Name, Email = currentUser.Email, ManagerId = currentUser.ManagerId, RoleId = currentUser.RoleId, StartBudget = currentUser.StartBudget});
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

            data.currentEditUser = umodel;

            return View("Index", data);
        }
    }
}
