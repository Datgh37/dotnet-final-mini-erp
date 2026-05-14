using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MiniERP_API.Models.Entities;
using MiniERP_API.Helpers;
using MiniERP_API.Repositories.Interfaces;

namespace MiniERP_API.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly string _cs;
        public BrandRepository(IConfiguration config) => _cs = config.GetConnectionString("DefaultConnection");

        public IEnumerable<Brand> GetAll()
        {
            var list = new List<Brand>();
            using var conn = new SqlConnection(_cs);
            var cmd = new SqlCommand(Queries.GetAllBrands, conn);
            conn.Open();
            using var r = cmd.ExecuteReader();
            while (r.Read()) list.Add(Map(r));
            return list;
        }

        public Brand GetById(int id)
        {
            using var conn = new SqlConnection(_cs);
            var cmd = new SqlCommand(Queries.GetBrandById, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            conn.Open();
            using var r = cmd.ExecuteReader();
            return r.Read() ? Map(r) : null;
        }

        public int Add(Brand b)
        {
            using var conn = new SqlConnection(_cs);
            var cmd = new SqlCommand(Queries.InsertBrand, conn);
            cmd.Parameters.AddWithValue("@Name", b.Name);
            cmd.Parameters.AddWithValue("@Desc", (object)b.Description ?? DBNull.Value);
            conn.Open();
            return (int)cmd.ExecuteScalar();
        }

        public void Update(Brand b)
        {
            using var conn = new SqlConnection(_cs);
            var cmd = new SqlCommand(Queries.UpdateBrand, conn);
            cmd.Parameters.AddWithValue("@Id", b.Id);
            cmd.Parameters.AddWithValue("@Name", b.Name);
            cmd.Parameters.AddWithValue("@Desc", (object)b.Description ?? DBNull.Value);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var conn = new SqlConnection(_cs);
            var cmd = new SqlCommand("sp_SoftDelete", conn) { CommandType = System.Data.CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@TableName", "Brands");
            cmd.Parameters.AddWithValue("@Id", id);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public Brand GetByName(string name)
        {
            using var conn = new SqlConnection(_cs);
            var cmd = new SqlCommand(Queries.GetBrandByName, conn);
            cmd.Parameters.AddWithValue("@Name", name);
            conn.Open();
            using var r = cmd.ExecuteReader();
            return r.Read() ? Map(r) : null;
        }

        private Brand Map(SqlDataReader r) => new Brand
        {
            Id = (int)r["Id"], Name = r["Name"].ToString(), Description = r["Description"]?.ToString(),
            CreatedAt = (DateTimeOffset)r["CreatedAt"], UpdatedAt = r["UpdatedAt"] as DateTimeOffset?, IsDeleted = (bool)r["IsDeleted"]
        };
    }
}
