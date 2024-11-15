using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.Booklet;

public class BookletViewDTO : BaseEntityViewDTO
{
    public string GradeName { get; set; }
    public string GroupName { get; set; }
    public string VariantName { get; set; }
    public string ExamName { get; set; }
    public string CompanyName { get; set; }
    public string AcademicYearName { get; set; }
}
