using EBC.Core.Constants;
using EBC.Core.Helpers.Validators.Dtos.BaseValidators;
using EBC.Data.DTOs.Identities.User;
using FluentValidation;

namespace EBC.Data.Validators.DTOs.Identity;

public class UserCreateDTOValidator : BaseEntityCreateDTOValidator<UserCreateDTO>
{
    public UserCreateDTOValidator() : base()
    {
        RuleFor(x => x.FirstName)
            .MaximumLength(50).WithMessage(string.Format(ValidationMessage.MaximumLength, 50));

        RuleFor(x => x.LastName)
            .MinimumLength(2).WithMessage(string.Format(ValidationMessage.MinimumLength, 2))
            .MaximumLength(50).WithMessage(string.Format(ValidationMessage.MaximumLength, 50));

        RuleFor(x => x.UserName)
            .MinimumLength(2).WithMessage(string.Format(ValidationMessage.MinimumLength, 2))
            .MaximumLength(50).WithMessage(string.Format(ValidationMessage.MaximumLength, 50));

        RuleFor(x => x.Password)
            .MinimumLength(6).WithMessage(string.Format(ValidationMessage.MinimumLength, 6))
            .MaximumLength(20).WithMessage(string.Format(ValidationMessage.MaximumLength, 20))
            .Matches("[A-Z]").WithMessage(ValidationMessage.CapitalLetter)
            .Matches("[a-z]").WithMessage(ValidationMessage.LowercaseLetter)
            .Matches("[0-9]").WithMessage(ValidationMessage.Number)
            .Matches("[^a-zA-Z0-9]").WithMessage(ValidationMessage.SpecialCharacter);

        RuleFor(x => x.PasswordConfirm)
            .MinimumLength(6).WithMessage(string.Format(ValidationMessage.MinimumLength, 6))
            .MaximumLength(20).WithMessage(string.Format(ValidationMessage.MaximumLength, 20))
            .Equal(x => x.Password).WithMessage(ValidationMessage.EqualPassword)
            .When(x => !string.IsNullOrWhiteSpace(x.PasswordConfirm)).WithMessage(ValidationMessage.EqualPassword);
    }
}
