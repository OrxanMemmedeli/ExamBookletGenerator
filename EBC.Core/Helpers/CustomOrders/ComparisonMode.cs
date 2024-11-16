namespace EBC.Core.Helpers.CustomOrders;

/// <summary>
/// Defines the modes for comparison logic.
/// </summary>
public enum ComparisonMode
{
    OnlyInteger,
    OnlyDecimal,
    OnlyVersion,
    IntegerAndDecimal,
    DecimalAndVersion,
    All
}
