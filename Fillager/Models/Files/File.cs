﻿using System;
using Fillager.Models.Account;

namespace Fillager.Models.Files
{
    public class File
    {
        public string FileName { get; set; }
        public string FileId { get; set; }
        public long Size { get; set; }
        public virtual ApplicationUser OwnerGuid { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}