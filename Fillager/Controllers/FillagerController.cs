using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Fillager.Controllers
{
  public class FillagerController : Controller
  {

    public IActionResult Index()
    {
      return View();
    }

    // 
    // GET: /HelloWorld/Welcome/ 

    public string Welcome()
    {
      return "Fucktards be welcome here...";
    }
  }
}