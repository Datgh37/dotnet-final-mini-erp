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
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<Product> GetAll()
        {
            var products = new List<Product>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(Queries.GetAllProducts, conn);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(MapProduct(reader));
                    }
                }
            }
            return products;
        }

        public Product GetById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(Queries.GetProductById, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) return MapProduct(reader);
                }
            }
            return null;
        }

        public int Add(Product p)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(Queries.InsertProduct, conn);
                AddProductParameters(cmd, p);
                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public void Update(Product p)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(Queries.UpdateProduct, conn);
                cmd.Parameters.AddWithValue("@Id", p.Id);
                AddProductParameters(cmd, p);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("sp_SoftDelete", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@TableName", "Products");
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public Product GetBySku(string sku)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(Queries.GetProductBySku, conn);
                cmd.Parameters.AddWithValue("@SKU", sku);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) return MapProduct(reader);
                }
            }
            return null;
        }

        public IEnumerable<Product> Search(string searchTerm, int? categoryId, int? brandId)
        {
            var products = new List<Product>();
            using (var conn = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Products WHERE IsDeleted = 0";
                
                if (!string.IsNullOrWhiteSpace(searchTerm))
                    sql += " AND (Name LIKE @Term OR SKU LIKE @Term)";
                
                if (categoryId.HasValue)
                    sql += " AND CategoryId = @CatId";
                
                if (brandId.HasValue)
                    sql += " AND BrandId = @BrandId";

                var cmd = new SqlCommand(sql, conn);
                
                if (!string.IsNullOrWhiteSpace(searchTerm))
                    cmd.Parameters.AddWithValue("@Term", $"%{searchTerm}%");
                
                if (categoryId.HasValue)
                    cmd.Parameters.AddWithValue("@CatId", categoryId.Value);
                
                if (brandId.HasValue)
                    cmd.Parameters.AddWithValue("@BrandId", brandId.Value);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(MapProduct(reader));
                    }
                }
            }
            return products;
        }

        private void AddProductParameters(SqlCommand cmd, Product p)
        {
            cmd.Parameters.AddWithValue("@CategoryId", (object)p.CategoryId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@BrandId", (object)p.BrandId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SKU", p.SKU);
            cmd.Parameters.AddWithValue("@Name", p.Name);
            cmd.Parameters.AddWithValue("@Description", (object)p.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Unit", (object)p.Unit ?? DBNull.Value);

            cmd.Parameters.AddWithValue("@CostPrice", p.CostPrice);
            cmd.Parameters.AddWithValue("@RetailPrice", p.RetailPrice);
            cmd.Parameters.AddWithValue("@StockQuantity", p.StockQuantity);
            cmd.Parameters.AddWithValue("@ImageUrl", (object)p.ImageUrl ?? DBNull.Value);
        }

        private Product MapProduct(SqlDataReader reader)
        {
            return new Product
            {
                Id = (int)reader["Id"],
                CategoryId = reader["CategoryId"] as int?,
                BrandId = reader["BrandId"] as int?,
                SKU = reader["SKU"].ToString(),
                Name = reader["Name"].ToString(),
                Description = reader["Description"].ToString(),
                Unit = reader["Unit"].ToString(),

                CostPrice = (decimal)reader["CostPrice"],
                RetailPrice = (decimal)reader["RetailPrice"],
                StockQuantity = (int)reader["StockQuantity"],
                ImageUrl = reader["ImageUrl"].ToString(),
                CreatedAt = (DateTimeOffset)reader["CreatedAt"],
                UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? null : (DateTimeOffset?)reader["UpdatedAt"],
                IsDeleted = (bool)reader["IsDeleted"]
            };
        }
    }
}
