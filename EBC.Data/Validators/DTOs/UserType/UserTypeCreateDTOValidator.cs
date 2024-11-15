using EBC.Core.Constants;
using EBC.Core.Helpers.Validators.Dtos.BaseValidators;
using EBC.Data.DTOs.UserType;
using FluentValidation;

namespace EBC.Data.Validators.DTOs.UserType;

public class UserTypeCreateDTOValidator : BaseEntityCreateDTOValidator<UserTypeCreateDTO>
{
    public UserTypeCreateDTOValidator() : base()
    {
        RuleFor(x => x.Type)
            .MinimumLength(3).WithMessage(string.Format(ValidationMessage.MinimumLength, 3))
            .MaximumLength(50).WithMessage(string.Format(ValidationMessage.MaximumLength, 50));
    }
}
