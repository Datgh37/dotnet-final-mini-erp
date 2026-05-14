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
    public class PurchaseOrderRepository : IPurchaseOrderRepository
    {
        private readonly string _cs;
        public PurchaseOrderRepository(IConfiguration config) => _cs = config.GetConnectionString("DefaultConnection");

        public IEnumerable<PurchaseOrder> GetAll(string status = null)
        {
            var list = new List<PurchaseOrder>();
            using var conn = new SqlConnection(_cs);
            string sql = "SELECT * FROM PurchaseOrders WHERE IsDeleted = 0";
            if (!string.IsNullOrWhiteSpace(status)) sql += " AND Status = @Status";
            
            var cmd = new SqlCommand(sql, conn);
            if (!string.IsNullOrWhiteSpace(status)) cmd.Parameters.AddWithValue("@Status", status);

            conn.Open();
            using var r = cmd.ExecuteReader();
            while (r.Read()) list.Add(MapOrder(r));
            return list;
        }

        public PurchaseOrder GetById(int id)
        {
            PurchaseOrder order = null;
            using var conn = new SqlConnection(_cs);
            conn.Open();

            // Lấy thông tin đơn hàng
            var cmd1 = new SqlCommand(Queries.GetPurchaseOrderById, conn);
            cmd1.Parameters.AddWithValue("@Id", id);
            using (var r = cmd1.ExecuteReader())
                if (r.Read()) order = MapOrder(r);

            if (order == null) return null;

            // Lấy danh sách items
            var cmd2 = new SqlCommand(Queries.GetPurchaseOrderItemsByOrderId, conn);
            cmd2.Parameters.AddWithValue("@Id", id);
            using (var r = cmd2.ExecuteReader())
                while (r.Read())
                    order.Items.Add(new PurchaseOrderItem
                    {
                        Id = (int)r["Id"], PurchaseOrderId = (int)r["PurchaseOrderId"],
                        ProductId = (int)r["ProductId"], Quantity = (int)r["Quantity"], UnitPrice = (decimal)r["UnitPrice"]
                    });
            return order;
        }

        public int CreateOrder(PurchaseOrder order)
        {
            using var conn = new SqlConnection(_cs);
            conn.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                // 1. Chèn PurchaseOrder
                var cmd1 = new SqlCommand(Queries.InsertPurchaseOrder, conn, tran);
                cmd1.Parameters.AddWithValue("@PONumber", order.PONumber);
                cmd1.Parameters.AddWithValue("@SupplierId", (object)order.SupplierId ?? DBNull.Value);
                cmd1.Parameters.AddWithValue("@OrderDate", (object)order.OrderDate ?? DBNull.Value);
                cmd1.Parameters.AddWithValue("@ExpectedDate", (object)order.ExpectedDate ?? DBNull.Value);
                cmd1.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                cmd1.Parameters.AddWithValue("@Notes", (object)order.Notes ?? DBNull.Value);
                cmd1.Parameters.AddWithValue("@CreatedBy", (object)order.CreatedBy ?? DBNull.Value);
                int poId = (int)cmd1.ExecuteScalar();

                // 2. Chèn PurchaseOrderItems
                foreach (var item in order.Items)
                {
                    var cmd2 = new SqlCommand(Queries.InsertPurchaseOrderItem, conn, tran);
                    cmd2.Parameters.AddWithValue("@POId", poId);
                    cmd2.Parameters.AddWithValue("@ProductId", item.ProductId);
                    cmd2.Parameters.AddWithValue("@Qty", item.Quantity);
                    cmd2.Parameters.AddWithValue("@Price", item.UnitPrice);
                    cmd2.ExecuteNonQuery();
                }

                tran.Commit();
                return poId;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        /// <summary>Sử dụng Stored Procedure sp_ReceivePurchaseOrder</summary>
        public void ReceiveOrder(int id, DateTime receivedDate, int receivedBy)
        {
            using var conn = new SqlConnection(_cs);
            var cmd = new SqlCommand("sp_ReceivePurchaseOrder", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@POId", id);
            cmd.Parameters.AddWithValue("@ReceivedDate", receivedDate);
            cmd.Parameters.AddWithValue("@ReceivedBy", receivedBy);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void CancelOrder(int id)
        {
            using var conn = new SqlConnection(_cs);
            var cmd = new SqlCommand(Queries.CancelPurchaseOrder, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            conn.Open();
            if (cmd.ExecuteNonQuery() == 0) throw new Exception("Đơn mua hàng không tồn tại hoặc đã được xử lý.");
        }

        public PurchaseOrder GetByNumber(string poNumber)
        {
            using var conn = new SqlConnection(_cs);
            var cmd = new SqlCommand(Queries.GetPurchaseOrderByNumber, conn);
            cmd.Parameters.AddWithValue("@Num", poNumber);
            conn.Open();
            using var r = cmd.ExecuteReader();
            return r.Read() ? MapOrder(r) : null;
        }

        private PurchaseOrder MapOrder(SqlDataReader r) => new PurchaseOrder
        {
            Id = (int)r["Id"], PONumber = r["PONumber"].ToString(), SupplierId = r["SupplierId"] as int?,
            OrderDate = r["OrderDate"] as DateTime?, ExpectedDate = r["ExpectedDate"] as DateTime?,
            ReceivedDate = r["ReceivedDate"] as DateTime?, Status = r["Status"]?.ToString(),
            TotalAmount = r["TotalAmount"] == DBNull.Value ? 0 : (decimal)r["TotalAmount"],
            Notes = r["Notes"]?.ToString(), CreatedBy = r["CreatedBy"] as int?,
            CreatedAt = (DateTimeOffset)r["CreatedAt"], UpdatedAt = r["UpdatedAt"] as DateTimeOffset?, IsDeleted = (bool)r["IsDeleted"]
        };
    }
}
