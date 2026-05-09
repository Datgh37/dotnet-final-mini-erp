namespace MiniERP_API.Models.Entities
{
    public class Product : BaseEntity
    {
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
        public string SKU { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public decimal CostPrice { get; set; }
        public decimal RetailPrice { get; set; }
        public int StockQuantity { get; set; }
        public string ImageUrl { get; set; }
    }
}
