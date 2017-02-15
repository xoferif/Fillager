using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fillager.Models.Account;
using Fillager.Models.Files;
using Fillager.ViewModels;

namespace Fillager
{
    public static class MappingConfig
    {
        public static void RegisterMaps()
        {
            AutoMapper.Mapper.Initialize(config =>
            {
                config.CreateMap<File, FileViewModel>();
                //config.CreateMap<ApplicationUser, ApplicationUserViewModel>();
            });
        }
    }
}
