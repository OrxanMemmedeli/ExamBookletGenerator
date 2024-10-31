using Newtonsoft.Json;
using System.Xml.Serialization;

namespace EBC.Core.Utilities;

/// <summary>
/// JSON və XML serializasiyası və deserializasiyası əməliyyatları üçün yardımçı sinif.
/// </summary>
public static class JsonSerializer
{
    /// <summary>
    /// Verilmiş obyekti JSON formatında serialize edir.
    /// </summary>
    /// <param name="obj">Serialize ediləcək obyekt.</param>
    /// <returns>JSON formatında obyektin string ifadəsi.</returns>
    public static string Serialize(object obj) => JsonConvert.SerializeObject(obj);


    /// <summary>
    /// Verilmiş obyekti formatlı JSON formatında serialize edir.
    /// </summary>
    /// <param name="obj">Serialize ediləcək obyekt.</param>
    /// <param name="format">JSON formatlama parametrləri.</param>
    /// <returns>Formatlı JSON string.</returns>
    public static string Serialize(object obj, Formatting format) => JsonConvert.SerializeObject(obj, format);


    /// <summary>
    /// Verilmiş obyekti xüsusi tarix formatı ilə JSON formatında serialize edir.
    /// </summary>
    /// <param name="obj">Serialize ediləcək obyekt.</param>
    /// <param name="dateFormatString">Tarix formatı.</param>
    /// <param name="format">JSON formatlama parametrləri, default `None`.</param>
    /// <returns>Tarix formatı nəzərə alınaraq JSON string.</returns>
    public static string Serialize(object obj, string dateFormatString, Formatting format = Formatting.None)
    {
        var jsonSettings = new JsonSerializerSettings
        {
            DateFormatString = dateFormatString,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = format
        };
        return JsonConvert.SerializeObject(obj, jsonSettings);
    }


    /// <summary>
    /// JSON formatındakı string ifadəni obyektə çevirir.
    /// </summary>
    /// <typeparam name="T">Dönüşdürüləcək obyektin tipi.</typeparam>
    /// <param name="json">JSON formatında string ifadə.</param>
    /// <returns>Tipə uyğun obyekt.</returns>
    public static T DeserializeObject<T>(this string json) => JsonConvert.DeserializeObject<T>(json);


    /// <summary>
    /// XML formatındakı string ifadəni obyektə çevirir.
    /// </summary>
    /// <typeparam name="T">Dönüşdürüləcək obyektin tipi.</typeparam>
    /// <param name="xmlContent">XML formatında string ifadə.</param>
    /// <returns>T tipinə uyğun obyekt və ya null (əgər deserializasiya uğursuz olarsa).</returns>
    public static T? DeserializeXML<T>(this string xmlContent) where T : class
    {
        ArgumentNullException.ThrowIfNullOrEmpty(xmlContent);

        var serializer = new XmlSerializer(typeof(T));
        using var stringReader = new StringReader(xmlContent);
        return serializer.Deserialize(stringReader) as T;
    }


    /// <summary>
    /// Verilmiş obyekti XML formatında serialize edir.
    /// </summary>
    /// <typeparam name="T">Serialize ediləcək obyektin tipi.</typeparam>
    /// <param name="objectToSerialize">Serialize ediləcək obyekt.</param>
    /// <returns>XML formatında obyektin string ifadəsi və ya null (əgər obyekt null-dursa).</returns>
    public static string? SerializeXML<T>(this T objectToSerialize)
    {
        ArgumentNullException.ThrowIfNull(objectToSerialize);

        var xmlSerializer = new XmlSerializer(typeof(T));
        using var stringWriter = new StringWriter();
        xmlSerializer.Serialize(stringWriter, objectToSerialize);
        return stringWriter.ToString();
    }
}
