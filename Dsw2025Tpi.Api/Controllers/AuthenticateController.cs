using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dsw2025Tpi.Api.Controllers;

[ApiController]
[Route("api/authenticate")]
public class AuthenticateController : ControllerBase
{
    #region Inyección de servicios
    private readonly AuthenticationService _service;

    public AuthenticateController(AuthenticationService authService)
    {
        _service = authService;
    }
    #endregion

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var res = await _service.Login(request);
        return Ok(res);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        await _service.Register(request);
        return Ok("Usuario registrado correctamente");
    }
}
