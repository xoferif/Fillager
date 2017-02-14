using System;
using System.Collections.Generic;
using System.Linq;
using Fillager.Controllers;
using Fillager.Models.Account;
using Fillager.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using Xunit.Sdk;
using System.Threading.Tasks;
using System.Threading;

namespace FillagerTests.Tests
{
  public class AccountControllerTest
  {
    [Fact]
    public void ReturnRegisterView()
    {
      // Arrange
      var dummyUser = new ApplicationUser { UserName = "ed", Email = "testc@test.dk", StorageUsed = 0 };
      var cancelToken = new CancellationTokenSource();
      var mockIuserStore = new Mock<IUserStore<ApplicationUser>>();
      mockIuserStore.Setup(x => x.UpdateAsync(dummyUser, cancelToken.Token))
                .Returns(Task.FromResult(IdentityResult.Success));
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
      //mockRoleManager.Setup(x => x.GetClaimsAsync(new UserRole { Name = "admin"  })).Returns(Task.)
      var userManager = new UserManager<ApplicationUser>(mockIuserStore.Object, mockIOptions.Object, mockIPasswordHasher.Object, mockIUserValidator.Object, mockIPassValidator.Object, mockILookupNormalizer.Object, mockIdentityErrorDescriber.Object, mockIServiceProvider.Object, mockILogger.Object);
      AccountController controller = new AccountController(userManager, mockSigninManager.Object, mockRoleManager.Object);

      // Act
      //Task<IdentityResult> tt = (Task<IdentityResult>)mockIuserStore.Object.CreateAsync(dummyUser);
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