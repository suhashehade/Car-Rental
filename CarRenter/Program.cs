using CarRenter.DB;
using CarRenter.DB.Models;
using CarRenter.DB.Seed;
using CarRenter.DB.Services;
using CarRenter.DB.Services.Interfaces;
using CarRenter.Endpoints;
using CarRenter.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarRenter;

internal abstract class MainClass
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddOpenApi();
        
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<CarRenterDbContext>(options =>
            options.UseSqlServer(connectionString));
        
        builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<CarRenterDbContext>() 
            .AddDefaultTokenProviders();
       
        
        builder.Services.AddScoped<IUserService, UserService>();
        
        builder.Services.AddValidatorsFromAssembly(typeof(MainClass).Assembly);
        
        var app = builder.Build();
        
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            await DatabaseSeeder.SeedRolesAsync(services);
        }

        app.MapGet("/", () => "Hello World!");
        
        app.MapUserEndpoints();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        await app.RunAsync();
    }
}