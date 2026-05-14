using System;
using AutoMapper;
using MiniERP_API.Models.DTOs;
using MiniERP_API.Models.Entities;
using MiniERP_API.Repositories.Interfaces;
using MiniERP_API.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MiniERP_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository userRepo, IMapper mapper, IConfiguration config)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _config = config;
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

            var userDto = _mapper.Map<UserDto>(user);
            var role = _userRepo.GetRoleName(user.Id);
            userDto.Role = role;

            return new AuthResponse
            {
                Token = GenerateJwtToken(user, role),
                RefreshToken = Guid.NewGuid().ToString(),
                User = userDto
            };
        }

        private string GenerateJwtToken(User user, string role)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
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
