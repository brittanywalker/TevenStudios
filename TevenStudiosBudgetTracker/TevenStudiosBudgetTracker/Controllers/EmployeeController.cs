using System;
using Microsoft.AspNetCore.Mvc;
using TevenStudiosBudgetTracker.Models;
using System.Dynamic;
using Microsoft.AspNetCore.Http;

namespace TevenStudiosBudgetTracker.Controllers
{
    public class EmployeeController : Controller
    {

        //Set Session names
        const string SessionKeyId = "_ID";
        const string SessionKeyRoleId = "_RoleId";
        const string SessionKeyName = "_Name";
        const string SessionKeyEmail = "_Email";

        /**
            Returns the Employee view
            
            @return the AdminViewData
        */
        public IActionResult Index()
        {
            ViewData["Message"] = "Employee page.";

            //Check if User is logged in, if not, make the url forbidden. This is useful if they attempt to type in the URL.
            if (HttpContext.Session.GetInt32(SessionKeyRoleId) == null)
            {
                return StatusCode(403);
            }

            //Check if they are a Manager or an Employee, if not, send them to the forbidden page.
            int Roleid = (int)HttpContext.Session.GetInt32(SessionKeyRoleId);

            if ((Roleid != 1) && (Roleid != 2))
            {
                return StatusCode(403);
            }

            dynamic mymodel = new ExpandoObject();

            // set user and transaction contexts
            TransactionContext transactionContext = HttpContext.RequestServices.GetService(typeof(TransactionContext)) as TransactionContext;
            UserContext userContext = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;

            // gets the current user's details
            User user = userContext.retrieveUserDetails((int)HttpContext.Session.GetInt32(SessionKeyId));
            mymodel.CurrentUser = user;

            // get and set the UI's budget
            double budget = transactionContext.getCurrentBudget(user.ID, user.ChangeAnnualBudgetDate, user.StartBudget, user.AnnualBudget, user.ChangeAnnualBudget);
            mymodel.Budget = budget;

            //get the user's max budget spend so they can't spend more than they currently have and will
            //accrue for the year
            var futureAccruedBudget = getUserMaxBudgetRequest(user);
            var maxBudget = futureAccruedBudget + budget;
            if (maxBudget > user.AnnualBudget)
            {
                maxBudget = user.AnnualBudget;
            }
            mymodel.MaxBudgetRequest = maxBudget;

            // pending request
            PendingRequestsContext context = HttpContext.RequestServices.GetService(typeof(PendingRequestsContext)) as PendingRequestsContext;
            mymodel.PendingRequests = context.GetAllPendingRequests(user.ID);

            // past requests
            mymodel.PastRequests = transactionContext.GetAllPastRequests((int)HttpContext.Session.GetInt32(SessionKeyId));

            return View(mymodel);
        }


        /**
            Function that submits a new request for the logged in employee

            @return the users model
        */ 
        [HttpPost]
        public IActionResult SubmitRequest()
        {
            // Build user model		
            PendingRequest newRequest = new PendingRequest();
            newRequest.Description = HttpContext.Request.Form["description"].ToString();
            newRequest.Cost = HttpContext.Request.Form["requestCost"].ToString();
            DateTime dateTimeNow = DateTime.Now;
            newRequest.Date = dateTimeNow.ToString("yyyy-MM-dd HH:mm:ss");

            // Get context		
            PendingRequestsContext pendingRequestContext = HttpContext.RequestServices.GetService(typeof(TevenStudiosBudgetTracker.Models.PendingRequestsContext)) as PendingRequestsContext;

            //Save user to database, get result		
            int result = pendingRequestContext.SubmitPendingRequest(newRequest, (int)HttpContext.Session.GetInt32(SessionKeyId));
            if (result > 0)
            {
                ViewBag.Result = " Request was successfully submitted";
                ViewBag.isSuccess = true;
            }
            else
            {
                ViewBag.Result = "Something went wrong";
                ViewBag.isSuccess = false;
            }

            ViewData["Message"] = "Employee page.";

            dynamic mymodel = new ExpandoObject();

            // set user and transaction contexts
            TransactionContext transactionContext = HttpContext.RequestServices.GetService(typeof(TransactionContext)) as TransactionContext;
            UserContext userContext = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;

            // gets the current user's details
            User user = userContext.retrieveUserDetails((int)HttpContext.Session.GetInt32(SessionKeyId));
            mymodel.CurrentUser = user;

            // get and set the UI's budget
            double budget = transactionContext.getCurrentBudget(user.ID, user.ChangeAnnualBudgetDate, user.StartBudget, user.AnnualBudget, user.ChangeAnnualBudget);
            mymodel.Budget = budget;

            //get the user's max budget spend so they can't spend more than they currently have and will
            //accrue for the year
            var futureAccruedBudget = getUserMaxBudgetRequest(user);
            var maxBudget = futureAccruedBudget + budget;
            if (maxBudget > user.AnnualBudget)
            {
                maxBudget = user.AnnualBudget;
            }
            mymodel.MaxBudgetRequest = maxBudget;

            // pending request
            PendingRequestsContext context = HttpContext.RequestServices.GetService(typeof(PendingRequestsContext)) as PendingRequestsContext;
            mymodel.PendingRequests = context.GetAllPendingRequests(user.ID);

            // past requests
            mymodel.PastRequests = transactionContext.GetAllPastRequests((int)HttpContext.Session.GetInt32(SessionKeyId));

            return View("Index", mymodel);
        }


        /**
            Function that gets the users max budget request

            @param user user whose max budget request is being calculated
            @return the passed users future accrued budget
        */
        public double getUserMaxBudgetRequest(User user)
        {
            DateTime today = DateTime.Today;
            Console.WriteLine("today: " + today);
            String year = today.ToString("yyyy");
            String date = user.StartDate.ToString("dd/MM");
            String time = user.StartDate.ToString("HH:mm:ss tt");
            String budgetChange = date + "/" + year + " " + time;
            DateTime budgetChangeDate = Convert.ToDateTime(budgetChange);
            int daysDifference = (int)(budgetChangeDate - today).TotalDays;

            double futureAccruedBudget;
            if (daysDifference > 0)
            {
                futureAccruedBudget = daysDifference * (user.AnnualBudget / 365);
            }
            else
            {
                var nextYearBudgetDate = budgetChangeDate.AddYears(1);
                int daysToBudgetYear = (int)(nextYearBudgetDate - today).TotalDays;
                futureAccruedBudget = daysToBudgetYear * (user.AnnualBudget / 365);
            }
            return futureAccruedBudget;
        }
    }
}