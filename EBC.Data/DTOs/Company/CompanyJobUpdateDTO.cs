namespace EBC.Data.DTOs.Company;

public class CompanyJobUpdateDTO
{
    public Guid CompanyId { get; set; }
    public bool IsPenal { get; set; }
    public DateTime? BlockedDate { get; set; }

}
