using Microsoft.AspNetCore.Mvc;
using MiniERP_API.Models.DTOs;
using MiniERP_API.Models.Entities;
using MiniERP_API.Services.Interfaces;

namespace MiniERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        public ProductsController(IProductService service) => _service = service;

        [HttpGet]
        public IActionResult GetAllProducts() => Ok(_service.GetActiveProducts());

        [HttpGet("{id}")]
        public IActionResult GetProductDetails(int id)
        {
            var p = _service.GetProduct(id);
            return p == null ? NotFound() : Ok(p);
        }

        [HttpPost]
        public IActionResult AddNewProduct(ProductCreateUpdateDto dto)
        {
            try
            {
                var id = _service.CreateProduct(dto);
                return CreatedAtAction(nameof(GetProductDetails), new { id }, dto);
            }
            catch (System.Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, ProductCreateUpdateDto dto)
        {
            try
            {
                _service.UpdateProduct(id, dto);
                return NoContent();
            }
            catch (System.Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            _service.DeleteProduct(id);
            return NoContent();
        }
    }
}
