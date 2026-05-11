using System;
using System.Collections.Generic;
using MiniERP_API.Models.DTOs;

namespace MiniERP_API.Services.Interfaces
{
    public interface IPurchaseOrderService
    {
        IEnumerable<PurchaseOrderDto> GetAll();
        PurchaseOrderDto GetById(int id);
        int CreateOrder(CreatePurchaseOrderDto dto);
        void ReceiveOrder(int id, DateTime receivedDate, int receivedBy);
        void CancelOrder(int id);
    }
}
