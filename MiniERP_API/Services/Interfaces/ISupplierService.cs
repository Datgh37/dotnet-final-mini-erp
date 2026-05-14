using System.Collections.Generic;
using MiniERP_API.Models.Entities;
using MiniERP_API.Models.DTOs;

namespace MiniERP_API.Services.Interfaces
{
    public interface ISupplierService
    {
        IEnumerable<SupplierDto> GetAll(string searchTerm = null);
        SupplierDto GetById(int id);
        int Create(SupplierCreateUpdateDto dto);
        void Update(int id, SupplierCreateUpdateDto dto);
        void Delete(int id);
    }
}
