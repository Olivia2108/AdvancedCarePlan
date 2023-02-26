using Application.ViewModels;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.IRepository
{
    public interface IPatientCarePlanRepository
    {
        Task<long> AddPatientCarePlan(PatientCarePlans data);
        Task<bool> IsUsernameExist(string username);
        Task<List<PatientCarePlanVM>> GetAllPatientCarePlans();
        Task<PatientCarePlanVM> GetCarePlanById(long carePlanId);
        Task<int> UpdateCarePlanById(long carePlanId, PatientCarePlans data);
        Task<int> DeleteCarePlanById(long carePlanId, string ipAddress);
    }
}
