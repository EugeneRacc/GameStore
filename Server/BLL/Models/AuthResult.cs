namespace BLL.Models
{
    public class AuthResult
    {
        public string Token { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
