using EBC.Core.Repositories.Concrete;
using EBC.Data.Entities;
using EBC.Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EBC.Data.Repositories.Concrete;

public class SubjectParameterRepository : GenericRepository<SubjectParameter>, ISubjectParameterRepository
{
    public SubjectParameterRepository(DbContext context) : base(context)
    {
    }
}
