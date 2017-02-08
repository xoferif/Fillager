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


        public bool VerifyUser()
        {
            return User != null && _loginManager.IsSignedIn(User);
        }

        #region registration

        public IActionResult Register()
        {
            return View("Registration");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel obj)
        {
            if (!ModelState.IsValid) return View("Registration", obj);

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
                return View("Registration", obj);
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
                    return View("Registration", obj);
                }
            }

            _userManager.AddToRoleAsync(user,
                "NormalUser").Wait();

            //Registered!

            //this.AddToastMessage("Success", $"Your account: {obj.UserName} has been registered", ToastType.Success);

            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region login / logoff

        public IActionResult Login()
        {
            return View("Login");
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
                    return RedirectToAction("Index", "Home");

                ModelState.AddModelError("", "Invalid login!");
            }
            return View("Login", obj);
        }


        [Authorize]
        public IActionResult LogOff()
        {
            _loginManager.SignOutAsync().Wait();
            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}