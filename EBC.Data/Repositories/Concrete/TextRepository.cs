using EBC.Core.Repositories.Concrete;
using EBC.Data.Entities;
using EBC.Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EBC.Data.Repositories.Concrete;

public class TextRepository : GenericRepository<Text>, ITextRepository
{
    public TextRepository(DbContext context) : base(context)
    {
    }
}
