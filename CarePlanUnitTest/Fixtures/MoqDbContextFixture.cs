using CareData.DataContext;
using CareData.DataContext.Infrastructure;
using CareData.Models;
using CareData.Repository;
using CareData.Repository.Contracts;
using CareDomain.Services.Contracts;
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


        public static Tuple<IPatientCarePlanRepository, List<CarePlan>, Mock<CareContext>> GetMock()
        {
            var stub = DbInitializer.GenerateData(10);


            var data = stub.AsQueryable().BuildMock();
            var mockDbContext = new Mock<CareContext>();

            var mockSet = new Mock<DbSet<CarePlan>>();

            mockSet
                .As<IQueryable<CarePlan>>()
                .Setup(m => m.Provider)
                .Returns(data.Provider);

            mockSet
                .As<IQueryable<CarePlan>>()
                .Setup(m => m.ElementType)
                .Returns(data.ElementType);

            mockSet
                .As<IQueryable<CarePlan>>()
                .Setup(m => m.Expression)
                .Returns(data.Expression);

            mockSet
                .As<IQueryable<CarePlan>>()
                .Setup(m => m.GetEnumerator())
                .Returns(data.GetEnumerator());


            mockDbContext
                .Setup(o => o.CarePlan)
                .Returns(() => mockSet.Object);


            mockDbContext
                .Setup(x => x.SaveChangesAsync(""))
                .Returns(Task.FromResult(1));


            IPatientCarePlanRepository repository = new PatientCarePlanRepository(mockDbContext.Object);

            return new Tuple<IPatientCarePlanRepository, List<CarePlan>, Mock<CareContext>>(repository, stub, mockDbContext);
        }

    }
}
