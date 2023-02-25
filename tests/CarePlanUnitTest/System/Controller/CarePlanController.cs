
using CarePlanUnitTest.Fixtures;
using Infrastructure.Services;
using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarePlanUnitTest.System.Controller
{
    public class CarePlanController : DbContextFixture
    {
        private readonly PatientCarePlanRepository _repository;
        private readonly PatientCarePlanService _carePlanService;

        public CarePlanController()
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
