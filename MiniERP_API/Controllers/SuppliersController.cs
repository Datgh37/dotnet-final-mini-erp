using Microsoft.AspNetCore.Mvc;
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
        public IActionResult CreateSupplier(Supplier s)
        {
            var id = _service.Create(s);
            return Ok(new { Id = id });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSupplier(int id, Supplier s)
        {
            if (id != s.Id) return BadRequest();
            _service.Update(s);
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
