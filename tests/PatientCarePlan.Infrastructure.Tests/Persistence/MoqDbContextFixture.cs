using Application.Common.Interfaces.IRepository;
using Castle.Core.Resource;
using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientCarePlan.Infrastructure.Tests.Persistence
{
    public class MoqDbContextFixture : IDisposable
    {
        protected readonly CareContext _context;
        public MoqDbContextFixture()
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
                await context.Set<PatientCarePlans>().AddAsync(plan);
            }


            await context.SaveChangesAsync();
        }
         

        public static Tuple<IPatientCarePlanRepository, List<PatientCarePlans>, Mock<CareContext>> GetMock()
        {
            var stub = DbInitializer.GenerateData(10);


            var data = stub.AsQueryable().BuildMock();
            var mockDbContext = new Mock<CareContext>();

            var mockSet = new Mock<DbSet<PatientCarePlans>>();

            mockSet
                .As<IQueryable<PatientCarePlans>>()
                .Setup(m => m.Provider)
                .Returns(data.Provider);

            mockSet
                .As<IQueryable<PatientCarePlans>>()
                .Setup(m => m.ElementType)
                .Returns(data.ElementType);

            mockSet
                .As<IQueryable<PatientCarePlans>>()
                .Setup(m => m.Expression)
                .Returns(data.Expression);

            mockSet
                .As<IQueryable<PatientCarePlans>>()
                .Setup(m => m.GetEnumerator())
                .Returns(data.GetEnumerator());


            mockDbContext
                .Setup(o => o.Set<PatientCarePlans>())
                .Returns(() => mockSet.Object);


            mockDbContext
                .Setup(x => x.SaveChanges())
                .Returns(1);

             
            mockDbContext
                .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(() => Task.Run(() => { return 1; })).Verifiable();
 


            IPatientCarePlanRepository repository = new PatientCarePlanRepository(mockDbContext.Object);

            return new Tuple<IPatientCarePlanRepository, List<PatientCarePlans>, Mock<CareContext>>(repository, stub, mockDbContext);
        }

         
    }
}
