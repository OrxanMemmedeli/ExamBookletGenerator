using EBC.Core.Constants;
using System.Diagnostics.Contracts;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EBC.Core.Models.ResultModel;


public class Result
{
    // Failure constructor with exception
    protected Result(Exception exception)
    {
        Exception = exception;
        IsSuccess = false;
    }

    protected Result(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }

    protected Result() {
        IsSuccess = true;

    }

    /// <summary>
    /// Nəticənin uğurlu olub-olmamasını göstərir.
    /// </summary>
    public bool IsSuccess { get; protected set; }

    /// <summary>
    /// Uğursuz nəticə halında yaranan xəta mesajlarını saxlayır.
    /// </summary>
    public IEnumerable<string> FailureMessages { get; protected set; } = Array.Empty<string>();

    /// <summary>
    /// İstisna obyekti ilə uğursuz nəticə.
    /// </summary>
    public Exception? Exception { get; protected set; }

    /// <summary>
    /// İstisna və ya xəta mesajlarını tək bir mesaj kimi qaytarır.
    /// </summary>
    public string ExceptionMessage => Exception != null
        ? Exception.Message
        : FailureMessages.Any()
            ? string.Join(Environment.NewLine, FailureMessages)
            : string.Empty;

    /// <summary>
    /// Uğurlu nəticə yaratmaq üçün istifadə olunan metod.
    /// </summary>
    public static Result Success()
    {
        var result = new Result();
        Succeed(result);
        return result;
    }

    /// <summary>
    /// Xəta mesajları ilə uğursuz nəticə yaratmaq üçün metod.
    /// </summary>
    public static Result Failure(params string[] failureMessages)
    {
        var result = new Result();
        Failure(result, failureMessages);
        return result;
    }

    /// <summary>
    /// İstisna obyekti ilə uğursuz nəticə yaratmaq üçün metod.
    /// </summary>
    public static Result Failure(Exception exception)
    {
        var result = new Result();
        Failure(result, exception);
        return result;
    }

    protected static void Succeed(Result result)
    {
        result.IsSuccess = true;
        result.FailureMessages = Array.Empty<string>();
    }

    protected static void Failure(Result result, params string[] failureMessages)
    {
        Contract.Requires(failureMessages != null && failureMessages.Any());
        result.IsSuccess = false;
        result.FailureMessages = failureMessages;
    }

    protected static void Failure(Result result, Exception exception)
    {
        Contract.Requires(exception != null);
        result.IsSuccess = false;
        result.Exception = exception;

        var errorMessages = new List<string>();
        while (exception != null)
        {
            errorMessages.Add(exception.Message);
            exception = exception.InnerException;
        }
        result.FailureMessages = errorMessages;
    }

    public static Result ExecuteWithHandling(Func<Result> func)
    {
        try
        {
            var result = func();
            return result.IsSuccess ? Success() : result;
        }
        catch (ApplicationException aex)
        {
            return Failure(aex);
        }
        catch (Exception ex)
        {
            return Failure(EBC.Core.Constants.ExceptionMessage.ErrorOccured);
        }
    }

    public static async Task<Result> ExecuteWithHandlingAsync(Func<Task<Result>> func)
    {
        try
        {
            var result = await func();
            return result.IsSuccess ? Success() : result;
        }
        catch (ApplicationException aex)
        {
            return Failure(aex);
        }
        catch (Exception ex)
        {
            return Failure(EBC.Core.Constants.ExceptionMessage.ErrorOccured);
        }
    }

}


/// <summary>
/// Generic Result sinifi, əməliyyatların nəticələrini idarə etmək üçün istifadə olunur.
/// Bu sinif uğurlu və ya uğursuz nəticələri təmsil edir, həmçinin uğursuzluq hallarında
/// istisna və ya xüsusi xəta mesajları saxlamağa imkan verir.
/// </summary>
/// <typeparam name="T">Uğurlu nəticə üçün qaytarılacaq dəyərin tipi.</typeparam>
public class Result<T> : Result
{
    public Result() :base() { }

    // Success constructor
    protected Result(T data) : base (true)
    {
        Data = data;
    }

    // Failure constructor with exception
    protected Result(Exception exception) : base(exception) { }


    /// <summary>
    /// Uğurlu nəticə halında qaytarılan məlumat.
    /// </summary>
    public T? Data { get; private set; }

    /// <summary>
    /// Xəta mesajları ilə generik uğursuz nəticə yaratmaq üçün metod.
    /// </summary>
    public new static Result<T> Failure(params string[] failureMessages)
    {
        var result = new Result<T>();
        Failure(result, failureMessages);
        return result;
    }

    /// <summary>
    /// İstisna obyekti ilə generik uğursuz nəticə yaratmaq üçün metod.
    /// </summary>
    public new static Result<T> Failure(Exception exception)
    {
        var result = new Result<T>();
        Failure(result, exception);
        return result;
    }

    /// <summary>
    /// Uğurlu nəticə yaratmaq üçün məlumat ilə birlikdə istifadə olunur.
    /// </summary>
    public static Result<T> Success(T data)
    {
        var result = new Result<T> { Data = data };
        Succeed(result);
        return result;
    }

    public static Result<T> ExecuteWithHandling<T>(Func<Result<T>> func)
    {
        try
        {
            var result = func();
            return result.IsSuccess ? Result<T>.Success(result.Data) : result;
        }
        catch (ApplicationException aex)
        {
            return Result<T>.Failure(aex);
        }
        catch (Exception ex)
        {
            return Result<T>.Failure(EBC.Core.Constants.ExceptionMessage.ErrorOccured);
        }
    }

    public static async Task<Result<T>> ExecuteWithHandlingAsync<T>(Func<Task<Result<T>>> func)
    {
        try
        {
            var result = await func();
            return result.IsSuccess ? Result<T>.Success(result.Data) : result;
        }
        catch (ApplicationException aex)
        {
            return Result<T>.Failure(aex);
        }
        catch (Exception ex)
        {
            return Result<T>.Failure(EBC.Core.Constants.ExceptionMessage.ErrorOccured);
        }
    }
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
        return result.IsSuccess ? onSuccess(result.Data) : onError(result.Exception!);
    }
}