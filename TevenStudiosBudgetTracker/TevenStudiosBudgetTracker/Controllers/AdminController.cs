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
            return View();
        }
    }
}