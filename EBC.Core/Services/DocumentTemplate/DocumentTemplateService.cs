using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;


namespace EBC.Core.Services.DocumentTemplate;

/// <summary>
/// Şablon Word sənədlərində placeholder-ları əvəz etmək üçün funksionallıq təmin edir.
/// </summary>
public class DocumentTemplateService : IDocumentTemplateService
{
    /// <inheritdoc/>
    public async Task ReplacePlaceholdersAndSaveAsync(
        string templatePath,
        string outputPath,
        IDictionary<string, string> data,
        string bookletContent)
    {
        // Giriş parametrlərini yoxlayır. Əgər null olarsa, istisna atılır.
        ArgumentNullException.ThrowIfNull(templatePath, nameof(templatePath));
        ArgumentNullException.ThrowIfNull(outputPath, nameof(outputPath));
        ArgumentNullException.ThrowIfNull(data, nameof(data));
        ArgumentNullException.ThrowIfNull(bookletContent, nameof(bookletContent));

        // Şablon faylını yaddaşa yükləyir.
        using var memoryStream = await LoadTemplateToMemoryStreamAsync(templatePath);

        // Yaddaşdakı faylı açır və placeholder-ları əvəz edir.
        await GenerateDocumentAsync(memoryStream, data, bookletContent);

        // Dəyişdirilmiş faylı göstərilən fayl yolunda saxlayır.
        await SaveDocumentToFileAsync(memoryStream, outputPath);
    }

    /// <inheritdoc/>
    public async Task<byte[]> ReplacePlaceholdersAndReturnAsync(
        string templatePath,
        IDictionary<string, string> data,
        string bookletContent)
    {
        // Giriş parametrlərini yoxlayır. Əgər null olarsa, istisna atılır.
        ArgumentNullException.ThrowIfNull(templatePath, nameof(templatePath));
        ArgumentNullException.ThrowIfNull(data, nameof(data));
        ArgumentNullException.ThrowIfNull(bookletContent, nameof(bookletContent));

        // Şablon faylını yaddaşa yükləyir.
        using var memoryStream = await LoadTemplateToMemoryStreamAsync(templatePath);

        // Yaddaşdakı faylı açır və placeholder-ları əvəz edir.
        await GenerateDocumentAsync(memoryStream, data, bookletContent);

        // Yaddaş axınını bayt massivinə çevirib qaytarır.
        return memoryStream.ToArray();
    }


    /// <summary>
    /// Şablon faylını yaddaşa yükləyir.
    /// </summary>
    /// <param name="templatePath">Şablon faylının tam fayl yolu.</param>
    /// <returns>Yaddaşa yüklənmiş fayl axını.</returns>
    private static async Task<MemoryStream> LoadTemplateToMemoryStreamAsync(string templatePath)
    {
        var memoryStream = new MemoryStream();
        using (var fileStream = File.Open(templatePath, FileMode.Open, FileAccess.Read))
        {
            // Şablon faylı oxunaraq yaddaş axınına yazılır.
            await fileStream.CopyToAsync(memoryStream);
        }
        memoryStream.Seek(0, SeekOrigin.Begin);
        return memoryStream;
    }

    /// <summary>
    /// Placeholder-ları əvəz edir və sənədə yeni kontent əlavə edir.
    /// </summary>
    /// <param name="memoryStream">Yaddaş axını.</param>
    /// <param name="data">Placeholder-lar üçün məlumat.</param>
    /// <param name="bookletContent">Əlavə ediləcək suallar və kontent.</param>
    private static async Task GenerateDocumentAsync(
        MemoryStream memoryStream,
        IDictionary<string, string> data,
        string bookletContent)
    {
        using var wordDocument = WordprocessingDocument.Open(memoryStream, true);

        // Sənədi yükləyir və yoxlayır.
        var document = wordDocument.MainDocumentPart?.Document
                        ?? throw new InvalidOperationException("Document cannot be null.");

        // Placeholder sahələrini əvəz edir.
        ReplacePlaceholders(document, data);

        // BookletContent sahəsini əvəz edir və yeni kontent əlavə edir.
        ReplaceBookletContent(document, "{BookletContent}", bookletContent);

        // Dəyişiklikləri sənədə tətbiq edir.
        document.Save();
        await Task.CompletedTask; // Asinxron marker.
    }

