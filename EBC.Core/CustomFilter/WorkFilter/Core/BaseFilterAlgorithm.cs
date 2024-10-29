using EBC.Core.CustomFilter.WorkFilter.Filters;
using EBC.Core.CustomFilter.WorkFilter.Utilities;
using System.Linq.Expressions;
using System.Reflection;

namespace EBC.Core.CustomFilter.WorkFilter.Core;

public static class BaseFilterAlgorithm<TEntity> where TEntity : class
{
    /// <summary>
    /// Təyin edilmiş filtr modelinə əsasən <typeparamref name="TEntity"/> tipində bir filtr ifadəsi yaradır.
    /// </summary>
    /// <typeparam name="TFilter">Filtr modelinin tipi</typeparam>
    /// <param name="filterModel">Filtr modelinin instansı</param>
    /// <returns>Uyğunlaşdırılmış filtr ifadəsi və ya null</returns>
    public static Expression<Func<TEntity, bool>> GenerateFilterExpression<TFilter>(TFilter filterModel)
    {
        if (filterModel == null) throw new ArgumentNullException(nameof(filterModel));

        var parameter = Expression.Parameter(typeof(TEntity), "entity");
        Expression? filterExpression = null;

        // Filter modelindəki FilterType tipində olan və adi field-lərlə uyğunlaşan xassələri əldə edirik
        var filterProperties = typeof(TFilter).GetProperties()
            .Where(p => p.PropertyType != typeof(FilterOperation) &&
                        typeof(TFilter).GetProperty($"{p.Name}FilterType") != null);

        foreach (var property in filterProperties)
        {
            var propertyValue = property.GetValue(filterModel)?.ToString();
            if (!string.IsNullOrEmpty(propertyValue) && propertyValue != default(DateTime).ToString())
            {
                filterExpression = AddFilterExpressions(parameter, property.Name, propertyValue, filterExpression, filterModel);
            }
        }

        return Expression.Lambda<Func<TEntity, bool>>(filterExpression ?? Expression.Constant(true), parameter);
    }

    private static Expression AddFilterExpressions<TFilter>(
        ParameterExpression parameter,
        string propertyName,
        string propertyValue,
        Expression filterExpression,
        TFilter filterModel)
    {
        var filterType = GetFilterType<TFilter>(propertyName, filterModel);

        var expressionBuilder = new ExpressionBuilder();

        // ExpressionBuilder-dan istifadə edərək ifadələri yaradırıq
        var equality = expressionBuilder.CreateExpressionForType(parameter, propertyName, propertyValue, filterType);

        // Mövcud ifadələri birləşdiririk
        return filterExpression == null ? equality : Expression.AndAlso(filterExpression, equality);
    }

    private static FilterOperation GetFilterType<TFilter>(string propertyName, TFilter filterModel)
    {
        var filterTypeProperty = typeof(TFilter).GetProperty($"{propertyName}FilterType");

        return filterTypeProperty != null && filterTypeProperty.GetValue(filterModel) is FilterOperation filterOperation
            ? filterOperation
            : FilterOperation.Equals;
    }
}
