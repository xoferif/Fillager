using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fillager.Models;
using Fillager.Models.Account;
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

        private readonly UserManager<UserIdentity> _userManager;
        private readonly IMinioService _minioService;

        public FillagerController(UserManager<UserIdentity>
            userManager, IMinioService minioService)
        {
            _userManager = userManager;
            _minioService = minioService;
        }


        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> UploadFiles(IList<IFormFile> files)
        {
            var file = files.FirstOrDefault();
            var memStream = new MemoryStream();

            file.CopyTo(memStream);

            var id = await _minioService.UploadFile("testbucket", memStream);
            return View("TransferWindow");
        }

        public FileResult DownloadFile(string id)
        {
            bool amIAllowedToDownload = true;
            if (amIAllowedToDownload)//todo check against db if cur. user is allowed 
            {
                return File(_minioService.DownloadFile("testbucket", id), "application/x-msdownload", "test.md");
            }
            return null;
        }
    }
}