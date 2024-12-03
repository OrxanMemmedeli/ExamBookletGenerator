using DocumentFormat.OpenXml.Spreadsheet;
using EBC.Core.Constants;
using EBC.Core.Helpers.Extensions.Reader;
using EBC.Core.Helpers.Generator.EmailTemplate;
using EBC.Core.Jobs.Common;
using EBC.Core.Jobs.Models;
using EBC.Core.Services.BackgroundServices;
using EBC.Core.Services.EmailService;
using EBC.Data.Entities;
using EBC.Data.Enums;
using EBC.Data.Repositories.Abstract;
using Hangfire;

namespace EBC.Business.Jobs;

public class DebtEmailSenderJob : BaseJob
{
    private readonly JobTime _jobTime;
    private readonly ICompanyRepository _companyRepository;
    private readonly IBackgroundTaskQueue<SendingEmail> _taskQueue;

    public DebtEmailSenderJob(
        JobTime jobTime,
        ICompanyRepository companyRepository,
        IBackgroundTaskQueue<SendingEmail> taskQueue)
    {
        _jobTime = jobTime;
        _companyRepository = companyRepository;
        _taskQueue = taskQueue;
    }

    public override string CronExpression
        => Cron.Monthly(day: _jobTime.Day, hour: _jobTime.Hour); 

    public override async Task Execute()
    {
        var companies = await _companyRepository.GetAll(x => x.IsConfirm && !x.IsStopSubscription && !x.IsDeleted && !x.IsFree, true, null, x => x.PaymentSummary);

        foreach (var company in companies)
        {
            await SendEmail(company);
        }
    }

    private async Task SendEmail(Company company)
    {
        var templateGenerator = new EmailTemplateGenerator(ApplicationCommonField.ApplicationName);

        EmailTemplate _content = templateGenerator.GenerateDebtEmail(company.Name, company.PaymentSummary?.TotalPayment, company.PaymentSummary?.TotalDebt, company.PaymentSummary?.CurrentDebt);

        await _taskQueue.QueueAsync(new SendingEmail
        {
            Company = company,
            SubjectType = EmailSubjectType.Debt,
            Content = _content.GetHtmlContent()
        });      
    }
}
