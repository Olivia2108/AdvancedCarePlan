using CareData.DataContext.Contracts;
using CareData.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareData.Repository
{
    public class PatientCarePlanRepository : IPatientCarePlanRepository
    {
        private readonly ICareContext _careContext;

        public PatientCarePlanRepository(ICareContext careContext)
        {
            _careContext = careContext;
        }
    }
}
