using Bogus; 
using FluentValidation;
using FluentValidation.Results;
using Application.Dto;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Application.Dto.Validator
{
	 

	public class CarePlanValidator : AbstractValidator<PatientCarePlanDto>
	{
		public CarePlanValidator()
		{

			RuleFor(x => x.Title).NotNull().NotEmpty().MaximumLength(450);
			RuleFor(x => x.Reason).NotNull().NotEmpty().MaximumLength(1000);
			RuleFor(x => x.Action).NotNull().NotEmpty().MaximumLength(1000);
			RuleFor(x => x.PatientName).NotNull().NotEmpty().MaximumLength(450);
			RuleFor(x => x.UserName).NotNull().NotEmpty().MaximumLength(450); 
			RuleFor(x => x.Completed).Must(x => x == false || x == true);
			RuleFor(x => x.Outcome).NotEmpty().NotEmpty().MaximumLength(1000).When(x => x.Completed.Equals(true));
			RuleFor(x => x.TargetStartDate).NotNull().NotEmpty();
			RuleFor(x => x.ActualStartDate).NotNull().NotEmpty();
			RuleFor(x => x.ActualEndDate).NotNull().NotEmpty().When(x => x.Completed.Equals(true));
			RuleFor(x => x.IpAddress).NotNull().NotEmpty();


		}

		protected override bool PreValidate(ValidationContext<PatientCarePlanDto> context, ValidationResult result)
		{
			if (context.InstanceToValidate == null)
			{
				result.Errors.Add(new ValidationFailure($"PatientCarePlanDto", "Please ensure a model was supplied."));
				return false;
			}
			return true;
		}
	}
}
