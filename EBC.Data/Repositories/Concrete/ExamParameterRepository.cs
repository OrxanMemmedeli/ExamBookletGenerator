using EBC.Core.Repositories.Concrete;
using EBC.Data.Entities;
using EBC.Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EBC.Data.Repositories.Concrete;

public class ExamParameterRepository : GenericRepository<ExamParameter>, IExamParameterRepository
{
    public ExamParameterRepository(DbContext context) : base(context)
    {
    }
}
