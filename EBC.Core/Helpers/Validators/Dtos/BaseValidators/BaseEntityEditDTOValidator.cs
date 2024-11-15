using EBC.Core.Constants;
using EBC.Core.Models.Commons;
using FluentValidation;

namespace EBC.Core.Helpers.Validators.Dtos.BaseValidators;

/// <summary>
/// Bütün EDIT DTO-lar üçün ümumi doğrulama qaydalarını təmin edən baza validator sinfi.
/// </summary>
/// <typeparam name="TDto">Validasiya ediləcək DTO sinifi</typeparam>
public class BaseEntityEditDTOValidator<TDto> : BaseValidator<TDto> where TDto : BaseEntityEditDTO
{
    /// <summary>
    /// Constructor - Ümumi doğrulama qaydalarını tətbiq edir.
    /// </summary>
    public BaseEntityEditDTOValidator()
    {
        // Xüsusi qaydalar buraya əlavə oluna bilər
        RuleFor(x => x.CreatedDate).Must(date => date >= DateTime.Now.AddMinutes(-2) && date != null).WithMessage(ValidationMessage.DateTimeMinValue);

    }
}
