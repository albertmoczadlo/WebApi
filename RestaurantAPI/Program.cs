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


        var app = builder.Build();
        // Configure the HTTP request pipeline.

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

        //app.UseSwagger();
        //app.UseSwaggerUI(c => {
        //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        //    c.RoutePrefix = string.Empty;
        //});

        //if (app.Environment.IsDevelopment())
        //{
        //    app.UseSwagger();
        //    app.UseSwaggerUI();
        //}

        //app.UseSwaggerUI(options =>
        //{
        //    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Restaurant API");
        //    options.RoutePrefix = string.Empty;
        //});

        app.UseHttpsRedirection();

        app.MapControllers();

        app.Run();
    }
}
