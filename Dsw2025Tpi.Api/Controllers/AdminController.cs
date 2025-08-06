using Dsw2025Tpi.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dsw2025Tpi.Api.Controllers;

[ApiController]
[AllowAnonymous]
//[Authorize(Roles = "Admin")]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleModel request)
    {
        if(request == null || 
           string.IsNullOrEmpty(request.Username) || 
           string.IsNullOrEmpty(request.Role)) 
            return BadRequest("Datos incompletos para asignar rol.");

        var roleExists = await _roleManager.RoleExistsAsync(request.Role);
        if (!roleExists)
            return NotFound($"El rol '{request.Role}' no existe.");

        var user = await _userManager.FindByNameAsync(request.Username);
        if (user is null) 
            return NotFound("Usuario no encontrado.");

        if (await _userManager.IsInRoleAsync(user, request.Role)) 
            return BadRequest("El usuario ya tiene ese rol.");

        var result = await _userManager.AddToRoleAsync(user, request.Role);
        if (!result.Succeeded) 
            return BadRequest(result.Errors);

        return Ok($"Rol '{request.Role}' asignado al usuario '{request.Username}'.");
    }
}

