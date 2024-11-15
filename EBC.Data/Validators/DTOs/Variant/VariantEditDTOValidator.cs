using EBC.Core.Constants;
using EBC.Core.Helpers.Validators.Dtos.BaseValidators;
using EBC.Data.DTOs.Variant;
using FluentValidation;

namespace EBC.Data.Validators.DTOs.Variant;

public class VariantEditDTOValidator : BaseEntityEditDTOValidator<VariantEditDTO>
{
    public VariantEditDTOValidator() : base()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(string.Format(ValidationMessage.NotEmptyAndNotNull))
            .NotNull().WithMessage(string.Format(ValidationMessage.NotEmptyAndNotNull))
            .MaximumLength(50).WithMessage(string.Format(ValidationMessage.MaximumLength, 50));
    }
}
