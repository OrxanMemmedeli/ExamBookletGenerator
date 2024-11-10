using EBC.Core.Models.ResultModel;

namespace EBC.Core.Models.Responses;

public class PaginateResult<T> : Result<IEnumerable<T>>
{
    /// <summary>
    /// Hal-hazırki səhifə nömrəsi.
    /// </summary>
    public int PageNumber { get; private set; }

    /// <summary>
    /// Səhifədəki elementlərin sayı.
    /// </summary>
    public int PageSize { get; private set; }

    /// <summary>
    /// Məlumatların ümumi sayı.
    /// </summary>
    public int DataCount { get; private set; }

    /// <summary>
    /// Ümumi səhifə sayı.
    /// </summary>
    public int PageCount => (int)Math.Ceiling((double)DataCount / PageSize);

    /// <summary>
    /// Əvvəlki səhifənin mövcud olub-olmadığını göstərir.
    /// </summary>
    public bool HasPrevious => PageNumber > 1;

    /// <summary>
    /// Növbəti səhifənin mövcud olub-olmadığını göstərir.
    /// </summary>
    public bool HasNext => PageNumber < PageCount;

    private PaginateResult(IEnumerable<T> value, int pageNumber, int pageSize, int dataCount)
        : base(value)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        DataCount = dataCount;
    }

    private PaginateResult(Exception exception) : base(exception) { }

    /// <summary>
    /// Uğurlu səhifələnmiş nəticə yaratmaq üçün metod.
    /// </summary>
    public static PaginateResult<T> Success(IEnumerable<T> value, int pageNumber, int pageSize, int dataCount) =>
        new PaginateResult<T>(value, pageNumber, pageSize, dataCount);

    /// <summary>
    /// İstisna ilə uğursuz nəticə yaratmaq üçün metod.
    /// </summary>
    public static new PaginateResult<T> Failure(Exception exception) =>
        new PaginateResult<T>(exception);

    /// <summary>
    /// Xəta mesajları ilə uğursuz nəticə yaratmaq üçün metod.
    /// </summary>
    public static new PaginateResult<T> Failure(params string[] failureMessages)
    {
        var result = new PaginateResult<T>(new Exception("Pagination Error"));
        Failure(result, failureMessages);
        return result;
    }
}

