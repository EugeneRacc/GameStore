using AutoMapper;
using BLL.Mapper;

namespace BLL.Tests.Helpers
{
    public static class SeedData
    {
        public static IMapper CreateMapperProfile()
        {
            var myProfile = new AutoMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

            return new AutoMapper.Mapper(configuration);
        }
    }
}
