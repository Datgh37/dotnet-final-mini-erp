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

        public IEnumerable<UserDto> GetAll()
        {
            var users = _repo.GetAll();
            var dtos = _mapper.Map<IEnumerable<UserDto>>(users);
            foreach (var d in dtos)
            {
                d.Role = _repo.GetRoleName(d.Id);
            }
            return dtos;
        }

        public UserDto GetById(int id)
        {
            var user = _repo.GetById(id);
            if (user == null) return null;
            var dto = _mapper.Map<UserDto>(user);
            dto.Role = _repo.GetRoleName(user.Id);
            return dto;
        }

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

        public int Create(RegisterRequest dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserName)) throw new System.Exception("Tên đăng nhập không được để trống.");
            
            var existing = _repo.GetByUserName(dto.UserName);
            if (existing != null) throw new System.Exception($"Tên đăng nhập '{dto.UserName}' đã tồn tại.");

            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                FullName = dto.FullName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            int userId = _repo.Add(user);
            
            // Gán role mặc định là Staff
            int roleId = _repo.GetRoleIdByName("Staff");
            
            if (roleId > 0)
            {
                _repo.AssignRole(userId, roleId);
            }

            return userId;
        }

        public void Delete(int id) => _repo.Delete(id);

        public void ChangePassword(int id, UserPasswordChangeDto dto)
        {
            var user = _repo.GetById(id);
            if (user == null) throw new System.Exception("Người dùng không tồn tại.");

            bool isValid = false;
            // Kiểm tra mật khẩu cũ (Hỗ trợ cả BCrypt và Raw password giống AuthService)
            if (!string.IsNullOrEmpty(user.PasswordHash) && user.PasswordHash.StartsWith("$2"))
            {
                isValid = BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.PasswordHash);
            }
            else
            {
                if (dto.OldPassword == user.PasswordHash) isValid = true;
            }

            if (!isValid) throw new System.Exception("Mật khẩu cũ không chính xác.");

            if (dto.OldPassword == dto.NewPassword)
            {
                throw new System.Exception("Mật khẩu mới không được trùng với mật khẩu cũ.");
            }

            // Hash mật khẩu mới và lưu lại
            var newHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            _repo.ChangePassword(id, newHash);
        }
    }
}
