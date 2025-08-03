using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dsw2025Tpi.Api.Controllers;

[ApiController]
[Route("api/authenticate")]
public class AuthenticateController : ControllerBase
{
    private readonly JwtTokenService _jwtTokenService;

    public AuthenticateController(JwtTokenService jwtTokenService)
    {
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel request)
    {
        if(request.Username == "admin" && request.Password == "1234")
        {
            var token = _jwtTokenService.GenerateToken(request.Username);
            return Ok(new { token });
        }

        return Unauthorized();
    }   

}
