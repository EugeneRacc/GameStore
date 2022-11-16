using BLL.Models;

namespace BLL.Interfaces
{
    public interface IAdminService
    {
        public Task<UserModel> SetRoleToUser(string roleName, string userId);
    }
}
