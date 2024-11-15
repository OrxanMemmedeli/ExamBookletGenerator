using EBC.Core.Constants;
using EBC.Core.Helpers.Validators.Dtos.BaseValidators;
using EBC.Data.DTOs.Exam;
using FluentValidation;

namespace EBC.Data.Validators.DTOs.Exam;

public class ExamCreateDTOValidator : BaseEntityCreateDTOValidator<ExamCreateDTO>
{
    public ExamCreateDTOValidator() : base()
    {
        RuleFor(x => x.Name)
            .MaximumLength(250).WithMessage(string.Format(ValidationMessage.MaximumLength, 250));
    }
}
