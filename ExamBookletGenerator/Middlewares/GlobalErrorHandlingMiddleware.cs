using EBC.Core.Caching.Abstract;
using EBC.Core.Exceptions;
using EBC.Data.Entities;
using EBC.Data.Repositories.Abstract;
using Microsoft.Extensions.Caching.Memory;

namespace ExamBookletGenerator.Middlewares;

public class GlobalErrorHandlingMiddleware : IMiddleware
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ICachingService<IMemoryCache> _cachingService;
    private readonly bool _isDevelopment;

    public GlobalErrorHandlingMiddleware(
        IServiceProvider serviceProvider,
        ICachingService<IMemoryCache> cachingService,
        bool isDevelopment)
    {
        _serviceProvider = serviceProvider;
        _cachingService = cachingService;
        _isDevelopment = isDevelopment;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            if (_isDevelopment)
                throw ex; // Development mühitində exception olduğu kimi atılır.

            var errorCacheKey = $"{ex.GetType().FullName}-{ex.Message}"; // Hər bir error üçün unikal açar
            var error = _cachingService.ReadFromCache<SysException>(errorCacheKey);

            if (error == null)
            {
                using var scope = _serviceProvider.CreateScope();
                var _sysExceptionRepository = scope.ServiceProvider.GetRequiredService<ISysExceptionRepository>();

                error = new SysException
                {
                    Id = Guid.NewGuid(),
                    Exception = ex.ToString(),
                    Message = ex.Message,
                    RequestPath = context.Request.Path,
                    StackTrace = ex.StackTrace,
                    UserName = context.User.Identity?.Name,
                    UserIP = context.Connection.RemoteIpAddress?.ToString(),
                    StatusCode = ex switch
                    {
                        NotFoundException => StatusCodes.Status404NotFound,
                        BadRequestException => StatusCodes.Status400BadRequest,
                        UnhadleException => StatusCodes.Status500InternalServerError,
                        _ => StatusCodes.Status500InternalServerError
                    }
                };

                await _sysExceptionRepository.AddAsync(error);
                _cachingService.WriteToCache(errorCacheKey, error, TimeSpan.FromHours(12));
            }

            context.Response.Redirect($"/Home/Error?StatusCode={error.StatusCode}");
        }
    }
}
