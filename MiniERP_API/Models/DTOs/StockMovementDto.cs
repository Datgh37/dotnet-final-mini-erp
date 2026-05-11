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
        public int ProductId { get; set; }
        public int Quantity { get; set; } // Số lượng điều chỉnh (+ hoặc -)
        public string Reason { get; set; }
    }
}
