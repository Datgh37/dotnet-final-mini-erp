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
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IPurchaseOrderRepository _repo;
        private readonly IMapper _mapper;
        public PurchaseOrderService(IPurchaseOrderRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

        public IEnumerable<PurchaseOrderDto> GetAll() => _mapper.Map<IEnumerable<PurchaseOrderDto>>(_repo.GetAll());
        public PurchaseOrderDto GetById(int id) => _mapper.Map<PurchaseOrderDto>(_repo.GetById(id));

        public int CreateOrder(CreatePurchaseOrderDto dto)
        {
            var order = _mapper.Map<PurchaseOrder>(dto);
            order.PONumber = "PO-" + DateTime.Now.Ticks.ToString().Substring(10);
            if (dto.Items != null)
            {
                order.TotalAmount = dto.Items.Sum(i => i.UnitPrice * i.Quantity);
            }
            else
            {
                order.TotalAmount = 0;
            }
            return _repo.CreateOrder(order);
        }

        public void ReceiveOrder(int id, DateTime receivedDate, int receivedBy) => _repo.ReceiveOrder(id, receivedDate, receivedBy);
        public void CancelOrder(int id) => _repo.CancelOrder(id);
    }
}
