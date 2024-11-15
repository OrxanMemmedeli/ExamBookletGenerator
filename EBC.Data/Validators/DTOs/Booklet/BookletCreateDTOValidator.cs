using EBC.Core.Helpers.Validators.Dtos.BaseValidators;
using EBC.Data.DTOs.Booklet;
using FluentValidation;

namespace EBC.Data.Validators.DTOs.Booklet;

public class BookletCreateDTOValidator : BaseEntityCreateDTOValidator<BookletCreateDTO>
{
    public BookletCreateDTOValidator() : base()
    {
    }
}
