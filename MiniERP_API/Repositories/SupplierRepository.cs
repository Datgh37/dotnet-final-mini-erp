using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MiniERP_API.Models.Entities;
using MiniERP_API.Helpers;
using MiniERP_API.Repositories.Interfaces;

namespace MiniERP_API.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly string _connectionString;

        public SupplierRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<Supplier> GetAll()
        {
            var suppliers = new List<Supplier>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(Queries.GetAllSuppliers, conn);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        suppliers.Add(new Supplier
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            ContactPerson = reader["ContactPerson"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            Email = reader["Email"].ToString(),
                            Address = reader["Address"].ToString(),
                            CreatedAt = (DateTimeOffset)reader["CreatedAt"],
                            UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? null : (DateTimeOffset?)reader["UpdatedAt"],
                            IsDeleted = (bool)reader["IsDeleted"]
                        });
                    }
                }
            }
            return suppliers;
        }

        public Supplier GetById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(Queries.GetSupplierById, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Supplier {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            ContactPerson = reader["ContactPerson"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            Email = reader["Email"].ToString(),
                            Address = reader["Address"].ToString(),
                            CreatedAt = (DateTimeOffset)reader["CreatedAt"],
                            UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? null : (DateTimeOffset?)reader["UpdatedAt"],
                            IsDeleted = (bool)reader["IsDeleted"]
                        };
                    }
                }
            }
            return null;
        }

        public int Add(Supplier s)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(Queries.InsertSupplier, conn);
                cmd.Parameters.AddWithValue("@Name", s.Name);
                cmd.Parameters.AddWithValue("@ContactPerson", (object)s.ContactPerson ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Phone", (object)s.Phone ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", (object)s.Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Address", (object)s.Address ?? DBNull.Value);
                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public void Update(Supplier s)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(Queries.UpdateSupplier, conn);
                cmd.Parameters.AddWithValue("@Id", s.Id);
                cmd.Parameters.AddWithValue("@Name", s.Name);
                cmd.Parameters.AddWithValue("@ContactPerson", (object)s.ContactPerson ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Phone", (object)s.Phone ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", (object)s.Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Address", (object)s.Address ?? DBNull.Value);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("sp_SoftDelete", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@TableName", "Suppliers");
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public Supplier GetByName(string name)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(Queries.GetSupplierByName, conn);
                cmd.Parameters.AddWithValue("@Name", name);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapSupplier(reader);
                    }
                }
            }
            return null;
        }

        public IEnumerable<Supplier> Search(string searchTerm)
        {
            var list = new List<Supplier>();
            using (var conn = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Suppliers WHERE IsDeleted = 0";
                if (!string.IsNullOrWhiteSpace(searchTerm))
                    sql += " AND (Name LIKE @Term OR ContactPerson LIKE @Term)";
                
                var cmd = new SqlCommand(sql, conn);
                if (!string.IsNullOrWhiteSpace(searchTerm))
                    cmd.Parameters.AddWithValue("@Term", $"%{searchTerm}%");
                
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read()) list.Add(MapSupplier(reader));
            }
            return list;
        }

        private Supplier MapSupplier(SqlDataReader reader)
        {
            return new Supplier {
                Id = (int)reader["Id"],
                Name = reader["Name"].ToString(),
                ContactPerson = reader["ContactPerson"].ToString(),
                Phone = reader["Phone"].ToString(),
                Email = reader["Email"].ToString(),
                Address = reader["Address"].ToString(),
                CreatedAt = (DateTimeOffset)reader["CreatedAt"],
                UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? null : (DateTimeOffset?)reader["UpdatedAt"],
                IsDeleted = (bool)reader["IsDeleted"]
            };
        }
    }
}
