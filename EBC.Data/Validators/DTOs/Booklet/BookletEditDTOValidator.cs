using EBC.Core.Helpers.Validators.Dtos.BaseValidators;
using EBC.Data.DTOs.Booklet;
using FluentValidation;

namespace EBC.Data.Validators.DTOs.Booklet;

public class BookletEditDTOValidator : BaseEntityEditDTOValidator<BookletEditDTO>
{
    public BookletEditDTOValidator() : base()
    {
    }
}
