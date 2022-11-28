using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using DAL.Enums;
using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BLL.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public AuthenticationService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, 
            IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper, TokenValidationParameters tokenValidationParameters)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mapper = mapper;
            _tokenValidationParameters = tokenValidationParameters;
        }

        public async Task RegisterUserAsync(RegisterModel registerModel)
        {
            var userExists = await _userManager.FindByEmailAsync(registerModel.Email);
            if (userExists != null)
                throw new GameStoreException("User with such email already exist");
            var newUser = new User
            {
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                Email = registerModel.Email,
                UserName = registerModel.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(newUser, registerModel.Password);
            if (!result.Succeeded)
            {
                var messageForExc = new StringBuilder("");
                foreach (var identityError in result.Errors)
                {
                    messageForExc.Append($"{identityError.Code} - {identityError.Description}");
                    messageForExc.Append("\n");
                }
                throw new GameStoreException(messageForExc.ToString());
            }
            switch (registerModel.Role)
            {
                case RoleType.Admin:
                    await _userManager.AddToRoleAsync(newUser, "Admin");
                    break;
                case RoleType.Manager:
                    await _userManager.AddToRoleAsync(newUser, "Manager");
                    break;
                default:
                    await _userManager.AddToRoleAsync(newUser, "User");
                    break;
            }
        }

        public async Task<AuthResult> LoginUserAsync(LoginModel loginModel)
        {
            var userExists = await _userManager.FindByEmailAsync(loginModel.Email);
            if (userExists != null && await _userManager.CheckPasswordAsync(userExists, loginModel.Password))
            {
                var tokenValue = await GenerateJwtToken(userExists, null);
                return tokenValue;
            }
            throw new AuthenticationException("Email or password is incorrect");
        }

        public async Task<AuthResult> VerifyAndGenerateTokenAsync(TokenRequestModel tokenRequest)
        {
            var jwtTokenHandle = new JwtSecurityTokenHandler();
            var storedToken = await _unitOfWork.RefreshTokenRepository
                                               .GetRefreshTokenByToken(tokenRequest.RefreshToken);
            var dbUser = await _userManager.FindByIdAsync(storedToken.UserId);
            try
            {
                var tokenCheckResult = jwtTokenHandle.ValidateToken(tokenRequest.Token, _tokenValidationParameters,
                    out var validatedToken);
                return await GenerateJwtToken(dbUser, storedToken);
            }
            catch (SecurityTokenExpiredException e)
            {
                if (storedToken.ExpirationDate >= DateTime.UtcNow)
                {
                    return await GenerateJwtToken(dbUser, storedToken);
                }
                return await GenerateJwtToken(dbUser, null);
            }
        }

        private async Task<AuthResult> GenerateJwtToken(User userExists, RefreshToken? rToken)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userExists.UserName),
                new Claim(ClaimTypes.NameIdentifier, userExists.Id),
                new Claim(JwtRegisteredClaimNames.Email, userExists.Email),
                new Claim(JwtRegisteredClaimNames.Sub, userExists.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var userRoles = await _userManager.GetRolesAsync(userExists);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var authSignKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.UtcNow.AddMinutes(5),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSignKey, SecurityAlgorithms.HmacSha256));
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            if (rToken != null)
            {
                return new AuthResult
                {
                    Token = jwtToken,
                    RefreshToken = rToken.Token,
                    ExpirationTime = token.ValidTo
                };
            }
            var refreshToken = await GenerateRefreshTokenAsync(userExists, token);
            return new AuthResult
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token,
                ExpirationTime = token.ValidTo
            };
        }

        private async Task<RefreshTokenModel> GenerateRefreshTokenAsync(User userExists, JwtSecurityToken token)
        {
            var refreshToken = new RefreshTokenModel
            {
                TokenId = token.Id,
                IsRevoked = false,
                UserId = userExists.Id,
                CreatingDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddMonths(6),
                Token = Guid.NewGuid() + "-" + Guid.NewGuid()
            };
            await _unitOfWork.RefreshTokenRepository.AddAsync(_mapper.Map<RefreshToken>(refreshToken));
            await _unitOfWork.SaveAsync();
            return refreshToken;
        }
    }
}
