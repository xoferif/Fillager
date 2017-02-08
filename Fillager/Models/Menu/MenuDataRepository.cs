using System.Threading.Tasks;
using Fillager.Controllers;

namespace Fillager.Models.Menu
{
    public class MenuDataRepository
    {
        private readonly AccountController _accountController;

        public MenuDataRepository(AccountController accountController)
        {
            _accountController = accountController;
        }

        public MenuItemListModel MenuList { get; set; }

        public Task<MenuItemListModel> GetMenu(bool loggedIn, int menutype, bool isAdmin)
        {
            MenuList = new MenuItemListModel();
            if (menutype == 0)
            {
                MenuList.MenuItems.Add(new MenuItemModel(2, "Home", "Index", "Home", "Home", 0));
                MenuList.MenuItems.Add(new MenuItemModel(3, "Help", "", "Home", "About", 0));
                if (loggedIn)
                    MenuList.MenuItems.Add(new MenuItemModel(10, "Your Files", "PrivateFileList", "Fillager",
                        "Your Files", 0));
                MenuList.MenuItems.Add(new MenuItemModel(7, "Public Files", "PublicFileList", "Fillager", "Public Files",
                    0));
                MenuList.MenuItems.Add(new MenuItemModel(4, "About", "About", "Home", "About", 3));
                MenuList.MenuItems.Add(new MenuItemModel(5, "Contact Us", "Contact", "Home", "Contact", 3));
            }
            else if (menutype == 1)
            {
                if (loggedIn)
                {
                    if (isAdmin)
                        MenuList.MenuItems.Add(new MenuItemModel(7, "Admin", "", "Admin", "Admin", 1));
                    MenuList.MenuItems.Add(new MenuItemModel(8, "Log off", "LogOff", "Account", "Sign Up", 1));
                    MenuList.MenuItems.Add(new MenuItemModel(11, "Users", "Users", "Admin", "Users", 7));
                    MenuList.MenuItems.Add(new MenuItemModel(12, "Statistics", "Statistics", "Admin", "Statistics", 7));
                }
                else
                {
                    MenuList.MenuItems.Add(new MenuItemModel(9, "Sign Up", "Register", "Account", "Sign Up", 1));
                    MenuList.MenuItems.Add(new MenuItemModel(10, "Login", "Login", "Account", "Login", 1));
                }
            }

            return Task.FromResult(MenuList);
        }
    }
}