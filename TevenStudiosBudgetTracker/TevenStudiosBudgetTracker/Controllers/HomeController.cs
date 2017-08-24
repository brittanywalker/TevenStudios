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

        [HttpPost]
        public IActionResult GetDetails()
        {
            User umodel = new User();
            umodel.Name = HttpContext.Request.Form["name"].ToString();
            umodel.Email = HttpContext.Request.Form["email"].ToString();
            umodel.ManagerId = Int32.Parse(HttpContext.Request.Form["manager"].ToString());
            umodel.RoleId = Int32.Parse(HttpContext.Request.Form["role"].ToString());

            ViewBag.Result = "Good job, " + umodel.Name + "was added!";

            // Not sure if this is correct, but need to reload data some how
            // Maybe have this as a method as might be used multiple times
            UserContext context = HttpContext.RequestServices.GetService(typeof(TevenStudiosBudgetTracker.Models.UserContext)) as UserContext;
            AdminViewData data = new AdminViewData();
            data.Users = context.GetAllUsers();
            data.Managers = context.GetAllManagers();

            return View("Index", data);
        }
    }
}
