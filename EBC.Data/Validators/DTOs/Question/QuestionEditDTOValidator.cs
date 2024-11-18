using EBC.Core.Constants;
using EBC.Core.Helpers.Validators.Dtos.BaseValidators;
using EBC.Data.DTOs.Question;
using FluentValidation;

namespace EBC.Data.Validators.DTOs.Question
{
    public class QuestionEditDTOValidator : BaseEntityEditDTOValidator<QuestionEditDTO>
    {
        public QuestionEditDTOValidator()
        {
            RuleFor(x => x.Content)
                .MaximumLength(15000).WithMessage(string.Format(ValidationMessage.MaximumLength, 15000));
        }
    }
}
