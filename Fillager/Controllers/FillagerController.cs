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
  public class FillagerController : Controller
  {

    public IActionResult Index()
    {
      return View();
    }

    public IActionResult Contact()
    {
      return View();
    }
    public IActionResult About()
    {
      return View();
    }

    private readonly UserManager<IdentityUser> userManager;

    public FillagerController(UserManager<IdentityUser>
                          userManager)
    {
      this.userManager = userManager;
    }


    [Authorize]
    public IActionResult LoginIndex()
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