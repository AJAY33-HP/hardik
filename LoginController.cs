using Microsoft.AspNetCore.Mvc;
using wipmanagement.api.DTOs;
using wipmanagement.api.Interfaces.Services;

namespace wipmanagement.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IAuthService _authService;

        public LoginController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(LoginResponse), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Login([FromBody] LoginEmailRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var res = await _authService.AuthenticateByEmailAsync(request);
            if (res == null)
            {
                return Unauthorized();
            }

            return Ok(res);
        }
    }
}
