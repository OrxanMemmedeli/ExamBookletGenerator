using EBC.Core.Repositories.Concrete;
using EBC.Data.Entities;
using EBC.Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EBC.Data.Repositories.Concrete;

public class GradeRepository : GenericRepository<Grade>, IGradeRepository
{
    public GradeRepository(DbContext context) : base(context)
    {
    }
}
