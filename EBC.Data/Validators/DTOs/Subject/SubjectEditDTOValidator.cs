using EBC.Core.Helpers.Validators.Dtos.BaseValidators;
using EBC.Data.DTOs.Subject;
using FluentValidation;

namespace EBC.Data.Validators.DTOs.Subject;

public class SubjectEditDTOValidator : BaseEntityEditDTOValidator<SubjectEditDTO>
{
    public SubjectEditDTOValidator() : base()
    {
        RuleFor(x => x.Name)
            .MinimumLength(3).WithMessage(string.Format(ValidationMessage.MinimumLength, 3))
            .MaximumLength(50).WithMessage(string.Format(ValidationMessage.MaximumLength, 50));

        RuleFor(x => x.AmountForQuestion)
            .GreaterThanOrEqualTo(0).WithMessage(string.Format(ValidationMessage.GreaterThanOrEqualTo, 0));
    }
}
