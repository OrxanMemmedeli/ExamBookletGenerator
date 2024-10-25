namespace EBC.Core.Services.Abstract;


/// <summary>
/// Təsdiqləmə URL-lərinin yaradılması və yoxlanılması üçün interfeys.
/// </summary>
public interface IUrlService
{
    /// <summary>
    /// Şifrələnmiş təsdiqləmə URL-i yaradır.
    /// </summary>
    /// <param name="companyId">Təsdiqləmə URL-ində istifadə olunacaq şirkət ID-si.</param>
    /// <returns>Yaradılmış təsdiqləmə URL-i (string formatında).</returns>
    string GenerateUrl(Guid companyId);

    /// <summary>
    /// Təsdiqləmə URL-ini açaraq daxilindəki `companyId` dəyərini geri qaytarır.
    /// </summary>
    /// <param name="url">Açılacaq təsdiqləmə URL-i.</param>
    /// <returns>URL-dən çıxarılan şirkət ID-si (`Guid` formatında) və ya uğursuzluq halında `Guid.Empty`.</returns>
    Guid ConfirmUrl(string url);
}
