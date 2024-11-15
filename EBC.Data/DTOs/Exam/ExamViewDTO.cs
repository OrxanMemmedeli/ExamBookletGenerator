using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.Exam;

public class ExamViewDTO : BaseEntityViewDTO
{
    public string Name { get; set; }
    public string GradeName { get; set; }
    public string ExamParameterName { get; set; }
}
