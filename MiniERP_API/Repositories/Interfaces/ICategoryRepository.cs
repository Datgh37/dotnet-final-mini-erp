using System.Collections.Generic;
using MiniERP_API.Models.Entities;

namespace MiniERP_API.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        IEnumerable<ProductCategory> GetAll();
        ProductCategory GetById(int id);
        int Add(ProductCategory category);
        void Update(ProductCategory category);
        void Delete(int id);
        ProductCategory GetByName(string name);
    }
}
