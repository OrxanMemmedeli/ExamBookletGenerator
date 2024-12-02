using EBC.Core.Repositories.Concrete;
using EBC.Data.Entities;
using EBC.Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EBC.Data.Repositories.Concrete;

public class SectionRepository : GenericRepository<Section>, ISectionRepository
{
    public SectionRepository(DbContext context) : base(context)
    {
    }
}
