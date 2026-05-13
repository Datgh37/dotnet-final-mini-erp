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

        public IEnumerable<CustomerDto> GetAll() => _mapper.Map<IEnumerable<CustomerDto>>(_repo.GetAll());
        public CustomerDto GetById(int id) => _mapper.Map<CustomerDto>(_repo.GetById(id));
        public int Create(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.Name)) throw new System.Exception("Tên khách hàng không được để trống.");
            return _repo.Add(customer);
        }
        public void Update(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.Name)) throw new System.Exception("Tên khách hàng không được để trống.");
            _repo.Update(customer);
        }
        public void Delete(int id) => _repo.Delete(id);
    }
}
