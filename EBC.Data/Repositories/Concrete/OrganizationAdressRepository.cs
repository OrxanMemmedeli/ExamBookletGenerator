using EBC.Core.Repositories.Concrete;
using EBC.Data.Entities.Identity;
using EBC.Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EBC.Data.Repositories.Concrete;

public class OrganizationAdressRepository : GenericRepository<OrganizationAdress>, IOrganizationAdressRepository
{
    public OrganizationAdressRepository(DbContext context) : base(context)
    {
    }
}
