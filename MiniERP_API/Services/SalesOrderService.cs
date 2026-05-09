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

        public IEnumerable<SalesOrderDto> GetAll() => _mapper.Map<IEnumerable<SalesOrderDto>>(_orderRepo.GetAll());
        public SalesOrderDto GetById(int id) => _mapper.Map<SalesOrderDto>(_orderRepo.GetById(id));

        public int PlaceOrder(CreateSalesOrderDto dto)
        {
            var order = _mapper.Map<SalesOrder>(dto);
            order.OrderNumber = "SO-" + DateTime.Now.Ticks.ToString().Substring(10);
            order.TotalAmount = dto.Items.Sum(i => (i.UnitPrice - i.Discount) * i.Quantity);
            return _orderRepo.CreateOrder(order);
        }

        public void UpdateStatus(int id, string status) => _orderRepo.UpdateStatus(id, status);
    }
}
