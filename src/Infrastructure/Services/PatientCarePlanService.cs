using Application.Common.Interfaces.IServices;
using Application.Common.Interfaces.IRepository; 
using Domain.Constants;
using Domain.Exceptions;
using Application.ViewModels;
using Domain.Entities;
using Application.Dto;
using Application.Dto.Validator;

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


                var data = new PatientCarePlan
                {
                    PatientName = carePlanDto.PatientName,
                    UserName = carePlanDto.UserName,
                    Action = carePlanDto.Action,
                    ActualEndDate = carePlanDto.ActualEndDate,
                    ActualStartDate = carePlanDto.ActualStartDate,
                    TargetStartDate = carePlanDto.TargetStartDate,
                    Completed = carePlanDto.Completed,
                    Outcome = carePlanDto.Outcome,
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


    }
}
