using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
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

        public AuthenticationService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, 
            IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task RegisterUserAsync(RegisterModel registerModel)
        {
            var userExists = await _userManager.FindByEmailAsync(registerModel.Email);
            if (userExists != null)
                throw new GameStoreException("User with such email already exist");
            var newUser = new User()
            {
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                Email = registerModel.Email,
                UserName = registerModel.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(newUser, registerModel.Password);
            if (!result.Succeeded) throw new GameStoreException("Something went wrong");
        }

        public async Task<AuthResult> LoginUserAsync(LoginModel loginModel)
        {
            var userExists = await _userManager.FindByEmailAsync(loginModel.Email);
            if (userExists != null && await _userManager.CheckPasswordAsync(userExists, loginModel.Password))
            {
                var tokenValue = GenerateJwtTokenAsync(userExists);
                return tokenValue;
            }
            throw new AuthenticationException("Email or password is incorrect");
        }

        private AuthResult GenerateJwtTokenAsync(User userExists)
        {
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userExists.UserName),
                new Claim(ClaimTypes.NameIdentifier, userExists.Id),
                new Claim(JwtRegisteredClaimNames.Email, userExists.Email),
                new Claim(JwtRegisteredClaimNames.Sub, userExists.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var authSignKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.UtcNow.AddMinutes(5),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSignKey, SecurityAlgorithms.HmacSha256));
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return new AuthResult()
            {
                Token = jwtToken,
                ExpirationTime = token.ValidTo
            };
        }
    }
}
