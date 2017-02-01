using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Fillager.ViewModels;

namespace Fillager.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _loginManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public AccountController(UserManager<IdentityUser> userManager,
           SignInManager<IdentityUser> loginManager,
           RoleManager<IdentityRole> roleManager)
        {
            this._userManager = userManager;
            this._loginManager = loginManager;
            this._roleManager = roleManager;
        }

        #region registration

        public IActionResult Register()
        {
            return View("RegistrationView");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel obj)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser();
                user.UserName = obj.UserName;
                user.Email = obj.Email;
                
                IdentityResult result = _userManager.CreateAsync
                (user, obj.Password).Result;

                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync("NormalUser").Result)
                    {
                        IdentityRole role = new IdentityRole();
                        role.Name = "NormalUser";
                        IdentityResult roleResult = _roleManager.
                        CreateAsync(role).Result;
                        if (!roleResult.Succeeded)
                        {
                            ModelState.AddModelError("",
                             "Error while creating role!");
                            return View("RegistrationView",obj);
                        }
                    }

                    _userManager.AddToRoleAsync(user,
                                 "NormalUser").Wait();
                    return RedirectToAction("Login", "Account");
                }
            }
            return View("RegistrationView",obj);
        }

        #endregion

        #region login / logoff
        public IActionResult Login()
        {
            return View("LoginView");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel obj)
        {
            if (ModelState.IsValid)
            {
                var result = _loginManager.PasswordSignInAsync
                (obj.UserName, obj.Password,
                  obj.RememberMe, false).Result;

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Fillager");
                }

                ModelState.AddModelError("", "Invalid login!");
            }

            return View("LoginView",obj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogOff()
        {
            _loginManager.SignOutAsync().Wait();
            return RedirectToAction("Login", "Account");
        }

        #endregion

    }
}