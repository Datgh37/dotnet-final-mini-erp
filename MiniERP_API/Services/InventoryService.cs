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
            => _repo.AdjustStock(dto.ProductId, dto.Quantity, dto.Reason, userId);
    }
}
