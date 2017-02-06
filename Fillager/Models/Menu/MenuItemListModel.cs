using System.Collections.Generic;

namespace Fillager.Models.Menu
{
    public class MenuItemListModel
    {
        public MenuItemListModel()
        {
            MenuItems = new List<MenuItemModel>();
        }

        public List<MenuItemModel> MenuItems { get; set; }
    }
}