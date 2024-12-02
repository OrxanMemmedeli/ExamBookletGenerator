using EBC.Core.Repositories.Concrete;
using EBC.Data.Entities;
using EBC.Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;


namespace EBC.Data.Repositories.Concrete;

public class AuthenticationHistoryRepository : GenericRepository<AuthenticationHistory>, IAuthenticationHistoryRepository
{
    public AuthenticationHistoryRepository(DbContext context) : base(context)
    {
    }
}
