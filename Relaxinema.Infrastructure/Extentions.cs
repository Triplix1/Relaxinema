using Microsoft.Extensions.DependencyInjection;
using Relaxinema.Core.Domain.RepositoryContracts;
using Relaxinema.Infrastructure.Repositories;

namespace Relaxinema.Infrastructure
{
    public static class Extentions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
