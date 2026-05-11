namespace MiniERP_API.Models.Entities
{
    public class Customer : BaseEntity
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
