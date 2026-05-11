using System;

namespace MiniERP_API.Models.Entities
{
    public class User : BaseEntity
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FullName { get; set; }
    }
}
