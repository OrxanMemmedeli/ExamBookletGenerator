using System.Net;

namespace EBC.Core.Models.Responses;

public class PaginateResponse<T>
{
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    public bool IsSucceeded { get; set; } = true;
    public string Message { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int DataCount { get; set; }
    public int PageCount => (int)Math.Ceiling((double)DataCount / PageSize);
    public bool HasPrevious => PageNumber > 1;
    public bool HasNext => PageNumber < PageCount;
    public IEnumerable<T> Data { get; set; } = new List<T>();

    // DataCount və səhifələnmiş məlumatlarla əsas konstruktor
    public PaginateResponse(IEnumerable<T> data, int pageNumber, int pageSize, int dataCount)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        DataCount = dataCount;
        Data = data;
    }

    // Boş konstruktor
    public PaginateResponse() { }
}
