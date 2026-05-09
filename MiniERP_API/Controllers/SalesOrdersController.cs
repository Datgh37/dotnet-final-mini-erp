using Microsoft.AspNetCore.Mvc;
using MiniERP_API.Models.DTOs;
using MiniERP_API.Services.Interfaces;

namespace MiniERP_API.Controllers
{
    [ApiController]
    [Route("api/sales-orders")]
    public class SalesOrdersController : ControllerBase
    {
        private readonly ISalesOrderService _service;
        public SalesOrdersController(ISalesOrderService service) => _service = service;

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var order = _service.GetById(id);
            return order == null ? NotFound() : Ok(order);
        }

        [HttpPost]
        public IActionResult PlaceOrder(CreateSalesOrderDto dto)
        {
            var id = _service.PlaceOrder(dto);
            return CreatedAtAction(nameof(GetById), new { id }, dto);
        }

        [HttpPatch("{id}/status")]
        public IActionResult UpdateStatus(int id, [FromBody] UpdateStatusDto dto)
        {
            _service.UpdateStatus(id, dto.Status);
            return Ok(new { message = "Cập nhật trạng thái đơn hàng thành công." });
        }
    }

    public class UpdateStatusDto
    {
        public string Status { get; set; }
    }
}
