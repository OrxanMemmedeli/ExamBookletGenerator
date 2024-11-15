using EBC.Core.Constants;
using EBC.Core.Helpers.Validators.Dtos.BaseValidators;
using EBC.Data.DTOs.Company;
using FluentValidation;

namespace EBC.Data.Validators.DTOs.Company;

public class CompanyEditDTOValidator : BaseEntityEditDTOValidator<CompanyEditDTO>
{
    public CompanyEditDTOValidator() : base()
    {
        RuleFor(x => x.Name)
            .MaximumLength(50).WithMessage(string.Format(ValidationMessage.MaximumLength, 50));

        RuleFor(x => x.LogoUrl)
            .MaximumLength(550).WithMessage(string.Format(ValidationMessage.MaximumLength, 550));

        RuleFor(x => x.DailyAmount)
            .GreaterThanOrEqualTo(0).WithMessage(string.Format(ValidationMessage.GreaterThanOrEqualTo, 0));

        RuleFor(x => x.PersentOfFine)
            .GreaterThanOrEqualTo(0).WithMessage(string.Format(ValidationMessage.GreaterThanOrEqualTo, 0));

        RuleFor(x => x.DebtLimit)
            .GreaterThanOrEqualTo(0).WithMessage(string.Format(ValidationMessage.GreaterThanOrEqualTo, 0));
    }
}
