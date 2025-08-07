using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Interfaces;
using Dsw2025Tpi.Application.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dsw2025Tpi.Api.Controllers;

[ApiController]
[Route("api/authenticate")]
public class AuthenticateController : ControllerBase
{
    #region Inyección de servicios
    private readonly IAuthenticationService _authService;

    public AuthenticateController(IAuthenticationService authService)
    {
        _authService = authService;
    }
    #endregion

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var res = await _authService.LoginAsync(request);
        return Ok(res);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        await _authService.RegisterAsync(request);
        return Ok("Usuario registrado correctamente");
    }
}
