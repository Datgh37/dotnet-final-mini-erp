using Microsoft.AspNetCore.Mvc;
using MiniERP_API.Models.Entities;
using MiniERP_API.Services.Interfaces;

namespace MiniERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _service;
        public CustomersController(ICustomerService service) => _service = service;

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var customer = _service.GetById(id);
            return customer == null ? NotFound() : Ok(customer);
        }

        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            var id = _service.Create(customer);
            return CreatedAtAction(nameof(GetById), new { id }, customer);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Customer customer)
        {
            if (id != customer.Id) return BadRequest();
            _service.Update(customer);
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
