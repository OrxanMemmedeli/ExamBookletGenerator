using EBC.Core.Repositories.Concrete;
using EBC.Data.Entities;
using EBC.Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EBC.Data.Repositories.Concrete;

public class SysExceptionRepository : GenericRepository<SysException>, ISysExceptionRepository
{
    public SysExceptionRepository(DbContext context) : base(context)
    {
    }
}
