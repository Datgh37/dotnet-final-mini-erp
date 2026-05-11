using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MiniERP_API.Models.Entities;
using MiniERP_API.Repositories.Interfaces;

namespace MiniERP_API.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly string _cs;
        public CategoryRepository(IConfiguration config) => _cs = config.GetConnectionString("DefaultConnection");

        public IEnumerable<ProductCategory> GetAll()
        {
            var list = new List<ProductCategory>();
            using var conn = new SqlConnection(_cs);
            var cmd = new SqlCommand("SELECT * FROM ProductCategories WHERE IsDeleted = 0", conn);
            conn.Open();
            using var r = cmd.ExecuteReader();
            while (r.Read()) list.Add(Map(r));
            return list;
        }

        public ProductCategory GetById(int id)
        {
            using var conn = new SqlConnection(_cs);
            var cmd = new SqlCommand("SELECT * FROM ProductCategories WHERE Id = @Id AND IsDeleted = 0", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            conn.Open();
            using var r = cmd.ExecuteReader();
            return r.Read() ? Map(r) : null;
        }

        public int Add(ProductCategory c)
        {
            using var conn = new SqlConnection(_cs);
            var cmd = new SqlCommand("INSERT INTO ProductCategories (Name, ParentCategoryId) VALUES (@Name, @ParentId); SELECT CAST(SCOPE_IDENTITY() as int);", conn);
            cmd.Parameters.AddWithValue("@Name", c.Name);
            cmd.Parameters.AddWithValue("@ParentId", (object)c.ParentCategoryId ?? DBNull.Value);
            conn.Open();
            return (int)cmd.ExecuteScalar();
        }

        public void Update(ProductCategory c)
        {
            using var conn = new SqlConnection(_cs);
            var cmd = new SqlCommand("UPDATE ProductCategories SET Name = @Name, ParentCategoryId = @ParentId, UpdatedAt = SYSDATETIMEOFFSET() WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", c.Id);
            cmd.Parameters.AddWithValue("@Name", c.Name);
            cmd.Parameters.AddWithValue("@ParentId", (object)c.ParentCategoryId ?? DBNull.Value);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var conn = new SqlConnection(_cs);
            var cmd = new SqlCommand("sp_SoftDelete", conn) { CommandType = System.Data.CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@TableName", "ProductCategories");
            cmd.Parameters.AddWithValue("@Id", id);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        private ProductCategory Map(SqlDataReader r) => new ProductCategory
        {
            Id = (int)r["Id"], Name = r["Name"].ToString(), ParentCategoryId = r["ParentCategoryId"] as int?,
            CreatedAt = (DateTimeOffset)r["CreatedAt"], UpdatedAt = r["UpdatedAt"] as DateTimeOffset?, IsDeleted = (bool)r["IsDeleted"]
        };
    }
}
