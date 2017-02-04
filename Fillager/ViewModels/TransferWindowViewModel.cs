using System.Collections.Generic;
using Fillager.Models.Files;

namespace Fillager.ViewModels
{
    public class TransferWindowViewModel
    {
        public IList<File> PublicFiles { get; set; }
        public IList<File> PrivateFiles { get; set; }

    }
}
