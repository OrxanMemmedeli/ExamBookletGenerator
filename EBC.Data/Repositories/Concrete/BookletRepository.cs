using EBC.Core.Repositories.Concrete;
using EBC.Data.Entities;
using EBC.Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EBC.Data.Repositories.Concrete;

public class BookletRepository : GenericRepository<Booklet>, IBookletRepository
{
    public BookletRepository(DbContext context) : base(context)
    {
    }
}
