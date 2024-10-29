using System.Text;

namespace EBC.Core.Helpers.Generator;

/// <summary>
/// Müxtəlif komponentləri birləşdirərək unikal kod yaratmaq üçün istifadə olunan sinifdir.
/// Bu sinif <see cref="GenerateCode"/> metodu vasitəsilə, prefiks olaraq verilən mətn və cari tarix/zaman komponentlərini birləşdirir,
/// nəticədə unikal bir kod qaytarır. 
/// </summary>
public static class TimeStampKeyGenerator
{
    /// <summary>
    /// Unikal kodu yaratmaq üçün metod.
    /// Bu metodun məqsədi, verilmiş prefiksi və cari tarixi birləşdirərək hər dəfə fərqli bir kod yaratmaqdır.
    /// Kodun unikallığını təmin etmək üçün bu komponentlərdən istifadə olunur:
    /// - Prefiks: İstifadəçinin daxil etdiyi mətn (istəyə bağlı),
    /// - İlin son iki rəqəmi,
    /// - Ayın iki rəqəmi,
    /// - Günün iki rəqəmi,
    /// - Saat (24 saat formatında),
    /// - Dəqiqə,
    /// - Saniyə,
    /// - Milisaniyə.
    /// Hər bir komponentin sətirə çevrilməsi və birləşdirilməsi nəticəsində unikal bir kod yaradılır.
    /// </summary>
    /// <param name="startText">
    /// Kodun əvvəlində istifadə olunacaq prefiks mətnidir. 
    /// İstifadəçi prefiks daxil etmədikdə, standart olaraq boş sətir istifadə edilir.
    /// </param>
    /// <returns>
    /// Prefiks və zaman məlumatlarını ehtiva edən unikal kodu sətir kimi qaytarır.
    /// Məsələn, prefiks "STI-" olduqda və vaxt "23:59:59.999" olduqda kod "STI-230101235959999" formasında ola bilər.
    /// </returns>
    public static string GenerateCode(string? startText = null)
    {
        DateTime now = DateTime.Now;

        StringBuilder @string = new StringBuilder();

        @string.Append(startText)          // Prefiks
           .Append(now.ToString("yy"))     // İl
           .Append(now.ToString("MM"))     // Ay
           .Append(now.ToString("dd"))     // Gün
           .Append(now.ToString("HH"))     // Saat
           .Append(now.ToString("mm"))     // Dəqiqə
           .Append(now.ToString("ss"))     // Saniyə
           .Append(now.ToString("fff"));   // Milisaniyə

        return @string.ToString();
    }
}
