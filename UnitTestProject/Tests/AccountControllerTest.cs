using System;
using System.Collections.Generic;
using System.Linq;
using Fillager.Controllers;
using Fillager.Models.Account;
using Fillager.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;

namespace FillagerTests.Tests
{
  public class AccountControllerTest
  {
    [Fact]
    public void ReturnRegisterView()
    {

      // Arrange
      var mockIuserStore = new Mock<IUserStore<ApplicationUser>>().Object;
      var userManager = new Mock<UserManager<ApplicationUser>>(mockIuserStore,
          null, null, null, null, null, null, null, null).Object;
      var context = new Mock<HttpContext>();
      var signinManager = new Mock<SignInManager<ApplicationUser>>(userManager,
          new HttpContextAccessor { HttpContext = context.Object },
          new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
          null, null)
      { CallBase = true }.Object;

      var roleManager = new Mock<RoleManager<UserRole>>(new Mock<IRoleStore<UserRole>>().Object,
          null,
          new Mock<ILookupNormalizer>().Object,
          null, null, 
          new HttpContextAccessor { HttpContext = context.Object })
          { CallBase = true }.Object;
      AccountController controller = new AccountController(userManager, signinManager, roleManager);

      // Act
      var result = controller.Register();

      // Assert
      var viewResult = Assert.IsType<ViewResult>(result);
      var model = Assert.IsAssignableFrom<IEnumerable<RegisterViewModel>>(viewResult.ViewData.Model);
      Assert.Equal(0,model.Count());
    }

    public void ReturnLoginView()
    {
    }

    public void ReturnLogOffView()
    {
    }
    private static Mock<SignInManager<TUser>> MockSignInManager<TUser>() where TUser : class
    {
      var context = new Mock<HttpContext>();
      var manager = MockUserManager<TUser>();
      return new Mock<SignInManager<TUser>>(manager.Object,
          new HttpContextAccessor { HttpContext = context.Object },
          new Mock<IUserClaimsPrincipalFactory<TUser>>().Object,
          null, null)
      { CallBase = true };
    }
    private static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
    {
      IList<IUserValidator<TUser>> UserValidators = new List<IUserValidator<TUser>>();
      IList<IPasswordValidator<TUser>> PasswordValidators = new List<IPasswordValidator<TUser>>();

      var store = new Mock<IUserStore<TUser>>();
      UserValidators.Add(new UserValidator<TUser>());
      PasswordValidators.Add(new PasswordValidator<TUser>());
      var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, UserValidators, PasswordValidators, null, null, null, null, null);
      return mgr;
    }
  }
}