using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MiniERP_API.Models.DTOs;
using MiniERP_API.Models.Entities;
using MiniERP_API.Repositories.Interfaces;
using MiniERP_API.Services.Interfaces;

namespace MiniERP_API.Services
{
    public class SalesOrderService : ISalesOrderService
    {
        private readonly ISalesOrderRepository _orderRepo;
        private readonly ICustomerRepository _customerRepo;
        private readonly IProductRepository _productRepo;
        private readonly IInventoryService _inventoryService;
        private readonly IMapper _mapper;

        public SalesOrderService(ISalesOrderRepository orderRepo, 
                                ICustomerRepository customerRepo,
                                IProductRepository productRepo,
                                IInventoryService inventoryService,
                                IMapper mapper)
        {
            _orderRepo = orderRepo;
            _customerRepo = customerRepo;
            _productRepo = productRepo;
            _inventoryService = inventoryService;
            _mapper = mapper;
        }

        public IEnumerable<SalesOrderDto> GetAll(string status = null, int? customerId = null, System.DateTime? fromDate = null, System.DateTime? toDate = null) 
        {
            var orders = _orderRepo.GetAll(status, customerId, fromDate, toDate);
            var dtos = _mapper.Map<IEnumerable<SalesOrderDto>>(orders);
            foreach (var d in dtos)
            {
                if (d.CustomerId.HasValue)
                {
                    var cust = _customerRepo.GetById(d.CustomerId.Value);
                    d.CustomerName = cust?.Name ?? "Khách lẻ";
                }
                else d.CustomerName = "Khách lẻ";
            }
            return dtos;
        }

        public SalesOrderDto GetById(int id) 
        {
            var order = _orderRepo.GetById(id);
            if (order == null) return null;
            var dto = _mapper.Map<SalesOrderDto>(order);
            
            if (dto.CustomerId.HasValue)
            {
                var cust = _customerRepo.GetById(dto.CustomerId.Value);
                dto.CustomerName = cust?.Name ?? "Khách lẻ";
            }
            else dto.CustomerName = "Khách lẻ";

            if (dto.Items != null)
            {
                foreach (var item in dto.Items)
                {
                    var prod = _productRepo.GetById(item.ProductId);
                    item.ProductName = prod?.Name ?? "Sản phẩm không tồn tại";
                }
            }
            
            return dto;
        }

        public int PlaceOrder(CreateSalesOrderDto dto)
        {
            if (dto.Items == null || !dto.Items.Any())
                throw new System.Exception("Đơn hàng phải có ít nhất một sản phẩm.");

            if (dto.Items.Any(i => i.Quantity <= 0))
                throw new System.Exception("Số lượng sản phẩm phải lớn hơn 0.");

            if (dto.Items.Any(i => i.UnitPrice < 0 || i.Discount < 0))
                throw new System.Exception("Giá và chiết khấu không được âm.");

            if (dto.OrderDate.HasValue && dto.OrderDate.Value.Date < DateTime.Now.Date)
                throw new System.Exception("Ngày đặt hàng không được ở quá khứ.");

            var order = _mapper.Map<SalesOrder>(dto);
            order.OrderNumber = "SO-" + DateTime.Now.Ticks.ToString().Substring(10);
            
            // Safety check for auto-generated OrderNumber
            var existing = _orderRepo.GetByNumber(order.OrderNumber);
            if (existing != null) order.OrderNumber += "-R"; // Simple collision avoidance

            order.OrderDate = dto.OrderDate ?? DateTime.Now;
            order.TotalAmount = dto.Items.Sum(i => (i.UnitPrice - i.Discount) * i.Quantity);

            return _orderRepo.CreateOrder(order);
        }

        public void UpdateStatus(int id, string status)
        {
            var order = _orderRepo.GetById(id);
            if (order == null) throw new System.Exception("Đơn hàng không tồn tại.");

            string oldStatus = order.Status.ToUpper();
            string newStatus = status.ToUpper();

            if (oldStatus == newStatus) return;

            var validStatuses = new[] { "NEW", "PROCESSING", "SHIPPED", "COMPLETED", "CANCELLED" };
            if (!validStatuses.Contains(newStatus))
                throw new System.Exception("Trạng thái đơn hàng không hợp lệ.");

            // 1. Stock Deduction Logic (OUT)
            // If moving from a "Not Delivered" state to a "Delivered" state
            bool wasDelivered = oldStatus == "SHIPPED" || oldStatus == "COMPLETED";
            bool isDelivered = newStatus == "SHIPPED" || newStatus == "COMPLETED";

            if (!wasDelivered && isDelivered)
            {
                // Deduct stock
                foreach (var item in order.Items)
                {
                    _inventoryService.AdjustStock(new StockAdjustDto {
                        ProductId = item.ProductId,
                        Quantity = -item.Quantity, // Negative for OUT
                        Reason = $"Xuất kho cho đơn hàng {order.OrderNumber}"
                    }, null);
                }
            }
            // 2. Stock Revert Logic (IN)
            // If moving from a "Delivered" state to a "Cancelled" or "Pending" state
            else if (wasDelivered && (newStatus == "CANCELLED" || newStatus == "NEW" || newStatus == "PROCESSING"))
            {
                // Return stock
                foreach (var item in order.Items)
                {
                    _inventoryService.AdjustStock(new StockAdjustDto {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity, // Positive for IN
                        Reason = $"Hoàn kho do hủy/thay đổi đơn hàng {order.OrderNumber}"
                    }, null);
                }
            }

            _orderRepo.UpdateStatus(id, newStatus);
        }
    }
}
