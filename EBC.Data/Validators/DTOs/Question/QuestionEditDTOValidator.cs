using Business.Validations.DTOs.BaseFields;
using CoreLayer.Constants;
using DTOLayer.DTOs.Question;
using EBC.Core.Constants;
using EBC.Core.Helpers.Validators.Dtos.BaseValidators;
using EBC.Data.DTOs.Question;
using EntityLayer.Constants;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBC.Data.Validators.DTOs.Question
{
    public class QuestionEditDTOValidator : BaseEntityEditDTOValidator<QuestionEditDTO>
    {
        public QuestionEditDTOValidator()
        {
            RuleFor(x => x.Content)
                .MaximumLength(15000).WithMessage(string.Format(ValidationMessage.MaximumLength, 15000));
        }
    }
}
