using EBC.Core.Repositories.Abstract;
using EBC.Core.Repositories.Concrete;
using EBC.Data.Entities;
using EBC.Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EBC.Data.Repositories.Concrete;

public class SendingEmailRepository : GenericRepository<SendingEmail>, ISendingEmailRepository
{
    public SendingEmailRepository(DbContext context) : base(context)
    {
    }
}
