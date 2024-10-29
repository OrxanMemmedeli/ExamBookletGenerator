using EBC.Core.CustomFilter.WorkFilter.Filters;
using EBC.Core.CustomFilter.WorkFilter.SortingAndPaging;
using System.Linq.Expressions;
using System.Reflection;

namespace EBC.Core.CustomFilter.WorkFilter.Utilities;

public class ExpressionBuilder
{
    /// <summary>
    /// `ViewFilter` parametri əsasında bir ifadə funksiyası yaradır.
    /// </summary>
    /// <typeparam name="T">Filter tətbiq olunacaq obyekt tipi.</typeparam>
    /// <param name="viewFilter">Tətbiq olunacaq filterin strukturu.</param>
    /// <returns>Filter kriteriyalarına əsaslanan ifadə funksiyası.</returns>
    public Expression<Func<T, bool>> GetExpressionFunc<T>(ViewFilter viewFilter) where T : class
    {
        var parameter = Expression.Parameter(typeof(T));
        Expression? expression = null;

        foreach (var item in viewFilter.Filters)
        {
            var right = GetExpression(item, parameter);

            expression = expression == null
                ? right
                : item.Condition == FilterPartRelations.Or
                    ? Expression.OrElse(expression, right)
                    : Expression.AndAlso(expression, right);
        }

        return Expression.Lambda<Func<T, bool>>(expression, parameter);
    }

    /// <summary>
    /// Məlumatları sıralamaq və səhifələmək üçün metod.
    /// </summary>
    /// <typeparam name="T">Məlumat sinifi.</typeparam>
    /// <param name="data">Sıralanacaq və səhifələnəcək məlumatlar.</param>
    /// <param name="paging">Səhifələmə məlumatları.</param>
    /// <param name="sorting">Sıralama məlumatları.</param>
    /// <returns>Sıralanmış və səhifələnmiş məlumatlar.</returns>
    public IEnumerable<T> GetData<T>(IQueryable<T> data, PagingModel paging, SortingModel sorting)
    {
        data = OrderBy(data, sorting.SortBy, sorting.Direction);
        return paging.Take.HasValue && paging.Skip.HasValue
            ? data.Skip(paging.Skip.Value).Take(paging.Take.Value).ToList()
            : data.ToList();
    }

