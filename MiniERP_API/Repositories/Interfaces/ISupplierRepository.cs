using System.Collections.Generic;
using MiniERP_API.Models.Entities;

namespace MiniERP_API.Repositories.Interfaces
{
    public interface ISupplierRepository
    {
        IEnumerable<Supplier> GetAll();
        Supplier GetById(int id);
        int Add(Supplier supplier);
        void Update(Supplier supplier);
        void Delete(int id);
        Supplier GetByName(string name);
        IEnumerable<Supplier> Search(string searchTerm);
    }
}
