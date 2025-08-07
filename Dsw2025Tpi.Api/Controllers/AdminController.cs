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
    private readonly IAdminService _userRoleService;

    public AdminController(UserManager<IdentityUser> userManager, 
        RoleManager<IdentityRole> roleManager,
        IAdminService userRoleService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _userRoleService = userRoleService;
    }
    #endregion

    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole([FromBody] RoleRequest request)
    {
        var response = await _userRoleService.AssignRole(request);
        return Ok(response);
    }

    [HttpPost("remove-role")]
    public async Task<IActionResult> RemoveRole([FromBody] RoleRequest request)
    {
        var response = await _userRoleService.RemoveRole(request);
        return Ok(response);
    }
}

