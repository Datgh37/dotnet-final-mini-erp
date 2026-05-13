using System.ComponentModel.DataAnnotations;

namespace MiniERP_API.Models.DTOs
{
    public class LoginRequest
    {
        /// <example>admin</example>
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc.")]
        public string UserName { get; set; }
        
        /// <example>Admin@123</example>
        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        public string Password { get; set; }
    }

    public class RegisterRequest
    {
        /// <example>manager_test</example>
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc.")]
        public string UserName { get; set; }
        
        /// <example>manager@test.com</example>
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Định dạng Email không hợp lệ.")]
        public string Email { get; set; }
        
        /// <example>Password@999</example>
        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string Password { get; set; }
        
        /// <example>Test Manager Account</example>
        [Required(ErrorMessage = "Họ và tên là bắt buộc.")]
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
