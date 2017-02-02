using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Fillager.Models
{
    public class UserRole : IdentityRole
    {
        public long DefaultStorage { get; [Authorize(Policy = "ElevatedRights")] set; } = 30 * 1024;
    }
}
