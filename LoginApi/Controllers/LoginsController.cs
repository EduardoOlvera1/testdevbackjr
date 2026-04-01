using Microsoft.AspNetCore.Mvc;
using LoginApi.Services;
using LoginApi.DTOs.Login;

namespace LoginApi.Controllers
{
    [ApiController]
    [Route("logins")]
    public class LoginsController : ControllerBase
    {
        private readonly LoginService _service;

        public LoginsController(LoginService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _service.GetAll();
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateLoginDto dto)
        {
            var result = await _service.Create(dto);

            if (!result.ok)
                return BadRequest(result.error);

            return Ok(result.data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UpdateLoginDto dto)
        {
            var ok = await _service.Update(id, dto);

            if (!ok)
                return NotFound();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.Delete(id);

            if (!ok)
                return NotFound();

            return NoContent();
        }
    }
}