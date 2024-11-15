using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.QuestionParameter;

public class QuestionParameterViewDTO : BaseEntityViewDTO
{
    public int StartQuestionNumber { get; set; }
    public int EndQuestionNumber { get; set; }
}
