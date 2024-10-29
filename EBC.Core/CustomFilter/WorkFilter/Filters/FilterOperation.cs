namespace EBC.Core.CustomFilter.WorkFilter.Filters;
/// <summary>
/// Filtr əməliyyatlarının növlərini təyin edən enum.
/// Məsələn, bərabərlik, böyükdür, kiçikdir, arasında, daxil olmaq və s.
/// </summary>
public enum FilterOperation
{
    Equals = 1,
    NotEquals = 2,
    GreaterThan = 3,
    LessThan = 4,
    In = 5,
    NotIn = 6,
    Between = 7,
    Contains = 8,
    NotContains = 9
}
