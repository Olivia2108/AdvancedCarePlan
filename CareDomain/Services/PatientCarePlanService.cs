using CareData.Repository.Contracts;
using CareDomain.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareDomain.Services
{
	public class PatientCarePlanService : IPatientCarePlanService
	{
        private readonly IPatientCarePlanRepository _repository;

        public PatientCarePlanService(IPatientCarePlanRepository repository)
		{
            _repository = repository;
        }
	}
}
