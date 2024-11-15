using EBC.Core.Constants;
using EBC.Core.Helpers.Validators.Dtos.BaseValidators;
using EBC.Data.DTOs.Section;
using FluentValidation;

namespace EBC.Data.Validators.DTOs.Section;

public class SectionCreateDTOValidator : BaseEntityCreateDTOValidator<SectionCreateDTO>
{
    public SectionCreateDTOValidator() : base()
    {
        RuleFor(x => x.Name)
            .MinimumLength(3).WithMessage(string.Format(ValidationMessage.MinimumLength, 3))
            .MaximumLength(250).WithMessage(string.Format(ValidationMessage.MaximumLength, 250));
    }
}
