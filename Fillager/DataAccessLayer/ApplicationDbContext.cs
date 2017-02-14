using System;
using Fillager.Models.Account;
using Fillager.Models.Files;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fillager.DataAccessLayer
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, UserRole, string>
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
            try
            {
                //auto-migrate this db, should be disabled in production
                Database.Migrate();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public DbSet<File> Files { get; set; }

        public DbSet<Fillager.Models.Account.ApplicationUser> ApplicationUser { get; set; }
    }
}