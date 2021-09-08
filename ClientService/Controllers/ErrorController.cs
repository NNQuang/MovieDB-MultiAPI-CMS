using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientService.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/Error/{Code:int}")]
        public IActionResult Error(int Code)
        {
            switch (Code)
            {
                case 404:
                    return View("_404");
                case 401:
                    return View("_401");
                default:
                    return View("_OtherErrors");
            }
        }
    }
}
