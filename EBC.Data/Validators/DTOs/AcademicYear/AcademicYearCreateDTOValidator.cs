using EBC.Core.Constants;
using EBC.Core.Helpers.Validators.Dtos.BaseValidators;
using EBC.Data.DTOs.AcademicYear;
using FluentValidation;

namespace EBC.Data.Validators.DTOs.AcademicYear;

public class AcademicYearCreateDTOValidator : BaseEntityCreateDTOValidator<AcademicYearCreateDTO>
{
    public AcademicYearCreateDTOValidator() : base()
    {
        RuleFor(x => x.Name)
            .MaximumLength(50).WithMessage(string.Format(ValidationMessage.MaximumLength, 50));
    }
}
