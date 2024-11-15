using EBC.Core.Constants;
using EBC.Core.Helpers.Validators.Dtos.BaseValidators;
using EBC.Data.DTOs.QuestionType;
using FluentValidation;

namespace EBC.Data.Validators.DTOs.QuestionType;

public class QuestionTypeCreateDTOValidator : BaseEntityCreateDTOValidator<QuestionTypeCreateDTO>
{
    public QuestionTypeCreateDTOValidator() : base()
    {
        RuleFor(x => x.ResponseType)
            .MinimumLength(3).WithMessage(string.Format(ValidationMessage.MinimumLength, 3))
            .MaximumLength(50).WithMessage(string.Format(ValidationMessage.MaximumLength, 50));

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage(string.Format(ValidationMessage.MaximumLength, 500));

        RuleFor(x => x.ResponseCount)
            .GreaterThanOrEqualTo(0).WithMessage(string.Format(ValidationMessage.GreaterThanOrEqualTo, 0));
    }
}
