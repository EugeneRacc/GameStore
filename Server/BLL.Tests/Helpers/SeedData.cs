using AutoMapper;
using BLL.Mapper;

namespace BLL.Tests.Helpers
{
    public static class SeedData
    {
        public static IMapper CreateMapperProfile()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            return new AutoMapper.Mapper(configuration);
        }
    }
}
