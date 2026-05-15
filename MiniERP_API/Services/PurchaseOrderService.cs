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
        private readonly ISupplierRepository _supplierRepo;
        private readonly IProductRepository _productRepo;
        private readonly IMapper _mapper;

        public PurchaseOrderService(IPurchaseOrderRepository repo, 
                                   ISupplierRepository supplierRepo,
                                   IProductRepository productRepo,
                                   IMapper mapper) 
        { 
            _repo = repo; 
            _supplierRepo = supplierRepo;
            _productRepo = productRepo;
            _mapper = mapper; 
        }

        public IEnumerable<PurchaseOrderDto> GetAll(string status = null) 
        {
            var orders = _repo.GetAll(status);
            var dtos = _mapper.Map<IEnumerable<PurchaseOrderDto>>(orders);
            foreach (var d in dtos)
            {
                if (d.SupplierId.HasValue)
                {
                    var sup = _supplierRepo.GetById(d.SupplierId.Value);
                    d.SupplierName = sup?.Name ?? "N/A";
                }
            }
            return dtos;
        }

        public PurchaseOrderDto GetById(int id) 
        {
            var order = _repo.GetById(id);
            if (order == null) return null;
            var dto = _mapper.Map<PurchaseOrderDto>(order);

            if (dto.SupplierId.HasValue)
            {
                var sup = _supplierRepo.GetById(dto.SupplierId.Value);
                dto.SupplierName = sup?.Name ?? "N/A";
            }

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

        public int CreateOrder(CreatePurchaseOrderDto dto)
        {
            if (dto.Items == null || !dto.Items.Any())
                throw new System.Exception("Đơn hàng phải có ít nhất một sản phẩm.");

            if (dto.Items.Any(i => i.Quantity <= 0))
                throw new System.Exception("Số lượng sản phẩm phải lớn hơn 0.");

            if (dto.Items.Any(i => i.UnitPrice < 0))
                throw new System.Exception("Đơn giá không được âm.");

            if (dto.OrderDate.HasValue && dto.OrderDate.Value.Date < DateTime.Now.Date)
                throw new System.Exception("Ngày đặt hàng không được ở quá khứ.");

            if (dto.ExpectedDate.HasValue && dto.ExpectedDate.Value.Date < DateTime.Now.Date)
                throw new System.Exception("Ngày dự kiến nhận không được ở quá khứ.");

            if (!dto.OrderDate.HasValue) dto.OrderDate = DateTime.Now;

            var order = _mapper.Map<PurchaseOrder>(dto);
            order.PONumber = "PO-" + DateTime.Now.Ticks.ToString().Substring(10);
            
            // Safety check for auto-generated PONumber
            var existing = _repo.GetByNumber(order.PONumber);
            if (existing != null) order.PONumber += "-R"; // Simple collision avoidance

            order.OrderDate = dto.OrderDate.Value;
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
