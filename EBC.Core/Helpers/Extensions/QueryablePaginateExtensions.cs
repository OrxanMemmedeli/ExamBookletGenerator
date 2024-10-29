using EBC.Core.Models.Responses;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace EBC.Core.Helpers.Extensions;

public static class QueryablePaginateExtensions
{
    public static async Task<PaginateResponse<T>> ToPaginateAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize)
    {
        //minimum deyerin 1 olmasini temin edir
        pageNumber = Math.Max(pageNumber, 1);
        pageSize = Math.Max(pageSize, 10);

        int dataCount = await source.CountAsync().ConfigureAwait(false);
        List<T> data = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync().ConfigureAwait(false);

        return new PaginateResponse<T>(data, pageNumber, pageSize, dataCount)
        {
            StatusCode = HttpStatusCode.OK,
            IsSucceeded = true,
            Message = "Əməliyyat uğurla başa çatdı"
        };
    }

    public static PaginateResponse<T> ToPaginate<T>(this IQueryable<T> source, int pageNumber, int pageSize)
    {
        pageNumber = Math.Max(pageNumber, 1);
        pageSize = Math.Max(pageSize, 10);

        int dataCount = source.Count();
        List<T> data = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return new PaginateResponse<T>(data, pageNumber, pageSize, dataCount)
        {
            StatusCode = HttpStatusCode.OK,
            IsSucceeded = true,
            Message = "Əməliyyat uğurla başa çatdı"
        };
    }
}

