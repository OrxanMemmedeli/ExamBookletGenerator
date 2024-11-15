using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.QuestionType;

public class QuestionTypeIndexDTO : BaseEntityViewDTO
{
    public string ResponseType { get; set; }
    public double? ResponseCount { get; set; }
    public bool IsShowAnswer { get; set; } = false;
    public string Description { get; set; }
}
