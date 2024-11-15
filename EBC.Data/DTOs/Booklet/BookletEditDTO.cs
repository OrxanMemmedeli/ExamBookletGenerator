using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.Booklet;

public class BookletEditDTO : BaseEntityEditDTO
{
    public Guid GradeId { get; set; }
    public Guid? GroupId { get; set; }
    public Guid VariantId { get; set; }
    public Guid ExamId { get; set; }
    public Guid? CompanyId { get; set; }
    public Guid AcademicYearId { get; set; }
}
