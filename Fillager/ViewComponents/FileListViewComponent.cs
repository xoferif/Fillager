using Fillager.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Fillager.ViewComponents
{
    public class FileListViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(FileListViewModel viewModel)
        {
            return View(viewModel);
        }
    }
}