using BLL.Models;

namespace BLL.Interfaces
{
    public interface IAuthenticationService
    {
        public Task RegisterUserAsync (RegisterModel registerModel);
        public Task<AuthResult> LoginUserAsync(LoginModel loginModel);
        public Task<AuthResult> VerifyAndGenerateTokenAsync(TokenRequestModel tokenRequest);
    }
}
