using EBC.Core.Constants;
using EBC.Core.Helpers.Validators.Dtos.BaseValidators;
using EBC.Data.DTOs.Response;
using FluentValidation;

namespace EBC.Data.Validators.DTOs.Response;

public class ResponseCreateDTOValidator : BaseEntityCreateDTOValidator<ResponseCreateDTO>
{
    public ResponseCreateDTOValidator() : base()
    {
        RuleFor(x => x.Title)
            .MaximumLength(20).WithMessage(string.Format(ValidationMessage.MaximumLength, 20));

        RuleFor(x => x.Content)
            .MaximumLength(800).WithMessage(string.Format(ValidationMessage.MaximumLength, 800));
    }
}
