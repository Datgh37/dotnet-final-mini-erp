using System.Collections.Generic;

namespace MiniERP_API.Models.DTOs
{
    public class CreateSalesOrderDto
    {
        /// <example>1</example>
        public int? CustomerId { get; set; }
        
        /// <example>CASH</example>
        public string PaymentMethod { get; set; }
        
        /// <example>123 Nguyen Trai, Q.5, TP.HCM</example>
        public string ShippingAddress { get; set; }
        
        /// <example>Giao hàng giờ hành chính</example>
        public string Notes { get; set; }
        
        public List<CreateSalesOrderItemDto> Items { get; set; }
    }
}
