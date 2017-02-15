using System.Collections.Generic;

namespace Fillager.ViewModels
{
    public class FileListViewModel
    {
        public FileListViewModel(IList<FileViewModel> files)
        {
            Files = files;
        }

        public IList<FileViewModel> Files { get; set; }
        public bool Editable { get; set; } = false;
        public bool ShowPublicMarker { get; set; } = false;
        public int DiskUsedPct { get; set; } = 0;
        public bool ShowDiskUsedPctMarker { get; set; } = false;
    }
}