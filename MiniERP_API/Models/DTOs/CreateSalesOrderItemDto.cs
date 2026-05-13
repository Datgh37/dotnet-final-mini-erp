namespace MiniERP_API.Models.DTOs
{
    public class CreateSalesOrderItemDto
    {
        /// <example>1</example>
        public int ProductId { get; set; }
        
        /// <example>2</example>
        public int Quantity { get; set; }
        
        /// <example>150000</example>
        public decimal UnitPrice { get; set; }
        
        /// <example>10000</example>
        public decimal Discount { get; set; }
    }
}
