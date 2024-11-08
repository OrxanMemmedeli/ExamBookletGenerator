using EBC.Core.Constants;
using Microsoft.Extensions.Configuration;
using System.Threading.Channels;

namespace EBC.Core.Services.BackgroundServices;

/// <summary>
/// Asinxron olaraq işləyən tapşırıq növbəsini idarə edən sinif.
/// </summary>
/// <typeparam name="T">Tapşırıq növü.</typeparam>
public class BackgroundTaskQueue<T> : IBackgroundTaskQueue<T>
{
    private readonly Channel<T> _queue;

    /// <summary>
    /// Yeni bir <see cref="BackgroundTaskQueue{T}"/> instansı yaradır.
    /// </summary>
    /// <param name="configuration">Queue ölçüsünü təyin etmək üçün konfiqurasiya obyekti.</param>
    public BackgroundTaskQueue(IConfiguration configuration)
    {
        int.TryParse(configuration[ApplicationCommonField.QueueCapacity], out int capacity);

        var options = new BoundedChannelOptions(capacity <= 0 ? 100 : capacity)
        {
            FullMode = BoundedChannelFullMode.Wait
        };
        _queue = Channel.CreateBounded<T>(options);
    }

    /// <summary>
    /// Tapşırığı növbəyə əlavə edir.
    /// </summary>
    /// <param name="item">Növbəyə əlavə ediləcək tapşırıq.</param>
    /// <returns>Tapşırığın növbəyə əlavə edilməsi üçün asinxron əməliyyat.</returns>
    public async ValueTask QueueAsync(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        await _queue.Writer.WriteAsync(item);
    }

    /// <summary>
    /// Növbədən tapşırıq götürür.
    /// </summary>
    /// <param name="cancellationToken">Əməliyyatı ləğv etmək üçün istifadə edilən token.</param>
    /// <returns>Növbədən götürülən tapşırıq.</returns>
    public async ValueTask<T> DequeueAsync(CancellationToken cancellationToken)
    {
        var item = await _queue.Reader.ReadAsync(cancellationToken);
        return item;
    }
}
