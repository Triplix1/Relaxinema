using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Relaxinema.Core.Domain.RepositoryContracts;
using Relaxinema.Infrastructure.DatabaseContext;
using Relaxinema.Infrastructure.Repositories;
using System.Text;

namespace Relaxinema.Infrastructure
{
    public static class Extentions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFilmRepository, FilmRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();

            // services.AddDbContext<ApplicationDbContext>(options =>
            // {
            //     options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            // });
            
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(config.GetSection("ConnectionStrings").Value);
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            return services;
        }
    }
}
