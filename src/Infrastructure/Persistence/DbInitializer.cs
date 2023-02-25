using Bogus;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using static Domain.Enums.GeneralEnums;

namespace Infrastructure.Persistence
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
            var stub = GenerateData(10);

            foreach (var employee in stub)
            {
                context.CarePlan.AddAsync(employee);
            }

            var ty = context.SaveChangesAsync(stub.FirstOrDefault().IpAddress).GetAwaiter().GetResult();
        }

        public static List<PatientCarePlan> GenerateData(int count)
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
                "Triage setup",
                "Medication prescribed",
                "Injection prescribed",
                "Emotional support provided",
                "Monitoring technology used",
                "Proper handwashing procedures used",
                "Support provided"
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

            var faker = new Faker<PatientCarePlan>()
                .RuleFor(c => c.Title, f => f.PickRandom(titles))
                .RuleFor(c => c.PatientName, f => f.Person.FullName)
                .RuleFor(c => c.UserName, f => f.Person.UserName)
                .RuleFor(c => c.Reason, f => f.PickRandom(reason))
                .RuleFor(c => c.Action, f => f.PickRandom(actions))
                .RuleFor(c => c.IsActive, f => true)
                .RuleFor(c => c.Completed, f => f.PickRandom(completed))
                .RuleFor(c => c.IpAddress, f => f.Internet.IpAddress().ToString())
                .RuleFor(c => c.DateCreated, f => f.Date.Recent(5))
                .RuleFor(c => c.TargetStartDate, f => f.Date.Recent(3))
                .RuleFor(c => c.ActualStartDate, f => f.Date.Soon(5));


            var random = new Random();
            var plans = faker.Generate(count);
            plans.Where(x => x.Completed).ToList().ForEach(async plan =>
            {
                plan.Outcome = outcome
                [
                    new Random().Next(0, outcome.Length)
                ];
                plan.ActualEndDate = plan.ActualStartDate.AddDays(30);
            });


            return plans;

        }


        public static int GiveMeANumber(List<PatientCarePlan> patients)
        {
            List<long> IdList = patients.Select(person => person.Id).ToList();
            var myArray = IdList.ToArray();
            var exclude = new HashSet<long>(myArray);
            var range = Enumerable.Range(1, 100).Where(i => !exclude.Contains(i));

            var rand = new Random();
            int index = rand.Next(0, 100 - exclude.Count);
            return range.ElementAt(index);
        }
    }

}
