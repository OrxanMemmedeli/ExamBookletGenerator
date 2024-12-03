using EBC.Core.Models.Commons;
using EBC.Core.Models.Enums;

namespace EBC.Data.DTOs.PaymentOrDebt;

public class PaymentOrDebtCreateDTO : BaseEntityCreateDTO
{
    public decimal Amount { get; set; }
    public PaymentType PaymentType { get; set; }

    public Guid CompanyId { get; set; }
}
