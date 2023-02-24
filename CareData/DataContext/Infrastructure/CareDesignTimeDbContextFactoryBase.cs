using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CareData.DataContext.Infrastructure
{
    public abstract class CareDesignTimeDbContextFactoryBase<TContext> : IDesignTimeDbContextFactory<TContext> where TContext : DbContext
	{
		private const string ConnectionStringName = "CareConn";
		private const string AspNetCoreEnvironment = "ASPNETCORE_ENVIRONMENT";

		public TContext CreateDbContext(string[] args)
		{
			var basePath = Directory.GetCurrentDirectory();
			return Create(basePath, Environment.GetEnvironmentVariable(AspNetCoreEnvironment));
		}

		protected abstract TContext CreateNewInstance(DbContextOptions<TContext> options);

		private TContext Create(string basePath, string? environmentName)
		{

			var configuration = new ConfigurationBuilder()
				.SetBasePath(basePath)
				.AddJsonFile("appsettings.json", false, true)
				.AddJsonFile($"appsettings.{environmentName}.json", true)
				.AddEnvironmentVariables()
				.Build();

			var connectionString = configuration.GetConnectionString(ConnectionStringName);

			return Create(connectionString);
		}

		private TContext Create(string connectionString)
		{
			if (string.IsNullOrEmpty(connectionString))
			{
				throw new ArgumentException($"Connection string '{ConnectionStringName}' is null or empty.", nameof(connectionString));
			}

			Console.WriteLine($"DesignTimeDbContextFactoryBase.Create(string): Connection string: '{connectionString}'.");

			var optionsBuilder = new DbContextOptionsBuilder<TContext>();

			optionsBuilder.UseSqlServer(connectionString);

			return CreateNewInstance(optionsBuilder.Options);
		}
	}
}
