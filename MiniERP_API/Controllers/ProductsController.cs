using Microsoft.AspNetCore.Mvc;
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
        public IActionResult AddNewProduct(Product product)
        {
            try
            {
                var id = _service.CreateProduct(product);
                return CreatedAtAction(nameof(GetProductDetails), new { id }, product);
            }
            catch (System.Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, Product product)
        {
            try
            {
                if (id != product.Id) return BadRequest(new { message = "ID không khớp." });
                _service.UpdateProduct(product);
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
