using System.Globalization;

namespace EBC.Core.Helpers.Extensions;

/// <summary>
/// DateTime obyektləri üçün genişləndirici metodlar sinfi.
/// </summary>
public static class DateTimeExtension
{
    /// <summary>
    /// DateTime obyektini Oracle tarix formatına çevirir.
    /// </summary>
    /// <param name="dateTime">Tarix dəyəri.</param>
    /// <returns>Oracle formatında tarix sətiri.</returns>
    public static string ToOracleDate(this DateTime dateTime)
    {
        return dateTime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Verilmiş günün son saatını (23:59:59) əldə edir.
    /// </summary>
    /// <param name="dateTime">Tarix dəyəri.</param>
    /// <returns>Günün son saatını ehtiva edən <see cref="DateTime"/> dəyəri.</returns>
    public static DateTime GetDayLastTime(this DateTime dateTime)
    {
        return dateTime.Date.Add(new TimeSpan(23, 59, 59));
    }

    /// <summary>
    /// Verilmiş ayın son gününü əldə edir.
    /// </summary>
    /// <param name="dateTime">Tarix dəyəri.</param>
    /// <returns>Ayın son gününü ehtiva edən <see cref="DateTime"/> dəyəri.</returns>
    public static DateTime GetMonthLastDate(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
    }

    /// <summary>
    /// Verilmiş ayın ilk gününü əldə edir.
    /// </summary>
    /// <param name="dateTime">Tarix dəyəri.</param>
    /// <returns>Ayın ilk gününü ehtiva edən <see cref="DateTime"/> dəyəri.</returns>
    public static DateTime GetMonthStartDate(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, 1);
    }

