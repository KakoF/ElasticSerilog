
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace ElasticSerilog.Common
{
    public static class BuildExtension
    {
        public static void ConfigureLogging(this WebApplicationBuilder builder)
        {
			var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
			var configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{environment}.json", optional: true)
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

			builder.Host.UseSerilog();

		}

		public static void AddDocumentation(this WebApplicationBuilder builder)
        {
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
		}

        public static void AddDataContexts(this WebApplicationBuilder builder)
        {


        }

        public static void AddCrossOrigin(this WebApplicationBuilder builder)
        {

        }

        public static void AddServices(this WebApplicationBuilder builder)
        {

        }

		public static ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
		{
			return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]!))
			{
				AutoRegisterTemplate = true,
				IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name!.ToLower().Replace(".", "-")}-{environment.ToLower()}-{DateTime.UtcNow:yyyy-MM}",
				NumberOfReplicas = 1,
				NumberOfShards = 2,
			};
		}
	}
}
