using Fillager.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FillagerTests.Tests
{
  [TestClass]
  public class AdminControllerTest
  {
    [TestMethod]
    public void Users()
    {
      AdminController controller = new AdminController();

      ViewResult result = controller.Users() as ViewResult;

      Assert.IsNotNull(result);
    }

    [TestMethod]
    public void Statistics()
    {
      AdminController controller = new AdminController();

      ViewResult result = controller.Statistics() as ViewResult;

      Assert.IsNotNull(result);
    }
  }
}
