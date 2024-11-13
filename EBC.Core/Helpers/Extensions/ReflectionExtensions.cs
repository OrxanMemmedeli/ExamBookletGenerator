namespace EBC.Core.Helpers.Extensions;

/// <summary>
/// Refleksiya vasitəsilə nullable olub olmadığını yoxlamaq üçün genişləndirmə metodları.
/// </summary>
public static class ReflectionExtensions
{
    /// <summary>
    /// Property-nin nullable olub olmadığını yoxlayır.
    /// </summary>
    /// <param name="propertyInfo">Yoxlanılacaq property məlumatı</param>
    /// <returns>Property nullable-dirsə true, deyilse false qaytarır</returns>
    public static bool IsNullable(this System.Reflection.PropertyInfo propertyInfo)
    {
        if (!propertyInfo.PropertyType.IsValueType) return true;
        return Nullable.GetUnderlyingType(propertyInfo.PropertyType) != null;
    }
}