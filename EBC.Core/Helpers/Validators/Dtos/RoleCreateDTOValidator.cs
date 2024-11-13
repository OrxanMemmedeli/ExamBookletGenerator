using EBC.Core.Constants;
using EBC.Core.Helpers.Validators.Dtos.BaseValidators;
using EBC.Core.Models.Dtos.Identities.Role;
using FluentValidation;

namespace EBC.Core.Helpers.Validators.Dtos;

public class RoleCreateDTOValidator : BaseEntityCreateDTOValidator<RoleCreateDTO>
{
    public RoleCreateDTOValidator() : base()
    {
        // Xüsusi qayda: Maksimum uzunluq
        RuleFor(x => x.Name)
            .MaximumLength(50).WithMessage(String.Format(ValidationMessage.MaximumLength, 50));
    }
}
