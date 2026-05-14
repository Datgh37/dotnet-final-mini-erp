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

        public IEnumerable<PurchaseOrderDto> GetAll(string status = null) 
            => _mapper.Map<IEnumerable<PurchaseOrderDto>>(_repo.GetAll(status));
        public PurchaseOrderDto GetById(int id) => _mapper.Map<PurchaseOrderDto>(_repo.GetById(id));

        public int CreateOrder(CreatePurchaseOrderDto dto)
        {
            if (dto.Items == null || !dto.Items.Any())
                throw new System.Exception("Đơn hàng phải có ít nhất một sản phẩm.");

            if (dto.Items.Any(i => i.Quantity <= 0))
                throw new System.Exception("Số lượng sản phẩm phải lớn hơn 0.");

            if (dto.Items.Any(i => i.UnitPrice < 0))
                throw new System.Exception("Đơn giá không được âm.");

            var order = _mapper.Map<PurchaseOrder>(dto);
            order.PONumber = "PO-" + DateTime.Now.Ticks.ToString().Substring(10);
            
            // Safety check for auto-generated PONumber
            var existing = _repo.GetByNumber(order.PONumber);
            if (existing != null) order.PONumber += "-R"; // Simple collision avoidance

            order.TotalAmount = dto.Items.Sum(i => i.UnitPrice * i.Quantity);

            return _repo.CreateOrder(order);
        }

        public void ReceiveOrder(int id, DateTime receivedDate, int receivedBy)
        {
            var order = _repo.GetById(id);
            if (order == null) throw new System.Exception("Đơn mua hàng không tồn tại.");
            if (order.Status != "PENDING") throw new System.Exception("Đơn mua hàng đã được xử lý hoặc hủy.");

            _repo.ReceiveOrder(id, receivedDate, receivedBy);
        }

        public void CancelOrder(int id)
        {
            var order = _repo.GetById(id);
            if (order == null) throw new System.Exception("Đơn mua hàng không tồn tại.");
            if (order.Status != "PENDING") throw new System.Exception("Chỉ có thể hủy đơn đang ở trạng thái PENDING.");

            _repo.CancelOrder(id);
        }
    }
}
