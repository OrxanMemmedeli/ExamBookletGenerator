namespace EBC.Data.DTOs.Company;

public class CompanyCacheViewDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } //+
    public string Domain { get; set; } //+

    public DateTime? BlockedCompanyDate { get; set; }
    public decimal DailyAmount { get; set; }  // günlük məbləğ AZN
    public decimal DebtLimit { get; set; } // maksimal borc limiti
    public decimal PercentOfFine { get; set; } // limit aşıldıqda məbləğə artım faizi AZN
    public string Code { get; set; } //+
    public string Key { get; set; }
    public string EmailAdress { get; set; }

    public bool IsFree { get; set; }
    public bool IsPenal { get; set; }
    public bool IsActive { get; set; }
    public bool IsStopSubscription { get; set; }
    public bool IsConfirm { get; set; }


    public decimal? TotalPayment { get; set; }
    public decimal? TotalDebt { get; set; }
    public decimal? CurrentDebt { get; set; }

    public bool Status { get; set; }
    public bool IsDeleted { get; set; }
}
