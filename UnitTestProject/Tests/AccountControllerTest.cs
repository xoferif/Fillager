using System;
using System.Collections.Generic;
using System.Linq;
using Fillager.Controllers;
using Fillager.Models.Account;
using Fillager.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Xunit;
using Xunit.Sdk;
using Assert = Xunit.Assert;

namespace FillagerTests.Tests
{
  public class AccountControllerTest
  {
    [Fact]
    public void ReturnRegisterView()
    {
      // Arrange mock for userManager
      var mockIuserStore = new Mock<IUserStore<ApplicationUser>>();
      var mockIOptions = new Mock<IOptions<IdentityOptions>>();
      var mockIPasswordHasher = new Mock<IPasswordHasher<ApplicationUser>>();
      var mockIUserValidator = new Mock<IEnumerable<IUserValidator<ApplicationUser>>>();
      var mockIPassValidator = new Mock<IEnumerable<IPasswordValidator<ApplicationUser>>>();
      var mockILookupNormalizer = new Mock<ILookupNormalizer>();
      var mockIdentityErrorDescriber = new Mock<IdentityErrorDescriber>();
      var mockIServiceProvider = new Mock<IServiceProvider>();
      var mockILogger = new Mock<ILogger<UserManager<ApplicationUser>>>();

      var mockSigninManager = new Mock<SignInManager<ApplicationUser>>();
      var mockRoleManager = new Mock<RoleManager<UserRole>>();
      var userManager = new UserManager<ApplicationUser>(mockIuserStore.Object, mockIOptions.Object, mockIPasswordHasher.Object, mockIUserValidator.Object, mockIPassValidator.Object, mockILookupNormalizer.Object, mockIdentityErrorDescriber.Object, mockIServiceProvider.Object, mockILogger.Object);
      AccountController controller = new AccountController(userManager, mockSigninManager.Object, mockRoleManager.Object);
      
      // Act
      var result = controller.Register();

      // Assert
      var viewResult = Assert.IsType<ViewResult>(result);
      var model = Assert.IsAssignableFrom<IEnumerable<RegisterViewModel>>(viewResult.ViewData.Model);
      Assert.Equal(1,model.Count());
    }

    public void ReturnLoginView()
    {
    }

    public void ReturnLogOffView()
    {
    }
  }
}