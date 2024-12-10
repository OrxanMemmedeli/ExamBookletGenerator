using EBC.Core.Constants;
using EBC.Core.Helpers.Validators.Dtos.BaseValidators;
using EBC.Data.DTOs.Identities.Role;
using FluentValidation;

namespace EBC.Data.Validators.DTOs.Identity;

public class RoleEditDTOValidator : BaseEntityEditDTOValidator<RoleEditDTO>
{
    public RoleEditDTOValidator() : base()
    {
        RuleFor(x => x.Name)
            .MaximumLength(50).WithMessage(String.Format(ValidationMessage.MaximumLength, 50))
            .Must(name => !name.Contains(',')).WithMessage(ValidationMessage.NotCanContainsComma);
    }
}
