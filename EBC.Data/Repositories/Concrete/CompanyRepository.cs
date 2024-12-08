using EBC.Core.Constants;
using EBC.Core.Helpers.Extensions.Reader;
using EBC.Core.Helpers.Generator;
using EBC.Core.Helpers.Generator.EmailTemplate;
using EBC.Core.Models.ResultModel;
using EBC.Core.Repositories.Concrete;
using EBC.Core.Services.Abstract;
using EBC.Data.DTOs.Company;
using EBC.Data.Entities;
using EBC.Data.Enums;
using EBC.Data.Repositories.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EBC.Data.Repositories.Concrete;

public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
{
    private readonly IUrlService _urlService;
    private readonly ISendingEmailRepository _sendingEmailRepository;
    private readonly IPaymentSummaryRepository _paymentSummaryRepository;
    private readonly ICompanyRepository _companyRepository;

    public CompanyRepository(
        DbContext context,
        IUrlService urlService,
        ISendingEmailRepository sendingEmailRepository,
        IPaymentSummaryRepository paymentSummaryRepository,
        ICompanyRepository companyRepository) : base(context)
    {
        _urlService = urlService;
        _sendingEmailRepository = sendingEmailRepository;
        _paymentSummaryRepository = paymentSummaryRepository;
        _companyRepository = companyRepository;
    }

    public async Task<Result> EditCompanyWithoutPayment(CompanyEditDTO company)
    {
        var mainEntity = await base.GetSingleAsync(x => x.Id == company.Id);

        mainEntity.Name = company.Name;
        mainEntity.Domain = company.Domain;
        mainEntity.DailyAmount = company.DailyAmount;
        mainEntity.DebtLimit = company.DebtLimit;
        mainEntity.PercentOfFine = company.PercentOfFine;
        mainEntity.Code = company.Code;
        mainEntity.IsFree = company.IsFree;
        mainEntity.IsStopSubscription = company.IsStopSubscription;

        var result = await base.UpdateAsync(mainEntity);

        return result > 0
            ? Result.Success()
            : Result.Failure(ExceptionMessage.AnErrorWhenSave);
    }

    public async Task<Result> SaveCompanyWithPaymentSummary(Company company, HttpContext httpContext)
    {
        Guid companyId = Guid.NewGuid();
        Guid summaryId = Guid.NewGuid();


        Parallel.Invoke(
            () => SavePaymentSummaryEntity(companyId, summaryId),
            () => SaveCompanyEntity(company, companyId, summaryId),
            () => SetConfirmAndWelcomeEmails(company)
        );

        var result = await SaveChangesAsync();

        return result > 0
            ? Result.Success()
            : Result.Failure(ExceptionMessage.AnErrorWhenSave);
    }

    public async Task<Result> StartSubscription(Guid id)
    {
        var company = await base.GetByIdAsync(id);
        company.IsStopSubscription = false;

        StartSubscriptionEmail(company);
        var result = await base.UpdateAsync(company);

        return result > 0
            ? Result.Success()
            : Result.Failure(ExceptionMessage.AnErrorWhenSave);
    }

    public async Task<Result> StopSubscription(Guid id)
    {
        var company = await base.GetByIdAsync(id, false, x => x.PaymentSummary);

        if (company.PaymentSummary?.CurrentDebt > 0)
            return Result.Failure(string.Format(ExceptionMessage.HaveDebt, company.Name, company.PaymentSummary.CurrentDebt));

        company.IsStopSubscription = true;

        StopSubscriptionEmail(company);
        var result = await base.UpdateAsync(company);

        return result > 0
            ? Result.Success()
            : Result.Failure(ExceptionMessage.AnErrorWhenSave);
    }

    public List<CompanyCacheViewDTO> GetCompaniesForCache()
        => base.GetAllQueryable(x => true, true, null, x => x.PaymentSummary).Select(x => new CompanyCacheViewDTO
        {
            Id = x.Id,

            Name = x.Name,
            Domain = x.Domain,
            Code = x.Code,
            Key = x.Key,

            BlockedCompanyDate = x.BlockedCompanyDate,

            DailyAmount = x.DailyAmount,
            DebtLimit = x.DebtLimit,
            PercentOfFine = x.PercentOfFine,

            IsActive = x.IsActive,
            IsFree = x.IsFree,
            IsPenal = x.IsPenal,
            IsStopSubscription = x.IsStopSubscription,

            CurrentDebt = x.PaymentSummary == null ? 0 : x.PaymentSummary.CurrentDebt,
            TotalDebt = x.PaymentSummary == null ? 0 : x.PaymentSummary.TotalDebt,
            TotalPayment = x.PaymentSummary == null ? 0 : x.PaymentSummary.TotalPayment,

            IsDeleted = x.IsDeleted,
            Status = x.Status
        }).ToList();


    public Result<CompanyAccountResponceDTO> GetAccountResponce(CompanyCacheViewDTO company)
    {
        if (company.IsStopSubscription)
        {
            return Result<CompanyAccountResponceDTO>.Failure(new CompanyAccountResponceDTO
            {
                CurrentDebt = company.CurrentDebt,
                Name = company.Name,
                TotalDebt = company.TotalDebt,
                TotalPayment = company.TotalPayment
            }, string.Format(ExceptionMessage.StopSubscriptionCompany, company.Name));
        }
        else if (!company.IsActive)
        {
            return Result<CompanyAccountResponceDTO>.Failure(new CompanyAccountResponceDTO
            {
                CurrentDebt = company.CurrentDebt,
                Name = company.Name,
                TotalDebt = company.TotalDebt,
                TotalPayment = company.TotalPayment
            }, string.Format(ExceptionMessage.DeActiveCompany,
                company.Name,
                $"{company.TotalPayment} ₼",
                $"{company.TotalDebt} ₼",
                $"{(company.IsPenal == true ? (company.DailyAmount * company.PercentOfFine / 100) : company.DailyAmount)} ₼",
                company.BlockedCompanyDate?.ToString("dd/MM/yyyy"))
            );
        }
        else if (company.IsFree && company.IsActive && !company.IsDeleted && !company.IsStopSubscription)
        {
            return Result<CompanyAccountResponceDTO>.Success(new CompanyAccountResponceDTO
            {
                CurrentDebt = company.CurrentDebt,
                Name = company.Name,
                TotalDebt = company.TotalDebt,
                TotalPayment = company.TotalPayment
            });
        }
        else if (!company.IsFree && company.IsActive && !company.IsDeleted && !company.IsStopSubscription)
        {
            return Result<CompanyAccountResponceDTO>.Success(new CompanyAccountResponceDTO
            {
                CurrentDebt = company.CurrentDebt,
                Name = company.Name,
                TotalDebt = company.TotalDebt,
                TotalPayment = company.TotalPayment
            });
        }

        return Result<CompanyAccountResponceDTO>.Failure(ExceptionMessage.ErrorOccured);
    }

    public async Task<Result> GenerateNewSecretKey(Guid id)
    {
        var company = await base.GetByIdAsync(id);

        company.Key = KeyGenerator.Generate(30, false);

        var result = await base.UpdateAsync(company);

        return result > 0
            ? Result.Success()
            : Result.Failure(ExceptionMessage.AnErrorWhenSave);
    }

    public async Task<CompanyCommonDataDTO> GetCompanyGeneralData(Expression<Func<Company, bool>> predicate)
        => await base.entity.Where(predicate)
                .Include(x => x.PaymentSummary)
                .Select(x => x.PaymentSummary)
                .GroupBy(x => 1)
                .Select(g => new CompanyCommonDataDTO
                {
                    CompanyCount = g.Count(),
                    TotalPayment = g.Sum(x => x.TotalPayment),
                    TotalDebt = g.Sum(x => x.TotalDebt),
                    CurrentDebt = g.Sum(x => x.CurrentDebt)
                })
                .FirstOrDefaultAsync() ?? new CompanyCommonDataDTO();

    public async Task<Result> ConfirmMail(string encodeData)
    {
        Guid id = _urlService.ConfirmUrl(encodeData);
        var company = await base.GetByIdAsync(id);

        if (company == null)
            return Result.Failure(ExceptionMessage.AnErrorWhenSave);
        else
        {
            if (company.IsConfirm == true)
                return Result.Success();

            company.IsConfirm = true;

            var result = await base.UpdateAsync(company);

            return result > 0
                ? Result.Success()
                : Result.Failure(ExceptionMessage.AnErrorWhenSave);
        }

    }

    #region PrivateMethods
    private void SavePaymentSummaryEntity(Guid companyId, Guid summaryId)
    {
        PaymentSummary summary = new PaymentSummary
        {
            TotalDebt = 0,
            TotalPayment = 0,
            CurrentDebt = 0,
            CompanyId = companyId,
            Id = summaryId,
        };

        _paymentSummaryRepository.AddWithoutSave(summary);
    }

    private void SaveCompanyEntity(Company company, Guid companyId, Guid summaryId)
    {
        company.Id = companyId;
        company.Key = KeyGenerator.Generate(30, false);
        company.IsPenal = false;
        company.IsActive = true;

        _companyRepository.AddWithoutSave(company);
    }

    private void SetConfirmAndWelcomeEmails(Company company)
    {
        string url = _urlService.GenerateUrl(company.Id);

        var emailTemplateGeneratorConfirm = new EmailTemplateGenerator(ApplicationCommonField.ApplicationName);
        var emailTemplateGeneratorWelcome = new EmailTemplateGenerator(ApplicationCommonField.ApplicationName);

        EmailTemplate _contentConfirm = emailTemplateGeneratorConfirm.GenerateConfirmEmail(company.Name, url);
        EmailTemplate _contentWelcome = emailTemplateGeneratorWelcome.GenerateWelcomeEmail(company.Name, company.DailyAmount, company.DebtLimit, company.PercentOfFine);

        _sendingEmailRepository.AddRangeAsyncWithoutSave(new List<SendingEmail>
        {
            new SendingEmail()
            {
                CompanyId = company.Id,
                SubjectType = EmailSubjectType.Confirm,
                IsSent = false,
                Subject = EmailSubjectType.Confirm.GetDescription(),
                Content = _contentConfirm.GetHtmlContent()
            },
            new SendingEmail()
            {
                CompanyId = company.Id,
                SubjectType = EmailSubjectType.Welcome,
                IsSent = false,
                Subject = EmailSubjectType.Welcome.GetDescription(),
                Content = _contentWelcome.GetHtmlContent()
            }
        });
    }

    private void StartSubscriptionEmail(Company company)
    {
        var emailTemplateGenerator = new EmailTemplateGenerator(ApplicationCommonField.ApplicationName);

        EmailTemplate _content = emailTemplateGenerator.GenerateComeBackEmail(company.Name, company.DailyAmount, company.DebtLimit, company.PercentOfFine);

        _sendingEmailRepository.AddAsync(new SendingEmail()
        {
            CompanyId = company.Id,
            SubjectType = EmailSubjectType.ComeBack,
            IsSent = false,
            Subject = EmailSubjectType.ComeBack.GetDescription(),
            Content = _content.GetHtmlContent()
        });
    }

    private void StopSubscriptionEmail(Company company)
    {
        var emailTemplateGenerator = new EmailTemplateGenerator(ApplicationCommonField.ApplicationName);

        EmailTemplate _content = emailTemplateGenerator.GenerateStopSubscriptionEmail(company.Name);

        _sendingEmailRepository.AddAsync(new SendingEmail()
        {
            CompanyId = company.Id,
            SubjectType = EmailSubjectType.StopSubscription,
            IsSent = false,
            Subject = EmailSubjectType.StopSubscription.GetDescription(),
            Content = _content.GetHtmlContent()
        });
    }

    #endregion
}
