using System.Threading.Tasks;
using Fillager.Controllers;
using Fillager.Models.Account;
using Microsoft.AspNetCore.Identity;

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

        public Task<MenuItemListModel> GetMainMenu()
        {
            MenuList = new MenuItemListModel();
            //Menu 1
            MenuList.MenuItems.Add(new MenuItemModel(2, "Home", "Index", "Home", "Home", 0));
            MenuList.MenuItems.Add(new MenuItemModel(3, "Help", "", "Home", "About", 0));
            MenuList.MenuItems.Add(new MenuItemModel(6, "Fillager", "TransferWindow", "Fillager", "TransferWindow", 0));

            MenuList.MenuItems.Add(new MenuItemModel(4, "About", "About", "Home", "About", 3));
            MenuList.MenuItems.Add(new MenuItemModel(5, "Contact Us", "Contact", "Home", "Contact", 3));

            return Task.FromResult(MenuList);
        }

        public Task<MenuItemListModel> GetAccountMenu(bool loggedIn)
        {
            MenuList = new MenuItemListModel();
            if (loggedIn)
            {
                MenuList.MenuItems.Add(new MenuItemModel(7, "Admin", "", "Admin", "Admin", 1));
                MenuList.MenuItems.Add(new MenuItemModel(8, "Log off", "Register", "Account", "Sign Up", 1));
                MenuList.MenuItems.Add(new MenuItemModel(11, "Users", "About", "Home", "Users", 7));
                MenuList.MenuItems.Add(new MenuItemModel(12, "Statistics", "Contact", "Home", "Statistics", 7));
            }
            else
            {
                MenuList.MenuItems.Add(new MenuItemModel(9, "Sign Up", "Register", "Account", "Sign Up", 1));
                MenuList.MenuItems.Add(new MenuItemModel(10, "Login", "Login", "Account", "Login", 1));
            }
            return Task.FromResult(MenuList);
        }
    }
}