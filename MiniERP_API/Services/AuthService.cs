using System;
using AutoMapper;
using MiniERP_API.Models.DTOs;
using MiniERP_API.Models.Entities;
using MiniERP_API.Repositories.Interfaces;
using MiniERP_API.Services.Interfaces;

namespace MiniERP_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public AuthService(IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public AuthResponse Login(LoginRequest request)
        {
            var user = _userRepo.GetByUserName(request.UserName);
            if (user == null) throw new Exception("Sai tên đăng nhập hoặc mật khẩu.");

            bool isValid = false;

            // Kiểm tra xem mật khẩu trong DB có phải là định dạng BCrypt không (bắt đầu bằng $2)
            if (!string.IsNullOrEmpty(user.PasswordHash) && user.PasswordHash.StartsWith("$2"))
            {
                try 
                { 
                    isValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash); 
                }
                catch { isValid = false; }
            }
            else
            {
                // Nếu là mật khẩu thô (Raw) hoặc định dạng cũ, kiểm tra so khớp trực tiếp
                if (request.Password == user.PasswordHash)
                {
                    isValid = true;
                    // Tự động nâng cấp (Migration) lên BCrypt hash để bảo mật hơn
                    var newHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
                    _userRepo.ChangePassword(user.Id, newHash);
                }
            }

            if (!isValid) throw new Exception("Sai tên đăng nhập hoặc mật khẩu.");

            return new AuthResponse
            {
                Token = "mock-jwt-token-" + Guid.NewGuid().ToString(),
                RefreshToken = "mock-refresh-token-" + Guid.NewGuid().ToString(),
                User = _mapper.Map<UserDto>(user)
            };
        }

        public void Register(RegisterRequest request)
        {
            var existing = _userRepo.GetByUserName(request.UserName);
            if (existing != null) throw new Exception("Tên đăng nhập đã tồn tại.");

            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                FullName = request.FullName,
                // Băm mật khẩu bằng BCrypt trước khi lưu vào Database
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            _userRepo.Add(user);
        }

        public AuthResponse RefreshToken(RefreshTokenRequest request)
        {
            // Mock refresh logic
            return new AuthResponse
            {
                Token = "new-mock-jwt-token-" + Guid.NewGuid().ToString(),
                RefreshToken = "new-mock-refresh-token-" + Guid.NewGuid().ToString()
            };
        }
    }
}
