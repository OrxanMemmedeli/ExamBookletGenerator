
namespace EBC.Core.Constants;

public static class ValidationMessage
{
    public const string NotEmptyAndNotNullWithName = "{0} sahəsi boş ola bilməz";   
    public const string NotEmptyAndNotNull = "Bu sahə boş ola bilməz";
    public const string NotEqualToDefault = "Bu sahə varsayılan(default) dəyərdə olmamalıdır";
    public const string MinimumLengthWithName = "{0} sahəsi ən az {1} simvol ola bilər";
    public const string MinimumLength = "Bu sahə ən az {0} simvol ola bilər";
    public const string MaximumLengthWithName = "{0} sahəsi ən çox {1} simvol ola bilər";
    public const string MaximumLength = "Bu sahə ən çox {0} simvol ola bilər";
    public const string EmailAddress = "Email düzgün yazılmamışdır";
    public const string CapitalLetter = "Şifrədə ən azı 1 BÖYÜK simvol olmalıdır (nümunə Aa123!!)";
    public const string LowercaseLetter = "Şifrədə ən azı 1 KİÇİK simvol olmalıdır (nümunə Aa123!!)";
    public const string Number = "Şifrədə ən azı 1 RƏQƏM olmalıdır (nümunə Aa123!!)";
    public const string SpecialCharacter = "Şifrədə xüsusi simvollar olmalıdır (nümunə Aa123!!)";
    public const string EqualPassword = "Şifrə ilə təkrarı arasında uyğunsuzluq var";
    public const string NotEqual = "{0} sahələri eyni ola bilməz";
    public const string LessThanOrEqualTo = "{0} tarixin ən kiçik qiyməti {1} ola bilər";
    public const string InclusiveBetween = "qiymət aralığı {0} - {1} kimi olmalıdır";
    public const string WrongGuidFormat = "xətalı Guid formatı";
    public const string DateTimeMinValue = "Daxil edilmiş tarix cari tarixdən kiçik ola bilməz";
    public const string GreaterThanOrEqualTo = "Bu sahə {1} və ya daha böyük dəyər olmalıdır";
    public const string ContainsTextForTextTitle = "Mətn Başlığında mütləq formada \"{0} – {1}\" ifadəsi yer almalıdır. (Sual nömrələrini avtomatik vermək üçün)";

    public const string PaymentGreaterFromDebt = "Ödəniş məbləği borcdan az ola bilməz. {0} tarixi üçün minimal ödəniş məbləği {1} olmalıdır.";

}
