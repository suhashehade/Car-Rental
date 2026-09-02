using CarRenter.DB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CarRenter.DB.Seed;

public static class DatabaseSeeder
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Preference>().HasData(
            new Preference { Id = "11111111-1111-1111-1111-111111111111", PreferenceName = "GPS Navigation System" },
            new Preference { Id = "22222222-2222-2222-2222-222222222222", PreferenceName = "Child Safety Seat" },
            new Preference { Id = "33333333-3333-3333-3333-333333333333", PreferenceName = "Full Coverage Insurance" },
            new Preference { Id = "44444444-4444-4444-4444-444444444444", PreferenceName = "Additional Driver" },
            new Preference { Id = "55555555-5555-5555-5555-555555555555", PreferenceName = "Wi-Fi Hotspot" },
            new Preference { Id = "66666666-6666-6666-6666-666666666666", PreferenceName = "Roadside Assistance" },
            new Preference { Id = "77777777-7777-7777-7777-777777777777", PreferenceName = "Roof Luggage Rack" },
            new Preference { Id = "88888888-8888-8888-8888-888888888888", PreferenceName = "Snow Chains / Winter Tires" },
            new Preference { Id = "99999999-9999-9999-9999-999999999999", PreferenceName = "Non-Smoking Vehicle" },
            new Preference { Id = "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", PreferenceName = "Pet Friendly" }
        );
        
        modelBuilder.Entity<Car>().HasData(
            new Car 
            { 
                Id = "c1111111-1111-1111-1111-111111111111", 
                Brand = "Toyota", 
                Model = "Camry", 
                Color = "Silver", 
                HourlyPrice = 12.50m 
            },
            new Car 
            { 
                Id = "c2222222-2222-2222-2222-222222222222", 
                Brand = "Hyundai", 
                Model = "Elantra", 
                Color = "White", 
                HourlyPrice = 10.00m 
            },
            new Car 
            { 
                Id = "c3333333-3333-3333-3333-333333333333", 
                Brand = "Honda", 
                Model = "Civic", 
                Color = "Black", 
                HourlyPrice = 11.00m 
            },
            new Car 
            { 
                Id = "c4444444-4444-4444-4444-444444444444", 
                Brand = "BMW", 
                Model = "Series 5", 
                Color = "Dark Blue", 
                HourlyPrice = 28.00m 
            },
            new Car 
            { 
                Id = "c5555555-5555-5555-5555-555555555555", 
                Brand = "Mercedes-Benz", 
                Model = "C-Class", 
                Color = "Grey", 
                HourlyPrice = 30.00m 
            },
            new Car 
            { 
                Id = "c6666666-6666-6666-6666-666666666666", 
                Brand = "Kia", 
                Model = "Sportage", 
                Color = "Red", 
                HourlyPrice = 15.00m 
            },
            new Car 
            { 
                Id = "c7777777-7777-7777-7777-777777777777", 
                Brand = "Nissan", 
                Model = "Rogue", 
                Color = "White", 
                HourlyPrice = 14.50m 
            },
            new Car 
            { 
                Id = "c8888888-8888-8888-8888-888888888888", 
                Brand = "Ford", 
                Model = "Mustang", 
                Color = "Yellow", 
                HourlyPrice = 35.00m 
            }
        );
    }
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roleNames = ["Admin", "Customer"];

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}