namespace EBC.Core.Constants;

public static class ResponseMessages
{
    public static string OperationCompletedSuccessfully { get; private set; } = "Əməliyyat uğurla başa çatdı";
    public static string AnErrorOccurred { get; private set; } = "Əməliyyat zamanı xəta baş verdi";
    public static string ErrorOnSave { get; private set; } = "Məlumat yadda saxlanılmadı";
    public static string DataNotFound { get; private set; } = "Məlumat tapılmadı";
    public static string DataIsNull { get; private set; } = "Məlumat boşdur";
    public static string NotSupportedOperation { get; private set; } = "{0} əməliyyatı {1} üçün dəstəklənmir";
    public static string DuplicateUser { get; private set; } = "İstifadəçi adı artıq mövcuddur";
    public static string OutstandingDebt { get; private set; } = "'{0}' müəssisəsinin {1} ₼ borcu var. Borc bağlanmadan abonəlik dayandırıla bilməz";
    public static string InvalidAuthorizationToken { get; private set; } = "Cari xidmətdən istifadə etmək səlahiyyətiniz yoxdur.";
    public static string FreeSubscription { get; private set; } = "Hörmətli '{0}', abonəliyiniz ödənişsizdir və girişiniz təsdiqlənmişdir.";
    public static string ActiveSubscriptionNotice { get; private set; } = "Hörmətli '{0}', abonəliyinizin ödəniş və borc məlumatları təqdim edilmişdir. Borc limiti keçildikdə sistemə giriş məhdudlaşdırılacaq.";
    public static string DeactivatedSubscriptionNotice { get; private set; } = "Hörmətli '{0}', abonəliyinizin ödəniş ({1}) və borc ({2}) məlumatları təqdim edilmişdir. Borc gündəlik olaraq artacaq. Borc tam ödənilmədən sistemə giriş mümkün deyil. Bloklanma tarixi: {3}";
    public static string SubscriptionSuspended { get; private set; } = "Hörmətli '{0}', abonəliyiniz dondurulub və sistemdən istifadəniz məhdudlaşdırılıb.";
}

