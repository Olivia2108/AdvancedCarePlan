using Application.Common.Interfaces.IRepository;
using Application.Common.Interfaces.IServices;
using CarePlanUnitTest.Mocks;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarePlanUnitTest.Fixtures
{
    public class MockIRepositoryWrapper 
    {
        public static Mock<IRepositoryWrapper> GetMock()
        {
            var mock = new Mock<IRepositoryWrapper>();
            // Setup the mock

            var patientRepoMock = MockIPatientCarePlanRepository.GetMock();
            var patientServiceMock = MockIPatientCarePlanRepository.GetMock();

            //mock.Setup(m => m.patientService).Returns(() => patientServiceMock.Object);

            //mock.Setup(m => m.patientRepo).Returns(() => patientRepoMock.Object); 
            //mock.Setup(m => m.Save()).Callback(() => { return; });


            return mock;
        }
    }
}
