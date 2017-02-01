using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Fillager.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;

        public HomeController(UserManager<IdentityUser>
                              userManager)
        {
            this.userManager = userManager;
        }


        [Authorize]
        public IActionResult Index()
        {
            IdentityUser user = userManager.GetUserAsync
                                 (HttpContext.User).Result;

            ViewBag.Message = $"Welcome {user.UserName}!";
            if (userManager.IsInRoleAsync(user, "NormalUser").Result)
            {
                ViewBag.RoleMessage = "You are a NormalUser.";
            }
            return View();
        }

    }
}