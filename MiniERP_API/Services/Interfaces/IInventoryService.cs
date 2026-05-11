using System.Collections.Generic;
using MiniERP_API.Models.DTOs;

namespace MiniERP_API.Services.Interfaces
{
    public interface IInventoryService
    {
        IEnumerable<StockMovementDto> GetMovements(int? productId, string movementType);
        void AdjustStock(StockAdjustDto dto, int? userId);
    }
}
