using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.PaymentSummary
{
    public class PaymentSummaryUpdateDTO : BaseEntityEditDTO
    {
        public decimal TotalPayment { get; set; }
        public decimal TotalDebt { get; set; }
        public Guid CompanyId { get; set; }
    }
}
