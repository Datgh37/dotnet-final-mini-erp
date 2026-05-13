using System.Collections.Generic;
using MiniERP_API.Models.DTOs;
using MiniERP_API.Models.Entities;

namespace MiniERP_API.Services.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<CategoryDto> GetAll();
        CategoryDto GetById(int id);
        int Create(CategoryCreateUpdateDto dto);
        void Update(int id, CategoryCreateUpdateDto dto);
        void Delete(int id);
    }
}
