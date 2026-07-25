using ContentHub.Application.Authorization;
using ContentHub.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Infrastructure.Identity;

/// <summary>
/// Seeds the initial Identity data.
/// </summary>
public sealed class IdentityDataSeeder
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<IdentityDataSeeder> _logger;

    public IdentityDataSeeder(RoleManager<IdentityRole<Guid>> roleManager, UserManager<AppUser> userManager, ILogger<IdentityDataSeeder> logger)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        await SeedRolesAsync();

        await SeedAdministratorAsync();
    }

    private async Task SeedRolesAsync()
    {
        var roles = new[]
        {
            Roles.Admin,
            Roles.Author,
            Roles.User
        };

        foreach (var role in roles)
        {
            if (await _roleManager.RoleExistsAsync(role))
                continue;

            var result = await _roleManager.CreateAsync(new IdentityRole<Guid>(role));

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(x => x.Description));

                throw new InvalidOperationException(errors);
            }

            _logger.LogInformation("Role '{Role}' created successfully.", role);
        }

    }

    private async Task SeedAdministratorAsync()
    {
        const string email = "admin@gmail.com";

        if (await _userManager.FindByEmailAsync(email) is not null)
            return;

        var administrator = new AppUser(
            firstName: "System",
            lastName: "Administrator",
            email: email)
        { UserName = email };


        var result = await _userManager.CreateAsync(administrator, "Admin123");

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(x => x.Description));

            throw new InvalidOperationException(errors);
        }

        await _userManager.AddToRoleAsync(administrator, Roles.Admin);

        _logger.LogInformation("Administrator account created.");
    }
}
