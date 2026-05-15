using System;
using System.Collections.Generic;

namespace MiniERP_API.Models.DTOs
{
    public class SalesOrderDto
    {
        /// <example>1</example>
        public int Id { get; set; }
        
        /// <example>SO-12345678</example>
        public string OrderNumber { get; set; }
        
        /// <example>1</example>
        public int? CustomerId { get; set; }
        
        /// <example>Nguyen Van An</example>
        public string CustomerName { get; set; }
        
        /// <example>2026-05-14T10:00:00Z</example>
        public DateTime OrderDate { get; set; }
        
        /// <example>NEW</example>
        public string Status { get; set; }
        
        /// <example>CASH</example>
        public string PaymentMethod { get; set; }
        
        /// <example>PENDING</example>
        public string PaymentStatus { get; set; }
        
        /// <example>2500000</example>
        public decimal TotalAmount { get; set; }
        
        /// <example>123 Nguyen Trai, Q.5, TP.HCM</example>
        public string ShippingAddress { get; set; }
        
        /// <example>Giao hàng giờ hành chính</example>
        public string Notes { get; set; }
        
        public List<SalesOrderItemDto> Items { get; set; }
    }

    public class SalesOrderItemDto
    {
        /// <example>1</example>
        public int Id { get; set; }
        
        /// <example>1</example>
        public int ProductId { get; set; }
        
        /// <example>iPhone 15 Pro</example>
        public string ProductName { get; set; }
        
        /// <example>2</example>
        public int Quantity { get; set; }
        
        /// <example>1250000</example>
        public decimal UnitPrice { get; set; }
        
        /// <example>0</example>
        public decimal Discount { get; set; }
    }
}