    public Expression CreateExpressionForType(ParameterExpression parameter, string propertyName, string propertyValue, FilterOperation filterType)
    {
        var property = Expression.Property(parameter, propertyName);
        Expression equality = null;

        // Guid tipi üçün xüsusi yoxlama
        if (property.Type == typeof(Guid) || property.Type == typeof(Guid?))
        {
            if (Guid.TryParse(propertyValue.ToString(), out Guid propertyGuidValue) && propertyGuidValue != Guid.Empty)
            {
                equality = filterType switch
                {
                    FilterOperation.Equals => Expression.Equal(property, Expression.Constant(propertyGuidValue)),
                    FilterOperation.NotEquals => Expression.NotEqual(property, Expression.Constant(propertyGuidValue)),
                    FilterOperation.In => GetInExpression((MemberExpression)property, new object[] { propertyGuidValue }),
                    FilterOperation.NotIn => GetNotInExpression((MemberExpression)property, new object[] { propertyGuidValue }),
                    _ => throw new NotSupportedException($"Filter type '{filterType}' is not supported for Guid properties.")
                };
            }
        }
        else
        {
            switch (Type.GetTypeCode(property.Type))
            {
                case TypeCode.String:
                    equality = filterType switch
                    {
                        FilterOperation.Equals => Expression.Equal(property, Expression.Constant(propertyValue.ToString())),
                        FilterOperation.NotEquals => Expression.NotEqual(property, Expression.Constant(propertyValue.ToString())),
                        FilterOperation.Contains => GetContainsExpression((MemberExpression)property, propertyValue.ToString()),
                        FilterOperation.NotContains => GetNotContainsExpression((MemberExpression)property, propertyValue.ToString()),
                        FilterOperation.In => GetInExpression((MemberExpression)property, ((string)propertyValue).Split(',')),
                        FilterOperation.NotIn => GetNotInExpression((MemberExpression)property, ((string)propertyValue).Split(',')),
                        _ => throw new NotSupportedException($"Filter type '{filterType}' is not supported for string properties.")
                    };
                    break;

                case TypeCode.DateTime:
                    if (DateTime.TryParse(propertyValue.ToString(), out DateTime dateValue))
                    {
                        equality = filterType switch
                        {
                            FilterOperation.Equals => CreateDateComparisonExpression(property, dateValue, FilterOperation.Equals),
                            FilterOperation.NotEquals => CreateDateComparisonExpression(property, dateValue, FilterOperation.NotEquals),
                            FilterOperation.GreaterThan => CreateDateComparisonExpression(property, dateValue, FilterOperation.GreaterThan),
                            FilterOperation.LessThan => CreateDateComparisonExpression(property, dateValue, FilterOperation.LessThan),
                            FilterOperation.Between => GetBetweenExpression((MemberExpression)property, dateValue, dateValue.AddDays(1)),
                            _ => throw new NotSupportedException($"Filter type '{filterType}' is not supported for DateTime properties.")
                        };
                    }
                    break;

                case TypeCode.Decimal:
                    if (decimal.TryParse(propertyValue.ToString(), out decimal decimalValue))
                    {
                        equality = filterType switch
                        {
                            FilterOperation.Equals => Expression.Equal(property, Expression.Constant(decimalValue)),
                            FilterOperation.NotEquals => Expression.NotEqual(property, Expression.Constant(decimalValue)),
                            FilterOperation.GreaterThan => Expression.GreaterThan(property, Expression.Constant(decimalValue)),
                            FilterOperation.LessThan => Expression.LessThan(property, Expression.Constant(decimalValue)),
                            _ => throw new NotSupportedException($"Filter type '{filterType}' is not supported for decimal properties.")
                        };
                    }
                    break;

                case TypeCode.Int32:
                    if (int.TryParse(propertyValue.ToString(), out int intValue))
                    {
                        equality = filterType switch
                        {
                            FilterOperation.Equals => Expression.Equal(property, Expression.Constant(intValue)),
                            FilterOperation.NotEquals => Expression.NotEqual(property, Expression.Constant(intValue)),
                            FilterOperation.GreaterThan => Expression.GreaterThan(property, Expression.Constant(intValue)),
                            FilterOperation.LessThan => Expression.LessThan(property, Expression.Constant(intValue)),
                            _ => throw new NotSupportedException($"Filter type '{filterType}' is not supported for int properties.")
                        };
                    }
                    break;

                case TypeCode.Boolean:
                    if (bool.TryParse(propertyValue.ToString(), out bool boolValue))
                    {
                        equality = filterType switch
                        {
                            FilterOperation.Equals => Expression.Equal(property, Expression.Constant(boolValue)),
                            FilterOperation.NotEquals => Expression.NotEqual(property, Expression.Constant(boolValue)),
                            _ => throw new NotSupportedException($"Filter type '{filterType}' is not supported for boolean properties.")
                        };
                    }
                    break;

                default:
                    if (property.Type.IsEnum)
                    {
                        var enumValue = Enum.Parse(property.Type, propertyValue.ToString());
                        equality = filterType switch
                        {
                            FilterOperation.Equals => Expression.Equal(property, Expression.Constant(enumValue)),
                            FilterOperation.NotEquals => Expression.NotEqual(property, Expression.Constant(enumValue)),
                            _ => throw new NotSupportedException($"Filter type '{filterType}' is not supported for enum properties.")
                        };
                    }
                    else
                    {
                        throw new NotSupportedException($"Filtering is not supported for properties of type '{property.Type}'.");
                    }
                    break;
            }
        }

        return equality;
    }



