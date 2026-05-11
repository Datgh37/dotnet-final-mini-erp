using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

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
                    TotalOrders = (int)r["TotalOrders"],
                    DailyRevenue = (decimal)r["DailyRevenue"]
                });
            return list;
        }

        public IEnumerable<dynamic> GetTopSellingProducts(int top)
        {
            var list = new List<dynamic>();
            using var conn = new SqlConnection(_cs);
            var cmd = new SqlCommand(@"SELECT TOP (@Top) P.Id, P.Name, P.SKU, 
                SUM(SOI.Quantity) AS TotalSold, SUM((SOI.UnitPrice - SOI.Discount) * SOI.Quantity) AS TotalRevenue
                FROM SalesOrderItems SOI
                JOIN Products P ON SOI.ProductId = P.Id
                JOIN SalesOrders SO ON SOI.SalesOrderId = SO.Id
                WHERE SO.Status = 'COMPLETED' AND SO.IsDeleted = 0
                GROUP BY P.Id, P.Name, P.SKU
                ORDER BY TotalSold DESC", conn);
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
            var cmd = new SqlCommand(@"SELECT P.Id, P.Name, P.SKU, P.StockQuantity, PC.Name AS CategoryName
                FROM Products P LEFT JOIN ProductCategories PC ON P.CategoryId = PC.Id
                WHERE P.StockQuantity < @Threshold AND P.IsDeleted = 0
                ORDER BY P.StockQuantity ASC", conn);
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
