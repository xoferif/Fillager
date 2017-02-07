using System.Threading.Tasks;
using Fillager.Models.Account;
using Fillager.Models.Menu;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fillager.ViewComponents
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly MenuDataRepository _menuDataRepository;

        public NavigationMenuViewComponent(MenuDataRepository menuDataRepository)
        {
            _menuDataRepository = menuDataRepository;
        }

        //public IViewComponentResult Invoke()
        //{
        //    MenuItemListModel model = _menuDataRepository.GetMenus();
        //    return View(model);
        //}

        public async Task<IViewComponentResult> InvokeAsync()
        {
            MenuItemListModel model = await _menuDataRepository.GetMainMenu();
            return View(model);
        }
    }
}