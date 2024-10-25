namespace EBC.Core.Helpers.Extensions;


/// <summary>
/// Sayıları sətir formasında ifadə etmək üçün genişləndirici metodlar sinfi.
/// </summary>
public static class NumberExtension
{
    /// <summary>
    /// Verilmiş rəqəmi müvafiq Azərbaycan dilində sətir formasında qaytarır.
    /// </summary>
    /// <param name="number">Rəqəm dəyəri.</param>
    /// <returns>Rəqəmin sətir formasında tərcüməsi.</returns>
    public static string ConvertUnitToString(string number)
    {
        if (string.IsNullOrEmpty(number)) return string.Empty;

        return int.Parse(number) switch
        {
            1 => "Bir",
            2 => "İki",
            3 => "Üç",
            4 => "Dörd",
            5 => "Beş",
            6 => "Altı",
            7 => "Yeddi",
            8 => "Səkkiz",
            9 => "Doqquz",
            _ => string.Empty
        };
    }

    /// <summary>
    /// Verilmiş onluq rəqəmi sətir formasında ifadə edir.
    /// </summary>
    /// <param name="number">Onluq rəqəm dəyəri.</param>
    /// <returns>Onluq rəqəmin sətir formasında tərcüməsi.</returns>
    public static string ConvertDecimalToString(string number)
    {
        if (string.IsNullOrEmpty(number)) return string.Empty;

        int _Number = int.Parse(number);
        return _Number switch
        {
            10 => "On",
            20 => "İyirmi",
            30 => "Otuz",
            40 => "Qırx",
            50 => "Əlli",
            60 => "Altmış",
            70 => "Yetmiş",
            80 => "Səksən",
            90 => "Doxsan",
            _ => _Number > 0 ? $"{ConvertDecimalToString(number.Substring(0, 1) + "0")} {ConvertUnitToString(number.Substring(1))}" : string.Empty
        };
    }

    /// <summary>
    /// Verilmiş tam rəqəmi sətir formasında ifadə edir.
    /// </summary>
    /// <param name="number">Tam rəqəm dəyəri.</param>
    /// <returns>Rəqəmin sətir formasında tərcüməsi.</returns>
    public static string ConvertWholeNumberToString(string number)
    {
        if (string.IsNullOrEmpty(number)) return string.Empty;

        double dblAmt = Convert.ToDouble(number);
        if (dblAmt == 0) return string.Empty;

        int numDigits = number.Length;
        string place = numDigits switch
        {
            1 => ConvertUnitToString(number),
            2 => ConvertDecimalToString(number),
            3 => " Yüz ",
            4 or 5 or 6 => " Min ",
            7 or 8 or 9 => " Milyon ",
            10 or 11 or 12 => " Milyard ",
            _ => string.Empty
        };

        if (numDigits <= 2)
            return place;

        int pos = numDigits % (place == " Yüz " ? 3 : (place == " Min " ? 4 : 7)) + 1;

        return number.Substring(0, pos) != "0" && number.Substring(pos) != "0"
            ? $"{ConvertWholeNumberToString(number.Substring(0, pos))}{place}{ConvertWholeNumberToString(number.Substring(pos))}"
            : $"{ConvertWholeNumberToString(number.Substring(0, pos))}{ConvertWholeNumberToString(number.Substring(pos))}";
    }

    /// <summary>
    /// Onluq nöqtəsi olan rəqəmi "manat" və "qəpik" ilə birlikdə sətir formasında ifadə edir.
    /// </summary>
    /// <param name="numb">Rəqəm dəyəri.</param>
    /// <returns>Rəqəmin manat və qəpiklə birlikdə ifadəsi.</returns>
    public static string ConvertDecimalPointNumbersToString(this string numb)
    {
        if (string.IsNullOrEmpty(numb)) return string.Empty;

        string wholeNo = numb, points = "";
        int decimalPlace = numb.IndexOf(".");
        if (decimalPlace > 0)
        {
            wholeNo = numb.Substring(0, decimalPlace);
            points = numb.Substring(decimalPlace + 1);
        }

        string andStr = "Manat";
        string endStr = "qəp.";
        return points != "00"
            ? $"{ConvertWholeNumberToString(wholeNo).Trim()} {andStr} {points} {endStr}"
            : $"{ConvertWholeNumberToString(wholeNo).Trim()} {andStr}";
    }
}
