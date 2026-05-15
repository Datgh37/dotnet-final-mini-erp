using System.ComponentModel.DataAnnotations;

namespace MiniERP_API.Models.DTOs
{
    public class UserDto
    {
        /// <example>1</example>
        public int Id { get; set; }
        
        /// <example>admin</example>
        public string UserName { get; set; }
        
        /// <example>admin@minierp.com</example>
        public string Email { get; set; }
        
        /// <example>System Administrator</example>
        public string FullName { get; set; }
        
        /// <example>Admin</example>
        public string Role { get; set; }
    }

    public class UserUpdateDto
    {
        /// <example>staff</example>
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc.")]
        public string UserName { get; set; }

        /// <example>staff@minierp.com</example>
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Định dạng Email không hợp lệ.")]
        public string Email { get; set; }
        
        /// <example>Staff</example>
        [Required(ErrorMessage = "Họ và tên là bắt buộc.")]
        public string FullName { get; set; }

        /// <example>Staff</example>
        public string Role { get; set; }
    }

    public class UserPasswordChangeDto
    {
        /// <example>admin@123</example>
        [Required(ErrorMessage = "Mật khẩu cũ là bắt buộc.")]
        public string OldPassword { get; set; }
        
        /// <example>NewPassword@456</example>
        [Required(ErrorMessage = "Mật khẩu mới là bắt buộc.")]
        [MinLength(6, ErrorMessage = "Mật khẩu mới phải có ít nhất 6 ký tự.")]
        public string NewPassword { get; set; }
    }
}
