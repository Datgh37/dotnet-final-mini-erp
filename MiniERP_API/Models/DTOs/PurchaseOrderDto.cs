using System;
using System.Collections.Generic;

namespace MiniERP_API.Models.DTOs
{
    public class PurchaseOrderDto
    {
        public int Id { get; set; }
        public string PONumber { get; set; }
        public int? SupplierId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string Notes { get; set; }
        public List<PurchaseOrderItemDto> Items { get; set; }
    }

    public class PurchaseOrderItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class CreatePurchaseOrderDto
    {
        /// <example>1</example>
        public int? SupplierId { get; set; }
        
        /// <example>2026-05-15T00:00:00Z</example>
        public DateTime? OrderDate { get; set; }
        
        /// <example>2026-05-20T00:00:00Z</example>
        public DateTime? ExpectedDate { get; set; }
        
        /// <example>Nhập hàng bổ sung kho tháng 5</example>
        public string Notes { get; set; }
        
        public List<CreatePurchaseOrderItemDto> Items { get; set; }
    }

    public class CreatePurchaseOrderItemDto
    {
        /// <example>1</example>
        public int ProductId { get; set; }
        
        /// <example>50</example>
        public int Quantity { get; set; }
        
        /// <example>85000</example>
        public decimal UnitPrice { get; set; }
    }
}
