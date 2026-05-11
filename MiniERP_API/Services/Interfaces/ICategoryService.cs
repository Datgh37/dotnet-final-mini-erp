using System.Collections.Generic;
using MiniERP_API.Models.DTOs;
using MiniERP_API.Models.Entities;

namespace MiniERP_API.Services.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<CategoryDto> GetAll();
        CategoryDto GetById(int id);
        int Create(ProductCategory category);
        void Update(ProductCategory category);
        void Delete(int id);
    }
}
