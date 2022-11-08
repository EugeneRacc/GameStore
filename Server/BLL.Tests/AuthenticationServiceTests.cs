
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using AutoFixture;
using AutoMapper;
using BLL.Exceptions;
using BLL.Mapper;
using BLL.Models;
using BLL.Services;
using DAL.Entities;
using DAL.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;

namespace BLL.Tests
{
    public class AuthenticationServiceTests
    {
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
        private readonly Mock<IUnitOfWork> _dbMock = new Mock<IUnitOfWork>();
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public AuthenticationServiceTests()
        {
            _userManagerMock = TestHelpers.MockUserManager<User>();
            _roleManagerMock = TestHelpers.MockRoleManager<IdentityRole>();
            _mapper = TestHelpers.GetMapper();
            _configuration = TestHelpers.GetConfiguration();
            _tokenValidationParameters = TestHelpers.GetTokenValidationParameters();
        }

        [Fact]
        public async Task RegisterUserAsync_WithExistedEmail_ShouldThrowGameStoreExc()
        {
            //Arrange
            var fixture = new Fixture();
            var registerModel = fixture.Create<RegisterModel>();
            var existedUser = fixture.Create<User>();
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                            .ReturnsAsync(existedUser);
            var authService = new AuthenticationService(_userManagerMock.Object, _roleManagerMock.Object, _dbMock.Object,
                _configuration, _mapper, _tokenValidationParameters);

            //act
            Func<Task> act = async () => await authService.RegisterUserAsync(registerModel);

            //assert
            await act.Should().ThrowExactlyAsync<GameStoreException>();
        }

        [Fact]
        public async Task RegisterUserAsync_WithCorrectRegisterModel_ShouldCallCreateAsyncOnce()
        {
            //Arrange
            var fixture = new Fixture();
            var registerModel = fixture.Create<RegisterModel>();
            User notExistedUser = null;
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                            .ReturnsAsync(notExistedUser);
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                            .ReturnsAsync(IdentityResult.Success);
            var authService = new AuthenticationService(_userManagerMock.Object, _roleManagerMock.Object, _dbMock.Object,
                _configuration, _mapper, _tokenValidationParameters);

            //act
            await authService.RegisterUserAsync(registerModel);

            //assert
            _userManagerMock.Verify(x 
                => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task LoginUserAsync_WithNotValidEmail_ShouldThrowAuthExc()
        {
            //Arrange
            var fixture = new Fixture();
            User notExistedUser = null;
            var loginModel = fixture.Create<LoginModel>();
            _userManagerMock.Setup(x => x.FindByEmailAsync(loginModel.Email))
                            .ReturnsAsync(notExistedUser);
            var authService = new AuthenticationService(_userManagerMock.Object, _roleManagerMock.Object, _dbMock.Object,
                _configuration, _mapper, _tokenValidationParameters);

            //act
            Func<Task> act = async () => await authService.LoginUserAsync(loginModel);

            //assert
            await act.Should().ThrowExactlyAsync<AuthenticationException>();
        }

        [Fact]
        public async Task LoginUserAsync_WithValidEmail_ShouldReturnAuthResultModel()
        {
            //Arrange
            var fixture = new Fixture();
            var existedUser = fixture.Create<User>();
            var loginModel = fixture.Create<LoginModel>();
            _userManagerMock.Setup(x => x.FindByEmailAsync(loginModel.Email))
                            .ReturnsAsync(existedUser);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(existedUser, loginModel.Password))
                            .ReturnsAsync(true);
            _dbMock.Setup(x => x.RefreshTokenRepository.AddAsync(It.IsAny<RefreshToken>()))
                   .Verifiable();
            _dbMock.Setup(x => x.SaveAsync())
                   .Verifiable();
            var authService = new AuthenticationService(_userManagerMock.Object, _roleManagerMock.Object, _dbMock.Object,
                _configuration, _mapper, _tokenValidationParameters);

            //act
            var tokenModel = await authService.LoginUserAsync(loginModel);

            //assert
            tokenModel.Should().BeOfType<AuthResult>();
        }

        //TODO Doesn't work
        [Fact]
        public async Task VerifyAndGenerateTokenAsync_WithValidToken_ShouldReturnAuthResultModel()
        {
            //Arrange
            var jwtTokenHandlerMock = new Mock<JwtSecurityTokenHandler>();
            var fixture = new Fixture();
            var refreshToken = fixture.Create<RefreshToken>();
            var user = fixture.Create<User>();
            var tokenRequestModel = fixture.Create<TokenRequestModel>();
            SecurityToken securityToken;
            _dbMock.Setup(x => x.RefreshTokenRepository.GetRefreshTokenByToken(It.IsAny<string>()))
                   .ReturnsAsync(refreshToken);
            _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                            .ReturnsAsync(user);
            jwtTokenHandlerMock.Setup(x => x.ValidateToken( tokenRequestModel.Token, 
                                _tokenValidationParameters,
                                out securityToken))
                               .Returns(new ClaimsPrincipal())
                               .Verifiable();

            var authService = new AuthenticationService(_userManagerMock.Object, _roleManagerMock.Object, _dbMock.Object,
                _configuration, _mapper, _tokenValidationParameters);

            //act
            var tokenModel = await authService.VerifyAndGenerateTokenAsync(tokenRequestModel);

            //assert
            tokenModel.Should().BeOfType<AuthResult>();
        }
    }
}
