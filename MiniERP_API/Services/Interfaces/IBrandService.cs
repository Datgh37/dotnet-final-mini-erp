using System.Collections.Generic;
using MiniERP_API.Models.DTOs;
using MiniERP_API.Models.Entities;

namespace MiniERP_API.Services.Interfaces
{
    public interface IBrandService
    {
        IEnumerable<BrandDto> GetAll();
        BrandDto GetById(int id);
        int Create(Brand brand);
        void Update(Brand brand);
        void Delete(int id);
    }
}
