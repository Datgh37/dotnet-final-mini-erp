using System.Collections.Generic;
using MiniERP_API.Models.Entities;

namespace MiniERP_API.Repositories.Interfaces
{
    public interface ISalesOrderRepository
    {
        IEnumerable<SalesOrder> GetAll();
        SalesOrder GetById(int id);
        int CreateOrder(SalesOrder order);
        void UpdateStatus(int id, string status);
    }
}
