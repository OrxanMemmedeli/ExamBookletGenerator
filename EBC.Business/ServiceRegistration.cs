using EBC.Business.BackgroundServices.Concrete;
using EBC.Business.BackgroundServices.EmailQueueService;
using EBC.Core;
using EBC.Core.Services.BackgroundServices;
using EBC.Data.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBC.Business;

public static class ServiceRegistration
{
    public static IServiceCollection AddBusinessLayerServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // BacgroundService
        RegisterBackgroundService(services, configuration);

        return services;
    }

    private static void RegisterBackgroundService(IServiceCollection services, IConfiguration configuration)
    {
        if (!ServiceOptions.UseBackgroundService)
            return;

        // BackgroundTaskQueue üçün SendingEmail növünü qeydiyyatdan keçirin
        services.AddSingleton<IBackgroundTaskQueue<SendingEmail>>(provider => new BackgroundTaskQueue<SendingEmail>(configuration));

        // EmailQueueHostedService xidmətini qeydiyyatdan keçirin
        services.AddHostedService<EmailQueueHostedService>();

        // EmailDispatcherService qeydiyyatı
        services.AddScoped<IEmailDispatcherService, EmailDispatcherService>();

    }
}
