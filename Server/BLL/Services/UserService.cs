using AutoMapper;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<User> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<UserModel>> GetAllAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            return _mapper.Map<IEnumerable<UserModel>>(users);
        }

        public async Task<UserModel> GetByIdAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var mappedUser = _mapper.Map<UserModel>(user);
            mappedUser.RoleNames = await _userManager.GetRolesAsync(user);
            return mappedUser;
        }

        public async Task<UserModel> AddAsync(UserModel model)
        {
            model.Id = Guid.NewGuid();
            await _userManager.CreateAsync(_mapper.Map<User>(model));
            return model;
        }

        public async Task<UserModel> UpdateAsync(UserModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if (user == null)
            {
                throw new GameStoreException("User with such ID doesn't exist");
            }

            user.FirstName = model.FirstName ?? user.FirstName;
            user.LastName = model.LastName ?? user.LastName;
            user.Email = model.Email ?? user.Email;
            await _userManager.UpdateAsync(user);
            return _mapper.Map<UserModel>(user);
        }

        public async Task DeleteAsync(UserModel model)
        {
            await _userManager.DeleteAsync(_mapper.Map<User>(model));
        }
    }
}
