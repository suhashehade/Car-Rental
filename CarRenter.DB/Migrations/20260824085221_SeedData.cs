using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarRenter.DB.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Car",
                columns: new[] { "Id", "Brand", "Color", "HourlyPrice", "Model" },
                values: new object[,]
                {
                    { "c1111111-1111-1111-1111-111111111111", "Toyota", "Silver", 12.50m, "Camry" },
                    { "c2222222-2222-2222-2222-222222222222", "Hyundai", "White", 10.00m, "Elantra" },
                    { "c3333333-3333-3333-3333-333333333333", "Honda", "Black", 11.00m, "Civic" },
                    { "c4444444-4444-4444-4444-444444444444", "BMW", "Dark Blue", 28.00m, "Series 5" },
                    { "c5555555-5555-5555-5555-555555555555", "Mercedes-Benz", "Grey", 30.00m, "C-Class" },
                    { "c6666666-6666-6666-6666-666666666666", "Kia", "Red", 15.00m, "Sportage" },
                    { "c7777777-7777-7777-7777-777777777777", "Nissan", "White", 14.50m, "Rogue" },
                    { "c8888888-8888-8888-8888-888888888888", "Ford", "Yellow", 35.00m, "Mustang" }
                });

            migrationBuilder.InsertData(
                table: "Preference",
                columns: new[] { "Id", "PreferenceName" },
                values: new object[,]
                {
                    { "11111111-1111-1111-1111-111111111111", "GPS Navigation System" },
                    { "22222222-2222-2222-2222-222222222222", "Child Safety Seat" },
                    { "33333333-3333-3333-3333-333333333333", "Full Coverage Insurance" },
                    { "44444444-4444-4444-4444-444444444444", "Additional Driver" },
                    { "55555555-5555-5555-5555-555555555555", "Wi-Fi Hotspot" },
                    { "66666666-6666-6666-6666-666666666666", "Roadside Assistance" },
                    { "77777777-7777-7777-7777-777777777777", "Roof Luggage Rack" },
                    { "88888888-8888-8888-8888-888888888888", "Snow Chains / Winter Tires" },
                    { "99999999-9999-9999-9999-999999999999", "Non-Smoking Vehicle" },
                    { "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", "Pet Friendly" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Car",
                keyColumn: "Id",
                keyValue: "c1111111-1111-1111-1111-111111111111");

            migrationBuilder.DeleteData(
                table: "Car",
                keyColumn: "Id",
                keyValue: "c2222222-2222-2222-2222-222222222222");

            migrationBuilder.DeleteData(
                table: "Car",
                keyColumn: "Id",
                keyValue: "c3333333-3333-3333-3333-333333333333");

            migrationBuilder.DeleteData(
                table: "Car",
                keyColumn: "Id",
                keyValue: "c4444444-4444-4444-4444-444444444444");

            migrationBuilder.DeleteData(
                table: "Car",
                keyColumn: "Id",
                keyValue: "c5555555-5555-5555-5555-555555555555");

            migrationBuilder.DeleteData(
                table: "Car",
                keyColumn: "Id",
                keyValue: "c6666666-6666-6666-6666-666666666666");

            migrationBuilder.DeleteData(
                table: "Car",
                keyColumn: "Id",
                keyValue: "c7777777-7777-7777-7777-777777777777");

            migrationBuilder.DeleteData(
                table: "Car",
                keyColumn: "Id",
                keyValue: "c8888888-8888-8888-8888-888888888888");

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: "11111111-1111-1111-1111-111111111111");

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: "22222222-2222-2222-2222-222222222222");

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: "33333333-3333-3333-3333-333333333333");

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: "44444444-4444-4444-4444-444444444444");

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: "55555555-5555-5555-5555-555555555555");

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: "66666666-6666-6666-6666-666666666666");

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: "77777777-7777-7777-7777-777777777777");

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: "88888888-8888-8888-8888-888888888888");

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: "99999999-9999-9999-9999-999999999999");

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        }
    }
}
