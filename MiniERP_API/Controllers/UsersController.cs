using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniERP_API.Models.DTOs;
using MiniERP_API.Services.Interfaces;

namespace MiniERP_API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        public UsersController(IUserService service) => _service = service;

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());
        
        [HttpPost]
        public IActionResult Create(RegisterRequest dto)
        {
            try
            {
                var id = _service.Create(dto);
                return CreatedAtAction(nameof(GetById), new { id }, new { id, message = "Đã tạo tài khoản thành công." });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

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
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId != null && int.Parse(currentUserId) == id)
            {
                return BadRequest(new { message = "Bạn không thể xóa tài khoản của chính mình." });
            }

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
