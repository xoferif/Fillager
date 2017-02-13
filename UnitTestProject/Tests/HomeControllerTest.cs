using Fillager.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FillagerTests.Tests
{
  [TestClass]
  public class HomeControllerTest
  {
    [TestMethod]
    public void Index()
    {
      HomeController controller = new HomeController();

      ViewResult result = controller.Index() as ViewResult;

      Assert.IsNotNull(result);
    }

    [TestMethod]
    public void About()
    {
      HomeController controller = new HomeController();

      ViewResult result = controller.About() as ViewResult;

      Assert.IsNotNull(result);
    }

    [TestMethod]
    public void Contact()
    {
      HomeController controller = new HomeController();

      ViewResult result = controller.Contact() as ViewResult;

      Assert.IsNotNull(result);
    }

    [TestMethod]
    public void Error()
    {
      HomeController controller = new HomeController();

      ViewResult result = controller.Error() as ViewResult;

      Assert.IsNotNull(result);
    }
  }
}
