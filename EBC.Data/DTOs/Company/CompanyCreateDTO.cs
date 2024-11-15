using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.Company;

public class CompanyCreateDTO : BaseEntityCreateDTO
{
    public string Name { get; set; }
    public string LogoUrl { get; set; }

    public decimal DailyAmount { get; set; }
    public decimal DebtLimit { get; set; }
    public decimal PersentOfFine { get; set; }

    public bool IsFree { get; set; }

}
