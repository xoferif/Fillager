using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private const int DefaultStorage = 30 * 1024 * 1024; //Todo ENV var from docker
        private const int PublicStorageLimit = 250 * 1024 * 1024; //Todo ENNV var from docker
        private readonly IBackupQueueService _backupQueueService;
        private readonly ApplicationDbContext _db;
        private readonly IMinioService _minioService;

        private readonly UserManager<ApplicationUser> _userManager;

        public FillagerController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext db, IMinioService minioService, IBackupQueueService backupQueueService)
        {
            _userManager = userManager;
            _db = db;
            _minioService = minioService;
            _backupQueueService = backupQueueService;
        }

        private bool FileExists(string id)
        {
            return _db.Files.Any(e => e.FileId == id);
        }

        #region view requests

        [HttpGet]
        [Authorize]
        public IActionResult PrivateFileList()
        {
            var privateFiles = new List<File>();

            var user = _userManager.GetUserAsync(User).Result; //Users = Claims principal, get the object instead
            if (User.Identity.IsAuthenticated)
                privateFiles = _db.Files.Where(file => file.OwnerGuid.Id == user.Id).ToList();

            var usedSpacePct = CalculateUsedPct(user.StorageUsed, DefaultStorage + user.StorageSpace);

            var mappedFiles = Mapper.Map<List<File>, List<FileViewModel>>(privateFiles);

            var viewmodel = new FileListViewModel(mappedFiles)
            {
                DiskUsedPct = usedSpacePct,
                Editable = true,
                ShowPublicMarker = true,
                ShowDiskUsedPctMarker = true
            };


            return View("PrivateTransferWindow", viewmodel);
        }

        private static int CalculateUsedPct(double used, double max) => Convert.ToInt32(used / max * 100);

        [HttpGet]
        public IActionResult PublicFileList()
        {
            var publicFiles = _db.Files.Where(file => file.IsPublic).Include(file => file.OwnerGuid).ToList();
            var usedSpacePct = CalculateUsedPct(
                publicFiles.Where(file => file.OwnerGuid == null).Sum(file => file.Size), PublicStorageLimit);

            var mappedFiles = Mapper.Map<List<File>, List<FileViewModel>>(publicFiles);

            var viewmodel = new FileListViewModel(mappedFiles)
            {
                DiskUsedPct = usedSpacePct,
                ShowDiskUsedPctMarker = true
            };


            return View("PublicTransferWindow", viewmodel);
        }

        #endregion

        #region actions (Upload, Download, Edit, Delete)

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadPublicFiles(IList<IFormFile> files)
        {
            var sumOfFiles = files.Sum(file => file.Length);
            if (sumOfFiles > PublicStorageLimit)
                return RedirectToAction("PublicFileList", "Fillager", new {error = "NotEnoughStorage"});
            var usedStorage = _db.Files.Where(file => file.OwnerGuid == null && file.IsPublic).Sum(file => file.Size);
            if (usedStorage + sumOfFiles >=
                PublicStorageLimit)
            {
                var availableStorage = PublicStorageLimit - usedStorage;
                await FreeUpSpaceInPublicDrive(sumOfFiles - availableStorage);
            }

            await UploadTask(files, true);
            return RedirectToAction("PublicFileList");
        }

        /// <summary>
        ///     removes public files from the db until the given space is made available
        /// </summary>
        /// <param name="spaceToFreeUp">space to remove</param>
        /// <returns></returns>
        private async Task FreeUpSpaceInPublicDrive(long spaceToFreeUp)
        {
            var filesToDelete = _db.Files
                .Where(file => file.OwnerGuid == null && file.IsPublic)
                .OrderBy(file => file.CreatedDateTime);

            while (spaceToFreeUp > 0)
            {
                var target = filesToDelete.Take(3).ToList(); //take the 3 oldest files

                spaceToFreeUp -= target.Sum(file => file.Size); //subtract their size from the space still needed

                _db.Files.RemoveRange(target); //remove them
            }

            await _db.SaveChangesAsync();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadPrivateFiles(IList<IFormFile> files)
        {
            var sumOfFiles = files.Sum(file => file.Length);

            var user = await _userManager.GetUserAsync(User);

            var allowedStorage = user.StorageSpace + DefaultStorage;
            var usedStorage = user.StorageUsed;

            if (usedStorage + sumOfFiles > allowedStorage)
                return Json(new {error = "Not enough free storage"});


            await UploadTask(files, false);
            await UpdateUsedSpaceForUser(user, sumOfFiles);
            return Ok();
            //return RedirectToAction("PrivateFileList");
        }

        public async Task UpdateUsedSpaceForUser(ApplicationUser user, long additionalStorage)
        {
            user.StorageUsed += additionalStorage;
            _db.Update(user);
            await _db.SaveChangesAsync();

            //recalculate sum of all the users files

#pragma warning disable 4014

            var sumOfAllFiles = user.Files.Sum(file => file.Size); //why is this broken?
            sumOfAllFiles = _db.Files.Where(file => file.OwnerGuid == user).Sum(file => file.Size);
            if (sumOfAllFiles != user.StorageUsed)
            {
                user.StorageUsed = sumOfAllFiles;
                _db.Update(user);
                _db.SaveChangesAsync(); //fire and forget this event
            }
#pragma warning restore 4014
        }


        private async Task UploadTask(IEnumerable<IFormFile> files, bool forcePublic)
        {
            foreach (var file in files)
            {
                var memStream = new MemoryStream();

                file.CopyTo(memStream);

                var id = await _minioService.UploadFile("pocbucket", memStream);

                await _db.Files.AddAsync(new File
                {
                    FileId = id,
                    FileName = file.FileName,
                    IsPublic = forcePublic,
                    Size = file.Length,
                    CreatedDateTime = DateTime.Now,
                    OwnerGuid = forcePublic ? null : await _userManager.GetUserAsync(User)
                });

                await _db.SaveChangesAsync();
                _backupQueueService.SendBackupRequest("pocbucket", id);
            }
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
                return File(_minioService.DownloadFile("pocbucket", id), "application/x-msdownload", file.FileName);
            return null; //TODO cannot return null from a FileResult........ 
        }

        public IActionResult EditFilePopup(string fileId)
        {
            return PartialView("Partials/EditFilePopup", _db.Files.Find(fileId));
        }

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
            return RedirectToAction("PrivateFileList");
        }


        public IActionResult DeletePopup(string fileId)
        {
            var file = _db.Files.Find(fileId);

            ViewBag.FileName = file.FileName;
            ViewBag.FileId = file.FileId;

            return PartialView("Partials/DeleteFilePopup");
        }

        [HttpPost]
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
                await UpdateUsedSpaceForUser(user, -file.Size);
                return RedirectToAction("PrivateFileList");
            }

            return Unauthorized();
        }

        public async Task<IActionResult> DeleteFiles(string[] checkbox)
        {
            foreach (var fileId in checkbox)
            {
                var file = await _db.Files.FindAsync(fileId);
                var user = await _userManager.GetUserAsync(User);

                if (file.OwnerGuid != null && file.OwnerGuid == user)
                {
                    _db.Files.Remove(file);
                    _db.SaveChanges();
                    await UpdateUsedSpaceForUser(user, -file.Size);
                }
                else
                {
                    return Unauthorized();
                }
            }
            return RedirectToAction("PrivateFileList");
        }

        #endregion
    }
}