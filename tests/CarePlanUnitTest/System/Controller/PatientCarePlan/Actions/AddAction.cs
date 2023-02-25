using CarePlanUnitTest.Fixtures;
using Infrastructure.Repository;
using Infrastructure.Services; 

namespace CarePlanUnitTest.System.Controller.PatientCarePlan.Actions
{
    public class AddAction : DbContextFixture
    {
        private readonly PatientCarePlanRepository _repository;
        private readonly PatientCarePlanService _carePlanService;

        public AddAction()
        {
            switch (_repository)
            {
                case null:
                    _repository = new PatientCarePlanRepository(_context);
                    break;
            }
            switch (_carePlanService)
            {
                case null:
                    _carePlanService = new PatientCarePlanService(_repository);
                    break;
            }

        }


    }
}
