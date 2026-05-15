using System.Collections.Generic;
using AutoMapper;
using MiniERP_API.Models.DTOs;
using MiniERP_API.Repositories.Interfaces;
using MiniERP_API.Services.Interfaces;

namespace MiniERP_API.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _repo;
        private readonly IProductRepository _productRepo;
        private readonly IMapper _mapper;
        public InventoryService(IInventoryRepository repo, IProductRepository productRepo, IMapper mapper) 
        { 
            _repo = repo; 
            _productRepo = productRepo;
            _mapper = mapper; 
        }

        public IEnumerable<StockMovementDto> GetMovements(int? productId, string movementType)
        {
            var movements = _repo.GetMovements(productId, movementType);
            var dtos = _mapper.Map<IEnumerable<StockMovementDto>>(movements);
            
            foreach (var d in dtos)
            {
                var prod = _productRepo.GetById(d.ProductId);
                if (prod != null)
                {
                    d.ProductName = prod.Name;
                    d.SKU = prod.SKU;
                }
                else
                {
                    d.ProductName = $"SP-{d.ProductId}";
                    d.SKU = "N/A";
                }
            }
            
            return dtos;
        }

        public void AdjustStock(StockAdjustDto dto, int? userId)
        {
            if (dto.Quantity == 0) throw new System.Exception("Số lượng điều chỉnh phải khác 0.");
            if (string.IsNullOrWhiteSpace(dto.Reason)) throw new System.Exception("Lý do điều chỉnh không được để trống.");
            _repo.AdjustStock(dto.ProductId, dto.Quantity, dto.Reason, userId);
        }
    }
}
