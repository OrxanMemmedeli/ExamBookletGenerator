using EBC.Core.Entities.Common;

namespace EBC.Data.Entities;

public class AuthenticationHistory : BaseEntity<Guid>
{
    public string IPAdress { get; set; }
    public bool IsLoggedIn { get; set; }
    public Guid CompanyId { get; set; }
    public virtual Company Company { get; set; }

}
