using EBC.Core.Repositories.Concrete;
using EBC.Data.Entities.Identity;
using EBC.Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EBC.Data.Repositories.Concrete;

public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    public RoleRepository(DbContext context) : base(context)
    {
    }
}
