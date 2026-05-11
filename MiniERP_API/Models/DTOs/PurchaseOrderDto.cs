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
        public int? SupplierId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public string Notes { get; set; }
        public List<CreatePurchaseOrderItemDto> Items { get; set; }
    }

    public class CreatePurchaseOrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
