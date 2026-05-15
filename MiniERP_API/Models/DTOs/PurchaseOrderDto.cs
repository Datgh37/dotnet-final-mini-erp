using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MiniERP_API.Models.DTOs
{
    public class PurchaseOrderDto
    {
        /// <example>1</example>
        public int Id { get; set; }
        
        /// <example>PO-87654321</example>
        public string PONumber { get; set; }
        
        /// <example>1</example>
        public int? SupplierId { get; set; }
        
        /// <example>Cong ty The Gioi So</example>
        public string SupplierName { get; set; }
        
        /// <example>2026-05-14T10:00:00Z</example>
        public DateTime? OrderDate { get; set; }
        
        /// <example>2026-05-20T10:00:00Z</example>
        public DateTime? ExpectedDate { get; set; }
        
        /// <example>2026-05-14T10:00:00Z</example>
        public DateTime? ReceivedDate { get; set; }
        
        /// <example>RECEIVED</example>
        public string Status { get; set; }
        
        /// <example>4250000</example>
        public decimal TotalAmount { get; set; }
        
        /// <example>Nhập hàng bổ sung kho tháng 5</example>
        public string Notes { get; set; }
        
        public List<PurchaseOrderItemDto> Items { get; set; }
    }

    public class PurchaseOrderItemDto
    {
        /// <example>1</example>
        public int Id { get; set; }
        
        /// <example>1</example>
        public int ProductId { get; set; }
        
        /// <example>iPhone 15 Pro</example>
        public string ProductName { get; set; }
        
        /// <example>50</example>
        public int Quantity { get; set; }
        
        /// <example>85000</example>
        public decimal UnitPrice { get; set; }
    }

    public class CreatePurchaseOrderDto
    {
        /// <example>1</example>
        [Required(ErrorMessage = "Nhà cung cấp là bắt buộc.")]
        public int? SupplierId { get; set; }
        
        /// <example>2026-05-15T00:00:00Z</example>
        public DateTime? OrderDate { get; set; }
        
        /// <example>2026-05-20T00:00:00Z</example>
        public DateTime? ExpectedDate { get; set; }
        
        /// <example>Nhập hàng bổ sung kho tháng 5</example>
        public string Notes { get; set; }
        
        [Required(ErrorMessage = "Danh sách sản phẩm không được để trống.")]
        public List<CreatePurchaseOrderItemDto> Items { get; set; }
    }

    public class CreatePurchaseOrderItemDto
    {
        /// <example>1</example>
        [Required(ErrorMessage = "Sản phẩm là bắt buộc.")]
        public int ProductId { get; set; }
        
        /// <example>50</example>
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải ít nhất là 1.")]
        public int Quantity { get; set; }
        
        /// <example>85000</example>
        [Range(0, double.MaxValue, ErrorMessage = "Đơn giá không được âm.")]
        public decimal UnitPrice { get; set; }
    }
}
