﻿using System.Linq;
using System.Threading.Tasks;
using Fillager.DataAccessLayer;
using Fillager.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fillager.Controllers
{
    [Authorize]
    //[Authorize(Policy = "ElevatedRights")] <-- this will add policy based authentication instead of just needing to be logged in.
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Users()
        {
            return View(await _context.Users.ToListAsync());
        }

        public IActionResult Statistics()
        {
            return View();
        }


        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
                return NotFound();

            var applicationUser = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
                return NotFound();
            return View("EditUser", applicationUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,
            [Bind("EarnedExtraStorage,PayedExtraStorage,OtherStorageBonus,UserName,Email,PhoneNumber,LockoutEnabled")] ApplicationUser applicationUser)
        {
            if (!_context.Users.Any(user => user.Id == id))
                return NotFound();

            if (!ModelState.IsValid) return View(applicationUser);

            try
            {
                //get object with all its fields from the database, and set the properties editable
                var userobj = _context.Users.FirstOrDefault(user => user.Id == id);

                userobj.EarnedExtraStorage = applicationUser.EarnedExtraStorage;
                userobj.PayedExtraStorage = applicationUser.PayedExtraStorage;
                userobj.OtherStorageBonus = applicationUser.OtherStorageBonus;
                //userobj.UserName = applicationUser.UserName;
                userobj.Email = applicationUser.Email;
                userobj.PhoneNumber = applicationUser.PhoneNumber;
                userobj.LockoutEnabled = applicationUser.LockoutEnabled;

                _context.Update(userobj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationUserExists(applicationUser.Id))
                    return NotFound();
                throw;
            }
            return RedirectToAction("Users");
        }

        private bool ApplicationUserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}