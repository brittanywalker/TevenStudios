using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TevenStudiosBudgetTracker.Models;
using System.Dynamic;
using Microsoft.AspNetCore.Http;

namespace TevenStudiosBudgetTracker.Controllers
{
    public class AdminController : Controller
    {
        //Set Session names
        const string SessionKeyId = "_ID";
        const string SessionKeyRoleId = "_RoleId";
        const string SessionKeyName = "_Name";
        const string SessionKeyEmail = "_Email";

        public IActionResult Index()
        {
            ViewData["Message"] = "Admin page.";

            //Check if User is logged in, if not, make the url forbidden. This is useful if they attempt to type in the URL.
            if (HttpContext.Session.GetInt32(SessionKeyRoleId) == null)
            {
                return StatusCode(403);
            }

            //Check if they are an admin, if not, send them to the forbidden page.
            int Roleid = (int)HttpContext.Session.GetInt32(SessionKeyRoleId);

            if (Roleid != 0)
            {
                return StatusCode(403);
            }

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

        //Get the details of a user from form and create new user.
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

        //Edit user gets the details of the selected user.
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

        //Delete user from database given their ID.
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

        //Get User data given their ID, return in Json object.
        public IActionResult GetCurrentUserData(int UserID)
        {
            UserContext context = HttpContext.RequestServices.GetService(typeof(TevenStudiosBudgetTracker.Models.UserContext)) as UserContext;
            User currentUser = context.retrieveUserDetails(UserID);

            return Json(new { ID = currentUser.ID, Name = currentUser.Name, Email = currentUser.Email, ManagerId = currentUser.ManagerId, RoleId = currentUser.RoleId, StartBudget = currentUser.StartBudget, AnnualBudget = currentUser.AnnualBudget });
        }

    }
}