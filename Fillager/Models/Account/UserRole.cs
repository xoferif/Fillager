﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Fillager.Models.Account
{
    public class UserRole : IdentityRole
    {
        public long DefaultStorage { get; [Authorize(Policy = "ElevatedRights")] set; } = 30 * 1024;
    }
}