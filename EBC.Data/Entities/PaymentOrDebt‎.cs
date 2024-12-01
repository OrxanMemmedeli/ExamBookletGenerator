using EBC.Core.Entities.Common;
using EBC.Core.Models.Enums;

namespace EBC.Data.Entities;

public class PaymentOrDebt‎: BaseEntity<Guid>
{
    public decimal Amount { get; set; }
    public PaymentType PaymentType { get; set; }

    public Guid CompanyId { get; set; }
    public virtual Company Company { get; set; }
}
