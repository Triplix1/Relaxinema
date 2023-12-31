using Microsoft.EntityFrameworkCore;
using Relaxinema.Core.Extentions;
using Relaxinema.Core.Helpers;
using Relaxinema.Core.MailConfig;
using Relaxinema.Core.ServiceContracts;
using Relaxinema.Infrastructure;
using Relaxinema.Infrastructure.DatabaseContext;
using Relaxinema.Infrastructure.Seed;
using Relaxinema.WebAPI.Filters;
using Relaxinema.WebAPI.Middlewares;

namespace Relaxinema.WebAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // builder.Services.AddControllers();
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(new ValidateModelsFilterAttribute());
            });
            
            builder.Services.AddCore(builder.Configuration);
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
            builder.Services.Configure<MailConfig>(builder.Configuration.GetSection("MailConfig"));
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            
            app.UseMiddleware<ExceptionMiddleware>();
            
            app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithOrigins("http://localhost:4201", "http://localhost:4200", "https://relaxinema.onrender.com"));

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();
            
            using var scope = app.Services.CreateScope();
            var services  = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                var rolesService = services.GetRequiredService<IRoleService>();
                await context.Database.MigrateAsync();
                await Seed.SeedUsers(context, rolesService);
            }
            catch (Exception ex)
            {
                var logger = services.GetService<ILogger<Program>>();
                logger.LogError(ex, "An error occured during migration");
            }

            app.Run();
        }
    }
}