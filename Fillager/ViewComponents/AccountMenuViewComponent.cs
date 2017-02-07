using System.Threading.Tasks;
using Fillager.Models.Account;
using Fillager.Models.Menu;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fillager.ViewComponents
{
    public class AccountMenuViewComponent : ViewComponent
    {
        private readonly SignInManager<UserIdentity> _loginManager;
        private readonly MenuDataRepository _menuDataRepository;

        public AccountMenuViewComponent(MenuDataRepository menuDataRepository, SignInManager<UserIdentity> loginManager)
        {
            _menuDataRepository = menuDataRepository;
            _loginManager = loginManager;
        }

        //public IViewComponentResult Invoke()
        //{
        //    MenuItemListModel model = _menuDataRepository.GetMenus();
        //    return View(model);
        //}

        public async Task<IViewComponentResult> InvokeAsync()
        {
            bool loggedin;
            loggedin = _loginManager.IsSignedIn(HttpContext.User);
            var model = await _menuDataRepository.GetAccountMenu(loggedin);
            return View(model);
        }
    }
}