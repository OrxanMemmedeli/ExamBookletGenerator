using EBC.Core.Constants;
using EBC.Core.Helpers.Validators.Dtos.BaseValidators;
using EBC.Data.DTOs.QuestionLevel;
using FluentValidation;

namespace EBC.Data.Validators.DTOs.QuestionLevel
{
    public class QuestionLevelCreateDTOValidator : BaseEntityCreateDTOValidator<QuestionLevelCreateDTO>
    {
        public QuestionLevelCreateDTOValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(50).WithMessage(string.Format(ValidationMessage.MaximumLength, 50));

            RuleFor(x => x.Level)
                .InclusiveBetween((short)1, (short)5).WithMessage(string.Format(ValidationMessage.InclusiveBetween, 1, 5));
        }
    }
