namespace MiniERP_API.Models.Entities
{
    public class Brand : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
