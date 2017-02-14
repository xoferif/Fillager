using Fillager.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Xunit.Sdk;
using Assert = Xunit.Assert;

namespace FillagerTests.Tests
{
  public class HomeControllerTest
  {
    [Fact]
    public void Index()
    {
      HomeController controller = new HomeController();

      ViewResult result = controller.Index() as ViewResult;

      Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void About()
    {
      HomeController controller = new HomeController();

      ViewResult result = controller.About() as ViewResult;

      Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Contact()
    {
      HomeController controller = new HomeController();

      ViewResult result = controller.Contact() as ViewResult;

      Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Error()
    {
      HomeController controller = new HomeController();

      ViewResult result = controller.Error() as ViewResult;

      Assert.IsType<ViewResult>(result);
    }
  }
}
