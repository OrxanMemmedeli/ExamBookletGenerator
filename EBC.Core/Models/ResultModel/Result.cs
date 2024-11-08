using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

/// <summary>
/// Generic Result sinifi, əməliyyatların nəticələrini idarə etmək üçün istifadə olunur.
/// Bu sinif uğurlu və ya uğursuz nəticələri təmsil edir, həmçinin uğursuzluq hallarında
/// istisna və ya xüsusi xəta mesajları saxlamağa imkan verir.
/// </summary>
/// <typeparam name="T">Uğurlu nəticə üçün qaytarılacaq dəyərin tipi.</typeparam>
public class Result<T>
{
    /// <summary>
    /// Əməliyyatın nəticəsi uğurlu olduğu halda dəyəri saxlayır.
    /// Əgər nəticə uğursuzdursa, bu dəyər <c>null</c> olacaq.
    /// </summary>
    public T? Value { get; private set; }

    /// <summary>
    /// Uğursuz nəticə halında baş verən istisna məlumatını saxlayır.
    /// </summary>
    public Exception? Exception { get; private set; }

    /// <summary>
    /// Nəticənin uğurlu olub-olmamasını təyin edir.
    /// </summary>
    public bool IsSuccess { get; private set; }

    /// <summary>
    /// Uğursuz nəticə halında baş verən xüsusi xəta mesajlarını saxlayır.
    /// Əgər nəticə uğurludursa, bu dəyər boş bir siyahı olacaq.
    /// </summary>
    public IEnumerable<string> FailureMessages { get; private set; } = Array.Empty<string>();

    /// <summary>
    /// Uğursuz nəticə halında istisna və ya xəta mesajlarının birləşdirilmiş halını qaytarır.
    /// </summary>
    public string ExceptionMessage => Exception != null
        ? Exception.Message
        : FailureMessages.Any()
            ? string.Join(Environment.NewLine, FailureMessages)
            : string.Empty;

    /// <summary>
    /// Uğurlu nəticə üçün konstruktor.
    /// </summary>
    /// <param name="value">Uğurlu nəticə dəyəri. Əgər <paramref name="value"/> <c>null</c> dəyərinə malikdirsə,
    /// bu zaman <see cref="Activator.CreateInstance"/> vasitəsilə boş bir obyekt olaraq təyin edilir.</param>
    protected Result(T? value)
    {
        IsSuccess = true;
        Value = value ?? (T)Activator.CreateInstance(typeof(T))!;
    }

    /// <summary>
    /// İstisna əsasında uğursuz nəticə üçün konstruktor.
    /// </summary>
    /// <param name="exception">Baş verən istisna. Bu parametr <c>null</c> ola bilməz.</param>
    protected Result(Exception exception)
    {
        Value = default;
        IsSuccess = false;
        FailureMessages = ExtractErrorMessages(exception);
        Exception = exception ?? throw new ArgumentNullException(nameof(exception), "Exception cannot be null.");
    }

    /// <summary>
    /// Xəta mesajları əsasında uğursuz nəticə üçün konstruktor.
    /// </summary>
    /// <param name="failureMessages">Baş verən xətanın mesajları. Bu parametr <c>null</c> ola bilməz.</param>
    protected Result(IEnumerable<string> failureMessages)
    {
        Value = default;
        IsSuccess = false;
        FailureMessages = failureMessages ?? throw new ArgumentNullException(nameof(failureMessages), "Failure messages cannot be null.");
        Exception = null;
    }

    /// <summary>
    /// Uğurlu nəticə yaratmaq üçün istifadə olunur.
    /// </summary>
    /// <param name="value">Uğurlu nəticənin dəyəri. Dəyər göstərilməzsə, <c>null</c> və ya boş obyekt olaraq qəbul edilir.</param>
    /// <returns>Uğurlu nəticə olan <see cref="Result{T}"/> obyekti.</returns>
    public static Result<T> Success(T? value = default) => new(value);

    /// <summary>
    /// İstisna əsasında uğursuz nəticə yaratmaq üçün istifadə olunur.
    /// </summary>
    /// <param name="exception">Baş verən istisna. Bu parametr <c>null</c> ola bilməz.</param>
    /// <returns>İstisna ilə uğursuz nəticə olan <see cref="Result{T}"/> obyekti.</returns>
    public static Result<T> Failure(Exception exception) => new(exception);

    /// <summary>
    /// Xəta mesajları əsasında uğursuz nəticə yaratmaq üçün istifadə olunur.
    /// </summary>
    /// <param name="failureMessages">Baş verən xətanın mesajları. Bu parametr <c>null</c> və ya boş ola bilməz.</param>
    /// <returns>Xəta mesajları ilə uğursuz nəticə olan <see cref="Result{T}"/> obyekti.</returns>
    public static Result<T> Failure(params string[] failureMessages)
    {
        Contract.Requires(failureMessages != null && failureMessages.Any());
        return new Result<T>(failureMessages);
    }

    /// <summary>
    /// İstisna ağacındakı bütün mesajları çıxaran yardımçı metod.
    /// </summary>
    /// <param name="exception">Baş verən istisna.</param>
    /// <returns>İstisna ağacında olan bütün mesajların siyahısı.</returns>
    private static IEnumerable<string> ExtractErrorMessages(Exception exception)
    {
        var errorMessages = new List<string>();
        while (exception != null)
        {
            errorMessages.Add(exception.Message);
            exception = exception.InnerException;
        }
        return errorMessages;
    }

    /// <summary>
    /// Nəticəni işləmək üçün uğurlu və ya uğursuz nəticəyə əsasən uyğun metod çağırır.
    /// </summary>
    /// <typeparam name="TResult">Nəticənin qaytarılma tipi.</typeparam>
    /// <param name="onSuccess">Uğurlu nəticə üçün işləyəcək metod.</param>
    /// <param name="onError">Səhv nəticə üçün işləyəcək metod.</param>
    /// <returns>İşlənmiş nəticə.</returns>
    public TResult Match<TResult>(Func<T?, TResult> onSuccess, Func<Exception, TResult> onError)
        => IsSuccess ? onSuccess(Value) : onError(Exception!);
}


public static class ResultExtentions
{
    /// <summary>
    /// Uğurlu və ya uğursuz nəticəni işləmək üçün genişləndirmə metodu.
    /// </summary>
    /// <typeparam name="T">Nəticənin dəyərinin tipi.</typeparam>
    /// <typeparam name="TResult">Geri qaytarılacaq nəticənin tipi.</typeparam>
    /// <param name="result">İşlənəcək nəticə.</param>
    /// <param name="onSuccess">Uğurlu nəticə üçün çağırılacaq funksiya.</param>
    /// <param name="onError">Uğursuz nəticə üçün çağırılacaq funksiya.</param>
    /// <returns>İşlənmiş nəticə.</returns>
    public static TResult Match<T, TResult>(this Result<T> result, Func<T?, TResult> onSuccess, Func<Exception, TResult> onError)
    {
        ArgumentNullException.ThrowIfNull(result);
        return result.IsSuccess ? onSuccess(result.Value) : onError(result.Exception!);
    }
}