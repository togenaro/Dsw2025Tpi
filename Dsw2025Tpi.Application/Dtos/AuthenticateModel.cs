using Microsoft.Identity.Client;

namespace Dsw2025Tpi.Application.Dtos;

public record LoginRequest(string Username, string Password);

public record LoginResponse
(
    string Token,
    string Username,
    IEnumerable<string> Roles
);

public record RegisterRequest(string Username, string Password, string Email);
