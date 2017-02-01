using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fillager.Models
{
    public class MyIdentityDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public MyIdentityDbContext(DbContextOptions<MyIdentityDbContext> options)
        : base(options)
        {
            //intentionaly left blank
        }
    }
}
