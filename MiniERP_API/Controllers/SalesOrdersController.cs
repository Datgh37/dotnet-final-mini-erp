using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniERP_API.Models.DTOs;
using MiniERP_API.Services.Interfaces;

namespace MiniERP_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/sales-orders")]
    public class SalesOrdersController : ControllerBase
    {
        private readonly ISalesOrderService _service;
        public SalesOrdersController(ISalesOrderService service) => _service = service;

        [HttpGet]
        public IActionResult GetAll([FromQuery] string? status, [FromQuery] int? customerId, [FromQuery] System.DateTime? fromDate, [FromQuery] System.DateTime? toDate) 
            => Ok(_service.GetAll(status, customerId, fromDate, toDate));

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var order = _service.GetById(id);
            return order == null ? NotFound() : Ok(order);
        }

        [HttpPost]
        public IActionResult PlaceOrder(CreateSalesOrderDto dto)
        {
            try
            {
                var id = _service.PlaceOrder(dto);
                return CreatedAtAction(nameof(GetById), new { id }, dto);
            }
            catch (System.Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPatch("{id}/status")]
        public IActionResult UpdateStatus(int id, [FromBody] UpdateStatusDto dto)
        {
            try
            {
                _service.UpdateStatus(id, dto.Status);
                return Ok(new { message = "Cập nhật trạng thái đơn hàng thành công." });
            }
            catch (System.Exception ex) { return BadRequest(new { message = ex.Message }); }
        }
    }

    public class UpdateStatusDto
    {
        public string Status { get; set; }
    }
}
