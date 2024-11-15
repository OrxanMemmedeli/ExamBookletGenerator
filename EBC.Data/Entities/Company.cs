using EBC.Core.Entities.Common;
using EBC.Data.Entities.CombineEntities;
using EBC.Data.Entities.ExceptionalEntities;

namespace EBC.Data.Entities;

public class Company : BaseEntity<Guid>
{
    public Company()
    {
        CompanyUsers = new HashSet<CompanyUser>();
        Payments = new HashSet<Payment>();
    }

    public string Name { get; set; }
    public string LogoUrl { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? BlockedDate { get; set; }
    public decimal DailyAmount { get; set; }  // günlük məbləğ AZN
    public decimal DebtLimit { get; set; } // maksimal borc limiti
    public decimal PersentOfFine { get; set; } // limit aşıldıqda məbləğə artım faizi AZN

    public bool IsFree { get; set; }
    public bool IsPenal { get; set; }

    public virtual PaymentSummary? PaymentSummary { get; set; }

    #region Collections 

    public ICollection<CompanyUser> CompanyUsers { get; set; }
    public ICollection<Payment> Payments { get; set; }

    #endregion

}
