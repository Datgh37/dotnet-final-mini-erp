using System.Collections.Generic;
using MiniERP_API.Models.DTOs;
using MiniERP_API.Models.Entities;

namespace MiniERP_API.Services.Interfaces
{
    public interface IProductService
    {
        IEnumerable<ProductDto> GetActiveProducts(string searchTerm = null, int? categoryId = null, int? brandId = null);
        ProductDto GetProduct(int id);
        int CreateProduct(ProductCreateUpdateDto dto);
        void UpdateProduct(int id, ProductCreateUpdateDto dto);
        void DeleteProduct(int id);
    }
}
