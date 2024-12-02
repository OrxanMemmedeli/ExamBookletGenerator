using EBC.Core.Repositories.Concrete;
using EBC.Data.Entities;
using EBC.Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EBC.Data.Repositories.Concrete;

public class GroupRepository : GenericRepository<Group>, IGroupRepository
{
    public GroupRepository(DbContext context) : base(context)
    {
    }
}
