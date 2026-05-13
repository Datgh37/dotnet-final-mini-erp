using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MiniERP_API.Models.Entities;
using MiniERP_API.Helpers;
using MiniERP_API.Repositories.Interfaces;

namespace MiniERP_API.Repositories
{
    public class SalesOrderRepository : ISalesOrderRepository
    {
        private readonly string _cs;

        public SalesOrderRepository(IConfiguration config)
        {
            _cs = config.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<SalesOrder> GetAll()
        {
            var list = new List<SalesOrder>();
            using var conn = new SqlConnection(_cs);
            var cmd = new SqlCommand(Queries.GetAllSalesOrders, conn);
            conn.Open();
            using var r = cmd.ExecuteReader();
            while (r.Read()) list.Add(MapOrder(r));
            return list;
        }

        public SalesOrder GetById(int id)
        {
            SalesOrder order = null;
            using var conn = new SqlConnection(_cs);
            conn.Open();

            var cmd1 = new SqlCommand(Queries.GetSalesOrderById, conn);
            cmd1.Parameters.AddWithValue("@Id", id);
            using (var r = cmd1.ExecuteReader())
                if (r.Read()) order = MapOrder(r);

            if (order == null) return null;

            var cmd2 = new SqlCommand(Queries.GetSalesOrderItemsByOrderId, conn);
            cmd2.Parameters.AddWithValue("@OrderId", id);
            using (var r = cmd2.ExecuteReader())
                while (r.Read())
                    order.Items.Add(new SalesOrderItem
                    {
                        Id = (int)r["Id"], SalesOrderId = (int)r["SalesOrderId"],
                        ProductId = (int)r["ProductId"], Quantity = (int)r["Quantity"],
                        UnitPrice = (decimal)r["UnitPrice"],
                        Discount = r["Discount"] == DBNull.Value ? 0 : (decimal)r["Discount"]
                    });
            return order;
        }

        /// <summary>Sử dụng Stored Procedure sp_CreateSalesOrder với JSON</summary>
        public int CreateOrder(SalesOrder order)
        {
            // Xây dựng JSON cho danh sách Items
            string jsonItems = "[]";
            if (order.Items != null)
            {
                jsonItems = System.Text.Json.JsonSerializer.Serialize(order.Items);
            }

            using var conn = new SqlConnection(_cs);
            var cmd = new SqlCommand("sp_CreateSalesOrder", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@OrderNumber", order.OrderNumber);
            cmd.Parameters.AddWithValue("@CustomerId", (object)order.CustomerId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
            cmd.Parameters.AddWithValue("@PaymentMethod", (object)order.PaymentMethod ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ShippingAddress", (object)order.ShippingAddress ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Notes", (object)order.Notes ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedBy", (object)order.CreatedBy ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@OrderItems", jsonItems);

            conn.Open();
            return (int)cmd.ExecuteScalar();
        }

        public void UpdateStatus(int id, string status)
        {
            using var conn = new SqlConnection(_cs);
            var cmd = new SqlCommand(Queries.UpdateSalesOrderStatus, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Status", status);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        private SalesOrder MapOrder(SqlDataReader r) => new SalesOrder
        {
            Id = (int)r["Id"], OrderNumber = r["OrderNumber"].ToString(),
            CustomerId = r["CustomerId"] as int?, OrderDate = (DateTime)r["OrderDate"],
            Status = r["Status"]?.ToString(), PaymentMethod = r["PaymentMethod"]?.ToString(),
            PaymentStatus = r["PaymentStatus"]?.ToString(),
            TotalAmount = r["TotalAmount"] == DBNull.Value ? 0 : (decimal)r["TotalAmount"],
            ShippingAddress = r["ShippingAddress"]?.ToString(), Notes = r["Notes"]?.ToString(),
            CreatedBy = r["CreatedBy"] as int?,
            CreatedAt = (DateTimeOffset)r["CreatedAt"], UpdatedAt = r["UpdatedAt"] as DateTimeOffset?,
            IsDeleted = (bool)r["IsDeleted"]
        };
    }
}
