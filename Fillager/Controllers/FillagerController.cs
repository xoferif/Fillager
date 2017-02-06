using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fillager.DataAccessLayer;
using Fillager.Models.Account;
using Fillager.Services;
using Fillager.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        //todo these are view requests too..... should be moved up
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
            return RedirectToAction("TransferWindow");
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
            return null; //TODO cannot return null from a FileResult........ 
        }

        private bool FileExists(string id)
        {
            return _db.Files.Any(e => e.FileId == id);
        }

        #region view requests

        public IActionResult TransferWindow()
        {
            var publicFiles = _db.Files.Where(file => file.OwnerGuid == null && file.IsPublic).ToList();
            var privateFiles = new List<File>();

            var user = _userManager.GetUserAsync(User).Result; //Users = Claims principal, get the object instead
            if (User.Identity.IsAuthenticated)
                privateFiles = _db.Files.Where(file => file.OwnerGuid.Id == user.Id).ToList();

            var viewmodel = new TransferWindowViewModel {PrivateFiles = privateFiles, PublicFiles = publicFiles};
            return View(viewmodel);
        }

        public IActionResult DeletePopup(string fileId)
        {
            var file = _db.Files.Find(fileId);

            ViewBag.FileName = file.FileName;
            ViewBag.FileId = file.FileId;

            return PartialView("Partials/DeleteFilePopup");
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFile(string fileId)
        {
            var file = await _db.Files.FindAsync(fileId);
            var user = await _userManager.GetUserAsync(User);

            if (file.OwnerGuid != null && file.OwnerGuid == user)
            {
                _db.Files.Remove(file);
                _db.SaveChanges();
                return RedirectToAction("TransferWindow");
            }

            return Unauthorized();
        }

        public IActionResult EditFilePopup(string fileId)
        {
            return PartialView("Partials/EditFilePopup", _db.Files.Find(fileId));
        }

        // POST: Files/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFile([Bind("FileId,FileName,IsPublic")] File file)
        {
            if (file.FileId == null) return NotFound();
            if (!ModelState.IsValid) return PartialView("Partials/EditFilePopup", file);
            if (!FileExists(file.FileId)) return NotFound();

            try
            {
                var user = await _userManager.GetUserAsync(User);
                var originalFile = await _db.Files.FindAsync(file.FileId);

                if (originalFile.OwnerGuid != user) return Unauthorized();

                var updatedFile = originalFile;

                updatedFile.FileName = file.FileName;
                updatedFile.IsPublic = file.IsPublic;

                _db.Update(updatedFile); //todo check user has access to file
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (FileExists(file.FileId))
                    return NotFound();
                throw;
            }
            return RedirectToAction("TransferWindow");
        }

        #endregion
    }
}