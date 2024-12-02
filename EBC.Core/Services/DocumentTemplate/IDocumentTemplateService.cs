using EBC.Core.Entities.Common;

namespace EBC.Core.Services.DocumentTemplate;

/// <summary>
/// Şablon Word fayllarında placeholder sahələrini əvəzləmək üçün servisin interfeysi.
/// </summary>
public interface IDocumentTemplateService<T> where T : BaseEntity<Guid>
{
    /// <summary>
    /// Word şablon faylında placeholder-ları əvəz edir və nəticəni fayl kimi saxlayır.
    /// </summary>
    /// <param name="templatePath">Şablon faylının tam fayl yolu.</param>
    /// <param name="outputPath">Nəticə faylının saxlanacağı tam yol.</param>
    /// <param name="data">Placeholder sahələri üçün açar-dəyər cütlükləri.</param>
    /// <param name="bookletContent">Dinamik olaraq əlavə olunacaq suallar və məzmun.</param>
    /// <returns>Heç bir nəticə qaytarmır; faylı saxlayır.</returns>
    Task ReplacePlaceholdersAndSaveAsync(
        string templatePath,
        string outputPath,
        IDictionary<string, string> data,
        string bookletContent
    );

    /// <summary>
    /// Word şablon faylında placeholder-ları əvəz edir və nəticəni fayl kimi saxlayır.
    /// </summary>
    /// <param name="templatePath">Şablon faylının tam fayl yolu.</param>
    /// <param name="outputPath">Nəticə faylının saxlanacağı tam yol.</param>
    /// <param name="data">Placeholder sahələri üçün açar-dəyər cütlükləri.</param>
    /// <param name="questions">Kitabçaya əlavə olunacaq suallar kolleksiyası.</param>
    /// <returns>Heç bir nəticə qaytarmır; faylı saxlayır.</returns>
    Task ReplacePlaceholdersAndSaveAsync(
        string templatePath,
        string outputPath,
        IDictionary<string, string> data,
        IEnumerable<T> content
    );


    /// <summary>
    /// Word şablon faylında placeholder-ları əvəz edir və nəticəni <c>byte[]</c> formatında qaytarır.
    /// </summary>
    /// <param name="templatePath">Şablon faylının tam fayl yolu.</param>
    /// <param name="data">Placeholder sahələri üçün açar-dəyər cütlükləri.</param>
    /// <param name="bookletContent">Dinamik olaraq əlavə olunacaq suallar və məzmun.</param>
    /// <returns>Nəticə sənədinin <c>byte[]</c> formatında təqdimatı.</returns>
    Task<byte[]> ReplacePlaceholdersAndReturnAsync(
        string templatePath,
        IDictionary<string, string> data,
        string bookletContent
    );

    /// <summary>
    /// Word şablon faylında placeholder-ları əvəz edir və nəticəni <c>byte[]</c> formatında qaytarır.
    /// </summary>
    /// <param name="templatePath">Şablon faylının tam fayl yolu.</param>
    /// <param name="data">Placeholder sahələri üçün açar-dəyər cütlükləri.</param>
    /// <param name="questions">Kitabçaya əlavə olunacaq suallar kolleksiyası.</param>
    /// <returns>Nəticə sənədinin <c>byte[]</c> formatında təqdimatı.</returns>
    Task<byte[]> ReplacePlaceholdersAndReturnAsync(
        string templatePath,
        IDictionary<string, string> data,
        IEnumerable<T> questions
    );
}
