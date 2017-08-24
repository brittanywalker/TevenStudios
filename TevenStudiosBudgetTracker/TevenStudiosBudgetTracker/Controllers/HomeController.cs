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
            return View();
        }

        public IActionResult Index()
        {
            ViewData["Message"] = "Employee page.";

            UserContext context = HttpContext.RequestServices.GetService(typeof(TevenStudiosBudgetTracker.Models.UserContext)) as UserContext;

            List<List<Models.User>> usersAndManagers = new List<List<Models.User>>();
            usersAndManagers.Add(context.GetAllUsers());
            usersAndManagers.Add(context.GetAllManagers());

            return View(usersAndManagers);
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
