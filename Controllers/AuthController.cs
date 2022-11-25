using Diary.DTOs;
using Diary.Models;
using Diary.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Diary.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("api/login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<LoginResponseDTO>>Login(LoginRequestDTO loginRequestDTO)
        {
            var response = await _authService.Login(loginRequestDTO);
            if(!response.Success)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response); 
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [AllowAnonymous]
        [HttpPost("api/register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<RegisterResponseDTO>>Register(RegisterRequestDTO registerRequestDTO)
        {
            var response = await _authService.Register(registerRequestDTO);
            if(!response.Success)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response); 
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }
    }
}