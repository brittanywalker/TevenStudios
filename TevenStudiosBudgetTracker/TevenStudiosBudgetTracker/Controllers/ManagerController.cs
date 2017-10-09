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
    public class ManagerController : Controller
    {
        //Set Session names
        const string SessionKeyId = "_ID";
        const string SessionKeyRoleId = "_RoleId";
        const string SessionKeyName = "_Name";
        const string SessionKeyEmail = "_Email";

        //This returns the manager view.
        public IActionResult Index()
        {
            ViewData["Message"] = "Management page.";

            //Check if User is logged in, if not, make the url forbidden. This is useful if they attempt to type in the URL.
            if (HttpContext.Session.GetInt32(SessionKeyRoleId) == null)
            {
                return StatusCode(403);
            }

            //Check if they are a Manager, if not, send them to the forbidden page.
            int Roleid = (int)HttpContext.Session.GetInt32(SessionKeyRoleId);

            if (Roleid != 2)
            {
                return StatusCode(403);
            }

            UserContext context = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;
            ManagerViewData data = new ManagerViewData();
            User user = context.retrieveUserDetails((int)HttpContext.Session.GetInt32(SessionKeyId));
            data.Employees = context.GetEmployeesForManager(user.ID);
            data.CurrentUser = user;

            PendingRequestsContext pcontext = HttpContext.RequestServices.GetService(typeof(PendingRequestsContext)) as PendingRequestsContext;
            data.PendingRequests = pcontext.GetAllPendingRequestsManager(user.ID);

            return View(data);
        }

        
        public IActionResult ApproveRequest(string ID)
        {
            ViewData["Message"] = "Management page.";

            // gets employee's pending requests
            PendingRequestsContext Pendingcontext = HttpContext.RequestServices.GetService(typeof(PendingRequestsContext)) as PendingRequestsContext;
            var ApprovedRequest = Pendingcontext.ApprovePendingRequest(ID);

            return Json(new { id = ID });
        }

        public IActionResult DeclineRequest(string ID)
        {
            ViewData["Message"] = "Management page.";

            // gets employee's pending requests
            PendingRequestsContext Pendingcontext = HttpContext.RequestServices.GetService(typeof(PendingRequestsContext)) as PendingRequestsContext;
            var ApprovedRequest = Pendingcontext.DeclinePendingRequest(ID);

            return Json(new { id = ID }); ;
        }

        // This function gets information about a selected user to be displayed on the right hand side of the manager screen
        public IActionResult GetSelectedInfo(int UserID)
        {
            ViewData["Message"] = "Management page.";

            // gets selected employee
            UserContext context = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;
            User selectedEmployee = context.retrieveUserDetails(UserID);

            // gets employee's pending requests
            PendingRequestsContext Pendingcontext = HttpContext.RequestServices.GetService(typeof(PendingRequestsContext)) as PendingRequestsContext;
            var pendingRequests = Pendingcontext.GetAllPendingRequests(UserID);

            // gets employee's past requests
            TransactionContext transactionContext = HttpContext.RequestServices.GetService(typeof(TransactionContext)) as TransactionContext;
            var pastRequests = transactionContext.GetAllPastRequests(UserID);

            // gets the employees current budget 
            double budget = transactionContext.getCurrentBudget(selectedEmployee.ID, selectedEmployee.ChangeAnnualBudgetDate, selectedEmployee.StartBudget, selectedEmployee.AnnualBudget, selectedEmployee.ChangeAnnualBudget);

            return Json(new { id = UserID, selectedEmployee = selectedEmployee, currentBudget = budget, pendingRequests = pendingRequests, pastRequests = pastRequests });
        }

    }
}