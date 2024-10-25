using System.Text;

namespace EBC.Core.Helpers.Generator;

/// <summary>
/// KeyGenerator sinfi təsadüfi simvollardan ibarət açar (şifrə) yaratmaq üçün istifadə edilir.
/// </summary>
public static class KeyGenerator
{
    /// <summary>
    /// Təsadüfi simvollardan ibarət açar yaradır.
    /// </summary>
    /// <param name="length">Yaradılacaq açarın uzunluğu. Default olaraq 10-dur.</param>
    /// <param name="addSymbols">Əgər true olaraq təyin edilsə, yaradılan açarda xüsusi simvollar da daxil ediləcəkdir. Default olaraq true-dur.</param>
    /// <returns>Təsadüfi simvollardan ibarət açar string formatında qaytarılır.</returns>
    public static string Generate(int length = 10, bool addSymbols = true)
    {
        Random random = new Random();

        // İstifadə ediləcək xüsusi simvollar
        const string symbols = "!@#$%^&*()_+=-[]{}|;:,.<>/?";

        // Böyük və kiçik hərflər, rəqəmlərdən ibarət simvollar
        StringBuilder chars = new StringBuilder();

        // Kiçik hərflər əlavə olunur
        chars.Append('a', 26);
        // Böyük hərflər əlavə olunur
        chars.Append('A', 26);
        // Rəqəmlər əlavə olunur
        chars.Append('0', 10);

        // Əgər xüsusi simvollar daxil edilməlidirsə, onları da əlavə edir
        if (addSymbols)
            chars.Insert(0, symbols);

        // Açarı yaratmaq üçün təsadüfi simvollar seçilir
        string key = new string(Enumerable.Repeat(chars, length)
                                              .Select(s => s[random.Next(s.Length)]).ToArray());

        // Yaradılmış açar qaytarılır
        return key;
    }
}
