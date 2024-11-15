using EBC.Core.Constants;
using EBC.Core.Helpers.Validators.Dtos.BaseValidators;
using EBC.Data.DTOs.QuestionParameter;
using FluentValidation;

namespace EBC.Data.Validators.DTOs.QuestionParameter;

public class QuestionParameterCreateDTOValidator : BaseEntityCreateDTOValidator<QuestionParameterCreateDTO>
{
    public QuestionParameterCreateDTOValidator() : base()
    {
        RuleFor(x => x.StartQuestionNumber)
              .GreaterThanOrEqualTo(1).WithMessage(string.Format(ValidationMessage.GreaterThanOrEqualTo, 1));

        RuleFor(x => x.EndQuestionNumber)
              .GreaterThanOrEqualTo(x => x.StartQuestionNumber).WithMessage((a, EndQuestionNumber) => string.Format(ValidationMessage.GreaterThanOrEqualTo, a.StartQuestionNumber))
              .GreaterThanOrEqualTo(x => x.StartQuestionNumber).WithMessage((x, StartQuestionNumber) => string.Format(ValidationMessage.GreaterThanOrEqualTo, x.EndQuestionNumber))
              .GreaterThanOrEqualTo(x => x.StartQuestionNumber).WithMessage(x => string.Format(ValidationMessage.GreaterThanOrEqualTo, x.StartQuestionNumber));
    }
}
