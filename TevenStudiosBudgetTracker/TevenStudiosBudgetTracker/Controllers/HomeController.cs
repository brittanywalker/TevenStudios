using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TevenStudiosBudgetTracker.Models;

namespace TevenStudiosBudgetTracker.Controllers
{
    public class HomeController : Controller
    {
        public int CurrentUserID = 1;

        public IActionResult Employee()
        {
            ViewData["Message"] = "Employee page.";

            TransactionContext context = HttpContext.RequestServices.GetService(typeof(TransactionContext)) as TransactionContext;

            // return View(context.getCurrentBudget());
            PendingRequestsContext context = HttpContext.RequestServices.GetService(typeof(TevenStudiosBudgetTracker.Models.PendingRequestsContext)) as PendingRequestsContext;
            // TODO: Use the current user's actual ID number here
            return View(context.GetAllPendingRequests(CurrentUserID));
        }

        public IActionResult Index()
        {
            ViewData["Message"] = "Employee page.";

            UserContext context = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;

            return View(context.GetAllUsers());
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
    }
}
