using EBC.Core.Repositories.Concrete;
using EBC.Data.Entities;
using EBC.Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EBC.Data.Repositories.Concrete;

public class QuestionTypeRepository : GenericRepository<QuestionType>, IQuestionTypeRepository
{
    public QuestionTypeRepository(DbContext context) : base(context)
    {
    }
}
