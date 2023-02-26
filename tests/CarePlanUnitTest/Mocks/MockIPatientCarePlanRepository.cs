using Application.Common.Interfaces.IRepository;
using Application.Common.Interfaces.IServices;
using Domain.Entities;
using Infrastructure.Persistence;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarePlanUnitTest.Mocks
{
    public class MockIPatientCarePlanRepository
    {
        public static Mock<IPatientCarePlanRepository> GetMock()
        {
            var mock = new Mock<IPatientCarePlanRepository>();

            var stub = DbInitializer.GenerateData(3);


            mock.Setup(m => m.GetAllPatientCarePlans()).Returns(() => stub);
            mock.Setup(m => m.AddPatientCarePlan(It.IsAny<PatientCarePlan>()))
                .Callback(() => { return; });

            //mock.Setup(m => m.GetOwnerById(It.IsAny<Guid>()))
            //    .Returns((Guid id) => owners.FirstOrDefault(o => o.Id == id));
            //mock.Setup(m => m.GetOwnerWithDetails(It.IsAny<Guid>()))
            //    .Returns((Guid id) => owners.FirstOrDefault(o => o.Id == id));
            //mock.Setup(m => m.UpdateOwner(It.IsAny<Owner>()))
            //   .Callback(() => { return; });
            //mock.Setup(m => m.DeleteOwner(It.IsAny<Owner>()))
            //   .Callback(() => { return; });

            // Set up

            return mock;
        }
    }
}
