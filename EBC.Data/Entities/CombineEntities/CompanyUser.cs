namespace EBC.Data.Entities.CombineEntities;

public class CompanyUser
{
    public Guid AppUserId { get; set; }
    public Guid CompanyId { get; set; }

    public AppUser AppUser { get; set; }
    public Company Company { get; set; }
}
