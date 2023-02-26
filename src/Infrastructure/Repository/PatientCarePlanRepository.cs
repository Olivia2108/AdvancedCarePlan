using Application.Common.Interfaces.IDbContext;
using Application.Common.Interfaces.IRepository; 
using Application.ViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.Persistence;
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
        private readonly CareContext _context; 

        public PatientCarePlanRepository(CareContext context)
        {
            _context = context; 
        }



        public async Task<long> AddPatientCarePlan(PatientCarePlans data)
        {
            //var gh = _context.Database.GetDbConnection().ConnectionString;
            try
            {
                await _context.Set<PatientCarePlans>().AddAsync(data);
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
                var patient = await _context.Set<PatientCarePlans>().Where(x => x.UserName == username).AsNoTracking().FirstOrDefaultAsync();
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
                var records = await _context.Set<PatientCarePlans>()
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




        public async Task<PatientCarePlanVM> GetCarePlanById(long carePlanId)
        {
            try
            {
                var records = await _context.Set<PatientCarePlans>()
                                    .Where(x => x.Id == carePlanId && x.IsActive && !x.IsDeleted) 
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
                                        Title = x.Title,
                                        Id = x.Id,
                                        DateCreated = x.DateCreated,

                                    })
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(); 
                return records;
            }
            catch (Exception ex)
            {
                LoggerMiddleware.LogError(ex.Message);
                return new PatientCarePlanVM();
            }
        }


        public async Task<int> UpdateCarePlanById(long carePlanId, PatientCarePlans data)
        {
            try
            {
                

                var record = await _context.Set<PatientCarePlans>().Where(x => x.Id == carePlanId).FirstOrDefaultAsync();
                switch (record == null)
                {
                    case true:
                        return (int)DbInfo.NoIdFound;
                }
                record.Action = data.Action;
                record.Outcome = data.Outcome;
                record.PatientName = data.PatientName;
                record.TargetStartDate = data.TargetStartDate;
                record.ActualStartDate = data.ActualStartDate;
                record.ActualEndDate = data.ActualEndDate;
                record.IsActive = true;
                record.Completed = data.Completed;
                record.Reason = data.Reason;
                record.Title = data.Title;
                record.DateModified = DateTime.Now;

                var save = await _context.SaveChangesAsync(data.IpAddress);
                return save;
            }
            catch (Exception ex)
            {
                LoggerMiddleware.LogError(ex.Message);
                return (int)DbInfo.ErrorThrown;
            }
        }




        public async Task<int> DeleteCarePlanById(long carePlanId, string ipAddress)
        {
            try
            {
                var emply = await _context.Set<PatientCarePlans>().Where(x => x.Id == carePlanId).FirstOrDefaultAsync();
                switch (emply == null)
                {
                    case true:
                        return (int)DbInfo.NoIdFound;
                }
                emply.IsActive = false;
                emply.IsDeleted = true;
                emply.DateDeleted = DateTime.Now;

                var save = await _context.SaveChangesAsync(ipAddress);
                return save;
            }
            catch (Exception ex)
            {
                LoggerMiddleware.LogError(ex.Message);
                return (int)DbInfo.ErrorThrown;
            }
        }




    }
}
