
using CareShared.Middleware.Exceptions;
using NLog;

namespace CarePlanWeb
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
				builder.Services.AddControllersWithViews()
					.AddRazorRuntimeCompilation();

				 

				var app = builder.Build();

				// Configure the HTTP request pipeline.
				if (!app.Environment.IsDevelopment())
				{
					app.UseExceptionHandler("/Home/Error");
					// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
					app.UseHsts();
				}


				//Configure Exception Middelware
				app.UseMiddleware<ExceptionMiddleware>();
				app.UseHttpsRedirection();
				app.UseStaticFiles();

				app.UseRouting();

				app.UseAuthorization();

				app.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");

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
