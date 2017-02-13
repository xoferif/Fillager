using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Fillager.ViewComponents
{
    public class FileUploadZoneViewComponent : ViewComponent
    {
#pragma warning disable 1998
        public async Task<IViewComponentResult> InvokeAsync()
#pragma warning restore 1998
        {
            return View();
        }
    }
}
