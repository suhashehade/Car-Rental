using System.Text;
using CarRenter.DB;
using CarRenter.DB.Configurations;
using CarRenter.DB.Models;
using CarRenter.DB.Repositories;
using CarRenter.DB.Repositories.Interfaces;
using CarRenter.DB.Seed;
using CarRenter.DB.Services;
using CarRenter.DB.Services.Interfaces;
using CarRenter.Endpoints;
using CarRenter.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<ICarService, CarService>();
        
        builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
        builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        
        builder.Services.AddValidatorsFromAssemblies(new[]
        {
            typeof(MainClass).Assembly,                     
            typeof(RegisterUserDtoValidator).Assembly,
            typeof(LoginValidator).Assembly       
        });
        
        var jwtConfig = builder.Configuration.GetSection("JwtConfig").Get<JwtConfig>();


        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtConfig!.Issuer,
        
                    ValidateAudience = true,
                    ValidAudience = jwtConfig.Audience,
        
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key)),
        
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        builder.Services.AddAuthorization();
  
        
        var app = builder.Build();
        
        app.UseAuthentication();
        app.UseAuthorization(); 
        
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            await DatabaseSeeder.SeedRolesAsync(services);
        }

        app.MapGet("/", () => "Hello World!");

        app.MapUserEndpoints();
        app.MapReservationEndpoints();
        app.MapCarEndpoints();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        await app.RunAsync();
    }
}