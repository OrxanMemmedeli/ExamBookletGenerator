using EBC.Business.BackgroundServices.EmailQueueService;
using EBC.Core.Helpers.Extensions.Reader;
using EBC.Core.Services.EmailService;
using EBC.Data.Entities;

namespace EBC.Business.BackgroundServices.Concrete;

public class EmailDispatcherService : IEmailDispatcherService
{
    private readonly IEmailService _emailService;

    public EmailDispatcherService(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task ProcessEmailAsync(SendingEmail email, List<Guid> updatedIds)
    {
        var result = await _emailService.SendEmailAsync(
            email.Company.EmailAdress,
            email.SubjectType.GetDescription(),
            email.Content,
            true
        );

        if (result.IsSuccess)
        {
            updatedIds.Add(email.Id);
        }
    }
}