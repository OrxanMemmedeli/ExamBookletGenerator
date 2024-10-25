using EBC.Core.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Net.Http;

namespace EBC.Core.Services.Concrete;

/// <summary>
/// Təsdiqləmə URL-ləri yaratmaq və yoxlamaq üçün istifadə edilən xidmət.
/// </summary>
public class ConfirmUrlService : IUrlService
{
    private readonly IUrlHelperFactory _urlHelperFactory;
    private readonly HttpContext _httpContext;
    private const string EncryptionKey = "B256587896C19577E2D81392B523B";


    /// <summary>
    /// ConfirmUrlService konstruktoru.
    /// </summary>
    /// <param name="urlHelperFactory">URL yaratmaq üçün tələb olunan URL helper fabriki.</param>
    /// <param name="httpContextAccessor">Cari HTTP kontekstini əldə etmək üçün tələb olunan HttpContextAccessor.</param>
    public ConfirmUrlService(IUrlHelperFactory urlHelperFactory, IHttpContextAccessor httpContextAccessor)
    {
        _urlHelperFactory = urlHelperFactory;
        _httpContext = httpContextAccessor.HttpContext;
    }


    /// <summary>
    /// Şifrələnmiş təsdiqləmə URL-i yaradır.
    /// </summary>
    /// <param name="companyId">Təsdiqləmə URL-ində istifadə olunacaq şirkət ID-si.</param>
    /// <returns>Yaradılmış təsdiqləmə URL-i (string formatında).</returns>
    public string GenerateUrl(Guid companyId)
    {
        string encodedCompanyId = EncryptionService.Encrypt(companyId.ToString(), EncryptionKey);
        return $"{_httpContext.Request.Scheme}://{_httpContext.Request.Host}/confirm/c/{encodedCompanyId}";
    }


    /// <summary>
    /// Təsdiqləmə URL-ini açaraq daxilindəki `companyId` dəyərini geri qaytarır.
    /// </summary>
    /// <param name="url">Açılacaq təsdiqləmə URL-i.</param>
    /// <returns>URL-dən çıxarılan şirkət ID-si (`Guid` formatında) və ya uğursuzluq halında `Guid.Empty`.</returns>
    public Guid ConfirmUrl(string url)
    {
        string decodedResult = EncryptionService.Decrypt(url, EncryptionKey);
        return Guid.TryParse(decodedResult, out var companyId) ? companyId : Guid.Empty;
    }
}