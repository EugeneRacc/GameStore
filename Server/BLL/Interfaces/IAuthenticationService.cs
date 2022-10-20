using BLL.Models;

namespace BLL.Interfaces
{
    public interface IAuthenticationService
    {
        public Task RegisterUserAsync (RegisterModel registerModel);
    }
}
