namespace MiniERP_API.Models.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
    }

    public class UserUpdateDto
    {
        public string Email { get; set; }
        public string FullName { get; set; }
    }

    public class UserPasswordChangeDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
