using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fillager.Models
{
    public class MyIdentityDbContext : IdentityDbContext<UserIdentity, UserRole, string>
    {
        public MyIdentityDbContext(DbContextOptions<MyIdentityDbContext> options)
        : base(options)
        {
            //intentionaly left blank
        }
    }
}
