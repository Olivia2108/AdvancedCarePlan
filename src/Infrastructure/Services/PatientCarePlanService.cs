using Application.Common.Interfaces.IServices;
using Application.Common.Interfaces.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
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
