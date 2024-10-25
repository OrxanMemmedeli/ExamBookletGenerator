using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using EBC.Core.Constants;


namespace EBC.Core.Services.EmailService;


/// <summary>
/// GmailService sinfi Gmail SMTP serveri vasitəsilə e-poçt göndərmə xidmətini təmin edir.
/// IEmailService interfeysini implement edir və IConfiguration-dan SMTP parametrlərini alır.
/// </summary>
public class GmailService : IEmailService
{
    private readonly string _fromName;
    private readonly string _fromAddress;
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _password;

    /// <summary>
    /// GmailService konstruktoru. SMTP server və digər e-poçt parametrlərini IConfiguration vasitəsilə oxuyur.
    /// </summary>
    /// <param name="configuration">E-poçt parametrlərini oxumaq üçün IConfiguration obyekti.</param>
    public GmailService(IConfiguration configuration)
    {
        _fromName = configuration.GetSection("EmailSettings")["FromName"] ?? GmailDefaultValues.FromName;
        _fromAddress = configuration.GetSection("EmailSettings")["FromAddress"] ?? GmailDefaultValues.FromAddress;
        _smtpServer = configuration.GetSection("EmailSettings")["SmtpServer"] ?? GmailDefaultValues.SmtpServer;
        _smtpPort = int.TryParse(configuration.GetSection("EmailSettings")["SmtpPort"], out var port) ? port : GmailDefaultValues.SmtpPort;
        _password = configuration.GetSection("EmailSettings")["Password"] ?? GmailDefaultValues.Password;
    }

    /// <summary>
    /// Verilən e-poçt məlumatları ilə e-poçt göndərir.
    /// </summary>
    /// <param name="to">Göndəriləcək e-poçt ünvanı.</param>
    /// <param name="subject">E-poçtun mövzusu.</param>
    /// <param name="body">E-poçtun məzmunu.</param>
    /// <param name="isHtml">E-poçt məzmununun HTML formatında olub olmadığını göstərir.</param>
    /// <returns>Asinxron əməliyyatın nəticəsi.</returns>
    public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = false)
    {
        var mimeMessage = new MimeMessage();

        mimeMessage.From.Add(new MailboxAddress(_fromName, _fromAddress));
        mimeMessage.To.Add(new MailboxAddress(to, to));
        mimeMessage.Subject = subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = isHtml ? body : null,
            TextBody = !isHtml ? body : null
        };

        mimeMessage.Body = bodyBuilder.ToMessageBody();

        using var smtpClient = new SmtpClient();

        await smtpClient.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
        await smtpClient.AuthenticateAsync(_fromAddress, _password);
        await smtpClient.SendAsync(mimeMessage);
        await smtpClient.DisconnectAsync(true);
    }
}
