using EBC.Core.Entities.Identity;
using EBC.Core.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EBC.Core.Repositories.Concrete;

public class OrganizationAdressRoleRepository : GenericRepository<OrganizationAdressRole>, IOrganizationAdressRoleRepository
{
    public OrganizationAdressRoleRepository(DbContext context) : base(context)
    {
    }
}
