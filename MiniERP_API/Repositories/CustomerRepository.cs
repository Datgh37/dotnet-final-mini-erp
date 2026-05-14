using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MiniERP_API.Models.Entities;
using MiniERP_API.Helpers;
using MiniERP_API.Repositories.Interfaces;

namespace MiniERP_API.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _cs;
        public CustomerRepository(IConfiguration config) => _cs = config.GetConnectionString("DefaultConnection");

        public IEnumerable<Customer> GetAll()
        {
            var list = new List<Customer>();
            using var conn = new SqlConnection(_cs);
            var cmd = new SqlCommand(Queries.GetAllCustomers, conn);
            conn.Open();
            using var r = cmd.ExecuteReader();
            while (r.Read()) list.Add(Map(r));
            return list;
        }

        public Customer GetById(int id)
        {
            using var conn = new SqlConnection(_cs);
            var cmd = new SqlCommand(Queries.GetCustomerById, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            conn.Open();
            using var r = cmd.ExecuteReader();
            return r.Read() ? Map(r) : null;
        }

        public int Add(Customer c)
        {
            using var conn = new SqlConnection(_cs);
            var cmd = new SqlCommand(Queries.InsertCustomer, conn);
            cmd.Parameters.AddWithValue("@UserId", (object)c.UserId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Name", c.Name);
            cmd.Parameters.AddWithValue("@Email", (object)c.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Phone", (object)c.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Address", (object)c.Address ?? DBNull.Value);
            conn.Open();
            return (int)cmd.ExecuteScalar();
        }

        public void Update(Customer c)
        {
            using var conn = new SqlConnection(_cs);
            var cmd = new SqlCommand(Queries.UpdateCustomer, conn);
            cmd.Parameters.AddWithValue("@Id", c.Id);
            cmd.Parameters.AddWithValue("@Name", c.Name);
            cmd.Parameters.AddWithValue("@Email", (object)c.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Phone", (object)c.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Address", (object)c.Address ?? DBNull.Value);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var conn = new SqlConnection(_cs);
            var cmd = new SqlCommand("sp_SoftDelete", conn) { CommandType = System.Data.CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@TableName", "Customers");
            cmd.Parameters.AddWithValue("@Id", id);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public IEnumerable<Customer> Search(string searchTerm)
        {
            var list = new List<Customer>();
            using var conn = new SqlConnection(_cs);
            string sql = "SELECT * FROM Customers WHERE IsDeleted = 0";
            if (!string.IsNullOrWhiteSpace(searchTerm))
                sql += " AND (Name LIKE @Term OR Email LIKE @Term OR Phone LIKE @Term)";
            
            var cmd = new SqlCommand(sql, conn);
            if (!string.IsNullOrWhiteSpace(searchTerm))
                cmd.Parameters.AddWithValue("@Term", $"%{searchTerm}%");
                
            conn.Open();
            using var r = cmd.ExecuteReader();
            while (r.Read()) list.Add(Map(r));
            return list;
        }

        private Customer Map(SqlDataReader r) => new Customer
        {
            Id = (int)r["Id"], UserId = r["UserId"] as int?, Name = r["Name"].ToString(),
            Email = r["Email"]?.ToString(), Phone = r["Phone"]?.ToString(),
            Address = r["Address"]?.ToString(),
            CreatedAt = (DateTimeOffset)r["CreatedAt"], UpdatedAt = r["UpdatedAt"] as DateTimeOffset?, IsDeleted = (bool)r["IsDeleted"]
        };
    }
}
