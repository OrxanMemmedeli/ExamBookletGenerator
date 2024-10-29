using EBC.Core.Entities.Identity;
using EBC.Core.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EBC.Core.Repositories.Concrete;

public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
{
    public UserRoleRepository(DbContext context) : base(context)
    {
    }
}
