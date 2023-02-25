using Application.Common.Interfaces.IDbContext;
using Application.Common.Interfaces.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
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
