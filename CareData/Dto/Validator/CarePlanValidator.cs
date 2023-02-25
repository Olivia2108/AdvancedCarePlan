using Bogus;
using CareShared.Dto;
using FluentValidation;
using FluentValidation.Results; 
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace CareData.Dto.Validator
{
	 

	public class CarePlanValidator : AbstractValidator<CarePlanDto>
	{
		public CarePlanValidator()
		{

			RuleFor(x => x.Title).NotNull().MaximumLength(450).WithMessage("Title is required");
			RuleFor(x => x.Reason).NotNull().NotEmpty().MaximumLength(1000);
			RuleFor(x => x.Action).NotNull().NotEmpty().MaximumLength(1000);
			RuleFor(x => x.PatientName).NotNull().MaximumLength(450).WithMessage("Patient Name is required.");
			RuleFor(x => x.UserName).NotNull().MaximumLength(450).WithMessage("User Name is required."); 
			RuleFor(x => x.Completed).Must(x => x == false || x == true).WithMessage("Completed is required.");
			RuleFor(x => x.Outcome).NotEmpty().MaximumLength(1000).When(x => x.Completed.Equals(true)).WithMessage("Outcome is required.");
			RuleFor(x => x.TargetStartDate).NotNull().NotEmpty().WithMessage("Target Start Date is required.");
			RuleFor(x => x.ActualStartDate).NotNull().NotEmpty().WithMessage("Actual Start Date is required.");
			RuleFor(x => x.ActualEndDate).NotNull().NotEmpty().When(x => x.Completed.Equals(true)).WithMessage("Actual End Date is required.");
			RuleFor(x => x.IpAddress).NotNull().NotEmpty().WithMessage("IpAddress is required.");


		}

		protected override bool PreValidate(ValidationContext<CarePlanDto> context, ValidationResult result)
		{
			if (context.InstanceToValidate == null)
			{
				result.Errors.Add(new ValidationFailure($"CarePlanDto", "Please ensure a model was supplied."));
				return false;
			}
			return true;
		}
	}
}
