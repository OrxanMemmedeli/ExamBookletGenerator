using EBC.Core.Models.Commons;
using EBC.Data.DTOs.PaymentSummary;

namespace EBC.Data.DTOs.Company;

public class CompanyCreateDTO : BaseEntityCreateDTO
{
    public string LogoUrl { get; set; }
    public string Name { get; set; }
    public string Domain { get; set; }
    public string Code { get; set; }
    public string EmailAdress { get; set; }

    public DateTime? JoinDate { get; set; }
    public decimal DailyAmount { get; set; }  // günlük məbləğ AZN
    public decimal DebtLimit { get; set; } // maksimal borc limiti
    public decimal PercentOfFine { get; set; } // limit aşıldıqda məbləğə artım faizi AZN

    public bool IsFree { get; set; }
    public bool IsStopSubscription { get; set; }
    public PaymentSummaryCreateDTO PaymentSummaryCreateDTO { get; set; }
}
