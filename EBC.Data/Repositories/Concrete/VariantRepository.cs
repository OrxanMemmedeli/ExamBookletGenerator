using EBC.Core.Repositories.Concrete;
using EBC.Data.Entities;
using EBC.Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EBC.Data.Repositories.Concrete;

public class VariantRepository : GenericRepository<Variant>, IVariantRepository
{
    public VariantRepository(DbContext context) : base(context)
    {
    }
}
