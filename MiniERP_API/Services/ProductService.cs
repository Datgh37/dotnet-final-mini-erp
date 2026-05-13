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

        public IEnumerable<ProductDto> GetActiveProducts()
        {
            var products = _repo.GetAll();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public ProductDto GetProduct(int id)
        {
            var product = _repo.GetById(id);
            return _mapper.Map<ProductDto>(product);
        }

        public int CreateProduct(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name)) throw new System.Exception("Tên sản phẩm không được để trống.");
            if (product.CostPrice < 0 || product.RetailPrice < 0) throw new System.Exception("Giá trị không hợp lệ.");
            return _repo.Add(product);
        }

        public void UpdateProduct(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name)) throw new System.Exception("Tên sản phẩm không được để trống.");
            if (product.CostPrice < 0 || product.RetailPrice < 0) throw new System.Exception("Giá trị không hợp lệ.");
            _repo.Update(product);
        }
        public void DeleteProduct(int id) => _repo.Delete(id);
    }
}
