namespace EBC.Core.Models.Responses;

public class PaginateResult<T> : Result<IEnumerable<T>>
{
    public int PageNumber { get; }
    public int PageSize { get; }
    public int DataCount { get; }
    public int PageCount => (int)Math.Ceiling((double)DataCount / PageSize);
    public bool HasPrevious => PageNumber > 1;
    public bool HasNext => PageNumber < PageCount;

    protected PaginateResult(IEnumerable<T> value, int pageNumber, int pageSize, int dataCount)
        : base(value)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        DataCount = dataCount;
    }

    protected PaginateResult(Exception exception)
        : base(exception) { }

    public static PaginateResult<T> Success(IEnumerable<T> value, int pageNumber, int pageSize, int dataCount) =>
        new(value, pageNumber, pageSize, dataCount);

    public static new PaginateResult<T> Failure(Exception exception) => new(exception);
}
