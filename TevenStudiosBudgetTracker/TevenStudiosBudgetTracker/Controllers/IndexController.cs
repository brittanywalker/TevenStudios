using System;
using Microsoft.AspNetCore.Mvc;
using TevenStudiosBudgetTracker.Models;
using System.Dynamic;
using Microsoft.AspNetCore.Http;

namespace TevenStudiosBudgetTracker.Controllers
{
    public class IndexController : Controller
    {
        //Set Session names
        const string SessionKeyId = "_ID";
        const string SessionKeyRoleId = "_RoleId";
        const string SessionKeyName = "_Name";
        const string SessionKeyEmail = "_Email";

        /**
            Function used to sign user in when using google authenticator

            @param userEmail users email passed from google authenticator
            @return JSON with success boolean and redirect location
        */
        public IActionResult GoogleLogin(string userEmail)
        {
            // Sets the users session
            UserContext context = HttpContext.RequestServices.GetService(typeof(TevenStudiosBudgetTracker.Models.UserContext)) as UserContext;
            User result = context.GetUserByEmail(userEmail);
            if (result.Name != null)
            {
                HttpContext.Session.SetInt32(SessionKeyId, result.ID);
                HttpContext.Session.SetInt32(SessionKeyRoleId, result.RoleId);
                HttpContext.Session.SetString(SessionKeyName, result.Name);
                HttpContext.Session.SetString(SessionKeyEmail, result.Email);
            }
            // If failed to login, then success message if false and provide message
            else
            {  
                return this.Json(new { success = false, message = "Failed login, please try again" });
            }

            return RedirectToAction("LoginSuccessful");
        }

        /**
            Function used to generate JSON containing success message and redirect location

            @return JSON with success boolean and redirect string
        */
        public JsonResult LoginSuccessful()
        {
            string Name = HttpContext.Session.GetString(SessionKeyName);
            int Roleid = (int)HttpContext.Session.GetInt32(SessionKeyRoleId);

            // Generates JSON to determine redirect page, based off of Roleid
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
    }
}