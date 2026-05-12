using System.Collections.Generic;
using MiniERP_API.Models.Entities;
using MiniERP_API.Models.DTOs;

namespace MiniERP_API.Services.Interfaces
{
    public interface ISupplierService
    {
        IEnumerable<SupplierDto> GetAll();
        SupplierDto GetById(int id);
        int Create(Supplier s);
        void Update(Supplier s);
        void Delete(int id);
    }
}
