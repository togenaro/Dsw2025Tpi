using Dsw2025Tpi.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Dsw2025Tpi.Application.Interfaces;
using Dsw2025Tpi.Application.Services;

namespace Dsw2025Tpi.Api.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    #region Inyección de los servicios
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUserRoleService _userRoleService;

    public AdminController(UserManager<IdentityUser> userManager, 
        RoleManager<IdentityRole> roleManager,
        IUserRoleService userRoleService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _userRoleService = userRoleService;
    }
    #endregion

    [HttpPost("assign-role")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignRoleAsync([FromBody] RoleRequest request)
    {
        var response = await _userRoleService.AssignRoleAsync(request);
        return Ok(response);
    }
}

