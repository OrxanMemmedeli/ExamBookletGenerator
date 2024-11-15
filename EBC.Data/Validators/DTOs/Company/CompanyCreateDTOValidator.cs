using EBC.Core.Constants;
using EBC.Core.Helpers.Validators.Dtos.BaseValidators;
using EBC.Data.DTOs.Company;
using FluentValidation;

namespace EBC.Data.Validators.DTOs.Company;

public class CompanyCreateDTOValidator : BaseEntityCreateDTOValidator<CompanyCreateDTO>
{
    public CompanyCreateDTOValidator() : base()
    {
        RuleFor(x => x.Name)
            .MaximumLength(70).WithMessage(string.Format(ValidationMessage.MaximumLength, 70));

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
