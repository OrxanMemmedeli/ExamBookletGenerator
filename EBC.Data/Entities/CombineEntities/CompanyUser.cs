using EBC.Data.Entities.Identity;

namespace EBC.Data.Entities.CombineEntities;

public class CompanyUser
{
    public Guid UserId { get; set; }
    public Guid CompanyId { get; set; }

    public User User { get; set; }
    public Company Company { get; set; }
}
