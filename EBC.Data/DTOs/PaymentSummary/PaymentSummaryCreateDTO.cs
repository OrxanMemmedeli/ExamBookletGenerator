using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.PaymentSummary;

public class PaymentSummaryCreateDTO : BaseEntityCreateDTO
{
    public decimal TotalPayment { get; set; }
    public decimal TotalDebt { get; set; }
    public Guid CompanyId { get; set; }
}
