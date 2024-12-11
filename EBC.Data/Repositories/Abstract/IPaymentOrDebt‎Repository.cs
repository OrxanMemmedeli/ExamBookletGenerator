using EBC.Core.Models.ResultModel;
using EBC.Core.Repositories.Abstract;
using EBC.Data.Entities;

namespace EBC.Data.Repositories.Abstract;

public interface IPaymentOrDebtRepository : IGenericRepository<PaymentOrDebt>
{
    Task<Result> AddPaymentAsnyc(PaymentOrDebt paymentOrDebt);
}
