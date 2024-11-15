using EBC.Core.Constants;
using EBC.Core.Helpers.Validators.Dtos.BaseValidators;
using EBC.Data.DTOs.Group;
using FluentValidation;

namespace EBC.Data.Validators.DTOs.Group;

public class GroupEditDTOValidator : BaseEntityEditDTOValidator<GroupEditDTO>
{
    public GroupEditDTOValidator() : base()
    {
        RuleFor(x => x.Name)
            .MaximumLength(50).WithMessage(string.Format(ValidationMessage.MaximumLength, 50));
    }
}
