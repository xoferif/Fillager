using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Fillager.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Users()
        {
            ViewData["Message"] = "Users Management";

            return View();
        }

        public IActionResult Statistics()
        {
            ViewData["Message"] = "System statistics";

            return View();
        }
    }
}
