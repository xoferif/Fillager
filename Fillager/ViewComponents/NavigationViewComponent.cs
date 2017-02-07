using System.Threading.Tasks;
using Fillager.Models.Account;
using Fillager.Models.Menu;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fillager.ViewComponents
{
    public class NavigationViewComponent : ViewComponent
    {
        private readonly SignInManager<UserIdentity> _loginManager;
        private readonly MenuDataRepository _menuDataRepository;

        public NavigationViewComponent(MenuDataRepository menuDataRepository, SignInManager<UserIdentity> loginManager)
        {
            _menuDataRepository = menuDataRepository;
            _loginManager = loginManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(int i)
        {
            ViewBag.MenuType = i;
            bool loggedin = _loginManager.IsSignedIn(HttpContext.User);
            //bool isAdmin = User.IsInRole("Admin");
            bool isAdmin = true;
            MenuItemListModel model = await _menuDataRepository.GetMenu(loggedin, i, isAdmin);
            return View(model);
        }
    }
}