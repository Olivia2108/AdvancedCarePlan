using Application.Common.Interfaces.IServices;
using Application.Common.Interfaces.IRepository; 
using Domain.Constants;
using Domain.Exceptions;
using Application.ViewModels;
using Domain.Entities;
using Application.Dto;
using Application.Dto.Validator;
using static Domain.Enums.GeneralEnums;

namespace Infrastructure.Services
{
    public class PatientCarePlanService : IPatientCarePlanService
	{
        private readonly IPatientCarePlanRepository _repository;

        public PatientCarePlanService(IPatientCarePlanRepository repository)
		{
            _repository = repository;
        }


        public async Task<ResponseVM> AddPatientCarePlan(PatientCarePlanDto carePlanDto)
        {
            try
            {

                //validate user data
                var validateData = new CarePlanValidator();
                var valid = await validateData.ValidateAsync(carePlanDto);
                switch (valid.IsValid)
                {
                    case false:
                        //var errors = valid.Errors.Select(l => l.ErrorMessage).ToArray();
                        var errors = valid.Errors.Select(l => l.ErrorMessage).FirstOrDefault();
                        return new ResponseVM
                        {
                            Message = string.Join("; ", errors),
                            Success = false
                        };
                }

                var isExist = await _repository.IsUsernameExist(username: carePlanDto.UserName);
                switch (isExist)
                {
                    case true:
                        return new ResponseVM
                        {
                            Message = ResponseConstants.IsExist,
                            Success = false
                        };
                }


                var data = new PatientCarePlans
                {
                    PatientName = carePlanDto.PatientName,
                    UserName = carePlanDto.UserName,
                    Action = carePlanDto.Action,
                    ActualEndDate = carePlanDto.Completed ? carePlanDto.ActualEndDate: Convert.ToDateTime("0001-01-01 00:00:00.0000000"),
                    ActualStartDate = carePlanDto.ActualStartDate,
                    TargetStartDate = carePlanDto.TargetStartDate,
                    Completed = carePlanDto.Completed,
                    Outcome = carePlanDto.Completed ? carePlanDto.Outcome: "",
                    Reason = carePlanDto.Reason,
                    Title = carePlanDto.Title,
                    IpAddress = carePlanDto.IpAddress
                };

                var result = await _repository.AddPatientCarePlan(data);
                #pragma warning disable IDE0066
                switch (result)
                {

                    case 0:
                        return new ResponseVM
                        {
                            Message = ResponseConstants.NotSaved,
                            Success = false
                        };
                    default:
                        return new ResponseVM
                        {
                            Data = result,
                            Message = ResponseConstants.Saved,
                            Success = true
                        };
                }
            }
            catch (Exception ex)
            {
                LoggerMiddleware.LogError(ex.Message);
                return new ResponseVM
                {
                    Message = ErrorConstants.ServiceSaveError,
                    Success = false
                }; ;

            }
        }



        public async Task<ResponseVM> GetAllPatientCarePlans()
        {
            try
            {
                var result = await _repository.GetAllPatientCarePlans();
                return result.Count > 0 ?
                    new ResponseVM
                    {
                        Data = result,
                        Message = ResponseConstants.Found,
                        Success = true,
                    }

                    :
                    new ResponseVM
                    {
                        Message = ResponseConstants.NotFound,
                        Success = true
                    };

            }
            catch (Exception ex)
            {
                LoggerMiddleware.LogError(ex.Message);
                return new ResponseVM
                {
                    Message = ErrorConstants.ServiceFetchError,
                    Success = false
                }; ;
            }
        }



        public async Task<ResponseVM> GetCarePlanById(long careplanId)
        {
            try
            {
                
                var result = await _repository.GetCarePlanById(careplanId);
                #pragma warning disable IDE0066
                switch (result)
                {

                    case null:
                        return new ResponseVM
                        {
                            Message = ResponseConstants.NotFound,
                            Success = true
                        };
                    default:
                        return new ResponseVM
                        {
                            Data = result,
                            Message = ResponseConstants.Found,
                            Success = true
                        };
                }

            }
            catch (Exception ex)
            {
                LoggerMiddleware.LogError(ex.Message);
                return new ResponseVM
                {
                    Message = ErrorConstants.ServiceFetchError,
                    Success = false
                }; ;
            }
        }

        public async Task<ResponseVM> UpdateCarePlanById(PatientCarePlanDto carePlanDto, long Id)
        {
            try
            {

                //validate user data
                var validateData = new CarePlanValidator();
                var valid = await validateData.ValidateAsync(carePlanDto);
                switch (valid.IsValid)
                {
                    case false:
                        var errors = valid.Errors.Select(l => l.ErrorMessage).FirstOrDefault();
                        return new ResponseVM
                        {
                            Message = string.Join("; ", errors),
                            Success = false
                        };
                }



                var data = new PatientCarePlans
                {
                    PatientName = carePlanDto.PatientName,
                    UserName = carePlanDto.UserName,
                    Action = carePlanDto.Action,
                    ActualEndDate = carePlanDto.Completed ? carePlanDto.ActualEndDate : Convert.ToDateTime("0001-01-01 00:00:00.0000000"),
                    ActualStartDate = carePlanDto.ActualStartDate,
                    TargetStartDate = carePlanDto.TargetStartDate,
                    Completed = carePlanDto.Completed,
                    Outcome = carePlanDto.Completed ? carePlanDto.Outcome : "",
                    Reason = carePlanDto.Reason,
                    Title = carePlanDto.Title,
                    IpAddress = carePlanDto.IpAddress
                };

                var result = await _repository.UpdateCarePlanById(Id, data);
                switch (result)
                {

                    case 0:
                        return new ResponseVM
                        {
                            Message = ResponseConstants.NotUpdated,
                            Success = false
                        };

                    case -4:
                        return new ResponseVM
                        {
                            Message = ResponseConstants.NotFound,
                            Success = true
                        };
                    default:
                        return new ResponseVM
                        {
                            Data = result,
                            Message = ResponseConstants.Updated,
                            Success = true
                        };
                }
            }
            catch (Exception ex)
            {
                LoggerMiddleware.LogError(ex.Message);
                return new ResponseVM
                {
                    Message = ErrorConstants.ServiceUpdateError,
                    Success = false
                };

            }
        }


        public async Task<ResponseVM> DeleteCarePlanById(long carePlanId, string ipAddress)
        {
            try
            {
                var result = await _repository.DeleteCarePlanById(carePlanId, ipAddress);
                switch (result)
                {
                    case var delete when delete == (int)DbInfo.NoIdFound:
                        return new ResponseVM
                        {
                            Message = ResponseConstants.NotDeleted,
                            Success = true
                        };
                    case var delete when delete == (int)DbInfo.ErrorThrown:
                        return new ResponseVM
                        {
                            Message = ErrorConstants.DbFetchError,
                            Success = false
                        };
                    default:
                        return new ResponseVM
                        {
                            Data = result,
                            Message = ResponseConstants.Deleted,
                            Success = true
                        };


                }

            }
            catch (Exception ex)
            {
                LoggerMiddleware.LogError(ex.Message);
                return new ResponseVM
                {
                    Message = ErrorConstants.ServiceDeleteError,
                    Success = false
                }; ;
            }
        }


    }
}
