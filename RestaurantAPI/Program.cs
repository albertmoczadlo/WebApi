using Microsoft.EntityFrameworkCore;
using NLog.Web;
using RestaurantAPI.Entities;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Middleware;
using RestaurantAPI.Middleware;
using RestaurantAPI.Services;

namespace RestaurantAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddScoped<ErrorHandlingMiddlewere>();
        builder.Services.AddDbContext<RestaurantDbContext>(
            options=>options.UseSqlServer(builder.Configuration.GetConnectionString(
                "RestaurantDbContextConnection")?? throw new InvalidOperationException("Connection not found")));
        builder.Services.AddScoped<RestaurantSeeder>();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddScoped<IRestaurantService, RestaurantService>();
        builder.Services.AddScoped<RequestTimeMiddleware>();
        builder.Services.AddScoped<IDishService, DishService>();

        builder.Logging.ClearProviders();
        builder.Logging.SetMinimumLevel(LogLevel.Trace);
        builder.Host.UseNLog();


        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var seeder = scope.ServiceProvider.GetService<RestaurantSeeder>();
            seeder.Seed();
        }


        app.UseMiddleware<RequestTimeMiddleware>();
        app.UseMiddleware<ErrorHandlingMiddlewere>();
        app.UseHttpsRedirection();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Restaurant API");
                options.RoutePrefix = string.Empty;
            });
        }

        app.MapControllers();

        app.Run();
    }
}
