using EBC.Core.Constants;
using EBC.Core.Helpers.Validators.Dtos.BaseValidators;
using EBC.Data.DTOs.ExamParameter;
using FluentValidation;

namespace EBC.Data.Validators.DTOs.ExamParameter;

public class ExamParameterEditDTOValidator : BaseEntityEditDTOValidator<ExamParameterEditDTO>
{
    public ExamParameterEditDTOValidator() : base()
    {
        {
            RuleFor(x => x.Name)
                .MaximumLength(70).WithMessage(string.Format(ValidationMessage.MaximumLength, 70));

            RuleFor(x => x.Description)
                .MaximumLength(2500).WithMessage(string.Format(ValidationMessage.MaximumLength, 2500));

            RuleFor(x => x.SubjectCount)
                .GreaterThanOrEqualTo(1).WithMessage(string.Format(ValidationMessage.GreaterThanOrEqualTo, 1));
        }
    }
}
