using System.Collections.Generic;
using MiniERP_API.Models.Entities;

namespace MiniERP_API.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        IEnumerable<Role> GetAll();
        int Add(Role role);
    }
}
