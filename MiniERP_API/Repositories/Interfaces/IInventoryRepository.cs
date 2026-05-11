using System.Collections.Generic;
using MiniERP_API.Models.Entities;

namespace MiniERP_API.Repositories.Interfaces
{
    public interface IInventoryRepository
    {
        IEnumerable<StockMovement> GetMovements(int? productId, string movementType);
        void AdjustStock(int productId, int quantity, string reason, int? createdBy);
    }
}
