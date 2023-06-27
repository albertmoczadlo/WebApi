using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;

namespace RestaurantAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<RestaurantDbContext>(
            options=>options.UseSqlServer(builder.Configuration.GetConnectionString(
                "RestaurantDbContextConnection")?? throw new InvalidOperationException("Connection not found")));
        builder.Services.AddScoped<RestaurantSeeder>();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var seeder = scope.ServiceProvider.GetService<RestaurantSeeder>();
            seeder.Seed();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Restaurant API");
                options.RoutePrefix = string.Empty;
            });
        }

        app.UseHttpsRedirection();

        app.MapControllers();

        app.Run();
    }
}
