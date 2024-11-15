using EBC.Core.Constants;
using EBC.Core.Helpers.Validators.Dtos.BaseValidators;
using EBC.Data.DTOs.Text;
using FluentValidation;

namespace EBC.Data.Validators.DTOs.Text;

public class TextEditDTOValidator : BaseEntityEditDTOValidator<TextEditDTO>
{
    public TextEditDTOValidator() : base()
    {
        RuleFor(x => x.Name)
            .MaximumLength(150).WithMessage(string.Format(ValidationMessage.MaximumLength, 150));

        RuleFor(x => x.Title)
            .Must(title => title.Contains("{0} – {1}")).WithMessage(string.Format(ValidationMessage.ContainsTextForTextTitle))
            .MaximumLength(500).WithMessage(string.Format(ValidationMessage.MaximumLength, 500));
    }
}
