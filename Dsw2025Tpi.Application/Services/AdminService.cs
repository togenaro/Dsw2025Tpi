using Dsw2025Tpi.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dsw2025Tpi.Application.Dtos;

namespace Dsw2025Tpi.Application.Services;

public class AdminService : IAdminService
{
    #region Inyección de dependencias
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminService(UserManager<IdentityUser> um, RoleManager<IdentityRole> rm)
    {
        _userManager = um;
        _roleManager = rm;
    }
    #endregion

    public async Task<RoleResponse> AssignRole(RoleRequest request)
    {
        if(string.IsNullOrWhiteSpace(request.Username) ||
           string.IsNullOrWhiteSpace(request.Role))
            throw new ArgumentException("Datos incompletos.");

        var user = await _userManager.FindByNameAsync(request.Username);
        if (user is null)
            throw new InvalidOperationException("Usuario no encontrado.");

        if (!await _roleManager.RoleExistsAsync(request.Role))
            throw new InvalidOperationException($"El rol '{request.Role}' no existe.");

        if (await _userManager.IsInRoleAsync(user, request.Role))
            throw new InvalidOperationException("El usuario ya tiene ese rol.");

        var result = await _userManager.AddToRoleAsync(user, request.Role);
        if (!result.Succeeded)
            throw new InvalidOperationException($"No se pudo asignar rol.");

        return new RoleResponse
        (
            user.UserName!,
            _userManager.GetRolesAsync(user).Result.FirstOrDefault() ?? string.Empty
        );
    }

    public async Task<RoleResponse> RemoveRole(RoleRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) ||
            string.IsNullOrWhiteSpace(request.Role))
            throw new ArgumentException("Datos incorrectos.");

        var user = await _userManager.FindByNameAsync(request.Username);
        if (user is null)
            throw new InvalidOperationException("Usuario no encontrado.");

        if (!await _roleManager.RoleExistsAsync(request.Role))
            throw new InvalidOperationException($"El rol '{request.Role}' no existe.");

        if (!await _userManager.IsInRoleAsync(user, request.Role))
            throw new InvalidOperationException("El usuario no tiene ese rol.");

        var result = await _userManager.RemoveFromRoleAsync(user, request.Role);
        if (!result.Succeeded)
            throw new InvalidOperationException($"No se pudo eliminar el rol.");

        return new RoleResponse
        (
            user.UserName!,
            _userManager.GetRolesAsync(user).Result.FirstOrDefault() ?? string.Empty
        );
    }

    public Task<RoleResponse> CreateRole(string roleName)
    {
        throw new NotImplementedException();
    }
}
