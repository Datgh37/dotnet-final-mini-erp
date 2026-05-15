using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MiniERP_API.Models.DTOs
{
    public class CreateSalesOrderDto
    {
        /// <example>1</example>
        [Required(ErrorMessage = "Khách hàng là bắt buộc.")]
        public int? CustomerId { get; set; }
        
        /// <example>2026-05-14T10:00:00Z</example>
        public System.DateTime? OrderDate { get; set; }
        
        /// <example>CASH</example>
        public string PaymentMethod { get; set; }
        
        /// <example>123 Nguyen Trai, Q.5, TP.HCM</example>
        public string ShippingAddress { get; set; }
        
        /// <example>Giao hàng giờ hành chính</example>
        public string Notes { get; set; }
        
        [Required(ErrorMessage = "Danh sách sản phẩm không được để trống.")]
        public List<CreateSalesOrderItemDto> Items { get; set; }
    }
}
