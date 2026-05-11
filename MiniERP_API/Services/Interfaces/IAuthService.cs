using MiniERP_API.Models.DTOs;

namespace MiniERP_API.Services.Interfaces
{
    public interface IAuthService
    {
        AuthResponse Login(LoginRequest request);
        void Register(RegisterRequest request);
        AuthResponse RefreshToken(RefreshTokenRequest request);
    }
}
