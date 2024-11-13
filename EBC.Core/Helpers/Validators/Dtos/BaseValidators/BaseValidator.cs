using EBC.Core.Attributes;
using EBC.Core.Constants;
using EBC.Core.CustomFilter.WorkFilter.Filters;
using EBC.Core.Helpers.Extensions;
using FluentValidation;

namespace EBC.Core.Helpers.Validators.Dtos.BaseValidators;

/// <summary>
/// Bütün EDIT DTO-lar üçün ümumi doğrulama qaydalarını təmin edən baza validator sinfi.
/// </summary>
/// <typeparam name="TDto">Validasiya ediləcək DTO sinifi</typeparam>
public abstract class BaseValidator<TDto> : AbstractValidator<TDto>
{
    /// <summary>
    /// Constructor - Ümumi doğrulama qaydalarını tətbiq edir.
    /// </summary>
    protected BaseValidator()
    {
        // Ümumi qaydalar tətbiq olunur (nullable olmayan bütün tiplər üçün)
        ApplyCommonRules();
    }

    /// <summary>
    /// Nullable olmayan, atribut ilə işarələnməmiş və `FilterOperation` tipində olmayan bütün tiplər üçün ümumi doğrulama qaydalarını tətbiq edir.
    /// </summary>
    private void ApplyCommonRules()
    {
        foreach (var property in typeof(TDto).GetProperties()
                                             .Where(p => !p.IsNullable()
                                                         && !p.IsDefined(typeof(IgnoreValidationAttribute), true)
                                                         && p.PropertyType != typeof(FilterOperation)))
        {
            // String tiplər üçün boş olmama qaydası
            if (property.PropertyType == typeof(string))
            {
                RuleFor(dto => (string)property.GetValue(dto))
                    .NotNull().WithMessage(ValidationMessage.NotEmptyAndNotNull)
                    .NotEmpty().WithMessage(ValidationMessage.NotEmptyAndNotNull);
            }
            // Digər nullable olmayan tiplər üçün default dəyərin yoxlanması
            else if (property.PropertyType.IsValueType)
            {
                var defaultValue = Activator.CreateInstance(property.PropertyType);
                RuleFor(dto => property.GetValue(dto))
                    .Must(value => value != null && !Equals(value, defaultValue))
                    .WithMessage(ValidationMessage.NotEqualToDefault);
            }
        }
    }
}
