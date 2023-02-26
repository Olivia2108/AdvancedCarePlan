using AutoMapper; 
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure.RegisterServices
{
	public static class InfrastructureDependency
	{
		public static IServiceCollection AddDependency(this IServiceCollection services, IConfiguration? configuration)
		{

			 
			services.Scan(i =>
			{
				i.FromCallingAssembly().AddClasses(x => x.Where(p => p.IsClass && p.Name.EndsWith("Repository")))
					.AsImplementedInterfaces()
					.WithScopedLifetime();
			});
			services.Scan(i =>
			{
				i.FromCallingAssembly().AddClasses(x => x.Where(p => p.IsClass && p.Name.EndsWith("Service")))
					.AsImplementedInterfaces()
					.WithScopedLifetime();
			});

			services.Scan(i =>
			{
				i.FromCallingAssembly().AddClasses(x => x.Where(p => p.IsClass && p.Name.EndsWith("Context")))
					.AsImplementedInterfaces()
					.WithScopedLifetime();
			});

			services.AddAutoMapper(typeof(InfrastructureDependency));
			return services;


		}
	}
}
