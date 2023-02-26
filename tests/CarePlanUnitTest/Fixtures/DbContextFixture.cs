
using Application.Common.Interfaces.IRepository;
using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CarePlanUnitTest.Fixtures
{
    public class DbContextFixture : IDisposable
    {
        protected readonly CareContext _context;
        public DbContextFixture()
        {
            var options = new DbContextOptionsBuilder<CareContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            _context = new CareContext(options);

            _context.Database.EnsureCreated();
            Seed(_context).GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        private async Task Seed(CareContext context)
        {
            var stub = DbInitializer.GenerateData(10);

            foreach (var plan in stub)
            {
                await context.Set<PatientCarePlan>().AddAsync(plan);
            }
             

            await context.SaveChangesAsync();
        }

         

    }
}
