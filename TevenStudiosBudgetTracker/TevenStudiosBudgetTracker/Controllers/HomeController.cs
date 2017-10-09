﻿using System;
using Microsoft.AspNetCore.Mvc;
using TevenStudiosBudgetTracker.Models;
using System.Dynamic;
using Microsoft.AspNetCore.Http;

namespace TevenStudiosBudgetTracker.Controllers
{

    public class HomeController : Controller
    {
        //Set Session names
        const string SessionKeyId = "_ID";
        const string SessionKeyRoleId = "_RoleId";
        const string SessionKeyName = "_Name";
        const string SessionKeyEmail = "_Email";

        public IActionResult Index()
        {
            ViewData["Message"] = "Home page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        //Doesn't seem to be used. DELETE? 
        public ActionResult SetCurrentUserIndex(int UserIndex)
        {
            UserContext context = HttpContext.RequestServices.GetService(typeof(TevenStudiosBudgetTracker.Models.UserContext)) as UserContext;
            AdminViewData data = new AdminViewData();
            data.Users = context.GetAllUsers();
            data.Managers = context.GetAllManagers();
            data.CurrentUserIndex = UserIndex;

            User umodel = new User
            {
                Name = data.Users[UserIndex].Name,
                Email = data.Users[UserIndex].Email,
                ManagerId = data.Users[UserIndex].ManagerId,
                RoleId = data.Users[UserIndex].RoleId,
                StartBudget = data.Users[UserIndex].StartBudget,
                AnnualBudget = data.Users[UserIndex].AnnualBudget
            };

            data.currentEditUser = umodel;

            return View("Admin", data);
        }

 
    }
}