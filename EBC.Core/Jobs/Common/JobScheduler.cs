using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace EBC.Core.Jobs.Common;

/// <summary>
/// Sistemdəki bütün işləri Hangfire üzərindən qeyd edən statik köməkçi sinif.
/// </summary>
public static class JobScheduler
{
    /// <summary>
    /// Sistemdə olan bütün işləri avtomatik qeyd edir.
    /// </summary>
    /// <param name="serviceProvider">DI kontekstindəki servislər.</param>
    public static void RegisterJobs(IServiceProvider serviceProvider)
    {
        // BaseJob sinifindən miras alan bütün iş tiplərini əldə edir
        var jobTypes = typeof(BaseJob).Assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(BaseJob)) && !t.IsAbstract);

        // Hər bir iş tipi üçün Hangfire üzərindən qeydiyyat aparır
        foreach (var jobType in jobTypes)
        {
            // İş obyektini yaradıb vaxt təyinatını əldə edir
            var job = (BaseJob)serviceProvider.GetRequiredService(jobType);
            var options = new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Local
            };

            RecurringJob.AddOrUpdate(
                jobType.Name,
                () => job.Execute(),
                job.CronExpression,
                options
            );
        }
    }
}


/* 
 
/// <summary>
/// Nümunə olaraq iş sinifi yaradılır və icra ediləcək iş burada təyin olunur.
/// </summary>
public class SampleJob : BaseJob
{
    public override string CronExpression => Cron.Daily(); // Bu iş hər gün təyin ediləcək.

    /// <summary>
    /// İşin yerinə yetiriləcəyi metod.
    /// </summary>
    public override async Task Execute()
    {
        // İş zamanı görüləcək əməliyyatlar
        Console.WriteLine("Sample job executed.");
        await Task.CompletedTask;
    }
}
 
 */