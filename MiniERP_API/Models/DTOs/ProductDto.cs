namespace MiniERP_API.Models.DTOs
{
    public class ProductDto
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

    public class ProductCreateUpdateDto
    {
        /// <example>1</example>
        public int? CategoryId { get; set; }

        /// <example>2</example>
        public int? BrandId { get; set; }
        
        /// <example>AP-IP15PM</example>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Mã SKU là bắt buộc.")]
        public string SKU { get; set; }
        
        /// <example>iPhone 15 Pro Max 256GB</example>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
        public string Name { get; set; }
        
        /// <example>Apple flagship smartphone with Titanium frame.</example>
        public string Description { get; set; }

        /// <example>Cái</example>
        public string Unit { get; set; }
        
        /// <example>1100.00</example>
        [System.ComponentModel.DataAnnotations.Range(0, double.MaxValue, ErrorMessage = "Giá vốn không được âm.")]
        public decimal CostPrice { get; set; }
        
        /// <example>1400.00</example>
        [System.ComponentModel.DataAnnotations.Range(0, double.MaxValue, ErrorMessage = "Giá bán không được âm.")]
        public decimal RetailPrice { get; set; }
        
        /// <example>15</example>
        public int StockQuantity { get; set; }

        /// <example>https://example.com/iphone15.jpg</example>
        public string ImageUrl { get; set; }
    }
}
