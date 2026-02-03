using ElasticSerilog.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.AddDocumentation();

builder.ConfigureLogging();
builder.AddClients();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
	app.ConfigureDevEnvironment();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
