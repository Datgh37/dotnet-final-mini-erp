using System.Collections.Generic;
using MiniERP_API.Models.Entities;

namespace MiniERP_API.Repositories.Interfaces
{
    public interface ISalesOrderRepository
    {
        IEnumerable<SalesOrder> GetAll(string status = null, int? customerId = null, System.DateTime? fromDate = null, System.DateTime? toDate = null);
        SalesOrder GetById(int id);
        int CreateOrder(SalesOrder order);
        void UpdateStatus(int id, string status);
        SalesOrder GetByNumber(string orderNumber);
    }
}
