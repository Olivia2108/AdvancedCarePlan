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
        Task<long> AddPatientCarePlan(PatientCarePlan data);
        Task<bool> IsUsernameExist(string username);
        Task<List<PatientCarePlanVM>> GetAllPatientCarePlans();
    }
}
