using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.Domain.RepositoryContracts;
using Relaxinema.Core.ServiceContracts;
using Relaxinema.Infrastructure.DatabaseContext;

namespace Relaxinema.Infrastructure.Seed;

public class Seed
{

    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public Seed(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedUsersAsync()
    {
        if (await _userManager.Users.AnyAsync()) return;

        var userData = await File.ReadAllTextAsync("D:\\Study\\Relaxinema\\Relaxinema.Infrastructure\\Seed\\UserSeedData.json");

        var options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};

        var users = JsonSerializer.Deserialize<List<User>>(userData, options);

        foreach (var user in users)
        {
            user.UserName = user.Nickname;
            var result = await _userManager.CreateAsync(user, "example");
            await _roleManager.CreateAsync(new IdentityRole<Guid>("admin"));
            
            if(result.Succeeded)
            {
                 var roleResult = await _userManager.AddToRoleAsync(user, UserRole.Admin.ToString().ToLower());
                 if(roleResult.Succeeded) Console.Write(" ");
            }
        }
    }
}