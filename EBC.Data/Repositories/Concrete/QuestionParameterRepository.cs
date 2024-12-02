using EBC.Core.Repositories.Concrete;
using EBC.Data.Entities;
using EBC.Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EBC.Data.Repositories.Concrete;

public class QuestionParameterRepository : GenericRepository<QuestionParameter>, IQuestionParameterRepository
{
    public QuestionParameterRepository(DbContext context) : base(context)
    {
    }
}
