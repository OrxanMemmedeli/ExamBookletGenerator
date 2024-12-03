using EBC.Core.Constants;
using EBC.Core.Helpers.Validators.Dtos.BaseValidators;
using EBC.Data.DTOs.Company;
using FluentValidation;

namespace EBC.Data.Validators.DTOs.Company;

public class CompanyCreateDTOValidator : BaseEntityCreateDTOValidator<CompanyCreateDTO>
{
    public CompanyCreateDTOValidator() : base()
    {
        RuleFor(x => x.LogoUrl)
            .MaximumLength(550).WithMessage(string.Format(ValidationMessage.MaximumLength, 550));

        RuleFor(x => x.Name)
            .MinimumLength(4).WithMessage(string.Format(ValidationMessage.MinimumLength, 4))
            .MaximumLength(250).WithMessage(string.Format(ValidationMessage.MaximumLength, 250));

        RuleFor(x => x.Domain)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _)).WithMessage(ValidationMessage.WrongLinkAdress)
            .MinimumLength(4).WithMessage(string.Format(ValidationMessage.MinimumLength, 4))
            .MaximumLength(250).WithMessage(string.Format(ValidationMessage.MaximumLength, 250));

        RuleFor(x => x.Code)
            .MinimumLength(2).WithMessage(string.Format(ValidationMessage.MinimumLength, 2))
            .MaximumLength(50).WithMessage(string.Format(ValidationMessage.MaximumLength, 50));

        RuleFor(x => x.EmailAdress)
            .EmailAddress().WithMessage(String.Format(ValidationMessage.EmailAddress))
            .MinimumLength(2).WithMessage(string.Format(ValidationMessage.MinimumLength, 2))
            .MaximumLength(250).WithMessage(string.Format(ValidationMessage.MaximumLength, 250));

        RuleFor(x => x.DailyAmount)
            .GreaterThanOrEqualTo(0).WithMessage(string.Format(ValidationMessage.GreaterThanOrEqualTo, 0));

        RuleFor(x => x.PercentOfFine)
            .GreaterThanOrEqualTo(0).WithMessage(string.Format(ValidationMessage.GreaterThanOrEqualTo, 0));

        RuleFor(x => x.DebtLimit)
            .GreaterThanOrEqualTo(0).WithMessage(string.Format(ValidationMessage.GreaterThanOrEqualTo, 0));

    }
}
