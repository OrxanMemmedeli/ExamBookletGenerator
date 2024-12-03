using EBC.Data.Entities;

namespace EBC.Business.BackgroundServices.EmailQueueService;

public interface IEmailDispatcherService
{
    Task ProcessEmailAsync(SendingEmail email, List<Guid> updatedIds);
}
