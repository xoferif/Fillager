using Fillager.Models.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fillager.Models.Account;
using Microsoft.AspNetCore.Identity;

namespace Fillager.Models.Menu
{
    public class MenuDataRepository
    {
        public MenuItemListModel MenuList { get; set; }

        public MenuDataRepository(UserManager<UserIdentity> userManager)
        {
            
        }

        public Task<MenuItemListModel> GetMenus()
        {
            MenuList = new MenuItemListModel();
            MenuList.MenuItems.Add(new MenuItemModel(2, "Home", "Index", "Home", "Home", 0));
            MenuList.MenuItems.Add(new MenuItemModel(3, "Help", "", "Home", "About", 0));
            MenuList.MenuItems.Add(new MenuItemModel(4, "About", "About", "Home", "About", 3));
            MenuList.MenuItems.Add(new MenuItemModel(5, "Contact Us", "Contact", "Home", "Contact", 3));
            MenuList.MenuItems.Add(new MenuItemModel(6, "Your Files", "PrivateFileList", "Fillager", "Your Files", 0)); //todo only show if logged in
            MenuList.MenuItems.Add(new MenuItemModel(7, "Public Files", "PublicFileList", "Fillager", "Public Files", 0));

            MenuList.MenuItems.Add(new MenuItemModel(8, "Sign Up", "Register", "Account", "Sign Up", 1));
            MenuList.MenuItems.Add(new MenuItemModel(9, "Login", "Login", "Account", "Login", 1));


            //MenuList.MenuItems.Add(new MenuItemModel(7, "Technology", "TechNews", "News", "Technology News", 3));
            //MenuList.MenuItems.Add(new MenuItemModel(8, "Political", "PoliticalNews", "News", "Political News", 3));
            //MenuList.MenuItems.Add(new MenuItemModel(9, "Sports", "SportsNews", "News", "Sports News", 3));

            //MenuList.MenuItems.Add(new MenuItemModel(10, "Latest Videos", "LatestVideos", "Videos", "Latest Videos", 4));
            //MenuList.MenuItems.Add(new MenuItemModel(11, "Must Watch Videos", "MustWatchVideos", "Videos", "Must Watch Videos", 4));

            return Task.FromResult(MenuList);
        }
    }
}
