using EBC.Core.Repositories.Concrete;
using EBC.Data.Entities;
using EBC.Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EBC.Data.Repositories.Concrete;

public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
{
    public QuestionRepository(DbContext context) : base(context)
    {
    }
}
