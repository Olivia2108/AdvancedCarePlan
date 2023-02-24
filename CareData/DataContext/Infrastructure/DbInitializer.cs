﻿using Bogus; 
using CareData.Models;
using CareShared.Middleware.Exceptions;
using Microsoft.EntityFrameworkCore; 
using static CareShared.Middleware.Enums.GeneralEnums;


namespace CareData.DataContext.Infrastructure
{
    public static class DbInitializer
    {
		public static void Initializer(CareContext context)
		{
			SeedEverything(context);
		}


		private static void SeedEverything(CareContext context)
		{
			context.Database.Migrate();

			if (context.CarePlan.Any())
			{
				LoggerMiddleware.LogInfo("Plan exist");
			}
			else
			{
				Seed(context);

			}

		}

		private static void Seed(CareContext context)
		{
			var stub = GenerateData(20);

			foreach (var employee in stub)
			{
				context.CarePlan.AddAsync(employee);
			}

			var ty = context.SaveChangesAsync(stub.FirstOrDefault().IpAddress).GetAwaiter().GetResult();
		}

		public static List<CarePlan> GenerateData(int count)
		{
			var titles = new string[] {
				nameof(Titles.Sir),
				nameof(Titles.Ma),
				nameof(Titles.Mr),
				nameof(Titles.Mrs),
				nameof(Titles.Ms),
				nameof(Titles.Miss),
				nameof(Titles.Madam),
				nameof(Titles.Master),
				nameof(Titles.Dr),
				nameof(Titles.Prof)
			};

			var actions = new string[] {
				nameof(Actions.Create),
				nameof(Actions.Delete),
				nameof(Actions.Update),
			};


			var outcome = new string[] {
				"Alive",
				"Mortality",
				"Patient satisfaction",
				"Functioning well mentally, physically, and socially",
				"Patient functional status (maintained or improved)",
				"Patient safety (protected or unharmed)"
			};
			

			var reason = new string[] {
				"Alive",
				"Mortality",
				"Patient satisfaction",
				"Functioning well mentally, physically, and socially",
				"Patient functional status (maintained or improved)",
				"Patient safety (protected or unharmed)"
			};

			var completed = new bool[] {
				true,
				false,
			};

			var faker = new Faker<CarePlan>()
				.RuleFor(c => c.Title, f => f.PickRandom(titles))
				.RuleFor(c => c.PatientName, f => f.Person.FullName)
				.RuleFor(c => c.UserName, f => f.Person.UserName)
				.RuleFor(c => c.Reason, f => f.PickRandom(reason))
				.RuleFor(c => c.Action, f => f.PickRandom(actions))
				.RuleFor(c => c.IsActive, f => true)
				.RuleFor(c => c.Completed, f => f.PickRandom(completed))
				.RuleFor(c => c.Outcome, f => string.Empty)
				.RuleFor(c => c.IpAddress, f => f.Internet.IpAddress().ToString())
				.RuleFor(c => c.DateCreated, f => f.Date.Recent(5))
				.RuleFor(c => c.TargetStartDate, f => f.Date.Recent(3))
				.RuleFor(c => c.ActualStartDate, f => f.Date.Soon(5));


			var random = new Random();
			var plans = faker.Generate(count);
			plans.Where(x=> x.Completed).ToList().ForEach(async plan =>
			{
				plan.Outcome = outcome
				[
					new Random().Next(0, outcome.Length)
				]; 
				plan.ActualEndDate = plan.ActualStartDate.AddDays(30); 
			});

			plans.Where(x => x.Action == nameof(Actions.Delete)).ToList().ForEach(async plan =>
			{
				plan.IsActive = false;
				plan.DateDeleted = DateTime.Now;
				plan.IsDeleted = true;
			});

			plans.Where(x => x.Action == nameof(Actions.Update)).ToList().ForEach(async plan =>
			{ 
				plan.DateUpdated = DateTime.Now; 
			});


			return plans;

		} 

	}
	 
}