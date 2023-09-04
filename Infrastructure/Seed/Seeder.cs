using Domain.Entities.User;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Seed;

public class Seeder
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public Seeder(DataContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedRole()
    {
        var newroles = new List<IdentityRole>()
        {
            new IdentityRole(Roles.Admin),
            new IdentityRole(Roles.User)
        };

        var existing = _roleManager.Roles.ToList();
        foreach (var role in newroles)
        {
            if (existing.Exists(e => e.Name == role.Name) == false)
            {
                await _roleManager.CreateAsync(role);
            }
        }
    }

    public async Task SeedUser()
    {
        var existing = await _userManager.FindByNameAsync("admin");
        if (existing != null) return;

        var identity = new User()
        {
            
            UserName = "admin",
            PhoneNumber = "+992005442641",
            Email = "admin@gmail.com",
            DateRegistred = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(identity, "hello123");
        await _userManager.AddToRoleAsync(identity, Roles.Admin);
    }
}

public class Roles
{
    public const string Admin = "Admin";
    public const string User = "User";
}