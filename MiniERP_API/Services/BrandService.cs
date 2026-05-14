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
        public int Create(BrandCreateUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name)) throw new System.Exception("Tên nhãn hàng không được để trống.");
            
            var existing = _repo.GetByName(dto.Name);
            if (existing != null) throw new System.Exception($"Tên nhãn hàng '{dto.Name}' đã tồn tại.");

            var brand = _mapper.Map<Brand>(dto);
            return _repo.Add(brand);
        }

        public void Update(int id, BrandCreateUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name)) throw new System.Exception("Tên nhãn hàng không được để trống.");
            
            var existing = _repo.GetById(id);
            if (existing == null) throw new System.Exception("Nhãn hàng không tồn tại.");

            if (existing.Name != dto.Name)
            {
                var duplicate = _repo.GetByName(dto.Name);
                if (duplicate != null) throw new System.Exception($"Tên nhãn hàng '{dto.Name}' đã bị trùng.");
            }

            _mapper.Map(dto, existing);
            existing.Id = id;
            _repo.Update(existing);
        }
        public void Delete(int id) => _repo.Delete(id);
    }
}
