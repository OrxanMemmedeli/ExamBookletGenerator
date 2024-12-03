using EBC.Core.Helpers.Extensions.Reader;
using EBC.Core.Jobs.Common;
using EBC.Core.Jobs.Models;
using EBC.Core.Services.BackgroundServices;
using EBC.Core.Services.EmailService;
using EBC.Data.Entities;
using EBC.Data.Enums;
using EBC.Data.Repositories.Abstract;
using Hangfire;

namespace EBC.Business.Jobs;

public class ConfirmAndPaymentEmailsSenderJob : BaseJob
{
    private readonly JobTime _jobTime;
    private readonly ISendingEmailRepository _sendingEmailRepository;
    private readonly IBackgroundTaskQueue<SendingEmail> _taskQueue;

    public ConfirmAndPaymentEmailsSenderJob(
        JobTime jobTime, 
        ISendingEmailRepository sendingEmailRepository,
        IBackgroundTaskQueue<SendingEmail> taskQueue)
    {
        _jobTime = jobTime;
        _sendingEmailRepository = sendingEmailRepository;
        _taskQueue = taskQueue;
    }

    public override string CronExpression
        => Cron.Hourly(minute: _jobTime.Minute); 

    public override async Task Execute()
    {
        var subjectTypes = new[] { EmailSubjectType.Payment, EmailSubjectType.Confirm };
        var emails = await _sendingEmailRepository.GetAll(x => !x.IsSent && subjectTypes.Contains(x.SubjectType), true, null, x => x.Company);

        List<Guid> updated = new List<Guid>();

        foreach (var email in emails)
        {
            await _taskQueue.QueueAsync(email);
        }
    }
}
