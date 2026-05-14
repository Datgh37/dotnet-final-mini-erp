using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniERP_API.Models.DTOs;
using MiniERP_API.Services.Interfaces;

namespace MiniERP_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/purchase-orders")]
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly IPurchaseOrderService _service;
        public PurchaseOrdersController(IPurchaseOrderService service) => _service = service;

        [HttpGet]
        public IActionResult GetAll([FromQuery] string? status) => Ok(_service.GetAll(status));

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var order = _service.GetById(id);
            return order == null ? NotFound() : Ok(order);
        }

        [HttpPost]
        public IActionResult Create(CreatePurchaseOrderDto dto)
        {
            try
            {
                var id = _service.CreateOrder(dto);
                return CreatedAtAction(nameof(GetById), new { id }, dto);
            }
            catch (System.Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPatch("{id}/receive")]
        public IActionResult Receive(int id, [FromQuery] int receivedBy)
        {
            try
            {
                _service.ReceiveOrder(id, DateTime.Now, receivedBy);
                return Ok(new { message = "Đã xác nhận nhập kho thành công." });
            }
            catch (System.Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPatch("{id}/cancel")]
        public IActionResult Cancel(int id)
        {
            try
            {
                _service.CancelOrder(id);
                return Ok(new { message = "Đã hủy đơn mua hàng." });
            }
            catch (System.Exception ex) { return BadRequest(new { message = ex.Message }); }
        }
    }
}
