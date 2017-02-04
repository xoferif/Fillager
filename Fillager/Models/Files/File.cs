using Fillager.Models.Account;

namespace Fillager.Models.Files
{
    public class File
    {
        public string FileName { get; set; }
        public string FileId { get; set; }
        public long Size { get; set; }
        public virtual UserIdentity OwnerGuid { get; set; }
        public bool IsPublic { get; set; }
    }
}