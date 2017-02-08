﻿using System.Collections.Generic;
using Fillager.Models.Files;
using Microsoft.AspNetCore.Mvc;

namespace Fillager.ViewComponents
{
    public class FileListViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<File> files, bool editable, bool showPublicMarker)
        {
            ViewBag.Editable = editable;
            ViewBag.ShowPublicMarker = showPublicMarker;
            return View(files);
        }
    }
}