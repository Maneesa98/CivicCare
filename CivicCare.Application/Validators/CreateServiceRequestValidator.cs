using FluentValidation;
using CivicCare.Application.Requests;

namespace CivicCare.Application.Validators;

public class CreateServiceRequestValidator
    : AbstractValidator<CreateServiceRequest>
{
    public CreateServiceRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required");

        RuleFor(x => x.Location)
            .NotEmpty()
            .WithMessage("Location is required");

        RuleFor(x => x.AssignedDepartmentId)
            .GreaterThan(0)
            .WithMessage("Department is required");
    }
}