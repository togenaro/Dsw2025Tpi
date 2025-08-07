using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Data.Seeders;

public static class IdentitySeeder
{
    public static async Task SeedRolesAsync(IServiceProvider provider)
    {
        var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roles = { "Admin", "User" };

        foreach (var r in roles)
        {
            if (!await roleManager.RoleExistsAsync(r))
                await roleManager.CreateAsync(new IdentityRole(r));
        }
    }
}
