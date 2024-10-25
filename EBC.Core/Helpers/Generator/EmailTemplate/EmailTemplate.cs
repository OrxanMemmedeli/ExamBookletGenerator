namespace EBC.Core.Helpers.Generator.EmailTemplate;

/// <summary>
/// EmailTemplate sinfi e-poçtun əsas məlumatlarını və HTML məzmununu ehtiva edir.
/// </summary>
public class EmailTemplate
{
    private readonly string _title;
    private readonly string _header;
    private readonly string _companyName;
    private readonly string _bodyContent;
    private readonly string _applicationName;


    /// <summary>
    /// EmailTemplate konstruktoru, bütün lazımi parametrləri qəbul edir.
    /// </summary>
    /// <param name="title">E-poçtun başlığı.</param>
    /// <param name="header">E-poçtun başlıq məzmunu.</param>
    /// <param name="companyName">E-poçtun göndəriləcəyi şirkət adı.</param>
    /// <param name="bodyContent">E-poçtun əsas məzmunu.</param>
    /// <param name="applicationName">E-poçtun göndərənin adı (Susmaya görə Sistemin adı ola bilər).</param>
    public EmailTemplate(string title, string header, string companyName, string bodyContent, string applicationName)
    {
        _title = title;
        _header = header;
        _companyName = companyName;
        _bodyContent = bodyContent;
        _applicationName = applicationName;
    }

    /// <summary>
    /// E-poçtun HTML formatında məzmununu yaradır.
    /// </summary>
    /// <returns>HTML formatında e-poçt məzmunu.</returns>
    public string GetHtmlContent()
    {
        string template = @"
            <!DOCTYPE html>
            <html lang=""az"">
            <head>
                <meta charset=""UTF-8"">
                <title>{0}</title>
                <style>
                    body {{ font-family: Arial, sans-serif; }}
                    .container {{ max-width: 600px; margin: 20px auto; padding: 20px; background-color: #ffffff; border-radius: 10px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); }}
                    .highlight {{ color: #ff0000; font-weight: bold; }}
                </style>
            </head>
            <body>
                <div class=""container"">
                    <h1>{1}</h1>
                    <p>Hörmətli <span class=""highlight"">[{2}]</span>,</p>
                    <p>{3}</p>
                    <p>Hörmətlə,<br/>[{4}]</p>
                </div>
            </body>
            </html>";

        return string.Format(template, _title, _header, _companyName, _bodyContent, _applicationName);
    }
}
