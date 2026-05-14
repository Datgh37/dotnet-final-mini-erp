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
        private readonly IMapper _mapper;

        public SalesOrderService(ISalesOrderRepository orderRepo, IMapper mapper)
        {
            _orderRepo = orderRepo;
            _mapper = mapper;
        }

        public IEnumerable<SalesOrderDto> GetAll(string status = null, int? customerId = null, System.DateTime? fromDate = null, System.DateTime? toDate = null) 
            => _mapper.Map<IEnumerable<SalesOrderDto>>(_orderRepo.GetAll(status, customerId, fromDate, toDate));
        public SalesOrderDto GetById(int id) => _mapper.Map<SalesOrderDto>(_orderRepo.GetById(id));

        public int PlaceOrder(CreateSalesOrderDto dto)
        {
            if (dto.Items == null || !dto.Items.Any())
                throw new System.Exception("Đơn hàng phải có ít nhất một sản phẩm.");

            if (dto.Items.Any(i => i.Quantity <= 0))
                throw new System.Exception("Số lượng sản phẩm phải lớn hơn 0.");

            if (dto.Items.Any(i => i.UnitPrice < 0 || i.Discount < 0))
                throw new System.Exception("Giá và chiết khấu không được âm.");

            var order = _mapper.Map<SalesOrder>(dto);
            order.OrderNumber = "SO-" + DateTime.Now.Ticks.ToString().Substring(10);
            
            // Safety check for auto-generated OrderNumber
            var existing = _orderRepo.GetByNumber(order.OrderNumber);
            if (existing != null) order.OrderNumber += "-R"; // Simple collision avoidance

            order.TotalAmount = dto.Items.Sum(i => (i.UnitPrice - i.Discount) * i.Quantity);

            return _orderRepo.CreateOrder(order);
        }

        public void UpdateStatus(int id, string status)
        {
            var order = _orderRepo.GetById(id);
            if (order == null) throw new System.Exception("Đơn hàng không tồn tại.");

            var validStatuses = new[] { "NEW", "PROCESSING", "SHIPPED", "COMPLETED", "CANCELLED" };
            if (!validStatuses.Contains(status.ToUpper()))
                throw new System.Exception("Trạng thái đơn hàng không hợp lệ.");

            _orderRepo.UpdateStatus(id, status.ToUpper());
        }
    }
}
