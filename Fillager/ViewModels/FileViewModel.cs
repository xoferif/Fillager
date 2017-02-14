using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fillager.Models.Account;
using Fillager.Models.Files;

namespace Fillager.ViewModels
{
    public class FileViewModel
    {
        public FileViewModel()
        {
            
        }

        //DB
        public string FileName { get; set; }
        public string FileId { get; set; }
        public long Size { get; set; }
        public virtual ApplicationUser OwnerGuid { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreatedDateTime { get; set; }

        //Own
        public bool IsChecked { get; set; }
    }
}
