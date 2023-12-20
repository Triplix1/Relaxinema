using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.ServiceContracts;
using Relaxinema.Infrastructure.DatabaseContext;

namespace Relaxinema.Infrastructure.Seed;

public static class Seed
{
    public static async Task SeedUsers(ApplicationDbContext context, IRoleService roleService)
    {
        if (await context.Users.AnyAsync()) return;

        var userData = await File.ReadAllTextAsync("UserSeedData.json");

        var options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};

        var users = JsonSerializer.Deserialize<List<User>>(userData, options);

        foreach (var user in users)
        {
            using var hmac = new HMACSHA512();
            
            user.Nickname = user.Nickname.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("example"));
            user.PasswordSalt = hmac.Key;

            context.Users.Add(user);
            await context.SaveChangesAsync();
            await roleService.AddToRoleAsync(user.Id, "admin");
            await context.SaveChangesAsync();
        }
    }
}