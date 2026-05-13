using System;

namespace MiniERP_API.Models.DTOs
{
    public class StockMovementDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string MovementType { get; set; }
        public int Quantity { get; set; }
        public string Reference { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
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
