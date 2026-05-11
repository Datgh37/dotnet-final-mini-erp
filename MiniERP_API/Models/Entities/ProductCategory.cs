namespace MiniERP_API.Models.Entities
{
    public class ProductCategory : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
