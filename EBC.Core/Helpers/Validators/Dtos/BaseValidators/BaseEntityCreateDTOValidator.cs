using EBC.Core.Models.Commons;

namespace EBC.Core.Helpers.Validators.Dtos.BaseValidators;

/// <summary>
/// Bütün DTO-lar üçün ümumi doğrulama qaydalarını təmin edən baza validator sinfi.
/// </summary>
/// <typeparam name="TDto">Validasiya ediləcək DTO sinifi</typeparam>
public class BaseEntityCreateDTOValidator<TDto> : BaseValidator<TDto> where TDto : BaseEntityCreateDTO

{
    /// <summary>
    /// Constructor - Ümumi doğrulama qaydalarını tətbiq edir.
    /// </summary>
    public BaseEntityCreateDTOValidator() : base()
    {
        // Xüsusi qaydalar buraya əlavə oluna bilər
    }

}
