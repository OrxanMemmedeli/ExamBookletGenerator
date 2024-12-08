using EBC.Core.Entities.Common;
using EBC.Data.Entities.CombineEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBC.Data.Entities;

public class Company : BaseEntity<Guid>
{
    public Company()
    {
        CompanyUsers = new HashSet<CompanyUser>();
        this.Transactions = new HashSet<PaymentOrDebt>();
        this.AuthenticationHistories = new HashSet<AuthenticationHistory>();
        this.SendingEmails = new HashSet<SendingEmail>();
    }

    public string Name { get; set; }
    public string Domain { get; set; } //+
    public string LogoUrl { get; set; }

    public byte[] LogoFile { get; set; }

    public DateTime? JoinDate { get; set; } //+
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

    public virtual PaymentSummary PaymentSummary { get; set; }

    #region Collections 

    public ICollection<CompanyUser> CompanyUsers { get; set; }
    public ICollection<PaymentOrDebt> Transactions { get; set; }
    public ICollection<SendingEmail> SendingEmails { get; set; }
    public ICollection<AuthenticationHistory> AuthenticationHistories { get; set; }
    #endregion

}
