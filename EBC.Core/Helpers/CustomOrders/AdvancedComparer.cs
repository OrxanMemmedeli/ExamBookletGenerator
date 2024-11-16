namespace EBC.Core.Helpers.CustomOrders;

/// <summary>
/// Flexible comparer for ordering based on the specified comparison mode.
/// </summary>
public class DynamicComparer : IComparer<string>
{
    private readonly ComparisonMode _mode;

    /// <summary>
    /// Initializes a new instance of the <see cref="DynamicComparer"/> class with a specific comparison mode.
    /// </summary>
    /// <param name="mode">The comparison mode to use.</param>
    public DynamicComparer(ComparisonMode mode)
    {
        _mode = mode;
    }

    /// <summary>
    /// Compares two strings based on the specified comparison mode.
    /// </summary>
    public int Compare(string x, string y)
    {
        // Null check
        if (x == null && y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;

        // Handle comparison logic based on mode
        return _mode switch
        {
            ComparisonMode.OnlyInteger => CompareIntegers(x, y),
            ComparisonMode.OnlyDecimal => CompareDecimals(x, y),
            ComparisonMode.OnlyVersion => CompareVersions(x, y),
            ComparisonMode.IntegerAndDecimal => CompareIntegerAndDecimal(x, y),
            ComparisonMode.DecimalAndVersion => CompareDecimalAndVersion(x, y),
            ComparisonMode.All => CompareAll(x, y),
            _ => string.Compare(x, y, StringComparison.Ordinal)
        };
    }

    /// <summary>
    /// Compares integers only.
    /// </summary>
    private static int CompareIntegers(string x, string y)
    {
        if (int.TryParse(x, out var intX) && int.TryParse(y, out var intY))
            return intX.CompareTo(intY);

        return string.Compare(x, y, StringComparison.Ordinal);
    }

    /// <summary>
    /// Compares decimals only.
    /// </summary>
    private static int CompareDecimals(string x, string y)
    {
        if (decimal.TryParse(x, out var decimalX) && decimal.TryParse(y, out var decimalY))
            return decimalX.CompareTo(decimalY);

        return string.Compare(x, y, StringComparison.Ordinal);
    }

    /// <summary>
    /// Compares version-like strings only.
    /// </summary>
    private static int CompareVersions(string x, string y)
    {
        var partsX = x.Split('.').Select(part => int.TryParse(part, out var val) ? val : 0).ToList();
        var partsY = y.Split('.').Select(part => int.TryParse(part, out var val) ? val : 0).ToList();

        for (int i = 0; i < Math.Min(partsX.Count, partsY.Count); i++)
        {
            if (partsX[i] < partsY[i]) return -1;
            if (partsX[i] > partsY[i]) return 1;
        }

        return partsX.Count.CompareTo(partsY.Count);
    }

    /// <summary>
    /// Compares integers and decimals together.
    /// </summary>
    private static int CompareIntegerAndDecimal(string x, string y)
    {
        if (decimal.TryParse(x, out var decimalX) && decimal.TryParse(y, out var decimalY))
            return decimalX.CompareTo(decimalY);

        return string.Compare(x, y, StringComparison.Ordinal);
    }

    /// <summary>
    /// Compares decimals and version-like strings together.
    /// </summary>
    private static int CompareDecimalAndVersion(string x, string y)
    {
        if (decimal.TryParse(x, out _) && decimal.TryParse(y, out _))
            return CompareDecimals(x, y);

        if (x.Contains('.') && y.Contains('.'))
            return CompareVersions(x, y);

        return string.Compare(x, y, StringComparison.Ordinal);
    }

    /// <summary>
    /// Compares all types (integers, decimals, and versions).
    /// </summary>
    private static int CompareAll(string x, string y)
    {
        if (int.TryParse(x, out _) && int.TryParse(y, out _))
            return CompareIntegers(x, y);

        if (decimal.TryParse(x, out _) && decimal.TryParse(y, out _))
            return CompareDecimals(x, y);

        if (x.Contains('.') && y.Contains('.'))
            return CompareVersions(x, y);

        return string.Compare(x, y, StringComparison.Ordinal);
    }

}

/*İstifadə Nümunələri
 

 * ---------------------------------------------------------------
1. Yalnız Tam Ədədlər:

var integers = new List<string> { "10", "2", "1", "5", "3" };
var comparer = new DynamicComparer(ComparisonMode.OnlyInteger);

var sortedIntegers = integers.OrderBy(x => x, comparer).ToList();
Console.WriteLine(string.Join(", ", sortedIntegers));
// Çıxış: 1, 2, 3, 5, 10



 * ---------------------------------------------------------------
2. Yalnız Ondalığa Malik Ədədlər:

var decimals = new List<string> { "1.2", "1.3", "12.5", "12.98" };
var comparer = new DynamicComparer(ComparisonMode.OnlyDecimal);

var sortedDecimals = decimals.OrderBy(x => x, comparer).ToList();
Console.WriteLine(string.Join(", ", sortedDecimals));
// Çıxış: 1.2, 1.3, 12.5, 12.98



 * ---------------------------------------------------------------
3. Yalnız Versiyalar:

var versions = new List<string> { "1.2.3", "4.5", "4.5.6.8", "2.5", "2.6", "2.7" };
var comparer = new DynamicComparer(ComparisonMode.OnlyVersion);

var sortedVersions = versions.OrderBy(x => x, comparer).ToList();
Console.WriteLine(string.Join(", ", sortedVersions));
// Çıxış: 1.2.3, 2.5, 2.6, 2.7, 4.5, 4.5.6.8



 * ---------------------------------------------------------------
4. Tam və Ondalıq:

var mixed = new List<string> { "1", "2.5", "3", "10", "2" };
var comparer = new DynamicComparer(ComparisonMode.IntegerAndDecimal);

var sortedMixed = mixed.OrderBy(x => x, comparer).ToList();
Console.WriteLine(string.Join(", ", sortedMixed));
// Çıxış: 1, 2, 2.5, 3, 10



 * ---------------------------------------------------------------
5. Hamısı (Qarışıq Verilənlər):

var all = new List<string> { "1", "2.5", "1.2.3", "12.98", "4.5.6.8", "10", "2.7", "1.3", "4.5", "2" };
var comparer = new DynamicComparer(ComparisonMode.All);

var sortedAll = all.OrderBy(x => x, comparer).ToList();
Console.WriteLine(string.Join(", ", sortedAll));
// Çıxış: 1, 2, 2.5, 2.7, 1.2.3, 1.3, 4.5, 4.5.6.8, 10, 12.98
 
 
 */