using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fillager.Models;
using Fillager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Fillager.Controllers
{
    public class FillagerController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult TransferWindow()
        {
            return View();
        }

        private readonly UserManager<UserIdentity> userManager;
        private readonly IMinioService _minioService;

        public FillagerController(UserManager<UserIdentity>
            userManager, IMinioService minioService)
        {
            this.userManager = userManager;
            this._minioService = minioService;
        }


        [Authorize]
        public IActionResult LoginIndex()
        {
            var user = userManager.GetUserAsync
                (HttpContext.User).Result;

            ViewBag.Message = $"Welcome {user.UserName}!";
            if (userManager.IsInRoleAsync(user, "NormalUser").Result)
            {
                ViewBag.RoleMessage = "You are a NormalUser.";
            }
            return View();
        }

        [HttpPost]
        //[Authorize]
        public IActionResult UploadFiles(IList<IFormFile> files)
        {
            var file = files.FirstOrDefault();
            var memStream = new MemoryStream();

            file.CopyTo(memStream);
            
            _minioService.UploadFile("testbucket", memStream);
            return Accepted();
        }
    }
}