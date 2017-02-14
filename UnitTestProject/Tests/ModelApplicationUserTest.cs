using Fillager.Models.Account;
using Fillager.Models.Files;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace FillagerTests.Tests
{
  public class ModelApplicationUserTest
  {
    [Fact]
    public void EarnedExtraStorage()
    {
      var aUser = new ApplicationUser();
      var earned = aUser.EarnedExtraStorage;
      earned = 10;

      Assert.Equal(10L, earned);
      Assert.IsType<long>(earned);
    }

    [Fact]
    public void PayedExtraStorage()
    {
      var aUser = new ApplicationUser();
      var payedES = aUser.PayedExtraStorage;
      payedES = 10;

      Assert.IsType<long>(payedES);
      Assert.Equal(10L, payedES);

    }
    [Fact]
    public void StorageSpace()
    {
      var aUser = new ApplicationUser();
      var userStorageSpace = aUser.StorageSpace;

      userStorageSpace = 100;

      Assert.IsType<long>(userStorageSpace);
      Assert.Equal(100L, userStorageSpace);
    }
    [Fact]
    public void OtherStorageBonus()
    {
      var aUser = new ApplicationUser();
      var otherStorageBonus = aUser.StorageSpace;

      otherStorageBonus = 100;

      Assert.IsType<long>(otherStorageBonus);
      Assert.Equal(100L, otherStorageBonus);
    }
    [Fact]
    public void StorageUsed()
    {
      var aUser = new ApplicationUser();
      var userStorageSpaceUsed = aUser.StorageSpace;
      
      userStorageSpaceUsed = 100;

      Assert.IsType<long>(userStorageSpaceUsed);
      Assert.Equal(100L, userStorageSpaceUsed);
    }
  }
}
