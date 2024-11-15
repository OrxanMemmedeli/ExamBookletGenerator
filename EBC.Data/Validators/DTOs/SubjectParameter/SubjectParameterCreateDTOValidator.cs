using EBC.Core.Constants;
using EBC.Core.Helpers.Validators.Dtos.BaseValidators;
using EBC.Data.DTOs.SubjectParameter;
using FluentValidation;

namespace EBC.Data.Validators.DTOs.SubjectParameter;

public class SubjectParameterCreateDTOValidator : BaseEntityCreateDTOValidator<SubjectParameterCreateDTO>
{
    public SubjectParameterCreateDTOValidator() : base()
    {
        RuleFor(x => x.QuestionCount)
              .GreaterThanOrEqualTo(1).WithMessage(string.Format(ValidationMessage.GreaterThanOrEqualTo, 1));

        RuleFor(x => x.Order)
              .GreaterThanOrEqualTo(1).WithMessage(string.Format(ValidationMessage.GreaterThanOrEqualTo, 1));
    }
}
