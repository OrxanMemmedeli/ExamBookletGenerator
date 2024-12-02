using EBC.Core.Repositories.Concrete;
using EBC.Data.Entities;
using EBC.Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EBC.Data.Repositories.Concrete;

public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
{
    public SubjectRepository(DbContext context) : base(context)
    {
    }
}
