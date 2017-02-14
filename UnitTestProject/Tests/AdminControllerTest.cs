using Fillager.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Xunit.Sdk;

namespace FillagerTests.Tests
{
  public class AdminControllerTest
  {
    [Fact]
    public void Users()
    {
      AdminController controller = new AdminController();

      ViewResult result = controller.Users() as ViewResult;

      Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Statistics()
    {
      AdminController controller = new AdminController();

      ViewResult result = controller.Statistics() as ViewResult;

      Assert.IsType<ViewResult>(result);
    }
  }
}
