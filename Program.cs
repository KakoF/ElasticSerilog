using ElasticSerilog.Common;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.AddDocumentation();

builder.ConfigureLogging();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
	app.ConfigureDevEnvironment();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


/*void ConfigureLogging()
{
	var environment = Environment.GetEnvironmentVariable("APSNETCORE_ENVIRONMENT");
	var configuration = new ConfigurationBuilder()
		.AddJsonFile("appsetings.json", optional: false, reloadOnChange: true)
		.AddJsonFile($"appsetings.{environment}.json", optional: true)
		.Build();

	Log.Logger = new LoggerConfiguration()
		.Enrich.FromLogContext()
		.Enrich.WithExceptionDetails()
		.WriteTo.Debug()
		.WriteTo.Console()
		.WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment!))
		.Enrich.WithProperty("Environment", environment)
		.Enrich.WithProperty("HostName", System.Net.Dns.GetHostName())
		.ReadFrom.Configuration(configuration: configuration)
		.CreateLogger();
}

ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
{
	return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]!)) 
	{
		AutoRegisterTemplate = true,
		IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name!.ToLower().Replace(".", "-")}-{environment.ToLower()}-{DateTime.UtcNow:yyyy-MM}",
		NumberOfReplicas = 1,
		NumberOfShards = 2,
	};
}*/