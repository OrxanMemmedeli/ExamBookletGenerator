using EBC.Core.Repositories.Concrete;
using EBC.Data.Entities;
using EBC.Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EBC.Data.Repositories.Concrete;

public class ExamRepository : GenericRepository<Exam>, IExamRepository
{
    public ExamRepository(DbContext context) : base(context)
    {
    }
}
