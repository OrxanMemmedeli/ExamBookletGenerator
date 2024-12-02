using EBC.Core.Repositories.Concrete;
using EBC.Data.Entities;
using EBC.Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EBC.Data.Repositories.Concrete;

public class PaymentSummaryRepository : GenericRepository<PaymentSummary>, IPaymentSummaryRepository
{
    public PaymentSummaryRepository(DbContext context) : base(context)
    {
    }
}
