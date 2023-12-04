using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Relaxinema.Core.Domain.RepositoryContracts;
using Relaxinema.Infrastructure.DatabaseContext;
using Relaxinema.Infrastructure.Repositories;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Relaxinema.Core.Domain.Entities;

namespace Relaxinema.Infrastructure
{
    public static class Extentions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IFilmRepository, FilmRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            //services.AddScoped<IRoleRepository, RoleRepository>();

            // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            // .AddJwtBearer(options =>
            // {
            //     options.TokenValidationParameters = new TokenValidationParameters
            //     {
            //         ValidateIssuerSigningKey = true,
            //         IssuerSigningKey =
            //             new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
            //         ValidateIssuer = false,
            //         ValidateAudience = false
            //     };
            // });
            
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });
            
            services.AddIdentity<User, IdentityRole<Guid>>(options =>
                {
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 5;
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddUserStore<UserStore<User, IdentityRole<Guid>, ApplicationDbContext, Guid>>()
                .AddRoleStore<RoleStore<IdentityRole<Guid>, ApplicationDbContext, Guid>>();
            
            services.AddAuthentication(options => {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = true,
                        ValidAudience = config["Jwt:Audience"],
                        ValidateIssuer = true,
                        ValidIssuer = config["Jwt:Issuer"],
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(config["Jwt:Key"]))
                    };
                });

            services.AddAuthorization();

            return services;
        }
    }
}