    #region Private Methods

    /// <summary>
    /// Filter şərtlərinə əsasən fərqli ifadələr yaradır.
    /// </summary>
    private Expression GetExpression(FilterModel item, ParameterExpression parameter, Expression expression = null)
    {
        var property = Expression.Property(parameter, item.FilterData.Key);

        expression = item.FilterData.Operation switch
        {
            FilterOperation.Equals => Expression.Equal(property, Expression.Convert(Expression.Constant(item.FilterData.Values[0]), property.Type)),
            FilterOperation.NotEquals => Expression.NotEqual(property, Expression.Convert(Expression.Constant(item.FilterData.Values[0]), property.Type)),
            FilterOperation.GreaterThan => Expression.GreaterThan(property, Expression.Convert(Expression.Constant(item.FilterData.Values[0]), property.Type)),
            FilterOperation.LessThan => Expression.LessThan(property, Expression.Convert(Expression.Constant(item.FilterData.Values[0]), property.Type)),
            FilterOperation.In => GetInExpression(property, item.FilterData.Values),
            FilterOperation.NotIn => GetNotInExpression(property, item.FilterData.Values),
            FilterOperation.Between => GetBetweenExpression(property, item.FilterData.Values[0], item.FilterData.Values[1]),
            FilterOperation.Contains => GetContainsExpression(property, item.FilterData.Values[0].ToString()),
            FilterOperation.NotContains => GetNotContainsExpression(property, item.FilterData.Values[0].ToString()),
            _ => throw new NotSupportedException($"The filter operation '{item.FilterData.Operation}' is not supported.")
        };

        if (item.FilterGroup != null)
        {
            foreach (var group in item.FilterGroup)
            {
                var subExpression = GetExpression(group, parameter);
                expression = item.Condition == FilterPartRelations.Or
                    ? Expression.OrElse(expression, subExpression)
                    : Expression.AndAlso(expression, subExpression);
            }
        }

        return expression;
    }

    private Expression GetInExpression(MemberExpression property, object[] values)
    {
        if (values.Length == 0) throw new ArgumentException("Values array is empty.");

        Expression expression = Expression.Equal(property, Expression.Constant(values[0]));
        for (int i = 1; i < values.Length; i++)
        {
            var right = Expression.Equal(property, Expression.Constant(values[i]));
            expression = Expression.OrElse(expression, right);
        }
        return expression;
    }

    private Expression GetNotInExpression(MemberExpression property, object[] values)
    {
        if (values.Length == 0) throw new ArgumentException("Values array is empty.");

        Expression expression = Expression.NotEqual(property, Expression.Constant(values[0]));
        for (int i = 1; i < values.Length; i++)
        {
            var right = Expression.NotEqual(property, Expression.Constant(values[i]));
            expression = Expression.AndAlso(expression, right);
        }
        return expression;
    }

    private Expression GetBetweenExpression(MemberExpression property, object value1, object value2)
    {
        if (value1 == null || value2 == null) throw new ArgumentException("Between values cannot be null.");

        var greaterThanOrEqual = Expression.GreaterThanOrEqual(property, Expression.Constant(value1));
        var lessThanOrEqual = Expression.LessThanOrEqual(property, Expression.Constant(value2));

        return Expression.AndAlso(greaterThanOrEqual, lessThanOrEqual);
    }

    private Expression GetContainsExpression(MemberExpression property, string value)
    {
        var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        return Expression.Call(property, method, Expression.Constant(value));
    }

    private Expression GetNotContainsExpression(MemberExpression property, string value)
    {
        var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        var contains = Expression.Call(property, method, Expression.Constant(value));
        return Expression.Not(contains);
    }

