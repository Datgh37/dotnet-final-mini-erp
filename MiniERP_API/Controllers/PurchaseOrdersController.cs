using System;
using Microsoft.AspNetCore.Mvc;
using MiniERP_API.Models.DTOs;
using MiniERP_API.Services.Interfaces;

namespace MiniERP_API.Controllers
{
    [ApiController]
    [Route("api/purchase-orders")]
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly IPurchaseOrderService _service;
        public PurchaseOrdersController(IPurchaseOrderService service) => _service = service;

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var order = _service.GetById(id);
            return order == null ? NotFound() : Ok(order);
        }

        [HttpPost]
        public IActionResult Create(CreatePurchaseOrderDto dto)
        {
            var id = _service.CreateOrder(dto);
            return CreatedAtAction(nameof(GetById), new { id }, dto);
        }

        [HttpPatch("{id}/receive")]
        public IActionResult Receive(int id, [FromQuery] int receivedBy)
        {
            _service.ReceiveOrder(id, DateTime.Now, receivedBy);
            return Ok(new { message = "Đã xác nhận nhập kho thành công." });
        }

        [HttpPatch("{id}/cancel")]
        public IActionResult Cancel(int id)
        {
            _service.CancelOrder(id);
            return Ok(new { message = "Đã hủy đơn mua hàng." });
        }
    }
}
