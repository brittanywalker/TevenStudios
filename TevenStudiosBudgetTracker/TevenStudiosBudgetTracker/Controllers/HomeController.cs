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
            // Build user model
            User umodel = new User();
            umodel.Name = HttpContext.Request.Form["name"].ToString();
            umodel.Email = HttpContext.Request.Form["email"].ToString();
            umodel.ManagerId = Int32.Parse(HttpContext.Request.Form["manager"].ToString());
            umodel.RoleId = Int32.Parse(HttpContext.Request.Form["role"].ToString());
            umodel.StartBudget = Int32.Parse(HttpContext.Request.Form["budget"].ToString());


            // Get context
            UserContext context = HttpContext.RequestServices.GetService(typeof(TevenStudiosBudgetTracker.Models.UserContext)) as UserContext;

            //Save user to database, get result
            int result = context.SaveUserDetails(umodel);
            if (result > 0)
            {
                ViewBag.Result = umodel.Name + " was successfully added";
            }
            else
            {
                ViewBag.Result = "Something went wrong";
            }
            // Not sure if this is correct, but need to reload data some how
            // Maybe have this as a method as might be used multiple times
            AdminViewData data = new AdminViewData();
            data.Users = context.GetAllUsers();
            data.Managers = context.GetAllManagers();

            return View("Index", data);
        }
		
		public IActionResult DeleteUser(int UserID)
        {
            UserContext context = HttpContext.RequestServices.GetService(typeof(TevenStudiosBudgetTracker.Models.UserContext)) as UserContext;
            int result = context.DeleteUserSQL(UserID);

            AdminViewData data = new AdminViewData();
            data.Users = context.GetAllUsers();
            data.Managers = context.GetAllManagers();
            return View("Index",data);
        }
    }
}
