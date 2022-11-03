using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.Text;
using AutoMapper;
using BLL.Mapper;

namespace BLL.Tests
{
    internal static class TestHelpers
    {
        public static IConfiguration GetConfiguration()
        {
            var myConfiguration = new Dictionary<string, string>
            {
                {"AllowedHosts", "*"},
                {"Jwt:Secret", "DhftOS5uphK3vmCJQrexST1RsyjZBjXWRgJMFPU4"},
                {"Jwt:Issuer", "https://localhost:44368/"},
                {"Jwt:Audience", "https://localhost:44368/"},
            };
            var configuration = new ConfigurationBuilder()
                                .AddInMemoryCollection(myConfiguration)
                                .Build();
            return configuration;
        }

        public static IMapper GetMapper()
        {
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            return new AutoMapper.Mapper(mapperConfiguration);
        }

        public static TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters();
        }

        public static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());
            return mgr;
        }

        public static Mock<RoleManager<TRole>> MockRoleManager<TRole>(IRoleStore<TRole> store = null) where TRole : class
        {
            store = store ?? new Mock<IRoleStore<TRole>>().Object;
            var roles = new List<IRoleValidator<TRole>>();
            roles.Add(new RoleValidator<TRole>());
            return new Mock<RoleManager<TRole>>(store, roles, MockLookupNormalizer(),
                new IdentityErrorDescriber(), null);
        }
        
        public static ILookupNormalizer MockLookupNormalizer()
        {
            var normalizerFunc = new Func<string, string>(i =>
            {
                if (i == null)
                {
                    return null;
                }
                else
                {
                    return Convert.ToBase64String(Encoding.UTF8.GetBytes(i)).ToUpperInvariant();
                }
            });
            var lookupNormalizer = new Mock<ILookupNormalizer>();
            lookupNormalizer.Setup(i => i.NormalizeName(It.IsAny<string>())).Returns(normalizerFunc);
            lookupNormalizer.Setup(i => i.NormalizeEmail(It.IsAny<string>())).Returns(normalizerFunc);
            return lookupNormalizer.Object;
        }
    }
}
