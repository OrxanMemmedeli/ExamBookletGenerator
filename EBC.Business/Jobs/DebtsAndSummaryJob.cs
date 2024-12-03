using EBC.Core.Jobs.Common;
using EBC.Core.Jobs.Models;
using EBC.Core.Models.Enums;
using EBC.Data.Entities;
using EBC.Data.Repositories.Abstract;
using Hangfire;
using Microsoft.Extensions.Options;
using System.Linq;

namespace EBC.Business.Jobs;

public class DebtsAndSummaryJob : BaseJob
{
    private readonly JobTime _jobTime;
    private readonly IPaymentOrDebtRepository _debtRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IPaymentSummaryRepository _paymentSummaryRepository;

    public DebtsAndSummaryJob(
        JobTime jobTime,
        IPaymentOrDebtRepository debtRepository,
        ICompanyRepository companyRepository,
        IPaymentSummaryRepository paymentSummaryRepository)
    {
        _jobTime = jobTime;
        _debtRepository = debtRepository;
        _companyRepository = companyRepository;
        _paymentSummaryRepository = paymentSummaryRepository;
    }

    public override string CronExpression
        => Cron.Daily(hour: _jobTime.Hour, minute: _jobTime.Minute); 

    public override async Task Execute()
    {
        List<Company> companies = await _companyRepository.GetAll(x =>
                !x.IsDeleted &&
                !x.IsFree &&
                !x.IsStopSubscription &&
                x.JoinDate <= DateTime.Now);

        await GenerateDebts(companies);

        await CalculatePaymentSummaries(companies);
    }

    private async Task CalculatePaymentSummaries(List<Company> companies)
    {
        var companyIds = companies.Select(o => o.Id);
        var paymentsummaries = await _paymentSummaryRepository.GetAll(x => companyIds.Contains(x.CompanyId));
        var debtAndPayments = await _debtRepository.GetAll(x => companyIds.Contains(x.CompanyId));

        foreach (var summary in paymentsummaries)
        {
            summary.TotalPayment = debtAndPayments.Where(x => x.CompanyId == summary.CompanyId && x.PaymentType == PaymentType.Payment).Sum(x => x.Amount);
            summary.TotalDebt = debtAndPayments.Where(x => x.CompanyId == summary.CompanyId && x.PaymentType == PaymentType.Debt).Sum(x => x.Amount);
            summary.CurrentDebt = summary.TotalDebt - summary.TotalPayment;
        }

        await _paymentSummaryRepository.BulkUpdate(paymentsummaries);
    }

    private async Task GenerateDebts(List<Company> companies)
    {
        List<PaymentOrDebt> debts = new List<PaymentOrDebt>();

        foreach (var company in companies.Where(x => !x.IsPenal))
        {
            debts.Add(new PaymentOrDebt()
            {
                Amount = company.DailyAmount,
                CompanyId = company.Id,
                PaymentType = PaymentType.Debt
            });
        }

        foreach (var company in companies.Where(x => x.IsPenal))
        {
            debts.Add(new PaymentOrDebt()
            {
                Amount = company.DailyAmount + (company.DailyAmount * company.PercentOfFine / 100),
                CompanyId = company.Id,
                PaymentType = PaymentType.Debt
            });
        }

        await _debtRepository.AddRangeAsync(debts);
    }
}
