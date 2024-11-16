
using System.Text.Json;
using Lemon.Backend.Entities;
using Lemon.Backend.Services;
using Microsoft.EntityFrameworkCore;

namespace Lemon.Backend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Data source services
        builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            // Use snake_case for JSON serialization
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
            options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower;
        });

        // Configure PostgreSQL database
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<LemonDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            // Configure the HTTP request pipeline.
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();

            // Configure migration automatically
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<LemonDbContext>();
            dbContext.Database.Migrate();
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
