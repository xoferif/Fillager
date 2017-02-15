using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Fillager.ViewComponents
{
    public class FileUploadZoneViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string type)
        {
            ViewBag.UploadType = type;
            return View();
        }
    }
}
