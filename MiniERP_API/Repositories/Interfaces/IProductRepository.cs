using System.Collections.Generic;
using MiniERP_API.Models.Entities;

namespace MiniERP_API.Repositories.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        Product GetById(int id);
        int Add(Product product);
        void Update(Product product);
        void Delete(int id);
        Product GetBySku(string sku);
        IEnumerable<Product> Search(string searchTerm, int? categoryId, int? brandId);
    }
}
