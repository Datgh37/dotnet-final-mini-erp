using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniERP_API.Repositories;

namespace MiniERP_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly ReportRepository _reportRepo;
        public ReportsController(ReportRepository reportRepo) => _reportRepo = reportRepo;

        /// <summary>Báo cáo doanh thu (mặc định 7 ngày gần nhất nếu không truyền from/to)</summary>
        [HttpGet("revenue")]
        public IActionResult GetRevenue([FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] int days = 7)
        {
            DateTime startDate = from ?? DateTime.Now.AddDays(-days);
            DateTime endDate = to ?? DateTime.Now;
            
            return Ok(_reportRepo.GetRevenueReport(startDate, endDate));
        }

        /// <summary>Báo cáo sản phẩm bán chạy</summary>
        [HttpGet("top-selling")]
        public IActionResult GetTopSelling([FromQuery] int top = 10)
            => Ok(_reportRepo.GetTopSellingProducts(top));

        /// <summary>Danh sách sản phẩm sắp hết hàng</summary>
        [HttpGet("low-stock")]
        public IActionResult GetLowStock([FromQuery] int threshold = 10)
            => Ok(_reportRepo.GetLowStockProducts(threshold));
    }
}
