using EBC.Core.Constants;
using EBC.Core.Helpers.Extensions.Reader;
using EBC.Core.Helpers.Generator.EmailTemplate;
using EBC.Core.Models.ResultModel;
using EBC.Core.Repositories.Concrete;
using EBC.Data.Entities;
using EBC.Data.Enums;
using EBC.Data.Repositories.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EBC.Data.Repositories.Concrete;

public class PaymentOrDebt‎Repository : GenericRepository<PaymentOrDebt>, IPaymentOrDebtRepository
{
    private readonly ISendingEmailRepository _sendingEmailRepository;
    public PaymentOrDebtRepository(DbContext context, ISendingEmailRepository sendingEmailRepository) : base(context)
    {
        _sendingEmailRepository = sendingEmailRepository;
    }

    public async Task<Result> AddPaymentAsnyc(PaymentOrDebt paymentOrDebt, ICompanyRepository _companyRepository)
    {
        var company = await _companyRepository.GetByIdAsync(paymentOrDebt.CompanyId, false, x => x.PaymentSummary);

        var message = string.Empty;

        SetPayment(paymentOrDebt, _companyRepository, company, out message);
        SetEmail(paymentOrDebt, company);

        return await SaveChangesAsync() > 0
            ? Result.Success()
            : Result.Failure(ExceptionMessage.AnErrorWhenSave);
    }

    private void SetEmail(PaymentOrDebt paymentOrDebt, Company company)
    {
        var emailTemplateGenerator = new EmailTemplateGenerator(ApplicationCommonField.ApplicationName);

        EmailTemplate _content = emailTemplateGenerator.GeneratePaymentEmail(company.Name, paymentOrDebt.Amount);

        _sendingEmailRepository.AddAsync(new SendingEmail()
        {
            CompanyId = company.Id,
            SubjectType = EmailSubjectType.Payment,
            IsSent = false,
            Subject = EmailSubjectType.Payment.GetDescription(),
            Content = _content.GetHtmlContent()
        });
    }

    private void SetPayment(PaymentOrDebt paymentOrDebt, ICompanyRepository _companyRepository, Company company, out string message)
    {
        if (!company.IsActive && company.PaymentSummary.CurrentDebt <= paymentOrDebt.Amount)
        {
            company.PaymentSummary = null;
            company.IsActive = true;
            company.IsPenal = false;

            _companyRepository.UpdateWithoutSave(company);
            base.AddWithoutSave(paymentOrDebt);
            message = "Borc tam ödənildi və sistem aktiv edildi.";
        }
        else
        {
            base.AddWithoutSave(paymentOrDebt);
            message = $"Borc tam ödənilmədiyi üçün sistemə giriş aktiv edilmədi. Sistemin açılması üçün əlavə olaraq {company.PaymentSummary.CurrentDebt - paymentOrDebt.Amount} ₼ ödəniş edilməlidir.";
        }
    }
}