    /// <summary>
    /// Sənəddəki placeholder-ları əvəz edir.
    /// </summary>
    /// <param name="document">Word sənədi obyekti.</param>
    /// <param name="data">Placeholder-lar üçün məlumat.</param>
    private static void ReplacePlaceholders(Document document, IDictionary<string, string> data)
    {
        foreach (var placeholder in data)
        {
            foreach (var text in document.Body?.Descendants<Text>() ?? Enumerable.Empty<Text>())
            {
                // Placeholder açarını uyğun olan mətnlə əvəz edir.
                if (text.Text.Contains(placeholder.Key, StringComparison.Ordinal))
                {
                    text.Text = text.Text.Replace(placeholder.Key, placeholder.Value, StringComparison.Ordinal);
                }
            }
        }
    }

    /// <summary>
    /// BookletContent sahəsini əvəz edir və yeni kontent əlavə edir.
    /// </summary>
    /// <param name="document">Word sənədi obyekti.</param>
    /// <param name="placeholder">BookletContent placeholder-ı.</param>
    /// <param name="bookletContent">Əlavə ediləcək suallar və kontent.</param>
    private static void ReplaceBookletContent(Document document, string placeholder, string bookletContent)
    {
        var paragraphs = document.Body?.Descendants<Paragraph>().ToList() ?? new List<Paragraph>();

        foreach (var paragraph in paragraphs)
        {
            var textElement = paragraph.Descendants<Text>().FirstOrDefault();
            if (textElement != null && textElement.Text.Contains(placeholder, StringComparison.Ordinal))
            {
                // Placeholder-i silir.
                textElement.Text = textElement.Text.Replace(placeholder, string.Empty, StringComparison.Ordinal);

                // Yeni kontenti sətirlər üzrə əlavə edir.
                foreach (var line in bookletContent.Split('\n', StringSplitOptions.TrimEntries))
                {
                    var newParagraph = new Paragraph(new Run(new Text(line)));
                    document.Body?.AppendChild(newParagraph);
                }
            }
        }
    }

    /// <summary>
    /// Yaddaş axınındakı sənədi fayl olaraq saxlayır.
    /// </summary>
    /// <param name="memoryStream">Yaddaş axını.</param>
    /// <param name="outputPath">Saxlanacaq fayl yolu.</param>
    private static async Task SaveDocumentToFileAsync(MemoryStream memoryStream, string outputPath)
    {
        memoryStream.Seek(0, SeekOrigin.Begin);
        using var fileStream = File.Create(outputPath);
        await memoryStream.CopyToAsync(fileStream);
    }
}



/*
 İstifade nümunəsi
 
        // Placeholder-lar üçün məlumat
        var placeholders = new Dictionary<string, string>
        {
            { "{Grade}", "VIII Sinif - III Qrup" },
            { "{Title}", "MSİ-2" },
            { "{VersionOrVariant}", "A variantı" },
            { "{Company}", "Türkiyə Dəyanət Vəqfi Bakı Türk Liseyi" },
            { "{Year}", "2024" }
        };

        // BookletContent məzmunu
        var bookletContent = @"
                                1. Hansı cümlədə isim işlənmişdir?
                                A) Uşaqlar sevinərək qaçdılar.
                                B) Təbiət həmişə gözəldir.
                                C) Onlar yavaş-yavaş gəldilər.
                                D) Səbir ən böyük xeyirdir.
                                E) Evdə heç kim yox idi.

                                2. Aşağıdakı sözlərdən hansında təsirlik hal şəkilçisi var?
                                A) kitab
                                B) kitabı
                                C) kitabda
                                D) kitabdan
                                E) kitablarla
                                ";

        // Şablon fayl və nəticə fayl yolları
        var templatePath = "ExamBooklet.docx";
        var outputPath = "OutputBooklet.docx";

        // Servisi istifadə edərək faylı saxla
        var documentService = new DocumentTemplateService();
        await documentService.ReplacePlaceholdersAndSaveAsync(templatePath, outputPath, placeholders, bookletContent);

        Console.WriteLine("Fayl uğurla saxlandı: " + outputPath);


        // Servisi istifadə edərək byte[] al
        var documentService = new DocumentTemplateService();
        var resultBytes = await documentService.ReplacePlaceholdersAndReturnAsync(templatePath, placeholders, bookletContent);

        // Alınmış byte[] məlumatı fayla yaz
        var outputPath = "OutputBookletFromBytes.docx";
        await File.WriteAllBytesAsync(outputPath, resultBytes);

        Console.WriteLine("Byte[] fayla yazıldı: " + outputPath);

 */