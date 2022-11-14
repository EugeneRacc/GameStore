using AutoMapper;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public AdminService(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<UserModel> SetRoleToUser(string roleName, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new GameStoreException($"User not found with such id: {userId}");
            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains(roleName))
                await _userManager.AddToRoleAsync(user, roleName);
            return _mapper.Map<UserModel>(user);
        }
    }
}
