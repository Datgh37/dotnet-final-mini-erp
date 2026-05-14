using System.Collections.Generic;
using MiniERP_API.Models.Entities;

namespace MiniERP_API.Repositories.Interfaces
{
    public interface IPurchaseOrderRepository
    {
        IEnumerable<PurchaseOrder> GetAll(string status = null);
        PurchaseOrder GetById(int id);
        int CreateOrder(PurchaseOrder order);
        void ReceiveOrder(int id, DateTime receivedDate, int receivedBy);
        void CancelOrder(int id);
        PurchaseOrder GetByNumber(string poNumber);
    }
}
