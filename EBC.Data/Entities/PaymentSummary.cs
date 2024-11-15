using EBC.Core.Entities.Common;

namespace EBC.Data.Entities;

public class PaymentSummary : BaseEntity<Guid>
{
    public decimal TotalPayment { get; set; }
    public decimal TotalDebt { get; set; }
    public decimal CurrentDebt { get; set; }

    public Guid CompanyId { get; set; }
    public virtual Company Company { get; set; }
}
