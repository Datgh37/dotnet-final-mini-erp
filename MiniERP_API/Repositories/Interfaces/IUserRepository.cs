using System.Collections.Generic;
using MiniERP_API.Models.Entities;

namespace MiniERP_API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User GetById(int id);
        User GetByUserName(string userName);
        int Add(User user);
        void Update(User user);
        void Delete(int id);
        void ChangePassword(int id, string passwordHash);
        void AssignRole(int userId, int roleId);
        int GetRoleIdByName(string roleName);
        string GetRoleName(int userId);
    }
}
