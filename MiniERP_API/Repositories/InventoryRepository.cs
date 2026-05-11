using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MiniERP_API.Models.Entities;
using MiniERP_API.Repositories.Interfaces;

namespace MiniERP_API.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly string _cs;
        public InventoryRepository(IConfiguration config) => _cs = config.GetConnectionString("DefaultConnection");

        public IEnumerable<StockMovement> GetMovements(int? productId, string movementType)
        {
            var list = new List<StockMovement>();
            using var conn = new SqlConnection(_cs);
            var sql = "SELECT * FROM StockMovements WHERE IsDeleted = 0";
            if (productId.HasValue) sql += " AND ProductId = @ProductId";
            if (!string.IsNullOrEmpty(movementType)) sql += " AND MovementType = @Type";
            sql += " ORDER BY CreatedAt DESC";

            var cmd = new SqlCommand(sql, conn);
            if (productId.HasValue) cmd.Parameters.AddWithValue("@ProductId", productId.Value);
            if (!string.IsNullOrEmpty(movementType)) cmd.Parameters.AddWithValue("@Type", movementType);

            conn.Open();
            using var r = cmd.ExecuteReader();
            while (r.Read())
                list.Add(new StockMovement
                {
                    Id = (int)r["Id"], ProductId = (int)r["ProductId"],
                    MovementType = r["MovementType"].ToString(), Quantity = (int)r["Quantity"],
                    Reference = r["Reference"]?.ToString(), CreatedBy = r["CreatedBy"] as int?,
                    CreatedAt = (DateTimeOffset)r["CreatedAt"]
                });
            return list;
        }

        public void AdjustStock(int productId, int quantity, string reason, int? createdBy)
        {
            using var conn = new SqlConnection(_cs);
            conn.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                // 1. Cập nhật tồn kho
                var cmd1 = new SqlCommand("UPDATE Products SET StockQuantity = StockQuantity + @Qty, UpdatedAt = SYSDATETIMEOFFSET() WHERE Id = @Id", conn, tran);
                cmd1.Parameters.AddWithValue("@Qty", quantity);
                cmd1.Parameters.AddWithValue("@Id", productId);
                cmd1.ExecuteNonQuery();

                // 2. Ghi nhật ký
                var cmd2 = new SqlCommand(@"INSERT INTO StockMovements (ProductId, MovementType, Quantity, Reference, CreatedBy) 
                    VALUES (@ProductId, 'ADJUSTMENT', @Qty, @Reason, @CreatedBy)", conn, tran);
                cmd2.Parameters.AddWithValue("@ProductId", productId);
                cmd2.Parameters.AddWithValue("@Qty", quantity);
                cmd2.Parameters.AddWithValue("@Reason", (object)reason ?? DBNull.Value);
                cmd2.Parameters.AddWithValue("@CreatedBy", (object)createdBy ?? DBNull.Value);
                cmd2.ExecuteNonQuery();

                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }
    }
}
