using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TevenStudiosBudgetTracker.Controllers
{
    public class StatusCodeController : Controller
    {
        [HttpGet("/StatusCode/{statusCode}")]
        public IActionResult Index(int statusCode)
        {
            return View(statusCode);
        }
}
}