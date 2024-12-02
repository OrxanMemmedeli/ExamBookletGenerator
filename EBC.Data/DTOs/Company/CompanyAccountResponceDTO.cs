namespace EBC.Data.DTOs.Company;

public class CompanyAccountResponceDTO
{
    public string Name { get; set; }

    public decimal? TotalPayment { get; set; }
    public decimal? TotalDebt { get; set; }
    public decimal? CurrentDebt { get; set; }
}
