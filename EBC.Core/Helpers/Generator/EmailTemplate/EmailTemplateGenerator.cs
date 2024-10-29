using EBC.Core.Constants;
using EBC.Core.Models.Enums;
using System.Text;

namespace EBC.Core.Helpers.Generator.EmailTemplate;

/// <summary>
/// EmailTemplateGenerator sinfi müxtəlif e-poçt şablonlarını yaratmaq üçün istifadə edilir.
/// </summary>
public class EmailTemplateGenerator
{
    private readonly Dictionary<EmailTemplateType, string> _emailTitlesAndHeaders;
    private readonly string _applicationName;

    /// <summary>
    /// EmailTemplateGenerator konstruktoru. Müxtəlif e-poçt başlıqları və şablonları ilə lüğəti doldurur.
    /// </summary>
    /// <param name="applicationName">E-poçtların sonunda göstəriləcək tətbiq adı.</param>
    public EmailTemplateGenerator(string applicationName)
    {
        _applicationName = applicationName;
        _emailTitlesAndHeaders = new Dictionary<EmailTemplateType, string>
        {
            { EmailTemplateType.Debt, "Borc Bildirişi" },
            { EmailTemplateType.Payment, "Ödəniş Bildirişi" },
            { EmailTemplateType.Welcome, "Xoş gəldin" },
            { EmailTemplateType.ComeBack, "Yenidən xoş gəldin" },
            { EmailTemplateType.Blocked, "Hesabınız bloklanmışdır" },
            { EmailTemplateType.StopSubscription, "Abonəliyiniz dayandırıldı" },
            { EmailTemplateType.Confirm, "Hesabın təsdiq edilməsi" }
        };
    }

    /// <summary>
    /// Borc bildirişi e-poçt şablonunu yaradır.
    /// </summary>
    public EmailTemplate GenerateDebtEmail(string companyName, decimal? totalPayment, decimal? totalDebt, decimal? currentDebt)
    {
        var body = new StringBuilder()
            .Append($"Bu elekron məktub aylıq olaraq göndərilir və borcunuzun miqdarını xatırlatmaq üçündür. ")
            .Append($"Sizin ümumi borcunuz <span class=\"highlight\">[{currentDebt}]</span> manatdır. ")
            .Append($"<br/>İndiyə dək olan ümumi ödəniş miqdarı {totalPayment}")
            .Append($"<br/>Ümumi borc miqdarı {totalDebt}")
            .ToString();

        return CreateEmailTemplate(EmailTemplateType.Debt, companyName, body);
    }

    /// <summary>
    /// Ödəniş bildirişi e-poçt şablonunu yaradır.
    /// </summary>
    public EmailTemplate GeneratePaymentEmail(string companyName, decimal amount)
    {
        var body = new StringBuilder()
            .Append($"Ödənişiniz qeydə alınmışdır. Ödəniş məbləği <span class=\"highlight\">[{amount}]</span> manatdır.")
            .ToString();

        return CreateEmailTemplate(EmailTemplateType.Payment, companyName, body);
    }

    /// <summary>
    /// Yeni istifadəçi üçün xoş gəldin e-poçt şablonunu yaradır.
    /// </summary>
    public EmailTemplate GenerateWelcomeEmail(string companyName, decimal dailyAmount, decimal debtLimit, decimal percentOfFine)
    {
        var body = new StringBuilder()
            .Append($"Yeni əməkdaşlıq üçün çox məmnunuq. Günlük məbləğ: {dailyAmount} azn<br/>")
            .Append($"Borc limiti: {debtLimit} azn<br/>")
            .Append($"Gecikmə faizi: {percentOfFine}%<br/><i>")
            .Append("Ödənişin gecikməsinə görə sistem tərəfindən günlük ödəniş hesablanır və bu proses əməkdaşlıq dayandırılana kimi davam edir.</i>")
            .ToString();

        return CreateEmailTemplate(EmailTemplateType.Welcome, companyName, body);
    }

    /// <summary>
    /// Geri qayıdan istifadəçi üçün xoş gəldin e-poçt şablonunu yaradır.
    /// </summary>
    public EmailTemplate GenerateComeBackEmail(string companyName, decimal dailyAmount, decimal debtLimit, decimal percentOfFine)
    {
        var body = new StringBuilder()
            .Append($"Yenidən geri döndüyünüz üçün çox minnətdarıq. Günlük məbləğ: {dailyAmount} azn<br/>")
            .Append($"Borc limiti: {debtLimit} azn<br/>")
            .Append($"Gecikmə faizi: {percentOfFine}%<br/><i>")
            .Append("Ödənişin gecikməsinə görə günlük ödəniş hesablanır və bu proses əməkdaşlıq dayandırılana kimi davam edir.</i>")
            .ToString();

        return CreateEmailTemplate(EmailTemplateType.ComeBack, companyName, body);
    }

    /// <summary>
    /// Bloklanmış hesab üçün e-poçt şablonu yaradır.
    /// </summary>
    public EmailTemplate GenerateBlockedEmail(string companyName, decimal? totalPayment, decimal? totalDebt, decimal? currentDebt)
    {
        var body = new StringBuilder()
            .Append("Sizin hesabınız təyin edilmiş limitini keçdiyi üçün sistemdən istifadəniz məhdudlaşdırılmışdır.<br/>")
            .Append($"Edilən ödəniş miqdarı: {totalPayment} azn<br/>")
            .Append($"Ümumi yaranmış borc: {totalDebt} azn<br/>")
            .Append($"Aktiv borc miqdarı: {currentDebt} azn")
            .ToString();

        return CreateEmailTemplate(EmailTemplateType.Blocked, companyName, body);
    }

    /// <summary>
    /// Abunəliyin dayandırılması üçün e-poçt şablonunu yaradır.
    /// </summary>
    public EmailTemplate GenerateStopSubscriptionEmail(string companyName)
    {
        var body = "Tələbinizə uyğun olaraq sizin ilə olan əməkdaşlıq dondurulmuşdur.";
        return CreateEmailTemplate(EmailTemplateType.StopSubscription, companyName, body);
    }

    /// <summary>
    /// Hesabın təsdiq edilməsi üçün e-poçt şablonunu yaradır.
    /// </summary>
    public EmailTemplate GenerateConfirmEmail(string companyName, string confirmUrl)
    {
        var body = new StringBuilder()
            .Append("Hesabınızı doğrulamaq üçün aşağıdakı linkə klikləyin:<br><br>")
            .Append(confirmUrl)
            .Append("<br><br>")
            .ToString();

        return CreateEmailTemplate(EmailTemplateType.Confirm, companyName, body);
    }

    /// <summary>
    /// Verilən məlumatlara əsasən e-poçt şablonu yaradır.
    /// </summary>
    private EmailTemplate CreateEmailTemplate(EmailTemplateType templateType, string companyName, string bodyContent)
    {
        var titleAndHeader = _emailTitlesAndHeaders[templateType];
        return new EmailTemplate(titleAndHeader, titleAndHeader, companyName, bodyContent, _applicationName);
    }
}