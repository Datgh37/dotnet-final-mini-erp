using System.Collections.Generic;
using AutoMapper;
using MiniERP_API.Models.DTOs;
using MiniERP_API.Models.Entities;
using MiniERP_API.Repositories.Interfaces;
using MiniERP_API.Services.Interfaces;

namespace MiniERP_API.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _repo;
        private readonly IMapper _mapper;
        public BrandService(IBrandRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

        public IEnumerable<BrandDto> GetAll() => _mapper.Map<IEnumerable<BrandDto>>(_repo.GetAll());
        public BrandDto GetById(int id) => _mapper.Map<BrandDto>(_repo.GetById(id));
        public int Create(Brand brand)
        {
            if (string.IsNullOrWhiteSpace(brand.Name)) throw new System.Exception("Tên nhãn hàng không được để trống.");
            return _repo.Add(brand);
        }
        public void Update(Brand brand)
        {
            if (string.IsNullOrWhiteSpace(brand.Name)) throw new System.Exception("Tên nhãn hàng không được để trống.");
            _repo.Update(brand);
        }
        public void Delete(int id) => _repo.Delete(id);
    }
}
