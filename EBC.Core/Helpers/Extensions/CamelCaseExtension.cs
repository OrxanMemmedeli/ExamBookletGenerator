using System.Text.RegularExpressions;

namespace EBC.Core.Helpers.Extensions;

/// <summary>
/// Sətirləri CamelCase formatından sözlərə bölmək üçün genişləndirici metodlar sinfi.
/// </summary>
public static class CamelCaseExtension
{
    /// <summary>
    /// Verilmiş sətiri CamelCase formatından sözlərə ayırır.
    /// </summary>
    /// <param name="str">CamelCase formatında olan sətir.</param>
    /// <returns>Sözlərə bölünmüş sətir və ya boş sətir.</returns>
    public static string SplitCamelCase(this string str)
    {
        if (string.IsNullOrWhiteSpace(str)) return string.Empty; // Null və ya boş yoxlama.

        // Performansı qorumaq üçün cəhd-xəta idarəetməsindən qaçın.
        return Regex.Replace(
            Regex.Replace(
                str,
                @"(\P{Ll})(\P{Ll}\p{Ll})",
                "$1 $2"
            ),
            @"(\p{Ll})(\P{Ll})",
            "$1 $2"
        );
    }
}
