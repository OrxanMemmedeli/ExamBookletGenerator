using EBC.Core.Helpers.Extensions.Reader;
using EBC.Core.Jobs.Common;
using EBC.Core.Jobs.Models;
using EBC.Core.Services.BackgroundServices;
using EBC.Core.Services.EmailService;
using EBC.Data.Entities;
using EBC.Data.Enums;
using EBC.Data.Repositories.Abstract;
using Hangfire;
using Microsoft.Extensions.Options;
using System.Linq;

namespace EBC.Business.Jobs;

public class EmailsSenderJob : BaseJob
{
    private readonly JobTime _jobTime;
    private readonly ISendingEmailRepository _sendingEmailRepository;
    private readonly IBackgroundTaskQueue<SendingEmail> _taskQueue;
    public EmailsSenderJob(
        JobTime jobTime,
        ISendingEmailRepository sendingEmailRepository,
        IBackgroundTaskQueue<SendingEmail> taskQueue)
    {
        _jobTime = jobTime;
        _sendingEmailRepository = sendingEmailRepository;
        _taskQueue = taskQueue;
    }

    public override string CronExpression
        => Cron.Daily(hour: _jobTime.Hour, minute: _jobTime.Minute); 

    public override async Task Execute()
    {
        var subjectTypes = new[] { EmailSubjectType.ComeBack, EmailSubjectType.Welcome, EmailSubjectType.StopSubscription, EmailSubjectType.Blocked };

        var emails = await _sendingEmailRepository.GetAll(x => !x.IsSent && subjectTypes.Contains(x.SubjectType), true, null, x => x.Company);

        foreach (var email in emails)
        {
            await _taskQueue.QueueAsync(email);
        }
    }
}
