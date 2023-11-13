using Microsoft.AspNetCore.Mvc;
using Relaxinema.Core.Extentions;
using Relaxinema.Core.Helpers;
using Relaxinema.Infrastructure;
using Relaxinema.WebAPI.Filters;
using Relaxinema.WebAPI.Middlewares;

namespace Relaxinema.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(new ValidateModelsFilterAttribute());
            });
            builder.Services.AddCore(builder.Configuration);
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithOrigins("http://localhost:4200"));

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

// "id": "a8606cdb-28c7-4292-979d-08dbd94d3239",
// "name": "Астрал",
// "year": 2022,
// "photoUrl": "string",
// "limitation": 18,
// "description": "Містичний фільм «Астрал» познайомить глядачів з загадками потойбічного світу. Головними героями картини виступають члени сім'ї викладача Джоша: його дружина Рене і троє їхніх дітей. Після переїзду в новий будинок, Рене зауважує дивні речі, що відбуваються в ньому. Одного разу її син Далтон забирається на горище і ненавмисно падає звідти. Наступного ранку дитина не може навіть прокинутися. Лікарі, викликані його батьками, точний діагноз хлопчика не встановлюють. Рене вмовляє Джоша перебратися в інший будинок, вважаючи його причиною усіх нещасть, що звалилися на сімейство. У новому житлі все повторюється знову: паранормальні явища і раніше переслідують господарів. Зневірившись, Рене запрошує фахівця з області привидів Еліс Рейнер. Оглянувши Далтона, вона розповідає батькам про астрал, в який потрапила його душа, завдяки надзвичайним здібностям. Виявляється, хлопчик і раніше міг туди проникати, але завжди повертався назад ...",
// "publish": true,
// "isExpected": false,
// "file": "string",
// "sources": [
// "<iframe id="pre" width="720" height="400" src="https://ashdi.vip/vod/8402" frameborder="0" allowfullscreen=""></iframe>"
//     ],
// "trailer": "<iframe width="560" height="315" src="https://www.youtube.com/embed/62rpZcMYa0A?si=2j_9JZCw0fWjojit" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share" allowfullscreen></iframe>",
// "genreNames": [
// "Хорор"
//     ]