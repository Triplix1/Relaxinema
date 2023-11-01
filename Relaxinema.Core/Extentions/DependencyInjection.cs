using Microsoft.Extensions.DependencyInjection;
using Relaxinema.Core.Domain.RepositoryContracts;
using System.Reflection;
using Relaxinema.Core.ServiceContracts;
using Relaxinema.Core.Services;

namespace Relaxinema.Core.Extentions
{
    public static class DependencyInjection
    {
        public static readonly Assembly Assembly = Assembly.GetExecutingAssembly();

        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly);
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IFilmService, FilmService>();
            services.AddScoped<IGenreService, GenreService>();

            return services;
        }
    }
}
