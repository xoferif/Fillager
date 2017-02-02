using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Fillager.ViewModels;
using Microsoft.AspNetCore.Authorization;

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
            _userManager = userManager;
            _loginManager = loginManager;
            _roleManager = roleManager;
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
            if (!ModelState.IsValid) return View("RegistrationView", obj);

            var user = new IdentityUser();
            user.UserName = obj.UserName;
            user.Email = obj.Email;
                
            var result = _userManager.CreateAsync
                (user, obj.Password).Result;

            
            if (!result.Succeeded)
            {
                obj.Errors = result.Errors.ToList();
                //todo use ModelState.AddModelError(); instead?
                return View("RegistrationView", obj);
            }

            if (!_roleManager.RoleExistsAsync("NormalUser").Result)
            {
                var role = new IdentityRole();
                role.Name = "NormalUser";

                var roleResult = _roleManager.
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

        #endregion

        #region login / logoff
        public IActionResult Login()
        {
            return View("LoginView");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoginAsync(LoginViewModel obj)
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


        [Authorize]
        public IActionResult LogOff()
        {
            _loginManager.SignOutAsync().Wait();
            return RedirectToAction("Index", "Fillager");
        }

        #endregion

    }
}