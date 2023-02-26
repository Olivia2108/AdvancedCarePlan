using Application.Dto;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.IServices
{
    public interface IPatientCarePlanService
    {
        Task<ResponseVM> AddPatientCarePlan(PatientCarePlanDto carePlanDto);
        Task<ResponseVM> GetAllPatientCarePlans();
        Task<ResponseVM> GetCarePlanById(long careplanId);
        Task<ResponseVM> UpdateCarePlanById(PatientCarePlanDto carePlanDto, long Id);
        Task<ResponseVM> DeleteCarePlanById(long carePlanId, string ipAddress);
    }
}
