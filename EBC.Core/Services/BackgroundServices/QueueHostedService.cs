using Microsoft.Extensions.Hosting;

namespace EBC.Core.Services.BackgroundServices;

/// <summary>
/// Növbədən tapşırıqları asinxron şəkildə işləyən background xidmət sinifi.
/// </summary>
public class QueueHostedService : BackgroundService
{
    private readonly IBackgroundTaskQueue<string> _queue;

    /// <summary>
    /// Yeni bir <see cref="QueueHostedService"/> instansı yaradır.
    /// </summary>
    /// <param name="queue">İşlənəcək tapşırıqları saxlayan növbə.</param>
    public QueueHostedService(IBackgroundTaskQueue<string> queue)
    {
        _queue = queue;
    }

    /// <summary>
    /// Növbədə olan tapşırıqları icra edir.
    /// </summary>
    /// <param name="stoppingToken">Xidmətin dayandırılması üçün istifadə edilən token.</param>
    /// <returns>Asinxron əməliyyat.</returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var workItem = await _queue.DequeueAsync(stoppingToken);

            // İşin icrası (məsələn, e-poçt göndərmə və ya məlumat yeniləmə)
            await Task.Delay(1000); // işin icra müddəti
        }
    }
}

