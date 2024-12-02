using EBC.Core.Repositories.Concrete;
using EBC.Data.Entities;
using EBC.Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EBC.Data.Repositories.Concrete;

public class UserTypeRepository : GenericRepository<UserType>, IUserTypeRepository
{
    public UserTypeRepository(DbContext context) : base(context)
    {
    }
}
