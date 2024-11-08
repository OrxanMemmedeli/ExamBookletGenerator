namespace EBC.Core.Services.BackgroundServices;

/// <summary>
/// Asinxron olaraq işləyən tapşırıq (queue) strukturunu təyin edən interfeys.
/// </summary>
/// <typeparam name="T">Tapşırıq növü.</typeparam>
public interface IBackgroundTaskQueue<T>
{
    /// <summary>
    /// Tapşırığı növbəyə əlavə edir.
    /// </summary>
    /// <param name="item">Növbəyə əlavə ediləcək tapşırıq.</param>
    /// <returns>Tapşırığın növbəyə əlavə edilməsi üçün asinxron əməliyyat.</returns>
    ValueTask QueueAsync(T item);

    /// <summary>
    /// Növbədən tapşırıq götürür.
    /// </summary>
    /// <param name="cancellationToken">Əməliyyatı ləğv etmək üçün istifadə edilən token.</param>
    /// <returns>Növbədən götürülən tapşırıq.</returns>
    ValueTask<T> DequeueAsync(CancellationToken cancellationToken);
}
