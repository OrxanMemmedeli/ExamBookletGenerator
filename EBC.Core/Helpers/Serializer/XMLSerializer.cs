
using System.Xml.Serialization;

namespace EBC.Core.Helpers.Serializer;

/// <summary>
/// XML formatında seriyalizasiya və deserializasiya əməliyyatlarını təmin edən genişləndirici metodlar.
/// </summary>
public static class XMLSerializer
{
    /// <summary>
    /// XML formatında olan məzmunu obyektə çevirir (deserializasiya edir).
    /// </summary>
    /// <typeparam name="T">Deserializasiya ediləcək obyektin tipi.</typeparam>
    /// <param name="xmlContent">Deserializasiya ediləcək XML formatında məzmun.</param>
    /// <returns>Deserializasiya edilmiş obyekt.</returns>
    public static T Deserialize<T>(this string xmlContent) where T : class
    {
        // Verilmiş tip üzrə XML seriyalizator yaradırıq
        XmlSerializer ser = new XmlSerializer(typeof(T));

        // StringReader ilə məzmunu oxuyuruq (blok olmadan using istifadə edilir)
        using StringReader sr = new StringReader(xmlContent);

        // Məzmun deserializasiya edilir və qaytarılır
        return (T)ser.Deserialize(sr);
    }

    /// <summary>
    /// Verilmiş obyekti XML formatında məzmuna çevirir (seriyalizasiya edir).
    /// </summary>
    /// <typeparam name="T">Seriyalizasiya ediləcək obyektin tipi.</typeparam>
    /// <param name="objectToSerialize">Seriyalizasiya ediləcək obyekt.</param>
    /// <returns>XML formatında məzmun.</returns>
    public static string Serialize<T>(this T objectToSerialize)
    {
        // Verilmiş obyektin tipi əsasında XML seriyalizator yaradılır
        XmlSerializer xmlSerializer = new XmlSerializer(objectToSerialize.GetType());

        // StringWriter ilə məzmunu yazırıq (blok olmadan using istifadə edilir)
        using StringWriter textWriter = new StringWriter();

        // Obyekt XML formatında seriyalizasiya edilir
        xmlSerializer.Serialize(textWriter, objectToSerialize);

        // Seriyalizasiya edilmiş məzmun qaytarılır
        return textWriter.ToString();
    }
}
