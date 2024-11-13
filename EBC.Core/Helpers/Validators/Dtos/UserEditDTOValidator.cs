using EBC.Core.Constants;
using EBC.Core.Helpers.Validators.Dtos.BaseValidators;
using EBC.Core.Models.Dtos.Identities.User;
using FluentValidation;

namespace EBC.Core.Helpers.Validators.Dtos;

public class UserEditDTOValidator : BaseEntityEditDTOValidator<UserEditDTO>
{
    public UserEditDTOValidator() : base()
    {
        RuleFor(x => x.FirstName)
            .MinimumLength(4).WithMessage(string.Format(ValidationMessage.MinimumLength, 4))
            .MaximumLength(50).WithMessage(string.Format(ValidationMessage.MaximumLength, 50));

        RuleFor(x => x.LastName)
            .MinimumLength(2).WithMessage(string.Format(ValidationMessage.MinimumLength, 2))
            .MaximumLength(50).WithMessage(string.Format(ValidationMessage.MaximumLength, 50));

        RuleFor(x => x.UserName)
            .MinimumLength(2).WithMessage(string.Format(ValidationMessage.MinimumLength, 2))
            .MaximumLength(50).WithMessage(string.Format(ValidationMessage.MaximumLength, 50));
    }
}
