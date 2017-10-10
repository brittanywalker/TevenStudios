using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TevenStudiosBudgetTracker.Controllers
{
    public class StatusCodeController : Controller
    {
        /**
            Function used to return the status code, redirect to appropriate page
            
            @return status code value
        */ 
        [HttpGet("/StatusCode/{statusCode}")]
        public IActionResult Index(int statusCode)
        {
            return View(statusCode);
        }
}
}