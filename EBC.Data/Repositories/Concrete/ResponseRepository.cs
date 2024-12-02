using EBC.Core.Repositories.Concrete;
using EBC.Data.Entities;
using EBC.Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EBC.Data.Repositories.Concrete;

public class ResponseRepository : GenericRepository<Response>, IResponseRepository
{
    public ResponseRepository(DbContext context) : base(context)
    {
    }
}
