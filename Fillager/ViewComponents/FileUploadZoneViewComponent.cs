using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Fillager.ViewComponents
{
    public class FileUploadZoneViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
