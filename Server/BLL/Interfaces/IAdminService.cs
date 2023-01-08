using BLL.Models;
using Microsoft.AspNetCore.Identity;

namespace BLL.Interfaces
{
    public interface IAdminService
    {
        public Task<UserModel> SetRoleToUser(string roleName, string userId);
        public Task<IEnumerable<UserModel>> GetUserByName(string? userName);
        public Task<IEnumerable<IdentityRole>> GetAllRoles();
    }
}
