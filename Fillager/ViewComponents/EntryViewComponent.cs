using System.Threading.Tasks;
using Fillager.Models.Menu;
using Microsoft.AspNetCore.Mvc;

namespace Fillager.ViewComponents
{
    public class EntryMenuViewComponent : ViewComponent
    {
        private readonly MenuDataRepository _menuDataRepository;

        public EntryMenuViewComponent(MenuDataRepository menuDataRepository)
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
            MenuItemListModel model = await _menuDataRepository.GetMenus();
            return View(model);
        }
    }
}
