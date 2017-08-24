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
        public IActionResult Employee()
        {
            ViewData["Message"] = "Employee page.";

            TransactionContext context = HttpContext.RequestServices.GetService(typeof(TransactionContext)) as TransactionContext;

            return View(context.getCurrentBudget());
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
