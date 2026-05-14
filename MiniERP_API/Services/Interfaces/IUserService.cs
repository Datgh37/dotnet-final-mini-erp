using System.Collections.Generic;
using MiniERP_API.Models.DTOs;

namespace MiniERP_API.Services.Interfaces
{
    public interface IUserService
    {
        IEnumerable<UserDto> GetAll();
        UserDto GetById(int id);
        void Update(int id, UserUpdateDto dto);
        void Delete(int id);
        void ChangePassword(int id, UserPasswordChangeDto dto);
        int Create(RegisterRequest dto);
    }
}
