using EBC.Core.Constants;
using EBC.Core.Helpers.Extensions.Reader;
using EBC.Core.Helpers.Generator.EmailTemplate;
using EBC.Core.Jobs.Common;
using EBC.Core.Jobs.Models;
using EBC.Data.Entities;
using EBC.Data.Enums;
using EBC.Data.Repositories.Abstract;
using Hangfire;

namespace EBC.Business.Jobs;

public class CompanyFieldsControlJob : BaseJob
{
    private readonly JobTime _jobTime;
    private readonly ICompanyRepository _companyRepository;
    private readonly ISendingEmailRepository _sendingEmailRepository;

    public CompanyFieldsControlJob(
        JobTime jobTime, 
        ICompanyRepository companyRepository, 
        ISendingEmailRepository sendingEmailRepository)
    {
        _jobTime = jobTime;
        _companyRepository = companyRepository;
        _sendingEmailRepository = sendingEmailRepository;
    }

    public override string CronExpression 
        => Cron.Daily(hour: _jobTime.Hour, minute: _jobTime.Minute); //00:25 hər gecə

    public override async Task Execute()
    {
        List<Company> companies = await _companyRepository.GetAll(x =>
                    !x.IsDeleted &&
                    !x.IsFree &&
                    x.IsActive &&
                    !x.IsStopSubscription &&
                    x.JoinDate <= DateTime.Now
                    , true, null, i => i.PaymentSummary);

        List<Company> updatedList = new List<Company>();

        foreach (Company comp in companies)
        {
            if (comp?.PaymentSummary?.CurrentDebt > 0 && comp.PaymentSummary.CurrentDebt <= comp.DebtLimit)
                comp.IsPenal = true;
            else if (comp?.PaymentSummary?.CurrentDebt > 0 && comp?.PaymentSummary?.CurrentDebt > comp?.DebtLimit)
            {
                comp.IsActive = false;
                comp.IsPenal = true;
                comp.BlockedCompanyDate = DateTime.Now;

                SetBlockedEmail(comp);
            }

            comp.PaymentSummary = new PaymentSummary();

            updatedList.Add(comp);
        }

        await _companyRepository.BulkUpdate(updatedList);
    }

    private void SetBlockedEmail(Company comp)
    {
        var templateGenerator = new EmailTemplateGenerator(ApplicationCommonField.ApplicationName);

        EmailTemplate _content = templateGenerator.GenerateBlockedEmail(comp.Name, comp.PaymentSummary.TotalPayment, comp.PaymentSummary.TotalDebt, comp.PaymentSummary.CurrentDebt);

        _sendingEmailRepository.AddWithoutSave(new SendingEmail()
        {
            CompanyId = comp.Id,
            SubjectType = EmailSubjectType.Blocked,
            IsSent = false,
            Subject = EmailSubjectType.Blocked.GetDescription(),
            Content = _content.GetHtmlContent()
        });
    }
}
