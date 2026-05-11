using System.Collections.Generic;
using AutoMapper;
using MiniERP_API.Models.DTOs;
using MiniERP_API.Models.Entities;
using MiniERP_API.Repositories.Interfaces;
using MiniERP_API.Services.Interfaces;

namespace MiniERP_API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;
        public UserService(IUserRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

        public IEnumerable<UserDto> GetAll() => _mapper.Map<IEnumerable<UserDto>>(_repo.GetAll());
        public UserDto GetById(int id) => _mapper.Map<UserDto>(_repo.GetById(id));

        public void Update(int id, UserUpdateDto dto)
        {
            var user = _repo.GetById(id);
            if (user != null)
            {
                user.Email = dto.Email;
                user.FullName = dto.FullName;
                _repo.Update(user);
            }
        }

        public void Delete(int id) => _repo.Delete(id);

        public void ChangePassword(int id, UserPasswordChangeDto dto)
        {
            // Note: In real app, verify old password here
            _repo.ChangePassword(id, dto.NewPassword); 
        }
    }
}
