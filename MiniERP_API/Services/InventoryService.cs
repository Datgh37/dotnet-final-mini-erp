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
        private readonly IMapper _mapper;
        public InventoryService(IInventoryRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

        public IEnumerable<StockMovementDto> GetMovements(int? productId, string movementType)
            => _mapper.Map<IEnumerable<StockMovementDto>>(_repo.GetMovements(productId, movementType));

        public void AdjustStock(StockAdjustDto dto, int? userId)
        {
            if (dto.Quantity == 0) throw new System.Exception("Số lượng điều chỉnh phải khác 0.");
            if (string.IsNullOrWhiteSpace(dto.Reason)) throw new System.Exception("Lý do điều chỉnh không được để trống.");
            _repo.AdjustStock(dto.ProductId, dto.Quantity, dto.Reason, userId);
        }
    }
}
