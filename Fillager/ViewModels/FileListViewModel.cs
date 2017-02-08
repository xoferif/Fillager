using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Fillager.Models.Files;

namespace Fillager.ViewModels
{
    public class FileListViewModel
    {
        public FileListViewModel(IList<File> files)
        {
            Files = files;
        }

        public IList<File> Files { get; set; }
        public bool Editable { get; set; } = false;
        public bool ShowPublicMarker { get; set; } = false;
        public int DiskUsedPct { get; set; } = 0;
        public bool ShowDiskUsedPctMarker { get; set; } = false;
    }
}
