using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fillager.DataAccessLayer;
using Fillager.Models.Account;
using Fillager.Services;
using Fillager.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using File = Fillager.Models.Files.File;

namespace Fillager.Controllers
{
    public class FillagerController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IMinioService _minioService;

        private readonly UserManager<UserIdentity> _userManager;

        public FillagerController(UserManager<UserIdentity>
            userManager, SignInManager<UserIdentity> signInManager, IMinioService minioService, ApplicationDbContext db)
        {
            _userManager = userManager;
            _minioService = minioService;
            _db = db;
        }

        #region view requests

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
            var publicFiles = _db.Files.Where(file => file.OwnerGuid == null && file.IsPublic).ToList();
            var privateFiles = new List<File>();

            var user = _userManager.GetUserAsync(User).Result; //Users = Claims principal, get the object instead
            if (User.Identity.IsAuthenticated)
            {
                privateFiles = _db.Files.Where(file => file.OwnerGuid.Id == user.Id).ToList();
            }

            var viewmodel = new TransferWindowViewModel() {PrivateFiles = privateFiles, PublicFiles = publicFiles};
            return View(viewmodel);
        }

        #endregion


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadFiles(IList<IFormFile> files)
        {
            foreach (var file in files)
            {
                var memStream = new MemoryStream();

                file.CopyTo(memStream);

                var id = await _minioService.UploadFile("testbucket", memStream);

                await _db.Files.AddAsync(new File
                {
                    FileId = id,
                    FileName = file.FileName,
                    IsPublic = false,
                    Size = file.Length,
                    OwnerGuid = await _userManager.GetUserAsync(User)
                });

                await _db.SaveChangesAsync();
            }
            return View("TransferWindow");
        }

        /// <summary>
        ///     Downloads the file with the specified id, if it is public,
        ///     or the logged in user is the owner of the file.
        /// </summary>
        /// <param name="id">id of the file to download</param>
        /// <returns>the file requested, or null if the file doesn't exist or the user isn't allowed to download it</returns>
        public FileResult DownloadFile(string id)
        {
            bool amIAllowedToDownload;
            var file = _db.Files.Find(id);

            if (file == null) return null; // this really the best way to deny stuff?


            var user = _userManager.GetUserAsync(User).Result; //Users = Claims principal, get the object instead

            amIAllowedToDownload = file.IsPublic ||
                                   User.Identity.IsAuthenticated && user.Equals(file.OwnerGuid);

            if (amIAllowedToDownload)
                return File(_minioService.DownloadFile("testbucket", id), "application/x-msdownload", file.FileName);
            return null;//TODO cannot return null from a FileResult........ 
        }
    }
}