
using CareDomain;
using NLog;

namespace CarePlanAPI
{
	public partial class Program
	{
		private static readonly string? Namespace = typeof(Program).Namespace;
		public static readonly string? AppName = Namespace;

		private static IConfiguration GetConfiguration()
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.Development.json", optional: true)
				.AddEnvironmentVariables();

			return builder.Build();
		}
		private static IConfiguration Configuration { get; set; }

		public static void Main(string[] args)
		{

			Configuration = GetConfiguration();
			try
			{

				var builder = WebApplication.CreateBuilder(args);

				// Add services to the container.

				builder.Services.AddControllers();
				// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
				builder.Services.AddEndpointsApiExplorer(); 


				//Call the Program DI class
				ConfigureDiServices(builder.Services);


				var app = builder.Build();


				SeedDatabase();

				void SeedDatabase()
				{
					using var scope = app.Services.CreateScope();
					//var scopedContext = scope.ServiceProvider.GetRequiredService<HahnContext>();
					//DbInitializer.Initializer(scopedContext);
				}


				// Configure the HTTP request pipeline.
				if (app.Environment.IsDevelopment())
				{
					app.UseSwagger();
					app.UseSwaggerUI();
				}

				app.UseHttpsRedirection();

				app.UseAuthorization();

				app.MapControllers();

				app.Run();
			}
			catch (Exception ex)
			{
				var type = ex.GetType().Name;
				if (type.Equals("StopTheHostException", StringComparison.Ordinal))
				{
					throw;
				}
				LoggerMiddleware.LogError($"Program terminated unexpectedly (ApplicationContext)! with appname {AppName} and Ex.Message being {ex}");
			}
			finally
			{
				LogManager.Shutdown();
			}
		}
	}
}