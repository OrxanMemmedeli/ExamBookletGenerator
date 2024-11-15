using EBC.Core.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace EBC.Data.Entities;

public class QuestionLevel : BaseEntity<Guid>
{
    public QuestionLevel()
    {
        Questions = new HashSet<Question>();
    }
    public string Name { get; set; }
    [Range(1, 5)]
    public short Level { get; set; }

    public ICollection<Question> Questions { get; set; }

}
