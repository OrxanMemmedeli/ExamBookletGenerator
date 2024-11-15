using EBC.Core.Entities.Common;

namespace EBC.Data.Entities;

public class Subject : BaseEntity<Guid>
{
    public Subject()
    {
        Sections = new HashSet<Section>();
        Responses = new HashSet<Response>();
        Questions = new HashSet<Question>();
        SubjectParameters = new HashSet<SubjectParameter>();
    }
    public string Name { get; set; }
    public decimal? AmountForQuestion { get; set; }

    public ICollection<Section> Sections { get; set; }
    public ICollection<Response> Responses { get; set; }
    public ICollection<Question> Questions { get; set; }
    public ICollection<SubjectParameter> SubjectParameters { get; set; }

}
