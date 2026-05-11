using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MiniERP_API.Models.Entities;
using MiniERP_API.Repositories.Interfaces;

namespace MiniERP_API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _cs;
        public UserRepository(IConfiguration config) => _cs = config.GetConnectionString("DefaultConnection");

        public IEnumerable<User> GetAll()
        {
            var list = new List<User>();
            using var conn = new SqlConnection(_cs);
            var cmd = new SqlCommand("SELECT * FROM Users WHERE IsDeleted = 0", conn);
            conn.Open();
            using var r = cmd.ExecuteReader();
            while (r.Read()) list.Add(Map(r));
            return list;
        }

        public User GetById(int id)
        {
            using var conn = new SqlConnection(_cs);
            var cmd = new SqlCommand("SELECT * FROM Users WHERE Id = @Id AND IsDeleted = 0", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            conn.Open();
            using var r = cmd.ExecuteReader();
            return r.Read() ? Map(r) : null;
        }

        public User GetByUserName(string userName)
        {
            using var conn = new SqlConnection(_cs);
            var cmd = new SqlCommand("SELECT * FROM Users WHERE UserName = @Name AND IsDeleted = 0", conn);
            cmd.Parameters.AddWithValue("@Name", userName);
            conn.Open();
            using var r = cmd.ExecuteReader();
            return r.Read() ? Map(r) : null;
        }

        public int Add(User u)
        {
            using var conn = new SqlConnection(_cs);
            var cmd = new SqlCommand(@"INSERT INTO Users (UserName, Email, PasswordHash, FullName) 
                VALUES (@UserName, @Email, @Pass, @Name); SELECT CAST(SCOPE_IDENTITY() as int);", conn);
            cmd.Parameters.AddWithValue("@UserName", u.UserName);
            cmd.Parameters.AddWithValue("@Email", (object)u.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Pass", u.PasswordHash);
            cmd.Parameters.AddWithValue("@Name", (object)u.FullName ?? DBNull.Value);
            conn.Open();
            return (int)cmd.ExecuteScalar();
        }

        public void Update(User u)
        {
            using var conn = new SqlConnection(_cs);
            var cmd = new SqlCommand("UPDATE Users SET Email = @Email, FullName = @Name, UpdatedAt = SYSDATETIMEOFFSET() WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", u.Id);
            cmd.Parameters.AddWithValue("@Email", (object)u.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Name", (object)u.FullName ?? DBNull.Value);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var conn = new SqlConnection(_cs);
            var cmd = new SqlCommand("sp_SoftDelete", conn) { CommandType = System.Data.CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@TableName", "Users");
            cmd.Parameters.AddWithValue("@Id", id);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void ChangePassword(int id, string passwordHash)
        {
            using var conn = new SqlConnection(_cs);
            var cmd = new SqlCommand("UPDATE Users SET PasswordHash = @Hash, UpdatedAt = SYSDATETIMEOFFSET() WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Hash", passwordHash);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        private User Map(SqlDataReader r) => new User
        {
            Id = (int)r["Id"], UserName = r["UserName"].ToString(), Email = r["Email"]?.ToString(),
            FullName = r["FullName"]?.ToString(), PasswordHash = r["PasswordHash"]?.ToString(),
            CreatedAt = (DateTimeOffset)r["CreatedAt"], UpdatedAt = r["UpdatedAt"] as DateTimeOffset?, IsDeleted = (bool)r["IsDeleted"]
        };
    }
}
