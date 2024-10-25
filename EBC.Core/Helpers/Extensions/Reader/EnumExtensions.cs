using Org.BouncyCastle.Asn1.Cms;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace EBC.Core.Helpers.Extensions.Reader;

/// <summary>
/// Enum tipləri üçün genişləndirici metodlar sinfi.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Verilmiş Enum dəyərinin <see cref="DescriptionAttribute"/> təsvirini qaytarır.
    /// </summary>
    /// <param name="value">Enum dəyəri.</param>
    /// <returns>Təsvir mətni və ya enum dəyərinin adı.</returns>
    public static string GetDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        if (field == null) return value.ToString(); 

        var attribute = field.GetCustomAttribute<DescriptionAttribute>();
        return attribute?.Description ?? value.ToString();
    }


    /// <summary>
    /// Verilmiş Enum dəyərinin <see cref="DisplayAttribute"/> ilə təsvirini qaytarır.
    /// </summary>
    /// <param name="value">Enum dəyəri.</param>
    /// <returns>Təsvir mətni və ya enum dəyərinin adı.</returns>
    public static string GetDisplayName(this Enum value)
    {
        var type = value.GetType();
        var fieldInfo = type.GetField(value.ToString());
        if (fieldInfo == null) return value.ToString(); 

        var displayAttribute = fieldInfo.GetCustomAttribute<DisplayAttribute>();
        return displayAttribute != null
            ? (displayAttribute.ResourceType == null
                ? displayAttribute.Name
                : LookupResource(displayAttribute.ResourceType, displayAttribute.Name))
            : value.ToString();
    }


    /// <summary>
    /// Verilmiş sətir dəyərinin Enum üzvü ilə əlaqəli <see cref="DisplayAttribute"/> təsvirini qaytarır.
    /// </summary>
    /// <param name="enumValue">Enum dəyərinin adı.</param>
    /// <param name="whereToSearch">Enum tipi.</param>
    /// <returns>Təsvir mətni və ya verilmiş dəyər.</returns>
    public static string GetDisplayName(this string enumValue, Type whereToSearch)
    {
        if (string.IsNullOrEmpty(enumValue)) return enumValue; // Null və ya boş sətir yoxlaması əlavə edildi.

        var field = whereToSearch.GetField(enumValue);
        if (field == null) return enumValue;

        var displayAttribute = field.GetCustomAttribute<DisplayAttribute>();
        return displayAttribute != null
            ? (displayAttribute.ResourceType == null
                ? displayAttribute.Name
                : LookupResource(displayAttribute.ResourceType, displayAttribute.Name))
            : enumValue;
    }


    /// <summary>
    /// Enum dəyərinin ekran adını (Display Name) əsas götürərək Enum dəyərini qaytarır.
    /// </summary>
    /// <typeparam name="TEnum">Enum tipi.</typeparam>
    /// <param name="value">Enum dəyərinin ekran adı.</param>
    /// <returns>Uyğun gələn Enum dəyəri.</returns>
    public static TEnum GetEnumValueByDisplayName<TEnum>(this string value) where TEnum : struct
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentNullException(nameof(value));

        var type = typeof(TEnum);
        if (!type.IsEnum) throw new InvalidOperationException($"{type} Enum növü olmalıdır.");

        var fields = type.GetFields();
        foreach (var field in fields)
        {
            var displayAttribute = field.GetCustomAttribute<DisplayAttribute>();
            var displayName = displayAttribute != null
                ? LookupResource(displayAttribute.ResourceType, displayAttribute.Name)
                : field.Name;

            if (displayName == value)
                return (TEnum)field.GetValue(null);
        }

        throw new ArgumentOutOfRangeException(nameof(value), "Uyğun gələn enum dəyəri tapılmadı.");
    }


    /// <summary>
    /// Verilmiş dəyərin göstərilən massivdə olub-olmadığını yoxlayır.
    /// </summary>
    /// <typeparam name="T">Dəyərin tipi.</typeparam>
    /// <param name="value">Yoxlanacaq dəyər.</param>
    /// <param name="source">Dəyərlər massivi.</param>
    /// <returns>Əgər dəyər massivdə varsa, true; əks halda false qaytarır.</returns>
    public static bool IsIn<T>(this T value, params T[] source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        return source.Contains(value);
    }


    /// <summary>
    /// Verilmiş sətir dəyərini uyğun gələn Enum dəyərinə çevirir.
    /// </summary>
    /// <typeparam name="TEnum">Enum tipi.</typeparam>
    /// <param name="value">Sətir dəyəri.</param>
    /// <returns>Uyğun gələn Enum dəyəri və ya default Enum dəyəri.</returns>
    public static TEnum ToEnum<TEnum>(this string value) where TEnum : struct
    {
        return Enum.TryParse<TEnum>(value, true, out var result) ? result : default;
    }


    /// <summary>
    /// Resurs menecerindən verilmiş resurs açarının dəyərini qaytarır.
    /// </summary>
    /// <param name="resourceManagerProvider">Resurs meneceri təminatçısı.</param>
    /// <param name="resourceKey">Resurs açarı.</param>
    /// <returns>Resurs dəyəri və ya açar adı.</returns>
    private static string LookupResource(IReflect resourceManagerProvider, string resourceKey)
    {
        if (resourceManagerProvider == null) // Null yoxlaması əlavə edildi
            throw new ArgumentNullException(nameof(resourceManagerProvider));

        if (string.IsNullOrEmpty(resourceKey)) // Null və ya boş sətir yoxlaması əlavə edildi
            throw new ArgumentNullException(nameof(resourceKey));

        var property = resourceManagerProvider
            .GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
            .FirstOrDefault(p => p.PropertyType == typeof(System.Resources.ResourceManager));

        if (property != null)
        {
            var resourceManager = (System.Resources.ResourceManager)property.GetValue(null, null);
            return resourceManager?.GetString(resourceKey) ?? resourceKey; // Null yoxlaması əlavə edildi.
        }

        return resourceKey; // Resurs tapılmadıqda açar adı qaytarılır.
    }
}
