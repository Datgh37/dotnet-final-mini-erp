using System.Collections.Generic;
using MiniERP_API.Models.DTOs;

namespace MiniERP_API.Services.Interfaces
{
    public interface ISalesOrderService
    {
        IEnumerable<SalesOrderDto> GetAll(string status = null, int? customerId = null, System.DateTime? fromDate = null, System.DateTime? toDate = null);
        SalesOrderDto GetById(int id);
        int PlaceOrder(CreateSalesOrderDto dto);
        void UpdateStatus(int id, string status);
    }
}
