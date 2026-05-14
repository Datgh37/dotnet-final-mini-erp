using System.Collections.Generic;
using MiniERP_API.Models.Entities;

namespace MiniERP_API.Repositories.Interfaces
{
    public interface IBrandRepository
    {
        IEnumerable<Brand> GetAll();
        Brand GetById(int id);
        int Add(Brand brand);
        void Update(Brand brand);
        void Delete(int id);
        Brand GetByName(string name);
    }
}