    /// <summary>
    /// Verilmiş ilin ilk gününü əldə edir.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static DateTime GetYearStartDate(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, 1, 1);
    }

    /// <summary>
    /// Verilmiş ilin son gününü əldə edir.
    /// </summary>
    /// <param name="dateTime">Tarix dəyəri.</param>
    /// <returns>İlin son gününü ehtiva edən <see cref="DateTime"/> dəyəri.</returns>
    public static DateTime GetYearLastDate(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, 12, 31);
    }

    /// <summary>
    /// Tarixi müəyyən bir formatda string kimi qaytarır.
    /// </summary>
    /// <param name="dateTime">Tarix dəyəri.</param>
    /// <returns>Formatlanmış tarix sətiri.</returns>
    public static string GetDateToString(this DateTime dateTime)
    {
        return dateTime.Month switch
        {
            1 => $"{dateTime.Day}-JAN-{dateTime.Year}",
            2 => $"{dateTime.Day}-FEB-{dateTime.Year}",
            3 => $"{dateTime.Day}-MAR-{dateTime.Year}",
            4 => $"{dateTime.Day}-APR-{dateTime.Year}",
            5 => $"{dateTime.Day}-MAY-{dateTime.Year}",
            6 => $"{dateTime.Day}-JUN-{dateTime.Year}",
            7 => $"{dateTime.Day}-JUL-{dateTime.Year}",
            8 => $"{dateTime.Day}-AUG-{dateTime.Year}",
            9 => $"{dateTime.Day}-SEPT-{dateTime.Year}",
            10 => $"{dateTime.Day}-OCT-{dateTime.Year}",
            11 => $"{dateTime.Day}-NOV-{dateTime.Year}",
            12 => $"{dateTime.Day}-DEC-{dateTime.Year}",
            _ => string.Empty,
        };

        #region OldCode
        //int month = dateTime.Month;
        //switch (month)
        //{
        //    case 1:
        //        return $"{dateTime.Day}-JAN-{dateTime.Year}";
        //    case 2:
        //        return $"{dateTime.Day}-FEB-{dateTime.Year}";
        //    case 3:
        //        return $"{dateTime.Day}-MAR-{dateTime.Year}";
        //    case 4:
        //        return $"{dateTime.Day}-APR-{dateTime.Year}";
        //    case 5:
        //        return $"{dateTime.Day}-MAY-{dateTime.Year}";
        //    case 6:
        //        return $"{dateTime.Day}-JUN-{dateTime.Year}";
        //    case 7:
        //        return $"{dateTime.Day}-JUL-{dateTime.Year}";
        //    case 8:
        //        return $"{dateTime.Day}-AUG-{dateTime.Year}";
        //    case 9:
        //        return $"{dateTime.Day}-SEPT-{dateTime.Year}";
        //    case 10:
        //        return $"{dateTime.Day}-OCT-{dateTime.Year}";
        //    case 11:
        //        return $"{dateTime.Day}-NOV-{dateTime.Year}";
        //    case 12:
        //        return $"{dateTime.Day}-DEC-{dateTime.Year}";

        //}

        //return String.Empty;
        #endregion
    }

    /// <summary>
    /// Tarixi Azərbaycan mədəniyyətinə uyğun string kimi qaytarır.
    /// </summary>
    /// <param name="dateTime">Tarix dəyəri.</param>
    /// <returns>Azərbaycan mədəniyyətinə uyğun formatlanmış tarix sətiri.</returns>
    public static string GetDateToStringCulture(this DateTime dateTime)
    {
        string month = dateTime.ToString("MMMM", new CultureInfo("az-Latn-AZ"));
        return $" \"{dateTime.Day}\"  \"{month}\" {GetYearWithPostfix(dateTime)} il";
    }

    /// <summary>
    /// Uzun formatda tarixi Azərbaycan mədəniyyətinə uyğun string kimi qaytarır.
    /// </summary>
    /// <param name="dateTime">Tarix dəyəri.</param>
    /// <returns>Uzun formatda Azərbaycan mədəniyyətinə uyğun tarix sətiri.</returns>
    public static string GetLongDateToStringCulture(this DateTime dateTime)
    {
        string month = dateTime.ToString("MMMM", new CultureInfo("az-Latn-AZ"));
        return $" {dateTime.Day}  {month} {GetYearWithPostfix(dateTime)} il, saat {dateTime:HH:mm}";
    }

    /// <summary>
    /// İli postfiks ilə birlikdə qaytarır.
    /// </summary>
    /// <param name="datetime">Tarix dəyəri.</param>
    /// <returns>İl dəyəri postfiks ilə birlikdə.</returns>
    public static string GetYearWithPostfix(this DateTime datetime)
    {
        int year = datetime.Year;
        return (year % 10, year % 100) switch
        {
            (1 or 2 or 5 or 7 or 8, _) => $"{year}-ci",
            (3 or 4, _) => $"{year}-cü",
            (6, _) => $"{year}-cı",
            (9, _) => $"{year}-cu",
            (0, 10 or 30) => $"{year}-cu",
            (0, 20 or 50 or 70) => $"{year}-ci",
            (0, 80 or 40 or 60 or 90) => $"{year}-cı",
            (0, 0) when year % 1000 != 0 => $"{year}-cü",
            (0, 0) => $"{year}-ci",
            _ => string.Empty,
        };

        #region OldCode
        //int year = datetime.Year;
        //switch (year % 10)
        //{
        //    case 1:
        //    case 2:
        //    case 5:
        //    case 7:
        //    case 8:
        //        return $"{year}-ci";
        //    case 3:
        //    case 4:
        //        return $"{year}-cü";

        //    case 6:
        //        return $"{year}-cı";

        //    case 9:
        //        return $"{year}-cu";

        //    case 0:
        //        switch (year % 100)
        //        {
        //            case 10:
        //            case 30:
        //                return $"{year}-cu";
        //            case 20:
        //            case 50:
        //            case 70:
        //                return $"{year}-ci";
        //            case 80:
        //            case 40:
        //            case 60:
        //            case 90:
        //                return $"{year}-cı";
        //            case 0:
        //                if (year % 1000 != 0)
        //                    return $"{year}-cü";
        //                else
        //                    return $"{year}-ci";
        //        }
        //        break;
        //}

        //return String.Empty;
        #endregion
    }

}
