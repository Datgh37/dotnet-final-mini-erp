using System.Collections.Generic;
using MiniERP_API.Models.Entities;

namespace MiniERP_API.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetAll();
        Customer GetById(int id);
        int Add(Customer customer);
        void Update(Customer customer);
        void Delete(int id);
        IEnumerable<Customer> Search(string searchTerm);
    }
}
