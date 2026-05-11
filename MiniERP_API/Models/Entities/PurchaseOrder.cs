using System;
using System.Collections.Generic;

namespace MiniERP_API.Models.Entities
{
    public class PurchaseOrder : BaseEntity
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
        public int? CreatedBy { get; set; }
        public List<PurchaseOrderItem> Items { get; set; } = new List<PurchaseOrderItem>();
    }
}
