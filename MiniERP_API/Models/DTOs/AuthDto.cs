using System.ComponentModel.DataAnnotations;

namespace MiniERP_API.Models.DTOs
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc.")]
        /// <example>admin</example>
        public string UserName { get; set; }
        
        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        /// <example>Admin@123</example>
        public string Password { get; set; }
    }

    public class RegisterRequest
    {
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc.")]
        /// <example>nhanvien1</example>
        public string UserName { get; set; }
        
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Định dạng Email không hợp lệ.")]
        /// <example>nv1@minierp.com</example>
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        /// <example>Password@123</example>
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Họ và tên là bắt buộc.")]
        /// <example>Nguyen Van A</example>
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
