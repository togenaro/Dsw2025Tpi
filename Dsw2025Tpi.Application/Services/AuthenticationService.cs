using Dsw2025Tpi.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dsw2025Tpi.Application.Dtos;

namespace Dsw2025Tpi.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtTokenService _jwt;

    public AuthenticationService(UserManager<IdentityUser> um, JwtTokenService jwt)
    {
        _userManager = um;
        _jwt = jwt;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.Username)
            ?? throw new UnauthorizedAccessException("Usuario no encontrado");

        if (!await _userManager.CheckPasswordAsync(user, request.Password))
            throw new UnauthorizedAccessException("Contraseña incorrecta");

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwt.GenerateToken(user.UserName!, roles);

        return new LoginResponse
        (
            token,
            user.UserName!,
            roles
        );
    }

    public async Task RegisterAsync(RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || 
            string.IsNullOrWhiteSpace(request.Email) || 
            string.IsNullOrWhiteSpace(request.Password))
        {
            throw new ArgumentException("Datos incompletos.");
        }
            
        var user = new IdentityUser { UserName = request.Username, Email = request.Email };
        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));
    }
}

