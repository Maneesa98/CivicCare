using CivicCare.Api.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CivicCare.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterRequest command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequest command)
        {
            var result = await _mediator.Send(command);
            return result != null ? Ok(result) : Unauthorized();
        }
    }
}