namespace EBC.Core.Constants;

public static class ExceptionMessage
{
    public const string ErrorOccured = "Əməliyat zamanı xəta baş verdi!";
    public const string ErrorOcurredByEcription = "Şifrələmə və ya deşifrələmə əməliyatı zamanı xəta baş verdi!";
    public const string NotFound = "Məlumat tapılmadı";
    public const string IsNull = "Məlumat boşdur";
    public const string AnErrorWhenSave = "Məlumat yadda saxlanılmadı";
    public const string NotSupported = "{0} action is not supported for {1}";
    public const string UniqueUser = "İstifadəçi adı mövcuddur";
    public const string HaveDebt = "'{0}' müəssisəsinin '{1}' ₼ borcu var. Borc bağlanmadan abonelik dayandırıla bilməz";
    public const string InvalidToken = "Cari xidmətdən istifafə etmə səlahiyətinə malik deyilsiniz.";
    public const string FreeCompany = "Hörmətli '{0}', sizin abonəliyiniz ödənişsizdir və girişiniz təsdiqlənmişdir.";
    public const string ActiveCompany = "Hörmətli '{0}', sizin abonəliyiniz üçün olan ödəniş və borc məlumatları təqdim edilmişdir. Borc limitiniz bitdiyi anda sistemə girişiniz məhdudlaşdırılacaqdır. G";
    public const string DeActiveCompany = "Hörmətli '{0}', abonəliyiniz üçün olan ödəniş ({1}) və borc ({2}) məlumatları təqdim edilmişdir. Borc günlük ({3}) olaraq hesablanır və tam ödənən günə dək artacaqdır. Borcu tam ödənilmədən sistemə girişiniz təmin edilə bilməz. Bloklanma tarixi: {4}";
    public const string StopSubscriptionCompany = "Hörmətli '{0}', sizin abonəliyiniz dondurulmuşdur. Bu səbəbdən sistemən istifadəniz məhdudlaşdırılıb.";

}
