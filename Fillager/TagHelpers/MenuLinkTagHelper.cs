using System;
using System.Linq;
using Fillager.Models.Menu;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Fillager.TagHelpers
{
    [HtmlTargetElement("menulink", Attributes = "controller-name, action-name, menu-text, menu-id")]
    public class MenuLinkTagHelper : TagHelper
    {
        public MenuLinkTagHelper(MenuDataRepository navigationMenu, IUrlHelperFactory urlHelper)
        {
            _navigationMenu = navigationMenu;
            _urlHelper = urlHelper;
        }

        public string ControllerName { get; set; }
        public string ActionName { get; set; }

        public string MenuText { get; set; }
        public int MenuId { get; set; }

        public MenuDataRepository _navigationMenu { get; set; }

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public IUrlHelperFactory _urlHelper { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "li";

            var routeData = ViewContext.RouteData.Values;
            var currentController = routeData["controller"];
            var currentAction = routeData["action"];

            var subMenus = _navigationMenu.GetMainMenu().Result.MenuItems.Where(m => m.ParentId == MenuId).ToList();

            if (subMenus.Count > 0)
            {
                var subMenuClass = "";

                var caretSpan = new TagBuilder("span");
                caretSpan.AddCssClass("caret");

                var dropdownMenu = new TagBuilder("a");
                dropdownMenu.MergeAttribute("href", "#");
                dropdownMenu.AddCssClass("dropdown-toggle");
                dropdownMenu.MergeAttribute("data-toggle", "dropdown");
                dropdownMenu.InnerHtml.Append(MenuText);
                dropdownMenu.InnerHtml.AppendHtml(caretSpan);

                var ul = new TagBuilder("ul");
                ul.AddCssClass("dropdown-menu");

                foreach (var subMenu in subMenus)
                {
                    var li = new TagBuilder("li");

                    var urlHelper = _urlHelper.GetUrlHelper(ViewContext);

                    var subMenuUrl = urlHelper.Action(subMenu.ActionName, subMenu.ControllerName);

                    var a = new TagBuilder("a");
                    a.MergeAttribute("href", $"{subMenuUrl}");
                    a.MergeAttribute("title", subMenu.MenuItemText);
                    a.InnerHtml.Append(subMenu.MenuItemText);

                    li.InnerHtml.AppendHtml(a);

                    ul.InnerHtml.AppendHtml(li);
                }

                if (subMenus.Any(s => s.ActionName == currentAction.ToString()) &&
                    subMenus.Any(s => s.ControllerName == currentController.ToString()))
                    subMenuClass = "dropdown active";
                else
                    subMenuClass = "dropdown";

                output.Attributes.Add("class", subMenuClass);

                output.Content.AppendHtml(dropdownMenu);
                output.Content.AppendHtml(ul);
            }
            else
            {
                var urlHelper = _urlHelper.GetUrlHelper(ViewContext);

                var menuUrl = urlHelper.Action(ActionName, ControllerName);

                var a = new TagBuilder("a");
                a.MergeAttribute("href", $"{menuUrl}");
                a.MergeAttribute("title", MenuText);
                a.InnerHtml.Append(MenuText);

                if (string.Equals(ActionName, currentAction as string, StringComparison.OrdinalIgnoreCase)
                    && string.Equals(ControllerName, currentController as string, StringComparison.OrdinalIgnoreCase))
                    output.Attributes.Add("class", "active");

                output.Content.AppendHtml(a);
            }
        }
    }
}