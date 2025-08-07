using Dsw2025Tpi.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Dsw2025Tpi.Application.Services;

namespace Dsw2025Tpi.Api.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    #region Inyección de los servicios
    private readonly AdminService _service;

    public AdminController(AdminService adminManager)
    {
        _service = adminManager;
    }
    #endregion

    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole([FromBody] RoleRequest request)
    {
        var response = await _service.AssignRole(request);
        return Ok(response);
    }

    [HttpPost("remove-role")]
    public async Task<IActionResult> RemoveRole([FromBody] RoleRequest request)
    {
        var response = await _service.RemoveRole(request);
        return Ok(response);
    }
}

