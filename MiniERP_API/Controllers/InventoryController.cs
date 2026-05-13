using Microsoft.AspNetCore.Mvc;
using MiniERP_API.Models.DTOs;
using MiniERP_API.Services.Interfaces;

namespace MiniERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _service;
        public InventoryController(IInventoryService service) => _service = service;

        [HttpGet("movements")]
        public IActionResult GetMovements([FromQuery] int? productId, [FromQuery] string movementType)
            => Ok(_service.GetMovements(productId, movementType));

        [HttpPost("adjust")]
        public IActionResult AdjustStock(StockAdjustDto dto, [FromQuery] int? userId)
        {
            try
            {
                _service.AdjustStock(dto, userId);
                return Ok(new { message = "Đã điều chỉnh tồn kho thành công." });
            }
            catch (System.Exception ex) { return BadRequest(new { message = ex.Message }); }
        }
    }
}
