using System;

namespace MiniERP_API.Models.Entities
{
    public class StockMovement : BaseEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string MovementType { get; set; } // IN, OUT, ADJUSTMENT, RETURN
        public int Quantity { get; set; }
        public string Reference { get; set; }
        public int? CreatedBy { get; set; }
    }
}
