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
        public int Create(ProductCategory category) => _repo.Add(category);
        public void Update(ProductCategory category) => _repo.Update(category);
        public void Delete(int id) => _repo.Delete(id);
    }
}
