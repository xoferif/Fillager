using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Fillager.Models
{
    public class UserIdentity : IdentityUser
    {
        public long EarnedExtraStorage { get; [Authorize] set; }
        public long PayedExtraStorage { get; [Authorize(Roles = "Admin, PayingUser")] set; } = 0;

        public long OtherStorageBonus
        {
            get;
            [Authorize(Policy = "ElevatedRights")]
            set;
        } = 0;

        public long StorageSpace => EarnedExtraStorage + PayedExtraStorage + OtherStorageBonus;

        public long StorageUsedIn { get; set; }

    }
}
