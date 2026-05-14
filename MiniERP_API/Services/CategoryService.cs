using System.Collections.Generic;
using AutoMapper;
using MiniERP_API.Models.DTOs;
using MiniERP_API.Models.Entities;
using MiniERP_API.Repositories.Interfaces;
using MiniERP_API.Services.Interfaces;

namespace MiniERP_API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

        public IEnumerable<CategoryDto> GetAll() => _mapper.Map<IEnumerable<CategoryDto>>(_repo.GetAll());
        public CategoryDto GetById(int id) => _mapper.Map<CategoryDto>(_repo.GetById(id));
        public int Create(CategoryCreateUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name)) throw new System.Exception("Tên danh mục không được để trống.");
            
            var existing = _repo.GetByName(dto.Name);
            if (existing != null) throw new System.Exception($"Tên danh mục '{dto.Name}' đã tồn tại.");

            var category = _mapper.Map<ProductCategory>(dto);
            return _repo.Add(category);
        }
        public void Update(int id, CategoryCreateUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name)) throw new System.Exception("Tên danh mục không được để trống.");
            
            var existing = _repo.GetById(id);
            if (existing == null) throw new System.Exception("Danh mục không tồn tại.");

            if (existing.Name != dto.Name)
            {
                var duplicate = _repo.GetByName(dto.Name);
                if (duplicate != null) throw new System.Exception($"Tên danh mục '{dto.Name}' đã bị trùng.");
            }

            _mapper.Map(dto, existing);
            existing.Id = id;
            _repo.Update(existing);
        }
        public void Delete(int id) => _repo.Delete(id);
    }
}
