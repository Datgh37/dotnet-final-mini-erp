using System.Collections.Generic;
using AutoMapper;
using MiniERP_API.Models.DTOs;
using MiniERP_API.Models.Entities;
using MiniERP_API.Repositories.Interfaces;
using MiniERP_API.Services.Interfaces;

namespace MiniERP_API.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repo;
        private readonly IMapper _mapper;
        public RoleService(IRoleRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

        public IEnumerable<RoleDto> GetAll() => _mapper.Map<IEnumerable<RoleDto>>(_repo.GetAll());

        public int Create(RoleDto dto)
        {
            var role = _mapper.Map<Role>(dto);
            return _repo.Add(role);
        }
    }
}
