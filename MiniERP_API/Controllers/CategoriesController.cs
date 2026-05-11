using Microsoft.AspNetCore.Mvc;
using MiniERP_API.Models.Entities;
using MiniERP_API.Services.Interfaces;

namespace MiniERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;
        public CategoriesController(ICategoryService service) => _service = service;

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var cat = _service.GetById(id);
            return cat == null ? NotFound() : Ok(cat);
        }

        [HttpPost]
        public IActionResult Create(ProductCategory category)
        {
            var id = _service.Create(category);
            return CreatedAtAction(nameof(GetById), new { id }, category);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, ProductCategory category)
        {
            if (id != category.Id) return BadRequest();
            _service.Update(category);
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
