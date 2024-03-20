using BookStore.BL.Interfaces;
using BookStore.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("register")]
        public async Task<IActionResult>
            Register([FromBody] UserRegistrationRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            if (string.IsNullOrEmpty(request.UserName) ||
                string.IsNullOrEmpty(request.Password))
            {
                return BadRequest();
            }

            var authResult = await
                _identityService
                    .RegisterAsync(
                        request.UserName,
                        request.Password);

            if (!authResult.IsSuccess) 
                return Problem($"Error registering user!");

            return Ok(authResult);
        }
    }
}
