using EBC.Core.Models.ResultModel;

namespace EBC.Core.Services.EmailService;

/// <summary>
/// IEmailService interfeysi e-poçt göndərmə xidmətini təmin edir.
/// Müxtəlif e-poçt providerləri üçün ümumi funksionallığı təmin edir.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// E-poçt göndərmə funksiyası.
    /// </summary>
    /// <param name="to">Göndəriləcək e-poçt ünvanı (alan şəxs).</param>
    /// <param name="subject">E-poçt mövzusu.</param>
    /// <param name="body">E-poçt məzmunu (gövdəsi).</param>
    /// <param name="isHtml">E-poçt məzmununun HTML olub olmadığını göstərir.</param>
    /// <returns>Asinxron əməliyyatın nəticəsi.</returns>
   Task<Result> SendEmailAsync(string to, string subject, string body, bool isHtml = false);
}
