
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
using System.Text;
using System.Threading.Tasks;

namespace CarePlanUnitTest.Fixtures
{
    public class MoqDbContextFixture : IDisposable
    {
        protected readonly CareContext _mockDbContext;
        //private readonly Mock<CareContext> _dbContext;
        public MoqDbContextFixture()
        {
            _mockDbContext = new Mock<CareContext>().Object;
            //_dbContext = new Mock<CareContext>(new DbContextOptions<CareContext>());
        }

        public void Dispose()
        {
            _mockDbContext.Database.EnsureDeleted();
            _mockDbContext.Dispose();
        }


        public static Tuple<IPatientCarePlanRepository, List<PatientCarePlan>, Mock<CareContext>> GetMock()
        {
            var stub = DbInitializer.GenerateData(10);


            var data = stub.AsQueryable().BuildMock();
            var mockDbContext = new Mock<CareContext>();

            var mockSet = new Mock<DbSet<PatientCarePlan>>();

            mockSet
                .As<IQueryable<PatientCarePlan>>()
                .Setup(m => m.Provider)
                .Returns(data.Provider);

            mockSet
                .As<IQueryable<PatientCarePlan>>()
                .Setup(m => m.ElementType)
                .Returns(data.ElementType);

            mockSet
                .As<IQueryable<PatientCarePlan>>()
                .Setup(m => m.Expression)
                .Returns(data.Expression);

            mockSet
                .As<IQueryable<PatientCarePlan>>()
                .Setup(m => m.GetEnumerator())
                .Returns(data.GetEnumerator());


            mockDbContext
                .Setup(o => o.Set<PatientCarePlan>())
                .Returns(() => mockSet.Object);


            mockDbContext
                .Setup(x => x.SaveChangesAsync(""))
                .Returns(Task.FromResult(1));


            IPatientCarePlanRepository repository = new PatientCarePlanRepository(mockDbContext.Object);

            return new Tuple<IPatientCarePlanRepository, List<PatientCarePlan>, Mock<CareContext>>(repository, stub, mockDbContext);
        }

    }
}
