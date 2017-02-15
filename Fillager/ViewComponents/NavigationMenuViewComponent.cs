using System.Threading.Tasks;
using Fillager.Models.Account;
using Fillager.Models.Menu;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fillager.ViewComponents
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly SignInManager<ApplicationUser> _loginManager;
        private readonly MenuDataRepository _menuDataRepository;

        public NavigationMenuViewComponent(MenuDataRepository menuDataRepository,
            SignInManager<ApplicationUser> loginManager)
        {
            _menuDataRepository = menuDataRepository;
            _loginManager = loginManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(int i)
        {
            ViewBag.MenuType = i;
            var loggedin = _loginManager.IsSignedIn(HttpContext.User);
            //bool isAdmin = User.IsInRole("Admin");
            var isAdmin = true;
            var model = await _menuDataRepository.GetMenu(loggedin, i, isAdmin);
            return View(model);
        }
    }
}