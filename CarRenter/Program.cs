namespace CarRenter;

internal abstract class  MainClass
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);
		
		builder.Services.AddOpenApi();
		
		var app = builder.Build();
		app.MapGet("/", () => "Hello World!");
		if (app.Environment.IsDevelopment())
		{
			app.MapOpenApi();
		}

		app.UseHttpsRedirection();


		app.Run();
		
    }
}