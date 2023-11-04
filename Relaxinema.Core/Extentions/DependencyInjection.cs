using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Relaxinema.Core.Helpers;
using Relaxinema.Core.ServiceContracts;
using Relaxinema.Core.Services;

namespace Relaxinema.Core.Extentions
{
    public static class DependencyInjection
    {
        public static readonly Assembly Assembly = Assembly.GetExecutingAssembly();

        public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration config)
        {
            services.AddAutoMapper(Assembly);
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IFilmService, FilmService>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IPhotoService, PhotoService>();
            
            return services;
        }
    }
}
