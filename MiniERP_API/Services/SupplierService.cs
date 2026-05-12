using System.Collections.Generic;
using AutoMapper;
using MiniERP_API.Models.Entities;
using MiniERP_API.Models.DTOs;
using MiniERP_API.Repositories.Interfaces;
using MiniERP_API.Services.Interfaces;

namespace MiniERP_API.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _repo;
        private readonly IMapper _mapper;

        public SupplierService(ISupplierRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public IEnumerable<SupplierDto> GetAll()
        {
            var suppliers = _repo.GetAll();
            return _mapper.Map<IEnumerable<SupplierDto>>(suppliers);
        }

        public SupplierDto GetById(int id)
        {
            var supplier = _repo.GetById(id);
            return _mapper.Map<SupplierDto>(supplier);
        }

        public int Create(Supplier s) => _repo.Add(s);
        public void Update(Supplier s) => _repo.Update(s);
        public void Delete(int id) => _repo.Delete(id);
    }
}
