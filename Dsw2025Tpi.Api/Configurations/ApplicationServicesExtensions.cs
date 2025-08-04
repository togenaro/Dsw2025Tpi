namespace Dsw2025Tpi.Api.Configurations;

using Microsoft.AspNetCore.Identity;

public static class ApplicationServicesExtensions
{
    public static void SeedRoles(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

        string[] roles = { "Admin", "User" };

        foreach (var role in roles)
        {
            var roleExists = roleManager.RoleExistsAsync(role).Result;
            if (!roleExists)
                roleManager.CreateAsync(new IdentityRole(role)).Wait();
        }

    }
}
