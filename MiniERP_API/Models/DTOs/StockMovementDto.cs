using System;

namespace MiniERP_API.Models.DTOs
{
    public class StockMovementDto
    {
        /// <example>101</example>
        public int Id { get; set; }
        
        /// <example>1</example>
        public int ProductId { get; set; }
        
        /// <example>IN</example>
        public string MovementType { get; set; }
        
        /// <example>50</example>
        public int Quantity { get; set; }
        
        /// <example>PO-20260514</example>
        public string Reference { get; set; }
        
        /// <example>2026-05-14T10:00:00Z</example>
        public DateTimeOffset CreatedAt { get; set; }

        /// <example>iPhone 15 Pro Max</example>
        public string ProductName { get; set; }

        /// <example>IP15PM</example>
        public string SKU { get; set; }
    }

    public class StockAdjustDto
    {
        /// <example>1</example>
        public int ProductId { get; set; }
        
        /// <example>10</example>
        public int Quantity { get; set; } // Số lượng điều chỉnh (+ hoặc -)
        
        /// <example>Kiểm kho định kỳ</example>
        public string Reason { get; set; }
    }
}
