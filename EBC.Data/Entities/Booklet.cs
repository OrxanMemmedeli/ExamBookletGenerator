using EBC.Core.Entities.Common;

namespace EBC.Data.Entities;

public class Booklet : BaseEntity<Guid>
{
    public Guid GradeId { get; set; }
    public Guid? GroupId { get; set; }
    public Guid VariantId { get; set; }
    public Guid ExamId { get; set; }
    public Guid AcademicYearId { get; set; }


    public virtual Grade Grade { get; set; }
    public virtual Group Group { get; set; }
    public virtual Variant Variant { get; set; }
    public virtual Exam Exam { get; set; }
    public virtual AcademicYear AcademicYear { get; set; }

}
