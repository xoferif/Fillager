using AutoMapper;
using Fillager.Models.Files;
using Fillager.ViewModels;

namespace Fillager
{
    public static class MappingConfig
    {
        public static void RegisterMaps()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<File, FileViewModel>();
                //config.CreateMap<ApplicationUser, ApplicationUserViewModel>();
            });
        }
    }
}