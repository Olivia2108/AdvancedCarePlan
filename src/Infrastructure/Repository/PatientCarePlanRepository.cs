using Application.Common.Interfaces.IDbContext;
using Application.Common.Interfaces.IRepository;
using Application.Mapping;
using Application.ViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Enums.GeneralEnums;

namespace Infrastructure.Repository
{
    public class PatientCarePlanRepository : IPatientCarePlanRepository
    {
        private readonly ICareContext _context; 

        public PatientCarePlanRepository(ICareContext context)
        {
            _context = context; 
        }



        public async Task<long> AddPatientCarePlan(PatientCarePlan data)
        {
            //var gh = _context.Database.GetDbConnection().ConnectionString;
            try
            {
                await _context.Set<PatientCarePlan>().AddAsync(data);
                var saves = await _context.SaveChangesAsync(data.IpAddress);
                return saves;
            }
            catch (Exception ex)
            {
                LoggerMiddleware.LogError(ex.Message);
                return (int)DbInfo.ErrorThrown;
            }
        }


        public async Task<bool> IsUsernameExist(string username)
        {
            try
            {
                var patient = await _context.Set<PatientCarePlan>().Where(x => x.UserName == username).AsNoTracking().FirstOrDefaultAsync();
                switch (patient)
                {
                    case null:
                        return false;
                    default:
                        return true;

                }

            }
            catch (Exception ex)
            {
                LoggerMiddleware.LogError(ex.Message);
                return false;
            }
        }



        public async Task<List<PatientCarePlanVM>> GetAllPatientCarePlans()
        {
            try
            {  
                var records = await _context.Set<PatientCarePlan>()
                                    .Where(x => x.IsActive && !x.IsDeleted)
                                    .OrderByDescending(x => x.DateCreated)
                                    .Select(x => new PatientCarePlanVM
                                    {
                                        UserName = x.UserName,
                                        PatientName = x.PatientName,
                                        ActualStartDate = x.ActualStartDate,
                                        ActualEndDate = x.ActualEndDate,
                                        TargetStartDate = x.TargetStartDate,
                                        Action = x.Action,
                                        Reason = x.Reason,
                                        Outcome = x.Outcome,
                                        Title= x.Title,
                                        Id= x.Id,
                                        DateCreated = x.DateCreated,

                                    })
                                    .AsNoTracking()
                                    .ToListAsync();
                  
                return records;
            }
            catch (Exception ex)
            {
                LoggerMiddleware.LogError(ex.Message);
                return new List<PatientCarePlanVM>();
            }
        }




    }
}
