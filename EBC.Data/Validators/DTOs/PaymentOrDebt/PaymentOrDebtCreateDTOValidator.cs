using EBC.Core.Constants;
using EBC.Data.DTOs.PaymentOrDebt;
using FluentValidation;

namespace EBC.Data.Validators.DTOs.PaymentOrDebt;

public class PaymentOrDebtCreateDTOValidator : AbstractValidator<PaymentOrDebtCreateDTO>
{
    public PaymentOrDebtCreateDTOValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThanOrEqualTo(10).WithMessage(string.Format(ValidationMessage.GreaterThanOrEqualTo, 10));
    }
}
