using System;
using Microsoft.AspNetCore.Mvc;
using MiniERP_API.Repositories;

namespace MiniERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly ReportRepository _reportRepo;
        public ReportsController(ReportRepository reportRepo) => _reportRepo = reportRepo;

        /// <summary>Báo cáo doanh thu (sử dụng SP sp_GetRevenueReport)</summary>
        [HttpGet("revenue")]
        public IActionResult GetRevenue([FromQuery] DateTime from, [FromQuery] DateTime to)
            => Ok(_reportRepo.GetRevenueReport(from, to));

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
