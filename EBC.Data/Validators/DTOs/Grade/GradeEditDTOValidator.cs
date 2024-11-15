using EBC.Core.Constants;
using EBC.Core.Helpers.Validators.Dtos.BaseValidators;
using EBC.Data.DTOs.Grade;
using FluentValidation;

namespace EBC.Data.Validators.DTOs.Grade
{
    public class GradeEditDTOValidator : BaseEntityEditDTOValidator<GradeEditDTO>
    {
        public GradeEditDTOValidator() : base()
        {
            RuleFor(x => x.Name)
                .MaximumLength(50).WithMessage(string.Format(ValidationMessage.MaximumLength, 50));
        }
    }
}
