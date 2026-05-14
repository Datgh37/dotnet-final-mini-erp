using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MiniERP_API.Helpers;

namespace MiniERP_API.Repositories
{
    public class ReportRepository
    {
        private readonly string _cs;
        public ReportRepository(IConfiguration config) => _cs = config.GetConnectionString("DefaultConnection");

        /// <summary>Sử dụng Stored Procedure sp_GetRevenueReport</summary>
        public IEnumerable<dynamic> GetRevenueReport(DateTime from, DateTime to)
        {
            var list = new List<dynamic>();
            using var conn = new SqlConnection(_cs);
            var cmd = new SqlCommand("sp_GetRevenueReport", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@FromDate", from);
            cmd.Parameters.AddWithValue("@ToDate", to);
            conn.Open();
            using var r = cmd.ExecuteReader();
            while (r.Read())
                list.Add(new
                {
                    OrderDate = r["OrderDate"],
                    TotalOrders = r["TotalOrders"] != DBNull.Value ? Convert.ToInt32(r["TotalOrders"]) : 0,
                    DailyRevenue = r["DailyRevenue"] != DBNull.Value ? Convert.ToDecimal(r["DailyRevenue"]) : 0m
                });
            return list;
        }

        public IEnumerable<dynamic> GetTopSellingProducts(int top)
        {
            var list = new List<dynamic>();
            using var conn = new SqlConnection(_cs);
            var cmd = new SqlCommand(Queries.GetTopSellingProducts, conn);
            cmd.Parameters.AddWithValue("@Top", top);
            conn.Open();
            using var r = cmd.ExecuteReader();
            while (r.Read())
                list.Add(new
                {
                    ProductId = (int)r["Id"], Name = r["Name"].ToString(), SKU = r["SKU"].ToString(),
                    TotalSold = (int)r["TotalSold"], TotalRevenue = (decimal)r["TotalRevenue"]
                });
            return list;
        }

        public IEnumerable<dynamic> GetLowStockProducts(int threshold)
        {
            var list = new List<dynamic>();
            using var conn = new SqlConnection(_cs);
            var cmd = new SqlCommand(Queries.GetLowStockProducts, conn);
            cmd.Parameters.AddWithValue("@Threshold", threshold);
            conn.Open();
            using var r = cmd.ExecuteReader();
            while (r.Read())
                list.Add(new
                {
                    ProductId = (int)r["Id"], Name = r["Name"].ToString(), SKU = r["SKU"].ToString(),
                    StockQuantity = (int)r["StockQuantity"], CategoryName = r["CategoryName"]?.ToString()
                });
            return list;
        }
    }
}
