using EBC.Core.Constants;
using EBC.Core.Helpers.Validators.Dtos.BaseValidators;
using EBC.Core.Models.Dtos.Identities.User;
using FluentValidation;

namespace EBC.Core.Helpers.Validators.Dtos;

public class UserPasswordEditDTOValidator : BaseValidator<UserPasswordEditDTO>
{
    public UserPasswordEditDTOValidator() : base()  
    {
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