    private IQueryable<T> OrderBy<T>(IQueryable<T> source, string orderByMember, SortDirection direction)
    {
        var parameter = Expression.Parameter(typeof(T));
        var propertyAccess = Expression.PropertyOrField(parameter, orderByMember);
        var keySelector = Expression.Lambda(propertyAccess, parameter);

        var methodName = direction == SortDirection.Ascending ? "OrderBy" : "OrderByDescending";
        var orderByCall = Expression.Call(
            typeof(Queryable),
            methodName,
            new Type[] { typeof(T), propertyAccess.Type },
            source.Expression,
            Expression.Quote(keySelector)
        );

        return source.Provider.CreateQuery<T>(orderByCall);
    }

    private Expression CreateDateComparisonExpression(Expression property, DateTime dateValue, FilterOperation filterType)
    {
        Expression equality = null;
        switch (filterType)
        {
            case FilterOperation.Equals:
                equality = Expression.AndAlso(
                    Expression.Equal(Expression.Property(property, "Year"), Expression.Constant(dateValue.Year)),
                    Expression.AndAlso(
                        Expression.Equal(Expression.Property(property, "Month"), Expression.Constant(dateValue.Month)),
                        Expression.Equal(Expression.Property(property, "Day"), Expression.Constant(dateValue.Day))
                    )
                );
                break;
            case FilterOperation.NotEquals:
                equality = Expression.OrElse(
                    Expression.NotEqual(Expression.Property(property, "Year"), Expression.Constant(dateValue.Year)),
                    Expression.OrElse(
                        Expression.NotEqual(Expression.Property(property, "Month"), Expression.Constant(dateValue.Month)),
                        Expression.NotEqual(Expression.Property(property, "Day"), Expression.Constant(dateValue.Day))
                    )
                );
                break;
            case FilterOperation.GreaterThan:
                equality = Expression.GreaterThan(property, Expression.Constant(dateValue));
                break;
            case FilterOperation.LessThan:
                equality = Expression.LessThan(property, Expression.Constant(dateValue));
                break;
            default:
                throw new NotSupportedException($"Filter type '{filterType}' is not supported for DateTime properties.");
        }

        return equality;
    }

    #endregion
}


/* Numune kod bloku
 
public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
}

public class Program
{
    public static void Main()
    {
        // Sample product data
        var products = new List<Product>
        {
            new Product { Id = Guid.NewGuid(), Name = "Laptop", Price = 1200, IsAvailable = true },
            new Product { Id = Guid.NewGuid(), Name = "Mouse", Price = 25, IsAvailable = true },
            new Product { Id = Guid.NewGuid(), Name = "Keyboard", Price = 50, IsAvailable = false },
            new Product { Id = Guid.NewGuid(), Name = "Monitor", Price = 300, IsAvailable = true },
            new Product { Id = Guid.NewGuid(), Name = "Phone", Price = 800, IsAvailable = false }
        }.AsQueryable();

        // Instantiate the ExpressionBuilder
        var expressionBuilder = new ExpressionBuilder();

        // Define filter criteria
        var viewFilter = new ViewFilter
        {
            Filters = new List<FilterModel>
            {
                new FilterModel
                {
                    FilterData = new FilterData
                    {
                        Key = nameof(Product.Price),
                        Operation = FilterOperation.GreaterThan,
                        Values = new object[] { 100 }
                    },
                    Condition = FilterPartRelations.And
                },
                new FilterModel
                {
                    FilterData = new FilterData
                    {
                        Key = nameof(Product.IsAvailable),
                        Operation = FilterOperation.Equals,
                        Values = new object[] { true }
                    },
                    Condition = FilterPartRelations.And
                }
            }
        };

        // Apply filtering
        var filterExpression = expressionBuilder.GetExpressionFunc<Product>(viewFilter);
        var filteredProducts = products.Where(filterExpression).ToList();

        // Display filtered products
        Console.WriteLine("Filtered Products:");
        foreach (var product in filteredProducts)
        {
            Console.WriteLine($"Product: {product.Name}, Price: {product.Price}, Available: {product.IsAvailable}");
        }
    }
}
 
 */