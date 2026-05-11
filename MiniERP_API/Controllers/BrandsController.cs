using Microsoft.AspNetCore.Mvc;
using MiniERP_API.Models.Entities;
using MiniERP_API.Services.Interfaces;

namespace MiniERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _service;
        public BrandsController(IBrandService service) => _service = service;

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var brand = _service.GetById(id);
            return brand == null ? NotFound() : Ok(brand);
        }

        [HttpPost]
        public IActionResult Create(Brand brand)
        {
            var id = _service.Create(brand);
            return CreatedAtAction(nameof(GetById), new { id }, brand);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Brand brand)
        {
            if (id != brand.Id) return BadRequest();
            _service.Update(brand);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return NoContent();
        }
    }
}
