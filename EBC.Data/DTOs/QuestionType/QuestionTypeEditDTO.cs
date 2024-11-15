using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.QuestionType;

public class QuestionTypeEditDTO : BaseEntityEditDTO
{
    public string ResponseType { get; set; }
    public double? ResponseCount { get; set; } = 0;
    public bool IsShowAnswer { get; set; } = false;
    public string Description { get; set; }
}