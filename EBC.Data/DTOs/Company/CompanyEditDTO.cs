using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.Company;

public class CompanyEditDTO : BaseEntityEditDTO
{
    public string Name { get; set; }
    public string Domain { get; set; }
    public string Code { get; set; }
    public string EmailAdress { get; set; }

    public decimal DailyAmount { get; set; }  // günlük məbləğ AZN
    public decimal DebtLimit { get; set; } // maksimal borc limiti
    public decimal PercentOfFine { get; set; } // limit aşıldıqda məbləğə artım faizi AZN

    public bool IsFree { get; set; }
    public bool IsStopSubscription { get; set; }

}
