using EBC.Business.BackgroundServices.EmailQueueService;
using EBC.Core.Services.BackgroundServices;
using EBC.Data.Entities;
using EBC.Data.Repositories.Abstract;
using Microsoft.Extensions.Hosting;

namespace EBC.Business.BackgroundServices.Concrete;

public class EmailQueueHostedService : BackgroundService
{
    private readonly IBackgroundTaskQueue<SendingEmail> _taskQueue;
    private readonly IEmailDispatcherService _dispatcherService;
    private readonly ISendingEmailRepository _sendingEmailRepository;

    public EmailQueueHostedService(
        IBackgroundTaskQueue<SendingEmail> taskQueue,
        IEmailDispatcherService dispatcherService,
        ISendingEmailRepository sendingEmailRepository)
    {
        _taskQueue = taskQueue;
        _dispatcherService = dispatcherService;
        _sendingEmailRepository = sendingEmailRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var updatedIds = new List<Guid>();

        while (!stoppingToken.IsCancellationRequested)
        {
            var email = await _taskQueue.DequeueAsync(stoppingToken);

            await _dispatcherService.ProcessEmailAsync(email, updatedIds);
        }

        if (updatedIds.Any())
        {
            var updatedRows = await _sendingEmailRepository.GetAll(x => updatedIds.Contains(x.Id));

            foreach (var row in updatedRows)
            {
                row.IsSent = true;
            }

            await _sendingEmailRepository.BulkUpdate(updatedRows);
        }
    }
}

