using System.Collections.Generic;
using MiniERP_API.Models.DTOs;
using MiniERP_API.Models.Entities;

namespace MiniERP_API.Services.Interfaces
{
    public interface IBrandService
    {
        IEnumerable<BrandDto> GetAll();
        BrandDto GetById(int id);
        int Create(BrandCreateUpdateDto dto);
        void Update(int id, BrandCreateUpdateDto dto);
        void Delete(int id);
    }
}
