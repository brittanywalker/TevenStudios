using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TevenStudiosBudgetTracker.Models;
using Microsoft.AspNetCore.Mvc.Routing;

namespace TevenStudiosBudgetTracker.Controllers
{

    public class HomeController : Controller
    {
        public int CurrentUserID = 1;

        public IActionResult Employee()
        {
            ViewData["Message"] = "Employee page.";

            TransactionContext transactionContext = HttpContext.RequestServices.GetService(typeof(TransactionContext)) as TransactionContext;
            UserContext userContext = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;
            User user = userContext.GetUser(CurrentUserID);
            double budget = transactionContext.getCurrentBudget(user.ID, user.StartDate, user.StartBudget, user.AnnualBudget);

            PendingRequestsContext pendingContext = HttpContext.RequestServices.GetService(typeof(TevenStudiosBudgetTracker.Models.PendingRequestsContext)) as PendingRequestsContext;
            // TODO: Use the current user's actual ID number here
            return View(pendingContext.GetAllPendingRequests(CurrentUserID));
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

        public ActionResult SetCurrentUserIndex(int UserIndex)
        {
            UserContext context = HttpContext.RequestServices.GetService(typeof(TevenStudiosBudgetTracker.Models.UserContext)) as UserContext;
            AdminViewData data = new AdminViewData();
            data.Users = context.GetAllUsers();
            data.Managers = context.GetAllManagers();
            data.CurrentUserIndex = UserIndex;
            Console.WriteLine("data user index set to: " + data.CurrentUserIndex);
            ViewBag.CurrentIndex = UserIndex;
            return View("Index", data);
            //var redirectUrl = new UrlHelper(Request.RequestContext).Action("Index", "Controller");
            //return Json(new { Url = redirectUrl });
        }
    }
}
