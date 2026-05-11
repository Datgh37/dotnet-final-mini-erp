using System.Collections.Generic;
using MiniERP_API.Models.DTOs;
using MiniERP_API.Models.Entities;

namespace MiniERP_API.Services.Interfaces
{
    public interface ICustomerService
    {
        IEnumerable<CustomerDto> GetAll();
        CustomerDto GetById(int id);
        int Create(Customer customer);
        void Update(Customer customer);
        void Delete(int id);
    }
}
