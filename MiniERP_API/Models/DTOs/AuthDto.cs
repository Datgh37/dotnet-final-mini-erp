namespace MiniERP_API.Models.DTOs
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class RegisterRequest
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
    }

    public class AuthResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public UserDto User { get; set; }
    }

    public class RefreshTokenRequest
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
