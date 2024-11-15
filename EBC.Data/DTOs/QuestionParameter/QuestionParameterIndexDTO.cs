using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.QuestionParameter;

public class QuestionParameterIndexDTO : BaseEntityViewDTO
{
    public int StartQuestionNumber { get; set; }
    public int EndQuestionNumber { get; set; }
}
