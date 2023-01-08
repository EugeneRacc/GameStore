using AutoMapper;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminService(UserManager<User> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
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

        public async Task<IEnumerable<UserModel>> GetUserByName(string? userName)
        {
            var users = await _userManager.Users
                                          .Where(user => user.Email.ToLower().Contains((userName ?? "").ToLower()))
                                          .ToListAsync();
            var mappedUsers = _mapper.Map<IEnumerable<UserModel>>(users).ToList();
            for (int i = 0; i < users.Count; i++)
            {
                mappedUsers[i].RoleNames = await _userManager.GetRolesAsync(users[i]);
            }
            return mappedUsers;
        }

        public async Task<IEnumerable<IdentityRole>> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return roles;
        }
    }
}
