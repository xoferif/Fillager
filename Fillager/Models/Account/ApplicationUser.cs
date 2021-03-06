﻿using System.Collections.Generic;
using Fillager.Models.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Fillager.Models.Account
{
    public class ApplicationUser : IdentityUser
    {
        public long EarnedExtraStorage { get; [Authorize] set; }
        public long PayedExtraStorage { get; [Authorize(Roles = "Admin, PayingUser")] set; } = 0;

        public long OtherStorageBonus { get; [Authorize(Policy = "ElevatedRights")] set; } = 0;

        public long StorageSpace => EarnedExtraStorage + PayedExtraStorage + OtherStorageBonus;
        //todo add role specific storage

        public long StorageUsed { get; set; }

        public virtual ICollection<File> Files { get; set; }
    }
}