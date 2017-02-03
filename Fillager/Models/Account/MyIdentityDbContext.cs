using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fillager.Models.Account
{
    public class MyIdentityDbContext : IdentityDbContext<UserIdentity, UserRole, string>
    {
        public MyIdentityDbContext(DbContextOptions<MyIdentityDbContext> options)
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
    }
}
