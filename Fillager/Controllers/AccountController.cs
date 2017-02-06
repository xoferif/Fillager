using System.Linq;
using Fillager.Models.Account;
using Fillager.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fillager.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<UserIdentity> _loginManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly UserManager<UserIdentity> _userManager;


        public AccountController(UserManager<UserIdentity> userManager,
            SignInManager<UserIdentity> loginManager,
            RoleManager<UserRole> roleManager)
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

            var user = new UserIdentity
            {
                UserName = obj.UserName,
                Email = obj.Email
            };

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
                var role = new UserRole();
                role.Name = "NormalUser";

                var roleResult = _roleManager.
                    CreateAsync(role).Result;
                if (!roleResult.Succeeded)
                {
                    ModelState.AddModelError("",
                        "Error while creating role!");
                    return View("RegistrationView", obj);
                }
            }

            _userManager.AddToRoleAsync(user,
                "NormalUser").Wait();

            //Registered!

            //this.AddToastMessage("Success", $"Your account: {obj.UserName} has been registered", ToastType.Success);

            return RedirectToAction("Index", "Fillager");
        }

        #endregion

        #region login / logoff

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
                    return RedirectToAction("Index", "Fillager");

                ModelState.AddModelError("", "Invalid login!");
            }
            return View("LoginView", obj);
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