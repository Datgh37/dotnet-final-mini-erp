using System.Collections.Generic;
using MiniERP_API.Models.DTOs;

namespace MiniERP_API.Services.Interfaces
{
    public interface IRoleService
    {
        IEnumerable<RoleDto> GetAll();
        int Create(RoleDto dto);
    }
}
