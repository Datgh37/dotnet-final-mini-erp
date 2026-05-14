using System.Collections.Generic;
using AutoMapper;
using MiniERP_API.Models.DTOs;
using MiniERP_API.Models.Entities;
using MiniERP_API.Repositories.Interfaces;
using MiniERP_API.Services.Interfaces;

namespace MiniERP_API.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repo;
        private readonly IMapper _mapper;
        public CustomerService(ICustomerRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

        public IEnumerable<CustomerDto> GetAll(string searchTerm = null)
        {
            var customers = string.IsNullOrWhiteSpace(searchTerm) ? _repo.GetAll() : _repo.Search(searchTerm);
            return _mapper.Map<IEnumerable<CustomerDto>>(customers);
        }
        public CustomerDto GetById(int id) => _mapper.Map<CustomerDto>(_repo.GetById(id));
        public int Create(CustomerCreateUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name)) throw new System.Exception("Tên khách hàng không được để trống.");
            var customer = _mapper.Map<Customer>(dto);
            return _repo.Add(customer);
        }
        public void Update(int id, CustomerCreateUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name)) throw new System.Exception("Tên khách hàng không được để trống.");
            var existing = _repo.GetById(id);
            if (existing == null) throw new System.Exception("Khách hàng không tồn tại.");
            _mapper.Map(dto, existing);
            existing.Id = id;
            _repo.Update(existing);
        }
        public void Delete(int id) => _repo.Delete(id);
    }
}
