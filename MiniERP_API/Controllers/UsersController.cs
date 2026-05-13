using Microsoft.AspNetCore.Mvc;
using MiniERP_API.Models.DTOs;
using MiniERP_API.Services.Interfaces;

namespace MiniERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        public UsersController(IUserService service) => _service = service;

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _service.GetById(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UserUpdateDto dto)
        {
            _service.Update(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return NoContent();
        }

        [HttpPatch("{id}/password")]
        public IActionResult ChangePassword(int id, UserPasswordChangeDto dto)
        {
            try 
            { 
                _service.ChangePassword(id, dto); 
                return Ok(new { message = "Đã đổi mật khẩu thành công." });
            }
            catch (System.Exception ex) 
            { 
                return BadRequest(new { message = ex.Message }); 
            }
        }
    }
}
