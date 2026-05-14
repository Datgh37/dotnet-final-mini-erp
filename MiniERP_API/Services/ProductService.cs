using System.Collections.Generic;
using AutoMapper;
using MiniERP_API.Models.DTOs;
using MiniERP_API.Models.Entities;
using MiniERP_API.Repositories.Interfaces;
using MiniERP_API.Services.Interfaces;

namespace MiniERP_API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public IEnumerable<ProductDto> GetActiveProducts(string searchTerm = null, int? categoryId = null, int? brandId = null)
        {
            IEnumerable<Product> products;
            if (string.IsNullOrWhiteSpace(searchTerm) && !categoryId.HasValue && !brandId.HasValue)
            {
                products = _repo.GetAll();
            }
            else
            {
                products = _repo.Search(searchTerm, categoryId, brandId);
            }
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public ProductDto GetProduct(int id)
        {
            var product = _repo.GetById(id);
            return _mapper.Map<ProductDto>(product);
        }

        public int CreateProduct(ProductCreateUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name)) throw new System.Exception("Tên sản phẩm không được để trống.");
            if (dto.CostPrice < 0 || dto.RetailPrice < 0) throw new System.Exception("Giá trị không hợp lệ.");
            
            // Validate SKU uniqueness
            var existing = _repo.GetBySku(dto.SKU);
            if (existing != null) throw new System.Exception($"Mã SKU '{dto.SKU}' đã tồn tại.");

            var product = _mapper.Map<Product>(dto);
            return _repo.Add(product);
        }

        public void UpdateProduct(int id, ProductCreateUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name)) throw new System.Exception("Tên sản phẩm không được để trống.");
            if (dto.CostPrice < 0 || dto.RetailPrice < 0) throw new System.Exception("Giá trị không hợp lệ.");
            
            var existing = _repo.GetById(id);
            if (existing == null) throw new System.Exception("Sản phẩm không tồn tại.");

            // Validate SKU uniqueness if changed
            if (existing.SKU != dto.SKU)
            {
                var duplicate = _repo.GetBySku(dto.SKU);
                if (duplicate != null) throw new System.Exception($"Mã SKU '{dto.SKU}' đã bị trùng với sản phẩm khác.");
            }

            _mapper.Map(dto, existing);
            existing.Id = id;
            _repo.Update(existing);
        }
        public void DeleteProduct(int id) => _repo.Delete(id);
    }
}
