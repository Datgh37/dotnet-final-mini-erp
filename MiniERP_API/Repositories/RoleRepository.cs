using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MiniERP_API.Models.Entities;
using MiniERP_API.Repositories.Interfaces;

namespace MiniERP_API.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly string _connectionString;
        public RoleRepository(IConfiguration config) => _connectionString = config.GetConnectionString("DefaultConnection");

        public IEnumerable<Role> GetAll()
        {
            var list = new List<Role>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT * FROM Roles WHERE IsDeleted = 0", conn);
                conn.Open();
                using (var r = cmd.ExecuteReader())
                    while (r.Read())
                        list.Add(new Role { Id = (int)r["Id"], Name = r["Name"].ToString() });
            }
            return list;
        }

        public int Add(Role role)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("INSERT INTO Roles (Name) VALUES (@Name); SELECT CAST(SCOPE_IDENTITY() as int);", conn);
                cmd.Parameters.AddWithValue("@Name", role.Name);
                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }
    }
}
