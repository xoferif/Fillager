using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Fillager.ViewComponents
{
    public class FileUploadZoneViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string type)
        {
            ViewBag.UploadType = type;
            return View();
        }
    }
}
