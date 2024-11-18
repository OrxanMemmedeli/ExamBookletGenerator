using EBC.Core.Entities;
using EBC.Core.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBC.Core.Repositories.Concrete;

public class SysExceptionRepository : GenericRepository<SysException>, ISysExceptionRepository
{
    public SysExceptionRepository(DbContext context) : base(context)
    {
    }
}
