using Microsoft.AspNetCore.Mvc;
using MiniERP_API.Models.DTOs;
using MiniERP_API.Models.Entities;
using MiniERP_API.Services.Interfaces;

namespace MiniERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _service;
        public SuppliersController(ISupplierService service) => _service = service;

        [HttpGet]
        public IActionResult GetSupplierList() => Ok(_service.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetSupplierById(int id)
        {
            var supplier = _service.GetById(id);
            if (supplier == null) return NotFound();
            return Ok(supplier);
        }

        [HttpPost]
        public IActionResult CreateSupplier(SupplierCreateUpdateDto dto)
        {
            var id = _service.Create(dto);
            return CreatedAtAction(nameof(GetSupplierById), new { id }, dto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSupplier(int id, SupplierCreateUpdateDto dto)
        {
            _service.Update(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSupplier(int id)
        {
            _service.Delete(id);
            return NoContent();
        }
    }
}
